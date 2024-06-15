using System;
namespace ASPProject.ViewModels.SelectedProducts
{
	public class SelectedProductCreateVM
	{
        public string FirstTitle { get; set; }
        public string SecondTitle { get; set; }
        public string Description { get; set; }
        public IFormFile NewImage { get; set; }
    }
}

