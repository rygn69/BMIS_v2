using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class Home_Layer
    {

        public IEnumerable<ProgramsModel> Programss()
        {
            List<ProgramsModel> approvedbudget = new List<ProgramsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select * from program", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProgramsModel emp = new ProgramsModel();
                    emp.ProgramID = reader.GetValue(0).ToString();
                    emp.ProgramDescription = reader.GetString(1);


                    approvedbudget.Add(emp);
                }
            }
            return approvedbudget;
        }

    }
}