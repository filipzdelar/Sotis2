using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models
{
    public class Question
    {
        [Key]
        public long ID { get; set; }
        public string QuestionText { get; set; }
        public Test Test { get; set; }
    }
}
