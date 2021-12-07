using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.DTO
{
    public class QuestionDTO
    {

        public QuestionDTO()
        {

        }

        public List<Answare> AnswaresDTO { get; set; }
        public int SerialNumber {get; set;}
        public string QuestionText { get; set; }
    }
}
