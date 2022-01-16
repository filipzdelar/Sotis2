using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.Graph.DTO
{
    public class GraphDTO
    {
        public GraphDTO()
        {

        }

        public GraphDTO(IList<NodeDto> Nodes, IList<EdgeDto> Edges)
        {
            this.Nodes = Nodes;
            this.Edges = Edges;
        }

        public GraphDTO(IList<NodeDto> Nodes, IList<EdgeDto> Edges, long? testID)
        {
            this.Nodes = Nodes;
            this.Edges = Edges;
            this.testID = testID;
        }

        [JsonProperty("nodes")]
        public IList<NodeDto> Nodes { get; set; }

        [JsonProperty("edges")]
        public IList<EdgeDto> Edges { get; set; }

        [JsonProperty("testID")]
        public long? testID { get; set; }


    }
}
