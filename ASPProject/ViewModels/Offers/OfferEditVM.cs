using System;
namespace ASPProject.ViewModels.Offers
{
	public class OfferEditVM
	{
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IFormFile NewImage { get; set; }
    }
}

