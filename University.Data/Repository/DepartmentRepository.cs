using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using University.Entity;

namespace University.Data.Repository
{
	public class DepartmentRepository : Repository<Department>
	{
		public DepartmentRepository(UniversityContext context) : base(context)
		{
		}

		public Task<Department> GetStudents(long id)
		{
			var department = this.entities.Include(d => d.Students)
									.SingleOrDefaultAsync(d => d.Id == id);
			return department;
		}

		public Task<Department> GetCourses(long id)
		{
			var department = this.entities.Include(d => d.Students)
									.SingleOrDefaultAsync(d => d.Id == id);
			return department;
		}

		public Task<Department> GetTeachers(long id)
		{
			var department = this.entities.Include(d => d.Students)
									.SingleOrDefaultAsync(d => d.Id == id);
			return department;
		}


	}
}
