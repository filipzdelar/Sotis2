using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models
{
    [Table("tbTests")]
    public class Test
    {
        public Test()
        {

        }

        public Test(TimeSpan testDuration)
        {
            TestDuration = testDuration;
        }

        public Test(long testId, TimeSpan testDuration)
        {
            ID = testId;
            TestDuration = testDuration;
        }

        [Key]
        public long ID { get; set; }
        public TimeSpan TestDuration { get; set; }
        public Subject Subject { get; set; }
    }
}
