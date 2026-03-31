using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.BudgetControl;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using iFMIS_BMS.Base;
using System.Data;
using System.Web.Script.Serialization;
using System.Configuration;

using System.Web.Mvc;
using System.Web.Security;
using iFMIS_BMS.Models;

using System.IO;
using System.Threading;
using System.Text;
using System.Net;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using iFMIS_BMS.PPMP;

namespace iFMIS_BMS.BusinessLayer.Layers.BudgetControl
{
    public class BudgetControl_Layer
    {
        serviceSoapClient PPMPdata = new serviceSoapClient();
        //public IEnumerable<BudgetControlModel> grObligated_list(int? Year)
        //{
        //    List<BudgetControlModel> obligated = new List<BudgetControlModel>();
        //    using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //    {
        //        var obligated_list = "";
        //        var User = Account.UserInfo.UserTypeDesc;
        //        var Office = Account.UserInfo.Department;
        //        var UserTypeID = Account.UserInfo.UserTypeID;

        //        obligated_list = @"dbo.sp_BMS_ControlGrid " + Year + "," + UserTypeID + ", '" + Office + "'";

        //        SqlCommand obligated_query = new SqlCommand(obligated_list, con);
        //        con.Open();
        //        SqlDataReader obligated_reader = obligated_query.ExecuteReader();
        //        while(obligated_reader.Read())
        //        {
        //            BudgetControlModel data = new BudgetControlModel();
        //            data.OBRNo = Convert.ToString(obligated_reader.GetValue(0));
        //            data.OfficeID = Convert.ToInt32(obligated_reader.GetValue(1));
        //            data.Amount = Convert.ToDecimal(obligated_reader.GetValue(2));
        //            data.PTOAccountCode = Convert.ToString(obligated_reader.GetValue(3));
        //            data.OfficeName = Convert.ToString(obligated_reader.GetValue(7));
        //            data.TransactionNo = Convert.ToString(obligated_reader.GetValue(8));
        //            obligated.Add(data);
        //        }
        //    }
        //    return obligated;
        //}
        public IEnumerable<BudgetControlModel> grObligated_list(int? Year)
        {
            List<BudgetControlModel> obligated = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var obligated_list = "";
                var User = Account.UserInfo.UserTypeDesc;
                var Office = Account.UserInfo.Department;
                var UserTypeID = Account.UserInfo.UserTypeID;

                obligated_list = @"dbo.sp_BMS_ControlGrid " + Year + "," + UserTypeID + ", '" + Office + "'";

                SqlCommand obligated_query = new SqlCommand(obligated_list, con);
                con.Open();
                SqlDataReader obligated_reader = obligated_query.ExecuteReader();
                while (obligated_reader.Read())
                {
                    BudgetControlModel data = new BudgetControlModel();
                    data.OBRNo = Convert.ToString(obligated_reader.GetValue(0));
                    data.OfficeID = Convert.ToInt32(obligated_reader.GetValue(1));
                    data.Amount = Convert.ToDecimal(obligated_reader.GetValue(2));
                    data.PTOAccountCode = Convert.ToString(obligated_reader.GetValue(3));
                    data.OfficeName = Convert.ToString(obligated_reader.GetValue(7));
                    data.TransactionNo = Convert.ToString(obligated_reader.GetValue(8));
                    data.transno = Convert.ToInt64(obligated_reader.GetValue(9));
                    data.enduser = Convert.ToString(obligated_reader.GetValue(10));
                    data.obrseries = Convert.ToString(obligated_reader.GetValue(11));
                    data.DateTimeEntered = Convert.ToString(obligated_reader.GetValue(13));
                    obligated.Add(data);
                }
            }
            return obligated;
        }
        public IEnumerable<BudgetControlModel> grNonofficecodelist(int? Year)
        {
            List<BudgetControlModel> data = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_NonOfficeCode "+Year+"", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    BudgetControlModel value = new BudgetControlModel();
                    value.grNonOfficeAcctID = Convert.ToInt32(reader.GetValue(0));
                    value.grNonOfficeAcctName = Convert.ToString(reader.GetValue(1));
                    value.grNonOfficeFunctionCode = Convert.ToInt32(reader.GetValue(2));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ExcessModel> grExcessControl(int? TransactionYear=0)
        {
            List<ExcessModel> data = new List<ExcessModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"exec sp_BMS_ExcessControlList " + TransactionYear +" ", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while(reader.Read())
                {
                    ExcessModel value = new ExcessModel();
                    value.grOBRNo = Convert.ToString(reader.GetValue(0));
                    value.grDescription = Convert.ToString(reader.GetValue(1));
                    value.grAmount = Convert.ToDouble(reader.GetValue(2));
                    value.grTrnnoID = Convert.ToInt32(reader.GetValue(3));
                    value.transno = Convert.ToInt64(reader.GetValue(4));
                    value.username= Convert.ToString(reader.GetValue(5));
                    value.seriesno = Convert.ToString(reader.GetValue(4));
                    data.Add(value);
                }
            }
            return data;
        }
        public string SetProgramOBR(int? Office, string OBRNo)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT dbo.fn_BMS_OBRwithFunctionCode(" + Office + ", '" + OBRNo + "', 0, 0, 0, 1)", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        public string SetPPAOBR(int? PPAID, string OBRNo, int? TransactionYear, int? AccountID)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT dbo.fn_BMS_OBRwithFunctionCode("+PPAID+", '"+OBRNo+"', "+TransactionYear+", 0, "+AccountID+", 2)", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        public string SetAccountOBR(int? ProgramID, int? AccountID, string OBRNo, int? TransactionYear)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn())){
                SqlCommand query = new SqlCommand(@"SELECT dbo.fn_BMS_OBRwithFunctionCode(0, '"+OBRNo+"', "+TransactionYear+", "+ProgramID+", "+AccountID+", 3)", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        public ExcessModel SaveExcessControl(string OBRNo, int? FundType, int? ExcessAccount, string ExcessDescription, double? Amount, int? PPAID, int? TransactionYear, double? Appropriation, double? Obligation, double? Allotment, double? Balance, int? Office, int? Program, int? NonOfficeAccount, string OBRSeries, int? TempIndicator, string ControlNo, int? SubAccount)
        {

            ExcessModel data = new ExcessModel(); ;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                Program = Program == null ? 0 : Program;
                NonOfficeAccount = NonOfficeAccount == null ? 0 : NonOfficeAccount;
                ExcessDescription = ExcessDescription.Replace("'", "''");
                var query = "";
                //Account.UserInfo.Department.ToString()
                if (TempIndicator == 1) //no controlno
                {
                    query = @"dbo.sp_BMS_SaveTempExcessControl '" + OBRNo + "', " + FundType + ", " + ExcessAccount + ", '" + ExcessDescription + "', " + Amount + ", " + PPAID + ", '" + Account.UserInfo.eid + "', " + TransactionYear + ", " + Appropriation + ", " + Obligation + ", " + Allotment + ", " + Balance + ", " + Office + ", " + Program + ", " + NonOfficeAccount + ", '" + OBRSeries + "',"+ SubAccount + ",'"+ ControlNo + "' ";
                }
                else if (TempIndicator == 2) //20% development fund -from ppdo - pbo
                {
                    query = @"dbo.sp_BMS_SaveExcessControl_FromTemp '" + ControlNo + "', " + FundType + ", " + ExcessAccount + ", '" + ExcessDescription + "', " + Amount + ", " + PPAID + ", '" + Account.UserInfo.eid + "', " + TransactionYear + ", " + Appropriation + ", " + Obligation + ", " + Allotment + ", " + Balance + ", " + Office + ", " + Program + ", " + NonOfficeAccount + ", '" + OBRNo + "'," + SubAccount + " ";
                }
                else //pbo
                {
                    query = @"dbo.sp_BMS_SaveExcessControl '" + OBRNo + "', " + FundType + ", " + ExcessAccount + ", '" + ExcessDescription + "', " + Amount + ", " + PPAID + ", '" + Account.UserInfo.eid + "', " + TransactionYear + ", " + Appropriation + ", " + Obligation + ", " + Allotment + ", " + Balance + ", " + Office + ", " + Program + ", " + NonOfficeAccount + ", '" + OBRSeries + "'," + SubAccount + " ";
                }
                try
                {
                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        data.message = Convert.ToString(reader.GetValue(0));
                        data.OBRNo = Convert.ToString(reader.GetValue(1));
                    }
                }catch(Exception ex)
                {
                    data.message = ex.Message;
                }
                
                //data = Convert.ToString(com.ExecuteScalar());

            }
            return data;
        }
        public string UpdateExcessControl(string OBRNo, int? FundType, int? ExcessAccount, string ExcessDescription, double? Amount, int? PPAID, int? TransactionYear, double? Appropriation, double? Obligation, double? Allotment, double? Balance, int? Office, int? Program, int? NonOfficeAccount, int? trnno, int? SubAccount,string OBRNoTemp)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                Program = Program == null ? 0 : Program;
                NonOfficeAccount = NonOfficeAccount == null ? 0 : NonOfficeAccount;
                ExcessDescription = ExcessDescription.Replace("'", "''");

                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_UpdateExcessControl '" + OBRNo + "', " + FundType + ", " + ExcessAccount + ", '" + ExcessDescription + "', " + Amount + ", " + PPAID + ", " + Account.UserInfo.eid + ", " + TransactionYear + ", " + Appropriation + ", " + Obligation + ", " + Allotment + ", " + Balance + ", " + Office + ", " + Program + ", " + NonOfficeAccount + ", " + trnno + ",'" + SubAccount + "','"+ OBRNoTemp + "' ", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        public string DeleteExcessControl(string OBRNo)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                OBRNo = OBRNo == null ? "0" : OBRNo;

                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_DeleteExcessControl '"+OBRNo+"', "+Account.UserInfo.eid+"", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        public double CheckCurrentAllotment(int? Excess, int? FundFlag)
        {
            double data = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExcessControlCurrentAllotment "+Excess+", "+FundFlag+"", con);
                con.Open();
                data = Convert.ToDouble(query.ExecuteScalar());
            }
            return data;
        }
        public int SearchFunctionCode(int? MainPPA, int? TransactionYear, int? PPA, int? Program, int? Account)
        {
            int data = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                MainPPA = MainPPA == null ? 0 : MainPPA;
                PPA = PPA == null ? 0 : PPA;
                SqlCommand query = new SqlCommand(@"sp_BMS_SearchNonOfficeCode " + TransactionYear + ", " + Program + ", " + Account + ", " + MainPPA + ", " + PPA + ", 1", con);
                con.Open();
                data = Convert.ToInt32(query.ExecuteScalar());
            }
            return data;
        }
        public OBRLogger GenerateOBRData(int? FundID)
        {
            OBRLogger data = new OBRLogger();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT TOP 1 a.trnno+1, CONCAT(" + FundID + ",'-',RIGHT(YEAR(GETDATE()), 2),'-',MONTH(GETDATE()),'-',FORMAT(RIGHT(a.OBRSeries, 4)+1, '0000')), dbo.fn_BMS_DateString() as DateTimeStamp FROM tbl_R_BMSObrLogs as a ORDER BY a.trnno DESC ", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {

                    data.TransactionNo = Convert.ToString(reader.GetValue(0));
                    data.OBRNo = Convert.ToString(reader.GetValue(1));
                    data.DateTimeStamp = Convert.ToString(reader.GetValue(2));
                }
            }
            return data;
        }
        public OBRLogger CheckInOBR(int? FundID, string UserInTimeStamp, int? UserID, string RefNo)
        {
            OBRLogger data = new OBRLogger();
            using (SqlConnection con = new SqlConnection(Common.MyConn())){

                RefNo = RefNo == "" ? null : RefNo;
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_CheckINOBR "+FundID+", "+Account.UserInfo.eid+", '"+RefNo+"'", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    data.TransactionNo = Convert.ToString(reader.GetValue(0));
                    data.OBRNo = Convert.ToString(reader.GetValue(1));
                    data.UserINTimeStamp = Convert.ToString(reader.GetValue(3));
                }
            }
            return data;
        }
        
        public OBRLogger SearchOBRDAta(int? TransactionNo,int? tyear)
        {
            OBRLogger data = new OBRLogger();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (Account.UserInfo.lgu == 0) //PGAS
                {
                    TransactionNo = TransactionNo == 0 ? 0 : TransactionNo;
                    SqlCommand query = new SqlCommand(@"dbo.sp_BMS_searchOBRDetails_year " + TransactionNo + "," + tyear + "", con);
                    con.Open();
                    SqlDataReader reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        data.ReferenceNo = Convert.ToString(reader.GetValue(0));
                        data.FundID = Convert.ToInt32(reader.GetValue(1));
                        data.OBRNo = Convert.ToString(reader.GetValue(2));
                        data.Description = Convert.ToString(reader.GetValue(3));
                        data.Amount = Convert.ToDouble(reader.GetValue(4));
                        data.ClaimantEmployee = Convert.ToString(reader.GetValue(5));
                        data.DateTimeIN = Convert.ToString(reader.GetValue(6));
                        data.DateTimeOut = Convert.ToString(reader.GetValue(7));
                        data.UserIDOut = Convert.ToInt32(reader.GetValue(8));
                        data.OBRNowithFnCode = Convert.ToString(reader.GetValue(9));
                        data.id = Convert.ToInt32(reader.GetValue(10));
                        data.account = "";
                        data.datetimeverified = "";
                    }
                }
                else //OTHER LGU's
                {
                    TransactionNo = TransactionNo == 0 ? 0 : TransactionNo;
                    SqlCommand query = new SqlCommand(@"dbo.sp_BMS_searchOBRDetails " + TransactionNo + "," + tyear + "", con);
                    con.Open();
                    SqlDataReader reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        data.ReferenceNo = Convert.ToString(reader.GetValue(0));
                        data.FundID = Convert.ToInt32(reader.GetValue(1));
                        data.OBRNo = Convert.ToString(reader.GetValue(2));
                        data.Description = Convert.ToString(reader.GetValue(3));
                        data.Amount = Convert.ToDouble(reader.GetValue(4));
                        data.ClaimantEmployee = Convert.ToString(reader.GetValue(5));
                        data.DateTimeIN = Convert.ToString(reader.GetValue(6));
                        data.DateTimeOut = Convert.ToString(reader.GetValue(7));
                        data.UserIDOut = Convert.ToInt32(reader.GetValue(8));
                        data.OBRNowithFnCode = Convert.ToString(reader.GetValue(9));
                        data.id = Convert.ToInt32(reader.GetValue(10));
                        data.account = Convert.ToString(reader.GetValue(11));
                        data.datetimeverified = Convert.ToString(reader.GetValue(12));
                    }
                }
            }
            return data;
        }
        public IEnumerable<ExcessModel> grExcessAppropriation(int? YearOf)
        {
            List<ExcessModel> data = new List<ExcessModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //if (YearOf == 0)
                //{
                //    YearOf = 2017;
                //}
                SqlCommand query = new SqlCommand(@"SELECT a.ExcessID, a.Account, a.Amount, a.YearOf, a.FundFlag FROM tbl_T_BMSExcessAppropriation as a WHERE a.ActionCode = 1 and [YearOf]=" + YearOf  + "  ORDER BY a.TransactionNo DESC", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ExcessModel value = new ExcessModel();
                    value.grExcessID = Convert.ToInt16(reader.GetValue(0));
                    value.grAccount = Convert.ToString(reader.GetValue(1));
                    value.grAmount = Convert.ToDouble(reader.GetValue(2));
                    value.grYearOf = Convert.ToInt32(reader.GetValue(3));
                    value.grFundFlag = Convert.ToInt32(reader.GetValue(4));
                    data.Add(value);
                }
            }
            return data;
        }
        
        public IEnumerable<BudgetControlModel> grCurrentObligated(string OBRNo, string type, int? Year, int? param)
        {
            List<BudgetControlModel> grData = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
               

                OBRNo = OBRNo == "" ? "0" : OBRNo;
                var query = "";
                if(param ==1 || @Account.UserInfo.lgu == 1){
                    query = @"dbo.sp_BMS_ViewAllotment " + type + ", '" + OBRNo + "', '" + Year + "'";
                    //query = @"dbo.sp_BMS_ViewAllotment_cafoa " + type + ", '" + OBRNo + "', '" + Year + "',1";
                }
                else
                {
                    query = @"dbo.sp_BMS_ViewAllotment_Temp " + type + ", '" + OBRNo + "', '" + Year + "'";
                }
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while(reader.Read()){
                    BudgetControlModel value = new BudgetControlModel();
                    value.grProgramID = Convert.ToInt32(reader.GetValue(0));
                    value.grAcctCharge = Convert.ToInt32(reader.GetValue(1));
                    value.grAcctCode = Convert.ToInt32(reader.GetValue(2));
                    value.grFundCode = Convert.ToInt32(reader.GetValue(3));
                    value.grTransType = Convert.ToInt32(reader.GetValue(4));
                    value.grModeOfExpense = Convert.ToInt32(reader.GetValue(5));
                    value.grAccountName = Convert.ToString(reader.GetValue(6));
                    value.grAmountDummy = Convert.ToDouble(reader.GetValue(7));
                    value.grAmount = Convert.ToDouble(reader.GetValue(7));
                    value.grDateTimeEntered = Convert.ToString(reader.GetValue(8));
                    value.grOBRNo = Convert.ToString(reader.GetValue(9));
                    value.grOOEID = Convert.ToInt32(reader.GetValue(10));
                    value.grOffice = Convert.ToInt32(reader.GetValue(11));
                    value.grDescription = Convert.ToString(reader.GetValue(12));
                    value.grAcctChecker = Convert.ToInt32(reader.GetValue(14));
                    value.grModePayment = Convert.ToInt32(reader.GetValue(16));
                    value.grifExist = Convert.ToInt32(reader.GetValue(16));
                    value.grtrnnoID = Convert.ToInt32(reader.GetValue(17));
                    value.grTEVControlNo = Convert.ToString(reader.GetValue(18));
                    value.greID = Convert.ToInt32(reader.GetValue(19));
                    value.grNonOffice = Convert.ToInt32(reader.GetValue(20));
                    value.grRemarks = Convert.ToString(reader.GetValue(21));
                    value.grORNumber = Convert.ToInt32(reader.GetValue(22));
                    value.grClaimant = Convert.ToString(reader.GetValue(23));
                    value.grSubsidyIncome = Convert.ToInt32(reader.GetValue(24));
                    value.grRefNo = Convert.ToString(reader.GetValue(25));
                    value.RefAmount = Convert.ToDouble(reader.GetValue(26));
                    grData.Add(value); 
                }
            }
            return grData;
        }
        public IEnumerable<OBRLogger> grOBR(int? Year)
        {
            List<OBRLogger> data = new List<OBRLogger>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                //var query = @"exec sp_BMS_OBRList";
                var query = @"exec sp_BMS_OBRList_v2  " + Year + "";
                // var query = @"SELECT a.TransactionNo, ISNULL(a.OBRNo, '') as OBRNO, ISNULL(a.OBRSeries, '') as OBRSeries, ISNULL(UserIDIn, 0) as UserIDIn, 
                //         ISNULL(UserIDOut, 0) as UserIDOut, ISNULL(a.OBRNo, '0'), trnno,isnull(b.[cttsno],'') cttsno,isnull(c.OfficeID,0) OfficeID
                //         FROM ifmis.dbo.tbl_R_BMSObrLogs as a left join
                //         [IFMIS].[dbo].[tbl_T_BMSCttsQR] as b on b.[transactionno]=a.[TransactionNo]  left join
                //tbl_T_BMSCurrentControl as c on c.obrno=a.obrno and c.ActionCode=1
                //         WHERE RIGHT(LEFT(OBRSeries, 6), 2) = RIGHT(" + Year + ", 2)" +
                //         " ORDER BY a.trnno DESC";

                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
               
                while (reader.Read())
                {
                    OBRLogger value = new OBRLogger();
                    value.grtrnno = Convert.ToString(reader.GetValue(0));
                    value.grOBRNo = Convert.ToString(reader.GetValue(1));
                    value.grOBRSeries = Convert.ToString(reader.GetValue(2));
                    value.grUserIDIn = Convert.ToInt32(reader.GetValue(3));
                    value.grUserIDOut = Convert.ToString(reader.GetValue(4));
                    value.OBRNowithFnCode = Convert.ToString(reader.GetValue(5));
                    value.grOBRID = Convert.ToInt32(reader.GetValue(6));
                    value.cttsno =Convert.ToString(reader.GetValue(7));
                    value.office = Convert.ToInt32(reader.GetValue(8));
                    value.claimant = Convert.ToString(reader.GetValue(9));
                    value.TAmount = Convert.ToString(reader.GetValue(10));
                    value.verify_tag = Convert.ToInt32(reader.GetValue(11));
                    value.officeassign = Convert.ToInt32(reader.GetValue(12));
                    value.program = Convert.ToString(reader.GetValue(13));
                    value.employeeassign = Convert.ToInt32(reader.GetValue(14));
                    value.otherindividual = Convert.ToString(reader.GetValue(15));
                    value.approveby = Convert.ToInt64(reader.GetValue(16));
                    data.Add(value);                   
                }
            }
            return data;
        }
        public IEnumerable<BudgetControlModel> grPPACurrentObligated(string OBRNo, int? param)
        {
            List<BudgetControlModel> data = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var query = "";
                if (param == 1)
                {
                    query = @"dbo.sp_BMS_ViewPPAControl '" + OBRNo + "', 1";
                }
                else if (param == 2)
                {
                    query = @"dbo.sp_BMS_ViewTempPPAControl '" + OBRNo + "', 1";
                }
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    BudgetControlModel value = new BudgetControlModel();
                    value.grRootPPA = Convert.ToInt32(reader.GetValue(0));
                    value.grPPAAccountName = Convert.ToString(reader.GetValue(1));
                    value.grPPA = Convert.ToInt32(reader.GetValue(2));
                    value.grPPAClaim = Convert.ToDouble(reader.GetValue(3));
                    value.grPPAModeOfExpense = Convert.ToInt32(reader.GetValue(4));
                    value.grPPATransactionType = Convert.ToInt32(reader.GetValue(5));
                    value.grPPAOOE = Convert.ToInt32(reader.GetValue(6));
                    value.grPPADescription = Convert.ToString(reader.GetValue(7));
                    value.grPPAID = Convert.ToInt32(reader.GetValue(8));
                    value.grPPATotalClaim = Convert.ToDouble(reader.GetValue(9));
                    value.grPPAAcctChecker = Convert.ToInt32(reader.GetValue(10));
                    value.grSubsidyID = Convert.ToInt32(reader.GetValue(11));
                    value.grPPAID = Convert.ToInt32(reader.GetValue(12));
                    value.grRemarks = Convert.ToString(reader.GetValue(13));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<BudgetControlModel> grPPAAccountName(string OBRNo, int? param)
        {
            List<BudgetControlModel> data = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var query = "";
                if (param == 1)
                {
                    query = @"dbo.sp_BMS_ViewPPAControl '" + OBRNo + "', 2";
                }else if(param == 2){
                    query = @"dbo.sp_BMS_ViewTempPPAControl '" + OBRNo + "', 2";
                }
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    BudgetControlModel value = new BudgetControlModel();
                    value.grPPAAccountName = Convert.ToString(reader.GetValue(0));
                    value.grPPAClaim = Convert.ToDouble(reader.GetValue(1));
                    value.grPPAID = Convert.ToInt32(reader.GetValue(2));
                    data.Add(value);
                }
            }
            return data;
        }
        
        public ExcessModel SearchExcessAirMark(string ControlNo)
        {
            ExcessModel data = new ExcessModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_SearchExcessAirMark '"+ControlNo+"'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    data.ExcessAppropriationID = Convert.ToInt32(reader.GetValue(0));
                    data.Amount = Convert.ToDouble(reader.GetValue(1));
                    data.ppaid = Convert.ToInt32(reader.GetValue(2));
                    data.Office = Convert.ToInt32(reader.GetValue(3));
                    data.ProgramID = Convert.ToInt32(reader.GetValue(4));
                    data.AccountID = Convert.ToInt32(reader.GetValue(5));
                    data.FundFlag = Convert.ToInt32(reader.GetValue(6));
                    data.OBRNo = Convert.ToString(reader.GetValue(7));
                    data.OBRSeries = Convert.ToString(reader.GetValue(8));
                    data.Description = Convert.ToString(reader.GetValue(9));
                }
            }
            return data;
        }
        public ExcessModel SearchTempExcessAirMark(string ControlNo)
        {
            ExcessModel data = new ExcessModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_SearchExcessAirMark_FromTemp '" + ControlNo + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    data.ExcessAppropriationID = Convert.ToInt32(reader.GetValue(0));
                    data.Amount = Convert.ToDouble(reader.GetValue(1));
                    data.ppaid = Convert.ToInt32(reader.GetValue(2));
                    data.Office = Convert.ToInt32(reader.GetValue(3));
                    data.ProgramID = Convert.ToInt32(reader.GetValue(4));
                    data.AccountID = Convert.ToInt32(reader.GetValue(5));
                    data.OBRNo = Convert.ToString(reader.GetValue(6));
                    data.Description = Convert.ToString(reader.GetValue(7));
                }
            }
            return data;
        }
        public string SearchTrrnoOD(string getControlNo,int tyear)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_SearchOBRControl_v2 '" + getControlNo + "', 2,"+ tyear + "", con);
                con.Open();
                data = query.ExecuteScalar().ToString();
            }
            return data;
        }
        public string SearchTrnno(string OBR)
        {
            var data = "";
            using(SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT ISNULL((SELECT trnno_id FROM tbl_T_BMSExcessControl WHERE OBRNo = '" + OBR + "'  and ActionCode = 1), 0)", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        public string DeletePPAItem(int? SubsidyID, int? PPAID)
        {
            
            using(SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_DeletePPAControl "+SubsidyID+", "+PPAID+", "+Account.UserInfo.eid+"", con);
                con.Open();
                try
                {

                    query.ExecuteNonQuery();
                    return "success";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            
        }
        public string ChangeOBR(string TempOBRNo, int? OfficeID)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                
                SqlCommand query = new SqlCommand(@"
                            declare @FunctionCode varchar(225)
                            declare @OBR varchar(255)
                            SELECT @FunctionCode = FunctionID FROM tbl_R_BMSOffices WHERE OfficeID = '"+OfficeID+"' " +
                            " SELECT @OBR = LEFT('" + TempOBRNo + "',4)+@FunctionCode+RIGHT('" + TempOBRNo + "',11)" + 
                            " SELECT @OBR ", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        public string NonOfficeFunctionCode(int? ProgramID, int? AccountID, string TempOBRNo)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"
                            declare @FunctionCode varchar(255)
                            declare @OBR varchar(255)
                            SELECT @FunctionCode = FunctionCode FROM tbl_R_BMSNonOfficeCode WHERE ProgramID = '"+ProgramID+"' and AccountID = '"+AccountID+"' " +
                            " SELECT @OBR = LEFT('" + TempOBRNo + "',4)+@FunctionCode+RIGHT('" + TempOBRNo + "',11)" +
                            " SELECT @OBR ", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        public string CheckDBYear()
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT RIGHT(YEAR(GETDATE()),2)", con);
                con.Open();
                data = Convert.ToString(com.ExecuteScalar());
            }
            return data;
        }
        public double SearchAllocatedAmount(int? OfficeID, int? ProgramID, int? AccountID, int? param, int? OOE, int? SubsidyIncome, int? Year)
        {
            double AmountRelease = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                AccountID = AccountID == null ? 0 : AccountID;
                OOE = OOE == null ? 0 : OOE;
                var IsIncome = 0;
                if (OfficeID == 38 || OfficeID == 37 || OfficeID == 41)
                {
                    IsIncome = SubsidyIncome == 1 ? 0 : 1;
                }

                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_AllotedAmount " + OfficeID + "," + AccountID + "," + ProgramID + "," + Year + "," + param + ",1, " + OOE + ", " + SubsidyIncome + ", " + IsIncome + "", con);
                con.Open();
                AmountRelease = Convert.ToDouble(com.ExecuteScalar());                
            }
            return AmountRelease;
        }
        public double SearchObligated(int? ProgramID, int? AccountID, int? param, int? OOE, int? SubsidyIncome, int? OfficeID, int? Year)
        {
            double ObligateAmount = 0;

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                AccountID = AccountID == null ? 0 : AccountID;
                OOE = OOE == null ? 0 : OOE;
                var IsIncome = 0;
                if (OfficeID == 38 || OfficeID == 37 || OfficeID == 41)
                {
                    IsIncome = SubsidyIncome == 1 ? 0 : 1;
                }
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_AllotedAmount 0," + AccountID + "," + ProgramID + "," + Year + "," + param + ",2, " + OOE + ", " + SubsidyIncome + ", " + IsIncome + "", con);
                con.Open();
                ObligateAmount = Convert.ToDouble(com.ExecuteScalar());
            }
            return ObligateAmount;
        }
        public double DiffObliandAllocated(int? OfficeID, int? ProgramID, int? AccountID, int? param, int? OOE, int? SubsidyIncome, int? Year)
        {
            double TotalDiff = 0;

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                AccountID = AccountID == null ? 0 : AccountID;
                OOE = OOE == null ? 0 : OOE;
                var IsIncome = 0;
                if (OfficeID == 38 || OfficeID == 37 || OfficeID == 41)
                {
                    IsIncome = SubsidyIncome == 1 ? 0 : 1;
                }

                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_AllotedAmount " + OfficeID + "," + AccountID + "," + ProgramID + "," + Year + "," + param + ",3, " + OOE + ", " + SubsidyIncome + ", " + IsIncome + "", con);
                con.Open();
                TotalDiff = Convert.ToDouble(com.ExecuteScalar().ToString());
            }
            return TotalDiff;
        }

        public double ChargeAllotmentAvailable(int? OfficeID, int? ProgramID, int? AccountID, int? OOE, int? SubsidyIncome, int? Year)
        {
            double AmountRelease = 0;
            //change on 5/30/2018 - xXx
            //string AmountRelease = "";
            //using (SqlConnection con = new SqlConnection(Common.MyConn()))
            //{

            //    AccountID = AccountID == null ? 0 : AccountID;
            //    OOE = OOE == null ? 0 : OOE;
            //    var IsIncome = 0;
            //    if (OfficeID == 38 || OfficeID == 37 || OfficeID == 41)
            //    {
            //        IsIncome = SubsidyIncome == 1 ? 0 : 1;
            //    }
            //    SqlCommand com = new SqlCommand(@"dbo.sp_BMS_AllotedAmount " + OfficeID + "," + AccountID + "," + ProgramID + "," + Year + ",3,1, " + OOE + ", " + SubsidyIncome + ", " + IsIncome + "", con);
            //    con.Open();
            //    com.CommandTimeout = 0;
            //    AmountRelease = Convert.ToDouble(com.ExecuteScalar());

            //}
            AccountID = AccountID == null ? 0 : AccountID;
            OOE = OOE == null ? 0 : OOE;
            var IsIncome = 0;
            if (OfficeID == 38 || OfficeID == 37 || OfficeID == 41)
            {
                IsIncome = SubsidyIncome == 1 ? 0 : 1;
            }
            DataTable _dt = new DataTable();
            string _sqlQuery = "dbo.sp_BMS_AllotedAmount " + OfficeID + "," + AccountID + "," + ProgramID + "," + Year + ",3,1, " + OOE + ", " + SubsidyIncome + ", " + IsIncome + "";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            AmountRelease = Convert.ToDouble(_dt.Rows[0][0].ToString());
            return AmountRelease;
        }
        public double SetAllotment(int? Year, int? RootPPA)
        {
            double AmountRelease = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_PPAComputation " + Year + ", " + RootPPA + ", 1", con);
                //SqlCommand com = new SqlCommand(@"dbo.sp_BMS_AllotedAmount 43, 2861, 55," + Year + ",2,1, 2, 0,0", con);
                con.Open();
                AmountRelease = Convert.ToDouble(com.ExecuteScalar());
            }
            return AmountRelease;
        }
        public double SetObligate(int? Year, int? RootPPA)
        {
            double AmountObligate = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_PPAComputation " + Year + ", " + RootPPA + ", 2", con);
                //SqlCommand com = new SqlCommand(@"dbo.sp_BMS_AllotedAmount 43, 2861, 55," + Year + ",2,2, 2, 0,0", con);
                con.Open();
                AmountObligate = Convert.ToDouble(com.ExecuteScalar());
            }
            return AmountObligate;
        }
        public double SetPPARelease(int? Year, int? RootPPA)
        {
            double AmountRelease = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_PPAComputation " + Year + ", " + RootPPA + ", 4", con);
                //SqlCommand com = new SqlCommand(@"dbo.sp_BMS_AllotedAmount 43, 2861, 55," + Year + ",2,2, 2, 0,0", con);
                con.Open();
                AmountRelease = Convert.ToDouble(com.ExecuteScalar());
            }
            return AmountRelease;
        }
        public double SetPPAObligation(int? Year, int? RootPPA)
        {
            double AmountObligate = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_PPAComputation " + Year + ", " + RootPPA + ", 3", con);
                //SqlCommand com = new SqlCommand(@"dbo.sp_BMS_AllotedAmount 43, 2861, 55," + Year + ",2,2, 2, 0,0", con);
                con.Open();
                AmountObligate = Convert.ToDouble(com.ExecuteScalar());
            }
            return AmountObligate;
        }
        public double getPPARelease(int? Year, int? RootPPA)
        {
            double AmountObligate = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_PPAComputation " + Year + ", " + RootPPA + ", 5", con);
                //SqlCommand com = new SqlCommand(@"dbo.sp_BMS_AllotedAmount 43, 2861, 55," + Year + ",2,2, 2, 0,0", con);
                con.Open();
                AmountObligate = Convert.ToDouble(com.ExecuteScalar());
            }
            return AmountObligate;
        }
        public double getPPAObligate(int? Year, int? RootPPA)
        {
            double AmountObligate = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_PPAComputation " + Year + ", " + RootPPA + ", 6", con);
                //SqlCommand com = new SqlCommand(@"dbo.sp_BMS_AllotedAmount 43, 2861, 55," + Year + ",2,2, 2, 0,0", con);
                con.Open();
                AmountObligate = Convert.ToDouble(com.ExecuteScalar());
            }
            return AmountObligate;
        }
        public double RootPPAObligate(int? Year, int? RootPPA)
        {
            double RootObligate = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_PPAComputation " + Year + ", " + RootPPA + ", 3", con);
                //SqlCommand com = new SqlCommand(@"dbo.sp_BMS_RootPPAObligate " + Year + ", " + RootPPA + "", con);
                con.Open();
                RootObligate = Convert.ToDouble(com.ExecuteScalar());
            }
            return RootObligate;
        }         
        public string AddControlNewTransaction(string ControlNo, int? TransactionType, int? ModeOfExpense, int? Office, int? FundType, int? Program, string TEVControlNo, int? PPANonOffice, int? SusProgram, int? OOE, string ExpenseDescription, double? AllotedAmountValue, double? ObligateValue, double? DiffObliandAllocatedValue, double? ClaimValue, double? BalanceAllotmentValue, int? AccountCharged, int? ActualAccount, double? AccntChargeValue, double? AmountInputed, double BalanceAllotmentAppointmentValue, string Remarks, int? User, int? param, int? type)
        {
            return null;
        }
        public BudgetControlModel AddControlRawData(BudgetControlModel formData)
        {
            
            BudgetControlModel data = new BudgetControlModel();
            var query = "";
            //var ExpenseDescription = formData.ExpenseDescription.Replace("'", "''");
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var TEVControlNoText = formData.TEVControlNoText == null ? "0" : formData.TEVControlNoText;
               // var ExpenseDescription =  formData.ExpenseDescription.Replace("'", "''") == null? "" :formData.ExpenseDescription.Replace("'", "''");
                query = @"dbo.sp_BMS_ControlAddRawData '" + formData.OBRNo + "', " + formData.TransactionType + ", " + formData.ModeOfExpense + ", " + formData.Office + ", " + formData.FundTypeValue + ", " + formData.Program + ", " +
                    "" + formData.ModePayment + ", " + formData.ObjOfExpenditure + ",'" + TEVControlNoText + "'," + formData.TEVControlNo + ", " + formData.PPANonOffice + ", " + formData.SusProgram + ", " + formData.OOEText + ", '" + formData.ExpenseDescription.Replace("'", "''") + "'," +
                    "" + formData.SubsidyIncome + ", " + formData.AllotedAmountValue + "," + formData.ObligateValue + ", " + formData.DiffObliandAllocatedValue + ", " + formData.AmountInputed + "," + formData.BalanceAllotmentValue + ", " + formData.AccountCharged + ", " +
                    "" + formData.ActualAmountValue + "," + formData.AccntChargeValue + ", " + formData.BalanceAllotmentAppointmentValue + ", " + formData.UserID + ", " + formData.OOE + ", '" + formData.UserTransactionID + "', '" + formData.OBRNoCenter + "'";


                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                
                try
                {
                    //SqlDataReader reader = com.ExecuteReader();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {

                        data.OBRNo = Convert.ToString(reader.GetValue(0));
                        data.trnno_id = Convert.ToInt32(reader.GetValue(1));
                        data.DateTimeEntered = Convert.ToString(reader.GetValue(2));
                        data.Amount = Convert.ToDecimal(reader.GetValue(3));
                    }
                    //var obr = Convert.ToString(com.ExecuteScalar());
                    
                    //com.ExecuteNonQuery();
                    return data;
                }
                catch (Exception ex)
                {
                    data.OBRNo = ex.Message;
                    data.trnno_id = 0;
                    data.DateTimeEntered = "";
                    data.Amount = 0;
                    return data;
                }
            }
        }
        public BudgetControlModel SaveControlData(BudgetControlModel formData)
        {
            BudgetControlModel data = new BudgetControlModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                formData.ExpenseDescription = formData.ExpenseDescription == null ? "" : formData.ExpenseDescription;
                var ExpenseDescription = formData.ExpenseDescription.Replace("'", "''");
                string query = "";

                string tmpID2 = "";
                if (formData.batchno2 == 0 && formData.lguid == 0)
                {
                    try
                    {
                        DataTable _dt = new DataTable();
                        string _sqlQuery = "Select [PayrollBatchNo] from tblAMIS_PayrollLocation where ([obrno]='" + formData.OBRNoCenter + "' or obrno='" + formData.OBRNo + "') and [Actioncode]=1";
                        _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["fmisqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                        tmpID2 = _dt.Rows[0][0].ToString();
                    }
                    catch { tmpID2 = "0"; }
                }
                else 
                {
                    tmpID2 = formData.batchno2.ToString();
                }
                formData.OBRNoCenter = formData.OBRNoCenter == null ? "" : formData.OBRNoCenter;
                if (Account.UserInfo.lgu == 0)//PGAS
                {
                    if (formData._RefIdentifer == 1 || formData.OBRNoCenter.Length == 19)//pgo to budget
                    {
                        query = @"dbo.sp_BMS_CurrentControlFrom_Temp '" + formData.OBRNo + "', " + formData.TransactionType + ", " + formData.ModeOfExpense + ", " + formData.Office + ", " + formData.FundTypeValue + ", " + formData.Program + ", " +
                        "" + formData.ModePayment + ", " + formData.ObjOfExpenditure + ",'" + formData.TEVControlNoText + "'," + formData.TEVControlNo + ", " + formData.PPANonOffice + ", " + formData.SusProgram + ", '" + formData.OOEText + "', '" + ExpenseDescription + "'," +
                        "" + formData.SubsidyIncome + ", " + formData.AllotedAmountValue + "," + formData.ObligateValue + ", " + formData.DiffObliandAllocatedValue + ", " + formData.AmountInputed + "," + formData.BalanceAllotmentValue + ", " + formData.AccountCharged + ", " +
                        "" + formData.ActualAmountValue + "," + formData.AccntChargeValue + ", " + formData.BalanceAllotmentAppointmentValue + ", " + formData.UserID + ", '" + formData.OBRNoCenter + "','" + tmpID2 + "'";
                    }
                    else

                    if ((formData.OBRNo == "0") || (formData.OBRNo.Length == 16)) //pgo only
                    {
                        query = @"dbo.sp_BMS_TempControl '" + formData.OBRNoEntry + "', " + formData.TransactionType + ", " + formData.ModeOfExpense + ", " + formData.OfficeID + ", " + formData.FundTypeValue + ", " + formData.Program + ", " +
                        "" + formData.ModePayment + ", " + formData.ObjOfExpenditure + ",'" + formData.TEVControlNoText + "'," + formData.TEVControlNo + ", " + formData.PPANonOffice + ", " + formData.SusProgram + ", '" + formData.OOEText + "', '" + ExpenseDescription + "'," +
                        "" + formData.SubsidyIncome + ", " + formData.AllotedAmountValue + "," + formData.ObligateValue + ", " + formData.DiffObliandAllocatedValue + ", " + formData.AmountInputed + "," + formData.BalanceAllotmentValue + ", " + formData.AccountCharged + ", " +
                        "" + formData.ActualAmountValue + "," + formData.AccntChargeValue + ", " + formData.BalanceAllotmentAppointmentValue + ", " + formData.UserID + ", '" + formData.OBRNoCenter + "', '" + formData.UserTransactionID + "','" + tmpID2 + "'";

                    }
                    else if (formData.OBRNo != "0")
                    { //budget only
                        query = @"dbo.sp_BMS_CurrentControl " + formData.OBRNo + ", " + formData.TransactionType + ", " + formData.ModeOfExpense + ", " + formData.Office + ", " + formData.FundTypeValue + ", " + formData.Program + ", " +
                        "" + formData.ModePayment + ", " + formData.ObjOfExpenditure + ",'" + formData.TEVControlNoText + "'," + formData.TEVControlNo + ", " + formData.PPANonOffice + ", " + formData.SusProgram + ", '" + formData.OOEText + "', '" + ExpenseDescription + "'," +
                        "" + formData.SubsidyIncome + ", " + formData.AllotedAmountValue + "," + formData.ObligateValue + ", " + formData.DiffObliandAllocatedValue + ", " + formData.AmountInputed + "," + formData.BalanceAllotmentValue + ", " + formData.AccountCharged + ", " +
                        "" + formData.ActualAmountValue + "," + formData.AccntChargeValue + ", " + formData.BalanceAllotmentAppointmentValue + ", " + formData.UserID + ", '" + formData.OBRNoCenter + "','" + tmpID2 + "'";
                    }

                }
                else //OTHER LGU's
                {
                    query = @"dbo.sp_BMS_CurrentControl " + formData.OBRNo + ", " + formData.TransactionType + ", " + formData.ModeOfExpense + ", " + formData.Office + ", " + formData.FundTypeValue + ", " + formData.Program + ", " +
                        "" + formData.ModePayment + ", " + formData.ObjOfExpenditure + ",'" + formData.TEVControlNoText + "'," + formData.TEVControlNo + ", " + formData.PPANonOffice + ", " + formData.SusProgram + ", '" + formData.OOEText + "', '" + ExpenseDescription + "'," +
                        "" + formData.SubsidyIncome + ", " + formData.AllotedAmountValue + "," + formData.ObligateValue + ", " + formData.DiffObliandAllocatedValue + ", " + formData.AmountInputed + "," + formData.BalanceAllotmentValue + ", " + formData.AccountCharged + ", " +
                        "" + formData.ActualAmountValue + "," + formData.AccntChargeValue + ", " + formData.BalanceAllotmentAppointmentValue + ", " + formData.UserID + ", '" + formData.OBRNoCenter + "','" + tmpID2 + "'";
                }
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                try
                {
                    //com.ExecuteNonQuery();
                    data.OBRNo = Convert.ToString(com.ExecuteScalar());
                    if (data.OBRNo == "Error")
                    { data.message = "Error"; }
                    else
                    {
                        data.message = "success";
                        //try
                        //{
                        //    data = PPMPdata.UpdateStatusTracking(1,DateTime.Now, data.OBRNo);
                            //return dt;
                        //}
                        //catch (Exception ex)
                        //{
                        //    return ex.Message;
                        //}
                    }
                }catch (Exception ex)
                {
                    data.OBRNo = "";
                    data.message = ex.Message;
                }
            }
            return data;
        }
        public string SaveNonOfficeCode(int? MainPPA, int? TransactionYear, int? PPA, int? Program, int? Account, int? FunctionCodeNum,int? OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                MainPPA = MainPPA == null ? 0 : MainPPA;
                PPA = PPA == null ? 0 : PPA;
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_SaveNonOfficeCode " + TransactionYear + ", " + Program + ", " + Account + ", " + MainPPA + ", " + PPA + ", " + FunctionCodeNum + ", 1,"+ OfficeID + "", con);
                con.Open();
                try
                {
                    query.ExecuteNonQuery();
                    return "success";
                }catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public BudgetControlModel SavePPAControl(BudgetControlModel formData)
        {
            BudgetControlModel data = new BudgetControlModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var query = "";
                var PPADescription = formData.PPADescription.Replace("'", "''");
                if (formData._RefIdentifier == 1) //pgo - pbo
                {
                    query = @"dbo.sp_BMS_SavePPAControl_FromTemp " + formData.RootPPA + ", " + formData.PPA + ", " + formData.PPAOOE + ", " + formData._AllotedAmount + ", " + formData._Obligate + ", " +
                         "" + formData._PPARelease + ", " + formData._PPAObligate + ", " + formData._PPAvailable + ", " + formData.AmountInputed + ", " + formData._Claim + ", '" + formData.PPATransOBRNo + "', " +
                         "'" + formData.TransactionNo + "', " + Account.UserInfo.eid + ", '" + PPADescription + "', " + formData.PPATransactionType + ", " + formData.PPAModeofExpense + ", " + formData.PPAFundType + ", 1";
 
                }else 
                if(formData.PPAOBRNo != "0" || Account.UserInfo.UserTypeID == 2 || Account.UserInfo.UserTypeID == 4 || Account.UserInfo.UserTypeID == 5)
                { // pbo only
                    query = @"dbo.sp_BMS_SavePPAControl " + formData.RootPPA + ", " + formData.PPA + ", " + formData.PPAOOE + ", " + formData._AllotedAmount + ", " + formData._Obligate + ", " +
                         "" + formData._PPARelease + ", " + formData._PPAObligate + ", " + formData._PPAvailable + ", " + formData.AmountInputed + ", " + formData._Claim + ", '" + formData.PPATransOBRNo + "', " +
                         "" + formData.PPAOBRNo + ", " + Account.UserInfo.eid + ", '" + PPADescription + "', " + formData.PPATransactionType + ", " + formData.PPAModeofExpense + ", " + formData.PPAFundType + ", 1, " + formData.TransactionYear + ","+ formData.PPAOffice + ","+ formData.PPAProgram + "";
                }
                else if (formData.PPAOBRNo == "0") //pgo only
                {
                     query = @"dbo.sp_BMS_SaveTempPPAControl " + formData.RootPPA + ", " + formData.PPA + ", " + formData.PPAOOE + ", " + formData._AllotedAmount + ", " + formData._Obligate + ", " +
                         "" + formData._PPARelease + ", " + formData._PPAObligate + ", " + formData._PPAvailable + ", " + formData.AmountInputed + ", " + formData._Claim + ", '" + formData.PPATransOBRNo + "', " +
                         "'" + formData.TransactionNo + "', " + Account.UserInfo.eid + ", '" + PPADescription + "', " + formData.PPATransactionType + ", " + formData.PPAModeofExpense + ", " + formData.PPAFundType + ", 1, '"+formData.UserTransactionID+"'";
                }


                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                try
                {
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        data.OBRNo = Convert.ToString(reader.GetValue(0));
                        data.ReturnStatus = Convert.ToInt32(reader.GetValue(1));
                        data.MTitle = Convert.ToString(reader.GetValue(2));
                        data.MBody = Convert.ToString(reader.GetValue(3));
                        data.MType = Convert.ToString(reader.GetValue(4));
                    }
                    //data.OBRNo = Convert.ToString(com.ExecuteScalar());
                    data.message = "success";
                    //com.ExecuteNonQuery();
                    return data;
                }
                catch(Exception ex)
                {

                    data.ReturnStatus = 2;
                    data.MTitle = "Error";
                    data.MBody = ex.Message;
                    data.MType = "error";
                    return data;
                }
            }
        }
        public string UpdatePPAControl(BudgetControlModel formData)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var PPADescription = formData.PPADescription.Replace("'", "''");
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_UpdatePPAControl " + formData.RootPPA + ", " + formData.PPA + ", " + formData.PPAOOE + ", " + formData._AllotedAmount + ", " + formData._Obligate + ", " +
                 "" + formData._PPARelease + ", " + formData._PPAObligate + ", " + formData._PPAvailable + ", " + formData.AmountInputed + ", " + formData._Claim + ", '" + formData.PPATransOBRNo + "', " +
                 "'" + formData.PPAOBRNo + "', " + Account.UserInfo.eid + ", '" + PPADescription + "', " + formData.PPATransactionType + ", " + formData.PPAModeofExpense + ", 1, " + formData._SubsidyID + ", " + formData._PPAID + "", con);
                con.Open();
                try
                {
                    com.ExecuteNonQuery();
                    return "success";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
        }
        public BudgetControlModel AddPPAControl(BudgetControlModel formData)
        {
            BudgetControlModel data = new BudgetControlModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var query = "";
                var PPADescription = formData.PPADescription.Replace("'", "''");
                if (formData.PPAOBRNo != "0")
                {
                    query = @"dbo.sp_BMS_SavePPAControl " + formData.RootPPA + ", " + formData.PPA + ", " + formData.PPAOOE + ", " + formData._AllotedAmount + ", " + formData._Obligate + ", " +
                "" + formData._PPARelease + ", " + formData._PPAObligate + ", " + formData._PPAvailable + ", " + formData.AmountInputed + ", " + formData._Claim + ", '" + formData.PPATransOBRNo + "', " +
                "'" + formData.PPAOBRNo + "', " + Account.UserInfo.eid + ", '" + PPADescription + "', " + formData.PPATransactionType + ", " + formData.PPAModeofExpense + ", " + formData.PPAFundType + ", 2";
                }
                else if (formData.PPAOBRNo == "0")
                {
                    query = @"dbo.sp_BMS_SaveTempPPAControl " + formData.RootPPA + ", " + formData.PPA + ", " + formData.PPAOOE + ", " + formData._AllotedAmount + ", " + formData._Obligate + ", " +
                "" + formData._PPARelease + ", " + formData._PPAObligate + ", " + formData._PPAvailable + ", " + formData.AmountInputed + ", " + formData._Claim + ", '" + formData.PPATransOBRNo + "', " +
                "'" + formData.PPATransOBRNo + "', " + Account.UserInfo.eid + ", '" + PPADescription + "', " + formData.PPATransactionType + ", " + formData.PPAModeofExpense + ", " + formData.PPAFundType + ", 2, '"+formData.UserTransactionID+"' ";
                }

                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                try
                {

                    //com.ExecuteNonQuery();
                    //return "success";
                    data.OBRNo = Convert.ToString(com.ExecuteScalar());
                    data.message = "success";
                    return data;
                }
                catch (Exception ex)
                {
                    data.message =  ex.Message;
                    return data;
                }

            }
        }
        public string UpdateControl(BudgetControlModel formData)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //var ExpenseDescription = formData.ExpenseDescription;

                
                var ExpenseDescription = formData.ExpenseDescription.Replace("'", "''");
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_UpdateControl '" + formData.OBRNo + "', " + formData.TransactionType + ", " + formData.ModeOfExpense + ", " + formData.Office + ", " + formData.FundTypeValue + ", " + formData.Program + ", " +
                    "" + formData.ModePayment + ", " + formData.ObjOfExpenditure + ",'" + formData.TEVControlNoText + "'," + formData.TEVControlNo + ", " + formData.PPANonOffice + ", " + formData.SusProgram + ", " + formData.OOEText + ", '" + ExpenseDescription + "'," +
                    "" + formData.SubsidyIncome + ", " + formData.AllotedAmountValue + "," + formData.ObligateValue + ", " + formData.DiffObliandAllocatedValue + ", " + formData.AmountInputed + "," + formData.BalanceAllotmentValue + ", " + formData.AccountCharged + ", " +
                    "" + formData.ActualAmountValue + "," + formData.AccntChargeValue + ", " + formData.CurrentAllotmentValue + ", " + formData.UserID + ", '" + formData.OBRNoCenter + "', '" + formData.grtrnnoID + "', '" + formData.PastAccount + "'", con);
                con.Open();
                try
                {
                    com.ExecuteNonQuery();
                    return "success";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string ReturnControl(BudgetControlModel formData)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                var data = "";
                
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_ReturnControl " + formData.OBRNo + ", " + formData.AmountInputed + ", " +
                    "" + formData.AllotedAmountValue + "," + formData.ObligateValue + ", " + formData.DiffObliandAllocatedValue + ", " + formData.BalanceAllotmentValue + ", " + formData.AccountCharged + ", " +
                    "" + formData.ActualAmountValue + "," + formData.AccntChargeValue + ", " + formData.CurrentAllotmentValue + ", " +
                    "" + formData.UserID + ", '" + formData.OBRNoCenter + "', '" + formData.grtrnnoID + "', '"+formData.Remarks.Replace("'","''") +"', '"+formData.ORNumber+"'", con);
                con.Open();

                try
                {
                    //com.ExecuteNonQuery();
                    data = Convert.ToString(com.ExecuteScalar());
                    //return "success";
                    if (data == "1")
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string PPAReturn(BudgetControlModel formData)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var PPADescription = formData.PPADescription.Replace("'", "''");
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_PPAReturn " + formData.RootPPA + ", " + formData.PPA + ", " + formData.PPAOOE + ", " + formData._AllotedAmount + ", " + formData._Obligate + ", " +
                 "" + formData._PPARelease + ", " + formData._PPAObligate + ", " + formData._PPAvailable + ", " + formData.AmountInputed + ", " + formData._Claim + ", '" + formData.PPATransOBRNo + "', " +
                 "" + formData.PPAOBRNo + ", " + Account.UserInfo.eid + ", '" + PPADescription + "', " + formData.PPATransactionType + ", " + formData.PPAModeofExpense + ", 1, " + formData._SubsidyID + ", " + formData._PPAID + "", con); con.Open();
                con.Open();
                try
                {
                    query.ExecuteNonQuery();
                    return "success";
                }
                catch(Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string PPACancel(string OBRNo)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_CancelPPAControl '"+OBRNo+"',"+Account.UserInfo.eid+"", con);
                con.Open();
                try
                {
                    query.ExecuteNonQuery();
                    return "success";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string CancelControl(BudgetControlModel formData)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {


                string tmpID2 = "0";

                //DataTable _dt = new DataTable();
                //string _sqlQuery = "Select [PayrollBatchNo] from tblAMIS_PayrollLocation where [obrno]='" + formData.OBRNoCenter + "' and [Actioncode]=1";
                //_dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["fmisqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                //tmpID2 = _dt.Rows[0][0].ToString();

                var data = "";
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_CancelControl '" + formData.OBRNo + "', '" + formData.OBRNoCenter + "', " + Account.UserInfo.eid + "," + tmpID2 + "", con);
                con.Open();
                data = Convert.ToString(com.ExecuteScalar());
                return data;
            }
        }
        public string DeleteControl(int? TrnnoID, string OBRNo, string ControlNo)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_DeleteAllotmentItem " + TrnnoID + ", '" + OBRNo + "', '" + ControlNo + "'," + Account.UserInfo.eid + "", con);
                con.Open();
                try
                {
                    query.ExecuteNonQuery();
                    data = "success";
                }catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return data;
        }
        public string CheckIfExisit(int? ControlNo)
        {
            var value = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn())){
                SqlCommand com = new SqlCommand(@"SELECT COUNT(*) FROM tbl_T_BMSSubsidiaryLedger as a
                            LEFT JOIN tbl_R_BMSObrLogs as b ON a.OBRNo = b.OBRNo
                            WHERE b.trnno = '"+ControlNo+"'", con);
                con.Open();
                value = Convert.ToString(com.ExecuteScalar());
            }
            return value;
        }
        public string CheckIfCanceled(int? ControlNo)
        {
            var value = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT OBRNo FROM tbl_R_BMSObrLogs WHERE trnno = '" + ControlNo + "' and ReferenceNo is NULL", con);
                con.Open();
                value = Convert.ToString(com.ExecuteScalar());
            }
            return value;
        }
        public double CheckCurrentControl(int? accountCharge, int? Program)
        {
            double valueObligate = 0, valueRelease = 0, valueDifference = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand Obligate = new SqlCommand(@"SELECT  SUM(a.Amount)
                    FROM tbl_T_BMSSubsidiaryLedger as a
                    WHERE a.Budget_AcctCharge = '" + accountCharge + "' and a.ProgramID = '" + Program + "' and a.ActionCode = 1 and a.DateTimeEntered LIKE CONCAT('%',YEAR(GETDATE()),'%')", con);
                con.Open();
                valueObligate = Convert.ToDouble(Obligate.ExecuteScalar());

                SqlCommand Release = new SqlCommand(@"SELECT (SUM(a.AmountPS)+SUM(a.AmountMOOE)+SUM(a.AmountCO))
                    FROM tbl_R_BMS_Release as a
                    WHERE a.FMISAccountCode = '" + accountCharge + "' and a.FMISProgramCode = '" + Program + "' and a.ActionCode = 1 and a.YearOf = YEAR(GETDATE())", con);
                valueRelease = Convert.ToDouble(Release.ExecuteScalar());

                valueDifference = valueRelease - valueObligate;
            }
            return valueDifference;
        }
        public double CheckClaim(int? ControlNo)
        {
            double value = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT ISNULL(SUM(Claim), 0) from tbl_R_BMSCurrentControl_RawData WHERE ControlNo = '"+ControlNo+"' and ActionCode = 1", con);
                con.Open();
                value = Convert.ToDouble(query.ExecuteScalar());
            }
            return value;
        }
        public string OBRUserTime()
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT dbo.fn_BMS_DateString()",con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        public OBRLogger CheckOutOBR(int? TransactionNo, string OBRNowithFnCode = "", string ObrNoASs="", long? approveby = 0)
        {
            OBRLogger data = new OBRLogger();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                string tmpID3 = "0";
                try
                {
                  
                    DataTable _dt2 = new DataTable();
                    string _sqlQuery = "exec getPayrollBatchno " + TransactionNo + "";
                    _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["fmisqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                    tmpID3 = _dt2.Rows[0][0].ToString();
                }
                catch { tmpID3 = "0"; }

                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_CheckOutOBR " + Account.UserInfo.eid + ", " + TransactionNo + ",'" + tmpID3 + "',"+ approveby + "", con);
                con.Open();
                try
                {
                    SqlDataReader reader = query.ExecuteReader();
                    while(reader.Read()){
                        data.type = "success";
                        data.title = "Success!";
                        data.message = "Transaction has been successfully checked out.";
                        data.UserOutTimeStamp = Convert.ToString(reader.GetValue(0));
                    }
                }catch(Exception ex)
                {
                    data.type = "error";
                    data.title = "Error";
                    data.message = ex.Message;
                    data.UserOutTimeStamp = "";
                  
                }
            }
            return data;
        }
        public OBRLogger CheckOutOBRv2(int? TransactionNo, string OBRNowithFnCode = "", string ObrNoASs = "", int? EmployeeForward = 0, string otherindiv_id = "")
        {
            OBRLogger data = new OBRLogger();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                string tmpID3 = "0";
                try
                {

                    DataTable _dt2 = new DataTable();
                    string _sqlQuery = "exec getPayrollBatchno " + TransactionNo + "";
                    _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["fmisqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                    tmpID3 = _dt2.Rows[0][0].ToString();
                }
                catch { tmpID3 = "0"; }

                SqlCommand query = new SqlCommand(@"dbo.[sp_BMS_CheckOutOBR_v2] " + Account.UserInfo.eid + ", " + TransactionNo + ",'" + tmpID3 + "',"+ EmployeeForward + ",'"+ otherindiv_id + "'", con);
                con.Open();
                try
                {
                    SqlDataReader reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        data.type = "success";
                        data.title = "Success!";
                        data.message = "Transaction has been successfully checked out.";
                        data.UserOutTimeStamp = Convert.ToString(reader.GetValue(0));
                    }
                }
                catch (Exception ex)
                {
                    data.type = "error";
                    data.title = "Error";
                    data.message = ex.Message;
                    data.UserOutTimeStamp = "";

                }
            }
            return data;
        }
        public string SaveAppropriation(double? Amount, string AccountName, int? Year, int? FundType)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExcessAppropriations "+Amount+", '"+AccountName+"', "+Year+", "+FundType+", "+Account.UserInfo.eid+"", con);
                con.Open();
                try
                {
                    query.ExecuteNonQuery();
                    return "success";
                }catch(Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string UpdateAppropriation(int? YearOf, double? AmountAppropriation, string AccountNameAppropriation, int? FundType, int? ExcessID, string PastAccount)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_UpdateExcessAppropriations "+YearOf+", "+AmountAppropriation+", '"+ AccountNameAppropriation.Replace("'","''") +"', "+FundType+", "+ExcessID+", "+Account.UserInfo.eid+", '"+PastAccount.Replace("'","''")+"'", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        public string DeleteAppropriation(int? ExcessID)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_DeleteExcessAppropriations " + ExcessID + ", " + Account.UserInfo.eid + "", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        public BudgetControlModel SearchAirMark(string getControlNo)
        {
            BudgetControlModel data = new BudgetControlModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                getControlNo = getControlNo == "" ? "" : getControlNo;
                SqlCommand query = new SqlCommand("dbo.sp_BMS_SearchAirMark '"+getControlNo+"'", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    data.AROBRNo = Convert.ToString(reader.GetValue(0));
                    data.AROBRSeries = Convert.ToString(reader.GetValue(1));
                    data.AROffice = Convert.ToInt32(reader.GetValue(2));
                    data.ARProgram = Convert.ToInt32(reader.GetValue(3));
                    data.ARActualOffice = Convert.ToString(reader.GetValue(4));
                    data.ARAccount = Convert.ToInt32(reader.GetValue(5));
                    data.ARAmount = Convert.ToDecimal(reader.GetValue(6));
                    data.ARIncome = Convert.ToInt32(reader.GetValue(7));
                    data.AROOE = Convert.ToInt32(reader.GetValue(8));
                    data.ARIfControlExist = Convert.ToInt32(reader.GetValue(9));
                    data.ExcessAppropriationID = Convert.ToInt32(reader.GetValue(10));
                    data.subppa = Convert.ToInt32(reader.GetValue(11));
                }

            }
            return data;
        }
        public BudgetControlModel PPAAirMark(string getControlNo, string PPATransOBRNo)
        {
            BudgetControlModel data = new BudgetControlModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                getControlNo = getControlNo == "" ? "" : getControlNo;
                SqlCommand query = new SqlCommand("dbo.sp_BMS_SearchPPAAirMark '" + getControlNo + "', '" + PPATransOBRNo + "'", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    data.AROBRNo = Convert.ToString(reader.GetValue(0));
                    data.AROBRSeries = Convert.ToString(reader.GetValue(1));
                    data.AROffice = Convert.ToInt32(reader.GetValue(2));
                    data.ARProgram = Convert.ToInt32(reader.GetValue(3));
                    data.ARActualOffice = Convert.ToString(reader.GetValue(4));
                    data.ARAccount = Convert.ToInt32(reader.GetValue(5));
                    data.ARAmount = Convert.ToDecimal(reader.GetValue(6));
                    data.ARIncome = Convert.ToInt32(reader.GetValue(7));
                    data.AROOE = Convert.ToInt32(reader.GetValue(8));
                    data.ARIfControlExist = Convert.ToInt32(reader.GetValue(9));
                    data.ExcessAppropriationID = Convert.ToInt32(reader.GetValue(10));
                    data.PPAID = Convert.ToInt32(reader.GetValue(11));
                    data.RootPPAID = Convert.ToInt32(reader.GetValue(12));
                }
            }
            return data;
        }
        public BudgetControlModel OfficeRemainingBalance(int? Office=0, int? Account=0, int? Program=0, int? TransactionYear=0)
        {
            BudgetControlModel data = new BudgetControlModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_OfficeRemainingBalance "+Office+", "+Account+", "+Program+", "+TransactionYear+"", con);
                con.Open();
                try
                {
                    SqlDataReader reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        data.Amount = Convert.ToDecimal(reader.GetValue(0));
                        data.OfficeName = Convert.ToString(reader.GetValue(1));
                        data.message = Convert.ToString(reader.GetValue(2));
                        data.type = "success";
                    }
                }
                catch (Exception ex)
                {
                    data.OfficeName = "Error";
                    data.message = ex.Message;
                    data.type = "error";
                    data.Amount = 0;
                }
                

            }
            return data;
        }
        public BudgetControlModel ComputeLDRRMF(int? PPANonOffice, int? Year)
        {
            BudgetControlModel data = new BudgetControlModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT * FROM dbo.fn_BMS_NonOfficeLDRRMF("+PPANonOffice+", "+Year+")", con);
                con.Open();
                try
                {
                    SqlDataReader reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        data.AllotedAmount = Convert.ToDecimal(reader.GetValue(0));
                        data.ObligatedAmount = Convert.ToDecimal(reader.GetValue(1));
                    }
                }
                catch (Exception ex)
                {
                    data.AllotedAmount = 0;
                    data.ObligatedAmount = 0;
                }


            }
            return data;
        }
        //----- Excess Control
        public ExcessModel SearchOBRDetails(string ControlNum, int? YearOf)
        {
            ExcessModel data = new ExcessModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                ControlNum = ControlNum == "" ? "" : ControlNum;
                SqlCommand query = new SqlCommand("dbo.sp_BMS_SearchOBR '"+ControlNum+"', "+YearOf+"", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    data.OBRNo = Convert.ToString(reader.GetValue(0));
                    data.OBRSeries = Convert.ToString(reader.GetValue(1));
                }
            }
            return data;
        }
        public ExcessModel EditExcessControl(int? TrnnoID)
        {
            ExcessModel data = new ExcessModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand("dbo.sp_BMS_SearchExcessControl "+TrnnoID+"", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    data.OBRNo = Convert.ToString(reader.GetValue(0));
                    data.Description = Convert.ToString(reader.GetValue(1));
                    data.Amount = Convert.ToDouble(reader.GetValue(2));
                    data.FundFlag = Convert.ToInt32(reader.GetValue(3));
                    data.ExcessAppropriationNo = Convert.ToString(reader.GetValue(4));
                    data.Appropriation = Convert.ToDouble(reader.GetValue(5));
                    data.Obligation = Convert.ToDouble(reader.GetValue(6));
                    data.Allotment = Convert.ToDouble(reader.GetValue(7));
                    data.Balance = Convert.ToDouble(reader.GetValue(8));
                    data.Office = Convert.ToInt32(reader.GetValue(9));
                    data.Program = Convert.ToString(reader.GetValue(10));
                    data.NonOfficeAccount = Convert.ToString(reader.GetValue(11));
                    data.AcctChecker = Convert.ToInt32(reader.GetValue(12));
                    data.trnno = Convert.ToInt32(reader.GetValue(13));
                    data.ppaid = Convert.ToInt32(reader.GetValue(14));
                    data.spoid = Convert.ToInt32(reader.GetValue(15));
                    data.obrnotemp= Convert.ToString(reader.GetValue(16));
                }
            }
            return data;
        }
        public double ExcessSetAppropriation(int? AccountID, int? FundType)
        {
            double data = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ViewExcessAppropriations " + FundType + ", 2, " + AccountID + ", 0", con);
                con.Open();
                data = Convert.ToDouble(query.ExecuteScalar());
            }
            return data;
        }
        public double ExcessObligation(int? AccountID, int? FundType)
        {
            double data = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ViewExcessAppropriations " + FundType + ", 3, " + AccountID + ", 0", con);
                con.Open();
                data = Convert.ToDouble(query.ExecuteScalar());
            }
            return data;
        }
        public string SearchOBRNo(int? TrnnoID)
        {
            var data = "";
            using(SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT OBRNo FROM tbl_T_BMSExcessControl WHERE trnno_id = '"+TrnnoID+"'", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        
        //----- End of Excess Control
        public string SetPrefixTempOBR(int? Year)
        {
            var data = "";
            using(SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_GenerateTempOBR "+Account.UserInfo.Department+", 1,"+ Year + "", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        
        public string SeTempcomOBR(int? Year,int? param)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_GenerateTempOBR " + Account.UserInfo.Department + ", "+ param + "," + Year + "", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        public BudgetControlModel CheckIfAirMark(string ControlNo)
        {
            BudgetControlModel data = new BudgetControlModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT * FROM  dbo.fn_BMS_CheckIfExistInAirMark('" + ControlNo + "')", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    data.FromAirMark = Convert.ToInt32(reader.GetValue(0));
                    data.FromOBR = Convert.ToInt32(reader.GetValue(1));
                }
                
            }
            return data;
        }
        public string SetPrefixTemp20OBR(int? Year)
        {

            
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_GenerateTempOBR " + Account.UserInfo.Department + ", 6,"+ Year + "", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }

        public SearchOBRResult_Model YearMonth(string ControlNo)
        {
            SearchOBRResult_Model data = new SearchOBRResult_Model(); ;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"
                    DECLARE @OBR VARCHAR(25) = '"+ControlNo+"' " +

                    " SELECT " +
                    " CASE WHEN a.OBRSeries = @OBR THEN 1 ELSE 0 END as OBRSeries, " +
                    " CASE WHEN a.NonOfficeTransNo = @OBR THEN 1 ELSE 0 END as NonOfficeTransNo, " +
                    " CASE WHEN a.TransactionNo = @OBR THEN 1 ELSE 0 END as TransactionNo, " +
                    " CASE WHEN a.OBRNo = @OBR THEN 1 ELSE 0 END as OBRNo " + 
                    " FROM tbl_R_BMSObrLogs as a " +
                    " WHERE (a.OBRSeries = '" + ControlNo + "' or a.NonOfficeTransNo = '" + ControlNo + "' or a.TransactionNo = '" + ControlNo + "' or a.OBRNo = '"+ControlNo+"')", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    data.resultOBRSeries = Convert.ToInt32(reader.GetValue(0));
                    data.resultNonOfficeTransNo = Convert.ToInt32(reader.GetValue(1));
                    data.resultTransactionNo = Convert.ToInt32(reader.GetValue(2));
                    data.resultOBRNo = Convert.ToInt32(reader.GetValue(3));
                }
            }
            return data;
        }
        public string OBRNoFromTempOBR(string ControlNo)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT dbo.fn_BMS_getOBRNoFromTempOBR('" + ControlNo + "')", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }

        //public string SearchOBR(string ControlNo)
        //{
        //    var dataOBRseries = "";
        //    var dataOBRNo = "";
        //    var data = "";
        //    using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //    {
        //        ControlNo = ControlNo == "" ? "0" : ControlNo;
        //        //                SqlCommand com = new SqlCommand(@"Select 
        //        //                    CASE WHEN COUNT(1) > 0 THEN MAX(OBRSeries) ELSE '' END AS [OBRSeries], 
        //        //                    CASE WHEN COUNT(1) > 0 THEN MAX(ISNULL(OBRNo, 'Null')) ELSE '' END AS [OBRNo]
        //        //                    FROM tbl_R_BMSObrLogs WHERE trnno = " + ControlNo + " or OBRNo = '" + ControlNo + "'", con);
        //        SqlCommand com = new SqlCommand(@"dbo.sp_BMS_SearchOBRControl '" + ControlNo + "', 1", con);
        //        con.Open();
        //        SqlDataReader reader = com.ExecuteReader();
        //        reader.Read();
        //        dataOBRseries = Convert.ToString(reader.GetValue(0));
        //        dataOBRNo = Convert.ToString(reader.GetValue(1));
        //        if (dataOBRNo == "Null")
        //        {
        //            data = dataOBRseries;
        //        }
        //        else
        //        {
        //            data = dataOBRNo;
        //        }

        //    }
        //    return data;
        //}

        public BudgetControlModel SearchOBR(string ControlNo)
        {
            var dataOBRseries = "";
            var dataOBRNo = "";
            BudgetControlModel data = new BudgetControlModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                ControlNo = ControlNo == "" ? "0" : ControlNo;
                //                SqlCommand com = new SqlCommand(@"Select 
                //                    CASE WHEN COUNT(1) > 0 THEN MAX(OBRSeries) ELSE '' END AS [OBRSeries], 
                //                    CASE WHEN COUNT(1) > 0 THEN MAX(ISNULL(OBRNo, 'Null')) ELSE '' END AS [OBRNo]
                //                    FROM tbl_R_BMSObrLogs WHERE trnno = " + ControlNo + " or OBRNo = '" + ControlNo + "'", con);
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_SearchOBRControl '" + ControlNo + "', 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dataOBRseries = Convert.ToString(reader.GetValue(0));
                    dataOBRNo = Convert.ToString(reader.GetValue(1));
                    if (dataOBRNo == "Null" || dataOBRNo == "")
                    {
                        data.OBRNo = dataOBRseries;
                    }
                    else
                    {
                        data.OBRNo = dataOBRNo;
                    }
                    data.OfficeID = Convert.ToInt32(reader.GetValue(2));
                    data.ProgramID = Convert.ToInt32(reader.GetValue(3));
                }
            }
            return data;
        }
        public BudgetControlModel OBRNo20FromTempOBR(string ControlNo)
        {
            BudgetControlModel data = new BudgetControlModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"Select * from [fn_BMS_get20OBRNoFromTempOBR_Log] ('" + ControlNo + "')", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    data.OBRNo = reader.GetValue(0).ToString();
                    data.obrseries = reader.GetValue(1).ToString();
                    data.functioncode = Convert.ToInt32(reader.GetValue(2));
                }
            }
            return data;

        }
        public int VerifyOfficeRef(string getControlNo)
        {
            var data = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT dbo.fn_BMS_CheckUserOfficeRef('" + Account.UserInfo.eid + "', '" + getControlNo + "')", con);
                con.Open();
                data = Convert.ToInt32(query.ExecuteScalar());
            }
            return data;
        }
        public int GetFundCode(int? OfficeID)
        {
            var data = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT FundID FROM tbl_R_BMSOffices WHERE OfficeID = "+OfficeID+"", con);
                con.Open();
                data = Convert.ToInt32(query.ExecuteScalar());
            }
            return data;
        }
        public string GenerateTransaction()
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_GenerateTransactionID "+Account.UserInfo.eid+" ", con);
                con.Open();
                data = Convert.ToString(query.ExecuteScalar());
            }
            return data;
        }
        public int CheckIfRef(string ControlNo)
        {
            var data = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT COUNT(*) FROM fmis.dbo.tblBMS_AirMark WHERE AlobsNo LIKE '%" + ControlNo + "'%", con);
                con.Open();
                data = Convert.ToInt32(query.ExecuteScalar());
            }
            return data;
        }
        public ExcessModel CheckIFOBRExistInAirMark(string OBRNo)
        {
            
            ExcessModel data = new ExcessModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT * FROM dbo.fn_BMS_CheckIFOBRExistinAirMark('" + OBRNo + "')", con);
                con.Open();
                //data = Convert.ToInt32(query.ExecuteScalar());
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    data.Count = Convert.ToInt32(reader.GetValue(0));
                    data.Amount = Convert.ToDouble(reader.GetValue(1));
                }
            }
            return data;
        }
        public BudgetControlModel SearchIfReferenceExist(string RefNo)
        {
            BudgetControlModel data = new BudgetControlModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_SearchIfReferenceExist '"+RefNo+"'", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    data.CountRow = Convert.ToInt32(reader.GetValue(0));
                    data.FundTypeID = Convert.ToInt32(reader.GetValue(1));
                    data.Description = Convert.ToString(reader.GetValue(2));
                    data.Amount = Convert.ToDecimal(reader.GetValue(3));
                    data.IfExist = Convert.ToInt32(reader.GetValue(4));
                    data.type = Convert.ToString(reader.GetValue(5));
                    data.message = Convert.ToString(reader.GetValue(6));
                }    
            }
            return data;
        }
        public OBRLogger UpdateOBR(int? TransactionNo, string RefNo, string Particular,int? Year,int? officeassign, int? EmployeeForward,string otherindiv_id, long approveby)
        {
            OBRLogger data = new OBRLogger();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                Particular = Particular.Replace("'", "''");
                if (Account.UserInfo.lgu == 0)//pgas
                {
                    SqlCommand query = new SqlCommand(@"dbo.sp_BMS_UPDATEOBR " + TransactionNo + ", '" + RefNo + "', '" + Particular + "'," + Year + "," + officeassign + ","+ approveby + "", con);
                    query.CommandTimeout = 0;
                    con.Open();
                    SqlDataReader reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        data.title = Convert.ToString(reader.GetValue(0));
                        data.message = Convert.ToString(reader.GetValue(1));
                        data.type = Convert.ToString(reader.GetValue(2));
                    }
                }
                else //other lgu's
                {
                    SqlCommand query = new SqlCommand(@"dbo.sp_BMS_UPDATEOBR " + TransactionNo + ", '" + RefNo + "', '" + Particular + "'," + Year + ","+ officeassign + ","+ EmployeeForward + ",'"+ otherindiv_id + "'," + approveby + "", con);
                    query.CommandTimeout = 0;
                    con.Open();
                    SqlDataReader reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        data.title = Convert.ToString(reader.GetValue(0));
                        data.message = Convert.ToString(reader.GetValue(1));
                        data.type = Convert.ToString(reader.GetValue(2));
                    }
                }
            }
            return data;
        }
    }
}