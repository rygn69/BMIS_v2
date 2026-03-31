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
    public class addProgram_Layer
    {

        public IEnumerable<ProgramsModel> addProgram(int offices_ID)
        {
            List<ProgramsModel> pross = new List<ProgramsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select distinct ProgramDescription from tbl_R_BMSOfficePrograms ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProgramsModel app = new ProgramsModel();
                    
                    app.ProgramDescription = reader.GetString(0);

                    pross.Add(app);
                }
            }
            return pross;
        }


    }
}