using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class ddProgramsesLayer
    {
        public IEnumerable<ProgramsModel> gPrograms(int? propYear, int? office_ID)
        {
            List<ProgramsModel> pross = new List<ProgramsModel>();
            if (propYear >= 2017)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("select Distinct ProgramID,ProgramDescription from tbl_R_BMSOfficePrograms where ProgramYear = '" + propYear + "' and OfficeID = '" + office_ID + "' and actioncode = 1 order by ProgramDescription", con);
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
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("select Distinct [FmisProgramCode] aS ProgramID,ProgramDescription from [fmis].[dbo].[tblRefBMS_BudgetProgram] where [YearOf] = '" + propYear + "' and [FmisOfficeCode] = '" + office_ID + "' and actioncode = 1 order by [ProgramDescription]", con);
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
            }
            return pross;
        }
        public IEnumerable<ProgramsModel> gProgramsrealign(int? propYear, int? office_ID, int? program_from)
        {
            List<ProgramsModel> pross = new List<ProgramsModel>();
            if (Account.UserInfo.UserTypeID == 4) //system admin
            {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("select Distinct ProgramID,ProgramDescription from tbl_R_BMSOfficePrograms where ProgramYear = '" + propYear + "' and OfficeID = '" + office_ID + "' and actioncode = 1 order by ProgramDescription", con);
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
            }
            else
            {
                // List<ProgramsModel> pross = new List<ProgramsModel>();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("select Distinct ProgramID,ProgramDescription from tbl_R_BMSOfficePrograms where ProgramYear = '" + propYear + "' and OfficeID = '" + office_ID + "' and ProgramID=" + program_from + " and actioncode = 1 order by ProgramDescription", con);
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
            }
            return pross;
        }

        public IEnumerable<ProgramsModel> gProgramsreversion(int? propYear, int? office_ID, int? program_from)
        {
            List<ProgramsModel> pross = new List<ProgramsModel>();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("select Distinct ProgramID,ProgramDescription from tbl_R_BMSOfficePrograms where ProgramYear = '" + propYear + "' and OfficeID = '" + office_ID + "' and actioncode = 1 order by ProgramDescription", con);
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