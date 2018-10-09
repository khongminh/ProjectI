using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using University.Data.Repository;
using University.Entity;

namespace University.Web.Controllers
{
    public class DepartmentManagerController : Controller
    {
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;
		private readonly DepartmentRepository departmentRepository;
		private readonly Repository<Student> studentRepository;
		private readonly Repository<Teacher> teacherRepository;

		public DepartmentManagerController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, DepartmentRepository departmentRepository,
		  Repository<Student> studentRepository, Repository<Teacher> teacherRepository )
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.departmentRepository = departmentRepository;
			this.studentRepository = studentRepository;
			this.teacherRepository = teacherRepository
		}

        public async Task<IActionResult> StudentIndex()
        {
			var userId = userManager.GetUserId(User);
			var department = this.departmentRepository.GetAll().Where(d => d.AccountId == userId).FirstOrDefault();
			if(department == null)
			{
				await signInManager.SignOutAsync();
				return RedirectToPage("Login");
			}
			var students = this.studentRepository.GetAll().Where(s => s.DepartmentId == department.Id);

            return View(students);
        }
    }
}