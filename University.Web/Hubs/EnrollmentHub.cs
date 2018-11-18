using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Data.Repository;
using University.Entity;

namespace University.Web.Hubs
{
	public class EnrollmentHub : Hub
	{
		private readonly IRepository<Enrollment> enrollmentRepository;
		private readonly IRepository<Classroom> classroomRepository;
		private readonly IRepository<Student> studentRepository;

		public EnrollmentHub(IRepository<Enrollment> enrollmentRepository, IRepository<Classroom> classroomRepository,
			IRepository<Student> studentRepository)
		{
			this.enrollmentRepository = enrollmentRepository;
			this.classroomRepository = classroomRepository;
			this.studentRepository = studentRepository;
		}

		public async Task Enrollment(IEnumerable<Enrollment> enrollments)
		{
			var classIds = new List<long>();
			foreach (var enrollment in enrollments)
			{
				var classroom = await this.classroomRepository.GetAll().Include(c => c.Enrollments).Where(c => c.Id == enrollment.ClassroomId).FirstOrDefaultAsync();
				if(classroom.Enrollments.Count() < classroom.MaxStudent)
				{
					this.enrollmentRepository.Insert(enrollment);
					classIds.Add(enrollment.ClassroomId);
				}
			}
			await this.enrollmentRepository.SaveChangeAsync();

			await Clients.All.SendAsync("Increase", classIds);
		}
	}
}
