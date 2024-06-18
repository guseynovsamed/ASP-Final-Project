using System;
using ASPProject.Models;

namespace ASPProject.ViewModels.Products
{
	public class ProductDetailVM
	{
        public string Name { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int RatingCount { get; set; } = 0;
        public int SellingCount { get; set; } = 0;
        public List<ProductImageVM> ProductImage { get; set; }

    }
}

