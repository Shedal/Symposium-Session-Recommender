using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using Castle.Components.DictionaryAdapter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Recommender.AgendaContent.Helpers;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Glass.Mapper;
using Glass.Mapper.Sc;
using Recommender.Foundation.CollectionModel.Facets;
using Sitecore.Data;
using Sitecore.XConnect;
using Context = Sitecore.Context;
using Log = Sitecore.Diagnostics.Log;
using Sitecore.XConnect.Collection.Model;

namespace Recommender.AgendaContent.Models
{
    public class AgendaModel
    {
        private List<SymposiumSession> symposiumSessions { get; set; }

        public List<RoomResourceModel> RoomResources { get; set; }
        public List<SessionModel> AttendeeSessions { get; set; }
        public List<SessionModel> AllSessions { get; set; }
        public KeyValuePair<string,double> PrimaryTrack { get; set; }
        public KeyValuePair<string, double> SecondaryTrack { get; set; }
        public KeyValuePair<string, double> PrimaryTopic { get; set; }
        public KeyValuePair<string, double> SecondaryTopic { get; set; }
        public KeyValuePair<string, double> TertiaryTopic { get; set; }
        public Dictionary<string,double> TopicScores { get; set; }
        public Dictionary<string, double> TrackScores { get; set; }
        public AttendeeProfile AttendeeProfile { get; set; }
        public Contact Contact { get; set; }

        public string AttendeeSessionsJson
        {
            get
            {
                return JsonConvert.SerializeObject(this.AttendeeSessions);
            }
        }

        public string AllSessionsJson
        {
            get
            {
                return JsonConvert.SerializeObject(this.AllSessions);
            }
        }

        public string RoomResourcesJson
        {
            get
            {
                return JsonConvert.SerializeObject(this.RoomResources);
            }
        }
        public PersonalInformation AttendeePersonalInformation { get; set; }

        public AgendaModel() { }

        public AgendaModel(ISitecoreContext sitecoreContext, AttendeeProfile attendeeProfile, PersonalInformation attendeePersonalInformation)
        {
            var sessionsFolder = Context.Database.GetItem("{88662E3F-6032-4F8F-BE04-4AA0E44B034A}");
            this.AttendeeProfile = attendeeProfile;
            this.AttendeePersonalInformation = attendeePersonalInformation;

            Log.Info("PopulateSymposiumSessions", "PopulateSymposiumSessions");
            PopulateSymposiumSessions(sessionsFolder, sitecoreContext);
            PopulateSymposiumRooms();

            if (this.AllSessions == null)
            {
                this.AllSessions = new List<SessionModel>();
                this.AttendeeSessions = new List<SessionModel>();
            }

            foreach (var symposiumSession in symposiumSessions)
            {
                this.AllSessions.Add(new SessionModel(symposiumSession));
            }

            if (attendeeProfile != null)
            {
            
                PopulateTracksAndTopics(attendeeProfile);

                //add all sessions from primary track
                //replace commas for Marketing track since the ML dataset does not include them
                
                var attendeeSessions = symposiumSessions.Where(x => x.TrackLabel == PrimaryTrack.Key.Replace(",", string.Empty) || x.TrackLabel==string.Empty || x.TrackLabel==null).ToList();
                
                //special case for commerce track
                if (PrimaryTrack.Key == "E-commerce experience")
                {
                    attendeeSessions = symposiumSessions.Where(x => x.Track == "E-commerce experience" || x.TrackLabel == string.Empty || x.TrackLabel == null).ToList();
                }

                //remove conflicting session based on scored topics
                attendeeSessions = RemoveConflictingSessions(attendeeSessions);

                //fill gaps in agenda with secondary track and scored topics
                attendeeSessions = FillGapsInAgenda(attendeeSessions);

                foreach (var symposiumSession in attendeeSessions)
                {
                    this.AttendeeSessions.Add(new SessionModel(symposiumSession));
                    Log.Info(symposiumSession.Title + " Added to attendee sessions list", "Added to attendee sessions list");
                }
            }
        }

        private SymposiumSession GetSpeakerAttendeeSession(List<SymposiumSession> sessions)
        {
            if (AttendeePersonalInformation != null)
            {
                var firstName = this.AttendeePersonalInformation.FirstName;
                var lastName = this.AttendeePersonalInformation.LastName;
                if (!firstName.IsNullOrEmpty() || !lastName.IsNullOrEmpty())
                {
                    var speakerName = (firstName.IsNullOrEmpty() ? string.Empty : firstName) + (lastName.IsNullOrEmpty() ? string.Empty : " " + lastName);
                    Log.Info("Attendee name" + speakerName, AttendeePersonalInformation);
                    var speakerNameArray = speakerName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    return sessions.Where(x => speakerNameArray.All(y => x.Speakers.Contains(y))).FirstOrDefault();
                }
            }
            
            return null;
        }

