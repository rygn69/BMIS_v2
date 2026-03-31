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
    public class GrantsLayer
    {

        public string AddNewGrants(string Grants_Name, int? Grants_Amount, int? Year)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("insert into dbo.tbl_R_BMSgrants (grants_name,grants_amount,OfficeID,Year) values ('"+Grants_Name+"',"+Grants_Amount+","+Account.UserInfo.Department+","+Year+")", con);
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

        public IEnumerable<GrantsModel> Grants(int? Year)
        {

           
            List<GrantsModel> GrantsList = new List<GrantsModel>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT grants_id, grants_name, grants_amount, OfficeID, Year FROM dbo.tbl_R_BMSgrants where OfficeID = "+Account.UserInfo.Department+" and Year = '"+Year+"'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    GrantsModel Grants = new GrantsModel();
                    Grants.grants_id = reader.GetInt32(0);
                    Grants.grants_name = reader.GetValue(1).ToString();
                    Grants.grants_Amount = Convert.ToDouble(reader.GetValue(2));
                    Grants.OfficeID = reader.GetInt32(3);
                    Grants.Year = reader.GetInt32(4);

                    GrantsList.Add(Grants);
                }

            }
            return GrantsList;
        }

        public GrantsModel EditGrants(int? grants_id)
        {
            GrantsModel GrantsList = new GrantsModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"SELECT grants_id, grants_name, grants_amount, OfficeID, Year FROM dbo.tbl_R_BMSgrants where grants_id=" + grants_id + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    GrantsList.grants_id = reader.GetInt32(0);
                    GrantsList.grants_name = reader.GetValue(1).ToString();
                    GrantsList.grants_Amount = Convert.ToDouble(reader.GetValue(2));
                    GrantsList.OfficeID = reader.GetInt32(3);
                    GrantsList.Year = reader.GetInt32(4);
                }
                return GrantsList;
            }
        }

        public string UpdateGrants(GrantsModel gra)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"  update dbo.tbl_R_BMSgrants set grants_name='"+gra.grants_name+"', grants_amount = "+gra.grants_Amount+" where grants_id = "+gra.grants_id+"", con);
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




       


    }
}