using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class CopyProgramAccount_Layer
    {
        public string CopyProgramAccount(copyProgram CopyProgramAccounts)
        {
           
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"sp_bms_CopyProgramAccounts " + CopyProgramAccounts.datepickerFROM + "," + CopyProgramAccounts.datepickerTO + "," + Account.UserInfo.eid.ToString() + "", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            
        }
        public void QueryInsert(int ProgramID, int AccountID, int ObjectOfExpendetureID, int ActionCode, int OrderNo, string timeDate, string AccountName, string UserInfo, string datepickerTO)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_insert = new SqlCommand(@"Insert into tbl_R_BMSPRogramAccounts values(" + ProgramID + ", " + AccountID + ", " + ObjectOfExpendetureID + ", " + ActionCode + ", '" + timeDate + "', '" + AccountName + "', '" + UserInfo + "', " + OrderNo + ", " + datepickerTO + ")", con);
                 con.Open();   
                    try
                    {
                        query_insert.ExecuteNonQuery();
                    }
                    catch
                    {
                        
                    }                   
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_insert = new SqlCommand(@"Insert into fmis.dbo.tblBMS_AnnualBudget_Account values(" + ProgramID + ", " + AccountID + ", " + ObjectOfExpendetureID + ", " + ActionCode + ", '" + timeDate + "', '" + AccountName + "', '" + UserInfo + "', " + OrderNo + ", " + datepickerTO + ")", con);
                con.Open();
                try
                {
                    query_insert.ExecuteNonQuery();
                }
                catch
                {

                }

            }
        }
        public void QueryInsertOfficeProgram(int op_OfficeID, int op_ProgramID, int op_ActionCode, string timeDate, string UserInfo, string op_ProgramDesc, int op_OrderNo, string datepickerTO, int op_FundCode)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_insertOfficeProgram = new SqlCommand(@"Insert into tbl_R_BMSOfficePrograms values("+op_OfficeID+", "+op_ProgramID+", "+op_ActionCode+", '"+timeDate+"', '"+UserInfo+"', '"+op_ProgramDesc+"', "+op_OrderNo+", '"+datepickerTO+"', "+op_FundCode+")", con);
                con.Open();
                try
                {
                    query_insertOfficeProgram.ExecuteNonQuery();
                }
                catch
                {

                }  
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_insertOfficeProgram = new SqlCommand(@"Insert into tblRefBMS_BudgetProgram values(" + op_OfficeID + ", " + op_ProgramID + ", " + op_ActionCode + ", '" + timeDate + "', '" + UserInfo + "', '" + op_ProgramDesc + "', " + op_OrderNo + ", '" + datepickerTO + "', " + op_FundCode + ")", con);
                con.Open();
                try
                {
                    query_insertOfficeProgram.ExecuteNonQuery();
                }
                catch
                {

                }
            }
        }
    }
}