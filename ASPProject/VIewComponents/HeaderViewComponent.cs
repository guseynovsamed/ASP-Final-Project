using System;
using Microsoft.AspNetCore.Mvc;

namespace ASPProject.VIewComponents
{
	public class HeaderViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return await Task.FromResult(View());
		}

    }
}

