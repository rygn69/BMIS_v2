using iFMIS_BMS.BusinessLayer.Layers.BudgetControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using iFMIS_BMS.Base;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.BudgetControl;
using iFMIS_BMS.BusinessLayer.Connector;

namespace iFMIS_BMS.Controllers
{
    [Authorize]
    public class ExcessController : Controller
    {
        // GET: Excess
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult EditExcessControl(int? TrnnoID)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.EditExcessControl(TrnnoID);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchOBRDetails(string ControlNum, int? YearOf)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.SearchOBRDetails(ControlNum, YearOf);
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
        public string SetPrefixTemp20OBR(int? Years)
        {

            BudgetControl_Layer data = new BudgetControl_Layer();
            //return data.SetPrefixTemp20OBR(Years);
            return data.SetPrefixTemp20OBR(Years);
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
        public JsonResult CheckIfAirMark(string ControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.CheckIfAirMark(ControlNo);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public int CheckIfRef(string ControlNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.CheckIfRef(ControlNo);
        }
        public JsonResult ExcessComputation(int? FundID, int? AccountID, int? Year)
        {
            BudgetExcess_Layer data = new BudgetExcess_Layer();
            var value = data.ExcessComputation(FundID, AccountID, Year);
            return Json(value, JsonRequestBehavior.AllowGet);
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
            var value = data.SaveExcessControl(OBRNo, FundType, ExcessAccount, ExcessDescription, Amount, PPAID, TransactionYear, Appropriation, Obligation, Allotment, Balance, Office, Program, NonOfficeAccount, OBRSeries, TempIndicator, ControlNo, SubAccount);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult EditExcessControl(int? TrnnoID)
        //{
        //    BudgetControl_Layer data = new BudgetControl_Layer();
        //    var value = data.EditExcessControl(TrnnoID);
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}
        public string UpdateExcessControl(string OBRNo, int? FundType, int? ExcessAccount, string ExcessDescription, double? Amount, int? PPAID, int? TransactionYear, double? Appropriation, double? Obligation, double? Allotment, double? Balance, int? Office, int? Program, int? NonOfficeAccount, int? trnno, int? SubAccount,string OBRNoTemp)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.UpdateExcessControl(OBRNo, FundType, ExcessAccount, ExcessDescription, Amount, PPAID, TransactionYear, Appropriation, Obligation, Allotment, Balance, Office, Program, NonOfficeAccount, trnno, SubAccount, OBRNoTemp);
        }
        public string DeleteExcessControl(string OBRNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.DeleteExcessControl(OBRNo);
        }
        public string SearchTrnno(string OBR)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.SearchTrnno(OBR);
        }
        public double CheckCurrentAllotment(int? ExcessAccount, int? Fundflag)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            return data.CheckCurrentAllotment(ExcessAccount, Fundflag);
        }
        public JsonResult CheckIFOBRExistInAirMark(string OBRNo)
        {
            BudgetControl_Layer data = new BudgetControl_Layer();
            var value = data.CheckIFOBRExistInAirMark(OBRNo);
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public int getSubAccountID(int? Program = 0, int? Account = 0, int? TransactionYear = 0)
        {
            string ExPayroll = "";
            try
            {

                DataTable _dt = new DataTable();
                string _sqlQuery = "SELECT SPO_ID, AccountName FROM tbl_R_BMSSPOAccounts_Excess WHERE AccountID = " + Account + " and YearOF = " + (TransactionYear + 1) + " and actioncode=1";
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

        public long GetExcessAccountID(int? TYear = 0)
        {
            long Excessbal = 0;
            try
            {

                DataTable _dt = new DataTable();
                string _sqlQuery = "Select [TransactionNo] from [IFMIS].[dbo].[tbl_T_BMSExcessAppropriation] where [YearOf]=" + TYear + " and [ActionCode]=1 and [Account] like '%sustainability%'";
                _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                Excessbal = Convert.ToInt64(_dt.Rows[0][0].ToString());
                return Excessbal;

            }
            catch
            {
                return 0;
            }
        }
        public double GetSubAccountBalance(int? TYear = 0, long? spoid = 0, long? tempExcessid=0,long? SubAccount=0)
        {
            double Excessbal = 0;
            try
            {

                DataTable _dt = new DataTable();
                string _sqlQuery = "exec sp_BMS_Esmf_balance " + TYear + "," + spoid + "," + tempExcessid + ","+ SubAccount + "";
                _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                Excessbal = Convert.ToDouble(_dt.Rows[0][2].ToString());
                return Excessbal;
               
            }
            catch
            {
                return 0;
            }
        }
        public JsonResult SearchComDetails(int? Office = 0,string comctrl = "",int? Year = 0)
        {

            BudgetControlModel data = new BudgetControlModel();
            DataTable _dt = new DataTable();
            string _sqlQuery = "exec sp_BMS_ExcessCommitment "+ Office + ",'"+ comctrl + "',"+ Year + "";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.OfficeID = Convert.ToInt32(_dt.Rows[0][0]);
                data.ProgramID = Convert.ToInt32(_dt.Rows[0][1]);
                data.AccountID = Convert.ToInt32(_dt.Rows[0][2]);
                data.particular = Convert.ToString(_dt.Rows[0][3]);
                data.Amount = Convert.ToDecimal(_dt.Rows[0][4]);
                data.OBRNo = Convert.ToString(_dt.Rows[0][5]);
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
        public int getCAFOAmode()
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                int qrcode = 0;
                SqlCommand com = new SqlCommand(@" Select count(menu_id) from [IFMIS].[dbo].[tbl_R_BMSpecialMenu] as b where [menu_id]=5 and [actioncode]=1 "+
                    "and menu_id in (select a.specialmenu from ifmis.dbo.tbl_R_BMSUserSpecialMenu as a where specialmenu=b.menu_id  and a.actioncode=1 and a.eid='"+ Account.UserInfo.eid.ToString() +"') ", con);
                con.Open();
                qrcode = Convert.ToInt32(com.ExecuteScalar());
                Session["qrcodeexcess"] = qrcode;
                return qrcode;
            }
        }
    }
}