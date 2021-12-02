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

        public List<QuestionDTO> QuestionsDTO { get; set; }
        public TimeSpan TestDuration { get; set; }
    }
}
