using System;
namespace ASPProject.ViewModels.Products
{
	public class ProductImageEditVM
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string Image { get; set; }
        public bool IsMain { get; set; }
	}
}

