using System;
using ASPProject.Models;

namespace ASPProject.ViewModels
{
	public class HomeVM
	{
		public List<SliderInfo> SliderInfos { get; set; }
        public List<Slider> Sliders { get; set; }
		public List<Featur> Featurs { get; set; }
		public List<Fact>Facts { get; set; }
		public List<SelectedProduct> SelectedProducts { get; set; }
    }
}

