using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using QueryFilterServiceApp.Data;
using QueryFilterServiceApp.Models;

namespace QueryFilterServiceApp.Services {
    public class ReportStorage : ReportStorageWebExtension {
        private readonly IUserService userService;
        private readonly SchoolContext dbContext;

        public ReportStorage(IUserService userService, SchoolContext dbContext) {
            this.userService = userService;
            this.dbContext = dbContext;
        }

        public override bool CanSetData(string url) {
            return true;
        }

        public override bool IsValidUrl(string url) {
            return true;
        }

        public override byte[] GetData(string url) {
            var userIdentity = userService.GetCurrentUserId();
            var reportData = dbContext.Reports.Where(a => a.ID == int.Parse(url) && a.Student.ID == userIdentity).FirstOrDefault();
            if(reportData != null) {
                return reportData.ReportLayout;
            } else {
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException(string.Format("Could not find report '{0}'.", url));
            }
        }

        public override Dictionary<string, string> GetUrls() {
            var userIdentity = userService.GetCurrentUserId();
            var reportData = dbContext.Reports.Where(a => a.Student.ID == userIdentity).Select(a => new ReportModel() { Id = a.ID.ToString(), Title = string.IsNullOrEmpty(a.DisplayName) ? "Noname Report" : a.DisplayName });
            var reports = reportData.ToList();
            return reports.ToDictionary(x => x.Id.ToString(), y => y.Title);
        }

        public override void SetData(XtraReport report, string url) {
            var userIdentity = userService.GetCurrentUserId();
            var reportEntity = dbContext.Reports.Where(a => a.ID == int.Parse(url) && a.Student.ID == userIdentity).FirstOrDefault();
            reportEntity.ReportLayout = ReportToByteArray(report);
            reportEntity.DisplayName = report.DisplayName;
            dbContext.SaveChanges();
        }

        public override string SetNewData(XtraReport report, string defaultUrl) {
            var userIdentity = userService.GetCurrentUserId();
            var user = dbContext.Students.Find(userIdentity);
            var newReport = new Report() { DisplayName = defaultUrl, ReportLayout = ReportToByteArray(report), Student = user };
            dbContext.Reports.Add(newReport);
            dbContext.SaveChanges();
            return newReport.ID.ToString();
        }

        static byte[] ReportToByteArray(XtraReport report) {
            using(var memoryStream = new MemoryStream()) {
                report.SaveLayoutToXml(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