        private void PopulateSymposiumRooms()
        {
            if (this.RoomResources == null)
            {
                this.RoomResources = new List<RoomResourceModel>();
            }
            foreach (var room in symposiumSessions.Select(x=>x.Room).Distinct())
            {
                if (!room.IsNullOrEmpty())
                {
                    var roomArray = room.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        
                    this.RoomResources.Add(new RoomResourceModel
                    {
                        Title = roomArray.Length > 1? roomArray[1]:string.Empty,
                        Building = roomArray[0],
                        Id = room.Replace(" ",string.Empty).Replace(",",string.Empty)
                    });
                }
            }
            this.RoomResources = this.RoomResources.OrderBy(x => x.Building).ThenBy(x => x.Title).ToList();
        }

        private List<SymposiumSession> FillGapsInAgenda(List<SymposiumSession> attendeeSessions)
        {
            Log.Info(" FillGapsInAgenda", " removing conflicting session");

            attendeeSessions = attendeeSessions.OrderBy(x => x.StartTime).ToList();
            for (int i = 0; i <= attendeeSessions.Count - 2; i++)
            {
                var timeSpace = attendeeSessions[i].StartTime - attendeeSessions[i + 1].StartTime;
                if (timeSpace.Minutes > 30)
                {
                    Log.Info(attendeeSessions[i].Title +" "+ attendeeSessions[i + 1].Title + " Gap found in agenda", "Gap in agenda");

                    //fill gaps from secondary track
                    var fillerSessions = symposiumSessions.Where(
                        x =>
                        x.TrackLabel == SecondaryTrack.Key &&
                            x.StartTime > attendeeSessions[i].StartTime &&
                            x.StartTime < attendeeSessions[i + 1].StartTime).ToList();
                    if (fillerSessions.Count == 1)
                    {
                        attendeeSessions.Add(fillerSessions.First());
                        Log.Info(fillerSessions.First().Title + " Single filler session found", "Gap in agenda");
                    }
                    else if (fillerSessions.Count > 1)
                    {
                        Log.Info("More than one filler session found", "Gap in agenda");
                        var fillerSession = GetHighestScoringSessionByTopicScore(fillerSessions);
                        attendeeSessions.Add(fillerSession);
                        Log.Info(fillerSessions.First().Title + " filler session found", "Gap in agenda");
                    }
                    else
                    {
                        Log.Info("No filler session found in secondary track. Filling with all sessions from any track.", "Gap in agenda");

                        fillerSessions = symposiumSessions.Where(
                        x =>
                           x.StartTime > attendeeSessions[i].StartTime &&
                           x.StartTime < attendeeSessions[i + 1].StartTime).ToList();
                        if (fillerSessions.Count == 1)
                        {
                            attendeeSessions.Add(fillerSessions.First());
                            Log.Info(fillerSessions.First().Title + " Single filler session found", "Gap in agenda");
                        }
                        else if (fillerSessions.Count > 1)
                        {
                            var fillerSession = GetHighestScoringSessionByTopicScore(fillerSessions);
                            attendeeSessions.Add(fillerSession);
                            Log.Info(fillerSessions.First().Title + " filler session found", "Gap in agenda");
                        }
                    }
                }
            }
            return attendeeSessions;
        }

        private List<SymposiumSession> RemoveConflictingSessions(List<SymposiumSession> attendeeSessions)
        {
            Log.Info(" RemoveConflictingSessions", " removing conflicting session");

            var localAttendeeSessions = attendeeSessions.ToList();
            if (localAttendeeSessions.Count > 0)
            {
                foreach (var session in attendeeSessions.ToList())
                {
                    var conflictingSessions = attendeeSessions.Where(x => x.StartTime == session.StartTime).ToList();
                    if (conflictingSessions.Count > 0)
                    {
                        var keepConflictingSesion = GetHighestScoringSessionByTopicScore(conflictingSessions);

                        foreach (var conflictingSession in conflictingSessions)
                        {
                            Log.Info(conflictingSession.Title + " conflicting session", "conflicting session");

                            if (keepConflictingSesion != null && conflictingSession.Title != keepConflictingSesion.Title)
                            {
                                localAttendeeSessions.Remove(conflictingSession);
                                Log.Info(conflictingSession.Title + " Removing conflicting session", " removing conflicting session");
                            }
                        }
                    }
                    else
                    {
                        Log.Info("No conflicting sessions were found", "conflicting session");
                    }
                }
            }
            return localAttendeeSessions;
        }

