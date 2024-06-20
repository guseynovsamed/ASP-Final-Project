using System;
using ASPProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPProject.Controllers
{
	public class ProductController : Controller
	{
		private readonly AppDbContext _context;

		public ProductController(AppDbContext context)
		{
			_context = context;
		}


		public async Task<IActionResult> Index(int? id)
		{
			if (id is null) return BadRequest();

			var product = await _context.Products.Where(m => !m.SoftDeleted)
												 .Include(m => m.Category)
												 .Include(m => m.ProductImage)
												 .FirstOrDefaultAsync(m => m.Id == id);
			if (product is null) return NotFound();

			return View(product);
		}
	}
}

