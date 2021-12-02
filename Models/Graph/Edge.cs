using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.Graph
{
    public class Edge
    {
        [JsonProperty("edge_table_name")]
        public string Name { get; set; }

        [JsonProperty("edge_table_id")]
        public int Id { get; set; }

        [JsonProperty("from_node_table_id")]
        public int From { get; set; }

        [JsonProperty("to_node_table_id")]
        public int To { get; set; }

        [JsonProperty("attributes")]
        public IList<Attribute> Attributes { get; set; }
    }
}
