using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models
{
    public class Question
    {
        public Question()
        {
        }

        public Question(string questionText)
        {
            QuestionText = questionText;
        }

        public Question(long iD, string questionText)
        {
            ID = iD;
            QuestionText = questionText;
        }

        public Question(long iD, string questionText, Test test)
        {
            ID = iD;
            QuestionText = questionText;
            Test = test;
        }

        [Key]
        public long ID { get; set; }
        public string QuestionText { get; set; }
        public Test Test { get; set; }

        [ForeignKey("Subject")]
        public long SubjectID { get; set; }
    }
}
