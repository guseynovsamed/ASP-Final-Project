using System;
using Microsoft.AspNetCore.Identity;

namespace ASPProject.Models
{
	public class AppUser : IdentityUser
    {
		public string FullName { get; set; }
		public int BasketId { get; set; }	
		public Basket Basket { get; set; }
		public ICollection<Comment> Comments { get; set; }
	}
}	

