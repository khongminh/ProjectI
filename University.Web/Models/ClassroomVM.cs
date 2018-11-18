using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University.Web.Models
{
	public class ClassroomVM
	{
		public string Code { get; set; }
		public long Semester { get; set; }
		public int MaxStudent { get; set; }

		public IEnumerable<TimeSlotVM> TimeSlots { get; set; }

		public long CourseId { get; set; }

		public long TeacherId { get; set; }
	}
}
