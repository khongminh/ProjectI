using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace University.Web.Models
{
	public class TeacherVM
	{
		public long Id { get; set; }

		[Required]
		public string Code { get; set; }
		[Required]
		public string Name { get; set; }
		public string Information { get; set; }
	}
}
