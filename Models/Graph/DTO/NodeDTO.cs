using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.Graph.DTO
{
    public class NodeDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }


        [JsonProperty("color")]
        public string Color { get; set; }
    }

}
