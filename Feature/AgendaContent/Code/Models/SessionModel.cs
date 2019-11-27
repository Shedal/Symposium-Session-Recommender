using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.StringExtensions;

namespace Recommender.AgendaContent.Models
{
    public class SessionModel
    {
        public SessionModel(SymposiumSession symposiumSession)
        {
            if (symposiumSession != null && symposiumSession.Item != null)
            {
                this.Description = HttpUtility.HtmlDecode((symposiumSession.Room.IsNullOrEmpty() ? string.Empty : "<b>" + symposiumSession.Room + "</b><br/>") + symposiumSession.Description + (symposiumSession.Speakers.IsNullOrEmpty() ? string.Empty : "<br/><b>" + symposiumSession.Speakers+"</b>"));
                this.EndTime = symposiumSession.EndTime;
                this.Room = symposiumSession.Room;
                this.SessionDate = symposiumSession.SessionDate;
                this.SessionType = symposiumSession.Type;
                this.Speakers = symposiumSession.Speakers;
                this.StartTime = symposiumSession.StartTime;
                this.Title = HttpUtility.HtmlDecode(symposiumSession.Title) ;
                this.Track = symposiumSession.Track;
                this.RoomResourceId = this.Room.Replace(" ", string.Empty).Replace(",", string.Empty);
                this.Id = Guid.NewGuid().ToString();
            }
        }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("resourceId")]
        public string RoomResourceId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("date")]
        public string SessionDate { get; set; }
        [JsonProperty("start")]
        public DateTime StartTime { get; set; }
        [JsonProperty("end")]
        public DateTime EndTime { get; set; }
        [JsonProperty("room")]
        public string Room { get; set; }
        [JsonProperty("type")]
        public string SessionType { get; set; }
        [JsonProperty("speakers")]
        public string Speakers { get; set; }
        [JsonProperty("track")]
        public string Track { get; set; }

        public class Speaker
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("jobTitle")]
            public string JobTitle { get; set; }
            [JsonProperty("company")]
            public string Company { get; set; }
        }
    }
}