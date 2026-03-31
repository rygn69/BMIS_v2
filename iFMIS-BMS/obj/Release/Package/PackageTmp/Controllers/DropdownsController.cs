using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iFMIS_BMS.BusinessLayer.Dropdowns;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Layers;
using System.Xml;
using Newtonsoft.Json;
using System.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Text;
using iFMIS_BMS.Base;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
using iFMIS_BMS.BusinessLayer.Layers.Maintenance;

namespace iFMIS_BMS.Controllers
{
    [Authorize]
    public class DropdownsController : Controller
    {
        // GET: Dropdowns
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ddlOffice()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.office_code();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOffice_AIPVerse(int? regularaipid, int? yearof)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.GetOffice_AIPVerse(regularaipid, yearof);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlOfficeControl()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.OfficeControl();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult dllgetEmployee(int? officeid=0)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.dllgetEmployee(officeid);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlOfficeControlByFund(int? FundType, int? YearOF)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.OfficeControlByFund(FundType, YearOF);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlOfficeControlByFund_Excess(int? FundType, int? YearOF)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.OfficeControlByFund_Excess(FundType, YearOF);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlProgramByFund_Excess(int? OfficeID, int? YearOf)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ProgramByFund_Excess(OfficeID, YearOf);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlAcctByFund_Excess(int? ProgramID, int? YearOf)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.AcctByFund_Excess(ProgramID, YearOf);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlAcctChargeByFund_Excess(int? FundType, int? YearOF)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.AcctChargeByFund_Excess(FundType, YearOF);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlgetProgramsByFund(int? FundType, int? OfficeID, int? YearOF)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ProgramsByFund(FundType, OfficeID, YearOF);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlCheckedByFund(int? FundType, int? ProgramID, int? YearOF)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.CheckedByFund(FundType, ProgramID, YearOF);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlRootPPA(int? Year)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.RootPPA(Year);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlPPAccounts(int? Year, int? Account)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.SubPPAAccounts(Year, Account);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlOfficesWithProposal()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ddlOfficesWithProposal();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlMainOfficesWithProposal()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ddlMainOfficesWithProposal();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ddlThirdLevelGroup()
        {
            string path = Server.MapPath("../XML/ThirdLevelGroup.xml");
            XmlDocument xd = new XmlDocument();
            xd.Load(path);
            XmlNodeList IDListNode = xd.SelectNodes("/Table/ThirdLevelGroup");

            var testCases = IDListNode
            .Cast<XmlNode>()
            .Select(x => new FundModel()
            {
                FundID = Convert.ToInt32(x.SelectSingleNode("ThirdLevelID").InnerText),
                FundName = x.SelectSingleNode("ThirdLevelDesc").InnerText
            });

            return Json(testCases, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlFundType()
        {
            string path = Server.MapPath("../XML/FundType.xml");
            XmlDocument xd = new XmlDocument();
            xd.Load(path);
            XmlNodeList IDListNode = xd.SelectNodes("/Table/Fundtype");

            var testCases = IDListNode
            .Cast<XmlNode>()
            .Select(x => new FundModel()
            {
                FundID = Convert.ToInt32(x.SelectSingleNode("FundTypeID").InnerText),
                FundName = x.SelectSingleNode("FundTypeName").InnerText
            });

            return Json(testCases, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ddltype()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.EmployeeType();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlPPA_DESC(int OfficeID, int MFO_ID)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ddlPPA_DESC(OfficeID, MFO_ID);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlAccountCost(int? OfficeID)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.OfficeAccountList(OfficeID);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlProgramAccount(int? ProgramID, int? TransactionYear)
        {
            DropdownLayers data = new DropdownLayers();
            var value = data.getProgramAccount(ProgramID, TransactionYear);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlgetPrograms(int? OfficeID, int? TransactionYear)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.getPrograms(OfficeID, TransactionYear);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlgetProgramsPastyear(int? OfficeID, int? TransactionYear)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ddlgetProgramsPastyear(OfficeID, TransactionYear);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlOOE(int? ProgramID)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ooe_list(ProgramID);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult filtered_ddlOOE(int? OfficeID, int? ProgramID, int? ProposalYear)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.filtered_ooe_list(OfficeID, ProgramID, ProposalYear);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ddlChildAccountCode(string ChildAccountCode)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ChildAccountCodelist(ChildAccountCode);
            var JsonReturnResult = Json(lst, JsonRequestBehavior.AllowGet);
            JsonReturnResult.MaxJsonLength = int.MaxValue;
            return JsonReturnResult;
        }
        public JsonResult CodeAccountName()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.CodeAccountName();
            var JsonReturnResult = Json(lst, JsonRequestBehavior.AllowGet);
            JsonReturnResult.MaxJsonLength = int.MaxValue;
            return JsonReturnResult;
        }
        public JsonResult ddl3rdleveldesc()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.thirdLvlDesc();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlFund()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.fund_code();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlCode()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.account_code();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlOrderby()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.orderby();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlProgram(string OfficeDropdown)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.filter_program_code(OfficeDropdown);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlCascadeProgram(string OfficeDropdown, int YearOf)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.filter_program_cascade(OfficeDropdown, YearOf);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlAccount(string AccountDropdown)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.account_list();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlAccountName()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.filter_account();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlSearchAccount(string OfficeDropdown, string ProgramDropdown)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.search_account(OfficeDropdown, ProgramDropdown);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlSearchFund(string AccountDropdown)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.search_fund(AccountDropdown);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlSearchCode(string AccountDropdown)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.search_code(AccountDropdown);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlProposalYear()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.proposal_year();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlOOENonOfficeLevel()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ooe_list_noneOfficeLevel();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ddlOOENonOfficeLevelnoco(int? realignid=0)
        {
            if (realignid == 1)
            {
                DropdownLayers ddl = new DropdownLayers();
                var lst = ddl.ooe_list_noneOfficeLevelnoco();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            else
            {
                DropdownLayers ddl = new DropdownLayers();
                var lst = ddl.ooe_list_noneOfficeLevel();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ExpenseClassLFC(int? OfficeID = 0, int? ProgramID = 0, int? YearOf = 0)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ExpenseClassLFC(OfficeID, ProgramID, YearOf);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProgramsLFC(int? propYear, int? office_ID)
        {
            DropdownLayers ddl = new DropdownLayers();
            Session["selectedOfficeID"] = office_ID;
            var lst = ddl.ProgramsLFC(propYear, office_ID);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ddlMagnaCartaAccount()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.MagnaCartaAccount();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlStep()
        {
            DropDownLayer ddl = new DropDownLayer();
            var lst = ddl.Step();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlAccountAndAccountCode(int YearActive)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.Accounts(YearActive);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlDivision(int OfficeID)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ddlPlantillaDivision(OfficeID);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getOfficeDivisions(int OfficeID)
        {
            DropDownLayer getOfficeDivisions = new DropDownLayer();
            var PlantillaList = getOfficeDivisions.GetOfficeDivision(OfficeID);
            return Json(PlantillaList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlPosition(string DivisionID)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ddlPlantillaPosition(DivisionID);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlNewAccountProgram(int OfficeID, int YearOf)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ddlNewAccountProgram(OfficeID, YearOf);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ddlEmploymentStatus()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ddlEmploymentStatus();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlCasualRate(int CRYear)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.CasualRate(CRYear);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public JsonResult userOffice()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.userOffice();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ddlClasses()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.classes();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ddlClassesARO()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.classesARO();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlTransactionType()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.transactionType();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlModeOfExpense(int? TransTypeID)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ModeOfExpense(TransTypeID);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlFundTypeControl()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.FundType();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlChecked(int? OfficeID, int? Program, int? TransactionYear)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.checkChecked(OfficeID, Program, TransactionYear);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlTEVControlNo(string ControlNo, int? trnnoID)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.TEVControlNo(ControlNo, trnnoID);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult changeExpenseClass(string item, int? param, int? Program, int? TransactionYear)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.changeExpense(item, param, Program, TransactionYear);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlAccounts(int? ProgramID, int? OOE,int? TYear)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.searchAccounts(ProgramID, OOE, TYear);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlAccountCharged(int? ProgramID, int? OOE, int? trnnoID, int? ModeIndicator, string OBRNo)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.searchAccountCharged(ProgramID, OOE, trnnoID, ModeIndicator, OBRNo);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Years()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.TransactionYears();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PPSASCode()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.PPSASCode();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult NonOfficeCodeYears()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.NonOfficeCodeYears();
            return Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult ddlPPSASAccounts(int? Year)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.PPSASAccounts(Year);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ExcessYears()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.TransactionExcessYears();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult YearsExcess()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.TransactionYearsExcess();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlExcessAppropriation(int? FundType, int? Years)
        {
            DropdownLayers data = new DropdownLayers();
            var value = data.ExcessAppropriations(FundType, Years);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlPPA(int? TransactionYear, int? AccountID)
        {
            DropdownLayers data = new DropdownLayers();
            var value = data.PPAAccounts(TransactionYear, AccountID);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult ddlPPA(int? TransactionYear, int? AccountID)
        //{
        //    DropdownLayers data = new DropdownLayers();
        //    var value = data.PPAAccounts(TransactionYear, AccountID);
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult ddlSections()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.ddlSections();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlPPANonOffice(int? ProgramID = 0, int? AccountID = 0, int? Year = 0, int? Excessid = 0)
        {
            DropdownLayers data = new DropdownLayers();
            var value = data.PPANonOffice(ProgramID, AccountID, Year, Excessid);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlPPANonOfficeExcess(int? ProgramID = 0, int? AccountID = 0, int? Year = 0, int? excessid = 0)
        {
            DropdownLayers data = new DropdownLayers();
            var value = data.PPANonOfficeExcess(ProgramID, AccountID, Year, excessid);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlMainPPA(int? TransactionYear)
        {
            DropdownLayers data = new DropdownLayers();
            var value = data.MainPPA(TransactionYear);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlPPAs(int? TransactionYear, int? AccountID)
        {
            DropdownLayers data = new DropdownLayers();
            var value = data.PPAs(TransactionYear, AccountID);
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MOS_Account(int? propYear, int? office_ID, int ooe_id, int? prog_id, int? vwAllAccountsid = 0)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.MOS_accounts(propYear, ooe_id, prog_id, vwAllAccountsid);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MOS_AccountNoEXP(int? propYear, int? office_ID, int? prog_id)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.MOS_AccountNoEXP(propYear, prog_id);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Account_Mos_From(int? office_ID_from, int? prog_id_from, int? ooe_id_from, int? year_, int? office_ID_to, int? prog_id_to, int? ooe_id_to, int? account_id_to)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.Account_Mos_From(office_ID_from, prog_id_from, ooe_id_from, year_, office_ID_to, prog_id_to, ooe_id_to, account_id_to);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Account_Mos_To(int? office_ID_from, int? prog_id_from, int? ooe_id_from, int? account_id_from, int? year_, int? office_ID_to, int? prog_id_to, int? ooe_id_to)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.Account_Mos_TO(office_ID_from, prog_id_from, ooe_id_from, account_id_from, year_, office_ID_to, prog_id_to, ooe_id_to);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public JsonResult dp_OOE()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.dp_OOE();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        #region Execution Utilities desu
        public JsonResult ddlOfficeControlFrom(int? trnno, int? Year, int? FundType)
        {
            DropdownLayers data = new DropdownLayers();
            var value = data.OfficeControlFrom(trnno, Year, FundType);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlOfficeControlFrom_Excess(int? trnno, int? YearOF, int? FundType)
        {
            DropdownLayers data = new DropdownLayers();
            var value = data.OfficeControlFrom_Excess(trnno, YearOF, FundType);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlAcctCharge_Excess(int? trnno, int? YearOF, int? FundType)
        {
            DropdownLayers data = new DropdownLayers();
            var value = data.AcctCharge_Excess(trnno, YearOF, FundType);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlgetProgramsFrom(int? trnno, int? Year, int? FundType)
        {
            DropdownLayers data = new DropdownLayers();
            var value = data.ProgramsFrom(trnno, Year, FundType);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlgetProgramsFrom_Excess(int? trnno, int? YearOF, int? FundType)
        {
            DropdownLayers data = new DropdownLayers();
            var value = data.ProgramsFrom_Excess(trnno, YearOF, FundType);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlgetAccountFrom_Excess(int? trnno, int? YearOF, int? FundType)
        {
            DropdownLayers data = new DropdownLayers();
            var value = data.AccountFrom_Excess(trnno, YearOF, FundType);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlObjOfExpenditureFrom(int? trnno, int? Year, int? FundType)
        {
            DropdownLayers data = new DropdownLayers();
            var value = data.ObjOfExpenditureFrom(trnno, Year, FundType);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public JsonResult MOS_AllAccount(int? propYear)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.MOS_AllAccount(propYear);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlProposalYearNew()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.proposal_yearnew();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ddlreport([DataSourceRequest]DataSourceRequest request, int? office = 0, int? yearof = 0)
        {

            string tempStr = "SELECT [aro_id],[arono]FROM  [IFMIS].[dbo].[tbl_T_BMSARO_xml] where actioncode=1 and packet=1 and office=" + office + " and year =" + yearof + " order by aro_id desc";
            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public JsonResult MOS_accountsNoMOOE(int? propYear, int? prog_id)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.MOS_accountsNoMOOE(propYear, prog_id);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MainPPAdropdown([DataSourceRequest]DataSourceRequest request, int? ppayear = 0,int? subppatag=0,int? Progid=0, long? accountid=0)
        {
            if (subppatag > 0 && accountid != 2861)
            {
                string tempStr = "SELECT [accountnameid] as ppaid,[accountname] as [description] FROM [IFMIS].[dbo].[tbl_R_BMSNonOffice] "+
                    "where  [yearof]="+ ppayear + " and accountid="+ accountid  + " and programid="+ Progid + " and nonofficeidparent=0 and [actioncode]=1 order by accountname";
                DataTable dt = tempStr.DataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
            else {
                string tempStr = "SELECT [ppaid],[description] FROM [memis].[dbo].[tblPPAs]where ppayear=" + ppayear + " and actualmotherppaid=0  and realign=0 order by sector,description";
                DataTable dt = tempStr.MemisDataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
        }
        public ActionResult SubPPAdropdown([DataSourceRequest]DataSourceRequest request, int? ppayear = 0, long? ppa_list = 0,int? Progid=0,long? accountid=0, int? subppatag=0)
        {
            if (subppatag > 0 && accountid != 2861){
                string tempStr = "Select spo_id as ppaid,AccountName as description from [fn_BMS_SubPPA] ("+ Progid + ","+ accountid + ","+ ppayear + ",0,"+ ppa_list + ") order by AccountName ";
                DataTable dt = tempStr.DataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
            else{
                string tempStr = "SELECT [ppaid],[description] FROM [memis].[dbo].[tblPPAs]where ppayear=" + ppayear + "and withchildppa=0 and [actualrootppaid]=" + ppa_list + " and realign=0 order by sector,description";
                DataTable dt = tempStr.MemisDataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
        }
        public JsonResult ddlOffice20()
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.office_code20();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlgetAccount(int? programID, int? TransactionYear)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.getAccounts(programID, TransactionYear);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getAccountsLink(int? programID, int? TransactionYear)
        {
            DropdownLayers ddl = new DropdownLayers();
            var lst = ddl.getAccountsLink(programID, TransactionYear);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult commitment([DataSourceRequest]DataSourceRequest request, int? propYear = 0, int? prog_id = 0, int? excess = 0)
        {
            if (excess == 0)
            {
                string tempStr = "SELECT a.AccountID as account_id,a.AccountName as account_name FROM IFMIS.dbo.tbl_R_BMSProgramAccounts as a inner join " +
                                    "[IFMIS].[dbo].[tbl_T_BMSBudgetProposal] as b on a.programid = b.[ProgramID] and a.[AccountID]=b.[AccountID] and accountyear=proposalyear " +
                                    "where a.AccountYear = '" + propYear + "' and a.ProgramID = '" + prog_id + "' and actioncode = 1 and proposalactioncode = 1 and[ProposalStatusCommittee] = 1 order by [ObjectOfExpendetureID], isnull([OrderNo],9999)";
                DataTable dt = tempStr.DataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
            else
            {
                string tempStr = "SELECT cast([TransactionNo] as bigint) as account_id,replace([Account],'#',' No.') as account_name FROM [IFMIS].[dbo].[tbl_T_BMSExcessAppropriation] where ActionCode=1 and yearof=" + propYear + " order by Account";
         
                DataTable dt = tempStr.DataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
        }
        public ActionResult getmunicipality([DataSourceRequest]DataSourceRequest request)
        {
            string tempStr = "SELECT right([citymunDesc],len([citymunDesc])-patindex('%-%',[citymunDesc])) as municipal,[citymunCode] FROM [COVID19].[dbo].[refcitymun] " +
                              "where provCode = 1603--citymunDesc like '%san francisco%' " +
                              "order by right([citymunDesc],len([citymunDesc])-patindex('%-%',[citymunDesc]))";
            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult getbarangay([DataSourceRequest]DataSourceRequest request,int? munid=0)
        {
            string tempStr = "SELECT [brgyCode],[brgyDesc]FROM [COVID19].[dbo].[refbrgy] where[provCode]=1603 and citymunCode="+ munid + "";
            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult gethistoryreport([DataSourceRequest]DataSourceRequest request, int? office=0, int program=0, long? accountid=0,int? year=0)
        {
            string tempStr = "";
            if (office != 0)
            {
                tempStr = "SELECT [com_id],[datetimegenerate] FROM [IFMIS].[dbo].[tbl_T_BMSCommitment_xml] where [officeid]=" + office + " and[programid]=" + program + " and[accountid]=" + accountid + " and year(cast([datetimegenerate] as date)) =" + year + " and  actioncode=1 order by cast(datetimegenerate as datetime) desc";
            }
            else
            {
                tempStr = "SELECT [com_id],[datetimegenerate] FROM [IFMIS].[dbo].[tbl_T_BMSCommitment_xml] where [accountid]=" + accountid + " and  actioncode=1 order by cast(datetimegenerate as datetime) desc";   
            }
            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
            //[officeid]=" + office +" and[programid]="+ program  + " and[accountid]="+ accountid + " and year(cast([datetimegenerate] as date)) =" + year + " and
        }
        public ActionResult linkyear([DataSourceRequest]DataSourceRequest request)
        {
            string tempStr = "SELECT top 3 [trnYear] FROM [IFMIS].[dbo].[tbl_R_BMSTransYear] order by cast([trnYear] as int) desc";
            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
    }
}