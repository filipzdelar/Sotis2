using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.DTO
{
    public class SemanticWebDTO
    {
        public int? id { get; set; }
        public int? lowerBoundery { get; set; }
        public int? upperBoundery { get; set; }
        public bool order { get; set; }

    }
}
