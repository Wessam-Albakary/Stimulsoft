using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Report;
using Stimulsoft.Report.Angular;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Web;

namespace Integrating_the_Report_Viewer_into_an_Application.Controllers
{
    [Controller]
    public class ViewerController : Controller
    {
        static ViewerController()
        {
            // How to Activate
            //Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnO...";
            //Stimulsoft.Base.StiLicense.LoadFromFile("license.key");
            //Stimulsoft.Base.StiLicense.LoadFromStream(stream);
        }

        [HttpPost]
        public IActionResult InitViewer()
        {
            var requestParams = StiAngularViewer.GetRequestParams(this);

            var options = new StiAngularViewerOptions();
            options.Actions.GetReport = "GetReport";
            options.Actions.ViewerEvent = "ViewerEvent";
            options.Appearance.ScrollbarsMode = true;
            //options.Actions.Interaction = "ViewerInteraction";
            options.Appearance.ScrollbarsMode = true;
            options.Toolbar.PrintDestination = StiPrintDestination.Default;
            options.Toolbar.ShowParametersButton = true;
            options.Toolbar.ShowOpenButton = false;
            options.Appearance.ShowTooltips = true;
            options.Appearance.ShowTooltipsHelp = false;
            options.Appearance.AllowMobileMode = false;
            options.Appearance.InterfaceType = StiInterfaceType.Mouse;




            //options.Localization = StiAngularHelper.MapPath(this, "Localization/ar.xml");
            options.Appearance.FullScreenMode = false;
            // options.Theme = StiViewerTheme.Office2013WhiteTeal;
            // options.Appearance.RightToLeft = true;
            //StiOptions.Viewer.RightToLeft = StiRightToLeftType.Yes;

            options.Exports.DefaultSettings.ExportToPdf.EmbeddedFonts = true;

            options.Exports.ShowExportToDocument = false;
            options.Exports.ShowExportToDocument = false;
            options.Exports.ShowExportToPdf = true;
            options.Exports.ShowExportToXps = false;
            options.Exports.ShowExportToPowerPoint = false;
            options.Exports.ShowExportToHtml = false;
            options.Exports.ShowExportToHtml5 = false;
            options.Exports.ShowExportToMht = false;
            options.Exports.ShowExportToText = false;
            options.Exports.ShowExportToRtf = false;
            options.Exports.ShowExportToWord2007 = true;
            options.Exports.ShowExportToOpenDocumentWriter = false;
            options.Exports.ShowExportToExcel = true;
            options.Exports.ShowExportToExcelXml = true;
            options.Exports.ShowExportToExcel2007 = true;
            options.Exports.ShowExportToOpenDocumentCalc = false;
            options.Exports.ShowExportToCsv = false;
            options.Exports.ShowExportToDbf = false;
            options.Exports.ShowExportToXml = false;
            options.Exports.ShowExportToDif = true;
            options.Exports.ShowExportToSylk = false;
            options.Exports.ShowExportToImageBmp = false;
            options.Exports.ShowExportToImageGif = false;
            options.Exports.ShowExportToImageJpeg = false;
            options.Exports.ShowExportToImagePcx = false;
            options.Exports.ShowExportToImagePng = false;
            options.Exports.ShowExportToImageTiff = false;
            options.Exports.ShowExportToImageMetafile = false;
            options.Exports.ShowExportToImageSvg = false;
            options.Exports.ShowExportToImageSvgz = false;
            options.Exports.OpenAfterExport = true;
            options.Toolbar.MenuAnimation = true;
            options.Toolbar.ShowAboutButton = false;
            options.Toolbar.ShowButtonCaptions = false;
            options.Toolbar.ShowFullScreenButton = true;
            options.Toolbar.ShowZoomButton = true;
            options.Toolbar.ShowSendEmailButton = true;
            options.Toolbar.ViewMode = StiWebViewMode.Continuous;
            options.Toolbar.Zoom = StiZoomMode.PageWidth;

            return StiAngularViewer.ViewerDataResult(requestParams, options);
        }

        [HttpPost]
        public   IActionResult GetReport()
        {
            var report =  ReportLoader(this);

            return StiAngularViewer.GetReportResult(this, report);
        }
        public StiReport ReportLoader(Controller ctrl)
        {
            var report = StiReport.CreateNewReport();
            try
            {
                var path = StiAngularHelper.MapPath(ctrl, $"Reports/TotalIncome.mrt");

                report.Load(path);

                report.Dictionary.Variables["OrganizationName"].Value = "دكتور على محمد على ";
                StiImage orgnizationLogo = report.GetComponents()["Image1"] as StiImage;

                StiImage ReayaLogo = report.GetComponents()["Image2"] as StiImage;
                Image LogoImage = Image.FromFile(StiAngularHelper.MapPath(ctrl, $@"wwwroot\global-img\logo-small.png"));
                orgnizationLogo.Image = LogoImage;
                ReayaLogo.Image = LogoImage;

                List<DtoRevenue> source = new List<DtoRevenue>();

                using (StreamReader r = new StreamReader(@"wwwroot\Sources\data.json"))
                {
                    string json = r.ReadToEnd();
                    source = JsonSerializer.Deserialize<List<DtoRevenue>>(json);
                }

                report.RegBusinessObject("revenues", source);
            }
            catch (Exception ex)
            { throw; }


            return report;
        }

        [HttpPost]
        public IActionResult ViewerEvent()
        {
            return StiAngularViewer.ViewerEventResult(this);
        }
    }

    public class DtoRevenue
    {
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public bool IsCash { get; set; }
        public int? InsuranceCompanyId { get; set; }
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
        public int SpecialtyId { get; set; }
        public string SpecialtyName { get; set; }
        public int PhysicianId { get; set; }
        public string FinancialType { get; set; }
        public string PhysicianLocName { get; set; }
        public DateTime ServiceDate { get; set; }
        public Guid EpisodeVisitGuid { get; set; }
        public Guid EpisodeVisitServiceGuid { get; set; }
        public Guid ServiceGuid { get; set; }
        public Guid ServiceCategoryId { get; set; }
        public string ServiceCategoryName { get; set; }
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        public decimal CShare { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Discount { get; set; }
        public string DiscountNote { get; set; }
        public decimal PriceBeforeDiscount { get; set; }
        public dtoInsuranceData InsuranceData { get; set; }
        public dtoReferralData ReferralData { get; set; }
    }
    public class dtoInsuranceData : GenericLookup<int?>
    {
        public GenericLookup<int?> ContractCategory { get; set; }
        public GenericLookup<int?> Beneficiary { get; set; }
        public string MembershipId { get; set; }
        public string BeneficiaryName { get; set; }
    }
    public class dtoReferralData : GenericLookup<int?>
    { }
    public class GenericLookup<T>
    {
        public T Id { get; set; }
        public string EngName { get; set; }
        public string LocName { get; set; }
    }
}


