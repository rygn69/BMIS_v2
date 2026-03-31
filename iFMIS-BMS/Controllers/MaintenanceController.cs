using iFMIS_BMS.BusinessLayer.Layers.Grid;
using iFMIS_BMS.BusinessLayer.Layers.Maintenance;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iFMIS_BMS.BusinessLayer.Layers;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
using System.Web.Script.Serialization;
using System.Data;
using System.IO;
using iFMIS_BMS.Base;
using System.Configuration;

namespace iFMIS_BMS.Controllers
{
    [Authorize]
    public class MaintenanceController : Controller
    {

        #region Control Utilities
        public ActionResult ControlUtilities()
        {
            return View("vwControlUtilities");
        }
        public ActionResult CParticulars()
        {
            return View("vwCParticulars");
        }
        public ActionResult PPSASCode()
        {
            return View("vwPPSASCode");
        }
        public ActionResult TransactionCharge()
        {
            return View("vwChangeAcctCharge");
        }
        public ActionResult TransactionChargeExcess()
        {
            return View("vwChangeAcctChargeExcess");
        }
        public ActionResult ChangeOfficeCharge()
        {
            return View("vwChangeOfficeCharge");
        }
        //public ActionResult ChangeParticulars()
        //{
        //    return View("vwChangeParticulars");
        //}
        public ActionResult ChangeParticulars()
        {
            return View("vwParticulars");
        }
        public ActionResult RefNoStatus()
        {
            return View("vwRefNoStatus");
        }
        public PartialViewResult TransferTransactionCharge()
        {
            return PartialView("pvTransferTransactionCharge");
        }
        public PartialViewResult TransferTransactionChargeExcess()
        {
            return PartialView("pvTransferTransactionChargeExcess");
        }
        public JsonResult ProgramData([DataSourceRequest] DataSourceRequest request, int OfficeID, int YearOf)
        {
            grPrograms program_list = new grPrograms();
            var program_lst = program_list.grProgram_list(OfficeID, YearOf);
            return Json(program_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult grPPSASACode([DataSourceRequest] DataSourceRequest request, int? Year)
        {
            PPSASCode_Layer PPSASACode = new PPSASCode_Layer();
            var PPSASACode_lst = PPSASACode.PPSASACode_list(Year);
            return Json(PPSASACode_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchAccount([DataSourceRequest] DataSourceRequest request, string FundDropdown)
        {
            grAccounts account_list = new grAccounts();
            var account_lst = account_list.grAccount_list(FundDropdown);
            return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getExistingAccounts([DataSourceRequest] DataSourceRequest request)
        {
            grAccounts account_list = new grAccounts();
            var account_lst = account_list.getExistingAccounts();
            return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchBuildPropose([DataSourceRequest] DataSourceRequest request, string FundDropdown)
        {
            grAccounts account_list = new grAccounts();
            var account_lst = account_list.grBuildProposed(FundDropdown);
            return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getOfficeAccounts([DataSourceRequest] DataSourceRequest request, int? OfficeID, int? YearOf)
        {
            grAccounts account_list = new grAccounts();
            var account_lst = account_list.getOfficeAccounts(OfficeID, YearOf);
            return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchAcountPropose([DataSourceRequest] DataSourceRequest request, string FundDropdown)
        {
            grAccounts account_list = new grAccounts();
            var account_lst = account_list.grAcountProposed(FundDropdown);
            return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult PPA_MFO([DataSourceRequest] DataSourceRequest request, int? OfficeID)
        {
            grPrograms PPA_LIST = new grPrograms();
            var ppa_lst = PPA_LIST.grPPA_MFO(OfficeID);
            return Json(ppa_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult MFO_Breakdown([DataSourceRequest] DataSourceRequest request, int? PPA_MFO_ID)
        {
            grPrograms PPA_LIST = new grPrograms();
            var ppa_lst = PPA_LIST.grPPA_MFO_Breakdown(PPA_MFO_ID);
            return Json(ppa_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchActiveAccount([DataSourceRequest] DataSourceRequest request)
        {
            grAccounts account_list = new grAccounts();
            var account_lst = account_list.grActiveAccount_list();
            return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchAccountDelete([DataSourceRequest] DataSourceRequest request)
        {
            grAccounts account_list = new grAccounts();
            var account_lst = account_list.grAccountDelete_list();
            return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchAccountNameID([DataSourceRequest] DataSourceRequest request, string AccountData)
        {
            grAccounts account_list = new grAccounts();
            var account_lst = account_list.grAccount_listData(AccountData);
            return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchAccountNameIDbyOOE([DataSourceRequest] DataSourceRequest request, string AccountData, string ProgramID)
        {
            grAccounts account_list = new grAccounts();
            var account_lst = account_list.grAccount_listOOEData(AccountData, ProgramID);
            return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchProgram([DataSourceRequest] DataSourceRequest request, string DataDropdown)
        {
            grPrograms account_info = new grPrograms();
            var accountInfo = account_info.grProgram_Info(DataDropdown);
            return Json(accountInfo.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchProgramDelete([DataSourceRequest] DataSourceRequest request, string DataDropdown)
        {
            grPrograms account_info = new grPrograms();
            var accountInfo = account_info.grProgramDelete_Info(DataDropdown);
            return Json(accountInfo.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchProgramAccountNonActive([DataSourceRequest] DataSourceRequest request, string OfficeDropdown, string ProgramDropdown, string AccountDropdown)
        {
            grAccounts account_info = new grAccounts();
            var accountInfo = account_info.grAccountNonActive_Info(OfficeDropdown, ProgramDropdown, AccountDropdown);
            return Json(accountInfo.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult OfficeWithMagnaCarta([DataSourceRequest] DataSourceRequest request, int? AccountID, int? BudgetYear)
        {
            grAccounts account_info = new grAccounts();
            var accountInfo = account_info.grMagnaCarta(AccountID, BudgetYear);
            return Json(accountInfo.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LBP5_Objectives([DataSourceRequest] DataSourceRequest request, int? OfficeID)
        {
            grPrograms lbp5_obj = new grPrograms();
            var lbp5Info = lbp5_obj.lbp5_info(OfficeID);
            return Json(lbp5Info.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LBP5_FS([DataSourceRequest] DataSourceRequest request, int? OfficeID)
        {
            grPrograms lbp5_fs = new grPrograms();
            var lbp5Info = lbp5_fs.lbp5_fs(OfficeID);
            return Json(lbp5Info.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Manage Programs
        [Authorize]
        public ActionResult AddPrograms()
        {

            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc);
            foreach (var item in mnu)
            {
                if (item.MenuID == 3003)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                return View("pvAddPrograms");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }

        }

        public string UpdatePrograms(programs UpdateProgram)
        {
            UpdateProgram_Layer VIL = new UpdateProgram_Layer();
            return VIL.UpdatePrograms(UpdateProgram);
        }
        public string UpdateProgramAccount(account_code ProgramAccount)
        {
            UpdateProgramAccount_Layer VIL = new UpdateProgramAccount_Layer();
            return VIL.UpdateProgramAccount(ProgramAccount);
        }
        public string UpdateAccounts(account_code UpdateAccount)
        {
            UpdateAccount_Layer VIL = new UpdateAccount_Layer();
            return VIL.UpdateAccounts(UpdateAccount);
        }

        public string AddProgramAccounts(saveProgramAccount ProgramAccount)
        {
            SaveProgramAccount_Layer VIL = new SaveProgramAccount_Layer();
            return VIL.SaveProgramAccount(ProgramAccount);
        }
        public string AddOfficeAccounts(int OfficeID, int ProgramID, int OOEID, int YearOf, int AccountID, int OrderNo, string AccountName)
        {
            SaveProgramAccount_Layer VIL = new SaveProgramAccount_Layer();
            return VIL.AddOfficeAccounts(OfficeID, ProgramID, OOEID, YearOf, AccountID, OrderNo, AccountName);
        }
        public string CheckUserStatus(checkStatus UserCheckStatus)
        {
            CheckStatus_Layer VIL = new CheckStatus_Layer();
            return VIL.CheckStatus(UserCheckStatus);
        }
        public ActionResult CreateProgramAccounts()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc);
            foreach (var item in mnu)
            {
                if (item.MenuID == 4014)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                return View("pvCreateNewPrograms");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        public string RemoveProgramAccount(int ProgramAccountID)
        {
            SaveProgram_Layer VIL = new SaveProgram_Layer();
            return VIL.RemoveProgramAccount(ProgramAccountID);
        }

        public ActionResult UpdateProgramAccounts()
        {
            return View("_UnAuthorizedAccess");
            //return View("pvUpdateProgramAccount");
        }
        public string SaveNewProgram(saveProgram Program)
        {
            SaveProgram_Layer VIL = new SaveProgram_Layer();
            return VIL.SaveNewProgram(Program);
            // continue
        }
        public ActionResult ManagePrograms()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc);
            foreach (var item in mnu)
            {
                if (item.MenuID == 4011)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                return View("pvManagePrograms");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        //public JsonResult SearchProgram([DataSourceRequest] DataSourceRequest request, string DataDropdown)
        //{
        //    grPrograms account_info = new grPrograms();
        //    var accountInfo = account_info.grProgram_Info(DataDropdown);
        //    return Json(accountInfo.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult SearchProgramDelete([DataSourceRequest] DataSourceRequest request, string DataDropdown)
        //    {
        //    grPrograms account_info = new grPrograms();
        //    var accountInfo = account_info.grProgramDelete_Info(DataDropdown);
        //    return Json(accountInfo.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult SearchProgramAccountNonActive([DataSourceRequest] DataSourceRequest request, string OfficeDropdown, string ProgramDropdown, string AccountDropdown)
        //        {
        //    grAccounts account_info = new grAccounts();
        //    var accountInfo = account_info.grAccountNonActive_Info(OfficeDropdown, ProgramDropdown, AccountDropdown);
        //    return Json(accountInfo.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //        }
        public string DeleteProgram(int program_id)
        {
            UpdateProgram_Layer VIL = new UpdateProgram_Layer();
            return VIL.DeleteProgram(program_id);
        }
        public string RestoreProgram(int program_id)
        {
            UpdateProgram_Layer VIL = new UpdateProgram_Layer();
            return VIL.RestoreProgram(program_id);
        }
        //public JsonResult ProgramData([DataSourceRequest] DataSourceRequest request, string DataDropdown)
        //    {
        //    grPrograms program_list = new grPrograms();
        //    var program_lst = program_list.grProgram_list(DataDropdown);
        //    return Json(program_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //    }
        #endregion

        #region Others / Menu Index
        public ActionResult Index()
        {
            return View();
        }
        public string CheckMotherOffice(int OfficeID)
        {
            var ReturnValue = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT b.OfficeName FROM [dbo].[tbl_R_BMSMainAndSubOffices] as a
                                                INNER JOIN pmis.dbo.OfficeDescription as b on b.OfficeID = a.MainOfficeID_PMIS 
                                                where SubOfficeID_IFMIS =" + OfficeID + "", con);
                con.Open();
                ReturnValue = Convert.ToString(com.ExecuteScalar());
                return ReturnValue;
            }
        }
        #endregion

        #region Unused maybe?
        public ActionResult UpdateUser()
        {
            return View("_UnAuthorizedAccess");
            //return View("pvUpdateUser");
        }
        public ActionResult UpdateOffice()
        {
            return View("_UnAuthorizedAccess");
            //return View("pvUpdateOffice");
        }

        public ActionResult UpdateProgram()
        {
            return View("_UnAuthorizedAccess");
            //return View("pvUpdateProgram");
        }

        public ActionResult UpdateAccount()
        {
            return View("_UnAuthorizedAccess");
            //return View("pvUpdateAccounts");
        }
        public ActionResult ChangeFMiSCode()
        {
            return View("_UnAuthorizedAccess");
            //return View("pvChangeFMisCode");
        }
        #endregion

        #region PPMP
        public ActionResult CopyPPMPData()
        {
            return View("vwCopyPPMPData");
        }
        public string CopyPPMPCurrentData(int? officeID)
        {
            CopyPPMPData_Layer CIL = new CopyPPMPData_Layer();
            return CIL.CopyPPMPCurrentData(officeID);
        }
        #endregion

        #region LBP FORM 5
        public JsonResult UpdateOrder([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<lbp5> Breakdown)
        {
            LBP5_Layer el = new LBP5_Layer();
            try
            {
                el.UpdateOrder(Breakdown);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(Breakdown.ToDataSourceResult(request, ModelState));
        }
        //public JsonResult LBP5_Objectives([DataSourceRequest] DataSourceRequest request, int? OfficeID)
        //    {
        //    grPrograms lbp5_obj = new grPrograms();
        //    var lbp5Info = lbp5_obj.lbp5_info(OfficeID);
        //    return Json(lbp5Info.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //    }
        //public JsonResult LBP5_FS([DataSourceRequest] DataSourceRequest request, int? OfficeID)
        //    {
        //    grPrograms lbp5_fs = new grPrograms();
        //    var lbp5Info = lbp5_fs.lbp5_fs(OfficeID);
        //    return Json(lbp5Info.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //    }

        public JsonResult UpdateMFOOrder([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<lbp5> MFOBreakdown)
        {
            LBP5_Layer el = new LBP5_Layer();
            try
            {
                el.UpdateMFOOrder(MFOBreakdown);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(MFOBreakdown.ToDataSourceResult(request, ModelState));
        }
        public JsonResult UpdateOBJOrder([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<lbp5> Objectives)
        {
            LBP5_Layer el = new LBP5_Layer();
            try
            {
                el.UpdateOBJOrder(Objectives);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(Objectives.ToDataSourceResult(request, ModelState));
        }
        public JsonResult UpdateFSOrder([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<lbp5> FS)
        {
            LBP5_Layer el = new LBP5_Layer();
            try
            {
                el.UpdateFSOrder(FS);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(FS.ToDataSourceResult(request, ModelState));
        }
        public string DeleteOBJ(int? OBJ_ID)
        {
            LBP5_Layer VIL = new LBP5_Layer();
            return VIL.DeleteOBJ(OBJ_ID);
        }
        public string DeleteFS(int? FS_ID)
        {
            LBP5_Layer VIL = new LBP5_Layer();
            return VIL.DeleteFS(FS_ID);
        }
        public string DeleteMFO(int? PPA_MFO_ID)
        {
            LBP5_Layer VIL = new LBP5_Layer();
            return VIL.DeleteMFO(PPA_MFO_ID);
        }
        public string DELETE_MFO_Breakdown(int? PPA_ID)
        {
            LBP5_Layer VIL = new LBP5_Layer();
            return VIL.DELETE_MFO_Breakdown(PPA_ID);
        }
        public string SaveOBJ(string ObjName, int? OrderBy)
        {
            LBP5_Layer VIL = new LBP5_Layer();
            return VIL.SumbitOBJ(ObjName, OrderBy);
        }
        public string SaveFS(string FSName, int? OrderBy)
        {
            LBP5_Layer VIL = new LBP5_Layer();
            return VIL.SumbitFS(FSName, OrderBy);
        }
        public string MFOBreakdown(string PPA_CodeRef, string PPADescription, double? AccountList, string PPA_TARGET, string PPA_Output_Indicator, string From, string To, int? Send_MFO_ID, int? PPA_DescriptionID)
        {
            LBP5_Layer VIL = new LBP5_Layer();
            return VIL.SumbitMFOBreakdown(PPA_CodeRef, PPADescription, AccountList, PPA_TARGET, PPA_Output_Indicator, From, To, Send_MFO_ID, PPA_DescriptionID);
        }
        public string SaveMFO(string ObjName, double? MFOCost, int? OrderBy)
        {
            LBP5_Layer VIL = new LBP5_Layer();
            return VIL.SumbitMFO(ObjName, MFOCost, OrderBy);
        }
        public string FSUpdate(string LBP5_FS, int? IDparam)
        {
            LBP5_Layer VIL = new LBP5_Layer();
            return VIL.FSUpdate(LBP5_FS, IDparam);
        }
        public string FSContent(int? YearDate, int? OfficeIDParam)
        {
            LBP5_Layer VIL = new LBP5_Layer();
            return VIL.ViewFSContent(YearDate, OfficeIDParam);
        }
        //public JsonResult PPA_MFO([DataSourceRequest] DataSourceRequest request, int? OfficeID)
        //{
        //    grPrograms PPA_LIST = new grPrograms();
        //    var ppa_lst = PPA_LIST.grPPA_MFO(OfficeID);
        //    return Json(ppa_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult MFO_Breakdown([DataSourceRequest] DataSourceRequest request, int? PPA_MFO_ID)
        //{
        //    grPrograms PPA_LIST = new grPrograms();
        //    var ppa_lst = PPA_LIST.grPPA_MFO_Breakdown(PPA_MFO_ID);
        //    return Json(ppa_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
        public PartialViewResult AddOBJ()
        {
            return PartialView("pvAddOBJ");
        }
        public PartialViewResult AddFS()
        {
            return PartialView("pvAddFS");
        }
        public PartialViewResult AddMFO()
        {
            return PartialView("pvAddMFO");
        }
        public PartialViewResult Edit_Objectives(string OrderBy, string Description, int ID)
        {
            lbp5 lbp5_list = new lbp5();
            lbp5_list.OBJID_param = ID;
            lbp5_list.OrderOBJ_param = OrderBy;
            lbp5_list.DescriptionOBJ_param = Description;
            return PartialView("pvEditObjectives", lbp5_list);
        }
        public PartialViewResult Edit_FS(int FS_OrderBy, string FS_DESC, int FS_ID)
        {
            lbp5 lbp5_list = new lbp5();
            lbp5_list.FS_IDparam = FS_ID;
            lbp5_list.FS_OrderByparam = FS_OrderBy;
            lbp5_list.FS_DESCparam = FS_DESC;
            return PartialView("pvEditFS", lbp5_list);
        }
        public PartialViewResult EditBreakdown(int PPA_ID, string PPA_Description, string PPA_Output_Indicator, string PPA_Target, string PPA_Implement_FROM, string PPA_Implement_TO, double PPA_Cost, string PPA_CodeRef, int PPA_MFO_ID, int Sub_PPA)
        {
            lbp5 lbp5_list = new lbp5();
            lbp5_list.PPA_IDparam = PPA_ID;
            lbp5_list.PPA_ID = PPA_Description == "" ? 0 : PPA_ID;
            lbp5_list.PPA_Descriptionparam = PPA_Description;
            lbp5_list.PPA_Output_Indicatorparam = PPA_Output_Indicator;
            lbp5_list.PPA_Targetparam = PPA_Target;
            lbp5_list.PPA_Implement_FROMparam = PPA_Implement_FROM;
            lbp5_list.PPA_Implement_TOparam = PPA_Implement_TO;
            lbp5_list.PPA_Costparam = PPA_Cost;
            lbp5_list.PPA_CodeRefparam = PPA_CodeRef;
            lbp5_list.PPA_MFO_ID = PPA_MFO_ID;
            lbp5_list.Sub_PPAparam = Sub_PPA;
            return PartialView("pvEditBreakdown", lbp5_list);

        }
        public PartialViewResult EditMFO(int PPA_MFO_ID, string PPA_MFO_Description, double MFO_COST, int? MFO_OrderBy)
        {
            lbp5 lbp5_list = new lbp5();
            lbp5_list.PPA_MFO_ID = PPA_MFO_ID;
            lbp5_list.PPA_MFO_Description = PPA_MFO_Description;
            lbp5_list.MFO_Cost = MFO_COST;
            lbp5_list.PPA_MFOOrderBy = MFO_OrderBy;
            return PartialView("pvEditMFO", lbp5_list);
        }


        public PartialViewResult ADDMFOBreakdown(int? PPA_MFO_ID)
        {
            lbp5 lbp_list = new lbp5();
            lbp_list.Send_MFO_ID = PPA_MFO_ID;

            return PartialView("pvADDMFOBreakdown", lbp_list);
        }
        public ActionResult ConfigureLBP5()
        {
            return View("vwLBP5Panel");
        }
        public ActionResult LBPForm1()
        {
            return View("vwLBP1Panel");
        }
        public string SaveEditOBJ(string ObjNameEDIT, string OrderBy, int ObjID)
        {
            LBP5_Layer VIL = new LBP5_Layer();
            return VIL.SaveEditOBJ(ObjNameEDIT, OrderBy, ObjID);
        }
        public string SaveEditFS(string FSNameEDIT, string OrderBy, int FSID)
        {
            LBP5_Layer VIL = new LBP5_Layer();
            return VIL.SaveEditFS(FSNameEDIT, OrderBy, FSID);
        }
        public string SaveEditMFO(int? MFOID, string MFONameEdit, double? MFO_COST, int? OrderBy)
        {
            LBP5_Layer VIL = new LBP5_Layer();
            return VIL.SaveEditMFO(MFOID, MFONameEdit, MFO_COST, OrderBy);
        }
        public string SaveEditBreakdown(int PPA_ID, string PPA_CodeRef, string PPADescription, int? AccountList, string PPA_TARGET, string PPA_Output_Indicator, string From, string To, int Send_MFO_ID, int? PPA_DescriptionID)
        {
            LBP5_Layer VIL = new LBP5_Layer();
            return VIL.SaveEditBreakdown(PPA_ID, PPA_CodeRef, PPADescription, AccountList, PPA_TARGET, PPA_Output_Indicator, From, To, Send_MFO_ID, PPA_DescriptionID);
        }

        public string CopyLBP5Data(int OfficeID)
        {
            LBP5_Layer LBP5_Layer = new LBP5_Layer();
            return LBP5_Layer.CopyLBP5Data(OfficeID);
        }
        #endregion

        #region Users

        public ActionResult UserType()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc);
            foreach (var item in mnu)
            {
                if (item.MenuID == 5015)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                return View("pvUserType");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        public JsonResult GetAdditionalRules(string UserTypeID)
        {
            UserRoleLayer UsrMenu = new UserRoleLayer();
            var mnu = UsrMenu.AdditionalRules(UserTypeID);
            List<int> Rules = new List<int>();
            Rules.Add(mnu.canReviewPS);
            Rules.Add(mnu.canReviewMOOE);
            Rules.Add(mnu.canReviewCO);
            Rules.Add(mnu.canReviewFE);
            return Json(Rules, JsonRequestBehavior.AllowGet);
        }
        public string updateUsertypeRule(int[] MenuIDs, int UsertypeID)
        {
            UserRoleLayer UserRoleLayer = new UserRoleLayer();
            return UserRoleLayer.UpdateUsertypeRule(MenuIDs, UsertypeID);
        }
        public string Updateusertype(int UsertypeID, int PS, int MOOE, int CO, int FE)
        {
            UserRoleLayer UserRoleLayer = new UserRoleLayer();
            return UserRoleLayer.UpdateUserType(UsertypeID, PS, MOOE, CO, FE);
        }
        public JsonResult getMenuList([DataSourceRequest] DataSourceRequest request,int? User_ID=0,int? eid=0)
        {
            MenuLayer Menu_Layer = new MenuLayer();
            var MenuList = Menu_Layer.getMenuList(User_ID, eid);
            var json = Json(MenuList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return (json);
        }
        #endregion

        #region Bar Chart
        public ActionResult pvBarChart()
        {

            Charts_layer Menu = new Charts_layer();

            var lst = Menu.BarCharts();
            return PartialView("pvBarChart", lst);
            //return View("pvBarChart");

        }
        #endregion

        #region Plantilla
        #region Plantilla - Hazard & Subsistence
        public string CheckHazardAndSubsistence(int SeriesID, int OfficeID, int isForFunding)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();

            return PlantillaLayer.CheckHazardAndSubsistence(SeriesID, OfficeID, isForFunding);
        }
        public string ReqExcludeOnHazard(int PlantillaItemNo, int isForFunding)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.ReqExcludeOnHazard(PlantillaItemNo, isForFunding);
        }
        public string ReqExcludeOnSubsistence(int PlantillaItemNo, int isForFunding)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.ReqExcludeOnSubsistence(PlantillaItemNo, isForFunding);
        }
        public string ReqIncludeOnHazard(int PlantillaItemNo, int isForFunding)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.ReqIncludeOnHazard(PlantillaItemNo, isForFunding);
        }
        public string ReqIncludeOnSubsistence(int PlantillaItemNo, int isForFunding)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.ReqIncludeOnSubsistence(PlantillaItemNo, isForFunding);
        }
        public JsonResult ddlHazardComputation()
        {
            PlantillaLayer ddl = new PlantillaLayer();
            var lst = ddl.ddlHazardComputation();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public string reqUpdateHazardFormulaUsed(int SeriesID, int OfficeID)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.reqUpdateHazardFormulaUsed(SeriesID, OfficeID);
        }
        #endregion
        public ActionResult MagnaCarta()
        {
            return View("pvMagnaCarta");
        }
        public ActionResult AccountComputationSettings()
        {
            return View("pvAccountComputationIndex");
        }
        public ActionResult StepIncrement()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc);
            foreach (var item in mnu)
            {
                if (item.MenuID == 6015)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                return View("pvStepIncrementIndex");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        [Authorize]
        public ActionResult PlantillaBuildUp()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc);
            foreach (var item in mnu)
            {
                if (item.MenuID == 6015)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                return View("pvPlantillaIndex");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }

        //public JsonResult OfficeWithMagnaCarta([DataSourceRequest] DataSourceRequest request, int? AccountID, int? BudgetYear)
        //{
        //    grAccounts account_info = new grAccounts();
        //    var accountInfo = account_info.grMagnaCarta(AccountID, BudgetYear);
        //    return Json(accountInfo.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}

        public string BuildupMagnaCarta(MagnaCarta BuildupMagnaCartaData)
        {
            BuildupMagnaCarta_Layer VIL = new BuildupMagnaCarta_Layer();
            return VIL.BuildupMagnaCarta(BuildupMagnaCartaData);
        }
        public PartialViewResult AddNewComputation()
        {
            AccountComputationModel AccountComputationModel = new AccountComputationModel();
            return PartialView("pvAccountComputationSettings", AccountComputationModel);
        }
        public JsonResult grPlantillaList([DataSourceRequest] DataSourceRequest request, int OfficeID, string isMainOffice, int PlanYear)
        {
            if (isMainOffice == "YES")
            {
                PlantillaLayer PlantillaLayer = new PlantillaLayer();
                var PlantillaList = PlantillaLayer.grPlantillaList(OfficeID, PlanYear);
                return Json(PlantillaList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            else
            {
                PlantillaLayer PlantillaLayer = new PlantillaLayer();
                var PlantillaList = PlantillaLayer.grPlantillaListSubOffice(OfficeID, PlanYear);
                return Json(PlantillaList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult grCasualList([DataSourceRequest] DataSourceRequest request, int OfficeID, int PlanYear)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            var PlantillaList = PlantillaLayer.grCasualList(OfficeID, PlanYear);
            return Json(PlantillaList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult grGetVacantPositions([DataSourceRequest] DataSourceRequest request)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            var PlantillaList = PlantillaLayer.grGetVacantPositions();
            return Json(PlantillaList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult grAccountComputationList([DataSourceRequest] DataSourceRequest request, int? yearActive)
        {
            grAccounts account_list = new grAccounts();
            var account_lst = account_list.grAccountComputationList(yearActive);
            return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string SaveNewComputation(int AccountCode, double Amount, int NoOfMonths, double Percentage,
                                        int isRoundOff, double MaxAmount, int EmployeeType, int YearActive)
        {
            AccountComputationLayer AccountComputationLayer = new AccountComputationLayer();
            return AccountComputationLayer.SaveNewComputation(AccountCode, Amount, NoOfMonths, Percentage, isRoundOff, MaxAmount, EmployeeType, YearActive);
        }
        public string CheckAccountComputation(int AccountCode, int YearActive)
        {
            AccountComputationLayer AccountComputationLayer = new AccountComputationLayer();
            return AccountComputationLayer.CheckAccountComputation(AccountCode, YearActive);
        }
        public string reqTransferToChildOffice(int SeriesID, int OfficeID)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.reqTransferToChildOffice(SeriesID, OfficeID);
        }
        public string reqTransferToMotherOffice(int SeriesID, int OfficeID)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.reqTransferToMotherOffice(SeriesID, OfficeID);
        }
        public ActionResult ReqUpdateComputation(int ComputationID, int AccountCode, double Amount, int NoOfMonths, double Percentage,
                                        short isRoundOff, double MaxAmount, int EmployeeType, string AccountName)
        {
            AccountComputationModel AccountComputationModel = new AccountComputationModel();
            AccountComputationModel.ComputationID = ComputationID;
            AccountComputationModel.AccountCode = AccountCode;
            AccountComputationModel.Amount = Amount;
            AccountComputationModel.Month = NoOfMonths;
            AccountComputationModel.Percentage = Percentage;
            AccountComputationModel.isRoundOf = isRoundOff;
            AccountComputationModel.MaxAmount = MaxAmount;
            AccountComputationModel.EmployeeType = EmployeeType;
            AccountComputationModel.AccountName = AccountName;
            return PartialView("pvAccountComputationSettings", AccountComputationModel);
        }
        public string UpdateComputation(int ComputationID, double Amount, int NoOfMonths, double Percentage,
                                        short isRoundOff, double MaxAmount, int EmployeeType)
        {
            AccountComputationLayer AccountComputationLayer = new AccountComputationLayer();
            return AccountComputationLayer.UpdateComputation(ComputationID, Amount, NoOfMonths, Percentage, isRoundOff, MaxAmount, EmployeeType);
        }
        public string ReqDeleteComputation(int ComputationID)
        {
            AccountComputationLayer AccountComputationLayer = new AccountComputationLayer();
            return AccountComputationLayer.DeleteComputation(ComputationID);
        }
        public string UpdatePlantilla(int SeriesID, int step)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.UpdateStepIncrement(SeriesID, step);
        }
        public PartialViewResult reqAddNewPlantillaItem(int PlanYear)
        {
            PlantillaModel PlantillaModel = new PlantillaModel();
            //Update on 3/27/2019 - xXx -used of parameter
            //   int year = DateTime.Now.Year;
            PlantillaModel.AppointmentDate = Convert.ToString(new DateTime(PlanYear + 1, 1, 1));
            return PartialView("pvAddNewPlantillaItem", PlantillaModel);
        }
        public PartialViewResult reqAddNewCasualItem()
        {
            PlantillaModel PlantillaModel = new PlantillaModel();
            int year = DateTime.Now.Year;
            PlantillaModel.AppointmentDate = Convert.ToString(new DateTime(year + 1, 1, 1));
            return PartialView("pvAddNewCasual", PlantillaModel);
        }
        public JsonResult GetPositionData(int PositionID, int PlanYear)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            var PositionData = PlantillaLayer.GetPositionData(PositionID, PlanYear);
            return Json(PositionData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSelectedCasualRate(int CasualRateID)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            var PositionData = PlantillaLayer.GetSelectedCasualRate(CasualRateID);
            return Json(PositionData, JsonRequestBehavior.AllowGet);
        }

        public string SaveNewPlantillaItem(int PositionID, string AppointmentDate, int EmploymentStatus, int DivisionID, int OfficeID, double HazardPay, double Subsistence, int PlanYear)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.savePlantillaItem(PositionID, AppointmentDate, EmploymentStatus, DivisionID, OfficeID, HazardPay, Subsistence, PlanYear);
        }
        public string RemoveProposedPlantillaItem(int PlantillaItemID)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.RemovePlantillaItem(PlantillaItemID);
        }
        public PartialViewResult ReqEditPlantillaItem(int ProposedItemID, int PlanYear)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PartialView("pvAddNewPlantillaItem", PlantillaLayer.GetSelectedPlantillaItemData(ProposedItemID, PlanYear));
        }
        public PartialViewResult ReqEditProposedCasual(int ProposedItemID)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PartialView("pvAddNewCasual", PlantillaLayer.GetSelectedCasualData(ProposedItemID));
        }
        public string UpdatePlantillaItem(int ProposedItemID, int PositionID, string AppointmentDate, int EmploymentStatus, int DivisionID, double HazardPay, double Subsistence, int PlanYear)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.UpdatePlantillaItem(ProposedItemID, PositionID, AppointmentDate, EmploymentStatus, DivisionID, HazardPay, Subsistence, PlanYear);
        }
        public string UpdateCasual(int ProposedItemID, string AppointmentDate, int EmploymentStatus, double HazardPay, double Subsistence)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.UpdateCasualItem(ProposedItemID, AppointmentDate, EmploymentStatus, HazardPay, Subsistence);
        }
        public string SaveNewCasual(int EmploymentStatus, string AppointmentDate, int OfficeID, double HazardPay, double Subsistence, int Quantity, int PlanYear)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.SaveNewCasual(EmploymentStatus, AppointmentDate, OfficeID, HazardPay, Subsistence, Quantity, PlanYear);
        }
        public string reqRemoveCasual(int SeriesID)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.reqRemoveCasual(SeriesID);
        }

        public string SaveVacantCasual(int EmploymentStatus, string AppointmentDate, int OfficeID, int Quantity, int PlanYear)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.SaveVacantCasual(EmploymentStatus, AppointmentDate, OfficeID, Quantity, PlanYear);
        }
        public string UpdateCasualVacant(int SeriesID, string AppointmentDate, int CasualRateID)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.UpdateCasualVacant(SeriesID, AppointmentDate, CasualRateID);
        }
        public string CheckWithHazardPay(int OfficeID, int PlanYear)
        {
            PlantillaLayer PlantillaLayer = new PlantillaLayer();
            return PlantillaLayer.CheckWithHazardPay(OfficeID, PlanYear);
        }


        public ActionResult pv_PlantillaOfPersonel()
        {
            return PartialView("pv_PlantillaOfPersonel");
        }
        public ActionResult pv_CasualEmployees()
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select [EnableSubmit] from [IFMIS].[dbo].[tbl_R_BMSubmitbtn] ", con);
                con.Open();
                Session["EnableAddCasual"] = Convert.ToBoolean(com.ExecuteScalar().ToString());
                con.Close();
            }
            return PartialView("pv_CasualEmployees");
        }
        #endregion

        #region SP - Ordinance
        public JsonResult grOrdinanceAttendance([DataSourceRequest] DataSourceRequest request)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            var AttendanceList = OrdinanceLayer.getAttendanceList();
            return Json(AttendanceList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string UpdateAuthor(int eid)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.UpdateAuthor(eid);
        }

        [HttpPost, ValidateInput(false)]
        public string SaveSection(string SectionDescription, int SectionOrder, string SectionName)
        {
            SectionDescription = HttpUtility.HtmlDecode(SectionDescription);
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.SaveSection(SectionDescription, SectionOrder, SectionName);
        }
        [HttpPost, ValidateInput(false)]
        public string UpdateSection(string SectionDescription, int SectionOrder, string SectionName, int SectionID)
        {
            SectionDescription = HttpUtility.HtmlDecode(SectionDescription);
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.UpdateSection(SectionDescription, SectionOrder, SectionName, SectionID);
        }

        [HttpPost, ValidateInput(false)]
        public string SaveSpecialProvision(int OfficeID, int SpecialProvisionNo, string SpecialProvision)
        {
            SpecialProvision = HttpUtility.HtmlDecode(SpecialProvision);
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.SaveSpecialProvision(OfficeID, SpecialProvisionNo, SpecialProvision);
        }
        [HttpPost, ValidateInput(false)]
        public string SaveSpecialProvisionMain(int OfficeID, int SpecialProvisionNo, string SpecialProvision)
        {
            SpecialProvision = HttpUtility.HtmlDecode(SpecialProvision);
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.SaveSpecialProvisionMain(OfficeID, SpecialProvisionNo, SpecialProvision);
        }
        [HttpPost, ValidateInput(false)]
        public string UpdateSpecialProvision(int OfficeID, int SpecialProvisionNo, string SpecialProvision, int SeriesID)
        {
            SpecialProvision = HttpUtility.HtmlDecode(SpecialProvision);
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.UpdateSpecialProvision(OfficeID, SpecialProvisionNo, SpecialProvision, SeriesID);
        }
        [HttpPost, ValidateInput(false)]
        public string UpdateSpecialProvisionMain(int OfficeID, int SpecialProvisionNo, string SpecialProvision, int SeriesID)
        {
            SpecialProvision = HttpUtility.HtmlDecode(SpecialProvision);
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.UpdateSpecialProvisionMain(OfficeID, SpecialProvisionNo, SpecialProvision, SeriesID);
        }

        public string UpdateOfficeDescription(int OfficeID, string OfficeDescription)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.UpdateOfficeDescription(OfficeID, OfficeDescription);
        }
        public string RemoveSection(int SectionID)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.RemoveSection(SectionID);
        }
        public string RemoveSpecialProvision(int SeriesID)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.RemoveSpecialProvision(SeriesID);
        }
        public string RemoveSpecialProvisionMain(int SeriesID)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.RemoveSpecialProvisionMain(SeriesID);
        }

        public string RemoveOfficeDescription(int DescriptionID)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.RemoveOfficeDescription(DescriptionID);
        }
        public string getDescriptionSelectedOffice(int OfficeID)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.getDescriptionSelectedOffice(OfficeID);
        }
        public int GetSpecialProvisionOrderNo(int OfficeID)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.GetSpecialProvisionOrderNo(OfficeID);
        }
        public int GetOrderNoSpecialProvisionMain(int OfficeID)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.GetOrderNoSpecialProvisionMain(OfficeID);
        }

        public ActionResult ConfigureOrdinance()
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            @ViewBag.eid = OrdinanceLayer.getOrdinanceAuthor();
            return View("pvConfigureOrdinance");
        }

        public PartialViewResult AddNewSectionWindow()
        {
            OrdinanceSectionModel OrdinanceSectionModel = new OrdinanceSectionModel();
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            OrdinanceSectionModel.SectionOrder = OrdinanceLayer.getSectionOrder();
            return PartialView("pvAddNewSectionWindow", OrdinanceSectionModel);
        }
        public PartialViewResult EditSection(int SectionID)
        {
            OrdinanceSectionModel OrdinanceSectionModel = new OrdinanceSectionModel();
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            OrdinanceSectionModel = OrdinanceLayer.getSelectedSectionForEdit(SectionID);
            return PartialView("pvAddNewSectionWindow", OrdinanceSectionModel);
        }
        public PartialViewResult EditSpecialProvisionWindow(int SeriesID)
        {
            OrdinanceSpecialProvisionsModel OrdinanceSpecialProvisionsModel = new OrdinanceSpecialProvisionsModel();
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            OrdinanceSpecialProvisionsModel = OrdinanceLayer.getSelectedSpecialProvisionForEdit(SeriesID);
            return PartialView("pvAddSpecialProvisionWindow", OrdinanceSpecialProvisionsModel);
        }
        public PartialViewResult EditSpecialProvisionWindowMain(int SeriesID)
        {
            OrdinanceSpecialProvisionsModel OrdinanceSpecialProvisionsModel = new OrdinanceSpecialProvisionsModel();
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            OrdinanceSpecialProvisionsModel = OrdinanceLayer.getSelectedSpecialProvisionMainForEdit(SeriesID);
            return PartialView("pvAddSpecialProvisionMainWindow", OrdinanceSpecialProvisionsModel);
        }


        public PartialViewResult AddSpecialProvisionWindow()
        {
            OrdinanceSpecialProvisionsModel OrdinanceSpecialProvisionsModel = new OrdinanceSpecialProvisionsModel();
            OrdinanceSpecialProvisionsModel.OrderNo = "1";
            OrdinanceSpecialProvisionsModel.OfficeName = "";
            return PartialView("pvAddSpecialProvisionWindow", OrdinanceSpecialProvisionsModel);
        }
        public PartialViewResult AddSpecialProvisionMainWindow()
        {
            OrdinanceSpecialProvisionsModel OrdinanceSpecialProvisionsModel = new OrdinanceSpecialProvisionsModel();
            OrdinanceSpecialProvisionsModel.OrderNo = "1";
            OrdinanceSpecialProvisionsModel.OfficeName = "";
            return PartialView("pvAddSpecialProvisionMainWindow", OrdinanceSpecialProvisionsModel);
        }
        public PartialViewResult UpdateOfficeDescWindow()
        {
            return PartialView("pvUpdateOfficeDescWindow");
        }
        public ActionResult SectionsTab()
        {
            return PartialView("pv_Ordinance_SectionsTab");
        }
        public ActionResult SpecialProvisionsTab()
        {
            return PartialView("pv_Ordinance_SpecialProvisionsTab");
        }
        public ActionResult SpecialProvisionsMainTab()
        {
            return PartialView("pv_Ordinance_SpecialProvisionsTabMainOffice");
        }

        public ActionResult GroupingPropertiesTab()
        {
            return PartialView("pv_Ordinance_GroupingPropertiesTab");
        }
        public ActionResult OfficeDescriptionTab()
        {
            return PartialView("pv_Ordinance_OfficeDescriptionTab");
        }
        public ActionResult AttendanceTab()
        {
            return PartialView("pv_Ordinance_AttendanceTab");
        }
        public ActionResult SignatoriesTab()
        {
            return PartialView("pv_Ordinance_SignatoriesTab");
        }


        public JsonResult ReadOrdinanceSections([DataSourceRequest] DataSourceRequest request)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            var SectionsList = OrdinanceLayer.ReadSectionsList();
            return Json(SectionsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReadOrdinanceSpecialProvisions([DataSourceRequest] DataSourceRequest request)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            var SectionsList = OrdinanceLayer.ReadOrdinanceSpecialProvisions();
            return Json(SectionsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReadOrdinanceSpecialProvisionsMainOffice([DataSourceRequest] DataSourceRequest request)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            var SectionsList = OrdinanceLayer.ReadOrdinanceSpecialProvisionsMainOffice();
            return Json(SectionsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReadOrdinanceDescriptions([DataSourceRequest] DataSourceRequest request)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            var OrdinanceList = OrdinanceLayer.ReadOrdinanceDescriptions();
            return Json(OrdinanceList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReadOfficeGrouping([DataSourceRequest] DataSourceRequest request)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            var OrdinanceList = OrdinanceLayer.ReadOfficeGrouping();
            return Json(OrdinanceList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string GroupSection(int? FormID, int SectionID)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            //return Convert.ToInt32(FormID).ToString();
            return OrdinanceLayer.GroupSection(FormID, SectionID);
        }
        public string RemoveGroup(int OfficeGroupingID)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.RemoveGroup(OfficeGroupingID);
        }

        public string UpdateOfficeOrder(int OfficeID, int OrderNo)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.UpdateOfficeOrder(OfficeID, OrderNo);
        }
        public string GroupOffice(int MainOfficeID, int SubOfficeID)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.GroupOffice(MainOfficeID, SubOfficeID);
        }
        public string CheckSubFormType(int SectionID)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.CheckSubFormType(SectionID);
        }
        public string CheckMainGroup(int FormID)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.CheckMainGroup(FormID);
        }
        public string AddToAttendance(int eid, string Designation, int Status)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.AddToAttendance(eid, Designation, Status);
        }
        public string RemoveEmployeeAttendance(int SeriesID)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.RemoveEmployeeAttendance(SeriesID);
        }

        [HttpPost, ValidateInput(false)]
        public string UpdateAttendanceHeader(string ReportTittle, string ReportDescription)
        {
            ReportDescription = HttpUtility.HtmlDecode(ReportDescription);
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.UpdateAttendanceHeader(ReportTittle, ReportDescription);
        }
        public JsonResult GetAttendanceHeader()
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            var OrdinanceList = OrdinanceLayer.GetAttendanceHeader();
            return Json(OrdinanceList.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReportSignatories()
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            var OrdinanceList = OrdinanceLayer.GetReportSignatories();
            return Json(OrdinanceList.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public string UpdateReportSignatory(string Signatory1Name, string Signatory1Pos, string Signatory2Name,
            string Signatory2Pos, string Signatory2Rule, string Signatory3Name, string Signatory3Pos)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.UpdateOrdinanceSignatories(Signatory1Name, Signatory1Pos, Signatory2Name, Signatory2Pos,
                Signatory2Rule, Signatory3Name, Signatory3Pos);
        }
        public string UpdateAttendanceOrder(int seriesID, int OrderNo)
        {
            OrdinanceLayer OrdinanceLayer = new OrdinanceLayer();
            return OrdinanceLayer.UpdateAttendanceOrder(seriesID, OrderNo);
        }
        #endregion

        #region Manage Accounts
        [Authorize]
        public ActionResult AddAccounts()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc);
            foreach (var item in mnu)
            {
                if (item.MenuID == 3004)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                return View("pvAddAccounts");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }

        public ActionResult ManageAccounts()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc);
            foreach (var item in mnu)
            {
                if (item.MenuID == 4012)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                return View("pvManageAccounts");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        public ActionResult ManageProgramAccounts()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc);
            foreach (var item in mnu)
            {
                if (item.MenuID == 4013)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                return View("pvManageProgramAccounts");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }

        }
        public string ResultAccountName(string AccountCode)
        {
            SearchAccountName ddl = new SearchAccountName();
            return ddl.search_AccountName(AccountCode);
        }
        public int ResultAccountID(string OfficeID, string AccountCode)
        {
            SearchAccountName ddl = new SearchAccountName();
            return ddl.search_AccountID(OfficeID, AccountCode);
        }
        public int SearchOrderNo(string AccountName)
        {
            SearchAccountName ddl = new SearchAccountName();
            return ddl.search_OrderByNo(AccountName);
        }
        public string createNewAccounts(int? AccountID, string AccountName, int? OfficeID, int? ProgramID, int? OOEID, double? amount, string OOEName, int? yearof)
        {
            CreateNewAccount_Layer VIL = new CreateNewAccount_Layer();
            return VIL.createNewAccounts(AccountID, AccountName, OfficeID, ProgramID, OOEID, amount, OOEName, yearof);
        }
        public string createSuggestNewAccounts(createNewAccount NewAccount)
        {
            CreateNewAccount_Layer VIL = new CreateNewAccount_Layer();
            return VIL.createSuggestNewAccounts(NewAccount);
        }
        public string DeleteAccount(int account_ID)
        {
            DeleteAccount_Layer VIL = new DeleteAccount_Layer();
            return VIL.DeleteAccount(account_ID);
        }
        public ActionResult AddProgramAccount()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc);
            foreach (var item in mnu)
            {
                if (item.MenuID == 3007)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                return View("pvAddProgramAccounts");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        public ActionResult CopyProgramAccounts()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc);
            foreach (var item in mnu)
            {
                if (item.MenuID == 3010)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                return View("pvCopyProgramaccounts");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        public string RestoreAccount(int account_ID)
        {
            RestoreAccount_Layer VIL = new RestoreAccount_Layer();
            return VIL.RestoreAccount(account_ID);
        }
        public string SaveNewAccount(account_code Account_info)
        {
            SaveAccount_Layer VIL = new SaveAccount_Layer();
            return VIL.SaveNewAccount(Account_info);
            // continue
        }
        public string CopyProgramAccount(copyProgram CopyProgramAccounts)
        {
            CopyProgramAccount_Layer VIL = new CopyProgramAccount_Layer();
            return VIL.CopyProgramAccount(CopyProgramAccounts);

        }
        //public JsonResult SearchActiveAccount([DataSourceRequest] DataSourceRequest request)
        //{
        //    grAccounts account_list = new grAccounts();
        //    var account_lst = account_list.grActiveAccount_list();
        //    return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult SearchAccountDelete([DataSourceRequest] DataSourceRequest request)
        //{
        //    grAccounts account_list = new grAccounts();
        //    var account_lst = account_list.grAccountDelete_list();
        //    return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult SearchAccountNameID([DataSourceRequest] DataSourceRequest request, string AccountData)
        //{
        //    grAccounts account_list = new grAccounts();
        //    var account_lst = account_list.grAccount_listData(AccountData);
        //    return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult SearchAccountNameIDbyOOE([DataSourceRequest] DataSourceRequest request, string AccountData, string ProgramID)
        //{
        //    grAccounts account_list = new grAccounts();
        //    var account_lst = account_list.grAccount_listOOEData(AccountData, ProgramID);
        //    return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult SearchAccount([DataSourceRequest] DataSourceRequest request, string FundDropdown)
        //{
        //    grAccounts account_list = new grAccounts();
        //    var account_lst = account_list.grAccount_list(FundDropdown);
        //    return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult SearchBuildPropose([DataSourceRequest] DataSourceRequest request, string FundDropdown)
        //{
        //    grAccounts account_list = new grAccounts();
        //    var account_lst = account_list.grBuildProposed(FundDropdown);
        //    return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult SearchAcountPropose([DataSourceRequest] DataSourceRequest request, string FundDropdown)
        //{
        //    grAccounts account_list = new grAccounts();
        //    var account_lst = account_list.grAcountProposed(FundDropdown);
        //    return Json(account_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
        public ActionResult ActiveAccountsTab()
        {
            return PartialView("pv_ActiveAccountsTab");
        }
        public ActionResult pvChartOfAccounts(int ProgramID, int OOEID, int YearOf)
        {
            UpdateAccount_Layer UpdateAccount = new UpdateAccount_Layer();
            ViewBag.AccountsList = UpdateAccount.getExistingAccounts(ProgramID, OOEID, YearOf);
            return PartialView("pvChartOfAccounts");
        }
        public JsonResult getChartOfAccountsData([DataSourceRequest] DataSourceRequest request)
        {
            UpdateAccount_Layer UpdateAccount = new UpdateAccount_Layer();
            var AccountsList = UpdateAccount.getChartOfAccountsData();
            var json = Json(AccountsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return (json);
        }
        public string getAccountOrderNo(int ProgramID, int OOEID, int YearOf)
        {
            UpdateAccount_Layer UpdateAccount = new UpdateAccount_Layer();
            return UpdateAccount.getAccountOrderNo(ProgramID, OOEID, YearOf);
        }
        public ActionResult MergeAccountIndex()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc);
            foreach (var item in mnu)
            {
                if (item.MenuID == 12036)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                return View("pv_MergeAccountIndex");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        public ActionResult NonActiveAccountsTab()
        {
            return PartialView("pv_NonActiveAccountsTab");
        }
        public ActionResult NewAccountsTab()
        {
            return PartialView("pv_NewAccountsTab");
        }
        public JsonResult GetNewCreatedAccountsFromProposal([DataSourceRequest] DataSourceRequest request)
        {
            grAccounts grAccounts = new grAccounts();
            var AccountsList = grAccounts.GetNewCreatedAccountsFromProposal();
            return Json(AccountsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult grAccountsToCombine([DataSourceRequest] DataSourceRequest request)
        {
            UpdateAccount_Layer UpdateAccount = new UpdateAccount_Layer();
            var AccountsList = UpdateAccount.grAccountsToCombine();
            return Json(AccountsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string SetMergeStatus(int SeriesID, int isCombine)
        {
            UpdateAccount_Layer UpdateAccount = new UpdateAccount_Layer();
            return UpdateAccount.SetMergeStatus(SeriesID, isCombine);
        }
        public string UpdateAccountOrder(int OrderNo, int AccountID)
        {
            UpdateAccount_Layer UpdateAccount = new UpdateAccount_Layer();
            return UpdateAccount.UpdateAccountOrder(OrderNo, AccountID);
        }
        public ActionResult reqShowTargetAccountWindow()
        {
            return PartialView("pvTargetAccountWindow");
        }
        public string UpdateAccountReference(int ReferenceAccount, string[] SelectedAccounts)
        {
            UpdateAccount_Layer UpdateAccount = new UpdateAccount_Layer();
            return UpdateAccount.UpdateAccountReference(ReferenceAccount, SelectedAccounts);
        }
        public JsonResult grTargetAccount([DataSourceRequest] DataSourceRequest request)
        {
            grAccounts grAccounts = new grAccounts();
            var AccountsList = grAccounts.grTargetAccount();
            var json = Json(AccountsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return (json);
            //return Json(AccountsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowBuildAccountWindow(int AccountID)
        {
            UpdateAccount_Layer UpdateAccount_Layer = new UpdateAccount_Layer();
            return PartialView("pv_BuildNewAccounts", UpdateAccount_Layer.getSelectedAccountData(AccountID));
        }
        public string BuildNewAccount(string AccountName, int AccountID, string AccountCode
            , string ChildAccountCode, int ProgramID, int ThirdLevelGroupID, string ThirdLevelGroupDesc
            , string FundType, int OrderNo, int RefYearOf, int RefProgramID)
        {
            UpdateAccount_Layer UpdateAccount_Layer = new UpdateAccount_Layer();
            return UpdateAccount_Layer.BuildNewAccount(AccountName, AccountID, AccountCode
            , ChildAccountCode, ProgramID, ThirdLevelGroupID, ThirdLevelGroupDesc
            , FundType, OrderNo, RefYearOf, RefProgramID);
        }
        #endregion

        #region AIP - Climate Change
        public ActionResult CCCodesIndex()
        {
            return View("pvCCCodesIndex");
        }
        public JsonResult grdCCCOdeListData([DataSourceRequest] DataSourceRequest request)
        {
            AIPPreparationLayer AIPPreparationLayer = new AIPPreparationLayer();
            var lst = AIPPreparationLayer.grdCCCOdeListData();
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult ShowAddNewCCCodeWindow(string Mode, string CCCOdeID)
        {
            //ViewBag.Mode = Mode + "," + CCCOdeID;
            ViewBag.Mode = Mode;
            ViewBag.CCCodeID = CCCOdeID;
            return PartialView("pvNewCCCodeWindow");
        }
        public string AddNewCCCode(string CCTypologyCode, string Description, int CCType, int OrderNo)
        {
            AIPPreparationLayer AIPPreparationLayer = new AIPPreparationLayer();
            return AIPPreparationLayer.AddNewCCCode(CCTypologyCode, Description, CCType, OrderNo);
        }
        public string getLatestOrderNoCCCode(int CCTYpe)
        {
            AIPPreparationLayer AIPPreparationLayer = new AIPPreparationLayer();
            return AIPPreparationLayer.getLatestOrderNoCCCode(CCTYpe);
        }
        public string RemoveCCCode(int CCCodeID)
        {
            AIPPreparationLayer AIPPreparationLayer = new AIPPreparationLayer();
            return AIPPreparationLayer.RemoveCCCode(CCCodeID);
        }
        public JsonResult GetSelectedCCCodeData(int CCCodeID)
        {
            AIPPreparationLayer Layer = new AIPPreparationLayer();
            var lst = Layer.GetSelectedCCCodeData(CCCodeID);
            List<string> AIPItemData = new List<string>();
            AIPItemData.Add(lst.CCCode); //[0]
            AIPItemData.Add(lst.CCCDescription);//[1]
            AIPItemData.Add(lst.CCCodeID.ToString());//[2]
            AIPItemData.Add(lst.OrderNo.ToString());//[3]
            return Json(AIPItemData, JsonRequestBehavior.AllowGet);
        }
        public string UpdateCCCode(string CCTypologyCode, string Description, int CCType, int OrderNo, int CCCodeID)
        {
            AIPPreparationLayer AIPPreparationLayer = new AIPPreparationLayer();
            return AIPPreparationLayer.UpdateCCCode(CCTypologyCode, Description, CCType, OrderNo, CCCodeID);
        }
        public string UpdateCCCodeOrderNo(int CCCodeID, int OrderNo)
        {
            AIPPreparationLayer AIPPreparationLayer = new AIPPreparationLayer();
            return AIPPreparationLayer.UpdateCCCodeOrderNo(CCCodeID, OrderNo);
        }
        #endregion

        #region Execution Utilities desu
        public PartialViewResult BuildUpPPSASCode(int? FMISAccountCode, string PPSASCode, string PPASeriesCode)
        {
            PPSASCode_Model data = new PPSASCode_Model();
            data.FMISAccountCode = FMISAccountCode;
            data.PPSASCode = PPSASCode;
            data.PPASeriesCode = PPASeriesCode;
            return PartialView("pvBuildUpPPSASCode", data);
        }
        public string UpdatePPSASCode(int? AccountCode, string PPSASCode, int? TransactionYear)
        {
            PPSASCode_Layer data = new PPSASCode_Layer();
            return data.UpdatePPSASCode(AccountCode, PPSASCode, TransactionYear);
        }
        public string SetPPSASCode(string PPSASAccount)
        {
            PPSASCode_Layer data = new PPSASCode_Layer();
            return data.SetPPSASCode(PPSASAccount);
        }
        public string SavePPSASCode(string PPSASSeries, string PPSASCode, string ChildPPSASCode, int? FMISAccount, int? YearOf)
        {
            PPSASCode_Layer data = new PPSASCode_Layer();
            return data.SavePPSASCode(PPSASSeries, PPSASCode, ChildPPSASCode, FMISAccount, YearOf);
        }
        public JsonResult searchOBR(string OBRNo)
        {
            TransferTransactionCharge_Layer data = new TransferTransactionCharge_Layer();
            var value = data.searchOBR(OBRNo);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult searchOBRExcess(string OBRNo)
        {
            TransferTransactionCharge_Layer data = new TransferTransactionCharge_Layer();
            var value = data.searchOBRExcess(OBRNo);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult grTransactionCharge([DataSourceRequest] DataSourceRequest request, int? trnno, int? Year)
        {
            TransferTransactionCharge_Layer data = new TransferTransactionCharge_Layer();
            var value = data.grTransactionCharge(trnno, Year);
            return Json(value.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public double SetAmountCharge(int? _trnno, int? AcctCharge)
        {
            TransferTransactionCharge_Layer data = new TransferTransactionCharge_Layer();
            return data.SetAmountCharge(_trnno, AcctCharge);
        }
        public double SetAmountCharge_Excess(int? _trnno, int? AcctCharge)
        {
            TransferTransactionCharge_Layer data = new TransferTransactionCharge_Layer();
            return data.SetAmountCharge_Excess(_trnno, AcctCharge);
        }
        public double CheckRemainingBalance(int? OfficeID, int? ProgramID, int? Account, int? Year, int? IsIncome)
        {
            TransferTransactionCharge_Layer data = new TransferTransactionCharge_Layer();
            return data.CheckRemainingBalance(OfficeID, ProgramID, Account, Year, IsIncome);
        }
        public double CheckRemainingBalance_Excess(int? Account, int? Year)
        {
            TransferTransactionCharge_Layer data = new TransferTransactionCharge_Layer();
            return data.CheckRemainingBalance_Excess(Account, Year);
        }
        public JsonResult AddTransferTransaction(int? OfficeID, int? ProgramID, int? AccountID, int? trnno, int? _Office, int? _Program, int? _Accounts, int? IsIncome)
        {
            TransferTransactionCharge_Layer data = new TransferTransactionCharge_Layer();
            var value = data.AddTransferTransaction(OfficeID, ProgramID, AccountID, trnno, _Office, _Program, _Accounts, IsIncome);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddTransferTransaction_Excess(int? OfficeID, int? AcctCharge, int? PPA, int? NewOffice, int? NewAcctCharge, int? NewPPA, int? Claim)
        {
            TransferTransactionCharge_Layer data = new TransferTransactionCharge_Layer();
            var value = data.AddTransferTransaction_Excess(OfficeID, AcctCharge, PPA, NewOffice, NewAcctCharge, NewPPA, Claim);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveTransfer(string OBRNo, int? Years)
        {
            TransferTransactionCharge_Layer data = new TransferTransactionCharge_Layer();
            var value = data.SaveTransfer(OBRNo, Years);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteTransfer(int? TempID)
        {
            TransferTransactionCharge_Layer data = new TransferTransactionCharge_Layer();
            var value = data.DeleteTransfer(TempID);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Preparation Cieling
        public ActionResult PreparationCielingIndex()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc);
            foreach (var item in mnu)
            {
                if (item.MenuID == 12037)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                return View("pv_PreparationCielingIndex");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        public JsonResult grOfficeCieling([DataSourceRequest] DataSourceRequest request, int isNonOffice)
        {
            OfficeProposalCielingLayer Layer = new OfficeProposalCielingLayer();
            var List = Layer.grOfficeCieling(isNonOffice);
            return Json(List.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string UpdateOfficeCielingPercentage(int SeriesID, int Percentage)
        {
            OfficeProposalCielingLayer Layer = new OfficeProposalCielingLayer();
            return Layer.UpdateOfficeCielingPercentage(SeriesID, Percentage);
        }
        public double getTotalProposedPerOffice(int OfficeID, int YearOf, int ProgramID,string ProgramDescription)
        {
            OfficeProposalCielingLayer Layer = new OfficeProposalCielingLayer();
            return Layer.getTotalProposedPerOffice(OfficeID, YearOf, ProgramID, ProgramDescription);
        }
        public double getOfficeTotalCieling(int OfficeID, int YearOf, int ProgramID)
        {
            OfficeProposalCielingLayer Layer = new OfficeProposalCielingLayer();
            return Layer.getOfficeTotalCieling(OfficeID, YearOf, ProgramID);
        }
        public string getOffiCeiling(int OfficeID,int proyear)
        {
            OfficeProposalCielingLayer Layer = new OfficeProposalCielingLayer();
            return Layer.getOffiCeiling(OfficeID, proyear);
        }
        public string UpdateCeilingStatus(int SeriesID, int ActionCode)
        {
            OfficeProposalCielingLayer Layer = new OfficeProposalCielingLayer();
            return Layer.UpdateCeilingStatus(SeriesID, ActionCode);
        }
        public ActionResult pv_CeilingGFTab()
        {
            return PartialView("pv_CeilingGFTab");
        }
        public ActionResult pv_CeilingEETab()
        {
            return PartialView("pv_CeilingEETab");
        }
        public string UpdateTotalProposed(int OfficeID, int YearOf, int ProgramID,string ProgramDescription)
        {  
            OfficeProposalCielingLayer Layer = new OfficeProposalCielingLayer();
            return Layer.UpdateTotalProposed(OfficeID, YearOf, ProgramID, ProgramDescription);
        }
        #endregion
        public ActionResult NonOfficeSub()
        {
            return View("pv_NonOfficeSub");
        }

        public ActionResult BuilNonOffice([DataSourceRequest]DataSourceRequest request, int? program = 0, int? accountTemp = 0, int? year = 0,int? excessid=0,int? showallid=0,int? motherAccountid=0,int? showllexcessid=0,int? fund=0)
        {
            //if (showallid == 1)
            //{
                string SQL = "";
                SQL = "exec sp_BMS_ExcessSubPPA "+ program + ","+ accountTemp + ","+ excessid + ","+ year + ","+ showallid + ","+ motherAccountid + ","+ showllexcessid + ","+ fund + "";
                DataTable dt = SQL.DataSet();

                var serializer = new JavaScriptSerializer();
                var result = new ContentResult();
                serializer.MaxJsonLength = Int32.MaxValue;
                result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;
        }

        public ActionResult BuilNonOffice_SubGrid([DataSourceRequest]DataSourceRequest request, int? program = 0, int? accountTemp = 0, int? year = 0,int? showallid = 0, int? motherAccountid = 0)
        {
            //if (showallid == 1)
            //{
            string SQL = "";
            SQL = "exec sp_BMS_ExcessSubPPA_v2 " + program + "," + accountTemp + "," + year + "," + showallid + "," + motherAccountid + "";
            DataTable dt = SQL.DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult NonOfficeDetails(int SPO_ID, int ProgramID, int AccountID, decimal Appropriation, int YearOf)
        {
            string SQL = "";
            //DateTime DateofApplication = Convert.ToDateTime(GET.Current_Date);
            SQL = "select [SPO_ID],[OfficeID],[ProgramID],[AccountID],AccountName,[Appropriation],[YearOf] from [vwBMS_NonSubAccount] where SPO_ID=" + SPO_ID + " and [ProgramID]=" + ProgramID + " and [AccountID]=" + AccountID + " and [YearOf]=" + YearOf + "";
            //ViewBag.AccountName = AccountName;
            //Session["propno"] = prop_id;
            DataTable dt = SQL.DataSet();
            return View("pv_NonOfficeSub", dt.Rows.Count == 0 ? null : dt.ToModel());
        }
        public ActionResult Execution()
        {
            return View("pv_ExecutionMaintenance");
        }
        public ActionResult SearchControlTrans([DataSourceRequest]DataSourceRequest request, string paramtr = "", int? opt = 0)
        {
            string SQL = "";
            SQL = "select [tracking_id],[PayrollBatchNo],[obrno],DTE,[location] from [fmis].[dbo].[tblAMIS_PayrollLocation] where [PayrollBatchNo]=" + paramtr + " and [Actioncode] < 3 order by [tracking_id]  ";
            DataTable dt = SQL.DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public string DeleteBatchno(long id)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = "";
                SqlCommand com = new SqlCommand(@"Update [fmis].[dbo].[tblAMIS_PayrollLocation] set [Actioncode]=3 where [tracking_id]="+ id +"", con);
                con.Open();
                data = Convert.ToString(com.ExecuteScalar());
                return data;


            }
        }
        public ActionResult SearchLoggerTrans([DataSourceRequest]DataSourceRequest request, string paramtr = "", int? opt = 0)
        {
            string SQL = "";
            SQL = "select trnno,obrno,obrseries,NonOfficeTransNo,ReferenceNo,TransactionNo from [IFMIS].[dbo].[tbl_R_BMSObrLogs] where [TransactionNo]=" + paramtr + " ";
            DataTable dt = SQL.DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        
        public string editTransno(long id)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = "";
                SqlCommand com = new SqlCommand(@"Update [fmis].[dbo].[tblAMIS_PayrollLocation] set [Actioncode]=3 where [tracking_id]=" + id + "", con);
                con.Open();
                data = Convert.ToString(com.ExecuteScalar());
                return data;
            }
        }
        public JsonResult UpdateLoggerTrans([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<account_code> LoggerTransaction)
        {
            Utilities ul = new Utilities();
            try
            {
                ul.UpdateLogTrans(LoggerTransaction);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(LoggerTransaction.ToDataSourceResult(request, ModelState));
        }
        
        public ActionResult SearchCurrentCtrl([DataSourceRequest]DataSourceRequest request, string paramtr = "", int? opt = 0)
        {
            string SQL = "";
            SQL = "select trnno,obrno,actioncode,datetimeentered from [IFMIS].[dbo].[tbl_T_BMSSubsidiaryLedger] where [obrno]='" + paramtr + "' ";
            DataTable dt = SQL.DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        
        public JsonResult deleteCurrentCtrl([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<account_code> CurrentTransaction)
        {
            Utilities ul = new Utilities();
            try
            {
                ul.deleteCurrentCtrl(CurrentTransaction);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(CurrentTransaction.ToDataSourceResult(request, ModelState));
        }
        public string InsertSubAccount(int? office=0, int? program=0,int? accountTemp=0,int? MotherAccount=0,string particularname="",double amount=0,int? ProposalYear=0,long? nonofficeidtemp = 0,int? actioncode=0, int? accountnameid=0,int? excessid = 0,long? accountactual=0)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.sp_BMS_InsertSubAccounts " + office + ", " + program + "," + accountTemp + ", " + MotherAccount + ",'" + particularname.Replace("'","''").ToString() + "'," + amount + "," + ProposalYear + "," + Account.UserInfo.eid.ToString() + "," + nonofficeidtemp + "," + actioncode + ","+ accountnameid + ","+ excessid + ","+ accountactual + "", con);
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
        public string DeleteSubAccount(long accountnameid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"sp_BMS_DeleteSubAccount " + accountnameid + ",'" + (Account.UserInfo.eid.ToString()) + "'", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                    //if (data != 0)
                    //{
                    //    return "success";
                    //}
                    //else
                    //{
                    //    return "Error!";
                    //}

                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        
        public double getParticularBalance(long MotherAccount, int ProposalYear)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = 0.00;
                SqlCommand com = new SqlCommand(@"select dbo.fn_BMS_SubAccountBal("+ MotherAccount + ","+ ProposalYear + ")", con);
                con.Open();
                data = Convert.ToDouble(com.ExecuteScalar());
                return data;
            }
        }
        public JsonResult SubAccountSAAB(long MotherAccount,int ProposalYear)
        {

            CurrentControl_Model data = new CurrentControl_Model();
            DataTable _dt = new DataTable();
            string _sqlQuery = "Select * from dbo.fn_BMS_SubAccountAAB (" + MotherAccount + "," + ProposalYear + ")";
            if (MotherAccount != 0)
            {
                _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                try
                {
                    data.Appropriation = Convert.ToDouble(_dt.Rows[0][0]);
                    data.AllotedAmount = Convert.ToDouble(_dt.Rows[0][1]);
                    data.BalanceAllotment = Convert.ToDouble(_dt.Rows[0][2]);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubAccountSAAB_WFP(long MotherAccount, int ProposalYear)
        {

            CurrentControl_Model data = new CurrentControl_Model();
            DataTable _dt = new DataTable();
            //string _sqlQuery = "Select * from ifmis.dbo.fn_BMSWFP_SubAccountAAB (3687," + ProposalYear + ")";
            string _sqlQuery = "exec  ifmis.dbo.sp_BMSWFP_SubAccountAAB " + MotherAccount + "," + ProposalYear + "";
            if (MotherAccount != 0)
            {
                _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                try
                {
                    data.Appropriation = Convert.ToDouble(_dt.Rows[0][0]);
                    data.AllotedAmount = Convert.ToDouble(_dt.Rows[0][1]);
                    data.BalanceAllotment = Convert.ToDouble(_dt.Rows[0][2]);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SubAccountSAAB_WFP20(long MotherAccount, int ProposalYear)
        {

            CurrentControl_Model data = new CurrentControl_Model();
            DataTable _dt = new DataTable();
            //string _sqlQuery = "Select * from ifmis.dbo.fn_BMSWFP_SubAccountAAB (3687," + ProposalYear + ")";
            string _sqlQuery = "exec  ifmis.dbo.sp_BMSWFP_SubAccountAAB_20 " + MotherAccount + "," + ProposalYear + "";
            if (MotherAccount != 0)
            {
                _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                try
                {
                    data.Appropriation = Convert.ToDouble(_dt.Rows[0][0]);
                    data.AllotedAmount = Convert.ToDouble(_dt.Rows[0][1]);
                    data.BalanceAllotment = Convert.ToDouble(_dt.Rows[0][2]);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SubAccountSAAB_WFPexcess(long MotherAccount, int ProposalYear)
        {

            CurrentControl_Model data = new CurrentControl_Model();
            DataTable _dt = new DataTable();
            //string _sqlQuery = "Select * from ifmis.dbo.fn_BMSWFP_SubAccountAAB (3687," + ProposalYear + ")";
            string _sqlQuery = "exec  ifmis.dbo.[sp_BMSWFP_SubAccountAAB_excess] " + MotherAccount + "," + ProposalYear + "";
            if (MotherAccount != 0)
            {
                _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                try
                {
                    data.Appropriation = Convert.ToDouble(_dt.Rows[0][0]);
                    data.AllotedAmount = Convert.ToDouble(_dt.Rows[0][1]);
                    data.BalanceAllotment = Convert.ToDouble(_dt.Rows[0][2]);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AccountSAAB(long accountnameid, int ProposalYear)
        {

            CurrentControl_Model data = new CurrentControl_Model();
            DataTable _dt = new DataTable();
            string _sqlQuery = "Select * from dbo.fn_BMS_SubAccountAAB (" + accountnameid + "," + ProposalYear + ")";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try {
                data.Appropriation = Convert.ToDouble(_dt.Rows[0][0]);
                data.AllotedAmount = Convert.ToDouble(_dt.Rows[0][1]);
                data.BalanceAllotment = Convert.ToDouble(_dt.Rows[0][2]);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TransactionNOCharge()
        {
            return View("pv_NOTransferCharge");
        }
        public ActionResult GetNOTransaction([DataSourceRequest]DataSourceRequest request, string obrno = "",int status=0)
        {
            string SQL = "";
            SQL = "exec sp_BMS_GridObrNo '" + obrno + "',"+ status + "";
            DataTable dt = SQL.DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public JsonResult GetNOSAOB(int office, long accountid, int ProposalYear,int status)
        {
            
            CurrentControl_Model data = new CurrentControl_Model();
            DataTable _dt = new DataTable();
            //string _sqlQuery = "SELECT [TotalRelease],[TotalObligation],[RemainingBalance],[OfficeID],[AccountCode] FROM [IFMIS].[dbo].[vwBMS_NonOffice_Transaction_IFMIS] where OfficeID=" + office + " and AccountCode=" + accountid + " and yearof=" + ProposalYear + "";
            string _sqlQuery = "exec ifmis.dbo.sp_BMS_SubPPAsBalance " + office + "," + accountid + "," + ProposalYear + ","+ status + "";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.Appropriation = Convert.ToDouble(_dt.Rows[0][0]);
                data.ObligatedAmount = Convert.ToDouble(_dt.Rows[0][1]);
                data.BalanceAllotment = Convert.ToDouble(_dt.Rows[0][2]);
            }
            catch (Exception ex)
            {
                 ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public string UpdateNOTransaction(long trnno, int office, string obrno,int status)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.[sp_BMS_UpdateNonOfficeTrans] " + trnno + ", " + office + "," + Account.UserInfo.eid.ToString() + ",'" + obrno + "',"+ status + "", con);
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
        public JsonResult GetApprovedBudget(int? program=0, long? accountTemp=0, int? ProposalYear=0,int? tempradid=0)
        {

            CurrentControl_Model data = new CurrentControl_Model();
            DataTable _dt = new DataTable();
            string _sqlQuery = "exec sp_NOAccountBal " + program + "," + accountTemp + "," + ProposalYear + ","+ tempradid + "";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.Appropriation = Convert.ToDouble(_dt.Rows[0][0]);
                data.BalanceAllotment = Convert.ToDouble(_dt.Rows[0][2]);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public string UpdateLInkAccount(int officeID, int ProgramID, long AccountID, int ProgramIDTo, long AccountIDTo,int proYer,int officeTo,int linkyear)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.sp_BMS_UpdateProposedAccount "+ officeID  + "," + ProgramID + ", " + AccountID + ","+ ProgramIDTo + ","+ AccountIDTo + ","+ proYer  + "," + Account.UserInfo.eid.ToString() + ","+ officeTo + ","+ linkyear + "", con);
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
        public string UpdateMergeAccount(int officeID, int ProgramID, long AccountID, int ProgramIDTo, long AccountIDTo, int proYer, int officeTo, int linkyear)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.[sp_BMS_MergeProposedAccount] " + officeID + "," + ProgramID + ", " + AccountID + "," + ProgramIDTo + "," + AccountIDTo + "," + proYer + "," + Account.UserInfo.eid.ToString() + "," + officeTo + "," + linkyear + "", con);
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
        public string TransferProgram(int officeID, int ProgramID, long AccountID, int ProgramIDTo, int proYer)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.sp_BMS_TransferProgram " + officeID + "," + ProgramID + ", " + AccountID + "," + ProgramIDTo + "," + proYer + "", con);
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
        public string RemoveLInkAccount(int officeID, int ProgramID, long AccountID)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"delete [IFMIS].[dbo].[tbl_R_BMSAccountsToMerge] where OfficeIDFrom="+ officeID + " and ProgramIDFrom=" + ProgramID + " and AccountIDFrom="+ AccountID + "", con);
                    con.Open();
                    data =  Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ApprovedBudgetPosting()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_ApproveAccountsOtherPostFmis] 2021," + Account.UserInfo.eid.ToString() +"", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string returntransaction(string ctrl="",int? tempobrid=0,int? refid=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReturnEarmark] '" + ctrl  + "',"+ tempobrid + ","+ refid + "", con);
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
        public PartialViewResult pvNOTransferPPACharge()
        {
            return PartialView("pv_NOTransferPPAChargev2");
        }
        public ActionResult getofficeppa([DataSourceRequest]DataSourceRequest request)
        {
            string tempStr = "";
            tempStr = "select OfficeID,Office from fn_BMS_ppaoffice() ";
            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        
        public ActionResult getofficeAccountppa([DataSourceRequest]DataSourceRequest request, int? office=0)
        {
            string tempStr = "";
            tempStr = "select * from fn_BMS_ppaOfficeAccount ("+ office + ")";
            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult dfpptperiod() {
            return View("pv_Dfpptperiod");
        }
        public string updateperiod(int? one = 0, int? two = 0, int? tre = 0, int? fur = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_DfpptPeriod] " + one + "," + two + "," + tre + ","+ fur + "", con);
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
        public JsonResult getdfpptperiod()
        {

            CurrentControl_Model data = new CurrentControl_Model();
            DataTable _dt = new DataTable();
            string _sqlQuery = "SELECT [period],[actioncode] FROM [IFMIS].[dbo].[tbl_R_BMSDfpptPeriod] order by [id]";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.first = Convert.ToInt16(_dt.Rows[0][1]);
                data.second = Convert.ToInt16(_dt.Rows[1][1]);
                data.third = Convert.ToInt16(_dt.Rows[2][1]);
                data.fourth = Convert.ToInt16(_dt.Rows[3][1]);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Transdate()
        {
            return View("pv_TransferDate");
        }
        public ActionResult EditTransDate([DataSourceRequest]DataSourceRequest request, string obrno = "")
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = "";
                SqlCommand com = new SqlCommand(@"select format(getdate(),'MM/dd/yyyy hh:mm:ss tt')", con);
                con.Open();
                data = Convert.ToString(com.ExecuteScalar());
                Session["datetime"] = data;
            }

            string SQL = "";
            SQL = "exec [sp_BMS_ViewTransDate] '" + obrno + "'";
            DataTable dt = SQL.DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public string savetransdate(string obrno = "",string tdate="")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_UpdateTransDate] '" + obrno + "','"+ tdate + "',"+ Account.UserInfo.eid +"", con);
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
        public ActionResult excessStatusreport()
        {
            return PartialView("pv_ExcessStatus");
        }
        public PartialViewResult ExcessStatusUnlink()
        {
            return PartialView("pv_ExcessStatusUnlink");
        }
        public PartialViewResult ExcessStatuslink()
        {
            return PartialView("pv_ExcessStatusLink");
        }
        public ActionResult ExcessStatus([DataSourceRequest]DataSourceRequest request, int? program = 0, int? accountTemp = 0, int? year = 0,int? fund = 0,int? status=0)
        {
            string SQL = "";
            SQL = "exec sp_BMS_ExcessStatus " + program + "," + accountTemp + "," + year + "," + fund + ","+ status + "";
            DataTable dt = SQL.DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public string deleteprogramdesc(int? id,int? year)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    var data = "";
                            SqlCommand com = new SqlCommand(@"[sp_BMS_DeleteProgramdecription] " + id + ",'" + Account.UserInfo.eid.ToString() + "'," + year + "", con);
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
        public ActionResult WFPsignatory()
        {
            return View("vwwfpsignatory");
        }
        public ActionResult wfpsig([DataSourceRequest]DataSourceRequest request)
        {
            string tempStr = "SELECT [eid],[EmpNameFull] FROM [pmis].[dbo].[vwMergeAllEmployee_Modified] where Status like '%elected%' order by EmpNameFull";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public string SaveWFPsignatory(int? eid=0, string signame="")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    var data = "";
                    SqlCommand com = new SqlCommand(@"[sp_BMS_WFPsignatory] " + eid + ",'" + signame + "',8", con);
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
        public int getgovsig()
        {

            DataTable prep_id = new DataTable();
            int govid = 0;
            string _sqlprep = "select [eid] from [IFMIS].[dbo].[tbl_R_BMS_WFPsignatory] where [orderno]=8";
            prep_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlprep).Tables[0];
            if (prep_id.Rows.Count > 0)
            {
                govid = Convert.ToInt32(prep_id.Rows[0][0]);
            }
            else
            {
                govid = 0;
            }
            return govid;
        }
        public ActionResult TemplateColor()
        {
            return View("pvTemplateColor");
        }

        public string changetemplatecolor(string tempcolor="")
        {
            try
            {
                var data = "";
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    
                        SqlCommand com = new SqlCommand(@"update  [IFMIS].[dbo].[tbl_R_BMS_TemplateColor] set [Color]='"+ tempcolor + "' where [id]=1", con);
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
        public string gettemplatecolor()
        {
            DataTable color_id = new DataTable();
            var tempcolor = "";
            string _sqlprep = "SELECT [Color] FROM [IFMIS].[dbo].[tbl_R_BMS_TemplateColor] where id=1";
            color_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlprep).Tables[0];
            if (color_id.Rows.Count == 1)
            {
                tempcolor =  Convert.ToString(color_id.Rows[0][0]);
            }
            else
            {
                tempcolor = "#1fa67a";
            }
            return tempcolor;

        }
        public JsonResult gettemplatecolor_login()
        {

            UserLogInfo data = new UserLogInfo();
            DataTable _dt = new DataTable();
            string _sqlQuery = "";

            _sqlQuery = "SELECT [Color] FROM [IFMIS].[dbo].[tbl_R_BMS_TemplateColor] where id=1";

            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.tempcolor = Convert.ToString(_dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Wfpexempt()
        {
            return View("vw_wfpadditem_exempt");
        }
        
        public string savewfpexemption(int? officeid=0, int? programid=0, int? accountid=0, int? tyear=0)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"Insert into ifmis.dbo.tbl_R_BMS_PPMPItem_AddExempt (OfficeID,programid, accountid, actioncode, tyear,datetimeentered) values (" + officeid + "," + programid + ","+ accountid + ",1,"+ tyear + ",format(getdate(),'MM/dd/yyyy hh:mm:ss'))", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult wfptitemexempt([DataSourceRequest]DataSourceRequest request, int? officeid = 0, int? tyear = 0)
        {
            string SQL = "";
            SQL = "exec sp_BMS_WFPItemExemption " + officeid + "," + tyear + "";
            DataTable dt = SQL.DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public string deletewfpexemption(long trnnoid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"update ifmis.dbo.tbl_R_BMS_PPMPItem_AddExempt set actioncode=4, DateTimeEntered=isnull(DateTimeEntered,'') +','+ format(getdate(),'MM/dd/yyyy hh:mm:ss') where trnnoid="+ trnnoid + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult AddItem()
        {
            return View("pv_WFPAddAccountItem");
        }
    }
}   