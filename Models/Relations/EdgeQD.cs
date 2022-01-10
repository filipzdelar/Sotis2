using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.Relations
{
    public class EdgeQD
    {

        public EdgeQD() { }

        public EdgeQD(long v1, long v2)
        {
            QuestionFromID = v1;
            DomainToID = v2;
        }

        [Key]
        public int ID { get; set; }

        [ForeignKey("Question")]
        public long QuestionFromID { get; set; }

        [ForeignKey("Domain")]
        public long DomainToID { get; set; }
    }
}
