﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace University.Data.Repository
{
	public class UserRepository
	{
		private UniversityContext _context;
		private UserManager<IdentityUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;

		public UserRepository(UniversityContext context,
							 UserManager<IdentityUser> userManager,
							 RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<string> CreateUserRole(string userName, string password, string roleName)
		{
			var studentUser = new IdentityUser()
			{
				UserName = userName
			};
			try
			{
				await _userManager.CreateAsync(studentUser, password);
				await _context.SaveChangesAsync();
				await _userManager.AddToRoleAsync(studentUser, roleName);
				await _context.SaveChangesAsync();
			}
			catch (Exception)
			{
				return null;
			}
			var user = await _userManager.FindByNameAsync(userName);
			return user.Id;
		}
	}
}
