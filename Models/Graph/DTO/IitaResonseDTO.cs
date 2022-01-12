using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.Graph.DTO
{
    public class IitaResonseDTO
    {
        public List<List<double>> diff { get; set; }
        public List<List<int>> implications { get; set; }

        [JsonProperty("error.rate")]
        public double ErrorRate { get; set; }

        [JsonProperty("selection.set.index")]
        public int SelectionSetIndex { get; set; }
        public int v { get; set; }
    }
}
