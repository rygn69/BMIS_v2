namespace iFMIS_BMS.Reports
{
    using iFMIS_BMS.BusinessLayer.Connector;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Configuration;
    using iFMIS_BMS.Base;
    using iFMIS_BMS.Classes;
    /// <summary>
    /// Summary description for LBP4New.
    /// </summary>
    public partial class LBP4New_Original : Telerik.Reporting.Report
    {
        //
        public static string GetOfficeDescription(int OfficeID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select top 1 OfficeName from tbl_R_BMSOffices where OfficeID = " + OfficeID + "", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string getDescription(int OfficeID, int TypeID, int YearOf)
        {
            var Result = "";
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select Description from tbl_R_BMSLBPForm4OtherData where Data_type = " + TypeID + " and ActionCode = 1 and OfficeID = " + OfficeID + " and YearOf = " + YearOf + " Order By OrderNo", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        if (Result == "")
                        {
                            Result = reader.GetValue(0).ToString();
                        }
                        else
                        {
                            Result = Result + Environment.NewLine + reader.GetValue(0).ToString();
                        }

                    }
                    return Result;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string getPreparedSignatory(int OfficeID, int Type) 
        {
            var PreparedSignatory = getSignatory(OfficeID);
            var ReturnValue = "";
            if (Type == 1)
            {
               ReturnValue =  PreparedSignatory.Split(':')[0];
            }
            else
            {
                ReturnValue = PreparedSignatory.Split(':')[1];
            }
            return ReturnValue;
        }
        public static string getTextBoxValue(int FieldID) 
        {
            return GlobalFunctions.getReportTextBoxValue(4, FieldID);
        }
        public static string getSignatory(int OfficeID) {
            var Query = "";
            try
            {
                if (OfficeID == 0)
	            {
		                Query = @"select top 1 Concat(UPPER(Firstname + ' ' + left(MI,1) + '. ' + LASTNAME),':', Position) from pmis.dbo.employee where eid = " + Account.UserInfo.eid + "";
	            }
                else
                {
                    Query = @"select Concat(UPPER(OfficeHead),':Department Head') FROM pmis.dbo.OfficeDescription WHERE OfficeID =
                                                      (select b.PMISOfficeID from dbo.tbl_R_BMSOffices as b where b.OfficeID = " + OfficeID + ") ";
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(Query, con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception)
            {
                return "No Signatory Found:No Signatory Found ";
            }
        }
        public LBP4New_Original(int OfficeID, int YearOf, int isNonOffice)
        {
           
            InitializeComponent();

            DataTable dt = new DataTable();

            using (SqlConnection constr = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT count(*) FROM [IFMIS].[dbo].[tbl_R_BMSLBPForm4OtherData] where officeid=" + OfficeID + " and yearof=" + YearOf + " and Data_Type in (1,2,3,4) and ActionCode=1", constr);
                constr.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    if (Convert.ToInt16(reader.GetValue(0).ToString()) >= 4)
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com2 = new SqlCommand(@"exec sp_BMS_LBP4 " + OfficeID + "," + YearOf + ",1", con);
                            con.Open();
                            dt.Load(com2.ExecuteReader());
                        }
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com2 = new SqlCommand(@"exec sp_BMS_LBP4 9999,9999,1", con);
                            con.Open();
                            dt.Load(com2.ExecuteReader());
                        }             
                     //   textBox83.Value = "Please fill out the office mandate, vision, mission and organizational outcome!";
                    }

                }
            }
            this.table1.DataSource = dt;
            this.ReportParameters["BudgetYear"].Value = YearOf;

            DataTable _dt2 = new DataTable();
            string _sqlQuery2 = "select * from fn_BMS_OfficeSignatory ("+ OfficeID  + ")";
            _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];
            textBox132.Value = _dt2.Rows[0][0].ToString();
            textBox133.Value = _dt2.Rows[0][1].ToString();

            DataTable _dt3 = new DataTable();
            string _sqlQuery3 = "select rtrim([OfficeHead]),[OfficeDesignation] from [pmis].[dbo].[OfficeDescription] where [OfficeID]=26";
            _dt3 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisqldb"].ToString(), CommandType.Text, _sqlQuery3).Tables[0];
            textBox46.Value = _dt3.Rows[0][0].ToString() + ", CPA, REA";
            textBox35.Value = "Provincial Treasurer";

            barcode1.Value = FUNCTION.GeneratePISControl();

            DataTable reportlog = new DataTable();
            using (SqlConnection conUser = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@" exec [sp_BMS_ReportLog] 14," + Account.UserInfo.eid + ",'LBPF4','" + barcode1.Value + "'", conUser);
                conUser.Open();
                reportlog.Load(com.ExecuteReader());

            }

        }
    }
}