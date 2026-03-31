using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Models;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Connector;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class SaveProgram_Layer
    {
        public string SaveNewProgram(saveProgram Program)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (Program.program_desc == null || Program.fund_id == 0 || Program.datepicker == "" || Program.office_id == 0)
                {
                    return "All fields are Required!";
                }
                else
                {
                var UserInfo = Account.UserInfo.eid.ToString();
                SqlCommand com = new SqlCommand(@"sp_BMS_CreateUpdateProgram " + Program.program_id + "," + Program.text_office_id
                                                            + "," + UserInfo + ",'" + Program.program_desc.Replace("'","''") + "'," + Program.orderBy + "," + Program.datepicker + "," + Program.fund_id + "", con);
                con.Open();
                return com.ExecuteScalar().ToString();
                }

            }
        }
        public string RemoveProgramAccount(int ProgramAccountID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var UserInfo = Account.UserInfo.eid.ToString();
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSProgramAccounts set ActionCode = 0 where ProgramAccountID = "+ProgramAccountID+"", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}