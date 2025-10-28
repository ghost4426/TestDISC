using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestDISC.ViewComponents.Home
{
	public class MostGraphV2ViewComponent : ViewComponent
    {
        public MostGraphV2ViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(typeof(MostGraphV2ViewComponent));
        }
    }
}

