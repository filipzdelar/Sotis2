using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.Graph.DTO
{
    public class GraphDTO
    {
        [JsonProperty("nodes")]
        public IList<NodeDto> Nodes { get; set; }

        [JsonProperty("edges")]
        public IList<EdgeDto> Edges { get; set; }
    }
}
