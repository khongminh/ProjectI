using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace University.Web.Models
{
	public class CreateDepartmentVM
	{
		[Required]
		public string DeptName { get; set; }

		[Required]
		public string Address { get; set; }

		[Required]
		public string Website { get; set; }

		[Required]
		public string AdminAccount { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
