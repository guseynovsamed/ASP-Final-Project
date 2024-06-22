using System;
using ASPProject.Data;
using ASPProject.Helpers.Enum;
using ASPProject.Models;
using ASPProject.ViewModels.Facts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPProject.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
	public class FactController : Controller
	{
        private readonly AppDbContext _context;

        public FactController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fact = await _context.Facts.OrderByDescending(m => m.Id).ToListAsync();
            var result = fact.Select(m => new FactVM { Id = m.Id, Title = m.Title, Description = m.Description, Icon = m.Icon }).ToList();
            return View(result);
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
        public async Task<IActionResult> Create(FactCreateVM request)
        {
            if (!ModelState.IsValid) return View();

            Fact fact = new()
            {
                Title = request.Title,
                Description = request.Description,
                Icon = request.Icon
            };

            await _context.Facts.AddAsync(fact);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            var fact = await _context.Facts.FirstOrDefaultAsync(m => m.Id == id);
            if (fact is null) return NotFound();

            FactDetailVM factDetail = new()
            {
                Title = fact.Title,
                Description = fact.Description,
                Icon = fact.Icon
            };

            return View(factDetail);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();
            if (id is null) return BadRequest();
            var fact = await _context.Facts.FirstOrDefaultAsync(m => m.Id == id);
            if (fact is null) return NotFound();
            _context.Facts.Remove(fact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var fact = await _context.Facts.FirstOrDefaultAsync(m => m.Id == id);
            if (fact is null) return NotFound();

            FactEditVM factEdit = new()
            {
                Title = fact.Title,
                Description = fact.Description,
                Icon = fact.Icon
            };

            return View(factEdit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int? id , FactEditVM factEdit)
        {
            if (!ModelState.IsValid) return View();
            if (id is null) return BadRequest();
            var fact = await _context.Facts.FirstOrDefaultAsync(m => m.Id == id);
            if (fact is null) return NotFound();

            fact.Title = factEdit.Title;
            fact.Description = factEdit.Description;
            fact.Icon = factEdit.Icon;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

