using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Recommender.AgendaContent.Models
{
    public class RoomResourceModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("building")]
        public string Building { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}