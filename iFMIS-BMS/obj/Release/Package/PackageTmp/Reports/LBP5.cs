namespace iFMIS_BMS.Reports
{
    using iFMIS_BMS.BusinessLayer.Connector;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for LBP5.
    /// </summary>
    /// 

    public partial class LBP5 : Telerik.Reporting.Report
    {
        public static string OfficeNameReport, ReportID;
        public static int OfficeID, Year;
        public static string OfficeHead, Designation;
        public static string OBJDESC(int j)
        {
            
            var td2 = "";
            var td_data = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                string SL = "SELECT OBJ_ORDERBY , OBJ_Description FROM tbl_R_LBP5_Objectives WHERE OfficeID = '" + OfficeID + "' and TransactionYear = '" + Year + "' and ActionCode = 1";

                SL="select * from( " +
                        " select row_number() over(order by OBJ_ORDERBY)as rw,OBJ_ORDERBY,Replace(OBJ_Description,'&','&amp;') as OBJ_Description from vw_Objectives " +
                        " where OfficeID = '"+OfficeID+"' and TransactionYear = '"+Year+"' and ActionCode = 1)t where  t.rw "+ (j== 0 ? "<=" : ">") + "(select count(*) from vw_Objectives " +
                        " where OfficeID = '"+OfficeID+"' and TransactionYear = '"+Year+"' and ActionCode = 1)/2";

                SqlCommand data = new SqlCommand(SL, con);
                con.Open();
                SqlDataReader reader = data.ExecuteReader();
                            while (reader.Read())
                            {
                                var order = Convert.ToInt32(reader.GetValue(0));
                                var desc = reader.GetString(2);
                                td_data = order + ". " + desc + "<br />";
                                td2 = td2 + td_data;
                               
                            }
                if(td2 == null){
                    td2 = "NO DATA AVAILABLE";
                }
            }
            return td2;
        }
        public static string FSDesc()
        {
            var FS_DESC = "";
            var td = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT FS_OrderBy, Replace(FS_Description,'&','&amp;') as FS_Description FROM tbl_R_LBP5_FS WHERE OfficeID = '" + OfficeID + "' and TransactionYear = '" + Year + "' and ActionCode = 1 ORDER BY FS_OrderBy, FS_Description asc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    var order = Convert.ToInt32(reader.GetValue(0));
                    var desc = reader.GetString(1);
                    td = order + ". " + desc + "<br />";
                    FS_DESC = FS_DESC + td;
                }

            }
            return FS_DESC;
        }

        public LBP5(int yearParam, int OfficeIDParam, string OfficeNameParam,string ComputerIP)
        {
            Year = yearParam;
            OfficeID = OfficeIDParam;
            InitializeComponent();
            txtOfficeName.Value = OfficeNameParam;
            var query = @"exec sp_BMS_LBP5 "+ OfficeIDParam +","+ yearParam + "";
            DataTable dt = new DataTable();
            dt = query.GetDataTable(Common.MyConn());
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr[2] = 0;
                dr[6] = 0;
                dt.Rows.Add(dr);
            }
            this.DataSource = dt;

            var BudgetOfficeID = 21;
            var GovernorsOfficeID = 1;
            var PrintedBy = "";
            var PrintedByPosition = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com2 = new SqlCommand(@"select top 1 UPPER(Firstname + ' ' + left(MI,1) + '. ' + LASTNAME), Position from pmis.dbo.employee where eid = " + Account.UserInfo.eid + "", con);
                con.Open();
                SqlDataReader reader = com2.ExecuteReader();
                reader.Read();
                PrintedBy = reader.GetValue(0).ToString();
                PrintedByPosition = reader.GetValue(1).ToString();
            }
            txtOfficeHead.Value = GlobalFunctions.getOfficeHead(OfficeID);
            txtOfficeHeadDesignation.Value = GlobalFunctions.getOfficeHeadDesignation(OfficeID);
            txtBudgetHead.Value = GlobalFunctions.getOfficeHead(BudgetOfficeID);
            txtBudgetHeadDesignation.Value = GlobalFunctions.getOfficeHeadDesignation(BudgetOfficeID);
            txtGovernor.Value = GlobalFunctions.getOfficeHead(GovernorsOfficeID);
            txtGovernorDesignation.Value = GlobalFunctions.getOfficeHeadDesignation(GovernorsOfficeID);
          //  QRCode.Value = GlobalFunctions.QRCodeValue(PrintedBy, ComputerIP);

        }

        public static string MyFormat(decimal num)
        {
            
            string format = "₱ {0:N2}";
            return string.Format(format, Math.Abs(num));

        }
        public static string ReportYear()
        {
            var YearString = Year.ToString();
            return YearString;
        }
    }
}