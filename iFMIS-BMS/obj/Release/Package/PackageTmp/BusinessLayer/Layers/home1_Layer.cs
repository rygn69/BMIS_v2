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
    public class home1_Layer
    {
        public IEnumerable<ProgramsModel> gPrograms( int? propYear1)
        {
            List<ProgramsModel> pross = new List<ProgramsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select ProgramID,ProgramDescription from tbl_R_BMSOfficePrograms where ProgramYear = '" + propYear1 + "' and OfficeID = '" + Account.UserInfo.Department.ToString() + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProgramsModel app = new ProgramsModel();
                    app.ProgramID = reader.GetValue(0).ToString();
                    app.ProgramDescription = reader.GetString(1);

                    pross.Add(app);
                }
            }
            return pross;
        }
    }
}