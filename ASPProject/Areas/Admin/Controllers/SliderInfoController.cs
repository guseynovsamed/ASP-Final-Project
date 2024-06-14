using System;
using ASPProject.Data;
using ASPProject.Helpers.Extensions;
using ASPProject.Models;
using ASPProject.ViewModels.Sliders.SliderInfos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace ASPProject.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class SliderInfoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderInfoController(AppDbContext context,
                                    IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<SliderInfo> sliderInfos = await _context.SliderInfos.ToListAsync();
            List<SliderInfoVM> result = sliderInfos.Select(m => new SliderInfoVM { Title = m.Title, Id = m.Id, Description = m.Description, BackgroundImage = m.BackgroundImage }).ToList();

            return View(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(SliderInfoCreateVM request)
        {
            if (!ModelState.IsValid) return View();

            if (!request.NewBackgroundImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "File must be only image format");
                return View();
            };

            if (!request.NewBackgroundImage.CheckFileSize(200))
            {
                ModelState.AddModelError("NewBackgroundImage", "Image size must be max 200kb");
                return View();
            }


            string fileName = Guid.NewGuid().ToString() + "-" + request.NewBackgroundImage.FileName;

            string path = Path.Combine(_env.WebRootPath, "assets/img", fileName);

            await request.NewBackgroundImage.SaveFileToLocalAsync(path);

            SliderInfo sliderInfo = new SliderInfo
            {
                Title = request.Title,
                Description = request.Description,
                BackgroundImage = fileName
            };


            await _context.SliderInfos.AddAsync(sliderInfo);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync(m => m.Id == id);
            if (sliderInfo is null) return NotFound();

            SliderInfoEditVM sliderInfoEdit = new SliderInfoEditVM
            {
                Title = sliderInfo.Title,
                Description = sliderInfo.Description,
                Image = sliderInfo.BackgroundImage
            };

            return View(sliderInfoEdit);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderInfoEditVM request)
        {
            if (id is null) return BadRequest();
            var sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync(m => m.Id == id);
            if (sliderInfo is null) return NotFound();


            if (request.NewBackgroundImage is not null)
            {

                if (!request.NewBackgroundImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "File must be only image format");
                    request.Image = sliderInfo.BackgroundImage;
                    return View(request);
                };

                if (!request.NewBackgroundImage.CheckFileSize(500))
                {
                    ModelState.AddModelError("NewImage", "Image size must be max 500kb");
                    request.Image = sliderInfo.BackgroundImage;
                    return View(request);
                }

                string oldPath = Path.Combine(_env.WebRootPath, "assets/img", sliderInfo.BackgroundImage);

                oldPath.DeleteFileFromLocal();

                string fileName = Guid.NewGuid().ToString() + "-" + request.NewBackgroundImage.FileName;

                string newPath = Path.Combine(_env.WebRootPath, "assets/img", fileName);

                await request.NewBackgroundImage.SaveFileToLocalAsync(newPath);

                sliderInfo.BackgroundImage = fileName;
            }

            sliderInfo.Title = request.Title;
            sliderInfo.Description = request.Description;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();
            if (id is null) return BadRequest();
            var sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync(m => m.Id == id);
            if (sliderInfo is null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, "assets/img", sliderInfo.BackgroundImage);
            path.DeleteFileFromLocal();
            _context.SliderInfos.Remove(sliderInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            SliderInfo sliderInfo = await _context.SliderInfos.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (sliderInfo is null) return NotFound();
            SliderInfoDetailVM model = new()
            {
                Title = sliderInfo.Title,
                Description = sliderInfo.Description,
                BackgroundImage = sliderInfo.BackgroundImage
            };
            return View(model);
        }
    }
}

