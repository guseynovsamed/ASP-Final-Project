using System;
using System.ComponentModel.DataAnnotations;

namespace ASPProject.ViewModels.Sliders.SliderInfos
{
	public class SliderInfoCreateVM
	{
        [Required]
        public string Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public IFormFile NewBackgroundImage { get; set; }
    }
}

