using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recommender.AttendeeForm.Modules;
using Recommender.Foundation.CognitiveServices;
using Recommender.Foundation.CollectionModel.Facets;
using Sitecore.Analytics;
using Sitecore.Analytics.Model;
using Sitecore.Analytics.Tracking;
using Sitecore.Common;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.StringExtensions;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Configuration;
using Sitecore.XConnect.Collection.Model;
using Contact = Sitecore.XConnect.Contact;

namespace Recommender.AttendeeForm.SubmitActions
{
    /// <summary>
    /// Submit action for updating <see cref="PersonalInformation"/> and <see cref="EmailAddressList"/> facets of a <see cref="Sitecore.XConnect.Contact"/>.
    /// </summary>
    /// <seealso cref="Sitecore.ExperienceForms.Processing.Actions.SubmitActionBase{UpdateContactData}" />
    public class TriggerRecommenderEngineAction : SubmitActionBase<string>
    {
        private static class FieldIds
        {
            public static readonly Guid TwitterHandle = new Guid("{32B7F55A-DA13-4471-9447-6658A89BA880}");
            public static readonly Guid LinkedInProfile = new Guid("{B2C1A83D-6D94-4D04-B0E7-90F57C0C5FBC}");
            public static readonly Guid FreeText = new Guid("{BBFF7F74-4C11-4899-AD9D-12EB07FBB74E}");
        }

        private static readonly string IdentificationSource = "sitecoreextranet";

        public TriggerRecommenderEngineAction(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            if (!Tracker.IsActive)
            {
                Tracker.StartTracking();
            }
            
            string twitterHandle = GetFieldValue(FieldIds.TwitterHandle, formSubmitContext.Fields);
            string linkedInProfileUrl = GetFieldValue(FieldIds.LinkedInProfile, formSubmitContext.Fields);
            string freeText = GetFieldValue(FieldIds.FreeText, formSubmitContext.Fields);

            try
            {
                Tracker.Current.Contact.ContactSaveMode = ContactSaveMode.AlwaysSave;

                using (IXdbContext client = SitecoreXConnectClientConfiguration.GetClient())
                {
                    //
                    // Identify the contact and set up its facets

                    Tracker.Current.Session.IdentifyAs("twitter", twitterHandle);

                    ContactManager contactManager = (ContactManager) Factory.CreateObject("tracking/contactManager", true);
                    contactManager.SaveContactToCollectionDb(Tracker.Current.Contact);

                    ContactExpandOptions expandOptions = new ContactExpandOptions(FacetKeys.AttendeeProfile, FacetKeys.SocialImportInfo, PersonalInformation.DefaultFacetKey);
                    IEntityReference<Contact> twitterContactReference = new IdentifiedContactReference("twitter", twitterHandle);

                    Contact contact = client.Get(twitterContactReference, expandOptions);

                    AttendeeProfile attendeeProfile = contact.GetFacet<AttendeeProfile>() ?? new AttendeeProfile();
                    attendeeProfile.TwitterHandle = twitterHandle;
                    attendeeProfile.LinkedInProfileUrl = linkedInProfileUrl;
                    attendeeProfile.FreeText = freeText;
                    attendeeProfile.TrackScores.Clear();
                    attendeeProfile.TopicScores.Clear();

                    client.SetFacet<AttendeeProfile>(contact, FacetKeys.AttendeeProfile, attendeeProfile);

                    var socialImportInfo = contact.GetFacet<SocialImportInfo>();
                    if (socialImportInfo == null)
                    {
                        client.SetFacet(contact, FacetKeys.SocialImportInfo, new SocialImportInfo());
                    }

                    client.Submit();

                    //
                    // Import social posts

                    IAttendeeDataImporter[] importers =
                    {
                        new TwitterDataImporter()
                    };

                    foreach (IAttendeeDataImporter importer in importers)
                    {
                        importer.ImportData(client, contact);
                    }

                    client.Submit();

                    //
                    // Score the contact's social posts using machine learning

                    expandOptions.Interactions = new RelatedInteractionsExpandOptions(FacetKeys.SocialPostInfo);
                    contact = client.Get(twitterContactReference, expandOptions);

                    var scorer = new AttendeeScorer(new CognitiveServicesClient());
                    new Func<Task>(() => scorer.ScoreAttendee(client, contact)).SuspendContextLock();

                    client.Submit();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return false;
            }
        }

        private static string GetFieldValue(Guid fieldId, IList<IViewModel> fields)
        {
            IViewModel model = fields.FirstOrDefault(f => Guid.Parse(f.ItemId) == fieldId);

            return model?.GetType().GetProperty("Value")?.GetValue(model, null)?.ToString() ?? string.Empty;
        }
    }
}
