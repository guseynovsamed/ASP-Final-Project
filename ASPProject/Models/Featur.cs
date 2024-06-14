using System;
using System.ComponentModel.DataAnnotations;

namespace ASPProject.Models
{
	public class Featur : BaseEntity
	{
		[Required]
		public string Icon { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
	}
}

