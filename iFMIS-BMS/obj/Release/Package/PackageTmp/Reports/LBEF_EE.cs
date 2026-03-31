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
    using System.Linq;
    using System.Configuration;
    using System.Data.SqlTypes;
    using iFMIS_BMS.Base;
    using iFMIS_BMS.Classes;


    /// <summary>
    /// Summary description for LBEF_EE.
    /// </summary>
    public partial class LBEF_EE : Telerik.Reporting.Report
    {
        public LBEF_EE(int? OfficeID, int? classtype, int? year, int? month_, int? batch, int? sort_, string note_, string ComputerIP)
        {
         
            InitializeComponent();



            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dtabs = new DataTable();
            DataTable inc_subs = new DataTable();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (sort_ == 0)
                {
                    SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_MonthlyRelease_LBEF  '" + OfficeID + "','" + year + "','" + month_ + "','" + batch + "'", con);
                    con.Open();
                    dt2.Load(com.ExecuteReader());
                }
                else
                {

                    SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_MonthlyRelease_LBEF_sort  '" + OfficeID + "','" + year + "','" + month_ + "','" + batch + "'", con);
                    con.Open();
                    dt2.Load(com.ExecuteReader());

                }

            }
            this.dt2.DataSource = dt2;
            //and '20' + SUBSTRING([OBRNo], 10, 2) = '" + year + "' 
            //            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            //            {
            //                SqlCommand com = new SqlCommand(@"select a.FundType, c.FunctionID ,CONCAT(c.OfficeName,' - ', d.ProgramDescription) as office_prog,CONCAT( e.AccountID ,'     ',  e.AccountName ) as account_id,
            //                                                  a.OOE_Name FROM IFMIS.dbo.tbl_T_BMSCurrentControl AS a 
            //                                                  inner join IFMIS.dbo.tbl_T_BMSSubsidiaryLedger as b
            //                                                  on a.OBRNo = b.OBRNo  and a.ProgramID = b.ProgramID 
            //                                                  inner join IFMIS.dbo.tbl_R_BMSOffices as c
            //                                                  on a.OfficeID = c.OfficeID
            //                                                  inner join IFMIS.dbo.tbl_R_BMSOfficePrograms as d
            //                                                  on a.ProgramID = d.ProgramID and a.OfficeID = d.OfficeID
            //                                                  inner join IFMIS.dbo.tbl_R_BMSProgramAccounts  as e
            //                                                  on a.ProgramID = e.ProgramID and b.FMISAccountCode = e.AccountID " +
            //                                                  "where a.OfficeID = '" + OfficeID + "' and a.ProgramID = '" + programID + "' and d.ProgramID = '" + programID + "' and b.FMISAccountCode = '" + accountID + "' " +
            //                                                  "and e.AccountID = '" + accountID + "'  and a.ActionCode = 1 and b.ActionCode = 1 and d.ActionCode = 1 and b.OOEID = '" + OOE_ID + "' " +
            //                                                  "group by  a.FundType, a.OOE_Name, c.FunctionID , c.OfficeName, d.ProgramDescription,e.AccountName,e.AccountID", con);
            //                con.Open();
            //                dt.Load(com.ExecuteReader());

            //            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@" select officename from IFMIS.dbo.tbl_R_BMSOffices where OfficeID = '" + OfficeID + "'", con);
                con.Open();
                txt_Office.Value = com.ExecuteScalar().ToString();

            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select class_type from IFMIS.dbo.tbl_R_BMS_A_Class where FundFlag = '" + classtype + "'", con);
                con.Open();
                txt_FundType.Value = com.ExecuteScalar().ToString() + " Proper";

            }

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_MonthlyRelease_LBEF_INC_SUBS  '" + OfficeID + "','" + year + "','" + month_ + "','" + batch + "'", con);
                con.Open();
                inc_subs.Load(com.ExecuteReader());

            }
            this.inc_subs.DataSource = inc_subs;

            DataTable _dt2 = new DataTable();
            string _sqlQuery2 = "Select [EmpNameFull],[Position] from[vwMergeAllEmployee] where eid = " + Account.UserInfo.eid + "";
            _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];

            textBox23.Value = _dt2.Rows[0][0].ToString();
            textBox34.Value = _dt2.Rows[0][1].ToString();

            txt_Today.Value = "Date Printed : " + DateTime.Now.ToString();
            txt_user.Value = Account.UserInfo.empName;

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DATENAME(month, DATEADD(month, '" + month_ + "'-1, CAST('2008-01-01' AS datetime)))", con);
                con.Open();
                txt_asOf.Value = "FOR THE MONTH OF " + com.ExecuteScalar().ToString();

            }
            if (note_ != "")
            {
                txt_notch.Value = note_;
            }
            else {
                textBox57.Visible = false;
                txt_notch.Value = "";
            }
        }
    }
}