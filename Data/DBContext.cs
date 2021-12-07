using Microsoft.EntityFrameworkCore;
using Sotis2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Sotis2.Models.Users;
using Sotis2.Models.DTO;

namespace Sotis2.Data
{
    public class DBContext : DbContext// IdentityDbContext<AppUser>
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Attempt> Attempts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answare> Answares { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>().ToTable("Test");
            modelBuilder.Entity<Attempt>().ToTable("Attempt");
            modelBuilder.Entity<Question>().ToTable("Question");
            modelBuilder.Entity<Answare>().ToTable("Answare");
            modelBuilder.Entity<Subject>().ToTable("Subject");


            //modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            //modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            //modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        }

        public DbSet<Sotis2.Models.DTO.QuestionWithAnswaresDTO> QuestionWithAnswaresDTO { get; set; }
    }
}
