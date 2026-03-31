using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
using iFMIS_BMS.BusinessLayer.Layers.Maintenance;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;
using iFMIS_BMS.Base;
using System.Configuration;
using iFMIS_BMS.BusinessLayer.Models.Budget_Preparation;

namespace iFMIS_BMS.Controllers
{
    [Authorize]
    public class BudgetPreparationController : Controller
    {
        // GET: BudgetPreparationweqweqweqw
        [Authorize]
        public ActionResult Home()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 12024)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select [EnableSubmit] from [IFMIS].[dbo].[tbl_R_BMSubmitbtn] ", con);
                    con.Open();
                    Session["EnableSubmitOA"] = Convert.ToBoolean(com.ExecuteScalar().ToString());
                    con.Close();
                }
                if (Account.UserInfo.UserTypeDesc == "Budget In-Charge")
                {
                    return View("OfficeAdmin");
                }
                else
                {
                    //  return RedirectToAction("pv_Home", "Annual");
                    return View("BudgetCommittee");
                }
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        [HttpGet]
        public PartialViewResult OfficeAccounts(int ProgramID, int officeID,int yearof)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select [EnableSubmit] from [IFMIS].[dbo].[tbl_R_BMSubmitbtn] ", con);
                con.Open();
                Session["EnableSubmit"] = Convert.ToBoolean(com.ExecuteScalar().ToString());
                con.Close();

                SqlCommand com2 = new SqlCommand(@"select [ProgramDescription] from ifmis.dbo.[tbl_R_BMSOfficePrograms] where actioncode=1 and [ProgramID]="+ ProgramID + " and [ProgramYear]="+ yearof + "", con);
                con.Open();
                Session["ProgramDescription"] = Convert.ToString(com2.ExecuteScalar());
                con.Close();

            }
            
            Session["ProgID1"] = ProgramID;
            Session["selectedOfficeID"] = officeID;

            return PartialView("pvOfficePanelTabStrip");
        }
        [HttpGet]
        public PartialViewResult OfficeAccountPanels()
        {
            return PartialView("pvOfficePanelAccounts");
        }
        [HttpGet]
        public PartialViewResult ApprovalAccountPanels()
        {
            return PartialView("pvOfficeApprovalAccounts");
        }
        [HttpGet]
        public PartialViewResult BudgetAccounts(int ProgramID)
        {
            Session["ProgID1"] = ProgramID;
            return PartialView("pvOfficePanelTabStrip");
            // return PartialView("pvBudgetPanelAccounts");
        }
        public PartialViewResult newAccounts(int? ooe_id)
        {
            return PartialView("pvNewAccounts");
        }
        public PartialViewResult ShowTransferToOtherOfficeWindow(int EmployeeID)
        {
            Session["EmployeeID"] = EmployeeID;
            return PartialView("pvTransferOffice");
        }

        public PartialViewResult viewComputationDetails(int AccountID, int OfficeID, int refYear, int ProposalYear)
        {
            Session["ProposalYear"] = ProposalYear;
            Session["selectedOfficeID"] = OfficeID;
            Session["selectedYear"] = refYear;
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            return PartialView("pvComputationDetails", OfficeAdmin_Layer.getAccountComputation(AccountID, OfficeID, refYear));
        }
        public PartialViewResult viewSalaryAndWagesComputation(string isCasual, int AccountID, int OfficeID, int refYear, int ProposalYear)
        {
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            Session["isCasual"] = isCasual;
            Session["ProposalYear"] = ProposalYear;
            Session["selectedOfficeID"] = OfficeID;
            Session["selectedYear"] = refYear;
            return PartialView("pvSalariesAndWagesComputation", OfficeAdmin_Layer.getAccountComputation(AccountID, OfficeID, refYear));
        }
        public PartialViewResult ReqAddCasual()
        {
            return PartialView("pv_AddNewCasual");
        }

        public PartialViewResult ReqTransferCasual(int OfficeID)
        {
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            var OfficeData = OfficeAdmin_Layer.getOfficeData(OfficeID);
            return PartialView("pvTransferCasual",OfficeData);
        }
        public PartialViewResult viewForFundingDetails(int AccountID)
        {
            @ViewBag.AccountID = AccountID;
            return PartialView("pvForFundingDetails");
        }
        public PartialViewResult viewFormulaDetails(AccountComputationModel AccountComputationModel)
        {
            return PartialView("pvFormulaDetail", AccountComputationModel);
        }
        #region Office Admin Process
        public string CheckComputationDetails(int AccountID, int refYear)
        {
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            return OfficeAdmin_Layer.CheckAccountComputation(AccountID,refYear);
        }
        public int getrefYear(int ProposalID)
        {
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            return OfficeAdmin_Layer.getrefYear(ProposalID);
        }
        public JsonResult SearchOfficeProgram_OfficeAdmin([DataSourceRequest] DataSourceRequest request, int? propYear1)
        {
            OfficeAdmin_Layer el = new OfficeAdmin_Layer();
            var lst = el.grSearchOfficeProgram(propYear1);
            return Json(lst.ToDataSourceResult(request));
        }
        public string CopyPreviousAmount(double? PreviousAmount, int? AccountID, int? ProgramID, int? ProposalYear, int OfficeID, string AccountName)
        {
            OfficeAdmin_Layer VIL = new OfficeAdmin_Layer();
            return VIL.UpdateCopyPreviousAmount(PreviousAmount, AccountID, ProgramID, ProposalYear,OfficeID, AccountName);
        }
        public string SubmitProposal(double? Amount, int? AccountID, int? ProgramID, int? ProposalYear, int OfficeID,int regularaipid)
        {
            OfficeAdmin_Layer VIL = new OfficeAdmin_Layer();
            return VIL.SubmitProposal(Amount, AccountID, ProgramID, ProposalYear, OfficeID, regularaipid);
        }
        public string SubmitProposalForHR(int? ProposalYear, int OfficeID)
        {
            OfficeAdmin_Layer VIL = new OfficeAdmin_Layer();
            return VIL.SubmitProposalForHR(ProposalYear, OfficeID);
        }
        public string GetComputationDescription(int OfficeID)
        {
            OfficeAdmin_Layer VIL = new OfficeAdmin_Layer();
            return VIL.GetComputationDescription(OfficeID);
        }
        public JsonResult grOfficeAccounts([DataSourceRequest] DataSourceRequest request, int? propYear1, int? officeID,int? ProgramID, int? suppletag)
        {
            OfficeAdmin_Layer el = new OfficeAdmin_Layer();
            var lst = el.grOfficeProgram(Convert.ToInt32(Session["ProgID1"]), propYear1, officeID, ProgramID, suppletag);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult grGetCasualToTransfer([DataSourceRequest] DataSourceRequest request, int? PMISOfficeID, int? IFMISOfficeID)
        {
            OfficeAdmin_Layer el = new OfficeAdmin_Layer();
            var lst = el.grGetCasualToTransfer(PMISOfficeID, IFMISOfficeID);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string TransferSelectedCasual(int? OfficeID, int? eid)
        {
            OfficeAdmin_Layer el = new OfficeAdmin_Layer();
            return el.TransferSelectedCasual(OfficeID, eid);
        }

        public JsonResult grGetComputationDetails([DataSourceRequest] DataSourceRequest request, int AccountCode, int OfficeID, int PropYear, int AccountID)
        {
            OfficeAdmin_Layer el = new OfficeAdmin_Layer();
            var lst = el.grGetComputationDetails(AccountCode, OfficeID, PropYear, AccountID);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult grGetComputationDetailsAdminView([DataSourceRequest] DataSourceRequest request, int OfficeID, int PropYear, string isCasual)
        {
            OfficeAdmin_Layer el = new OfficeAdmin_Layer();
            var lst = el.grGetComputationDetailsAdminView(OfficeID, PropYear, isCasual);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public decimal GetComputationOfAccount(int AccountCode, int OfficeID, int RefYear)
        {
            OfficeAdmin_Layer el = new OfficeAdmin_Layer();
            return el.setProposalAllotedAmount(AccountCode, OfficeID, RefYear, 0, 0, 0);
        }
        public JsonResult grGetForFundingDetails([DataSourceRequest] DataSourceRequest request, int? ProposalYear, int? officeID, int AccountID)
        {
                OfficeAdmin_Layer el = new OfficeAdmin_Layer();
                var lst = el.grGetForFundingDetails(ProposalYear, officeID);
                return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            
        }
        public JsonResult UpdateOffice_OfficeAdmin([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AccountsModel> Accounts)
        {
            EditHome2_Layer el = new EditHome2_Layer();
            try
            {
                el.UpdateAccountHome2(Accounts);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(Accounts.ToDataSourceResult(request, ModelState));
        }
        public JsonResult UpdatQuantity([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AccountDenomination> Denomination)
        {
            HRBudget_Layer DenominationList = new HRBudget_Layer();
            try
            {
                DenominationList.UpdateDenomination(Denomination);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(Denomination.ToDataSourceResult(request, ModelState));
        }
        public string CopyAllAmount(int ProgramID, int Year, int OfficeID)
        {
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            return OfficeAdmin_Layer.CopyAllAmounts(ProgramID, Year, OfficeID);
        }
        public JsonResult grEmployeeSalaryData([DataSourceRequest] DataSourceRequest request, int AccountCode, int officeID, int refYear)
        {
            OfficeAdmin_Layer el = new OfficeAdmin_Layer();
            var lst = el.getComputationDetails(AccountCode, officeID, refYear);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        #endregion

        // HR and BUDGET Process
        #region HR and BUDGET Process
        public JsonResult SearchOfficeProgram_HRBudget([DataSourceRequest] DataSourceRequest request, int? off_ID, int? propYear)
        {
            HRBudget_Layer el = new HRBudget_Layer();
            var lst = el.grSearchOfficeProgram(off_ID, propYear);
            return Json(lst.ToDataSourceResult(request));
        }
        public ActionResult UpdateOfficeAccounts([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AccountsModel> ABAccounts)
        {
            HRBudget_Layer el = new HRBudget_Layer();
            try
            {
                el.UpdateSelectedOfficeAccount(ABAccounts);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(ABAccounts.ToDataSourceResult(request, ModelState));
        }
 
        #endregion
        public JsonResult Proposal_AccountDenomination([DataSourceRequest] DataSourceRequest request, int? ProgramID, int? AccountID, int? ProposalYear, int? OfficeID,int? suppletag)
        {
            OfficeAdmin_Layer program_list = new OfficeAdmin_Layer();
            var program_lst = program_list.grAccountDenomination(ProgramID, AccountID, ProposalYear, OfficeID, suppletag);
            return Json(program_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Proposal_AccountDenominationBudgetApproval([DataSourceRequest] DataSourceRequest request, int? ProgramID, int? AccountID, int? ProposalYear, int? OfficeID,int? aipversion)
        {
            OfficeAdmin_Layer program_list = new OfficeAdmin_Layer();
            var program_lst = program_list.grAccountDenominationBudgetApproval(ProgramID, AccountID, ProposalYear, OfficeID, aipversion);
            return Json(program_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult NewAccountDenomination(int? ProgramID, int? AccountID, int? ProposalYear, int? OfficeID)
        {
            AccountDenomination DenominationInfo = new AccountDenomination();
            DenominationInfo.OfficeID = OfficeID;
            DenominationInfo.ProgramID = ProgramID;
            DenominationInfo.AccountID = AccountID;
            DenominationInfo.ProposalYear = ProposalYear;

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand query = new SqlCommand(@"Select isnull(Amount,0),isnull(Month,0) from tbl_R_BMSOtherComputations where AccountID = " + AccountID + "", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    DenominationInfo.DenominationAmount = Convert.ToDecimal(reader.GetValue(0));
                    DenominationInfo.DenominationMonth = Convert.ToDecimal(reader.GetValue(1));
                }
            }
            if ( DenominationInfo.DenominationMonth == 0 && DenominationInfo.DenominationAmount == 0)
            {
                DenominationInfo.DenominationMonth = 1;
                DenominationInfo.ActionCode = 1;
            }
            else
            {
                DenominationInfo.ActionCode = 0;
            }
            
            return PartialView("pvNewAccountDenomination", DenominationInfo);
        }

        public string SaveNewAccountDenomination(AccountDenomination DenominationInfo)
        {
            AccountDenomination_Layer VIL = new AccountDenomination_Layer();
            return VIL.SaveAccountDenomination(DenominationInfo);
        }
        public string SaveNewAccountDenominationLFC(AccountDenomination DenominationInfo)
        {
            AccountDenomination_Layer VIL = new AccountDenomination_Layer();
            return VIL.SaveAccountDenominationLFC(DenominationInfo);
        }
        public string DeleteAccountDenomination(int? AccountDenominationID, int OfficeID, double Amount)
        {
            AccountDenomination_Layer VIL = new AccountDenomination_Layer();
            return VIL.DeleteAccountDenomination(AccountDenominationID, OfficeID, Amount);
        }
        public string UpdateAccountDenominationData(AccountDenomination UpdateInfo)
        {
            AccountDenomination_Layer VIL = new AccountDenomination_Layer();
            return VIL.UpdateAccountDenominationData(UpdateInfo);
        }
        public PartialViewResult EditData(int? AccountDenominationID, string DenominationName, decimal DenominationAmount, int? AccountID, decimal TotalAmount, double QuantityPercentage, double DenominationMonth,int isPPMP)
        {
            UpdateAccountDenomination editData = new UpdateAccountDenomination();
            editData.AccountDenominationID = AccountDenominationID;
            editData.DenominationName = DenominationName;
            editData.DenominationAmount = DenominationAmount;
            editData.AccountID = AccountID;
            editData.TotalAmount = TotalAmount;
            editData.QuantityPercentage = QuantityPercentage;
            editData.DenominationMonth = DenominationMonth;
            editData.isPPMP = isPPMP;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand query = new SqlCommand(@"Select isnull(Amount,0),isnull(Month,0) from tbl_R_BMSOtherComputations where AccountID = " + AccountID + "", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    editData.DenominationAmount = Convert.ToDecimal(reader.GetValue(0));
                    editData.DenominationMonth = Convert.ToDouble(reader.GetValue(1));

                }
            }
            if (editData.DenominationMonth == 0 && editData.DenominationAmount == 0)
            {
                editData.ActionCode = 1;
            }
            else
            {
                editData.ActionCode = 0;
            }
            return PartialView("pvEditData", editData);
        }
        public string SaveDenomination(int? AccountID, int? ProgramID, int? ProposalYear, int? ProposalID, string OOEName)
        {
            AccountDenomination_Layer VIL = new AccountDenomination_Layer();
            return VIL.SaveDenomination(AccountID, ProgramID, ProposalYear, ProposalID, OOEName);
        }
        public string UpdateTotalAmount(int AccountID,int ProgramID,int OfficeID, int ProposalYear)
        {
            AccountDenomination_Layer VIL = new AccountDenomination_Layer();
            return VIL.UpdateTotalAmount(AccountID, ProgramID, OfficeID, ProposalYear);
        }
        public string UpdateAccountDenomination(int? AccountID, int? ProgramID, int? ProposalYear, int? ProposalID)
        {
            AccountDenomination_Layer VIL = new AccountDenomination_Layer();
            return VIL.UpdateAccountDenomination(AccountID, ProgramID, ProposalYear, ProposalID);
        }
        public string UpdateProgramAccount(int? AccountID, int? ProgramID, int? ProposalYear, int? ProposalID, string OfficeLevel)
        {
            AccountDenomination_Layer VIL = new AccountDenomination_Layer();
            return VIL.UpdateProgramAccount(AccountID, ProgramID, ProposalYear, ProposalID, OfficeLevel);
        }
        public string UpdateDenomination(int? AccountID, int? ProgramID, int? ProposalYear, int? ProposalID)
        {
            AccountDenomination_Layer VIL = new AccountDenomination_Layer();
            return VIL.UpdateDenomination(AccountID, ProgramID, ProposalYear, ProposalID);
        }
        public string UpdateBudgetDenomination(int? AccountID, int? ProgramID, int? ProposalYear, int? ProposalID)
        {
            AccountDenomination_Layer VIL = new AccountDenomination_Layer();
            return VIL.UpdateBudgetDenomination(AccountID, ProgramID, ProposalYear, ProposalID);
        }
        public string TransferOffice(int? eid, int? selectedYear, int officeID, string OfficeName, int CurrentOfficeID, int ProposalYear)
        {
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            if (OfficeAdmin_Layer.UpdateEmployeeOffice(eid, selectedYear, officeID, OfficeName) == "1")
            {
                if (OfficeAdmin_Layer.updateAccount_NewAmount(eid, selectedYear, CurrentOfficeID, ProposalYear) == "1")
                {
                    return OfficeAdmin_Layer.updateAccount_NewAmount(eid, selectedYear, officeID, ProposalYear);    
                }
                else
                {
                    return OfficeAdmin_Layer.updateAccount_NewAmount(eid, selectedYear, CurrentOfficeID, ProposalYear);
                }
                   
            }
            else
            {
                return OfficeAdmin_Layer.UpdateEmployeeOffice(eid, selectedYear, officeID, OfficeName);
            }
            
        }
        public string UpdatePPMPData(int ProposalYearParam, int OfficeDataIDParam)
        {
            PPMPdata_Layer getPPMP = new PPMPdata_Layer();
            return getPPMP.UpdatePPMPAmount(ProposalYearParam, OfficeDataIDParam);
        }
        public string RemoveProposal(int? AccountID, int? ProgramID, int? ProposalYear) 
        {
            OfficeAdmin_Layer delete = new OfficeAdmin_Layer();
            return delete.RemoveProposal(AccountID, ProgramID, ProposalYear);
        }
        public string SubmitAllNonOffice(int? ProposalYearParam)
        {
            OfficeAdmin_Layer OfficeAdminLayer = new OfficeAdmin_Layer();
            return OfficeAdminLayer.SubmitAllNonOffice(ProposalYearParam);
        }
        public ActionResult PrepareAIP()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 12025)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                return View("pvPrepare_AIP_Index");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
       
        public JsonResult ddlAddAIP_FundingSource([DataSourceRequest] DataSourceRequest request)
        {
            AIPPreparationLayer Layer = new AIPPreparationLayer();
            var lst = Layer.ddlAddAIP_FundingSource();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlAIPActivityDescription([DataSourceRequest] DataSourceRequest request, int OfficeID)
        {
            AIPPreparationLayer Layer = new AIPPreparationLayer();
            var lst = Layer.ddlAIPActivityDescription(OfficeID);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlAddAIP_ProgramActivities_Main_Data([DataSourceRequest] DataSourceRequest request, int OfficeID)
        {
            AIPPreparationLayer Layer = new AIPPreparationLayer();
            var lst = Layer.ddlAddAIP_ProgramActivities_Main_Data(OfficeID);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        

        public string SaveAIP(long? MotherAIP_ID, int OfficeID, int? EmplementingOfficeID, string FundingSource, string AIPRefCode,
            string Description, string StartDate, string CompletionDate, string ExpectedOutput, string PSAmount, string MOOEAmount,
            string COAmount, int? ClimateChangeType, string ClimateChangeAmount, string ClimateChangeTypologyCode, int? OrderNo)
        {
            AIPPreparationLayer Layer = new AIPPreparationLayer();
            return Layer.SaveAIP(MotherAIP_ID == null ? "NULL" : Convert.ToString(MotherAIP_ID), OfficeID,
            EmplementingOfficeID == null ? "NULL" : Convert.ToString(EmplementingOfficeID), FundingSource, AIPRefCode.Replace("'", "''"),
            Description.Replace("'", "''"), StartDate.Replace("'", "''"), CompletionDate.Replace("'", "''"), ExpectedOutput.Replace("'", "''"),
            PSAmount == "" ? "NULL" : PSAmount.Replace(",", ""), MOOEAmount == "" ? "NULL" : MOOEAmount.Replace(",", ""),
            COAmount == "" ? "NULL" : COAmount.Replace(",", ""), ClimateChangeType == 0 ? "NULL" : Convert.ToString(ClimateChangeType),
            ClimateChangeAmount == "" ? "NULL" : ClimateChangeAmount.Replace(",", ""),
            ClimateChangeTypologyCode.Replace("'", "''"), OrderNo == null ? 0 : OrderNo);
        }
        public string UpdateAIP(long? MotherAIP_ID, int? EmplementingOfficeID, string FundingSource, string AIPRefCode,
            string Description, string StartDate, string CompletionDate, string ExpectedOutput, string PSAmount, string MOOEAmount,
            string COAmount, int? ClimateChangeType, string ClimateChangeAmount, string ClimateChangeTypologyCode, int? OrderNo,int AIPID)
        {
            AIPPreparationLayer Layer = new AIPPreparationLayer();
            return Layer.UpdateAIP(MotherAIP_ID == null ? "NULL" : Convert.ToString(MotherAIP_ID),
            EmplementingOfficeID == null ? "NULL" : Convert.ToString(EmplementingOfficeID), FundingSource, AIPRefCode.Replace("'", "''"),
            Description.Replace("'", "''"), StartDate.Replace("'", "''"), CompletionDate.Replace("'", "''"), ExpectedOutput.Replace("'", "''"),
            PSAmount == "" ? "NULL" : PSAmount.Replace(",", ""), MOOEAmount == "" ? "NULL" : MOOEAmount.Replace(",", ""),
            COAmount == "" ? "NULL" : COAmount.Replace(",", ""), ClimateChangeType == 0 ? "NULL" : Convert.ToString(ClimateChangeType),
            ClimateChangeAmount == "" ? "NULL" : ClimateChangeAmount.Replace(",", ""),
            ClimateChangeTypologyCode.Replace("'", "''"), OrderNo == null ? 0 : OrderNo, AIPID);
        }
        public void NonOfficeToogleAIP(int? AIPID,string isNonOffice) { 
        AIPPreparationLayer Layer = new AIPPreparationLayer();
        Layer.NonOfficeToogleAIP(AIPID, isNonOffice == "true" ? 1 : 0);
        //return Layer.NonOfficeToogleAIP(AIPID, isNonOffice == "true" ? 1 : 0);
        }
        
        public string getAIPNewOrderNo(int OfficeID)
        {
            AIPPreparationLayer Layer = new AIPPreparationLayer();
            return Layer.getAIPNewOrderNo(OfficeID);
        }
        public JsonResult grdAIPListData([DataSourceRequest] DataSourceRequest request, int? OfficeID)
        {
            AIPPreparationLayer Layer = new AIPPreparationLayer();
            var AIPData = Layer.grdAIPListData(OfficeID);
            return Json(AIPData.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string getAIPGridFooterTotal(int? OfficeID)
        {
            AIPPreparationLayer Layer = new AIPPreparationLayer();
            return Layer.getAIPGridFooterTotal(OfficeID);
        }
        public string UpdateAIPOrderNo(int AIP_ID, int OrderNo) 
        {
            AIPPreparationLayer Layer = new AIPPreparationLayer();
            return Layer.UpdateAIPOrderNo(AIP_ID, OrderNo);
        }
        public string RemoveAIP(int AIP_ID)
        {
            AIPPreparationLayer Layer = new AIPPreparationLayer();
            return Layer.RemoveAIP(AIP_ID);
        }
        public JsonResult GetAIPDataForEdit(int AIPID)
        {
            AIPPreparationLayer Layer = new AIPPreparationLayer();
            var lst = Layer.GetAIPDataForEdit(AIPID);
            List<string> AIPItemData = new List<string>();
            AIPItemData.Add(lst.MotherAIPID); //[0]
            AIPItemData.Add(lst.EmplementingOffice);//[1]
            AIPItemData.Add(lst.AIPRefCode);//[2]
            AIPItemData.Add(lst.Description);//[3]
            AIPItemData.Add(lst.StartDate);//[4]
            AIPItemData.Add(lst.CompletionDate);//[5]
            AIPItemData.Add(lst.FundingSource);//[6]
            AIPItemData.Add(lst.PSAmount);//[7]
            AIPItemData.Add(lst.MOOEAmount);//[8]
            AIPItemData.Add(lst.COAmount);//[9]
            AIPItemData.Add(lst.OrderNo.ToString());//[10]
            AIPItemData.Add(lst.ExpectedOutput);//[11]
            AIPItemData.Add(lst.CCType);//[12]
            AIPItemData.Add(lst.CCAmount);//[13]
            AIPItemData.Add(lst.CCTypologyCode);//[14]
            return Json(AIPItemData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_DDLAIPYear([DataSourceRequest] DataSourceRequest request)
        {
            AIPPreparationLayer Layer = new AIPPreparationLayer();
            var lst = Layer.Get_DDLAIPYear();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ddlCCTypologyCode_Data([DataSourceRequest] DataSourceRequest request, int CCType)
        {
            AIPPreparationLayer Layer = new AIPPreparationLayer();
            var lst = Layer.ddlCCTypologyCode_Data(CCType);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfigureLBPForm4()
        {
            return View("pvConfigureLBPForm4");
            //var status = "";
            //MenuLayer Menu = new MenuLayer();
            //var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            //foreach (var item in mnu)
            //{
            //    if (item.MenuID == 12024)
            //    {
            //        status = "Authorized";
            //    }
            //}
            //if (status == "Authorized")
            //{
            //    if (Account.UserInfo.UserTypeDesc == "Budget In-Charge")
            //    {
            //        return View("OfficeAdmin");
            //    }
            //    else
            //    {
            //        //  return RedirectToAction("pv_Home", "Annual");
            //        return View("BudgetCommittee");
            //    }
            //}
            //else
            //{
            //    return View("_UnAuthorizedAccess");
            //}
        }
        public string FetchAIPData(int OfficeID) {
            LBPForm4PreparationLayer Layer = new LBPForm4PreparationLayer();
            return Layer.FetchAIPData(OfficeID);
        }
        public JsonResult grLBPForm4PPAs([DataSourceRequest] DataSourceRequest request, int? OfficeID,int? transyear)
        {
            LBPForm4PreparationLayer Layer = new LBPForm4PreparationLayer();
            var lst = Layer.grLBPForm4PPAs(OfficeID, transyear);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string getLBPForm4GridFooterTotal(int OfficeID)
        {
            LBPForm4PreparationLayer Layer = new LBPForm4PreparationLayer();
            return Layer.getLBPFORM4GridFooterTotal(OfficeID);
        }
        public ActionResult pvLBPForm4PPA_Tab()
        {
            return PartialView("pvLBPForm4PPA_Tab");
        }
        public ActionResult pvLBPForm4OtherData_Tab()
        {
            return PartialView("pvLBPForm4OtherData_Tab");
        }
        public PartialViewResult pvLBPForm4OtherData_Window(string Mode, string Series)
        {
            ViewBag.Mode = Mode + "," + Series;
            return PartialView("pvLBPForm4OtherData_Window");
        }
        
        
        public JsonResult ddlForm4DataType([DataSourceRequest] DataSourceRequest request)
        {
            LBPForm4PreparationLayer Layer = new LBPForm4PreparationLayer();
            var lst = Layer.ddlForm4DataType();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public string SaveNewForm4OtherData(int DataType, int OrderNo, string Description,int OfficeID,int yearof) {
            LBPForm4PreparationLayer Layer = new LBPForm4PreparationLayer();
            return Layer.SaveNewForm4OtherData(DataType,OrderNo,Description.Replace("'","''"),OfficeID, yearof);
        }
        public string UpdateForm4OtherData(int SeriesID,int DataType, int OrderNo, string Description,int yearof)
        {
            LBPForm4PreparationLayer Layer = new LBPForm4PreparationLayer();
            return Layer.UpdateForm4OtherData(SeriesID, DataType, OrderNo, Description.Replace("'", "''"), yearof);
        }
        public string DeleteForm4OtherData(int SeriesID)
        {
            LBPForm4PreparationLayer Layer = new LBPForm4PreparationLayer();
            return Layer.DeleteForm4OtherData(SeriesID);
        }
        public string GetForm4OtherDataOrderNo(int OfficeID,int DataType)
        {
            LBPForm4PreparationLayer Layer = new LBPForm4PreparationLayer();
            return Layer.GetForm4OtherDataOrderNo(OfficeID, DataType);
        }
        public JsonResult GetForm4OtherDataSelectedForEdit(int SeriesID)
        {
            LBPForm4PreparationLayer Layer = new LBPForm4PreparationLayer();
            var lst = Layer.GetForm4OtherDataSelectedForEdit(SeriesID);
            List<string> AIPItemData = new List<string>();
            AIPItemData.Add(lst.Description); //[0]
            AIPItemData.Add(lst.DataType.ToString());//[1]
            AIPItemData.Add(lst.OrderNo.ToString());//[2]
            
            return Json(AIPItemData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult grLBPForm4OtherData([DataSourceRequest] DataSourceRequest request, int? OfficeID, int? transyear)
        {
            LBPForm4PreparationLayer Layer = new LBPForm4PreparationLayer();
            var lst = Layer.grLBPForm4OtherData(OfficeID, transyear);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetForm4PPASelectedForEdit(int? OfficeID = 0, int? transyear = 0, long? transno=0)
        {
            LBPForm4PreparationLayer Layer = new LBPForm4PreparationLayer();
            var lst = Layer.GetForm4PPASelectedForEdit(OfficeID, transyear,transno);
            List<string> Data = new List<string>();
        
            Data.Add(lst.MajorFinalOutput);//[0]
            Data.Add(lst.PerformanceIndicator);//[1]
            Data.Add(lst.TargetForTheBudgetYear);//[2]
            Data.Add(lst.PSAmount.ToString());//[3]
            Data.Add(lst.isBold.ToString());//[4]
            Data.Add(lst.OrderNo.ToString());//[5]
            Data.Add(lst.PSMax.ToString());//[6]
            Data.Add(lst.MOOEMax.ToString());//[7]
            Data.Add(lst.COMax.ToString());//[8]
            Data.Add(lst.initiativeid.ToString());//[9]
            Data.Add(lst.initiative.ToString());//[10]
            Data.Add(lst.parentaipid.ToString());//[11]
            Data.Add(lst.parent_trnno.ToString());//[12]
            Data.Add(lst.target.ToString());//[13]
            Data.Add(lst.ChildTotAmount.ToString());//[14]

            return Json(Data, JsonRequestBehavior.AllowGet);
        }
        public string UpdateForm4PPA(int SeriesID, int OrderNo, string PPADesc,
                string MajorFinalOutput,string PerformanceIndicator,
                string Target, string PSAmount, string MOOEAmount, string COAmount, string isBold, string RefCode) 
        {
            LBPForm4PreparationLayer Layer = new LBPForm4PreparationLayer();
            return Layer.UpdateForm4PPA(SeriesID, OrderNo, PPADesc.Replace("'", "''"), MajorFinalOutput.Replace("'", "''"),
                    PerformanceIndicator.Replace("'", "''"), Target.Replace("'", "''"), PSAmount == "" ? "NULL" : PSAmount.Replace(",", ""),
                    MOOEAmount == "" ? "NULL" : MOOEAmount.Replace(",", ""),
                    COAmount == "" ? "NULL" : COAmount.Replace(",", ""), isBold == "true" ? "1" : "0", RefCode.Replace(",", ""));
        }   
        public string RemoveLBPForm4PPA(int transno)
        {
            LBPForm4PreparationLayer Layer = new LBPForm4PreparationLayer();
            return Layer.RemoveLBPForm4PPA(transno);
        }
        public double getTotalExpAmount(int? OfficeID=0,int? YearOf=0,int? ooeid=0,string ProgramDescription="",int? ProgramID=0,int? AccountID=0)
        {
            var TotalProposedAmount = 0.00;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
               
                SqlCommand com2 = new SqlCommand(@"exec sp_BMSGetAIPLump "+ OfficeID + ","+ YearOf + ","+ ooeid + ",'"+ ProgramDescription + "',"+ ProgramID + ","+ AccountID + "", con);
                con.Open();
                TotalProposedAmount = Convert.ToDouble(com2.ExecuteScalar().ToString());
            }
            return TotalProposedAmount;
        }
        public int getPPMPAccount(int? OfficeID, int? ProgramID, int? AccountID, int? YearOf)
        {
            //var noppmpaccount = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com2 = new SqlCommand(@"Select count([AccountDenominationID]) FROM [IFMIS].[dbo].[tbl_T_BMSAccountDenomination] where [OfficeID]="+ OfficeID + " and [ProgramID]="+ ProgramID + "  and [AccountID] ="+ AccountID + ""+
                    " and [TransactionYear]="+ YearOf + " and [ActionCode]=1 and [isPPMP]=1", con);
                con.Open();
                return Convert.ToInt32(com2.ExecuteScalar().ToString());
            }
           
        }
        //public double getProposedExpenseSummary(int? Yearof = 0, int? OfficeId = 0, int? ooeid = 0)
        //{
        //    using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //    {
        //        SqlCommand com = new SqlCommand(@"exec [sp_BMS_PieChartPerOfficeECDetailsTotal] " + Yearof + "," + OfficeId + "," + ooeid + "", con);
        //        con.Open();
        //        return Convert.ToDouble(com.ExecuteScalar().ToString());
        //    }
        //}
        public int getFund(int? officeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com2 = new SqlCommand(@"Select [FundID] from [IFMIS].[dbo].[tbl_R_BMSOffices] where [OfficeID]=" + officeID  + "", con);
                con.Open();
                return Convert.ToInt32(com2.ExecuteScalar().ToString());
            }
        }
        public PartialViewResult newAccountLink(int ProgramID,int AccountID,int ProposalYear)
        {
            Session["ProgramID"] = ProgramID;
            Session["AccountID"] = AccountID;
            Session["ProposalYear"] = ProposalYear;
            return PartialView("pv_AccountLink");
        }
        public string DownloadAIP(int OfficeID, int proyear)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn3()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.[sp_replicate_aip_breakdown_to_bms_accounts] " + getPMisofficeid(OfficeID)+ "," + proyear + "", con);
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
        public string DownloadAIPLump(int OfficeID, int proyear)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn3()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.[sp_spms_GenerateAIP_for_BMS] " + proyear + "," + getPMisofficeid(OfficeID) + ",0,0", con);
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

        public int getPMisofficeid(int? officeid = 0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select [PMISOfficeID] from tbl_R_BMSOffices where [OfficeID] =" + officeid + "", con);
                con.Open();
                return Convert.ToInt32(com.ExecuteScalar().ToString());
            }
        }
        public ActionResult ReadInitiative([DataSourceRequest]DataSourceRequest request,int officeid,int year)
        {
            string tempStr = "";
            tempStr = "select [tbl_R_BMSAIP_SPMS].[id] as id, [initiative] from [IFMIS].[dbo].[tbl_R_BMSAIP_SPMS] left join " +
                "[IFMIS].[dbo].[tbl_R_BMSOffices] on [PMISOfficeID]=[pmis_office_id] where OfficeID="+ officeid + " and year="+ year + " and ActionCode=1  order by initiative";
            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult ReadInitiativeEdit([DataSourceRequest]DataSourceRequest request, int officeid, int year)
        {
            string tempStr = "";
            tempStr = "select [tbl_R_BMSAIP_SPMS].[id] as id, [initiative] from [IFMIS].[dbo].[tbl_R_BMSAIP_SPMS] left join " +
                "[IFMIS].[dbo].[tbl_R_BMSOffices] on [PMISOfficeID]=[pmis_office_id] where OfficeID=" + officeid + " and year=" + year + " and ActionCode=1  ";
            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public JsonResult getAIPamount(long? id,int? year)
        {
            LBPForm4Model data = new LBPForm4Model();
            DataTable _dt = new DataTable();
            string _sqlQuery = "sp_BMS_PPAbreakdownbalance " + id + "," + year + "";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.appropriation = Convert.ToDouble(_dt.Rows[0][0]);
                data.breakdownamount = Convert.ToDouble(_dt.Rows[0][1]);
                data.balance = Convert.ToDouble(_dt.Rows[0][2]);
                data.OrderNo = Convert.ToInt32(_dt.Rows[0][3]);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
       
        }
        public string AddInitiative(int? OrderNo=0, 
               string MajorFinalOutput="", string PerformanceIndicator="",
               string Target="", double PSAmount=0.00, double MOOEAmount = 0.00, double COAmount = 0.00, string isBold="", int? initiative=0, string subinitiative="",int? year=0,int? mode=0,long? trnno=0,int? aiptag=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMSAddInitiative] "+ OrderNo +",'"+ MajorFinalOutput.Replace("'", "''") +"','"+ PerformanceIndicator.Replace("'", "''") +"','"+ Target.Replace("'", "''") +"',"+ PSAmount +","+ MOOEAmount +","+ COAmount +",'"+ isBold +"', "+ initiative +", '"+ subinitiative.Replace("'","''") +"',"+ year +","+ mode +","+ trnno +","+ aiptag + "", con);
                    com.CommandTimeout = 0;
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
        public PartialViewResult ShowAIPWindow(string Mode, string AIPID)
        {
            ViewBag.Mode = Mode + "," + AIPID;
            //return PartialView("pvAddNewAIPWindow");   
            return PartialView("pvLBPForm4PPA_WindowNew");
        }
        public PartialViewResult pvLBPForm4PPA_Window(long transno,int aiptag)
        {
            ViewBag.transno = transno;
            ViewBag.aiptag = aiptag;
            return PartialView("pvLBPForm4PPA_WindowUpdate");
        }
        public string SubmitAccount(int? officeID=0, int ? propyear=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec sp_BMS_SubmitProposedBudget "+ officeID + ","+ propyear + "," + Account.UserInfo.eid + "", con);
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
        public string ReturnPropose(int? officeID = 0, int? propyear = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec sp_BMS_ReturnProposed " + officeID + "," + propyear + "", con);
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
        public string Checkpsaccount(int? officeID = 0, int? propyear = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec  sp_BMS_NoPsAccountProposed " + officeID + "," + propyear + "", con);
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
        public PartialViewResult transferprogram(int ProgramID, int AccountID)
        {
            Session["ProgramID"] = ProgramID;
            Session["AccountID"] = AccountID;
            return PartialView("pv_Transferprogram");
        }
        //public int getpmisofficeid(int? officeID = 0)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //        {
        //            var data = 0;
        //            SqlCommand com = new SqlCommand(@"select PMISOfficeID from ifmis.dbo.tbl_R_BMSOffices where OfficeID=" + officeID + "", con);
        //            con.Open();
        //            data = Convert.ToInt16(com.ExecuteScalar());
        //            return data;
        //        }
        //    }
        //    catch //(Exception ex)
        //    {
        //        return 0;
        //    }
        //}
    }
}