        private SymposiumSession GetHighestScoringSessionByTopicScore(List<SymposiumSession> sessions)
        {
            var sortedTopicScores = TopicScores.OrderByDescending(x => x.Value);

            SymposiumSession highestScoringSession = null;

            highestScoringSession = GetSpeakerAttendeeSession(sessions);
            if (highestScoringSession != null)
            {
                return highestScoringSession;
            }

            foreach (var sortedTopicScore in sortedTopicScores)
            {
                Log.Info(sortedTopicScore.Key +" "+ sortedTopicScore.Value + " topic scores for GetHighestScoringSessionByTopicScore", "Topic scores");

                foreach (var conflictingSession in sessions)
                {
                    if (conflictingSession.TopicLabel == sortedTopicScore.Key)
                    {
                        highestScoringSession = conflictingSession;
                        Log.Info(highestScoringSession.Title + " highest scoring session found", "conflicting session");
                        break;
                    }
                }

                if (highestScoringSession != null)
                    break;
            }

            if(highestScoringSession==null)
                Log.Info(" highest scoring session NOT found", "conflicting session");
            return highestScoringSession;
        }

        private void PopulateTracksAndTopics(AttendeeProfile attendeeProfile)
        {
            Log.Info(" PopulateTracksAndTopics", "Attendee agenda");

            if(attendeeProfile!=null && attendeeProfile.TrackScores !=null && attendeeProfile.TopicScores!=null 
                && attendeeProfile.TrackScores.Count>0 && attendeeProfile.TopicScores.Count>0)
            {

                foreach (var attendeeProfileTrackScore in attendeeProfile.TrackScores)
                {
                    Log.Info("Attendee track scores:" + attendeeProfileTrackScore.Key+" "+attendeeProfileTrackScore.Value,"Attendee scores");
                }

                //populate track scores
                this.TrackScores = attendeeProfile.TrackScores;
                var sortedKvp = attendeeProfile.TrackScores.OrderByDescending(x => x.Value);
                PrimaryTrack = sortedKvp.FirstOrDefault();

                if (sortedKvp.Count() > 1)
                {
                    SecondaryTrack = sortedKvp.Skip(1).Take(1).FirstOrDefault();
                }




                foreach (var attendeeProfileTopicScore in attendeeProfile.TopicScores)
                {
                    Log.Info("Attendee topic scores:" + attendeeProfileTopicScore.Key + " " + attendeeProfileTopicScore.Value, "Attendee scores");
                }
                ////populate topic scores
                this.TopicScores = attendeeProfile.TopicScores;
                sortedKvp = attendeeProfile.TopicScores.OrderByDescending(x => x.Value);
                PrimaryTopic = sortedKvp.FirstOrDefault();

                if (sortedKvp.Count() > 1)
                {
                    SecondaryTopic = sortedKvp.Skip(1).Take(1).FirstOrDefault();
                }

                if (sortedKvp.Count() > 2)
                {
                    TertiaryTopic = sortedKvp.Skip(2).Take(1).FirstOrDefault();
                }

                //special case of commerce topic and commerce track
                if (PrimaryTopic.Value > PrimaryTrack.Value && PrimaryTopic.Key == "Commerce")
                {
                    PrimaryTrack = new KeyValuePair<string, double>("E-commerce experience", PrimaryTopic.Value);
                }
            }
        }

        private void PopulateSymposiumSessions(Item sessionItem, ISitecoreContext sitecoreContext)
        {
            if (symposiumSessions == null)
            {
                symposiumSessions = new List<SymposiumSession>();
            }

            if (sessionItem != null)
            {
                if (!sessionItem.HasChildren && sessionItem.IsDerived(SymposiumSession.TemplateId))
                    symposiumSessions.Add(sitecoreContext.Cast<SymposiumSession>(sessionItem));
                else if (sessionItem.HasChildren)
                {
                    foreach (Item sessionItemChild in sessionItem.Children.Where(x => x.IsDerived(SymposiumSession.TemplateId)))
                    {
                        PopulateSymposiumSessions(sessionItemChild, sitecoreContext);
                    }
                }
            }
        }
    }
}