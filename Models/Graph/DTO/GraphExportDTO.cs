using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.Graph.DTO
{
    public class GraphExportDTO
    {
        public int x { get; set; }
        public int y { get; set; }
        public string id { get; set; }
        public List<int> connections { get; set; }
    }
}
