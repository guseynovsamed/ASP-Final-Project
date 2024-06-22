using System;
using ASPProject.Helpers.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPProject.Areas.Admin.Controllers
{
	[Area(nameof(Admin))]
    [Authorize(Roles = "SuperAdmin , Admin")]
    public class DashboardController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
    }
}

