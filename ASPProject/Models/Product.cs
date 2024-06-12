using System;
namespace ASPProject.Models
{
	public class Product : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Count { get; set; }
	}	
}

