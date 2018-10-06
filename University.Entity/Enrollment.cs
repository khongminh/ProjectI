using System;
using System.Collections.Generic;
using System.Text;

namespace University.Entity
{
	public class Enrollment : BaseEntity
	{
		public long ClassroomId { get; set; }
		public Classroom Classroom { get; set; }

		public long StudentId { get; set; }
		public Student Student { get; set; }

		public int MidGrade { get; set; }
		public int FinalGrade { get; set; }
	}
}
