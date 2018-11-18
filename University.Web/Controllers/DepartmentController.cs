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
	public class DepartmentController : Controller
	{
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;
		private readonly DepartmentRepository departmentRepository;
		private readonly IRepository<Student> studentRepository;
		private readonly IRepository<Teacher> teacherRepository;
		private readonly IRepository<Course> courseRepository;
		private readonly IRepository<Prerequisite> prerequisiteRepository;
		private readonly IRepository<TimeEnrollment> timeEnrollmentRepository;
		private readonly IRepository<Classroom> classroomRepository;
		private readonly UserRepository userRepository;

		public DepartmentController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, DepartmentRepository departmentRepository,
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
			this.teacherRepository = teacherRepository;
			this.userRepository = userRepository;
			this.courseRepository = courseRepository;
			this.prerequisiteRepository = prerequisiteRepository;
			this.timeEnrollmentRepository = timeEnrollmentRepository;
			this.classroomRepository = classroomRepository;
		}

		[HttpGet]
		[Route("~/Department/Index")]
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		[Route("~/api/Department/")]
		public async Task<IActionResult> ListDepartment()
		{
			var departments = await this.departmentRepository.GetAll().ToListAsync();
			return Json(departments);
		}

		[HttpPost]
		[Route("~/api/Department/Add")]
		public async Task<IActionResult> Create([FromBody]Department department)
		{
			var userId = await this.userRepository.CreateUserRole(department.DeptEgName, department.DeptEgName, "DepartmentAdmin");
			if (userId != null)
			{
				var newDepartment = new Department()
				{
					Deptname = department.Deptname,
					DeptEgName = department.DeptEgName,
					Address = department.Address,
					Website = department.Website,
					AccountId = userId
				};
				this.departmentRepository.Insert(newDepartment);
				await this.departmentRepository.SaveChangeAsync();
				return Ok();
			}
			return BadRequest();
		}

	}
}
