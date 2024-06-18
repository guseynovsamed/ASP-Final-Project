using System;
using System.ComponentModel.DataAnnotations;

namespace ASPProject.ViewModels.Products
{
	public class ProductCreateVM
	{
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public List<IFormFile>? Image { get; set; }
        [Required(ErrorMessage = "Price can't be empty")]
        public decimal Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}

