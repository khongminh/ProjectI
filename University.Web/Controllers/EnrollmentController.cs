using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.Data.Repository;
using University.Entity;

namespace University.Web.Controllers
{
    public class EnrollmentController : Controller
    {
		private readonly IRepository<Classroom> classroomRepository;
		private readonly IRepository<Student> studentRepository;
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly IRepository<Enrollment> enrollmentRepository;
		private readonly IRepository<TimeEnrollment> timeEnrollmentRepository;

		public EnrollmentController(IRepository<Classroom> classroomRepository, IRepository<Student> studentRepository, IRepository<Enrollment> enrollmentRepository, UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager, IRepository<TimeEnrollment> timeEnrollmentRepository)
		{
			this.classroomRepository = classroomRepository;
			this.studentRepository = studentRepository;
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.enrollmentRepository = enrollmentRepository;
			this.timeEnrollmentRepository = timeEnrollmentRepository;
		}

		[Route("~/Enrollment/Index/")]
		public async Task<IActionResult> Index()
        {
			var userId = userManager.GetUserId(User);
			var student = this.studentRepository.GetAll().Where(d => d.AccountId == userId).FirstOrDefault();
			if (student != null)
			{
				ViewData["studentId"] = student.Id;
			}

			var semesters = this.timeEnrollmentRepository.GetAll();
			bool isTimeEnrollment = false;
			IEnumerable<Classroom> classrooms = null;
			foreach (var semester in semesters)
			{
				if(semester.StartTime <= DateTime.Now && DateTime.Now <= semester.EndTime)
				{
					isTimeEnrollment = true;
					ViewData["semester"] = semester.Semester;
					classrooms = await this.classroomRepository.GetAll().Where(c => c.Semester == semester.Semester).Include(c => c.Course).Include(c => c.Teacher).Include(c => c.TimeSlots).Include(c => c.Enrollments).ToListAsync();
					break;

				}
			}
			if (isTimeEnrollment)
			{
				ViewData["isTimeEnrollment"] = true;
				return View(classrooms);
			}
			else
			{
				ViewData["isTimeEnrollment"] = false;
				return View();
			}

			
			
			
        }

		public async Task<IActionResult> StudentEnrollmentIndex()
		{
			var userId = userManager.GetUserId(User);
			var student = this.studentRepository.GetAll().Where(d => d.AccountId == userId).FirstOrDefault();
			if (student == null)
			{
				return RedirectToAction("Login");
			}
			var classrooms = await this.enrollmentRepository.GetAll().Where(e => e.StudentId == student.Id).Include(e => e.Classroom).ThenInclude(c => c.Course).Include(e => e.Classroom).ThenInclude(c => c.TimeSlots).ToListAsync();

			return View(classrooms);

		}
	}
}