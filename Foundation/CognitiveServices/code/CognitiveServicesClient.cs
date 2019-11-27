using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.IoC;
using Newtonsoft.Json;
using Recommender.Foundation.CognitiveServices.Models;

namespace Recommender.Foundation.CognitiveServices
{
    public class CognitiveServicesClient
    {
        public class StringTable
        {
            public string[] ColumnNames { get; set; }
            public string[,] Values { get; set; }
        }

        public class ServiceOutput
        {
            public string Type { get; set; }
            public ServiceOuputValue Value { get; set; }
        }

        public class ServiceOuputValue
        {
            public string[] ColumnNames { get; set; }
            public string[] ColumnTypes { get; set; }
            public string[][] Values { get; set; }
        }

        private static readonly Regex ClassRegex = new Regex(".+\"(?<className>.+)\".*");

        public async System.Threading.Tasks.Task<Dictionary<string, double>> InvokeTracksService(string text)
        {
            ISitecoreContext context = SitecoreContextFactory.Default.GetSitecoreContext();
            var endpoint = context.GetItem<CognitiveServicesEndpoint>("/sitecore/content/CognitiveServices/Tracks");
            
            return await InvokeScoringService(text, endpoint);
        }

        public async System.Threading.Tasks.Task<Dictionary<string, double>> InvokeTopicsService(string text)
        {
            ISitecoreContext context = SitecoreContextFactory.Default.GetSitecoreContext();
            var endpoint = context.GetItem<CognitiveServicesEndpoint>("/sitecore/content/CognitiveServices/Topics");

            return await InvokeScoringService(text, endpoint);
        }

        public async System.Threading.Tasks.Task<Dictionary<string, double>> InvokeScoringService(string text, CognitiveServicesEndpoint endpoint)
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, StringTable>() {
                        {
                            "Unigrams Web Service Input",
                            new StringTable()
                            {
                                ColumnNames = new[] {"text_column"},
                                Values = new[,]
                                {
                                    {
                                        text
                                    }
                                }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", endpoint.ApiKey);

                client.BaseAddress = new Uri(endpoint.EndpointUrl.Url);
                

                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string output = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, ServiceOutput>>>(output);

                    string[] columnNames = result["Results"].First().Value.Value.ColumnNames;
                    string[] values = result["Results"].First().Value.Value.Values[0];
                    Array.Resize(ref columnNames, columnNames.Length - 1);
                    Array.Resize(ref values, values.Length - 1);

                    return Enumerable.Range(0, columnNames.Length)
                        .ToDictionary(
                            i => ClassRegex.Match(columnNames[i]).Groups["className"].Value,
                            i => Convert.ToDouble(values[i], CultureInfo.InvariantCulture));
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var ex = new Exception($"The request failed with status code: {response.StatusCode}. Full response: {responseContent}");
                    throw ex;
                }
            }
        }
    }
}