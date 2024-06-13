using System;
using ASPProject.Data;
using ASPProject.Models;
using ASPProject.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPProject.Areas.Admin.Controllers
{
	[Area(nameof(Admin))]
	public class SliderController : Controller
	{
		private readonly AppDbContext _context;

		public SliderController(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			List<Slider> sliders = await _context.Sliders.ToListAsync();
			List<SliderVM> result = sliders.Select(m => new SliderVM { Id = m.Id, Image = m.Image, Title = m.Title }).ToList();
			return View();
		}
	}
}

