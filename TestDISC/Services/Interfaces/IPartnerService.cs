using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TestDISC.Services.Interfaces
{
    public interface IPartnerService
    {
        List<SelectListItem> GetPartnerSelection();
    }
}

