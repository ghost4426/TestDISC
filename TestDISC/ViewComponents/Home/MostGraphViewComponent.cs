using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestDISC.ViewComponents.Home
{
    public class MostGraphViewComponent : ViewComponent
    {
        public MostGraphViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(typeof(MostGraphViewComponent));
        }
    }
}

