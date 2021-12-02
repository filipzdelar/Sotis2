using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.Graph
{
    public class Node
    {
        [JsonProperty("node_id")]
        public int Id { get; set; }

        [JsonProperty("node_name")]
        public string Name { get; set; }

        [JsonProperty("attributes")]
        public IList<Attribute> Attributes { get; set; }
    }
}
