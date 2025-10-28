using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestDISC.Models.ResultDISC;
using TestDISC.Models.User;

namespace TestDISC.ViewComponents.Home
{
	public class ExtraResultViewComponent : ViewComponent
	{
		public ExtraResultViewComponent()
		{
		}

        public async Task<IViewComponentResult> InvokeAsync((ResultDISCModel, UserCreate) item)
        {
            return View("ExtraResult", item);
        }
    }
}