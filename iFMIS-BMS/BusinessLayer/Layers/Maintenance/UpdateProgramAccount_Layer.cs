using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class UpdateProgramAccount_Layer
    {
        public string UpdateProgramAccount(account_code ProgramAccount)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_program = new SqlCommand(@"UPDATE sample_account SET ref_account_id="+ProgramAccount.ref_accountID+", program_id="+ProgramAccount.programID+", office_id="+ProgramAccount.officeID+" where account_id="+ProgramAccount.account_id, con);
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
    }
}