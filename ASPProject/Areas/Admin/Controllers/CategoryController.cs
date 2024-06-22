using System;
using System.Xml.Linq;
using ASPProject.Data;
using ASPProject.Helpers.Enum;
using ASPProject.Models;
using ASPProject.ViewModels.Categories;
using ASPProject.ViewModels.SelectedProducts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPProject.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories.ToListAsync();
            List<CategoryVM> categoryVM = _context.Categories.Select(m => new CategoryVM { Id = m.Id, Name = m.Name }).ToList();
            return View(categoryVM);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(CategoryCreateVM request)
        {
            if (!ModelState.IsValid) return View();

            bool existCategory = await _context.Categories.AnyAsync(m => m.Name.Trim() == request.Name.Trim());
            if (existCategory) { ModelState.AddModelError("Name", "This category already exist"); return View(); }

            Category category = new()
            {
                Name = request.Name
            };

            await _context.Categories.AddAsync(category);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Category category = await _context.Categories.Where(m => m.Id == id).Include(m => m.Products).FirstOrDefaultAsync();
            if (category is null) return NotFound();

            CategoryDetailVM model = new CategoryDetailVM()
            {
                Name = category.Name,
                ProductCount = category.Products.Count()
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            Category category = await _context.Categories.Where(m => m.Id == id).Include(m => m.Products).FirstOrDefaultAsync();
            if (category is null) return NotFound();
            _context.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            Category category = await _context.Categories.Where(m => m.Id == id).Include(m => m.Products).FirstOrDefaultAsync();
            if (category is null) return NotFound();

            CategoryEditVM categoryEditVM = new()
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(categoryEditVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int? id, CategoryEditVM request)
        {
            if (!ModelState.IsValid) return View();
            if (id is null) return BadRequest();
            Category category = await _context.Categories.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (category is null) return NotFound();

            bool existCategory = await _context.Categories.AnyAsync(m => m.Name.Trim() == request.Name.Trim());
            if (existCategory) { ModelState.AddModelError("Name", "This category already exist"); return View(); }

            category.Name = request.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

