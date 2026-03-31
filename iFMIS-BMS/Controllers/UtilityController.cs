  using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iFMIS_BMS.Reports;
using iFMIS_BMS.BusinessLayer.Models.DashBoard;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.Reports.Design;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Xml;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.Classes;

using System.Data.SqlClient;


namespace iFMIS_BMS.Controllers
{
    [Authorize]
    public class UtilityController : Controller
    {

        public ActionResult LineGraph()
        {


                //ViewBag.param = "<script>$(document).ready(function ()  { " +
                //                "$('#chart').kendoChart({ " +
                //                " legend: { " +
                //                        "position: \"right\"," +
                //                    "}," +
                //                    "seriesDefaults: { type:  \"line\", style: \"smooth\",style: \"hybrid\"}," +
                //                    "categoryAxis: {categories: [ " +
                //                    "[2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011]" +
                //                    "]},chartArea: {background: \"transparent\",width: 980,height: 800} " +
                //                    ",series: [ " +
                //                    "{ name: \"India\",data: [3.907, 7.943, 7.848, 9.284, 9.263, 9.801, 3.890, 8.238, 9.552, 6.855" +
                //                    "]}], " +
                //                    "valueAxis: {labels: {format: \"{0:N2}\"} " +
                //                    "},tooltip: {visible: true,format: \"{0:N2}\",template: \"#= series.name #: #= kendo.format('{0:N2}',value) #\"}})});</script>";

            return View("pv_LineGraph");
        }
        public ActionResult pv_Graph()
        {

            return PartialView("pv_Graph");

        }

