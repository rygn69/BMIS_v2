using iFMIS_BMS.BusinessLayer.Layers.BudgetControl;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.BudgetControl;
using iFMIS_BMS.Base;
using System.Data;
using System.Web.Script.Serialization;
using System.Configuration;
using iFMIS_BMS.BusinessLayer.Connector;
//using iFMIS_BMS.BusinessLayer.Models;
//using iFMIS_BMS.BusinessLayer.Models.BudgetControl;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.PPMP;

namespace iFMIS_BMS.Controllers
{
    [Authorize]
    public class BudgetControlController : Controller
    {
        serviceSoapClient PPMPdata = new serviceSoapClient();
        [Authorize] 
        // GET: BudgetControl
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Current()
        {
            return View("vwCurrent");
        }
        [Authorize]
        public ActionResult Excess()
        {
            return View("vwExcess");
        }
        [Authorize]
        public ActionResult OBR()
        {
            return View("vwOBRLogger");
        }
        public JsonResult CurrentControl([DataSourceRequest] DataSourceRequest request, int? Year)
        {
            BudgetControl_Layer obligated_list = new BudgetControl_Layer();
            var obligated_lst = obligated_list.grObligated_list(Year);
            return Json(obligated_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        //public ActionResult CurrentControl([DataSourceRequest]DataSourceRequest request, int? Year = 0)
        //{
        //    var User = Account.UserInfo.UserTypeDesc;
        //    var Office = Account.UserInfo.Department;
        //    var UserTypeID = Account.UserInfo.UserTypeID;

        //    string SQL = "";
        //    SQL = "dbo.sp_BMS_ControlGrid " + Year + "," + UserTypeID + ", '" + Office + "'";
        //    DataTable dt = SQL.DataSet();

        //    var serializer = new JavaScriptSerializer();
        //    var result = new ContentResult();
        //    serializer.MaxJsonLength = Int32.MaxValue;
        //    result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
        //    result.ContentType = "application/json";
        //    return result;

        //}
        public JsonResult grExcessControl([DataSourceRequest] DataSourceRequest request, int? TransactionYear)
        {
            BudgetControl_Layer excess_control = new BudgetControl_Layer();
            var excess_controlList = excess_control.grExcessControl(TransactionYear);
            return Json(excess_controlList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult grNonOfficeFunctionCode([DataSourceRequest] DataSourceRequest request, int? Year)
        {
            BudgetControl_Layer nonofficecodelist = new BudgetControl_Layer();
            var nonofficecode_lst = nonofficecodelist.grNonofficecodelist(Year);
            return Json(nonofficecode_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult CurrentObligated([DataSourceRequest] DataSourceRequest request, string OBRNo, string type, int? Year, int? param)
        {
            BudgetControl_Layer currentObligated = new BudgetControl_Layer();
            var lst = currentObligated.grCurrentObligated(OBRNo, type, Year, param);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult PPACurrentObligated([DataSourceRequest] DataSourceRequest request, string OBRNo, int? param)
        {
            BudgetControl_Layer currentObligated = new BudgetControl_Layer();
            var lst = currentObligated.grPPACurrentObligated(OBRNo, param);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult PPAAccountName([DataSourceRequest] DataSourceRequest request, string OBRNo, int? param)
        {
            BudgetControl_Layer currentObligated = new BudgetControl_Layer();
            var lst = currentObligated.grPPAAccountName(OBRNo, param);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ExcessAppropriation([DataSourceRequest] DataSourceRequest request, int YearOf = 0)
        {
            BudgetControl_Layer excessAppropriation = new BudgetControl_Layer();
            var lst = excessAppropriation.grExcessAppropriation(YearOf);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        //public JsonResult ExcessAppropriation([DataSourceRequest] DataSourceRequest request)
        //{
        //    BudgetControl_Layer excessAppropriation = new BudgetControl_Layer();
        //    var lst = excessAppropriation.grExcessAppropriation();
        //    return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult grOBRLogger([DataSourceRequest] DataSourceRequest request, int? Year)
        //{
        //    BudgetControl_Layer obrList = new BudgetControl_Layer();
        //    var obrLst = obrList.grOBR(Year);
        //    return Json(obrLst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}

        public JsonResult grOBRLogger([DataSourceRequest] DataSourceRequest request, int? Year)
        {
            BudgetControl_Layer obrList = new BudgetControl_Layer();
            var obrLst = obrList.grOBR(Year);
            return Json(obrLst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult PPAControl()
        {
            BudgetControlModel data = new BudgetControlModel();
            data.OBRValue = "0";
            return PartialView("pvPPAControl", data);
        }
        public PartialViewResult EditTransaction(string TransactionID)
        {
            BudgetControlModel data = new BudgetControlModel();
            data.OBRValue = TransactionID;
            return PartialView("pvAddNewTransaction", data);
        }
        public PartialViewResult AddNewOBR()
        {
            OBRLogger data = new OBRLogger();
            data.TransactionNo = "";
            data.trnno = 0;
            data.verify_tag = 0;
            data.employeeassign = 0;
            data.otherindividual = "";
            return PartialView("pvAddNewOBR", data);
        }
        public PartialViewResult ExcessRegistry(int? YearOf=0)
        {
            //Session["YearOf"] = YearOf;
            return PartialView("pvExcessRegistry");
        }
        public PartialViewResult ExcessEdit(int? TransactionYear)
        {
            ExcessModel data = new ExcessModel();
            data.TransactionYear = TransactionYear;
            return PartialView("pvExcessControl", data);
        }
        public PartialViewResult ExcessControl(int? TransactionYear)
        {
            ExcessModel data = new ExcessModel();
            data.TransactionYear = TransactionYear;
            return PartialView("pvExcessControl", data);
        }
        public PartialViewResult NonOfficeControl(int? TransactionYear)
        {
            BudgetControlModel data = new BudgetControlModel();
            data.TransactionYear = TransactionYear;
            return PartialView("pvNonOfficeControl", data);
        }
       
        public JsonResult SearchOBR(string ControlNo)
        {
            BudgetControl_Layer ddl = new BudgetControl_Layer();
            var lst = ddl.SearchOBR(ControlNo);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult OBRNo20FromTempOBR(string ControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.OBRNo20FromTempOBR(ControlNo);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchExcessAirMark(string ControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.SearchExcessAirMark(ControlNo);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchTempExcessAirMark(string ControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.SearchTempExcessAirMark(ControlNo);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public string SearchOBRNo(int? TrnnoID)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.SearchOBRNo(TrnnoID);
        }
        public string ChangeOBR(string TempOBRNo, int? OfficeID)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.ChangeOBR(TempOBRNo, OfficeID); 
        }
        public string NonOfficeFunctionCode(int? ProgramID, int? AccountID, string TempOBRNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.NonOfficeFunctionCode(ProgramID, AccountID, TempOBRNo);
        }
        public string SearchTrnnoID(string getControlNo,int tyear)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.SearchTrrnoOD(getControlNo, tyear);
        }
        public string SearchTrnno(string OBR)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.SearchTrnno(OBR);
        }
        public string DeletePPAItem(int? SubsidyID, int? PPAID)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.DeletePPAItem(SubsidyID, PPAID);
        }
        public string PPAReturn(BudgetControlModel formData)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.PPAReturn(formData);
        }
        public JsonResult CheckDBYear()
        {
            BudgetControl_Layer ddl = new BudgetControl_Layer();
            var lst = ddl.CheckDBYear();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchAllocatedAmount(int? OfficeID, int? ProgramID, int? AccountID, int? param, int? OOE, int? SubsidyIncome, int? Year)
        {
            BudgetControl_Layer ddl = new BudgetControl_Layer();
            var lst = ddl.SearchAllocatedAmount(OfficeID, ProgramID, AccountID, param, OOE, SubsidyIncome, Year);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChargeAllotmentAvailable(int? OfficeID, int? ProgramID, int? AccountID, int? OOE, int? SubsidyIncome, int? Year)
        {
            BudgetControl_Layer ddl = new BudgetControl_Layer();
            var lst = ddl.ChargeAllotmentAvailable(OfficeID, ProgramID, AccountID, OOE, SubsidyIncome, Year);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetAllotment(int? Year, int? RootPPA)
        {
            BudgetControl_Layer ddl = new BudgetControl_Layer();
            var lst = ddl.SetAllotment(Year, RootPPA);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetObligate(int? Year, int? RootPPA)
        {
            BudgetControl_Layer ddl = new BudgetControl_Layer();
            var lst = ddl.SetObligate(Year, RootPPA);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetPPARelease(int? Year, int? RootPPA)
        {
            BudgetControl_Layer ddl = new BudgetControl_Layer();
            var lst = ddl.SetPPARelease(Year, RootPPA);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetPPAObligation(int? Year, int? RootPPA)
        {
            BudgetControl_Layer ddl = new BudgetControl_Layer();
            var lst = ddl.SetPPAObligation(Year, RootPPA);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getPPARelease(int? Year, int? RootPPA)
        {
            BudgetControl_Layer ddl = new BudgetControl_Layer();
            var lst = ddl.getPPARelease(Year, RootPPA);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getPPAObligate(int? Year, int? RootPPA)
        {
            BudgetControl_Layer ddl = new BudgetControl_Layer();
            var lst = ddl.getPPAObligate(Year, RootPPA);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RootPPAObligate(int? Year, int? RootPPA)
        {
            BudgetControl_Layer ddl = new BudgetControl_Layer();
            var lst = ddl.RootPPAObligate(Year, RootPPA);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchObligated(int? ProgramID, int? AccountID, int? param, int? OOE, int? SubsidyIncome, int? OfficeID, int? Year)
        {
            BudgetControl_Layer ddl = new BudgetControl_Layer();
            var lst = ddl.SearchObligated(ProgramID, AccountID, param, OOE, SubsidyIncome, OfficeID, Year);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DiffObliandAllocated(int? OfficeID, int? ProgramID, int? AccountID, int? param, int? OOE, int? SubsidyIncome, int? Year)
        {
            BudgetControl_Layer ddl = new BudgetControl_Layer();
            var lst = ddl.DiffObliandAllocated(OfficeID, ProgramID, AccountID, param, OOE, SubsidyIncome, Year);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public string AddControlTransaction(string ControlNo, int? TransactionType, int? ModeOfExpense, int? Office, int? FundType, int? Program, string TEVControlNo, int? PPANonOffice, int? SusProgram, int? OOE, string ExpenseDescription, double? AllotedAmountValue, double? ObligateValue, double? DiffObliandAllocatedValue, double? ClaimValue, double? BalanceAllotmentValue, int? AccountCharged, int? ActualAccount, double? AccntChargeValue, double? AmountInputed, double BalanceAllotmentAppointmentValue, string Remarks, int? User, int? param, int? type)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.AddControlNewTransaction(ControlNo, TransactionType, ModeOfExpense, Office, FundType, Program, TEVControlNo, PPANonOffice, SusProgram, OOE, ExpenseDescription, AllotedAmountValue, ObligateValue, DiffObliandAllocatedValue, ClaimValue, BalanceAllotmentValue, AccountCharged, ActualAccount, AccntChargeValue, AmountInputed, BalanceAllotmentAppointmentValue, Remarks, User, param, type);
        }
        public JsonResult AddRawData(BudgetControlModel formData)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value =  data.AddControlRawData(formData);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public string SaveNonOfficeCode(int? MainPPA, int? TransactionYear, int? PPA, int? Program, int? Account, int? FunctionCodeNum,int? OfficeID)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.SaveNonOfficeCode(MainPPA, TransactionYear, PPA, Program, Account, FunctionCodeNum, OfficeID);
        }
        public JsonResult SaveControl(BudgetControlModel formData)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.SaveControlData(formData);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SavePPAControl(BudgetControlModel formData)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.SavePPAControl(formData);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddPPAControl(BudgetControlModel formData)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value =  data.AddPPAControl(formData);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public string UpdateControl(BudgetControlModel formData)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.UpdateControl(formData);
        }
        public string UpdatePPAControl(BudgetControlModel formData)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.UpdatePPAControl(formData);
        }
        public string ReturnControl(BudgetControlModel formData)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.ReturnControl(formData);
        }
        public string CancelControl(BudgetControlModel formData)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.CancelControl(formData);
        }
        public string DeleteControl(int? TrnnoID, string OBRNo, string ControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.DeleteControl(TrnnoID, OBRNo, ControlNo);
        }
        public string CheckIfExisit(int? ControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.CheckIfExisit(ControlNo);
        }
        public string CheckIfCanceled(int? ControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.CheckIfCanceled(ControlNo);
        }
        public double CheckCurrentControl(int? accountCharge, int? Program)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.CheckCurrentControl(accountCharge, Program);
        }
        public double CheckClaim(int? ControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.CheckClaim(ControlNo);
        }
        public string OBRUserTime()
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.OBRUserTime();
        }
        public JsonResult GenerateOBRData(int? FundID)
        {
            BudgetControl_Layer ddl = new BudgetControl_Layer();
            var lst = ddl.GenerateOBRData(FundID);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckInOBR(int? FundID, string UserInTimeStamp, int? UserID, string RefNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var lst = data.CheckInOBR(FundID, UserInTimeStamp, UserID, RefNo);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchOBRData(int? TransactionNo, int? tyear)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.SearchOBRDAta(TransactionNo, tyear);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckOutOBR(int? TransactionNo, string OBRNowithFnCode = "", string ObrNoASs="",long? approveby=0)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.CheckOutOBR(TransactionNo, OBRNowithFnCode, ObrNoASs, approveby);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckOutOBRv2(int? TransactionNo, string OBRNowithFnCode = "", string ObrNoASs = "",int? EmployeeForward=0 ,string otherindiv_id ="")
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.CheckOutOBRv2(TransactionNo, OBRNowithFnCode, ObrNoASs, EmployeeForward, otherindiv_id);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        
        public PartialViewResult ViewOBRDetails(string Transaction, int? trnno, string cttsno,int? office,int? officeassign,string program, int? verify_tag, int? employeeassign, string otherindividual,long? approveby)
        {
            OBRLogger data = new OBRLogger();
            data.TransactionNo = Transaction;
            data.trnno = trnno;
            data.cttsno = cttsno;
            data.office = office;
            data.officeassign = officeassign;
            data.program = program;
            data.verify_tag = verify_tag;
            data.employeeassign = employeeassign;
            data.otherindividual = otherindividual;
            data.approveby = approveby;
            return PartialView("pvAddNewOBR", data);
        }
        public PartialViewResult ViewFowardOffice(int? trnno, string OBRNowithFnCode = "", string ObrNoASs = "", int? EmployeeForward = 0, string otherindiv_id = "")
        {
            OBRLogger data2 = new OBRLogger();
            data2.trnno = trnno;
            data2.OBRNowithFnCode = OBRNowithFnCode;
            data2.grOBRNo = ObrNoASs;
            data2.employeeassign = EmployeeForward;
            data2.otherindividual = otherindiv_id;
            return PartialView("pv_FowardOffice", data2);
        }
        public string PPACancel(string OBRNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.PPACancel(OBRNo); 
        }
        public string SaveAppropriation(double? Amount, string AccountName, int? Year, int? FundType)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.SaveAppropriation(Amount, AccountName, Year, FundType);
        }
        public string UpdateAppropriation(int? YearOf, double? AmountAppropriation, string AccountNameAppropriation, int? FundType, int? ExcessID, string PastAccount)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.UpdateAppropriation(YearOf, AmountAppropriation, AccountNameAppropriation, FundType, ExcessID, PastAccount);
        }
        public string DeleteAppropriation(int? ExcessID)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.DeleteAppropriation(ExcessID);
        }
        public JsonResult SearchOBRDetails(string ControlNum, int? YearOf)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.SearchOBRDetails(ControlNum, YearOf);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchAirMark(string getControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.SearchAirMark(getControlNo);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditExcessControl(int? TrnnoID)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.EditExcessControl(TrnnoID);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public double ExcessSetAppropriation(int? AccountID, int? FundType)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.ExcessSetAppropriation(AccountID, FundType);
        }
        public double ExcessObligation(int? AccountID, int? FundType)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.ExcessObligation(AccountID, FundType);
        }
        public string SetProgramOBR(int? Office, string OBRNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.SetProgramOBR(Office, OBRNo);
        }
        public string SetPPAOBR(int? PPAID, string OBRNo, int? TransactionYear, int? AccountID)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.SetPPAOBR(PPAID, OBRNo, TransactionYear, AccountID);
        }
        public string SetAccountOBR(int? ProgramID, int? AccountID, string OBRNo, int? TransactionYear)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.SetAccountOBR(ProgramID, AccountID, OBRNo, TransactionYear);
        }
        public JsonResult SaveExcessControl(string OBRNo, int? FundType, int? ExcessAccount, string ExcessDescription, double? Amount, int? PPAID, int? TransactionYear, double? Appropriation, double? Obligation, double? Allotment, double? Balance, int? Office, int? Program, int? NonOfficeAccount, string OBRSeries, int? TempIndicator, string ControlNo, int? SubAccount)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.SaveExcessControl(OBRNo, FundType, ExcessAccount, ExcessDescription, Amount, PPAID, TransactionYear, Appropriation, Obligation, Allotment, Balance, Office, Program, NonOfficeAccount, OBRSeries, TempIndicator, ControlNo,SubAccount);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public string UpdateExcessControl(string OBRNo, int? FundType, int? ExcessAccount, string ExcessDescription, double? Amount, int? PPAID, int? TransactionYear, double? Appropriation, double? Obligation, double? Allotment, double? Balance, int? Office, int? Program, int? NonOfficeAccount, int? trnno, int? SubAccount)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.UpdateExcessControl(OBRNo, FundType, ExcessAccount, ExcessDescription, Amount, PPAID, TransactionYear, Appropriation, Obligation, Allotment, Balance, Office, Program, NonOfficeAccount, trnno,SubAccount,"0");
        }
        public string DeleteExcessControl(string OBRNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.DeleteExcessControl(OBRNo);
        }
        public double CheckCurrentAllotment(int? ExcessAccount, int? Fundflag)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.CheckCurrentAllotment(ExcessAccount, Fundflag);
        }
        public int SearchFunctionCode(int? MainPPA, int? TransactionYear, int? PPA, int? Program, int? Account)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.SearchFunctionCode(MainPPA, TransactionYear, PPA, Program, Account);
        }
        public string SetPrefixTempOBR(int? Year)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.SetPrefixTempOBR(Year);
        }
        
        public string SeTempcomOBR(int? Year, int? param)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.SeTempcomOBR(Year, param);
        }
        public JsonResult CheckIfAirMark(string ControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value =  data.CheckIfAirMark(ControlNo);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public string SetPrefixTemp20OBR(int? Year)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.SetPrefixTemp20OBR(Year);
        }
        public JsonResult YearMonth(string ControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.YearMonth(ControlNo);
            return Json(value, JsonRequestBehavior.AllowGet);

        }
        public string OBRNoFromTempOBR(string ControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.OBRNoFromTempOBR(ControlNo);
        }
        //public string OBRNo20FromTempOBR(string ControlNo)
        //{
        //    BudgetControl_Layer data = new BudgetControl_Layer();
        //    return data.OBRNo20FromTempOBR(ControlNo);
        //}
        
        public int VerifyOfficeRef(string getControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.VerifyOfficeRef(getControlNo);
        }
        public JsonResult PPAAirMark(string getControlNo, string PPATransOBRNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.PPAAirMark(getControlNo, PPATransOBRNo);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public int GetFundCode(int? OfficeID)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.GetFundCode(OfficeID);
        }
        public JsonResult OfficeRemainingBalance(int? Office=0, int? Account=0, int? Program=0, int? TransactionYear=0)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.OfficeRemainingBalance(Office, Account, Program, TransactionYear);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public string GenerateTransaction()
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.GenerateTransaction();
        }
        public JsonResult ComputeLDRRMF(int? PPANonOffice, int? Year)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.ComputeLDRRMF(PPANonOffice, Year);
            return Json(value, JsonRequestBehavior.AllowGet);  
        }
        public int CheckIfRef(string ControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.CheckIfRef(ControlNo);
        }
        public JsonResult CheckIFOBRExistInAirMark(string OBRNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.CheckIFOBRExistInAirMark(OBRNo);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchIfReferenceExist(string RefNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.SearchIfReferenceExist(RefNo);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateOBR(int? TransactionNo, string RefNo, string Particular,int? Year,int? officeassign, int? EmployeeForward,string otherindiv_id, long approveby)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.UpdateOBR(TransactionNo, RefNo, Particular, Year, officeassign,EmployeeForward, otherindiv_id, approveby);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        // JSON Result
        
        // New Update
        public JsonResult CurrentComputation(int? OfficeID=0, int? AccountID=0, int? ProgramID=0, int? YearOf=0, int? OOE=0, int? WithSubsidiaryFlag=0, int? param=0,string refno="")
        {
            CurrentControl_Layer data = new CurrentControl_Layer();
            var value = data.CurrentComputation(OfficeID, AccountID, ProgramID, YearOf, OOE, WithSubsidiaryFlag, param, refno);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckIfAirmarkExist(string ControlNo)
        {
            CurrentControl_Layer data = new CurrentControl_Layer();
            var value = data.CheckIfAirmarkExist(ControlNo);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckDvNoStatus(string OBRNO)
        {
            CurrentControl_Layer data = new CurrentControl_Layer();
            var value = data.CheckDvNoStatus(OBRNO);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ExcessComputation(int? FundID, int? AccountID, int? Year)
        {
            BudgetExcess_Layer data = new BudgetExcess_Layer();
            var value = data.ExcessComputation(FundID, AccountID, Year);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PPAComputation(int? Year, int? AccountID, int? RootPPA, int? PPAID,string ControlNo,int? ProgramID)
        {
            Budget20P_Layer data = new Budget20P_Layer();
            var value = data.PPAComputation(Year, AccountID, RootPPA, PPAID, ControlNo, ProgramID);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SPOComputation(int? ProgramID, int? AccountID, int? YearOF, int? IsIncome, int? SPO_ID)
        {
            CurrentControl_Layer data = new CurrentControl_Layer();
            var value = data.SPOComputation(ProgramID, AccountID, YearOF, IsIncome, SPO_ID);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult AddNewTransaction()
        {
            BudgetControlModel data = new BudgetControlModel();
            data.OBRValue = "0";
            data._Claim = 500;
            return PartialView("pvAddNewTransaction", data);
        }
        public PartialViewResult PayrollCJC(int? TransactionType)
        {
            return PartialView("pvPayrollBatchNo");
        }
        public string BatchDetails(string batchno="",int tyear=0)
        {
          
            string SQL;
            string SQL2;
            try
            {
                if (Account.UserInfo.lgu == 0)
                {
                    DataTable _dt = new DataTable();
                    string _sqlQuery = "exec usp_GetPayrollDetails_byBatchno '" + batchno + "'";
                    _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["fmisqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                    SQL = _dt.Rows[0]["sumTotal"].ToString() + ';' + _dt.Rows[0]["payee"].ToString() + ',' + _dt.Rows[0]["particular"].ToString() + ';' + _dt.Rows[0]["batchno"].ToString();
                    //return SQL;
                    try
                    {
                        DataTable _dt2 = new DataTable();
                        string _sqlQuery2 = "Select * from tblAMIS_PayrollLocation where payrollbatchno ='" + batchno + "' and [Actioncode]=1 and year(DTE)="+ tyear + "";
                        _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["fmisqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery2).Tables[0];
                        SQL2 = _dt2.Rows[0]["PayrollBatchNo"].ToString();

                        return SQL = _dt2.Rows[0]["obrno"].ToString();//"999999";
                    }
                    catch
                    {
                        return SQL;
                    }
                }
                else
                {
                    return SQL= "999999999";
                }
            }
            catch
            {
                return SQL = "";
            }
        }
        public ActionResult ViewDetails([DataSourceRequest]DataSourceRequest request, long? batchno)
        {
            string SQL = "";
            SQL = "exec usp_GetPayrollDetails_byBatchno " + batchno + "";
            DataTable dt = SQL.fmisDataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public string getGrossAmount(string obrno)
        {

            try
            {
                var data = "";
                using (SqlConnection con = new SqlConnection(Common.MyConn2()))
                {
                    SqlCommand query = new SqlCommand(@"exec sp_BMS_GrossAmount '" + obrno + "'", con);
                    con.Open();
                    data = query.ExecuteScalar().ToString();
                }
                return data;
            }
            catch
            {
                return "";
            }
        }
        public int GetExemptPayroll(long accntcode) 
        {
            string ExPayroll = "";
            try
            {

                DataTable _dt = new DataTable();
                string _sqlQuery = "SELECT accountcode FROM [IFMIS].[dbo].[tbl_R_BMS_ExemptPayroll] where accountcode=" + accntcode + " and actioncode=1";
                _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                ExPayroll = _dt.Rows[0][0].ToString();
                if (ExPayroll != "") 
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        public int NonOfficeSubAccount(int ProgramID, long AccountID, int TransYear,int status)
        {
            string ExPayroll = "";
            try
            {
                DataTable _dt = new DataTable();
                string _sqlQuery = "dbo.sp_BMS_DropdownNonOffice " + ProgramID + "," + AccountID + ", " + TransYear + ","+ status + "";
                _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                ExPayroll = _dt.Rows[0][0].ToString();
                if (ExPayroll != "0")
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        public string GetPayrollBatchNo(string obrNo, string tempObrno)
        {
            try
            {
                var data = "";
                using (SqlConnection con = new SqlConnection(Common.MyConn2()))
                {
                    SqlCommand query = new SqlCommand(@"Select [PayrollBatchNo] from tblAMIS_PayrollLocation where ([obrno] ='" + obrNo + "' or obrno='" + tempObrno + "') and [Actioncode]=1", con);
                    con.Open();
                    data = query.ExecuteScalar().ToString();
                }
                return data;
            }
            catch
            {
                return "";
            }
        }
        
        public JsonResult getFunctionEnableDisable(int id)
        {

            CurrentControl_Model data = new CurrentControl_Model();
            DataTable _dt = new DataTable();

            //using (SqlConnection con = new SqlConnection(Common.MyConn()))
            //{
            //    var qrcode = 0;
            //    SqlCommand com = new SqlCommand(@" Select count(menu_id) from [IFMIS].[dbo].[tbl_R_BMSpecialMenu] where [menu_id]=5 and [actioncode]=1", con);
            //    con.Open();
            //    qrcode = Convert.ToInt32(com.ExecuteScalar());
            //    Session["qrcode"] = qrcode;
            //}

            string _sqlQuery = "SELECT [enable],[beginyear] FROM [IFMIS].[dbo].[tbl_R_BMSEnableFunction] where [id]=" + id + "";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.enablefunction = Convert.ToBoolean(_dt.Rows[0][0]);
                data.beginyear = Convert.ToInt32(_dt.Rows[0][1]);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SubAccountbalance(int Office, long Account, int Program, int TransactionYear, int PPANonOffice, int Excess=0,string controlno="")
        {

            BudgetControlModel data = new BudgetControlModel();
            DataTable _dt = new DataTable();
            //string _sqlQuery = "exec sp_BMS_SubAccountRemainingBalance  "+ Office +", "+ Account +", "+ Program +", "+ TransactionYear +", "+ PPANonOffice +","+ Excess + "";
            string _sqlQuery = "exec sp_BMS_SubAccountRemainingBalanceEarmark  " + Office + ", " + Account + ", " + Program + ", " + TransactionYear + ", " + PPANonOffice + "," + Excess + ",'"+ controlno + "'";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.Amount = Convert.ToDecimal(_dt.Rows[0][0]);
                data.OfficeName = Convert.ToString(_dt.Rows[0][1]);
                data.message = Convert.ToString(_dt.Rows[0][2]);
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
        public int NonOfficeSubAccountExcess(int ProgramID, long AccountID, int TransYear,int ExcessAccount)
        {
            string ExPayroll = "";
            try
            {
                DataTable _dt = new DataTable();
                string _sqlQuery = "exec [sp_BMS_DropdownNonOfficeExcess] " + ProgramID + "," + AccountID + ", " + TransYear + ","+ ExcessAccount + "";
                _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                ExPayroll = _dt.Rows[0][0].ToString();
                if (ExPayroll != "0")
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        public JsonResult commitmentdetails(string comctrl="")
        {

            BudgetControlModel data = new BudgetControlModel();
            DataTable _dt = new DataTable();
            string _sqlQuery = "SELECT [officeid],[programid],[accountid],[particular],[amount] FROM [IFMIS].[dbo].[tbl_R_BMS_Release_Commitment] "+
                                "where actioncode = 1 and excess = 0 and transcode='"+ comctrl + "'";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.OfficeID = Convert.ToInt32(_dt.Rows[0][0]);
                data.ProgramID = Convert.ToInt32(_dt.Rows[0][1]);
                data.AccountID = Convert.ToInt32(_dt.Rows[0][2]);
                data.particular = Convert.ToString(_dt.Rows[0][3]);
                data.Amount = Convert.ToDecimal(_dt.Rows[0][4]);
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
        public PartialViewResult ControlForApproval(int? year_)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = 0;
                SqlCommand com = new SqlCommand(@" Select count(menu_id) from [IFMIS].[dbo].[tbl_R_BMSUserSpecialMenu] where [specialmenu]=3 and [eid]=" + Account.UserInfo.eid.ToString() + " and [actioncode]=1", con);
                con.Open();
                data = Convert.ToInt32(com.ExecuteScalar());
                Session["usercontrolapproved"] = data;
            }
            return PartialView("pv_ObligationForApproval");
        }

        public JsonResult control_for_approval([DataSourceRequest] DataSourceRequest request, int? year_ = 0)
        {

            List<BudgetControlModel> Ctrl_List = new List<BudgetControlModel>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_ControlforApproval] " + year_ + "", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    BudgetControlModel Ctrl = new BudgetControlModel();
                    Ctrl.transno = reader.GetInt64(0);
                    Ctrl.OfficeName = reader.GetValue(1).ToString();
                    Ctrl.OBRNo = reader.GetValue(2).ToString();
                    Ctrl.Description = reader.GetValue(3).ToString();
                    Ctrl.Amount = Convert.ToDecimal(reader.GetValue(4).ToString());
                    Ctrl.DateTimeEntered = reader.GetValue(5).ToString();
                    Ctrl.username = reader.GetValue(6).ToString();

                    Ctrl_List.Add(Ctrl);

                }
            }
            return Json(Ctrl_List.ToDataSourceResult(request));
        }
        public string ForApprovalCancel(long? transno=0)
        {
            try
            {
                var data = "";
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand query = new SqlCommand(@"exec sp_BMS_CancelControlForApproval " + transno + ","+ Account.UserInfo.eid.ToString() +"", con);
                    con.Open();
                    data =Convert.ToString(query.ExecuteScalar());
                }
                return data;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ApproveControl(string[] trnno_id)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Releaseid");

                var idx = 0;
                foreach (var relid in trnno_id)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = trnno_id[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("dbo.[sp_BMS_ControlApprove]", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@controlid", dt));
                    com.Parameters.Add(new SqlParameter("@userid", Account.UserInfo.eid));
                    con.Open();

                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public long SearchTransno(string cttsqr = "")
        {
            try
            {
                long data = 0;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand query = new SqlCommand(@"Select [transactionno] FROM [IFMIS].[dbo].[tbl_T_BMSCttsQR] where [cttsno]='"+ cttsqr + "' and isnull(cttsno,0) != 0", con);
                    con.Open();
                    data = Convert.ToInt64(query.ExecuteScalar());
                }
                return data;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int checktransyear(string ControlNo="",int? transyear=0)
        {
            try
            {
                int data = 0;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand query = new SqlCommand(@" exec sp_BMS_loggertransyear '"+ ControlNo + "',"+ transyear  + " ", con);
                    con.Open();
                    data = Convert.ToInt32(query.ExecuteScalar());
                }
                return data;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public PartialViewResult EditPPATransaction(string TransactionID)
        {
            BudgetControlModel data = new BudgetControlModel();
            data.OBRValue = TransactionID;
            return PartialView("pvPPAControl", data);
        }
        public int getfunctioncode(int OfficeID=0,int? ProgramID=0,long? AccountID=0,int? TransYear=0)
        {
            try
            {
                int data = 0;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand query = new SqlCommand(@"sb_BMS_FunctionCode "+ OfficeID  + ","+ ProgramID  + ","+ AccountID  + ","+ TransYear  + " ", con);
                    con.Open();
                    data = Convert.ToInt32(query.ExecuteScalar());
                }
                return data;
            }
            catch
            {
                return 0;
            }
        }
        public int transyearend(int? year=0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                int trnyear = 0;
                //SqlCommand com = new SqlCommand(@" SELECT tyearend FROM  [IFMIS].[dbo].[tbl_R_BMSTransYear] where [trnYear]="+ year + "", con);
                SqlCommand com = new SqlCommand(@"exec sp_BMS_UserRestriction "+ year + ","+ Account.UserInfo.eid +"", con);
                con.Open();
                trnyear = Convert.ToInt32(com.ExecuteScalar());
                return trnyear;
            }
        }
        public void financetracking(string OBRNo ="")
        {
            PPMPdata.UpdateStatusTracking(1,DateTime.Now,"", OBRNo);
        }
        public string Verifytrans(string obrno="")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    var verifier_true = 0;

                    SqlCommand query = new SqlCommand(@"select count([eid]) from  [IFMIS].[dbo].[tbl_R_BMS_Control_Verifier] where eid="+ Account.UserInfo.eid + "", con);
                    con.Open();
                    verifier_true = Convert.ToInt32(query.ExecuteScalar());
                    con.Close();

                    if (verifier_true != 0)
                    {
                        SqlCommand com = new SqlCommand(@"Update [IFMIS].[dbo].[tbl_R_BMSObrLogs] set verify=1,verifyby=" + Account.UserInfo.eid + ",DateTimeVerify=format(getdate(),'MM/dd/yyyy hh:mm:ss tt') where obrno='" + obrno + "'", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());

                        return "success";
                    }
                    else
                    {
                        return "Apologies! You do not have the authorization to verify this specific transaction.";
                    }
                } 
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string VerifytransDisprove(string obrno = "")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    var verifier_true = 0;

                    SqlCommand query = new SqlCommand(@"select count([eid]) from  [IFMIS].[dbo].[tbl_R_BMS_Control_Verifier] where eid=" + Account.UserInfo.eid + "", con);
                    con.Open();
                    verifier_true = Convert.ToInt32(query.ExecuteScalar());
                    con.Close();

                    if (verifier_true != 0)
                    {
                        SqlCommand com = new SqlCommand(@"Update [IFMIS].[dbo].[tbl_R_BMSObrLogs] set verify=0,verifyby=" + Account.UserInfo.eid + ",DateTimeVerify=format(getdate(),'MM/dd/yyyy hh:mm:ss tt'),[DateTimeOut]=NULL,[UserIDOut]=NULL where obrno='" + obrno + "'", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());

                        return "success";
                    }
                    else
                    {
                        return "Apologies! You do not have the authorization to verify this specific transaction.";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string setforwardoffice(int TransactionNo, int OfficeForward)
        {
            try
            {
                var data = "";
                using (SqlConnection con = new SqlConnection(Common.MyConn2()))
                {
                    SqlCommand query = new SqlCommand(@"update ifmis.dbo.tbl_R_BMSObrLogs set [Forward_office] ="+ OfficeForward + " where [trnno]="+ TransactionNo + "", con);
                    con.Open();
                    data = query.ExecuteScalar().ToString();
                }
                return data;
            }
            catch
            {
                return "";
            }
        }

        public ActionResult getEmployee([DataSourceRequest]DataSourceRequest request, int? officeid=0)
        {  
            string tempStr = "SELECT [eid],[EmpName] FROM [pmis].[dbo].[vwMergeAllEmployee_Modified] where [Department] in (SElect PMISOfficeID from ifmis.dbo.tbl_R_BMSOffices where officeid="+ officeid + ") order by [EmpName]";

            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
    }
}