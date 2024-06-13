using System;
namespace ASPProject.ViewModels.Sliders.SliderInfos
{
	public class SliderInfoVM
	{
		public int? Id { get; set; }
		public string Title { get; set; }
        public string? Description { get; set; }
		public string? BackgroundImage { get; set; }
        public string NewBackgroundImage { get; internal set; }
    }
}

