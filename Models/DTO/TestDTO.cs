using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.DTO
{
    public class TestDTO : Test
    {
        public TestDTO()
        {

        }

        public TestDTO(List<Question> questions)
        {
            Questions = questions;
        }

        public List<Question> Questions { get; set; }
    }
}
