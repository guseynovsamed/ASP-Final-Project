using System;
namespace ASPProject.ViewModels.Sliders.SliderInfos
{
	public class SliderInfoEditVM
	{
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public IFormFile? NewBackgroundImage { get; set; }
    }
}

