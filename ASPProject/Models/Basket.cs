using System;
namespace ASPProject.Models
{
    public class Basket : BaseEntity
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public int ProductCount { get; set; }
        public decimal TotalPrice { get; set; }
        public AppUser User { get; set; }
    }
}

