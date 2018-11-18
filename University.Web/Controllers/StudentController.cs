using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using University.Data.Repository;
using University.Entity;
using University.Web.Models;

namespace University.Web.Controllers
{
	[Produces("application/json")]
	public class StudentController : Controller
    {
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;
		private readonly DepartmentRepository departmentRepository;
		private readonly IRepository<Student> studentRepository;
		private readonly UserRepository userRepository;

		public StudentController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, DepartmentRepository departmentRepository,
		  IRepository<Student> studentRepository, IRepository<Teacher> teacherRepository,
		  IRepository<Course> courseRepository, IRepository<Prerequisite> prerequisiteRepository,
		  IRepository<TimeEnrollment> timeEnrollmentRepository,
		  IRepository<Classroom> classroomRepository,
		  UserRepository userRepository)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.departmentRepository = departmentRepository;
			this.studentRepository = studentRepository;
			this.userRepository = userRepository;
		}

		[Route("~/Student/Index")]
		public IActionResult IndexStudent()
		{
			return View();
		}

		[Route("~/api/Student/")]
		[HttpGet]
		public async Task<IActionResult> ListStudentDepartment()
		{
			var userId = userManager.GetUserId(User);
			var department = this.departmentRepository.GetAll().Where(d => d.AccountId == userId).FirstOrDefault();
			if (department == null)
			{
				await signInManager.SignOutAsync();
				return RedirectToPage("Login");
			}
			var students = this.studentRepository.GetAll().Where(s => s.DepartmentId == department.Id);

			return Json(students);
		}

		[HttpPost]
		[Route("~/api/Student/Add")]
		public async Task<IActionResult> CreateStudent([FromBody]StudentVM studentVM)
		{
			var userId = userManager.GetUserId(User);
			var department = this.departmentRepository.GetAll().Where(d => d.AccountId == userId).FirstOrDefault();
			if (department == null)
			{
				await signInManager.SignOutAsync();
				return RedirectToPage("Login");
			}

			var accountId = await this.userRepository.CreateUserRole(studentVM.Code, studentVM.Code, "Student");

			if (accountId != null)
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
		[Route("~/api/Student/Update/{id}")]
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
		[Route("~/api/Student/Delete/{id}")]
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