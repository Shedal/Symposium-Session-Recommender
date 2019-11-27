using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Recommender.Foundation.CognitiveServices.Models
{
    [SitecoreType(TemplateId = Ids.TemplateId, AutoMap = true, Cachable = true)]
    public class CognitiveServicesEndpoint
    {
        [SitecoreField(FieldId = Ids.EndpointUrlFieldId)]
        public virtual Link EndpointUrl { get; set; }

        [SitecoreField(FieldId = Ids.ApiKeyFieldId)]
        public virtual string ApiKey { get; set; }
    }

    internal class Ids
    {
        public const string TemplateId = "F4750BA4-B7AD-4D29-AC7D-C912DF5F392A";
        public const string EndpointUrlFieldId = "76402623-5736-4A3A-B02B-D6562CF91876";
        public const string ApiKeyFieldId = "84F7DC24-EB17-40B2-B9F6-57ED0DD3BE7A";
    }
}