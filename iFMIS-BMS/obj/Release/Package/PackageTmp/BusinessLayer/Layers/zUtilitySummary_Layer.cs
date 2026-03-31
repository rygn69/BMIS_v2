using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
using System.Data.SqlClient;
using System.IO;
using iFMIS_BMS.FMIS;
using System.Data;


namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class zUtilitySummary_Layer
    {
        public double GETPS(int office_id, int year_of)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_SummaryView_PS " + office_id + "," + year_of + "", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }

        public double GETMOOE(int office_id, int year_of)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_SummaryView_MOOE " + office_id + "," + year_of + "", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }

        public double GETCO(int office_id, int year_of)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_SummaryView_CO " + office_id + "," + year_of + "", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }

        public IEnumerable<summary_model> readSummary(int? office_id, int? year_of)
        {


            List<summary_model> Summary_list = new List<summary_model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_SummaryView_Num_Employees " + office_id + "," + year_of + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    summary_model summary = new summary_model();
                    //summary.summary_id = reader.GetInt32(0);
                    summary.summary_group = reader.GetValue(0).ToString();
                    summary.summary_desc = reader.GetValue(1).ToString();
                    summary.summary_count = reader.GetInt32(2);

                    Summary_list.Add(summary);
                }

            }
            return Summary_list;
        }


        public string GETOPIS(int? office_id)
        {


            var officeAb = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_time = new SqlCommand(@"select CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where OfficeID = " + office_id + "", con);
                con.Open();

                officeAb = query_time.ExecuteScalar().ToString();
                return officeAb;

            }
        }

    }
}