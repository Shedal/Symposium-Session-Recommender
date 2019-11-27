using Recommender.Foundation.CollectionModel.Events;
using Recommender.Foundation.CollectionModel.Facets;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;

namespace Recommender.Foundation.CollectionModel
{
    public class CustomDataModel
    {
        public static XdbModel Model { get; } = BuildModel();

        private static XdbModel BuildModel()
        {
            XdbModelBuilder xdbModelBuilder = new XdbModelBuilder("Recommender", new XdbModelVersion(1, 1));
            xdbModelBuilder.ReferenceModel(Sitecore.XConnect.Collection.Model.CollectionModel.Model);
            xdbModelBuilder.DefineFacet<Contact, AttendeeProfile>("AttendeeProfile");
            xdbModelBuilder.DefineFacet<Contact, SocialImportInfo>("SocialImportInfo");
            xdbModelBuilder.DefineFacet<Interaction, SocialPostInfo>("SocialPostInfo");
            xdbModelBuilder.DefineEventType<SocialPostEvent>(false);
            return xdbModelBuilder.BuildModel();
        }
    }
}