using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.DTO
{
    public class TmpQuestionDTO
    {

        public TmpQuestionDTO()
        {

        }

        public TmpQuestionDTO(List<TmpAnsware> tmpAnswaresDTO, string questionText)
        {
            TmpAnswaresDTO = tmpAnswaresDTO;
            QuestionText = questionText;
        }

        public TmpQuestionDTO(Question question)
        {
            QuestionText = question.QuestionText;
        }

        public List<TmpAnsware> TmpAnswaresDTO { get; set; }
        public string QuestionText { get; set; }
    }
}
