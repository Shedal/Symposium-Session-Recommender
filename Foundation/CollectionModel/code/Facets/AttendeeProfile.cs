using System;
using System.Collections.Generic;
using Sitecore.XConnect;

namespace Recommender.Foundation.CollectionModel.Facets
{
    [Serializable]
    [FacetKey(FacetKeys.AttendeeProfile)]
    public class AttendeeProfile : Facet
    {
        public string TwitterHandle { get; set; }
        public string LinkedInProfileUrl { get; set; }
        public string FreeText { get; set; }

        public Dictionary<string, double> TrackScores { get; } = new Dictionary<string, double>();
        public Dictionary<string, double> TopicScores { get; } = new Dictionary<string, double>();

        public int PostsProcessed { get; set; }
    }
}