using System;
using System.Collections.Generic;
using System.Text;

namespace University.Entity
{
	public class TimeEnrollment : BaseEntity
	{
		public long Semester { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
	}
}
