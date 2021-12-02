using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sotis2.Models.Graph
{
    public class Graph
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nodes")]
        public IList<Node> Nodes { get; set; }

        [JsonProperty("edges")]
        public IList<Edge> Edges { get; set; }
    }
}
