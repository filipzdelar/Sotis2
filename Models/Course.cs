using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models
{

    [Table("tbCourses")]
    public class Course
    {
        public Course()
        {
        }

        [Key]
        public long ID { get; set; }

        public string Name { get; set; }

    }
}
