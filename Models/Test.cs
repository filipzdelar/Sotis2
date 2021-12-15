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

        public Test(TimeSpan testDuration, Course course, string name)
        {
            TestDuration = testDuration;
            Course = course;
            Name = name;
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

        public Test(long testId, TimeSpan testDuration, Course course)
        {
            ID = testId;
            TestDuration = testDuration;
            Course = course;
        }

        public Test(long testId, TimeSpan testDuration, Course course, string name)
        {
            ID = testId;
            TestDuration = testDuration;
            Course = course;
            Name = name;
        }



        public Test(long testId, TimeSpan testDuration, Course course, string name, DateTime startOfTest)
        {
            ID = testId;
            TestDuration = testDuration;
            Course = course;
            Name = name;
            StartOfTest = startOfTest;
        }

        [Key]
        public long ID { get; set; }

        public string Name { get; set; }

        public DateTime StartOfTest { get; set; }

        public TimeSpan TestDuration { get; set; }
        public Subject Subject { get; set; }
        public Course Course { get; set; }
    }
}
