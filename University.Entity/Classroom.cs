using System;
using System.Collections.Generic;
using System.Text;

namespace University.Entity
{
	public class Classroom : BaseEntity
	{
		public string Code { get; set; }
		public string Semester { get; set; }

		public string CourserId { get; set; }
		public Course Course { get; set; }

		public string TeacherId { get; set; }
		public Teacher Teacher { get; set; }

		public IList<Student> Students { get; set; }

	}
}