        public JsonResult LineYear()
        {
            zLineUtility_Layer ddl = new zLineUtility_Layer();
            var lst = ddl.proposal_year();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GraphView()
        {

            return PartialView("pv_GraphViewall");
          
        }

        public PartialViewResult GraphViewData(int? Year_Of, int? OOE_ID, int? ACC_ID)
        {
            zLineUtility_Layer aaaa = new zLineUtility_Layer();
            // ViewBag.accountName = aaaa.getAbbrivation(Convert.ToInt32(Session["ProposalID"]), Convert.ToInt32(Session["gLineAccountCode"]), Convert.ToInt32(Session["gLineProgramID"]), Convert.ToInt32(Session["gLineAccountID"]), Convert.ToInt32(Session["LineYear"]), Convert.ToInt32(Session["LineOffice"]), Convert.ToInt32(Session["LineOOE"]));
            IEnumerable<AllLineGraph_Model> lst = aaaa.allLine(Year_Of, OOE_ID, ACC_ID);
            return PartialView("pv_GraphViewall_Data", lst);
        }

        public PartialViewResult smallgraph(long? ProposalID=0, int? gLineAccountCode=0, int? gLineProgramID=0, int? gLineAccountID=0, int? LineYear=0, int? LineOffice=0, int? LineOOE=0)
            {
            zLineUtility_Layer aaaa = new zLineUtility_Layer();
            ViewBag.accountName = aaaa.getAbbrivation(ProposalID, gLineAccountCode, gLineProgramID, gLineAccountID, LineYear, LineOffice, LineOOE);
            IEnumerable<LineGraph_Model> lst = aaaa.Statistics(ProposalID, gLineAccountCode, gLineProgramID, gLineAccountID, LineYear, LineOffice, LineOOE);
            return PartialView("pv_Graph", lst);
        }

        public JsonResult separate()
        {
            zLineUtility_Layer ddl = new zLineUtility_Layer();
            var lst = ddl.separate();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getOOEID()
        {
            zLineUtility_Layer ddl = new zLineUtility_Layer();
            var lst = ddl.getOOEID();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getAccountID(int?Year_Of, int? OOE_ID)
        {
            zLineUtility_Layer ddl = new zLineUtility_Layer();
            var lst = ddl.getAccountID(Year_Of,OOE_ID);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public string GETPS(int office_id, int year_of)
        {

            zUtilitySummary_Layer OfficeAdminLayer = new zUtilitySummary_Layer();
            return "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", OfficeAdminLayer.GETPS(office_id, year_of));
        }
        public string GETMOOE(int office_id, int year_of)
        {

            zUtilitySummary_Layer OfficeAdminLayer = new zUtilitySummary_Layer();
            return "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", OfficeAdminLayer.GETMOOE(office_id, year_of));
        }
        public string GETCO(int office_id, int year_of)
        {

            zUtilitySummary_Layer OfficeAdminLayer = new zUtilitySummary_Layer();
            return "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", OfficeAdminLayer.GETCO(office_id, year_of));
        }

        public string GETOPIS(int? office_id)
        {
            zUtilitySummary_Layer data = new zUtilitySummary_Layer();
            return data.GETOPIS(office_id);
        }
        public JsonResult getSummary([DataSourceRequest] DataSourceRequest request, int? office_id, int? year_of)
        {
            zUtilitySummary_Layer LoadSource = new zUtilitySummary_Layer();
            var lst = LoadSource.readSummary(office_id, year_of);
            return Json(lst.ToDataSourceResult(request));
        }


        public PartialViewResult sampleGraph(int? ProposalID, int? gLineAccountCode, int? gLineProgramID, int? gLineAccountID, int? LineYear, int? LineOffice, int? LineOOE)
        {
            zLineUtility_Layer aaaa = new zLineUtility_Layer();
            ViewBag.accountName = aaaa.getAbbrivation(ProposalID, gLineAccountCode, gLineProgramID, gLineAccountID, LineYear, LineOffice, LineOOE);
            IEnumerable<accChart_Model> lst = aaaa.teststatistic(ProposalID, gLineAccountCode, gLineProgramID, gLineAccountID, LineYear, LineOffice, LineOOE);
            return PartialView("pv_sampleGraph", lst);
        }
        public ActionResult UpdateProposedAmountView()
        {
            return View("pvUpdateProposedAmount");
        }

        public JsonResult GetAllProposedAccounts([DataSourceRequest] DataSourceRequest request)
        {
                Program_Layer pl = new Program_Layer();
                var lst = pl.ProposedAmountList();
                return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string updateAllProposedAccount()
        {
            Program_Layer pl = new Program_Layer();
            return pl.UpdateAllProposedAmount();
        }
        public ActionResult UpdateAccountNameIndex()
        {
            Program_Layer Layer = new Program_Layer();
            ViewData["ObjectOfExpenditures"] = Layer.PopulateObjectofExpenditure();
            ViewData["Programs"] = Layer.PopulatePrograms(0);
            ViewData["Accounts"] = Layer.PopulateAccounts();
            
            return View("pvUpdateAccountNameIndex");
        }
        public ActionResult getFilteredPrograms(int OfficeID)
        {
            Program_Layer Layer = new Program_Layer();
            var lst = Layer.PopulatePrograms(OfficeID);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListOfAccountsForEdit([DataSourceRequest] DataSourceRequest request,int OfficeID)
        {
            Program_Layer pl = new Program_Layer();
            var lst = pl.GetListOfAccountsForEdit(OfficeID);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateAccounts([DataSourceRequest] DataSourceRequest request,
        [Bind(Prefix = "models")]IEnumerable<AccountToUpdateModel> Accounts,int OfficeID)
        {
            Program_Layer pl = new Program_Layer();
            var lst = pl.GetAccountsForUpdate_Original(Accounts, OfficeID);
            if (Accounts != null && ModelState.IsValid)
            {
                    if (pl.UpdateAccounts(Accounts) == "1")
                    {
                        return Json(Accounts.ToDataSourceResult(request, ModelState));    
                    }
                    else
                    {
                        return Json(lst.ToDataSourceResult(request, ModelState));
                    }
            }
            else
            {
                return Json(lst.ToDataSourceResult(request, ModelState));
            }
            
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddNewAccount([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AccountToUpdateModel> Accounts,int OfficeID)
        {
            var results = new List<AccountToUpdateModel>();
            Program_Layer pl = new Program_Layer();
            var lst = pl.GetAccountsForUpdate_Original(Accounts, OfficeID);
            //if (Accounts != null && ModelState.IsValid)
            //{
            //    if (pl.UpdateAccounts(Accounts) == "1")
            //    {
            //        return Json(Accounts.ToDataSourceResult(request, ModelState));
            //    }
            //    else
            //    {
            //        return Json(lst.ToDataSourceResult(request, ModelState));
            //    }
            //}
            //else
            //{
                return Json(lst.ToDataSourceResult(request, ModelState));
            //}
        }

      
        public JsonResult OfficePSMooeCA(int? BudgetYear = 0, int? OfficeId = 0, int? ooeid=0)
        {
            zLineUtility_Layer ddl = new zLineUtility_Layer();
            var lst = ddl.OfficePSMooeCA(BudgetYear, OfficeId);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public JsonResult OfficePSMooeCAProgram(int? BudgetYear = 0, int? OfficeId = 0, int? ooeid = 0)
        {
            zLineUtility_Layer ddl = new zLineUtility_Layer();
            var lst = ddl.OfficePSMooeCAProgram(BudgetYear, OfficeId);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult expenseclassgraph(int? LineYear=0, int? LineOffice=0, int? LineOOE=0)
        {
            zLineUtility_Layer aaaa = new zLineUtility_Layer();
            // ViewBag.accountName = aaaa.getAbbrivation(ProposalID, gLineAccountCode, gLineProgramID, gLineAccountID, LineYear, LineOffice, LineOOE);
            IEnumerable<DashBoardModel> lst = aaaa.StatisticsExpenseClass(LineYear, LineOffice, LineOOE);
            return PartialView("pv_SummeryPerOfficeGraph", lst);
        }

        public JsonResult PieChartPSMooeCADetails(int? BudgetYear=0, int? OfficeId = 0, int? ooeid = 0)
        {
            zLineUtility_Layer ddl = new zLineUtility_Layer();
            var lst = ddl.PieChartPSMooeCADetails(BudgetYear, OfficeId,ooeid);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PieChartPSMooeCADetailsP2(int? yrof = 0, int? offId = 0, int? expid = 0, int? expid2 = 0)
        {
            zLineUtility_Layer ddl = new zLineUtility_Layer();
            var lst = ddl.PieChartPSMooeCADetailsP2(yrof, offId, expid, expid2);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccountDetails(int? yrof = 0, int? offId = 0, int? expid=0, string selPie = "")
        {
            zLineUtility_Layer GAD = new zLineUtility_Layer();
            var lst = GAD.GetAccountDetails(yrof, offId,expid, selPie);
            return Json(lst,JsonRequestBehavior.AllowGet);
        }
        //grid
        public JsonResult GetAccountDetailsGrid([DataSourceRequest] DataSourceRequest request, int? yrof = 0, int? offid = 0, int? expid = 0, string selpie = "")
        {
            zLineUtility_Layer gad = new zLineUtility_Layer();
            var lst = gad.getaccountdetailsgrid(yrof, offid, expid, selpie);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public double getProposedSummary(int? Yearof = 0,int? OfficeId=0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_PieChartPerOfficeExpenseTotal " + Yearof + "," + OfficeId + "", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }

        public double getProposedExpenseSummary(int? Yearof = 0, int? OfficeId = 0, int? ooeid = 0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_PieChartPerOfficeECDetailsTotal] " + Yearof + "," + OfficeId + "," + ooeid + "", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }
        public double getProposedPerAccount(int? Yearof = 0, int? OfficeId = 0, int? ooeid = 0, string expid2 = "")
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_PieChartPerOfficeECDetailsP2_Sum] " + Yearof + "," + OfficeId + "," + ooeid + ",'" + expid2 + "'", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }
        public double getProposedPerAccountBreakdown(int? Yearof = 0, int? OfficeId = 0, int? ooeid = 0, string expid2 = "")
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_GridAccount_Sum] " + Yearof + "," + OfficeId + ",'" + expid2 + "'," + ooeid + "", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }

        public JsonResult GetProposeCount(int? yrof = 0, int? offId = 0, int? expid = 0, string expid2 = "")
        {
            zLineUtility_Layer ddl = new zLineUtility_Layer();
            var lst = ddl.GetProposeCount(yrof, offId, expid2);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPSDetail([DataSourceRequest] DataSourceRequest request, int? yrof = 0, int? offId = 0, string selPie = "")
        {
            zLineUtility_Layer gad = new zLineUtility_Layer();
            var lst = gad.GetPSDetail(yrof, offId, selPie);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public int getPieChartPSMooeCACountTotal(int? Yearof = 0, int? OfficeId = 0, string expid2 = "")
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_PieChartProposedCount_Total] " + Yearof + "," + OfficeId + ",'" + expid2 + "'", con);
                con.Open();
                return Convert.ToInt32(com.ExecuteScalar().ToString());
            }
        }
        public JsonResult BarAccountComparison(int? BudgetYear = 0, string acctname = "")
        {
            zLineUtility_Layer gad = new zLineUtility_Layer();
            var lst = gad.BarAccountComparison(BudgetYear, acctname);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public double getBarChartComparisonTotal(int? Yearof = 0, string expid2 = "")
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_AccountTableTotal] " + Yearof + ",'" + expid2 + "'", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }
        public JsonResult BarAppropriationComparison(int? yearof = 0, int accountid = 0)
        {
            zLineUtility_Layer gad = new zLineUtility_Layer();
            var lst = gad.BarAppropriationComparison(yearof, accountid);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult SummaryPerOffice(int? office_id, int? year_of)
        {
            //ViewBag.office_id = office_id;
            //ViewBag.year_of = year_of;
            Session["office_id"] = office_id;
            Session["year_of"] = year_of;
            //edit on 7/20/2018 -xXx
            //return PartialView("pv_SummeryPerOffice");
            return PartialView("pv_SummeryPerOfficeGraph");
        }
        public PartialViewResult ProportionDetails(string offname="",int? acctnameid=0, int? yearof=0)
        {
            Session["offname"] = offname;
            Session["acctnameid"] = acctnameid;
            Session["yearof"] = yearof;
            return PartialView("pv_ProportionDetail");
        }
    }
}