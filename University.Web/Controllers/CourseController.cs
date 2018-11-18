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
	public class CourseController : Controller
    {
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;
		private readonly DepartmentRepository departmentRepository;
		private readonly IRepository<Course> courseRepository;
		private readonly IRepository<Prerequisite> prerequisiteRepository;
		private readonly IRepository<TimeEnrollment> timeEnrollmentRepository;
		private readonly IRepository<Classroom> classroomRepository;
		private readonly UserRepository userRepository;

		public CourseController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, DepartmentRepository departmentRepository,
		  IRepository<Student> studentRepository, IRepository<Teacher> teacherRepository,
		  IRepository<Course> courseRepository, IRepository<Prerequisite> prerequisiteRepository,
		  IRepository<TimeEnrollment> timeEnrollmentRepository,
		  IRepository<Classroom> classroomRepository,
		  UserRepository userRepository)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.departmentRepository = departmentRepository;
			this.userRepository = userRepository;
			this.courseRepository = courseRepository;
			this.prerequisiteRepository = prerequisiteRepository;
			this.timeEnrollmentRepository = timeEnrollmentRepository;
			this.classroomRepository = classroomRepository;
		}

		#region CRUD
		[Route("~/Course/Index")]
		public IActionResult IndexCourse()
		{
			return View();
		}

		[Route("~/api/Course")]
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
			var courses = await this.courseRepository.GetAll().Include(c => c.Prerequisites).ToListAsync();

			return Json(courses);
		}

		[HttpPost]
		[Route("~/api/Course/UpdatePrerequisite")]
		public async Task<IActionResult> PrerequisiteCourse([FromBody]PrerequisiteVM prerequisiteVM)
		{
			var course = await this.courseRepository.GetAll().Include(c => c.Prerequisites).Where(c => c.Id == prerequisiteVM.Id).FirstOrDefaultAsync();

			if (course == null)
				return NotFound();
			var currentPreque = course.Prerequisites;

			foreach (var pre in currentPreque)
			{
				if (!prerequisiteVM.Prerequisites.Contains<Prerequisite>(pre))
				{
					this.prerequisiteRepository.Delete(pre);
				}
			}

			foreach (var pre in prerequisiteVM.Prerequisites)
			{
				if (!currentPreque.Contains(pre))
				{
					this.prerequisiteRepository.Insert(pre);
				}
			}

			await this.courseRepository.SaveChangeAsync();
			return NoContent();
		}

		[HttpPost]
		[Route("~/api/Course/Add")]
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

				await this.courseRepository.SaveChangeAsync();

				return Ok();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}


		[HttpPut]
		[Route("~/api/Course/Update/{id}")]
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

			await this.courseRepository.SaveChangeAsync();
			return NoContent();
		}


		[HttpDelete]
		[Route("~/api/Course/Delete/{id}")]
		public async Task<IActionResult> DeleteCourse(long id)
		{
			var course = await this.courseRepository.GetAsync(id);
			if (course == null)
				return NotFound();
			this.courseRepository.Delete(course);

			await this.courseRepository.SaveChangeAsync();
			return NoContent();
		}
		#endregion

	}
}