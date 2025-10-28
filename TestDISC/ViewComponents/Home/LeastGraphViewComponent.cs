using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestDISC.ViewComponents.Home
{
    public class LeastGraphViewComponent : ViewComponent
    {
        public LeastGraphViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(typeof(LeastGraphViewComponent));
        }
    }
}

