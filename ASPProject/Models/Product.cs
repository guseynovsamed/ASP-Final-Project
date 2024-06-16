using System;
namespace ASPProject.Models
{
	public class Product : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Count { get; set; }
		public decimal Price { get; set; }
		public Category Category { get; set; }
		public int CategoryId { get; set; }
		public int RatingCount { get; set; }
		public int SellingCount { get; set; }
		public ICollection<ProductImage> ProductImage { get; set; }
		public ICollection<Comment> Comments { get; set; }

	}	
}

