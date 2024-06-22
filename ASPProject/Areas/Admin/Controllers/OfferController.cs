using System;
using ASPProject.Data;
using ASPProject.Helpers.Enum;
using ASPProject.Helpers.Extensions;
using ASPProject.Models;
using ASPProject.ViewModels.Offers;
using ASPProject.ViewModels.Sliders.SliderInfos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPProject.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class OfferController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public OfferController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Offer> offers = await _context.Offers.ToListAsync();
            List<OfferVM> result = await _context.Offers.Select(m => new OfferVM { Title = m.Title, Id = m.Id, Description = m.Description, Image = m.Image }).ToListAsync();
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
        public async Task<IActionResult> Create(OfferCreateVM request)
        {
            if (!ModelState.IsValid) return View();

            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("NewImage", "File must be only image format");
                return View();
            };

            if (!request.NewImage.CheckFileSize(200))
            {
                ModelState.AddModelError("NewImage", "Image size must be max 200kb");
                return View();
            }

            string filename = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;

            string path = Path.Combine(_env.WebRootPath, "assets/img", filename);

            await request.NewImage.SaveFileToLocalAsync(path);

            Offer offer = new()
            {
                Title = request.Title,
                Description = request.Description,
                Image = filename
            };

            await _context.Offers.AddAsync(offer);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            var offer = await _context.Offers.FirstOrDefaultAsync(m => m.Id == id);
            if (offer is null) return NotFound();

            OfferDetailVM offerDetailVM = new()
            {
                Title = offer.Title,
                Description = offer.Description,
                Image = offer.Image
            };

            return View(offerDetailVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var offer = await _context.Offers.FirstOrDefaultAsync(m => m.Id == id);
            if (offer is null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, "assets/img", offer.Image);
            path.DeleteFileFromLocal();
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var offer = await _context.Offers.FirstOrDefaultAsync(m => m.Id == id);
            if (offer is null) return NotFound();

            OfferEditVM offerEditVM = new()
            {
                Title = offer.Title,
                Description = offer.Description,
                Image = offer.Image
            };

            return View(offerEditVM);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int? id, OfferEditVM request)
        {
            if (id is null) return BadRequest();
            var offer = await _context.Offers.FirstOrDefaultAsync(m => m.Id == id);
            if (offer is null) return NotFound();


            if (request.NewImage is not null)
            {

                if (!request.NewImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "File must be only image format");
                    request.Image = offer.Image;
                    return View(request);
                };

                if (!request.NewImage.CheckFileSize(500))
                {
                    ModelState.AddModelError("NewImage", "Image size must be max 500kb");
                    request.Image = offer.Image;
                    return View(request);
                }

                string oldPath = Path.Combine(_env.WebRootPath, "assets/img", offer.Image);

                oldPath.DeleteFileFromLocal();

                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;

                string newPath = Path.Combine(_env.WebRootPath, "assets/img", fileName);

                await request.NewImage.SaveFileToLocalAsync(newPath);

                offer.Image = fileName;
            }

            offer.Title = request.Title;
            offer.Description = request.Description;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

