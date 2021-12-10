using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.DTO
{
    public class TestQuestionAnswerDTO
    {
        public TestQuestionAnswerDTO()
        {

        }

        public long? Id { get; set; }
        public List<QuestionWithAnswaresDTO> qWA { get; set; }
        //public List<QuestionDTO> QuestionsDTO { get; set; }
        public TimeSpan TestDuration { get; set; }
    }
}
