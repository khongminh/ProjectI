using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.Data.Repository;
using University.Entity;
using University.Web.Models;

namespace University.Web.Controllers
{
    public class ClassroomController : Controller
    {
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;
		private readonly IRepository<Course> courseRepository;
		private readonly IRepository<Prerequisite> prerequisiteRepository;
		private readonly IRepository<TimeEnrollment> timeEnrollmentRepository;
		private readonly IRepository<Classroom> classroomRepository;
		private readonly UserRepository userRepository;

		public ClassroomController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, DepartmentRepository departmentRepository,
		  IRepository<Student> studentRepository, IRepository<Teacher> teacherRepository,
		  IRepository<Course> courseRepository, IRepository<Prerequisite> prerequisiteRepository,
		  IRepository<TimeEnrollment> timeEnrollmentRepository,
		  IRepository<Classroom> classroomRepository,
		  UserRepository userRepository)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.userRepository = userRepository;
			this.courseRepository = courseRepository;
			this.prerequisiteRepository = prerequisiteRepository;
			this.timeEnrollmentRepository = timeEnrollmentRepository;
			this.classroomRepository = classroomRepository;
		}

		[Route("~/Semester/Index")]
		public async Task<IActionResult> IndexSemester()
		{
			var semesters = await this.timeEnrollmentRepository.GetAll().OrderByDescending(t => t.Semester).ToListAsync();

			return View(semesters);
		}

		[Route("~/OpenClass/{semesterId}")]
		public async Task<IActionResult> IndexOpenCourse(long semesterId)
		{
			ViewData["semesterId"] = semesterId;
			var semester = await this.timeEnrollmentRepository.GetAsync(semesterId);
			if (semester == null)
				return NotFound();
			ViewData["semester"] = semester.Semester;
			return View();
		}

		[Route("~/api/CourseWithClass/{semesterId}")]
		public async Task<IActionResult> CourseWithClass(long semesterId)
		{
			var semester = await this.timeEnrollmentRepository.GetAsync(semesterId);
			if (semester == null)
				return NotFound();

			if (DateTime.Now <= semester.EndTime)
				ViewData["newSemester"] = true;
			else
				ViewData["newSemester"] = false;

			var courses = await this.courseRepository.GetAll().Include(c => c.Classrooms).ThenInclude(cl => cl.TimeSlots).Include(c => c.Classrooms).ThenInclude(cl => cl.Teacher).ToListAsync();

			var resultCourses = courses.Select(c => new Course()
				{
					Classrooms = c.Classrooms.Where(cl => cl.Semester == semester.Semester),
					Code = c.Code,
					Credits = c.Credits,
					Description = c.Description,
					Id = c.Id,
					Title = c.Title
				});

			return Json(resultCourses);
		}

		[Route("~/api/Classroom/{classId}")]
		public IActionResult ClassDetail(long classId)
		{
			var classroom = this.classroomRepository.GetAll().Include(c => c.Teacher).Include(c => c.TimeSlots).FirstOrDefaultAsync(c => c.Id == classId);
			return Json(classroom);
		}

		[HttpPost]
		[Route("~/api/Classroom/Create/")]
		public async Task<IActionResult> CreateClass([FromBody]ClassroomVM classroomVM)
		{
			try
			{
				var semesters = await this.timeEnrollmentRepository.GetAll().OrderByDescending(t => t.Semester).FirstOrDefaultAsync();

				var newClass = new Classroom();
				newClass.Code = classroomVM.Code;
				newClass.Semester = semesters.Semester;
				newClass.MaxStudent = classroomVM.MaxStudent;
				newClass.CourseId = classroomVM.CourseId;
				newClass.TeacherId = classroomVM.TeacherId;
				newClass.TimeSlots = new List<TimeSlot>();
				foreach (var timeslot in classroomVM.TimeSlots)
				{
					newClass.TimeSlots.Add(new TimeSlot { Day = timeslot.Day, StartTime = timeslot.StartTime, EndTime = timeslot.EndTime, Room = timeslot.Room });
				}

				this.classroomRepository.Insert(newClass);
				await this.classroomRepository.SaveChangeAsync();

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest();
			}
		}
	}
}