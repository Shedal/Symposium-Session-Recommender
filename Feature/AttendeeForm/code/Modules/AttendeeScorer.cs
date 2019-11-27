using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recommender.Foundation.CognitiveServices;
using Recommender.Foundation.CollectionModel.Facets;
using Sitecore.Diagnostics;
using Sitecore.XConnect;

namespace Recommender.AttendeeForm.Modules
{
    public class AttendeeScorer
    {
        private readonly CognitiveServicesClient _services;

        public AttendeeScorer(CognitiveServicesClient services)
        {
            _services = services;
        }

        public async Task ScoreAttendee(IXdbContext client, Contact contact)
        {
            try
            {
                const int batchSize = 10;

                string text = string.Empty;
                int i = 0;

                List<SocialPostInfo> posts =
                    contact.Interactions
                        .Select(interaction => interaction.GetFacet<SocialPostInfo>())
                        .Where(p => p != null)
                        .ToList();

                int maxi = posts.Count - 1;
                
                AttendeeProfile profile = contact.GetFacet<AttendeeProfile>();

                Dictionary<string, double> scores;

                foreach (SocialPostInfo post in posts)
                {
                    text += " " + post.Text;

                    if (++i % batchSize == 0 || i == maxi)
                    {
                        profile.PostsProcessed += i % batchSize == 0 ? batchSize : i % batchSize;

                        scores = await _services.InvokeTracksService(text);

                        foreach (string key in scores.Keys)
                        {
                            if (!profile.TrackScores.ContainsKey(key))
                            {
                                profile.TrackScores[key] = scores[key];
                            }
                            else
                            {
                                profile.TrackScores[key] += scores[key];
                            }
                        }

                        scores = await _services.InvokeTopicsService(text);

                        foreach (string key in scores.Keys)
                        {
                            if (!profile.TopicScores.ContainsKey(key))
                            {
                                profile.TopicScores[key] = scores[key];
                            }
                            else
                            {
                                profile.TopicScores[key] += scores[key];
                            }
                        }

                        text = String.Empty;
                    }
                }
            
                client.SetFacet(contact, profile);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message,ex, typeof(AttendeeScorer));
                throw;
            }
        }
    }
}