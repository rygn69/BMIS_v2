using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.BusinessLayer.Models;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Models.Budget_Preparation;
using Kendo.Mvc.UI;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class Charts_layer
    {

        public IEnumerable<ChartModel> Statistics(int? OfficeID, int? ChartYear)
        {
           
            List<ChartModel> stats = new List<ChartModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"sp_chart_peroffice " + OfficeID.ToString() + "," + ChartYear.ToString() + ",1", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    ChartModel stat = new ChartModel();
                   
                    stat.perOfficePA = reader.GetValue(0).ToString() + " (₱" + Convert.ToDecimal(reader.GetValue(1)).ToString("N2") + ")";
                    stat.ProposedAmount = Convert.ToDecimal(reader.GetValue(1));
                    stat.Percentage = Convert.ToDecimal(reader.GetValue(2));
                    stat.naym = reader.GetValue(3).ToString();
                    stat.EmptySpace = reader.GetValue(4).ToString() + " (₱" + Convert.ToDecimal(reader.GetValue(1)).ToString("N2") + ")";
                    stat.Years = Convert.ToInt32(reader.GetValue(5));
                    //stat.amountLast = Convert.ToInt32(reader.GetValue(6));
                    //stat.amountLatest = Convert.ToInt32(reader.GetValue(7));
                    stats.Add(stat);
                }


            }
            return stats;
        }
                    


        public IEnumerable<ChartModel> StatisticsPA(int? OfficeID, int? ChartYear)
        {
            List<ChartModel> stats = new List<ChartModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"sp_chart_peroffice " + OfficeID.ToString() + "," + ChartYear.ToString() + ",1", con);




                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    ChartModel stat = new ChartModel();
                    stat.perOfficePA = reader.GetValue(0).ToString() + " (₱" + Convert.ToDecimal(reader.GetValue(1)).ToString("N2") + ")";
                    stat.ProposedAmount = Convert.ToDecimal(reader.GetValue(1));
                    stat.Percentage = Convert.ToDecimal(reader.GetValue(2));
                    stat.naym = reader.GetValue(3).ToString() ;
                    stat.EmptySpace = reader.GetValue(4).ToString() + " (₱" + Convert.ToDecimal(reader.GetValue(1)).ToString("N2") + ")";
                    stat.Years = Convert.ToInt32(reader.GetValue(5));
                    //stat.amountLast = Convert.ToInt32(reader.GetValue(6));
                    //stat.amountLatest = Convert.ToInt32(reader.GetValue(7));


                    stats.Add(stat);
                }


            }
            return stats;
        }

        public string getAbbrivation(int officeID)
        {
            var officeAb="";
            using (SqlConnection con = new SqlConnection(Common.MyConn())) {
                SqlCommand query_time = new SqlCommand(@"select OfficeAbbrivation from tbl_R_BMSOffices where officeID = "+officeID, con);
                con.Open();

                officeAb = query_time.ExecuteScalar().ToString();
                return officeAb;
            
            }
        }



        internal IEnumerable<ChartModel> BarCharts()
        {
            List<ChartModel> stats = new List<ChartModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"sp_BARchart_peroffice " + "4" + "," + "4" + ",1", con);




                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    ChartModel stat = new ChartModel();

                    stat.amountLast = Convert.ToInt32(reader.GetValue(0));
                    stat.amountLatest = Convert.ToInt32(reader.GetValue(1));
                    stats.Add(stat);
                }


            }
            return stats;
        }

        public IEnumerable<ChartModel> new_Statistics(int? OfficeID, int? ChartYear)
        {

            List<ChartModel> stats = new List<ChartModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_chart_peroffice_PIE " + OfficeID.ToString() + "," + ChartYear.ToString() + "", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    ChartModel stat = new ChartModel();

                    stat.name_type_mount = reader.GetValue(0).ToString() + " (₱" + Convert.ToDecimal(reader.GetValue(1)).ToString("N2") + ")";
                    stat.name_type = reader.GetValue(0).ToString();
                    stat.mounts = Convert.ToDouble(reader.GetValue(1));
                    stat.percentages_ = Convert.ToDecimal(reader.GetValue(2));

                    
                    stats.Add(stat);
                }


            }
            return stats;
        }
    }
}