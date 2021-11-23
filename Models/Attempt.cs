using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models
{
    [Table("tbAttempts")]
    public class Attempt
    {
        public Attempt()
        {
        }

        public Attempt(TimeSpan takenTime, float accuracy, int grade, DateTime startTime, DateTime endTime)
        {
            TakenTime = takenTime;
            Accuracy = accuracy;
            Grade = grade;
            StartTime = startTime;
            EndTime = endTime;
        }

        public Attempt(long iD, TimeSpan takenTime, float accuracy, int grade, DateTime startTime, DateTime endTime)
        {
            ID = iD;
            TakenTime = takenTime;
            Accuracy = accuracy;
            Grade = grade;
            StartTime = startTime;
            EndTime = endTime;
        }

        [Key]
        public long ID { get; set; }

        public TimeSpan TakenTime { get; set; }
        public float Accuracy { get; set; }
        public int Grade { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
