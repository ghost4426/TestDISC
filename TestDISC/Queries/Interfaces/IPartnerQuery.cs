using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TestDISC.Queries.Interfaces
{
    public interface IPartnerQuery
    {
        List<SelectListItem> QuerySelection();
    }
}

