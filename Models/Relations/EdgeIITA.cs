using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.Relations
{
    public class EdgeDD
    {
        public EdgeDD() { }

        public EdgeDD(long f, long t)
        {
            DomainFromID =  f;
            DomainToID = t;
        }

        [Key]
        public int ID { get; set; }

        [ForeignKey("Domain")]
        public long DomainFromID { get; set; }

        [ForeignKey("Domain")]
        public long DomainToID { get; set; }
    }
}
