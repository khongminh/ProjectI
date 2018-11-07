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
		private readonly IRepository<Course> courseRepository;
		private readonly IRepository<Prerequisite> prerequisiteRepository;
		private readonly UserRepository userRepository;

		public DepartmentManagerController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, DepartmentRepository departmentRepository,
		  IRepository<Student> studentRepository, IRepository<Teacher> teacherRepository,
		  IRepository<Course> courseRepository, IRepository<Prerequisite> prerequisiteRepository,
		  UserRepository userRepository)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.departmentRepository = departmentRepository;
			this.studentRepository = studentRepository;
			this.teacherRepository = teacherRepository;
			this.userRepository = userRepository;
			this.courseRepository = courseRepository;
			this.prerequisiteRepository = prerequisiteRepository;
		}

		#region Student
		[Route("~/Department/IndexStudent")]
		public IActionResult IndexStudent()
		{
			return View();
		}

		[Route("~/api/GetStudents")]
		[HttpGet]
		public async Task<IActionResult> ListStudent()
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
		[Route("~/api/AddStudent")]
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
		#endregion


		#region Teacher
		[Route("~/Department/IndexTeacher")]
		public IActionResult IndexTeacher()
		{
			return View();
		}

		[Route("~/api/GetTeachers")]
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
		[Route("~/api/AddTeacher")]
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
		[Route("~/api/UpdateTeacher/{id}")]
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
		[Route("~/api/DeleteTeacher/{id}")]
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
		#endregion


		#region Course
		[Route("~/Department/IndexCourse")]
		public IActionResult IndexCourse()
		{
			return View();
		}

		[Route("~/api/GetCourses")]
		[HttpGet]
		public async Task<IActionResult> ListCourse()
		{
			var userId = userManager.GetUserId(User);
			var department = this.departmentRepository.GetAll().Where(d => d.AccountId == userId).FirstOrDefault();
			if (department == null)
			{
				await signInManager.SignOutAsync();
				return RedirectToPage("Login");
			}
			var courses = this.courseRepository.GetAll().Where(s => s.DepartmentId == department.Id);

			var coursesVM = new List<Course>();
			foreach (var course in courses)
			{
				var courseVM = new CourseVM();
				courseVM.Code = course.Code;
				courseVM.Description = course.Description;
				courseVM.Credits = course.Credits;
				courseVM.Title = course.Title;

				courseVM.Prerequisite = this.prerequisiteRepository.GetAll().Where(p => p.CourseId == course.Id);
			}

			return Json(coursesVM);
		}

		[HttpPost]
		[Route("~/api/AddCourse")]
		public async Task<IActionResult> CreateCourse([FromBody]CourseVM courseVM)
		{
			var userId = userManager.GetUserId(User);
			var department = this.departmentRepository.GetAll().Where(d => d.AccountId == userId).FirstOrDefault();
			if (department == null)
			{
				await signInManager.SignOutAsync();
				return RedirectToPage("Login");
			}

			try
			{
				var course = new Course();

				course.Code = courseVM.Code;
				course.Title = courseVM.Title;
				course.Description = courseVM.Description;
				course.Credits = courseVM.Credits;
				course.DepartmentId = department.Id;
				course.Department = department;

				this.courseRepository.Insert(course);
				
				foreach (var pre in courseVM.Prerequisite)
				{
					this.prerequisiteRepository.Insert(pre);
				}
				await this.courseRepository.SaveChangeAsync();

				return Ok();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}


		[HttpPut]
		[Route("~/api/UpdateCourse/{id}")]
		public async Task<IActionResult> UpdateCourse(long id, [FromBody] CourseVM courseVM)
		{
			var course = await this.courseRepository.GetAsync(id);
			if (course == null)
				return NotFound();

			course.Code = courseVM.Code;
			course.Title = courseVM.Title;
			course.Description = courseVM.Description;
			course.Credits = courseVM.Credits;

			this.courseRepository.Update(course);

			foreach (var pre in this.prerequisiteRepository.GetAll().Where(p => p.Id == course.Id))
			{
				this.prerequisiteRepository.Delete(pre);
			}

			foreach (var pre in courseVM.Prerequisite)
			{
				this.prerequisiteRepository.Insert(pre);
			}

			await this.courseRepository.SaveChangeAsync();
			return NoContent();
		}


		[HttpDelete]
		[Route("~/api/DeleteCourse/{id}")]
		public async Task<IActionResult> DeleteCourse(long id)
		{
			var course = await this.courseRepository.GetAsync(id);
			if (course == null)
				return NotFound();
			this.courseRepository.Delete(course);
			foreach (var pre in this.prerequisiteRepository.GetAll().Where(p => p.Id == course.Id))
			{
				this.prerequisiteRepository.Delete(pre);
			}

			await this.courseRepository.SaveChangeAsync();
			return NoContent();
		}
		#endregion

	}
}
