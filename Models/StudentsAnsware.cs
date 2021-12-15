using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models
{
    public class StudentsAnsware
    {
        public StudentsAnsware()
        {

        }


        [Key]
        public long ID { get; set; }
        public string AnswareText { get; set; }
        public bool IsItTrue { get; set; }

        [ForeignKey("Answare")]
        public long AnswareID { get; set; }
    }
}
