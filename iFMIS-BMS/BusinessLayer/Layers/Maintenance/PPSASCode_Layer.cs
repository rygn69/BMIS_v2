using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class PPSASCode_Layer
    {
        public IEnumerable<PPSASCode_Model> PPSASACode_list(int? Year)
        {
            List<PPSASCode_Model> data = new List<PPSASCode_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"SELECT * FROM dbo.fn_BMS_GridPPSASCode(" + Year + ") ORDER BY x", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PPSASCode_Model value = new PPSASCode_Model();
                    value.FMISAccountCode = Convert.ToInt32(reader.GetValue(0));
                    value.ChildAccountCode = Convert.ToString(reader.GetValue(1));
                    value.AccountName = Convert.ToString(reader.GetValue(2));
                    value.PPAChildPPSASCode = Convert.ToString(reader.GetValue(3));
                    value.PPSASCode = Convert.ToString(reader.GetValue(4));
                    value.PPASeriesCode = Convert.ToString(reader.GetValue(5));
                    data.Add(value);
                }
            }
            return data;
        }

        public string UpdatePPSASCode(int? AccountCode, string PPSASCode, int? TransactionYear)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var query = @"dbo.sp_BMS_UpdatePPSASCode " + AccountCode + ", '" + PPSASCode + "', " + TransactionYear + ", '"+Account.UserInfo.eid+"'";
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                data = Convert.ToString(com.ExecuteScalar());
            }
            return data;
        }
        public string SetPPSASCode(string SetPPSASCode)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var query = @"dbo.sp_BMS_SearchPPSASCode " + SetPPSASCode + "";
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                data = Convert.ToString(com.ExecuteScalar());
            }
            return data;
        }
        public string SavePPSASCode(string PPSASSeries, string PPSASCode, string ChildPPSASCode, int? FMISAccount, int? YearOf)
        {
            var data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var query = @"dbo.sp_BMS_UpdatePPSASCode '" + PPSASSeries + "', '" + PPSASCode + "', '" + ChildPPSASCode + "', " + FMISAccount + ", "+YearOf+", "+Account.UserInfo.eid+"";
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                data = Convert.ToString(com.ExecuteScalar());
            }
            return data;
        }
    }
}