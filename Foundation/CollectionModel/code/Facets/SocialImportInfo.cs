using System;
using System.Collections.Generic;
using Sitecore.XConnect;

namespace Recommender.Foundation.CollectionModel.Facets
{
    [Serializable]
    [FacetKey(FacetKeys.SocialImportInfo)]
    public class SocialImportInfo : Facet
    {
        public Dictionary<string, string> ImportedPostIds { get; } = new Dictionary<string, string>();
    }
}