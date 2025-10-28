using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestDISC.ViewComponents.Home
{
	public class LeastGraphV2ViewComponent : ViewComponent
    {
        public LeastGraphV2ViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(typeof(LeastGraphV2ViewComponent));
        }
    }
}