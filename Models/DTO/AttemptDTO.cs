using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.DTO
{
    public class AttemptDTO
    {

        public AttemptDTO()
        {

        }

        public AttemptDTO(List<TmpQuestionDTO> tmpQuestionDTOs)
        {
            TmpQuestionDTOs = tmpQuestionDTOs;
        }

        public List<TmpQuestionDTO> TmpQuestionDTOs { get; set; }
        public int TmpSerialQuestion { get; set; }
        public int TotalNumberOfQuestions { get; set; }
    }
}
