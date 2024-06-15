using System;
using ASPProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASPProject.Data
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<SliderInfo> SliderInfos { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Featur> Featurs { get; set; }
        public DbSet<Fact> Facts { get; set; }
        public DbSet<SelectedProduct> SelectedProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

