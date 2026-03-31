using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;


namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class dpPrograms_Layer
    {

        public IEnumerable<ProgramsModel> Appointments(int? propYear)
        {
            List<ProgramsModel> appointments = new List<ProgramsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand("select DISTINCT ProgramID,ProgramDescription from tbl_R_BMSOfficePrograms where ProgramYear = '" + propYear + "' and OfficeID ='" + Account.UserInfo.Department.ToString() + "' ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProgramsModel app = new ProgramsModel();
                    app.ProgramID = reader.GetValue(0).ToString();
                    app.ProgramDescription = reader.GetString(1);

                    appointments.Add(app);
                }
            }
            return appointments;
        }

    }
}