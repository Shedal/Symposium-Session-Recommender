using Recommender.Foundation.CollectionModel.Facets;
using Sitecore.Analytics;
using Sitecore.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Configuration;
using Sitecore.XConnect.Collection.Model;
using Contact = Sitecore.XConnect.Contact;

namespace Recommender.Foundation.Utilities
{
    public class ContactUtil
    {
        public Contact GetCurrentContact()
        {
            string identifier = Tracker.Current.Contact.ContactId.ToString("N");

            IEntityReference<Contact> contactReference = new IdentifiedContactReference("xDB.Tracker", identifier);

            ContactExpandOptions expandOptions = new ContactExpandOptions(FacetKeys.AttendeeProfile, PersonalInformation.DefaultFacetKey);

            using (IXdbContext client = SitecoreXConnectClientConfiguration.GetClient())
            {
                IEntityReference<Contact> twitterContactReference = contactReference;

                Contact contact = client.Get(twitterContactReference, expandOptions);

                if (contact == null)
                {
                    Log.Info("Current contact could not be found in GetAttendeeProfileForContact: " + identifier, typeof(ContactUtil));
                }

                return contact;
            }
        }

        public AttendeeProfile GetAttendeeProfileForContact(Contact contact)
        {
            if (contact != null)
            {
                return contact.GetFacet<AttendeeProfile>();
            }

            Log.Info("Contact was null in GetAttendeeProfileForContact", typeof(ContactUtil));

            return null;
        }

        public PersonalInformation GetPersonalInformationForContact(Contact contact)
        {
            if (contact != null)
            {
                return contact.GetFacet<PersonalInformation>();
            }

            Log.Info("Contact was null in GetPersonalInformationForContact", typeof(ContactUtil));

            return null;
        }
    }
}