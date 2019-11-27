using LinqToTwitter;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Recommender.TwitterApi.Helpers
{
    public static class TwitterHelper
    {
        public static TwitterContext CreateContext()
        {
            IAuthorizer auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = "",
                    ConsumerSecret = "",
                    AccessToken = "",
                    AccessTokenSecret = ""
                }
            };

            auth.AuthorizeAsync().Wait();

            return new TwitterContext(auth);
        }

        public static User GetTwitterUserDetails(TwitterContext twitterCtx, string twitterHandle)
        {
            User user = twitterCtx.User.FirstOrDefault(u => u.Type == UserType.Show && u.ScreenName == twitterHandle);
            return user;
        }

        public static List<Status> RunUserTweetsQueryAsync(TwitterContext twitterCtx, string twitterHandle)
        {
            const int MaxTweetsToReturn = 100;
            const int MaxTotalResults = 100;

            // oldest id you already have for this search term
            ulong sinceID = 1;

            // used after the first query to track current session
            ulong maxID;

            var combinedSearchResults = new List<Status>();

            List<Status> tweets =
                (from tweet in twitterCtx.Status
                 where tweet.Type == StatusType.User &&
                       tweet.ScreenName == twitterHandle &&
                       tweet.Count == MaxTweetsToReturn &&
                       tweet.SinceID == sinceID &&
                       tweet.TweetMode == TweetMode.Extended &&
                       tweet.IncludeRetweets == false
                       //TODO: && tweet.FullText.Contains("Sitecore")
                 select tweet).ToList();

                combinedSearchResults.AddRange(tweets);
            ulong previousMaxID = ulong.MaxValue;
            do
            {
                // one less than the newest id you've just queried
                maxID = tweets.Min(status => status.StatusID) - 1;

                Debug.Assert(maxID < previousMaxID);
                previousMaxID = maxID;

                tweets =
                    (from tweet in twitterCtx.Status
                     where tweet.Type == StatusType.User &&
                           tweet.ScreenName == twitterHandle &&
                           tweet.Count == MaxTweetsToReturn &&
                           tweet.MaxID == maxID &&
                           tweet.SinceID == sinceID &&
                           tweet.TweetMode == TweetMode.Extended &&
                           tweet.IncludeRetweets == false
                     select tweet).ToList();

                combinedSearchResults.AddRange(tweets);

            } while (tweets.Any() && combinedSearchResults.Count < MaxTotalResults);

            return combinedSearchResults;
        }
    }
}
