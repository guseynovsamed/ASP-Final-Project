using System;
using ASPProject.Models;
using System.ComponentModel.DataAnnotations;

namespace ASPProject.ViewModels.Products
{
    public class ProductEditVM
    {
        [Required]
        [StringLength(20, ErrorMessage = "Length must be max 20")]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public decimal? Price { get; set; }
        public int Count { get; set; }
        public int? CategoryId { get; set; }
        public List<ProductImageEditVM>? ExistImage { get; set; }
        public List<IFormFile>? NewImages { get; set; }
    }
}

