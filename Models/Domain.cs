using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models
{
    public class Domain
    {
        public Domain()
        {

        }

        public Domain(string Type)
        {
            this.Type = Type;
        }

        public Domain(long iD, string answareText, bool isItTrue)
        {
            ID = iD;
            this.Type = Type;
        }

        [Key]
        public long ID { get; set; }
        public string Type { get; set; }

    }
}
