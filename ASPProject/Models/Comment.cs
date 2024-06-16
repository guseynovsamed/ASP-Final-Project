using System;
namespace ASPProject.Models
{
	public class Comment : BaseEntity
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Text { get; set; }
		public Product Product { get; set; }
		public int ProductId { get; set; }
        public AppUser User { get; set; }
		public int UserId { get; set; }


    }
}

