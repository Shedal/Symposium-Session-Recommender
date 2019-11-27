using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Recommender.TwitterApi.Helpers;

namespace Recommender.SocialMedia.Models
{
    public class InteractionsModel
    {
        public List<Interaction> Interactions { get; set; }

        public InteractionsModel(string handle)
        {
            //this.Interactions = TwitterHelper.RunUserTweetsQueryAsync();
        }

        public class Interaction
        {   
            public string UserId { get; set; }
            public string Post { get; set; }
            public DateTime TimeStamp { get; set; }
        }
    }
}