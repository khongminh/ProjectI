using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Entity;

namespace University.Web.Models
{
	public class OpenCourseVM
	{
		public long Id { get; set; }
		public string Code { get; set; }
		public long Semester { get; set; }

		public string CourserId { get; set; }
		public Course Course { get; set; }

		public string TeacherId { get; set; }
		public Teacher Teacher { get; set; }

		public IList<Student> Students { get; set; }
	}
}
