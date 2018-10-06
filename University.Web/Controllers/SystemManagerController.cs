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

		public SystemManagerController(DepartmentRepository departmentRepository,
									   UserRepository userRepository)
		{
			this.departmentRepository = departmentRepository;
			this.userRepository = userRepository;
		}

        public async Task<IActionResult> Index()
        {
			var departments = await this.departmentRepository.GetAllAsync();
            return View(departments);
        }

		public async Task<IActionResult> Detail(long id)
		{
			var department = await this.departmentRepository.GetDetail(id);
			if(department == null)
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
    }
}