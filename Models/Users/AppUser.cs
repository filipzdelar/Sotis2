using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.Users
{
    [Table("tbAppUsers")]
    public class AppUser : IdentityUser
    {

        //[Key]
        //public long ID { get; set; }

        //[Key]
        //public string ID { get; set; }

        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public string Address { get; set; }
        [PersonalData]
        public string Country { get; set; }
        [PersonalData]
        public string City { get; set; }
        
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
