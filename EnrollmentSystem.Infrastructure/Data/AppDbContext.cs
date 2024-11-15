using EnrollmentSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // This ctor gets called fe program.cs 
            // Connection string is specified in the AddDbContext method in program.cs, so you don't need it in OnConfiguring.

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            // I used Data annotations inside the entities, so no need to use fluent api

            // Also I can use inside Entities "convention-based relationships" or "convention-based foreign keys without Data annotations 

            //If a property in an entity is named {RelatedEntity}ID & its type matches the PK type of the related entity, EF Core infers it as a FK

            modelBuilder.Entity<Admin>().HasData(
                new Admin { AdminID = 1 } //Didn't work and I added it from database
);

            base.OnModelCreating(modelBuilder);
        }

    }
}
