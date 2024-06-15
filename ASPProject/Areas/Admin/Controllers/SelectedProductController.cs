using System;
using ASPProject.Data;
using ASPProject.Helpers.Extensions;
using ASPProject.Models;
using ASPProject.ViewModels.SelectedProducts;
using ASPProject.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPProject.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class SelectedProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public SelectedProductController(AppDbContext context,
                                         IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<SelectedProduct> selectedProducts = await _context.SelectedProducts.ToListAsync();
            List<SelectedProductVM> result = selectedProducts.Select(m => new SelectedProductVM { Description = m.Description, FirstTitle = m.FirstTitle, SecondTitle = m.SecondTitle, Image = m.Image, Id = m.Id }).ToList();
            return View(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SelectedProductCreateVM request)
        {
            if (!ModelState.IsValid) return View();

            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "File must be only image format");
                return View();
            };

            if (!request.NewImage.CheckFileSize(500))
            {
                ModelState.AddModelError("Image", "Image size must be max 500kb");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;

            string path = Path.Combine(_env.WebRootPath, "assets/img", fileName);

            await request.NewImage.SaveFileToLocalAsync(path);

            SelectedProduct selectedProduct = new()
            {
                FirstTitle = request.FirstTitle,
                SecondTitle = request.SecondTitle,
                Description = request.Description,
                Image = fileName
            };

            await _context.SelectedProducts.AddAsync(selectedProduct);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            var result = await _context.SelectedProducts.FirstOrDefaultAsync(m => m.Id == id);
            if (result is null) return NotFound();

            SelectedProductDetailVM selectedProductDetailVM = new()
            {
                FirstTitle = result.FirstTitle,
                SecondTitle = result.SecondTitle,
                Description = result.Description,
                Image = result.Image
            };

            return View(selectedProductDetailVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var result = await _context.SelectedProducts.FirstOrDefaultAsync(m => m.Id == id);
            if (result is null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, "assets/img", result.Image);
            path.DeleteFileFromLocal();
            _context.SelectedProducts.Remove(result);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var result = await _context.SelectedProducts.FirstOrDefaultAsync(m => m.Id == id);
            if (result is null) return NotFound();

            SelectedProductEditVM selectedProductEditVM = new()
            {
                FirstTitle = result.FirstTitle,
                SecondTitle = result.SecondTitle,
                Description = result.Description,
                Image = result.Image
            };

            return View(selectedProductEditVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SelectedProductEditVM request)
        {
            if (id is null) return BadRequest();
            var result = await _context.SelectedProducts.FirstOrDefaultAsync(m => m.Id == id);
            if (result is null) return NotFound();

            if (request.NewImage is not null)
            {
                if (!request.NewImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "File must be only image format");
                    request.Image = result.Image;
                    return View(request);
                };

                if (!request.NewImage.CheckFileSize(500))
                {
                    ModelState.AddModelError("NewImage", "Image size must be max 500kb");
                    request.Image = result.Image;
                    return View(request);
                }

                string oldPath = Path.Combine(_env.WebRootPath, "assets/img", result.Image);

                oldPath.DeleteFileFromLocal();

                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;

                string newPath = Path.Combine(_env.WebRootPath, "assets/img", fileName);

                await request.NewImage.SaveFileToLocalAsync(newPath);

                result.Image = fileName;
            }

            result.FirstTitle = request.FirstTitle;
            result.SecondTitle = request.SecondTitle;
            result.Description = request.Description;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}

