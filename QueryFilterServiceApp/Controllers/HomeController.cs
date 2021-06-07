using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueryFilterServiceApp.Data;
using QueryFilterServiceApp.Models;
using QueryFilterServiceApp.Services;

namespace QueryFilterServiceApp.Controllers {
    [Authorize]
    public class HomeController : Controller {
        public async Task<IActionResult> Index([FromServices] IUserService userService, [FromServices] SchoolContext dbContext) {
            var reportData = dbContext.Reports
                .Where(a => a.Student.ID == userService.GetCurrentUserId())
                .Select(a => new ReportModel() {
                    Id = a.ID.ToString(),
                    Title = string.IsNullOrEmpty(a.DisplayName) ? "Noname Report" : a.DisplayName });

            return View(await reportData.ToListAsync());
        }

        public IActionResult DesignReport(ReportModel model) {
            return View(model);
        }

        public IActionResult DisplayReport(ReportModel reportModel) {
            return View(reportModel);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveReport([FromServices] IUserService userService, [FromServices] SchoolContext dbContext, int reportId) {
            var userIdentity = userService.GetCurrentUserId();
            var reportData = await dbContext.Reports.Where(a => a.ID == reportId && a.Student.ID == userIdentity).FirstOrDefaultAsync();
            if(reportData != null) {
                dbContext.Reports.Remove(reportData);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
