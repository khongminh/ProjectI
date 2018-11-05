using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using University.Data.Repository;
using University.Entity;
using University.Web.Models;

namespace University.Web.Controllers
{
	[Produces("application/json")]
	[Route("Department")]
	public class DepartmentManagerController : Controller
    {
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;
		private readonly DepartmentRepository departmentRepository;
		private readonly IRepository<Student> studentRepository;
		private readonly IRepository<Teacher> teacherRepository;
		private readonly UserRepository userRepository;

		public DepartmentManagerController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, DepartmentRepository departmentRepository,
		  IRepository<Student> studentRepository, IRepository<Teacher> teacherRepository,
		  UserRepository userRepository)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.departmentRepository = departmentRepository;
			this.studentRepository = studentRepository;
			this.teacherRepository = teacherRepository;
			this.userRepository = userRepository;
		}

		[Route("~/Department/Index")]
		public IActionResult Index()
		{
			return View();
		}

		[Route("~/api/GetStudents")]
		[HttpGet]
		public async Task<IActionResult> ListStudent()
        {
			var userId = userManager.GetUserId(User);
			var department = this.departmentRepository.GetAll().Where(d => d.AccountId == userId).FirstOrDefault();
			if(department == null)
			{
				await signInManager.SignOutAsync();
				return RedirectToPage("Login");
			}
			var students = this.studentRepository.GetAll().Where(s => s.DepartmentId == department.Id);

			return Json(students);
		}

		[HttpPost]
		[Route("~/api/AddStudent")]
		public async Task<IActionResult> CreateStudent([FromBody]StudentVM studentVM)
		{
			if (studentVM.Code == null)
				return Json(new { result = "Failed post" });

			var userId = userManager.GetUserId(User);
			var department = this.departmentRepository.GetAll().Where(d => d.AccountId == userId).FirstOrDefault();
			if (department == null)
			{
				await signInManager.SignOutAsync();
				return RedirectToPage("Login");
			}

			var accountId = await this.userRepository.CreateUserRole(studentVM.Code, studentVM.Code, "Student");

			if(accountId != null)
			{
				var student = new Student();
				student.AccountId = accountId;
				student.Code = studentVM.Code;
				student.Name = studentVM.Name;
				student.Information = studentVM.Information;
				student.DepartmentId = department.Id;
				student.Department = department;

				this.studentRepository.Insert(student);
				await this.studentRepository.SaveChangeAsync();
				return Ok();
			}
			return BadRequest();
		}


		[HttpPut]
		[Route("~/api/UpdateStudent/{id}")]
		public async Task<IActionResult> UpdateStudent(long id, [FromBody] StudentVM studentVM)
		{
			if (studentVM.Id != id)
				return BadRequest();

			var student = await this.studentRepository.GetAsync(id);
			if (student == null)
				return NotFound();

			student.Code = studentVM.Code;
			student.Name = studentVM.Name;
			student.Information = studentVM.Information;

			this.studentRepository.Update(student);
			await this.studentRepository.SaveChangeAsync();
			return NoContent();
		}


		[HttpDelete]
		[Route("~/api/DeleteStudent/{id}")]
		public async Task<IActionResult> DeleteStudent(long id)
		{
			var student = await this.studentRepository.GetAsync(id);
			if (student == null)
				return NotFound();
			await this.userRepository.DeleteUser(student.AccountId);
			this.studentRepository.Delete(student);
			await this.studentRepository.SaveChangeAsync();

			return NoContent();
		}
	}
}
