using iFMIS_BMS.BusinessLayer.Connector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class RestoreAccount_Layer
    {
        public string RestoreAccount(int account_ID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_program = new SqlCommand(@"UPDATE tbl_R_BMSAccounts set Active = 1 where AccountID=" + account_ID.ToString(), con);
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