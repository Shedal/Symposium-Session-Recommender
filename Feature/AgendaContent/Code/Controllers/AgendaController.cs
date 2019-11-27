using System.Collections.Generic;
using System.Web.Mvc;
using Glass.Mapper.Sc.Web.Mvc;
using Recommender.AgendaContent.Models;
using Recommender.Foundation.Utilities;
using Sitecore.Diagnostics;
using Sitecore.XConnect;

namespace Recommender.AgendaContent.Controllers
{
    public class AgendaController : GlassController
    {

        public List<SymposiumSession> SymposiumSessions { get; set; }
        public ActionResult AgendaCalendar()
        {
            //var sitecoreContext = SitecoreContext;
            //var agendaModel = new AgendaModel(sitecoreContext, null);
            //return View("~/Views/Shared/Agenda/AgendaCalendar.cshtml", agendaModel);

            //TODO: uncomment when xConnecct works

            var contactUtil = new ContactUtil();
            Contact currentContact = contactUtil.GetCurrentContact();

            if (currentContact != null)
            {
                Log.Info("Current contact id: ", currentContact.Id);
                var currentAttendeeProfile = contactUtil.GetAttendeeProfileForContact(currentContact);
                var CurrentAttendeePersonalInformation = contactUtil.GetPersonalInformationForContact(currentContact);

                if (currentAttendeeProfile != null && currentAttendeeProfile.TrackScores != null &&
                    currentAttendeeProfile.TopicScores != null
                    && currentAttendeeProfile.TrackScores.Count > 0 && currentAttendeeProfile.TopicScores.Count > 0)
                {
                    var agendaModel = new AgendaModel(SitecoreContext, currentAttendeeProfile, CurrentAttendeePersonalInformation);

                    agendaModel.Contact = currentContact;
                    return View("~/Views/Shared/Agenda/AgendaCalendar.cshtml", agendaModel);
                }
                else
                {
                    var agendaModel = new AgendaModel(SitecoreContext, null, null);
                    return View("~/Views/Shared/Agenda/AgendaCalendar.cshtml", agendaModel);
                }

            }
            else
            {
                Log.Info("Current contact not found:", "AgendaContent ");

                var agendaModel = new AgendaModel(SitecoreContext, null, null);
                return View("~/Views/Shared/Agenda/AgendaCalendar.cshtml", agendaModel);
            }
        }
    }
}