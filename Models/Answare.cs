﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models
{
    public class Answare
    {
        [Key]
        public long ID { get; set; }
        public string AnswareText { get; set; }
        public bool IsItTrue { get; set; }

        [ForeignKey("Question")]
        public long QuestionID { get; set; }
    }
}
