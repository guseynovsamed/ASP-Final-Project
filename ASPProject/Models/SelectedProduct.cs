using System;
namespace ASPProject.Models
{
	public class SelectedProduct : BaseEntity
	{
		public string FirstTitle { get; set; }
        public string SecondTitle { get; set; }
        public string Description { get; set; }
		public string Image { get; set; }
	}
}

