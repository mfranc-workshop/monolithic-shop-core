using Microsoft.AspNetCore.Mvc;
using monolithic_shop_core.Services;

namespace monolithic_shop_core.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportGenerator _reportGenerator;

        public ReportsController(IReportGenerator reportGenerator)
        {
            _reportGenerator = reportGenerator;
        }

        [Route("/reports")]
        public IActionResult Index()
        {
            var report = _reportGenerator.GenerateReport();
            return View(report);
        }
    }
}
