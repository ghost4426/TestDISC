using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestDISC.Database.TestDISCModels;
using TestDISC.Models.Report;
using TestDISC.Models.UtilsProject;
using TestDISC.Models.UtilsProject.Filters;
using TestDISC.Services;
using TestDISC.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestDISC.Controllers
{
    [TypeFilter(typeof(AdminFilter))]
    public class ControlController : Controller
    {
        private readonly IResultDISCServices _resultDISCServices;
        private readonly IPartnerService _partnerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public ControlController(IResultDISCServices resultDISCServices,
            IPartnerService partnerService,
            IHttpContextAccessor httpContextAccessor)
        {
            _resultDISCServices = resultDISCServices;
            _partnerService = partnerService;
            _httpContextAccessor = httpContextAccessor;
            _session = httpContextAccessor.HttpContext.Session;
        }

        public IActionResult Index()
        {
            ViewBag.PartnerSelection = _partnerService.GetPartnerSelection();
            return View(new ReportFilter());
        }

        [HttpPost]
        public async Task<IActionResult> Index(ReportFilter filter)
        {
            var loginuser = _session.GetObjectFromJson<Loginuser>(Utils.NameSession);
            filter.partnerviewid = loginuser.Partnerid ?? 0;

            var dateNow = Utils.DateNow();
            var resutlDISCExcel = await _resultDISCServices.ExportExcel(dateNow, filter);

            return File(resutlDISCExcel.Stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", resutlDISCExcel.FileName);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(Utils.NameCookie) == true)
            {
                Response.Cookies.Delete(Utils.NameCookie);
            }

            _session.Remove(Utils.NameSession);

            return RedirectToAction("Login", "Home", new { Area = "" });
        }
    }
}

