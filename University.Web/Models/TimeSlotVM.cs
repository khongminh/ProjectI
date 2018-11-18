using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University.Web.Models
{
	public class TimeSlotVM
	{
		public string Day { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public string Room { get; set; }
	}
}
