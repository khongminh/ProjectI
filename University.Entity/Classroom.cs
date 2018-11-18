using System;
using System.Collections.Generic;
using System.Text;

namespace University.Entity
{
	public class Classroom : BaseEntity
	{
		public string Code { get; set; }
		public long Semester { get; set; }
		public int MaxStudent { get; set; }

		public ICollection<TimeSlot> TimeSlots { get; set; }

		public long CourseId { get; set; }
		public Course Course { get; set; }

		public long TeacherId { get; set; }
		public Teacher Teacher { get; set; }

		public ICollection<Enrollment> Enrollments { get; set; }

	}
}
