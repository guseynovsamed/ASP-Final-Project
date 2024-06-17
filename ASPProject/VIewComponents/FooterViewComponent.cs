using System;
using ASPProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPProject.ViewComponents
{
	public class FooterViewComponent : ViewComponent
	{
        public readonly AppDbContext _context;

        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(m => m.Key, m => m.Value);
            return await Task.FromResult(View(settings));
        }
    }
}

