using System;
using ASPProject.Data;
using ASPProject.Models;
using ASPProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPProject.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _context;

		public HomeController(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			List<SliderInfo> sliderInfos = await _context.SliderInfos.ToListAsync();

			HomeVM model = new()
			{
				SliderInfos = sliderInfos
			};

			return View(model);
		}

	}
}

