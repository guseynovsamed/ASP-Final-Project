using System;
using System.ComponentModel.DataAnnotations;

namespace ASPProject.ViewModels.Facts
{
	public class FactEditVM
	{
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Icon { get; set; }
    }
}

