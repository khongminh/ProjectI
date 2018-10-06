using System;
using System.Collections.Generic;
using System.Text;

namespace University.Entity
{
	public class TimeSlot : BaseEntity
	{
		public long ClassroomId { get; set; }
		public Classroom Classroom { get; set; }

		public string Day { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }

		public string Room { get; set; }
	}
}
