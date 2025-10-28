using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestDISC.Database;
using TestDISC.Queries.Interfaces;

namespace TestDISC.Queries
{
    public class PartnerQuery : IPartnerQuery
    {
        private readonly TestDISCContext _testDISCContext;

        public PartnerQuery(TestDISCContext testDISCContext)
        {
            _testDISCContext = testDISCContext;
        }

        public List<SelectListItem> QuerySelection()
        {
            return _testDISCContext.Partner.OrderBy(a => a.Id).Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name,
            }).ToList();
        }
    }
}

