using System;
using Microsoft.AspNetCore.Identity;

namespace ASPProject.Models
{
	public class AppUser : IdentityUser
    {
		public string FullName { get; set; }
	}
}	

