using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Budget_Preparation;
using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
using System.Data.SqlClient;
using System.Globalization;
using iFMIS_BMS.CTTS;
using iFMIS_BMS.PMS;
using iFMIS_BMS.Classes;
using System.Web.Script.Serialization;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Text;
using iFMIS_BMS.Base;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using iFMIS_BMS.BusinessLayer.Layers.Maintenance;


namespace iFMIS_BMS.Controllers
{
      [Authorize]      
    public class HomeController : Controller
    {
        TrackingSoapClient CTTSdata = new TrackingSoapClient();
        inventorySoapClient PMSdata = new inventorySoapClient();

        public Boolean CTTSEnable()
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_CTTSUserAccess "+ Account.UserInfo.eid.ToString() + ",1", con);
                con.Open();
                return Convert.ToBoolean(com.ExecuteScalar().ToString());
            }
        }
        public string GetRefno(string control,int isid,string iskeycode,string Particular, string amount)
        {
            try
            {
                string dt = CTTSdata.get_unique_refno(control, isid, iskeycode, Particular, amount);
                return dt;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //public DataTable GetAllEmployee()
        //{
        //    DataTable dt = PMSdata.AllEmployee();
        //    return dt;
        //}

        //public JsonResult GetAllEmployee()
        //{

        //    BudgetControlModel data = new BudgetControlModel();
        //    //DataTable _dt = new DataTable();
        //    //string _sqlQuery = "Select * from fn_BMS_OutgoingQR ('" + qr + "')";
        //    //_dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
        //    DataTable _dt = PMSdata.Office();
        //    try
        //    {
        //        data.OfficeID = Convert.ToInt32(_dt.Rows[0][0].ToString());
        //        data.OfficeName = _dt.Rows[0][1].ToString();
        //        data.officeabbr = _dt.Rows[0][2].ToString();
        //        data.type = "success";
        //    }
        //    catch (Exception ex)
        //    {
        //        data.OfficeName = "Error";
        //        data.message = ex.Message;
        //        data.type = "error";
        //        data.Amount = 0;
        //    }
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetInventoryProp([DataSourceRequest] DataSourceRequest request)
        {
            DataTable dt = PMSdata.Inventory();//("select " + tmpFields + " from vw_ActivePropertiesRevised where eid='" + ids + "'  and (PropertyNo='" + pProp + "' or OldPropertyNo='" + pProp + "') order by Description asc ").PropDataSet();
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }

        public ActionResult GetOfficeName([DataSourceRequest]DataSourceRequest request)
        {
            DataTable dt = PMSdata.Office();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        //public string GetObrDetails(string Refno) //table
        //{
        //    try
        //    {
        //        string dt = CTTSdata.get_obrdetails(Refno);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}
        public string SaveRefno(string uniquerefno, int statuscode,string remarks)
        {
            try
            {
                string dt = CTTSdata.save_status_log(uniquerefno, statuscode, remarks, Convert.ToInt32(Account.UserInfo.eid.ToString()));
                return dt;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string LogTrans(string uniquerefno, int statuscode, string remarks, int userid)
        {
            try
            {
                string dt = "walay pay function!...";//CTTSdata.(uniquerefno, statuscode, remarks, userid);
                return dt;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string LogTransLink(string uniquerefno)
        {
            try
            {
                string dt =  CTTSdata.get_tracking_link(uniquerefno);
                return dt;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult Index()
        {
            #region Original Index Beta Version 1.0
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 2)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                if (Account.UserInfo.UserTypeDesc == "Budget In-Charge")
                {
                    //return View("pv_ApprovedBudget");
                    return View("pv_ApprovedBudget");
                }
                else
                {
                    return View("pv_Home4BdgtHR");
                }
            }
            else
            {
                return View("_UnAuthorizedAccess");
            } 
            #endregion
            
        }
        public string UpdateConsolidatedAmount(int Yearof,long eid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("sp_bms_UpdateConsolidatedTotal " + Yearof + ","+ eid + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateConsolidatedAmountPerOffice(int officeid, int Yearof, long eid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("[sp_bms_UpdateConsolidated_PerOffice] "+ officeid + ", " + Yearof + "," + eid + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public PartialViewResult ApprovePanel()
        {
            return PartialView("pvApprovePanel");
        }
        public ActionResult ePortal()
        {
            return Redirect("https://pgas.ph/");
            //return Redirect("http://10.100.100.5/eportal/"); //tacurong city
            //return Redirect("https://pgzn.zamboangadelnorte.gov.ph/eportal"); //pgzn
        }

        public PartialViewResult pv_ApprovedBudget()
        {
            return PartialView("pv_ApprovedBudget");
        }
        public PartialViewResult viewForFundingDetails(string isCasual)
        {
            Session["isCasual"] = isCasual;
            return PartialView("pvProposedPositionConsolidation");
        }
         
        public PartialViewResult NewAccounts()
        {
            return PartialView("pvNewAccounts");
        }
        public PartialViewResult ReqChangeproposedDate(int? ProposedItemID)
        {
            ForFundingModel ForFundingModel = new ForFundingModel(); 
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select AppointmentDateEffectivity from tbl_R_BMSProposedNewItem where ProposedItemID = 
                                                (select ProposedItemID from tbl_R_BMSSubmittedForFundingData where SeriesID = '" + ProposedItemID + "')", con);
                    con.Open();
                    ForFundingModel.SalaryEffectivityDate = com.ExecuteScalar().ToString();
                    ForFundingModel.ProposedItemID = Convert.ToInt32(ProposedItemID);
            }
            return PartialView("pvChangeProposedDate", ForFundingModel);
        }
        public PartialViewResult ReqChangeEffectivityDate(int? ProposedItemID)
        {
            ForFundingModel ForFundingModel = new ForFundingModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select AppointmentDateEffectivity from tbl_R_BMSProposedNewItem where ProposedItemID = 
                                                (select ProposedItemID from tbl_R_BMSSubmittedForFundingData where SeriesID = '" + ProposedItemID + "')", con);
                con.Open();
                ForFundingModel.SalaryEffectivityDate = com.ExecuteScalar().ToString();
                ForFundingModel.ProposedItemID = Convert.ToInt32(ProposedItemID);
            }
            return PartialView("pvChangeEffecivityDateProposed", ForFundingModel);
        }
        public PartialViewResult ReqChangeproposedDateVacant(int? ProposedItemID, string ActualDate)
        {
            ForFundingModel ForFundingModel = new ForFundingModel();
            ForFundingModel.SalaryEffectivityDate = ActualDate + " 00:00:00";
            ForFundingModel.ProposedItemID = Convert.ToInt32(ProposedItemID);

            return PartialView("pvChangeProposedDateVacant", ForFundingModel);
        }
         
             
        public PartialViewResult ViewApproveAccounts(int? BudgetYear, int? OfficeID, int? ProgramID)
        {
            Approved data = new Approved();
            data.BudgetYear = BudgetYear;
            data.OfficeID = OfficeID;
            data.ProgramIDparams = ProgramID;
            return PartialView("pvViewApproveAccounts", data);
        }
        public PartialViewResult ViewHistoryAmount(int? ProposalID, string AccountName, float ProposalAmount)
        {
            AmountHistory data = new AmountHistory();
            data.ProposalID = ProposalID;
            data.AccountName = AccountName;
            data.ProposalAmount = ProposalAmount;
            return PartialView("pvViewHistoryAmount", data);
        }
        public PartialViewResult FullScreenUI()
        {
            return PartialView("pvFullScreenUIApproval");
        }
        public PartialViewResult ViewSummaryOffices()
        {
            return PartialView("pvViewSummaryOffices");
        }

        public JsonResult BudgetInCharge_ApproveAccounts([DataSourceRequest] DataSourceRequest request, int? prog_ID, int? OOE, int? proposalYear)
        {
            OfficeAdmin_Layer pl = new OfficeAdmin_Layer();
            var lst = pl.BudgetInCharge_ApproveAccounts(prog_ID, OOE, proposalYear);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SelectAllOffice([DataSourceRequest] DataSourceRequest request, int? ProposalYear)
        {
            OfficeAdmin_Layer pl = new OfficeAdmin_Layer();
            var lst = pl.SelectAllOffice(ProposalYear);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //var json = Json(lst.ToDataSourceResult(request));
            //json.MaxJsonLength = int.MaxValue;
            //return json;
        }
        public JsonResult BudgetAccounts_Utilization([DataSourceRequest] DataSourceRequest request, int? ProgramID, int? ProposalYear, int? OfficeID, int? OOEID)
        {
            OfficeAdmin_Layer pl = new OfficeAdmin_Layer();
            var lst = pl.Utilization(ProgramID, ProposalYear, OfficeID, OOEID);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult BudgetCommittee_ApproveAccounts([DataSourceRequest] DataSourceRequest request, int? prog_ID, int? OOE, int? proposalYear, int? OfficeID,int? regularaipid)
        {
            var OfficeLevel = "";
            if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                OfficeLevel = "ProposalStatusHR";
            }
            else if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                OfficeLevel = "ProposalStatusIncharge";
            }
            else
            {
                OfficeLevel = "ProposalStatusCommittee";
            }
            OfficeAdmin_Layer pl = new OfficeAdmin_Layer();
            var lst = pl.BudgetCommittee_ApproveAccounts(prog_ID, OOE, proposalYear, OfficeID, OfficeLevel, regularaipid);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AccountHistoryAmount([DataSourceRequest] DataSourceRequest request, int? ProposalID)
        {
            OfficeAdmin_Layer data = new OfficeAdmin_Layer();
            var list = data.AccountHistoryAmount(ProposalID);
            return Json(list.ToDataSourceResult(request));
        }
        public JsonResult Accounts_read_Approved([DataSourceRequest] DataSourceRequest request, int? prog_ID, int? proy_ID, int? ooe_id)
        {
            OfficeAdmin_Layer pl = new OfficeAdmin_Layer();
            var lst = pl.OfficeApprovedAccounts(prog_ID, proy_ID, ooe_id);
            return Json(lst.ToDataSourceResult(request));
        }
        public JsonResult grGetProposedPosition([DataSourceRequest] DataSourceRequest request, int? YearOf, int? OfficeID, string isCasual)
        {
                OfficeAdmin_Layer pl = new OfficeAdmin_Layer();
                var lst = pl.grGetProposedPosition(YearOf, OfficeID, isCasual);
                return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            
        }
        public JsonResult grGetProposedVerifiedPosition([DataSourceRequest] DataSourceRequest request, int? YearOf, int? OfficeID)
        {
            OfficeAdmin_Layer pl = new OfficeAdmin_Layer();
            var lst = pl.grGetProposedVerifiedPosition(YearOf, OfficeID);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
         
        public JsonResult Accounts_read_forApproval([DataSourceRequest] DataSourceRequest request, int? prog_ID, int? proy_ID, int? ooe_id)
        {
            Program_Layer pl = new Program_Layer();
            var lst = pl.forApprovalAccounts(prog_ID, proy_ID, ooe_id);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Accounts_read_Cancelled([DataSourceRequest] DataSourceRequest request, int? prog_ID, int? proy_ID, int? ooe_id)
        {
            Program_Layer pl = new Program_Layer();
            var lst = pl.CancelledAccounts(prog_ID, proy_ID, ooe_id);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListOfNewAccounts([DataSourceRequest] DataSourceRequest request, int? prog_ID, int? proy_ID)
        {
            OfficeAdmin_Layer pl = new OfficeAdmin_Layer();
            var lst = pl.newAccount(prog_ID, proy_ID);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult _update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AccountsModel> Accounts)
        {
            Program_Layer el = new Program_Layer();
            try
            {
                el.UpdateAccount1(Accounts);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(Accounts.ToDataSourceResult(request, ModelState));
        }
        public JsonResult UpdateProposedAccount([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AccountsModel> ProposedAccounts)
        {
            HRBudget_Layer el = new HRBudget_Layer();
            try
            {
                el.UpdateProposedAccount(ProposedAccounts);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(ProposedAccounts.ToDataSourceResult(request, ModelState));
        }
        public JsonResult UpdateNewAccount([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<NewAccounts> Accounts)
        {
            OfficeAdmin_Layer el = new OfficeAdmin_Layer();
            try
            {
                el.UpdateNewAccounts(Accounts);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(Accounts.ToDataSourceResult(request, ModelState));
        }
        public JsonResult UpdateAmounts([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AccountsModel> Accounts)
        {
            Program_Layer amounts = new Program_Layer();
            amounts.UpdateCurrentAmounts(Accounts);
            return Json(Accounts.ToDataSourceResult(request, ModelState));
        }

        //this is for Budget user and HR user

        public ActionResult pv_Home4BdgtHR()
        { 
            return View("pv_Home4BdgtHR");
        }
        
        public JsonResult Programs_read2([DataSourceRequest] DataSourceRequest request, int? prog_ID, int? propYear, int? office_ID)
        {
            BGAPALayer el = new BGAPALayer();
            var lst = el.Programss(prog_ID, propYear, office_ID);

            return Json(lst.ToDataSourceResult(request));
        }

        
        public JsonResult _update2([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AccountsModel> Accounts)
        {
            BGAPALayer el = new BGAPALayer();
            try
            {
                el.UpdateAccount(Accounts);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(Accounts.ToDataSourceResult(request, ModelState));
        }
        public ActionResult LoadMenu()
        {
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            return PartialView("_menus", mnu);
        }
       


        //public JsonResult LoadChart([DataSourceRequest] DataSourceRequest request, int? OfficeID, int? ChartYear)


        public ActionResult TabChart(int? OfficeID, int? ChartYear)
        {
            Session["OfficeID"] = OfficeID;
            if (Convert.ToInt32(Session["OfficeID"]) == 0)
            {
                return PartialView("pv_selectOpis");
            }
            //else
            //if (OfficeID == 0) {

            //    return PartialView("pvTabChart");

            //}

            else
            {
                Charts_layer Menu = new Charts_layer();
                ViewBag.oppis = Menu.getAbbrivation(Convert.ToInt32(Session["OfficeID"]));
                IEnumerable<ChartModel> lst = Menu.new_Statistics(OfficeID, ChartYear);
                return PartialView("pvTabChart", lst);
                                                                                                             
            }
                                                    

        }
                              //       public ActionResult LoadChart(int? OfficeID, int? ChartYear)
                                                                       
                              //  {

                                   
                              //      if (Convert.ToInt32(Session["OfficeID"]) == 0)
                              //      {
                              //          return PartialView("_charts");
                              //      }
                              //      else
                              //      {
                              //          Charts_layer Menu = new Charts_layer();
                              //          ViewBag.oppis = Menu.getAbbrivation(Convert.ToInt32(Session["OfficeID"]));
                              //          IEnumerable<ChartModel> lst = Menu.Statistics(OfficeID, ChartYear);
                              //          return PartialView("_charts", lst);
                              //      }
                              //}


        public ActionResult LoadChartPA(int? OfficeID, int? ChartYear)
        {
            Session["OfficeID"] = OfficeID;
            Session["ChartYear"] = ChartYear;
            if (Convert.ToInt32(Session["OfficeID"]) == 0)
            {
                return PartialView("_chartPA");
            }
            else 
            {
                                             
                Charts_layer Menu = new Charts_layer();
                ViewBag.oppis = Menu.getAbbrivation(Convert.ToInt32(Session["OfficeID"]));
                IEnumerable<ChartModel> lst = Menu.StatisticsPA(OfficeID, ChartYear);
                return PartialView("_chartPA", lst);
            }
        }

        //public ActionResult BarChart()
        //{
        //    if (Convert.ToInt32(Session["OfficeID"]) == 0)
        //    {
        //        return PartialView("pvBarChart");
        //    }
        //    else
        //    {

        //        Charts_layer Menu = new Charts_layer();
        //        ViewBag.oppis = Menu.getAbbrivation(Convert.ToInt32(Session["OfficeID"]));
        //        IEnumerable<ChartModel> lst = Menu.StatisticsPAl();
        //        return PartialView("pvBarChart", lst);
        //    }
        //}   





        public ActionResult ApprovedTab()
        {
            return PartialView("pv_ApprovedTab");
        }
        public ActionResult ProposedPositionsTabOfficeLevel()
        {
            return PartialView("pv_OfficeLevelProposedPositions");
        }
        public ActionResult ApprovalTab()
        {
            return PartialView("pv_ApprovalTab");
        }
        public ActionResult CancelledTab()
        {
            return PartialView("pv_CancelledTab");
        }
        //public ActionResult CreateNewAccounts()
        //{
        //    //return ActionResult
        //}
        public ActionResult pv_Budget_ApprovedTab()
        {
            return PartialView("pv_Budget_ApprovedTab");
        }
        public ActionResult pv_Budget_ApprovalTab()
        {
            ViewBag.UserType = Account.UserInfo.UserTypeDesc;
            return PartialView("pv_Budget_ApprovalTab", ViewBag.UserType);
        }
        public ActionResult ShowUnverifiedPosition()
        {
            return PartialView("pvUnVerifiedPosition");
        }
        public ActionResult ShowVerifiedPosition()
        {
            return PartialView("pvVerifiedPosition");
        }
        public ActionResult pv_Budget_NewAccountsTab()
        {
            ViewBag.UserType = Account.UserInfo.UserTypeDesc;
            return PartialView("pv_Budget_NewAccountsTab", ViewBag.UserType);
        }

        public ActionResult pv_Budget_CancelledTab()
        {
            return PartialView("pv_Budget_CancelledTab");
        }
        public ActionResult AddCasual(int PlanYear)
        {
            ViewBag.OfficeID = 0;
            ViewBag.SeriesID = 0;
            ViewBag.CasualRateID = 0;
            ViewBag.EffectivityDate = 0;
            ViewBag.PlanYear = PlanYear;
            return PartialView("pv_AddCasual");
        }
        public ActionResult ReqEditCasual(int SeriesID, int CasualRateID, string EffectivityDate,int OfficeID)
        {
            ViewBag.OfficeID = OfficeID;
            ViewBag.SeriesID = SeriesID;
            ViewBag.CasualRateID = CasualRateID;
            ViewBag.EffectivityDate = EffectivityDate;
            return PartialView("pv_AddCasual");
        }
            
        public string getUserType()
        {
            return Account.UserInfo.UserTypeDesc;
        }
        public JsonResult BudgetAccounts_read_Approved([DataSourceRequest] DataSourceRequest request, int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id)
        {
            Session["selectedOfficeID"] = office_ID;
            if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
            Program_Layer pl = new Program_Layer();
                var lst = pl.BudgetApprovedAccountsBudgetInCharge(prog_ID, office_ID, proy_ID, ooe_id);
                return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            else if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                Program_Layer pl = new Program_Layer();
                var lst = pl.BudgetApprovedAccountsHRMOInCharge(prog_ID, office_ID, proy_ID, ooe_id);
                return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            else // for budget Committee
            {
                Program_Layer pl = new Program_Layer();
                var lst = pl.BudgetApprovedAccountsCommittee(prog_ID, office_ID, proy_ID, ooe_id);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        }
        public JsonResult BudgetInChargeProposedAccounts([DataSourceRequest] DataSourceRequest request, int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id)
        {
            Program_Layer pl = new Program_Layer();
            var lst = pl.BudgetInChargeProposedAccounts(prog_ID, office_ID, proy_ID, ooe_id);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getExistingAccounts([DataSourceRequest] DataSourceRequest request)
        {
            Program_Layer pl = new Program_Layer();
            var lst = pl.getExistingAccounts();
            var a = Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            a.MaxJsonLength =int.MaxValue;
            return (a);
        }
            
        public JsonResult BudgetAccounts_read_forApproval([DataSourceRequest] DataSourceRequest request, int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id, double? percent,int? regularaipid)
        {
            if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                Program_Layer pl = new Program_Layer();
                var lst = pl.BudgetforApprovalAccountsCommittee(prog_ID, office_ID, proy_ID, ooe_id, percent, regularaipid);
                return Json(lst.ToDataSourceResult(request),  JsonRequestBehavior.AllowGet);
            } 
            else if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
            Program_Layer pl = new Program_Layer();
                var lst = pl.BudgetforApprovalAccountsHRMOInCharge(prog_ID, office_ID, proy_ID, ooe_id);
                return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            else // for budget Committee
            {
                Program_Layer pl = new Program_Layer();
                var lst = pl.BudgetforApprovalAccountsCommittee(prog_ID, office_ID, proy_ID, ooe_id, percent, regularaipid);
                return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
          
        }
        public JsonResult BudgetAccounts_read_Cancelled([DataSourceRequest] DataSourceRequest request, int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id)
        {
            if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                Program_Layer pl = new Program_Layer();
                var lst = pl.BudgetCancelledAccountsBudgetInCharge(prog_ID, office_ID, proy_ID, ooe_id);
                return Json(lst.ToDataSourceResult(request));
            }
            else if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
            Program_Layer pl = new Program_Layer();
                var lst = pl.BudgetCancelledAccountsHRMOInCharge(prog_ID, office_ID, proy_ID, ooe_id);
                return Json(lst.ToDataSourceResult(request));
            }
            else // for budget Committee
            {
                Program_Layer pl = new Program_Layer();
                var lst = pl.BudgetCancelledAccountsCommittee(prog_ID, office_ID, proy_ID, ooe_id);
            return Json(lst.ToDataSourceResult(request));
            }
        }
        public JsonResult getProposedPositionsOfficeLevel([DataSourceRequest] DataSourceRequest request, int? Yearof)
        {
                OfficeAdmin_Layer layer = new OfficeAdmin_Layer();
                var lst = layer.getProposedPositionsOfficeLevel(Yearof);
                return Json(lst.ToDataSourceResult(request));
        }
            




         //_________________________________________________________________



        public JsonResult _newUpdate([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AccountsModel> Accounts)
        {
            Program_Layer el = new Program_Layer();
            try
            {
                el.newUpdateAccount(Accounts);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(Accounts.ToDataSourceResult(request, ModelState));
        }
        public String ApproveProposal(string[] ProposalID, string[] ProposedAmounts, string[] SlashAmount,int regularaipid)
        {
            if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                return HRbgtAP_Layer.ApproveProposalBudgetInCharge(ProposalID);
            }
            else if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                return HRbgtAP_Layer.ApproveProposalHRMOInCharge(ProposalID);
            }
            else // for budget Committee
            {
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                return HRbgtAP_Layer.ApproveProposalBudgetCommittee(ProposalID, ProposedAmounts, SlashAmount, regularaipid);
            }
        }
        public string ApproveAllAccounts(int YearOf,int regularaipid) 
        { 
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                return HRbgtAP_Layer.ApproveAllAccounts(YearOf, regularaipid);
        }
        public string ZimbraEmail(int? ProgramID, int? OfficeID, int? BudgetYear, int? ooe_id)
        {
            HRBudget_Layer data = new HRBudget_Layer();
            return data.ZimbraEmailNotif(ProgramID, OfficeID, BudgetYear, ooe_id);
        }
        public string ApproveNewAccount(int Yearof, int OfficeID, int AccountCode, string AccountName, double ApprovedAmount, int ProposalID, int ProgramID, int OOEID)
        {
            HRBudget_Layer data = new HRBudget_Layer();
            return data.ApproveNewAccount(Yearof, OfficeID, AccountCode, AccountName, ApprovedAmount, ProposalID, ProgramID, OOEID);
        }
            
        public ActionResult ShowRemarskWindow()
        {
            return PartialView("pv_RemarksWindow");
        }
        public ActionResult viewBuildNewAccountWindow(int Yearof, int OfficeID, int AccountCode, string AccountName, double ApprovedAmount, int ProposalID, int ProgramID, int OOEID)
        {
            AccountsModel model = new AccountsModel();
            model.ProposalYear = Yearof;
            model.SelectedOfficeID = OfficeID;
            model.AccountCode = AccountCode;
            model.AccountName = AccountName;
            model.SlashAmount = ApprovedAmount;
            model.ProposalID = ProposalID;
            model.ProgramID = ProgramID;
            model.OOEID = OOEID;


            return PartialView("pvBuildAndApproveNewAccountWindow", model);
        }
        public string BuildAndApproveExistingAccount(int Yearof, int OfficeID,double ApprovedAmount,int ProposalID,int ProgramID, int OOEID,int AccountID)
        {
            HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
            return HRbgtAP_Layer.BuildAndApproveExistingAccount(Yearof, OfficeID,ApprovedAmount,ProposalID,ProgramID,OOEID,AccountID);
        }
        public string BuildAndApproveAccount(int Yearof, int OfficeID, double ApprovedAmount, int ProposalID, int ProgramID, 
                                            int OOEID, string AccountName, int AccountCode , string ChildAccountCode,
                                            string ThirdLevelGroup,string FundType)
        {
            HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
            return HRbgtAP_Layer.BuildAndApproveAccount(Yearof, OfficeID, ApprovedAmount, ProposalID, ProgramID, OOEID, AccountName, AccountCode, ChildAccountCode, ThirdLevelGroup, FundType);
        }
         
        public String DeclineProposal(string[] ProposalIDList, string Remarks)
        {
            if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                return HRbgtAP_Layer.DeclineProposalBudgetInCharge(ProposalIDList, Remarks);
            }
            else if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                return HRbgtAP_Layer.DeclineProposalHRMOInCharge(ProposalIDList, Remarks);
            }
            else // for budget Committee
            {
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                return HRbgtAP_Layer.DeclineProposalBudgetCommittee(ProposalIDList, Remarks);
            }
        }
        public ActionResult ShowDetailsWindow(int ProposalID, int OfficeID, int Year, int ProgramID)
        {
            if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                var OfficeLevel = "ProposalStatusInCharge";
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                Cancelled CancelledDetails = HRbgtAP_Layer.CancelledDetails(ProposalID, OfficeID, Year, ProgramID, OfficeLevel);
                return PartialView("pv_ViewDetailsnonOfficeAdmin", CancelledDetails);
            }
            else if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                var OfficeLevel = "ProposalStatusHR";
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                Cancelled CancelledDetails = HRbgtAP_Layer.CancelledDetails(ProposalID, OfficeID, Year, ProgramID, OfficeLevel);
                return PartialView("pv_ViewDetailsnonOfficeAdmin", CancelledDetails);
            }
            else // for budget Committee
            {
                var OfficeLevel = "ProposalStatusCommittee";
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                Cancelled CancelledDetails = HRbgtAP_Layer.CancelledDetails(ProposalID, OfficeID, Year, ProgramID, OfficeLevel);
                return PartialView("pv_ViewDetailsnonOfficeAdmin", CancelledDetails);
            }

        }
        public string ReqUndoApproved(int ProposalID, int regularaipid)
        {
            if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                var OfficeLevel = "ProposalStatusInCharge";
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                return HRbgtAP_Layer.UndoActionApproved(ProposalID, OfficeLevel, regularaipid);
            }
            else if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                var OfficeLevel = "ProposalStatusHR";
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                return HRbgtAP_Layer.UndoActionApproved(ProposalID, OfficeLevel, regularaipid);
        }
            else // for budget Committee
            {
                var OfficeLevel = "ProposalStatusCommittee";
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                return HRbgtAP_Layer.UndoActionApproved(ProposalID, OfficeLevel, regularaipid);
            }
        }
        public string reqUndoCancelled(int ProposalID)
        {
            if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                var OfficeLevel = "ProposalStatusInCharge";
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                return HRbgtAP_Layer.UndoActionCancelled(ProposalID, OfficeLevel);
            }
            else if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                var OfficeLevel = "ProposalStatusHR";
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                return HRbgtAP_Layer.UndoActionCancelled(ProposalID, OfficeLevel);
            }
            else // for budget Committee
            {
                var OfficeLevel = "ProposalStatusCommittee";
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                return HRbgtAP_Layer.UndoActionCancelled(ProposalID, OfficeLevel);
            }
        }
        public String ApproveProposalNewAccounts(string[] ProposalID, string[] ProposedAmounts)
        {
                HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
                return HRbgtAP_Layer.ApproveProposalNewAccounts(ProposalID, ProposedAmounts);
        }
        public JsonResult BudgetAccounts_read_NewAccounts([DataSourceRequest] DataSourceRequest request, int? prog_ID, int? office_ID, int? proy_ID)
        {
                Program_Layer pl = new Program_Layer();
                var lst = pl.BudgetNewAccounts(prog_ID, office_ID, proy_ID);
                return Json(lst.ToDataSourceResult(request));
        }
        public PartialViewResult ViewDetails(int ProposalYear, double ProposalAmount, string Remarks, string AccountName, string OOE, string OfficeLevel, string OfficeName, string EmpName, int ProposalID, int ProgramID, int AccountID, int ProposalDenominationCode)
        {
            Cancelled viewDetails = new Cancelled();
            viewDetails.ProposalYear = ProposalYear;
            viewDetails.ProposalAmount = ProposalAmount;
            viewDetails.Remarks = Remarks;
            viewDetails.AccountName = AccountName;
            viewDetails.OOE = OOE;
            viewDetails.OfficeLevel = OfficeLevel;
            viewDetails.OfficeName = OfficeName;
            viewDetails.EmpName = EmpName;
            viewDetails.ProposalID = ProposalID;
            viewDetails.ProgramID = ProgramID;
            viewDetails.AccountID = AccountID;
            viewDetails.ProposalDenominationCode = ProposalDenominationCode;
            return PartialView("vwViewDetails", viewDetails);
        }
        public PartialViewResult CreateNewAccounts(int? pro_ID)
        {
            NewAccounts data = new NewAccounts();
            data.ProgramID = pro_ID;
            return PartialView("pvCreateNewAccounts", data);
        }
        public PartialViewResult pv_Utilization()
        {
            return PartialView("pv_Utilization");
        }

        public PartialViewResult Utilization()
        {
            return PartialView("pvUtilizationRate");
        }
        public string SaveNewProgram(double? Budget, int? ProposalID)
        {
            OfficeAdmin_Layer VIL = new OfficeAdmin_Layer();
            return VIL.UpdateAmount(Budget, ProposalID);
        }

        public String CheckIFApproved(string ProposalID)
        {
            HRbgtAP_Layer HRbgtAP_Layer = new HRbgtAP_Layer();
            return HRbgtAP_Layer.CheckifApproved(ProposalID);
        }
        public PartialViewResult MyAccount()
        {
            return PartialView("_MyAccount");
        }

        public PartialViewResult AddParticular(int AccountID)
        {
            AccountDenomination data = new AccountDenomination();
            data.AccountID = AccountID;
            return PartialView("pvAddParticular", data);
        }
        public PartialViewResult ViewPlantillaSummary()
        {
            return PartialView("pv_PSSummary");
        }
        public PartialViewResult pvbudgetHistory()
        {
            return PartialView("pvbudgetHistory");
        }
        public PartialViewResult UpdateAccount(int? ProposalID, int? AccountID, int? ProgramID, int? OOEID, string OfficeName, int? OfficeID, string AccountName, int? TransactionYear)
        {
            AccountsModel account_list = new AccountsModel();
            account_list.UpdateProposalAccount = ProposalID;
            account_list.UpdateAccountID = AccountID;
            account_list.UpdateProgramID = ProgramID;
            account_list.UpdateOfficeName = OfficeName;
            account_list.UpdateOfficeID = OfficeID;
            account_list.AccountName = AccountName;
            account_list.UpdateTransactionYear = TransactionYear;
            return PartialView("pvUpdateAccount", account_list);
        }
        public string MarkForCreation(int ProposedItemID, string Remark)
        {
            OfficeAdmin_Layer OfficeAdminLayer = new OfficeAdmin_Layer();
            return OfficeAdminLayer.MarkForCreation(ProposedItemID, Remark);
        }
        public string MarkForFunding(int ProposedItemID, string Remark)
        {
            OfficeAdmin_Layer OfficeAdminLayer = new OfficeAdmin_Layer();
            return OfficeAdminLayer.MarkForFunding(ProposedItemID, Remark);
        }
        public string MarkVerified(int ProposedItemID, string Remark)
        {
            OfficeAdmin_Layer OfficeAdminLayer = new OfficeAdmin_Layer();
            return OfficeAdminLayer.MarkVerified(ProposedItemID, Remark);
        }
        public string ApproveProposedPositions(int Yearof, int OfficeID)
        {
            OfficeAdmin_Layer OfficeAdminLayer = new OfficeAdmin_Layer();
            return OfficeAdminLayer.ApproveProposedPositions(Yearof, OfficeID);
        }

        public string MarkRemove(int ProposedItemID)
        {
            OfficeAdmin_Layer OfficeAdminLayer = new OfficeAdmin_Layer();
            return OfficeAdminLayer.MarkRemove(ProposedItemID);
        }
        public string GetProposedTotalAmount(int Yearof, int OfficeID)
        {
       
            OfficeAdmin_Layer OfficeAdminLayer = new OfficeAdmin_Layer();
            return "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", OfficeAdminLayer.GetProposedTotalAmount(Yearof, OfficeID));
        }
        public string GetProjectedTotalAmount(int Yearof, int OfficeID)
        {
            OfficeAdmin_Layer OfficeAdminLayer = new OfficeAdmin_Layer();
            return "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", OfficeAdminLayer.GetProjectedTotalAmount(Yearof, OfficeID));
        }
        public string GetDifference(int Yearof, int OfficeID)
        {
            OfficeAdmin_Layer OfficeAdminLayer = new OfficeAdmin_Layer();
            if (OfficeAdminLayer.GetProjectedTotalAmount(Yearof, OfficeID) - OfficeAdminLayer.GetProposedTotalAmount(Yearof, OfficeID) < 0)
            {
                return "₱ (" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", (OfficeAdminLayer.GetProjectedTotalAmount(Yearof, OfficeID) - OfficeAdminLayer.GetProposedTotalAmount(Yearof, OfficeID))*-1) +")";
            }
            else
            {
                return "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", OfficeAdminLayer.GetProjectedTotalAmount(Yearof, OfficeID) - OfficeAdminLayer.GetProposedTotalAmount(Yearof, OfficeID));
            }
        }
         
         
        public string MarkUnVerified(int ProposedItemID, string Remark)
        {
            OfficeAdmin_Layer OfficeAdminLayer = new OfficeAdmin_Layer();
            return OfficeAdminLayer.MarkUnVerified(ProposedItemID, Remark);
        }
        public string CheckIfVerifiedByOtherLevel(int ProposedItemID, string Remark)
        {
            OfficeAdmin_Layer OfficeAdminLayer = new OfficeAdmin_Layer();
            return OfficeAdminLayer.CheckIfVerifiedByOtherLevel(ProposedItemID, Remark);
        }
         
        public string UpdateProposedDate(string DateValue, int SeriesID, int OfficeID)
        {
            OfficeAdmin_Layer OfficeAdminLayer = new OfficeAdmin_Layer();
            return OfficeAdminLayer.UpdateProposedDate(DateValue, SeriesID, OfficeID);
        }
        public string UpdateProposedDateVacant(string DateValue, int SeriesID, int OfficeID, string isCasual)
        {
            OfficeAdmin_Layer OfficeAdminLayer = new OfficeAdmin_Layer();
            return OfficeAdminLayer.UpdateProposedDateVacant(DateValue, SeriesID, OfficeID, isCasual);
        }
        public string RemoveProposal(int? ProposalID = 0, int? office = 0, int? proyear = 0,int? supplementalaipid=0)
        {
            HRBudget_Layer HrBudgetlayer = new HRBudget_Layer();
            return HrBudgetlayer.RemoveProposal(ProposalID, office, proyear, supplementalaipid);
        }
        public JsonResult getPlantillaSummary([DataSourceRequest] DataSourceRequest request, int? YearOf)
        {
                Program_Layer pl = new Program_Layer();
                var lst = pl.getPlantillaSummary(YearOf);
                return Json(lst.ToDataSourceResult(request));
        }
        public JsonResult getProposedPositionSummary([DataSourceRequest] DataSourceRequest request, int? YearOf)
        {
            Program_Layer pl = new Program_Layer();
            var lst = pl.getProposedPositionSummary(YearOf);
            return Json(lst.ToDataSourceResult(request));
        }
        public ActionResult PlantillaSummaryTab()
        {
            return PartialView("pvPlantillaSummary");
        }
        public ActionResult ProposedPositionsTab()
        {
            return PartialView("pvProposedPositionsSummary");
        }
        public string GetAccountsToCombine()
        {
            OfficeAdmin_Layer OfficeAdminLayer = new OfficeAdmin_Layer();
            return OfficeAdminLayer.GetAccountsToCombine();
        }
        public double GetProposedTotalAmountAll(int? Yearof=0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select dbo.fn_BMS_ProposalTotalAmount (" + Yearof + ")", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }
        public double GetProposedTotalAmountNonOffice(int? Yearof = 0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_PieChartNonOfficeTotal " + Yearof + "", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }
        public double GetProposedTotalAmountAllConsol(int? Yearof = 0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select dbo.fn_BMS_PsMooeCoConsolidated (" + Yearof + ")", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }
        public double TotalFilledVacantPosTotal(int? Yearof = 0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_PieChartFilledVacantPos " + Yearof + ",2", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }
        public double TotalAppropriation(int? Yearof = 0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_bms_appropriationtotal " + Yearof + ",12", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }
        public string generateHash()
        {
            return FUNCTION.GeneratePISControl();
        }
        public PartialViewResult UpdatePPSAS(int? ProposalID, string AccountName)
        {
            Session["ProposalID"] = ProposalID;
            Session["AccountName"] = AccountName;
            return PartialView("pvPPSASupdate");
        }
        public ActionResult Getppsasaccount([DataSourceRequest]DataSourceRequest request)
        {
            string tempStr = "";
            tempStr = "SELECT [ChartAccountID],[AccountName] FROM [fmis].[Accounting].[tbl_l_ChartOfAccountsParent] where levelno = 5 ";
            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public string ppsasupdate(long ProposalID,long ppsasid,string ppsasname, int BudgetYear,string budgetaccount)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.sp_bms_ppsasupdte " + ProposalID + ","+ ppsasid + ",'"+ ppsasname + "'," + Account.UserInfo.eid.ToString() + "," + BudgetYear + ",'"+ budgetaccount.Replace("'","''").ToString() + "'", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)    
            {
                return ex.Message;
            }
        }
        public string resetppsasaccount(long ProposalID)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.sp_bms_ppsasreset " + ProposalID + "," + Account.UserInfo.eid.ToString() + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        
        public JsonResult OutgoingQR(string qr = "")
        {

            BudgetControlModel data = new BudgetControlModel();
            DataTable _dt = new DataTable();
            string _sqlQuery = "Select * from fn_BMS_OutgoingQR ('" + qr + "')";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.transno = Convert.ToInt64(_dt.Rows[0][0].ToString());
                data.OBRNo = _dt.Rows[0][2].ToString();
                data.Amount = Convert.ToDecimal(_dt.Rows[0][3].ToString());
                data.particular = _dt.Rows[0][4].ToString();
                data.datetimein = _dt.Rows[0][5].ToString();
                data.datetimeout = _dt.Rows[0][6].ToString();
                data.trnno_id = Convert.ToInt32(_dt.Rows[0][7].ToString());
                data.transmode= Convert.ToInt32(_dt.Rows[0][8].ToString());
                data.datetimeverified = Convert.ToString(_dt.Rows[0][9].ToString());
                data.type = "success";
            }
            catch (Exception ex)
            {
                data.OfficeName = "Error";
                data.message = ex.Message;
                data.type = "error";
                data.Amount = 0;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GridbyProgram([DataSourceRequest] DataSourceRequest request, int? offid = 0,int? proYear=0)
        {

            List<BudgetControlModel> prog = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT [ProgramID],[ProgramDescription] FROM [IFMIS].[dbo].[tbl_R_BMSOfficePrograms] where officeid="+ offid + " and programyear="+ proYear + " and ActionCode=1 order by ProgramDescription", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    BudgetControlModel list = new BudgetControlModel();
                    list.ProgramID = Convert.ToInt32(reader.GetValue(0));
                    list.programname = reader.GetValue(1).ToString();
                    prog.Add(list);
                }
            }
            return Json(prog.ToDataSourceResult(request));
        }
        public JsonResult GridbyProgramAccount([DataSourceRequest] DataSourceRequest request, int? ProgramID = 0)
        {
            List<BudgetControlModel> prog = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT [ProgramID],[AccountID],[AccountName] FROM [IFMIS].[dbo].[tbl_R_BMSProgramAccounts] where [ProgramID]=" + ProgramID + " and [AccountYear]=2021 and ActionCode=1 order by [AccountName]", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    BudgetControlModel list = new BudgetControlModel();
                    list.ProgramID = Convert.ToInt32(reader.GetValue(0));
                    list.AccountID = Convert.ToInt32(reader.GetValue(1));
                    list.AccountName = reader.GetValue(2).ToString();
                    prog.Add(list);
                }
            }
            return Json(prog.ToDataSourceResult(request));
        }
        public PartialViewResult UpdatePlantillaSchedule(int? ProgramID=0,int? AccountID=0,int? proyear=0,double ProposalAmmount=0)
        {
            Session["ProgramID"] = ProgramID;
            Session["AccountID"] = AccountID;
            Session["proyear"] = proyear;
            Session["ProposalAmmount"] = ProposalAmmount;
            return PartialView("pvPlantillaSchedule");
        }
        public ActionResult getposition([DataSourceRequest]DataSourceRequest request)
        {
            string tempStr = "select Pos_Name,PositionCode from pmis.dbo.RefsPositions where isnull(sg,0) <> 0 order by Pos_Name ";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public string insertscheduledonom(int? progid=0, int? accountid=0, int? propyear=0, int? position=0, double monsalary=0.00,string datepicker="",int? noOfPosition=0,int? status=0)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"sp_BMS_JOCOSscheduling "+ progid + ","+ accountid + ","+ propyear + ","+ position + ","+ monsalary + ",'"+ datepicker + "'," + Account.UserInfo.eid.ToString() + ","+ noOfPosition + ","+ status + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public double getsalarybyposition(int? position = 0,int? proyear=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = 0.00;
                    SqlCommand com = new SqlCommand(@"select dbo.[fn_BMS_SalaryPerPosition] ("+ position + ","+ proyear + ")", con);
                    con.Open();
                    data = Convert.ToDouble(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public JsonResult Plantilla_JOS([DataSourceRequest]DataSourceRequest request, int? programid = 0, int? accountid = 0, int? yearof=0)
        {

            List<BudgetControlModel> prog = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec[sp_BMS_PlantillaJOCOSschedule] " + programid + "," + accountid + "," + yearof + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    BudgetControlModel emp = new BudgetControlModel();
                    emp.scheduleid = Convert.ToInt32(reader.GetValue(0));
                    emp.positionid = Convert.ToInt32(reader.GetValue(1));
                    emp.posname = reader.GetValue(2).ToString();
                    emp.sg = Convert.ToInt32(reader.GetValue(3));
                    emp.appointmentdate =reader.GetValue(4).ToString();
                    emp.Salary = Convert.ToDouble(reader.GetValue(5));
                    emp.noOfPersonnel = Convert.ToInt32(reader.GetValue(6));
                    emp.programidplantilla = Convert.ToInt32(reader.GetValue(7));
                    emp.accountidplantilla = Convert.ToInt32(reader.GetValue(8));
                    emp.totalSalary= Convert.ToDouble(reader.GetValue(9));
                    prog.Add(emp);
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public double TotalBreakdown( int? proyear=0,int? programid=0, int? accountid=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = 0.00;
                    SqlCommand com = new SqlCommand(@"select sum(totalSalary)  from tbl_T_BMSPlantillaSchedule where programid="+ programid + " and accountid="+ accountid + " and yearof="+ proyear + " and actioncode=1", con);
                    con.Open();
                    data = Convert.ToDouble(com.ExecuteScalar());
                    return data;
                }
            }
            catch //(Exception ex)
            {
                return 0;
            }
        }
        public string RemoveBreakdown(int? id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"update ifmis.dbo.tbl_T_BMSPlantillaSchedule set actioncode=4,usereid=usereid +','+ '"+ Account.UserInfo.eid.ToString() +"' where schedid=" + id +"", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch //(Exception ex)
            {
                return "error";
            }
        }
        public PartialViewResult utilization_summary(int? officeid=0,int? proyear=0)
        {
            Session["officeid"] = officeid;
            Session["proyear"] = proyear;
            //Session["proyear"] = proyear;
            //Session["ProposalAmmount"] = ProposalAmmount;
            return PartialView("pv_utilizationsummary");
        }
        public JsonResult proposal_utilizationsum([DataSourceRequest]DataSourceRequest request, int? officeid = 0, int? proyear = 0)
        {

            List<BudgetControlModel> prog = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec[sp_BMS_ProposalUtilization] " + officeid + "," + proyear + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    BudgetControlModel emp = new BudgetControlModel();
                    emp.OfficeID = Convert.ToInt32(reader.GetValue(0));
                    emp.OOE_Name =reader.GetValue(2).ToString();
                    emp.ooeid_uti = Convert.ToDouble(reader.GetValue(3));
                    emp.app1 = Convert.ToDecimal(reader.GetValue(4));
                    emp.allot1 = Convert.ToDecimal(reader.GetValue(5));
                    emp.obligate1 = Convert.ToDecimal(reader.GetValue(6));
                    emp.app_per1 = Convert.ToDecimal(reader.GetValue(7));
                    emp.allot_per1 = Convert.ToDecimal(reader.GetValue(8));
                    emp.app2 = Convert.ToDecimal(reader.GetValue(9));
                    emp.allot2 = Convert.ToDecimal(reader.GetValue(10));
                    emp.obligate2 = Convert.ToDecimal(reader.GetValue(11));
                    emp.app_per2 = Convert.ToDecimal(reader.GetValue(12));
                    emp.allot_per2 = Convert.ToDecimal(reader.GetValue(13));
                    emp.app3 = Convert.ToDecimal(reader.GetValue(14));
                    emp.allot3 = Convert.ToDecimal(reader.GetValue(15));
                    emp.obligate3 = Convert.ToDecimal(reader.GetValue(16));
                    emp.app_per3 = Convert.ToDecimal(reader.GetValue(17));
                    emp.allot_per3 = Convert.ToDecimal(reader.GetValue(18));
                  
                    prog.Add(emp);
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public PartialViewResult utilization_summary_detail(int? officeid = 0, int? proyear = 0,double ooeid=0.00)
        {
            Session["officeid"] = officeid;
            Session["proyear"] = proyear;
            Session["ooeid"] = ooeid;
         
            return PartialView("pv_utilizationsummary_detail");
        }
        public string resetproposerecord(int? officeid=0 , int? yearof=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"update  [IFMIS].[dbo].[tbl_T_BMSProposalAmount] set [actioncode]=2 where [yearof]=" + yearof + " and [officeid] in (Select PMISOfficeID from ifmis.dbo.tbl_R_BMSOffices where OfficeID=" + officeid + ")", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch //(Exception ex)
            {
                return "error";
            }
        }
        public string resetooerecord(int? officeid = 0, int? yearof = 0,int? ooe=0,int? programid=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec sp_BMS_ProposalResetOOE "+ programid + ","+ ooe + ","+ yearof + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Source;
            }
        }
        public int GenerateSAAO(int? YearOf = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = 0;
                    SqlCommand com = new SqlCommand(@"exec ifmis.dbo.sp_bms_Update_tbl_R_BMSCurrentYearAppropriations " + YearOf + "", con);
                    con.Open();
                    data = Convert.ToInt32(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public PartialViewResult AddProcurementitem(int? office = 0, int? program = 0, int? account = 0, int? tyear = 0, int? fundid = 0, int? mode_trans = 0, int? ooeclass = 0)
        {
            Session["office"] = office;
            Session["program"] = program;
            Session["account"] = account;
            Session["tyear"] = tyear;
            Session["fundid"] = fundid;
            Session["mode_trans"] = mode_trans;
            Session["ooeclass"] = ooeclass;
            return PartialView("pvAddProposalProcurement");
        }
        public JsonResult GetPPPMPitem_Proc([DataSourceRequest]DataSourceRequest request, int? office = 0, int? program = 0, int? account = 0, int? tyear = 0, int? fundid = 0, int? mode_trans = 0, int? ooeclass = 0,int? expendableid=0)
        {
            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_Proposalppmpitem " + office + "," + program + "," + account + "," + tyear + "," + ooeclass + "," + Account.UserInfo.eid + ","+ expendableid + "", con);
                con.Open();
                com.CommandTimeout = 0;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare loc = new WFPrepare();
                    loc.itemid = Convert.ToInt32(reader.GetValue(0));
                    loc.itemname = reader.GetValue(1).ToString().Replace("'", "''").ToString();
                    loc.unitcost = Convert.ToDouble(reader.GetValue(2));
                    loc.unit = Convert.ToString(reader.GetValue(3));
                    prog.Add(loc);
                }
            }

            var jsonResult = Json(prog.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public string additemepropose(string[] transno, int? officeid = 0, int? programid = 0, int? accountid = 0, int? tyear = 0, int? fundid = 0, int? mode_trans = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                var idx = 0;
                foreach (var trnno in transno)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = transno[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("dbo.[sp_BMS_ProposeItemADD]", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@trnno", dt));
                    com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                    com.Parameters.Add(new SqlParameter("@officeid", officeid));
                    com.Parameters.Add(new SqlParameter("@programid", programid));
                    com.Parameters.Add(new SqlParameter("@accountid", accountid));
                    com.Parameters.Add(new SqlParameter("@year", tyear));
                    con.Open();

                    return com.ExecuteScalar().ToString();
                }
                    
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
