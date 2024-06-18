using ASPProject.Data;
using ASPProject.Helpers;
using ASPProject.Helpers.Extensions;
using ASPProject.Models;
using ASPProject.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASPProject.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context,
                                 IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int take = 4)
        {
            var paginateDatas = await _context.Products.Include(m => m.Category)
                                   .Include(m => m.ProductImage).Skip((page - 1) * take).Take(take).ToListAsync();


            var mapedDatas = paginateDatas.Select(m => new ProductVM
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                SellingCount = m.SellingCount,
                RatingCount = m.RatingCount,
                Category = m.Category.Name,
                Count = m.Count,
                Image = m.ProductImage.FirstOrDefault(m => m.IsMain).Image,
            }).ToList();

            int pageCount = await GetPageCountAsync(4);


            Paginate<ProductVM> model = new(mapedDatas, pageCount, page);

            return View(model);
        }


        private async Task<int> GetPageCountAsync(int take)
        {
            var count = await _context.Products.CountAsync();

            return (int)Math.Ceiling((decimal)count / take);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();

            ViewBag.categories = new SelectList(categories, "Id", "Name");

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name");

            if (!ModelState.IsValid) return View();

            foreach (var item in request.Image)
            {
                if (!item.CheckFileSize(500))
                {
                    ModelState.AddModelError("Image", "Image size must be max 500kb");
                    return View();
                }

                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Image", "File size must be only Image");
                    return View();
                }
            }

            List<ProductImage> images = new();

            foreach (var item in request.Image)
            {
                string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;

                string path = Path.Combine(_env.WebRootPath, "assets/img", fileName);

                await item.SaveFileToLocalAsync(path);

                images.Add(new ProductImage
                {
                    Image = fileName
                });
            }

            images.FirstOrDefault().IsMain = true;

            Product product = new()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
                ProductImage = images,
                Count = request.Count
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var products = await _context.Products.Where(m => !m.SoftDeleted)
                                                     .Include(m => m.Category)
                                                     .Include(m => m.ProductImage)
                                                     .FirstOrDefaultAsync(m => m.Id == id);
            if (products is null) return NotFound();


            foreach (var item in products.ProductImage)
            {
                string path = Path.Combine(_env.WebRootPath, "assets/img", item.Image);

                path.DeleteFileFromLocal();
            }

            _context.Products.Remove(products);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            var product = await _context.Products.Where(m => !m.SoftDeleted)
                                                     .Include(m => m.Category)
                                                     .Include(m => m.ProductImage)
                                                     .FirstOrDefaultAsync(m => m.Id == id);
            if (product is null) return NotFound();

            List<ProductImageVM> productImages = new();

            foreach (var item in product.ProductImage)
            {
                productImages.Add(new ProductImageVM
                {
                    Image = item.Image,
                    IsMain = item.IsMain
                });
            }

            ProductDetailVM model = new()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category.Name,
                SellingCount = product.SellingCount,
                RatingCount = product.RatingCount,
                Count = product.Count,
                ProductImage = productImages
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            var categories = await _context.Categories.ToListAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name");

            if (!ModelState.IsValid) return BadRequest();
            if (id is null) return BadRequest();

            var product = await _context.Products.Where(m => !m.SoftDeleted)
                                                     .Include(m => m.Category)
                                                     .Include(m => m.ProductImage)
                                                     .FirstOrDefaultAsync(m => m.Id == id);
            if (product is null) return NotFound();




            ProductEditVM productEdit = new()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Count = product.Count,
                CategoryId = product.CategoryId,
                ExistImage = product.ProductImage.Select(m => new ProductImageEditVM
                {
                    Id = m.Id,
                    Image = m.Image,
                    IsMain = m.IsMain,
                    ProductId = m.ProductId
                }).ToList()
            };

            return View(productEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ProductEditVM request)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name");

            if (!ModelState.IsValid) return BadRequest();
            if (id is null) return BadRequest();

            var product = await _context.Products.Where(m => !m.SoftDeleted)
                                                     .Include(m => m.Category)
                                                     .Include(m => m.ProductImage)
                                                     .FirstOrDefaultAsync(m => m.Id == id);
            if (product is null) return NotFound();

            List<ProductImage> images = product.ProductImage.ToList();

            if (request.NewImages != null)
            {
                foreach (var item in request.NewImages)
                {
                    if (!item.CheckFileType("image/"))
                    {
                        request.ExistImage = product.ProductImage.Select(m => new ProductImageEditVM
                        {
                            Id = m.Id,
                            ProductId = m.ProductId,
                            Image = m.Image,
                            IsMain = m.IsMain
                        }).ToList();

                        ModelState.AddModelError("NewImages", "File type must be only Image");
                        return View(request);

                    }

                    if (!item.CheckFileSize(500))
                    {
                        request.ExistImage = product.ProductImage.Select(m => new ProductImageEditVM
                        {
                            Id = m.Id,
                            ProductId = m.ProductId,
                            Image = m.Image,
                            IsMain = m.IsMain
                        }).ToList();

                        ModelState.AddModelError("NewImages", "Image size must be max 500 kb");
                        return View(request);
                    }
                }


                foreach (var item in request.NewImages)
                {
                    string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;

                    string newPath = Path.Combine(_env.WebRootPath, "assets/img", fileName);

                    await item.SaveFileToLocalAsync(newPath);


                    ProductImage image = new()
                    {
                        Image = fileName
                    };

                    images.Add(image);

                }

            }   
                product.ProductImage = images;
                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = (decimal)request.Price;
                product.CategoryId = (int)request.CategoryId;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }
    }
}

