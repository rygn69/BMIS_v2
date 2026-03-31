using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class UpdateProgram_Layer
    {
        public string UpdatePrograms(programs UpdateProgram)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_program = new SqlCommand(@"UPDATE sample_program SET program_desc = '"+UpdateProgram.program_name+"', fund_id ="+UpdateProgram.fund_id+" where program_id="+UpdateProgram.program_id, con);

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
        public string DeleteProgram(int program_id)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_program = new SqlCommand(@"UPDATE tbl_R_BMSOfficePrograms SET ActionCode = 10 where OfficeProgramID=" + program_id.ToString(), con);

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
        public string RestoreProgram(int program_id)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_program = new SqlCommand(@"UPDATE tbl_R_BMSOfficePrograms SET ActionCode = 1 where OfficeProgramID=" + program_id.ToString(), con);

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