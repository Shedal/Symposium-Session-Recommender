using System;
using Sitecore.XConnect;

namespace Recommender.Foundation.CollectionModel.Facets
{
    [Serializable]
    [FacetKey(FacetKeys.SocialPostInfo)]
    public class SocialPostInfo : Facet
    {
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}