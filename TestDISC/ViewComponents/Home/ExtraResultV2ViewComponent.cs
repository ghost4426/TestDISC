using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestDISC.Models.ResultDISC;
using TestDISC.Models.User;

namespace TestDISC.ViewComponents.Home
{
	public class ExtraResultV2ViewComponent : ViewComponent
    {
        public ExtraResultV2ViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync((ResultDISCModel, UserCreate) item)
        {
            return View("ExtraResultV2", item);
        }
    }
}

