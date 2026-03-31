using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class dpOfficeLayer
    {

        public IEnumerable<OfficesModel> Offices()
        {
            List<OfficesModel> oppice = new List<OfficesModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select * from tbl_R_BMSOffices", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OfficesModel app = new OfficesModel();
                    app.OfficeID = reader.GetValue(1).ToString();
                    app.OfficeName = reader.GetValue(2).ToString();

                    oppice.Add(app);
                }
            }
            return oppice;
        }


        public IEnumerable<OfficesModel> Offices2()
        {
            List<OfficesModel> oppice = new List<OfficesModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select * from tbl_R_BMSOffices where OfficeID = '" + Account.UserInfo.Department.ToString() + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OfficesModel app = new OfficesModel();
                    app.OfficeID = reader.GetValue(1).ToString();
                    app.OfficeName = reader.GetValue(2).ToString();

                    oppice.Add(app);
                }
            }
            return oppice;
        }


    }
}