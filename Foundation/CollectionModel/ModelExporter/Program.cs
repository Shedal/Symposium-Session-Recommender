using System.IO;
using Recommender.Foundation.CollectionModel;
using Sitecore.XConnect.Serialization;

namespace ModelExporter
{
    class Program
    {
        static void Main()
        {
            var json = XdbModelWriter.Serialize(CustomDataModel.Model);
            File.WriteAllText(CustomDataModel.Model.FullName + ".json", json);
        }
    }
}
