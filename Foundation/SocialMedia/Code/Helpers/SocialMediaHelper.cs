using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Recommender.SocialMedia.Models;

namespace Recommender.SocialMedia.Helpers
{
    public class SocialMediaHelper
    {
        public InteractionsModel PopulateTwitterInteractionModel(string handle)
        {
            var interactionsModel = new InteractionsModel(handle);
            return interactionsModel;
        }
    }
}