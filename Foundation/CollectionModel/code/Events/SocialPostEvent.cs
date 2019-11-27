using System;
using Sitecore.XConnect;

namespace Recommender.Foundation.CollectionModel.Events
{
    public class SocialPostEvent : Event
    {
        public static readonly Guid EventDefinitionId = new Guid("{FB06A747-B235-4C05-BBBF-93A37FAB50EB}");
        
        public SocialPostEvent(string text, DateTime timestamp)
            : base(EventDefinitionId, timestamp)
        {
            this.Text = text;
        }
    }
}