using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TestDISC.Queries.Interfaces;
using TestDISC.Services.Interfaces;

namespace TestDISC.Services
{
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerQuery _partnerQuery;

        public PartnerService(IPartnerQuery partnerQuery)
        {
            _partnerQuery = partnerQuery;
        }

        public List<SelectListItem> GetPartnerSelection()
        {
            return _partnerQuery.QuerySelection();
        }
    }
}

