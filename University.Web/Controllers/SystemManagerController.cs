using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using University.Data.Repository;
using University.Entity;
using University.Web.Models;

namespace University.Web.Controllers
{
    public class SystemManagerController : Controller
    {
		private readonly DepartmentRepository departmentRepository;
		private readonly UserRepository userRepository;
		private readonly IRepository<TimeEnrollment> timeEnrollmentRepository;

		public SystemManagerController(DepartmentRepository departmentRepository,
									   UserRepository userRepository,
									   IRepository<TimeEnrollment> timeEnrollmentRepository)
		{
			this.departmentRepository = departmentRepository;
			this.userRepository = userRepository;
			this.timeEnrollmentRepository = timeEnrollmentRepository;
		}

		[HttpGet]
        public async Task<IActionResult> Index()
        {
			var departments = await this.departmentRepository.GetAllAsync();
            return View(departments);
        }

		[HttpGet]
		public async Task<IActionResult> DeptStudent(long id)
		{
			var department = await this.departmentRepository.GetStudents(id);
			if(department == null)
			{
				return NotFound();
			}
			return View(department);
		}

		[HttpGet]
		public async Task<IActionResult> DeptCourse(long id)
		{
			var department = await this.departmentRepository.GetCourses(id);
			if (department == null)
			{
				return NotFound();
			}
			return View(department);
		}

		[HttpGet]
		public async Task<IActionResult> DeptTeacher(long id)
		{
			var department = await this.departmentRepository.GetTeachers(id);
			if (department == null)
			{
				return NotFound();
			}
			return View(department);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateDepartmentVM departmentVM)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var userId = await this.userRepository.CreateUserRole(departmentVM.AdminAccount, departmentVM.Password, "DepartmentAdmin");
			if (userId != null)
			{
				var newDepartment = new Department()
				{
					Deptname = departmentVM.DeptName,
					Address = departmentVM.Address,
					Website = departmentVM.Website,
					AccountId = userId
				};
				this.departmentRepository.Insert(newDepartment);
				await this.departmentRepository.SaveChangeAsync();
				return RedirectToAction("Index");
			}
			ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi, vui lòng thử lại");
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> OpenEnrollmentCourse()
		{
			var pasts = await this.timeEnrollmentRepository.GetAllAsync();
			return View(pasts.OrderByDescending(p => p.Semester));
		}

		[HttpPost]
		public async Task<IActionResult> OpenEnrollmentCourse(TimeEnrollment timeEnrollment)
		{
			var semester = timeEnrollment.Semester % 10;
			if (semester != 1 && semester != 2 && semester != 3)
				return RedirectToAction("OpenEnrollmentCourse");

			var pasts = await this.timeEnrollmentRepository.GetAllAsync();

			var lasted = pasts.OrderByDescending(p => p.Semester).FirstOrDefault();

			if (lasted != null)
			{
				if (timeEnrollment.Semester == lasted.Semester)
				{
					if (pasts.Count() == 1 || CheckDateTime(pasts.OrderByDescending(p => p.Semester).ToArray()[1], timeEnrollment))
					{
						lasted.StartTime = timeEnrollment.StartTime;
						lasted.EndTime = timeEnrollment.EndTime;
						this.timeEnrollmentRepository.Update(lasted);
						await this.timeEnrollmentRepository.SaveChangeAsync();
					}
				}
				else
				{
					if (timeEnrollment.Semester > lasted.Semester && CheckDateTime(lasted, timeEnrollment))
					{
						this.timeEnrollmentRepository.Insert(timeEnrollment);
						await this.timeEnrollmentRepository.SaveChangeAsync();
					}
				}
			}
			return RedirectToAction("OpenEnrollmentCourse");
		}

		[NonAction]
		private bool CheckDateTime(TimeEnrollment lastTime, TimeEnrollment newTime)
		{
			if (DateTime.Now < lastTime.EndTime && lastTime.EndTime < newTime.StartTime && newTime.StartTime < newTime.EndTime)
				return true;
			return false;
		}

		
	}
}