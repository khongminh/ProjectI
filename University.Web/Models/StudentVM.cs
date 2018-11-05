using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University.Web.Models
{
	public class StudentVM
	{
		public long Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Information { get; set; }
	}
}
