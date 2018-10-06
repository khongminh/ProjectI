using System;
using System.Collections.Generic;
using System.Text;

namespace University.Entity
{
	public class Prerequisite : BaseEntity
	{
		public long CourseId { get; set; }
		public long PrereqId { get; set; }
	}
}
