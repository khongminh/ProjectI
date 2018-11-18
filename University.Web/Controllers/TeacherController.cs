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
	public class TeacherController : Controller
    {
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;
		private readonly DepartmentRepository departmentRepository;
		private readonly IRepository<Teacher> teacherRepository;
		private readonly UserRepository userRepository;

		public TeacherController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, DepartmentRepository departmentRepository,
		  IRepository<Student> studentRepository, IRepository<Teacher> teacherRepository,
		  IRepository<Course> courseRepository, IRepository<Prerequisite> prerequisiteRepository,
		  IRepository<TimeEnrollment> timeEnrollmentRepository,
		  IRepository<Classroom> classroomRepository,
		  UserRepository userRepository)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.departmentRepository = departmentRepository;
			this.teacherRepository = teacherRepository;
			this.userRepository = userRepository;
		}

		[Route("~/Teacher/Index")]
		public IActionResult IndexTeacher()
		{
			return View();
		}

		[Route("~/api/Teacher")]
		[HttpGet]
		public async Task<IActionResult> ListTeacher()
		{
			var userId = userManager.GetUserId(User);
			var department = this.departmentRepository.GetAll().Where(d => d.AccountId == userId).FirstOrDefault();
			if (department == null)
			{
				await signInManager.SignOutAsync();
				return RedirectToPage("Login");
			}
			var teachers = this.teacherRepository.GetAll().Where(s => s.DepartmentId == department.Id);

			return Json(teachers);
		}

		[HttpPost]
		[Route("~/api/Teacher/Add")]
		public async Task<IActionResult> CreateTeacher([FromBody]TeacherVM teacherVM)
		{
			var userId = userManager.GetUserId(User);
			var department = this.departmentRepository.GetAll().Where(d => d.AccountId == userId).FirstOrDefault();
			if (department == null)
			{
				await signInManager.SignOutAsync();
				return RedirectToPage("Login");
			}

			var accountId = await this.userRepository.CreateUserRole(teacherVM.Code, teacherVM.Code, "Teacher");

			if (accountId != null)
			{
				var teacher = new Teacher();
				teacher.AccountId = accountId;
				teacher.Code = teacherVM.Code;
				teacher.Name = teacherVM.Name;
				teacher.Information = teacherVM.Information;
				teacher.DepartmentId = department.Id;
				teacher.Department = department;

				this.teacherRepository.Insert(teacher);
				await this.teacherRepository.SaveChangeAsync();
				return Ok();
			}
			return BadRequest();
		}


		[HttpPut]
		[Route("~/api/Teacher/Update/{id}")]
		public async Task<IActionResult> UpdateTeacher(long id, [FromBody] TeacherVM teacherVM)
		{
			var teacher = await this.teacherRepository.GetAsync(id);
			if (teacher == null)
				return NotFound();

			teacher.Code = teacherVM.Code;
			teacher.Name = teacherVM.Name;
			teacher.Information = teacherVM.Information;

			this.teacherRepository.Update(teacher);
			await this.teacherRepository.SaveChangeAsync();
			return NoContent();
		}


		[HttpDelete]
		[Route("~/api/Teacher/Delete/{id}")]
		public async Task<IActionResult> DeleteTeacher(long id)
		{
			var teacher = await this.teacherRepository.GetAsync(id);
			if (teacher == null)
				return NotFound();
			await this.userRepository.DeleteUser(teacher.AccountId);
			this.teacherRepository.Delete(teacher);
			await this.teacherRepository.SaveChangeAsync();

			return NoContent();
		}
	}
}