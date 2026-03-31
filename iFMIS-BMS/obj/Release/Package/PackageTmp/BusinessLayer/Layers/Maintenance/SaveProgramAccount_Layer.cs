using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class SaveProgramAccount_Layer
    {
        //decimal FMISAccountCode;
        public string SaveProgramAccount(saveProgramAccount ProgramAccount)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (ProgramAccount.office_id == 0 ||ProgramAccount.program_id == 0 || ProgramAccount.ooe_id == 0 || ProgramAccount.orderBy == 0 || ProgramAccount.ProposalYear == 0 || ProgramAccount.AccountID == 0)
                {
                    return "All Fields required";
                }
                else
                {
                    var UserInfo = Account.UserInfo.eid.ToString();
                    SqlCommand com = new SqlCommand(@"sp_BMS_CreateUpdateProgramAccount "+ProgramAccount.ref_account_id+","+ProgramAccount.program_id+","+ProgramAccount.AccountID
                                                            + "," + ProgramAccount.ooe_id + ",'" + (ProgramAccount.account_name == null ? "" :ProgramAccount.account_name.Replace("'", "''"))
                                                            +"',"+UserInfo+","+ProgramAccount.orderBy+","+ProgramAccount.ProposalYear+","+ProgramAccount.isProposed+"", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }   
            }
        }
        public string AddOfficeAccounts(int OfficeID, int ProgramID, int OOEID, int YearOf, int AccountID, int OrderNo, string AccountName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var UserInfo = Account.UserInfo.eid.ToString();
                    SqlCommand com = new SqlCommand(@"sp_BMS_CreateUpdateProgramAccount 0," + ProgramID + "," + AccountID
                                                    + "," + OOEID + ",'" + (AccountName == null ? "" : AccountName.Replace("'", "''"))
                                                            + "'," + UserInfo + "," + OrderNo + "," + YearOf + ",0", con);
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