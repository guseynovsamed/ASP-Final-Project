using System;
using System.ComponentModel.DataAnnotations;

namespace ASPProject.ViewModels.Featurs
{
	public class FeaturEditVM
	{
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Icon { get; set; }
    }
}

