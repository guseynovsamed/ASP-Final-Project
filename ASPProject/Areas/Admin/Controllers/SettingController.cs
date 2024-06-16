using System;
using ASPProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace ASPProject.Areas.Admin.Controllers
{
	public class SettingController : Controller
	{
		private readonly AppDbContext _context;

		public SettingController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View();
		}

	}
}

