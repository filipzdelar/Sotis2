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
using Sotis2.Models.Relations;

namespace Sotis2.Data
{
    public class DBContext : IdentityDbContext<AppUser>
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Attempt> Attempts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answare> Answares { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<TmpAnsware> TmpAnswares { get; set; }
        public DbSet<StudentsAnsware> StudentsAnswares { get; set; }
        public DbSet<EdgeQD> EdgeQDs { get; set; }
        public DbSet<EdgeDD> EdgeDDs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Test>().ToTable("Test");
            modelBuilder.Entity<Attempt>().ToTable("Attempt");
            modelBuilder.Entity<Question>().ToTable("Question");
            modelBuilder.Entity<Answare>().ToTable("Answare");
            modelBuilder.Entity<TmpAnsware>().ToTable("TmpAnsware");
            modelBuilder.Entity<Subject>().ToTable("Subject");
            modelBuilder.Entity<Domain>().ToTable("Domain"); 
            modelBuilder.Entity<AppUser>().ToTable("AppUser");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<StudentsAnsware>().ToTable("StudentsAnsware");
            modelBuilder.Entity<EdgeQD>().ToTable("EdgeQD");
            modelBuilder.Entity<EdgeDD>().ToTable("EdgeDD");


            //modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            //modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            //modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        }

    }
}
