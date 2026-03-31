using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using iFMIS_BMS.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class UpdateAccount_Layer
    {
        public string UpdateAccounts(account_code UpdateAccount)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_program = new SqlCommand(@"UPDATE ref_account SET account_desc='"+UpdateAccount.account_desc+"', code='"+UpdateAccount.code+"', child_series_no='"+UpdateAccount.child_series_no+"', fund_id="+UpdateAccount.fund_id+" where ref_account_id ="+UpdateAccount.ref_account_id, con);

                con.Open();
                try
                {
                    query_program.ExecuteNonQuery();
                    return "Success";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string getAccountOrderNo(int ProgramID, int OOEID, int YearOf)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_program = new SqlCommand(@"select top 1 isnull(OrderNo,0) from tbl_R_BMSProgramAccounts Where ProgramID = " + ProgramID + " and ObjectofExpendetureID = " + OOEID
                    +" and AccountYear = "+YearOf+" order by OrderNo Desc", con);
                con.Open();
                try
                {
                    return (Convert.ToInt32(query_program.ExecuteScalar()) + 1).ToString();
                }
                catch (Exception)
                {
                    return "1";
                }
            }
        }
       public string UpdateAccountOrder(int orderNo, int AccountID){
           using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSProgramAccounts Set OrderNo = '" + orderNo + "' where AccountID = '" + AccountID + "'", con);
                SqlCommand com2 = new SqlCommand(@"Update fmis.dbo.tblBMS_AnnualBudget_Account Set OrderNo = '" + orderNo + "' where FMISAccountCode = '" + AccountID + "'", con);
                con.Open();
                try
                {
                    com.ExecuteNonQuery();
                    com2.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
       }
       public string SetMergeStatus(int SeriesID, int isCombine)
        {
            var Query = "";
            if (SeriesID == 0)
            {
                Query = @"Update tbl_R_BMSAccountsToCombine Set isCombineWithNonOffice = " + isCombine + " where ActionCode  = 1";
            }
            else
            {
                Query = @"Update tbl_R_BMSAccountsToCombine Set isCombineWithNonOffice = " + isCombine + " where SeriesID = " + SeriesID + "";
            }
           using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(Query, con);
                con.Open();
                try
                {
                    com.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
       }
        
       public string UpdateAccountReference(int ReferenceAccount, string[] SelectedAccounts)
       {
           try
           {
               DataTable dt = new DataTable();
               dt.Columns.Add("AccountID");
               var idx = 0;
               foreach (var AccountID in SelectedAccounts)
               {
                   DataRow dr = dt.NewRow();
                   dr[0] = SelectedAccounts[idx];
                   dt.Rows.Add(dr);
                   idx++;
               }
               using (SqlConnection con = new SqlConnection(Common.MyConn()))
               {
                   SqlCommand com = new SqlCommand("dbo.sp_bms_ChangeProposedAccountsReference", con);
                   com.CommandType = CommandType.StoredProcedure;
                   com.Parameters.Add(new SqlParameter("@SelectedAccounts", dt));
                   com.Parameters.Add(new SqlParameter("@ReferenceAccount", ReferenceAccount));
                   com.Parameters.Add(new SqlParameter("@Yearof", Convert.ToInt32(GetTransactionYear.TransactionYear())));
                   con.Open();
                   //  DataTable dt_record_holder = new DataTable();
                   // dt_record_holder.Load(com.ExecuteReader());
                   return com.ExecuteScalar().ToString();
               }
           }
           catch (Exception ex)
           {
               return ex.Message;
           }
          
       }
       public AccountsModel getSelectedAccountData(int AccountID)
       {
           AccountsModel AccountsModel = new AccountsModel();
           using (SqlConnection con = new SqlConnection(Common.MyConn()))
               {
                   SqlCommand com = new SqlCommand(@"select a.AccountID,isnull(b.OrderNo,0),b.ObjectOfExpendetureID,b.AccountName,c.OfficeID,b.ProgramID, a.YearOf from tbl_R_BMSAccountsForBuildUp as a 
                   LEFT JOIN tbl_R_BMSProgramAccounts as b on b.AccountID = a.AccountID
                   LEFT JOIN tbl_R_BMSOfficePrograms as c on c.ProgramID = b.ProgramID and c.ProgramYear = b.AccountYear and c.ActionCode = 1
                   where a.AccountID = " + AccountID + "", con);
                   con.Open();
                   SqlDataReader reader = com.ExecuteReader();
                   while (reader.Read())
                   {
                       AccountsModel.AccountID = Convert.ToInt32(reader.GetValue(0));
                       AccountsModel.AccountCode = Convert.ToInt32(reader.GetValue(1)); //ORDER NO.
                       AccountsModel.OOEID = Convert.ToInt32(reader.GetValue(2));
                       AccountsModel.AccountName = reader.GetValue(3).ToString();
                       AccountsModel.SelectedOfficeID = Convert.ToInt32(reader.GetValue(4));
                       AccountsModel.ProgramID = Convert.ToInt32(reader.GetValue(5));
                       AccountsModel.ProposalYear = Convert.ToInt32(reader.GetValue(6));
                   }

               }
           return AccountsModel;
       }
       public string BuildNewAccount(string AccountName, int AccountID, string AccountCode
          , string ChildAccountCode, int ProgramID, int ThirdLevelGroupID, string ThirdLevelGroupDesc
          , string FundType, int OrderNo, int RefYearOf, int RefProgramID)
       {
           try
           {
               using (SqlConnection con = new SqlConnection(Common.MyConn()))
               {
                   SqlCommand com = new SqlCommand(@"dbo.sp_BMS_BuildAccount", con);
                   com.CommandType = CommandType.StoredProcedure;
                   com.Parameters.Add(new SqlParameter("@AccountName", AccountName));
                   com.Parameters.Add(new SqlParameter("@RefAccountID", AccountID));
                   com.Parameters.Add(new SqlParameter("@AccountCode", AccountCode));
                   com.Parameters.Add(new SqlParameter("@ChildAccountCode", ChildAccountCode));
                   com.Parameters.Add(new SqlParameter("@ProgramID", ProgramID));
                   com.Parameters.Add(new SqlParameter("@ThirdLevelGroupID", ThirdLevelGroupID));
                   com.Parameters.Add(new SqlParameter("@ThirdLevelGroupDesc", ThirdLevelGroupDesc));
                   com.Parameters.Add(new SqlParameter("@FundType", FundType));
                   com.Parameters.Add(new SqlParameter("@OrderNo", OrderNo));
                   com.Parameters.Add(new SqlParameter("@RefYearOf", RefYearOf));
                   com.Parameters.Add(new SqlParameter("@RefProgramID", RefProgramID));
                   con.Open();
                   return com.ExecuteScalar().ToString();
               }
           }
           catch (Exception ex)
           {
               return ex.Message;
           }
       }
       public IEnumerable<AccountsToCombineModel> grAccountsToCombine()
       {
           List<AccountsToCombineModel> AccountList = new List<AccountsToCombineModel>();
           using (SqlConnection con = new SqlConnection(Common.MyConn()))
           {
               SqlCommand com = new SqlCommand(@"select a.SeriesID,b.AccountName + ' (' + b.FundType + ')',a.isCombineWithNonOffice from tbl_R_BMSAccountsToCombine as a
                                                LEFT JOIN tbl_R_BMSAccounts as b on b.FMISAccountCode = a.AccountID
                                                where a.ActionCode = 1 order by SeriesID", con);
               con.Open();
               SqlDataReader reader = com.ExecuteReader();
               while (reader.Read())
               {
                   AccountsToCombineModel account = new AccountsToCombineModel();
                   account.SeriesID = Convert.ToInt32(reader.GetValue(0));
                   account.AccountName = reader.GetValue(1).ToString();
                   account.isCombineWithNonOffice = Convert.ToInt32(reader.GetValue(2));
                   AccountList.Add(account);

               }
           }
           return AccountList;
       }
       public string getExistingAccounts(int ProgramID, int OOEID, int YearOf) 
       {
           try
           {
               using (SqlConnection con = new SqlConnection(Common.MyConn()))
               {
                   SqlCommand com = new SqlCommand(@"SELECT top 1 STUFF((
                                                SELECT ',' + Cast(AccountID as VARCHAR(100))
                                                FROM tbl_R_BMSProgramAccounts where ProgramID = "+ProgramID+" and ObjectOfExpendetureID = "+OOEID+" and AccountYear = "+YearOf+""
                                                +" FOR XML PATH('') " + ""
                                                +" ), 1, 1, '') "+ ""
                                                +" FROM tbl_R_BMSProgramAccounts where ProgramID = "+ProgramID+" and ObjectOfExpendetureID = "+OOEID+" and AccountYear = "+YearOf+"", con);
                   con.Open();
                   return com.ExecuteScalar().ToString();
               }
           }
           catch (Exception)
           {
               return "Error";
           }
       }
       public IEnumerable<ProgramAccountsModel> getChartOfAccountsData()
       {
           List<ProgramAccountsModel> AccountList = new List<ProgramAccountsModel>();
           using (SqlConnection con = new SqlConnection(Common.MyConn()))
           {
               SqlCommand com = new SqlCommand(@"SELECT DISTINCT
                            AccountName + ' (' +  isnull(FundType,'') + ')' as AccountName,
                            FMISAccountCode,FundType
                            FROM tbl_R_BMSAccounts
                            WHERE Active = 1 and AccountName is not null and AccountName not like '%payable%' and AccountName not like '%general fund%'", con);
               con.Open();
               SqlDataReader reader = com.ExecuteReader();
               while (reader.Read())
               {
                   ProgramAccountsModel account = new ProgramAccountsModel();
                   account.AccountID = Convert.ToInt32(reader.GetValue(1));
                   account.AccountDescripttion = reader.GetValue(0).ToString();
                   account.ProgramDescription = reader.GetValue(2).ToString();
                   AccountList.Add(account);

               }
           }
           return AccountList;
       }
        
    }
}