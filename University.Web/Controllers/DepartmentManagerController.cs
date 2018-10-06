using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace University.Web.Controllers
{
    public class DepartmentManagerController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}