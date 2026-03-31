using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.BusinessLayer.Models;
using System.Data.SqlClient;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class OthersLayer
    {
        public string AddNewOthers(string Others_Name, int? Others_Amount, int? Year)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("insert into dbo.tbl_R_BMSothers (other_name,other_amount,OfficeID,Year) values ('" + Others_Name + "'," + Others_Amount + "," + Account.UserInfo.Department + "," + Year + ")", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        public IEnumerable<OthersModel> Others(int? Year)
        {


            List<OthersModel> OthersList = new List<OthersModel>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT other_id,other_name,other_amount,OfficeID,Year FROM dbo.tbl_R_BMSothers where OfficeID = " + Account.UserInfo.Department + " and Year = '" + Year + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OthersModel Others = new OthersModel();
                    Others.other_id = reader.GetInt32(0);
                    Others.other_name = reader.GetValue(1).ToString();
                    Others.other_Amount = Convert.ToDouble(reader.GetValue(2));
                    Others.OfficeID = reader.GetInt32(3);
                    Others.Year = reader.GetInt32(4);

                    OthersList.Add(Others);
                }

            }
            return OthersList;
        }


        public OthersModel EditOthers(int? other_id)
        {
            OthersModel GrantsList = new OthersModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"SELECT other_id,other_name ,other_amount ,OfficeID ,Year FROM dbo.tbl_R_BMSothers where other_id=" + other_id + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    GrantsList.other_id = reader.GetInt32(0);
                    GrantsList.other_name = reader.GetValue(1).ToString();
                    GrantsList.other_Amount = Convert.ToDouble(reader.GetValue(2));
                    GrantsList.OfficeID = reader.GetInt32(3);
                    GrantsList.Year = reader.GetInt32(4);
                }
                return GrantsList;
            }
        }

        public string UpdateOthers(OthersModel oth)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"  update dbo.tbl_R_BMSothers set other_name = '" + oth.other_name + "', other_amount = " + oth.other_Amount + " where other_id = " + oth.other_id + "", con);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public IEnumerable<WFP> GetWFPDetail(int? office = 0, int? ooeid = 0, int? propyear = 0, int? qtr = 0)
        {
            //GRID//
            List<WFP> prog = new List<WFP>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_bms_WFP " + office + "," + ooeid + "," + propyear + "," + qtr + ",'','','','','','','','',0,0,0,0,0", con);
                con.Open();
                com.CommandTimeout = 0;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFP emp = new WFP();
                    emp.trnno = Convert.ToInt32(reader.GetValue(18));
                    emp.objectexpenditure = reader.GetValue(15).ToString();
                    emp.Appropriation = Convert.ToDouble(reader.GetValue(0));
                    emp.Reserved = Convert.ToDouble(reader.GetValue(1));
                    emp.Netprogram = Convert.ToDouble(reader.GetValue(5));
                    emp.Firstmonth = Convert.ToDouble(reader.GetValue(10));
                    emp.Secondmonth = Convert.ToDouble(reader.GetValue(11));
                    emp.Thirdmonth = Convert.ToDouble(reader.GetValue(12));
                    emp.MFO = reader.GetValue(21).ToString();
                    emp.PTFirstmonth = reader.GetValue(22).ToString();
                    emp.PTSecondmonth = reader.GetValue(23).ToString();
                    emp.PTThirdmonth = reader.GetValue(24).ToString();
                    emp.officeid = Convert.ToInt32(reader.GetValue(13));
                    emp.programid = Convert.ToInt32(reader.GetValue(19));
                    emp.accountid = Convert.ToInt64(reader.GetValue(14));
                    emp.officemfo= reader.GetValue(26).ToString();
                    emp.AppropriationNet = Convert.ToDouble(reader.GetValue(5));
                    emp.qtr = Convert.ToInt32(reader.GetValue(28));
                    emp.firstmonth_rel = Convert.ToDouble(reader.GetValue(29));
                    emp.secondmonth_rel = Convert.ToDouble(reader.GetValue(30));
                    emp.thirdmonth_rel = Convert.ToDouble(reader.GetValue(31));
                    emp.transno = Convert.ToInt64(reader.GetValue(32));
                    emp.MFOid = Convert.ToInt32(reader.GetValue(33));
                    prog.Add(emp);
                }

            }
            return prog;
        }

        public IEnumerable<ReportModel> GetAppProportionDetail(int? yearof = 0, int? accountid = 0)
        {
            //GRID//
            List<ReportModel> prog = new List<ReportModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_appropriationproportion "+ yearof + ","+ accountid + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ReportModel app = new ReportModel();
                    app.pmisofficeid = Convert.ToInt32(reader.GetValue(1));
                    app.OfficeName = reader.GetValue(2).ToString();
                    app.appropriation= Convert.ToDouble(reader.GetValue(6));
                    app.proportion = Convert.ToDouble(reader.GetValue(7));
                    app.trnno = Convert.ToInt32(reader.GetValue(8));
                
                    prog.Add(app);
                }

            }
            return prog;
        }

        public IEnumerable<ReportModel> GetAppProportionBreakdown(string offname = "", int? accountid = 0)
        {
            //GRID//
            List<ReportModel> prog = new List<ReportModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_bms_appropriationproportionbreakdown] '" + offname + "'," + accountid + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ReportModel app = new ReportModel();
                    app.trnno = Convert.ToInt32(reader.GetValue(0));
                    app.articlename = reader.GetValue(1).ToString();
                    app.enduser = reader.GetValue(2).ToString();
                
                    prog.Add(app);
                }

            }
            return prog;
        }

    }
}