using System;
using Microsoft.AspNetCore.Mvc;

namespace ASPProject.Areas.Admin.Controllers
{
	[Area(nameof(Admin))]
	public class DashboardController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
    }
}

