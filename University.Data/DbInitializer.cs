using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Data
{
	public class DbInitializer
	{
		private UniversityContext _context;
		private UserManager<IdentityUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;

		public DbInitializer(UniversityContext context,
							 UserManager<IdentityUser> userManager,
							 RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task CreateRole()
		{
			if(!_context.Roles.Any())
			{
				var systemAdminRole = new IdentityRole("SystemAdmin");
				await _roleManager.CreateAsync(systemAdminRole);

				var departmentAdminRole = new IdentityRole("DepartmentAdmin");
				await _roleManager.CreateAsync(departmentAdminRole);

				var studentRole = new IdentityRole("Student");
				await _roleManager.CreateAsync(studentRole);

				var teacherRole = new IdentityRole("Teacher");
				await _roleManager.CreateAsync(teacherRole);

				await _context.SaveChangesAsync();
			}
		}

		public async Task CreateUser()
		{
			if(!_context.Users.Any())
			{
				var systemAdmin = new IdentityUser()
				{
					UserName = "SystemAdmin",
				};
				await _userManager.CreateAsync(systemAdmin, "123456aA!");
				await _userManager.AddToRoleAsync(systemAdmin, "SystemAdmin");
				await _context.SaveChangesAsync();				
			}
		}

		public async Task SeedDataAsync()
		{
			await CreateRole();
			await CreateUser();
		}
	}
}
