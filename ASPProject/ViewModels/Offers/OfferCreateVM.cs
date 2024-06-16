using System;
using System.ComponentModel.DataAnnotations;

namespace ASPProject.ViewModels.Offers
{
	public class OfferCreateVM
	{
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile NewImage { get; set; }
    }
}

