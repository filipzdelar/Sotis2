using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.DTO
{
    public class QuestionWithAnswaresDTO : Question
    {

        public QuestionWithAnswaresDTO()
        {

        }

        public QuestionWithAnswaresDTO(List<Answare> answares)
        {
            Answares = answares;
        }

        public List<Answare> Answares { get; set; }
    }
}
