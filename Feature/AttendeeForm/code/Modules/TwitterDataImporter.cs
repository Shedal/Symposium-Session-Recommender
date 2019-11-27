using System;
using System.Collections.Generic;
using LinqToTwitter;
using Recommender.Foundation.CollectionModel.Events;
using Recommender.Foundation.CollectionModel.Facets;
using Recommender.TwitterApi.Helpers;
using Sitecore.Analytics.Model.Entities;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;

namespace Recommender.AttendeeForm.Modules
{
    public class TwitterDataImporter : IAttendeeDataImporter
    {
        public void ImportData(IXdbContext client, Contact contact)
        {
            string twitterHandle = contact.GetFacet<AttendeeProfile>().TwitterHandle;
            var socialImportInfo = contact.GetFacet<SocialImportInfo>();

            if (string.IsNullOrWhiteSpace(twitterHandle))
            {
                return;
            }

            TwitterContext context = TwitterHelper.CreateContext();
            List<Status> results = TwitterHelper.RunUserTweetsQueryAsync(context, twitterHandle);

            Guid channelId = new Guid("{6D3D2374-AF56-44FE-B99A-20843B440B58}"); // Twitter
            string userAgent = "RecommenderImporter";

            //
            // Import the contact's latest tweets

            foreach (Status tweet in results)
            {
                string tweetId = tweet.StatusID.ToString();

                if (socialImportInfo.ImportedPostIds.ContainsKey(tweetId))
                {
                    continue;
                }

                socialImportInfo.ImportedPostIds[tweetId] = "twitter";

                var interaction = new Interaction(contact, InteractionInitiator.Contact, channelId, userAgent);

                var postInfo = new SocialPostInfo
                {
                    CreatedAt = tweet.CreatedAt,
                    Text = tweet.FullText
                };

                client.SetFacet<SocialPostInfo>(interaction, FacetKeys.SocialPostInfo, postInfo);

                SocialPostEvent e = new SocialPostEvent(tweet.FullText, tweet.CreatedAt);

                interaction.Events.Add(e);

                client.AddInteraction(interaction);
            }

            client.SetFacet(contact, socialImportInfo);

            //
            // Update the contact's name

            var personal = contact.GetFacet<PersonalInformation>(PersonalInformation.DefaultFacetKey) ?? new PersonalInformation();

            User user = TwitterHelper.GetTwitterUserDetails(context, twitterHandle);
            if (user != null)
            {
                personal.FirstName = !string.IsNullOrWhiteSpace(user.Name) ? user.Name : user.ScreenName;
            }

            client.SetFacet<PersonalInformation>(contact, PersonalInformation.DefaultFacetKey, personal);
        }
    }
}