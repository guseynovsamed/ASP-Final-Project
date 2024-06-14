using System;
using ASPProject.Data;
using ASPProject.Helpers.Extensions;
using ASPProject.Models;
using ASPProject.ViewModels.Sliders;
using ASPProject.ViewModels.Sliders.SliderInfos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPProject.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context,
                                IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.ToListAsync();
            List<SliderVM> result = sliders.Select(m => new SliderVM { Id = m.Id, Image = m.Image, Title = m.Title }).ToList();
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM request)
        {
            if (!ModelState.IsValid) return View();

            if (!request.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "File must be only image format");
                return View();
            };

            if (!request.Image.CheckFileSize(500))
            {
                ModelState.AddModelError("Image", "Image size must be max 500kb");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.Image.FileName;

            string path = Path.Combine(_env.WebRootPath, "assets/img", fileName);

            await request.Image.SaveFileToLocalAsync(path);

            Slider slider = new Slider
            {
                Title = request.Title,
                Image = fileName
            };

            await _context.Sliders.AddAsync(slider);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);
            if (slider is null) return NotFound();

            SliderDetailVM sliderDetail = new()
            {
                Image = slider.Image,
                Title = slider.Title
            };

            return View(sliderDetail);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();
            if (id is null) return BadRequest();
            var slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);
            if (slider is null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, "assets/img", slider.Image);
            path.DeleteFileFromLocal();
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);
            if (slider is null) return NotFound();

            SliderEditVM sliderEditVM = new()
            {
                Image = slider.Image,
                Title = slider.Title,
            };

            return View(sliderEditVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderEditVM request)
        {
            if (id is null) return BadRequest();
            var slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);
            if (slider is null) return NotFound();

            if (request.NewImage is not null)
            {
                if (!request.NewImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "File must be only image format");
                    request.Image = slider.Image;
                    return View(request);
                };

                if (!request.NewImage.CheckFileSize(500))
                {
                    ModelState.AddModelError("NewImage", "Image size must be max 500kb");
                    request.Image = slider.Image;
                    return View(request);
                }

                string oldPath = Path.Combine(_env.WebRootPath, "assets/img", slider.Image);

                oldPath.DeleteFileFromLocal();

                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;

                string newPath = Path.Combine(_env.WebRootPath, "assets/img", fileName);

                await request.NewImage.SaveFileToLocalAsync(newPath);

                slider.Image = fileName;
            }

            slider.Title = request.Title;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

