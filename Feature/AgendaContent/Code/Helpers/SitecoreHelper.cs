using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.SecurityModel;

namespace Recommender.AgendaContent.Helpers
{
    public static class SitecoreHelper
    {
        /// <summary>
        /// Is the given item derived from a specific template?
        /// </summary>
        /// <param name="item">item to test</param>
        /// <param name="templateId">template ID to look for</param>
        /// <param name="checkSecurity">test the security state of the item?</param>
        /// <returns></returns>
        public static bool IsDerived(this Item item, string templateId, bool checkSecurity = false)
        {
            Guid templateGuid;
            if (!Guid.TryParse(templateId, out templateGuid))
                return false;
            return IsDerived(item, new ID(templateGuid), checkSecurity);
        }

        /// <summary>
        /// Is the given item derived from a specific template?
        /// </summary>
        /// <param name="item">item to test</param>
        /// <param name="templateId">template data ID to look for </param>
        /// <param name="checkSecurity">test the security state of the item?</param>
        /// <returns></returns>
        public static bool IsDerived(this Item item, ID templateId, bool checkSecurity = false)
        {
            if (item == null)
                return false;

            if (templateId.IsNull)
                return false;

            var state = SecurityState.Enabled;
            if (!checkSecurity) state = SecurityState.Disabled;

            using (new SecurityStateSwitcher(state))
            {
                var templateItem = item.Database.Templates[templateId];

                var isDerived = false;
                if (templateItem != null)
                {
                    var template = TemplateManager.GetTemplate(item);
                    if (template == null) return false;
                    isDerived = template.ID == templateItem.ID || template.DescendsFrom(templateItem.ID);
                }

                return isDerived;
            }
        }
    }
}