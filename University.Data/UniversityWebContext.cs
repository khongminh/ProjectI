using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using University.Entity;

namespace University.Data
{
    public class UniversityContext : IdentityDbContext<IdentityUser>
    {
        public UniversityContext(DbContextOptions<UniversityContext> options)
            : base(options)
        {
		}

		public DbSet<Student> Students { get; set; }
		public DbSet<Teacher> Teachers { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<Classroom> Classrooms { get; set; }
		public DbSet<Enrollment> Enrollments { get; set; }
		public DbSet<Prerequisite> Prerequisites { get; set; }
		public DbSet<TimeSlot> TimeSlots { get; set; }
		public DbSet<TimeEnrollment> TimeEnrollments { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);


			builder.Entity<Student>().ToTable("Student");
			builder.Entity<Teacher>().ToTable("Teacher");
			builder.Entity<Course>().ToTable("Course");
			builder.Entity<Department>().ToTable("Department");
			builder.Entity<Classroom>().ToTable("Classroom");
			builder.Entity<Enrollment>().ToTable("Enrollment");
			builder.Entity<Prerequisite>().ToTable("Prerequisite");
			builder.Entity<TimeSlot>().ToTable("TimeSlot");
			builder.Entity<TimeEnrollment>().ToTable("TimeEnrollment");
			builder.Entity<IdentityUser>().ToTable("User");
			builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
			builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
			builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
			builder.Entity<IdentityRole>().ToTable("Role");
			builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
			builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
			

			builder.Entity<Student>().HasIndex(s => s.Code).IsUnique(true);
			builder.Entity<Student>().Property(s => s.AccountId).IsRequired(true);
			builder.Entity<Teacher>().HasIndex(t => t.Code).IsUnique(true);
			builder.Entity<Teacher>().Property(t => t.AccountId).IsRequired(true);
			builder.Entity<Department>().HasIndex(d => d.Deptname).IsUnique(true);
			builder.Entity<Department>().Property(d => d.AccountId).IsRequired(true);
			builder.Entity<Course>().HasIndex(s => s.Code).IsUnique(true);
			builder.Entity<Classroom>().HasIndex(c => new { c.Code, c.Semester }).IsUnique(true);
			builder.Entity<Prerequisite>().HasKey(p => new { p.CourseId, p.PrereqId });
			builder.Entity<Prerequisite>().Ignore(p => p.Id);
			builder.Entity<TimeEnrollment>().HasIndex(t => t.Semester).IsUnique(true);
			
		}

	}
}
