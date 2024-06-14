using System;
using ASPProject.Data;
using ASPProject.Helpers.Extensions;
using ASPProject.Models;
using ASPProject.ViewModels.Featurs;
using ASPProject.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPProject.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class FeaturController : Controller
    {
        private readonly AppDbContext _context;

        public FeaturController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var featur = await _context.Featurs.OrderByDescending(m => m.Id).ToListAsync();
            var result = featur.Select(m => new FeaturVM { Id = m.Id, Title = m.Title, Description = m.Description, Icon = m.Icon }).ToList();
            return View(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeaturCreateVM request)
        {
            if (!ModelState.IsValid) return View();

            Featur featur = new()
            {
                Title = request.Title,
                Description = request.Description,
                Icon = request.Icon
            };

            await _context.Featurs.AddAsync(featur);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            var featur = await _context.Featurs.FirstOrDefaultAsync(m => m.Id == id);
            if (featur is null) return NotFound();

            FeaturDetailVM featurDetail = new()
            {
                Title = featur.Title,
                Description = featur.Description,
                Icon = featur.Icon
            };

            return View(featurDetail);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();
            if (id is null) return BadRequest();
            var featur = await _context.Featurs.FirstOrDefaultAsync(m => m.Id == id);
            if (featur is null) return NotFound();
            _context.Featurs.Remove(featur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var featur = await _context.Featurs.FirstOrDefaultAsync(m => m.Id == id);
            if (featur is null) return NotFound();

            FeaturEditVM featurEditVM = new()
            {
                Title = featur.Title,
                Description = featur.Description,
                Icon = featur.Icon
            };

            return View(featurEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, FeaturEditVM request)
        {
            if (!ModelState.IsValid) return View();
            if (id is null) return BadRequest();
            var featur = await _context.Featurs.FirstOrDefaultAsync(m => m.Id == id);
            if (featur is null) return NotFound();

            featur.Title = request.Title;
            featur.Description = request.Description;
            featur.Icon = request.Icon;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}

