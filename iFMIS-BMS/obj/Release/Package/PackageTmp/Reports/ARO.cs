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
    /// Summary description for LBEF.
    /// </summary>
    public partial class ARO : Telerik.Reporting.Report
    {
        public ARO(int? OfficeID, int? classtype, int? year, int? month_, int? batch, int? sort_, string note_,int? expclass,string purposeid,int? Fundtype,int? packet,int? reporthistory, string ComputerIP,int? is_float,int? budgettype,string dateissue)
        {
            
            InitializeComponent();


            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dtabs = new DataTable();

            bcode.Value = FUNCTION.GeneratePISControl();
            //bcode.Value = "RDMDKCTK";
            GlobalFunctions.QR_globalstr = bcode.Value;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                
                //if (sort_ == 0)
                //{
                    if (reporthistory != 0)
                    {
                        SqlCommand com = new SqlCommand(@"exec [sp_BMS_AROreadxml] "+ reporthistory +"", con);
                        con.Open();
                        dt2.Load(com.ExecuteReader());
                    }
                    else
                    {
                        if (sort_ == 0)
                        {
                            SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_MonthlyRelease_ARO  '" + OfficeID + "','" + year + "','" + month_ + "','" + batch + "'," + expclass + ",'" + note_ + "','" + purposeid + "'," + Fundtype + "," + Account.UserInfo.eid + "," + packet + ",'" + bcode.Value + "',"+ is_float + ","+ budgettype + ",'"+ dateissue  + "'", con);
                            con.Open();
                            dt2.Load(com.ExecuteReader());
                        }
                        else
                        {
                            SqlCommand com = new SqlCommand(@"IFMIS.dbo.[sp_MonthlyRelease_ARO_details]  '" + OfficeID + "','" + year + "','" + month_ + "','" + batch + "'," + expclass + ",'" + note_ + "','" + purposeid + "'," + Fundtype + "," + Account.UserInfo.eid + "," + packet + ",'" + bcode.Value + "',"+ is_float + "," + budgettype + ",'" + dateissue + "'", con);
                            con.Open();
                            dt2.Load(com.ExecuteReader());
                        }
                    }
                //}
                //else {

                //    SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_MonthlyRelease_LBEF_sort  '" + OfficeID + "','" + year + "','" + month_ + "','" + batch + "'", con);
                //    con.Open();
                //    dt2.Load(com.ExecuteReader());
                
                //}
            }
            this.table1.DataSource = dt2;

            if (reporthistory != 0) {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    DataTable _dt4 = new DataTable();
                    var isfloat = 0;
                    var budgetype = 0;
                    string _sqlQuery4 = "SELECT arono,format(cast(dateissued as date),'M/d/yyyy'),[recommendedby],[approvedby],[note],[purpose],[ooeid],isnull([isfloat],0) isfloat,[budgetype] FROM  [IFMIS].[dbo].[ ] where aro_id=" + reporthistory + "";
                    _dt4 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery4).Tables[0];

                    textBox8.Value = _dt4.Rows[0][0].ToString();//ARO no.
                    textBox15.Value= _dt4.Rows[0][1].ToString();
                    textBox35.Value = _dt4.Rows[0][2].ToString();
                    textBox55.Value = _dt4.Rows[0][3].ToString();
                    textBox1.Value = _dt4.Rows[0][4].ToString();
                    txt_purpose.Value= _dt4.Rows[0][5].ToString();
                    tempBox.Value = _dt4.Rows[0][6].ToString();
                    isfloat = Convert.ToInt16(_dt4.Rows[0][7].ToString());
                    budgetype= Convert.ToInt16(_dt4.Rows[0][8].ToString());
                    if (budgetype == 1) {
                        checkBox1.FalseValue = "1";
                        checkBox2.FalseValue = "";
                        checkBox3.FalseValue = "";
                    }
                    else if (budgetype == 2) {
                        checkBox1.FalseValue = "";
                        checkBox2.FalseValue = "1";
                        checkBox3.FalseValue = "";
                    }
                    else
                    {
                        checkBox1.FalseValue = "";
                        checkBox2.FalseValue = "";
                        checkBox3.FalseValue = "1";
                    }
                    if (isfloat == 0)
                    {
                        if (tempBox.Value == "0")
                        {
                            textBox2.Value = "ALLOTMENT RELEASE ORDER";
                            textBox27.Value = "AUTHORIZED APPROPRIATION (in Pesos)";
                        }
                        else if (tempBox.Value == "1")
                        {
                            textBox2.Value = "ALLOTMENT RELEASE ORDER FOR PERSONAL SERVICES";
                            textBox27.Value = "AUTHORIZED APPROPRIATION(PS in Pesos)";
                        }
                        else if (tempBox.Value == "2")
                        {
                            textBox2.Value = "ALLOTMENT RELEASE ORDER FOR MAINTENANCE AND OTHER OPERATING EXPENSE";
                            textBox27.Value = "AUTHORIZED APPROPRIATION(MOOE in Pesos)";
                        }
                        else if (tempBox.Value == "3")
                        {
                            textBox2.Value = "ALLOTMENT RELEASE ORDER FOR CAPITAL OUTLAY";
                            textBox27.Value = "AUTHORIZED APPROPRIATION(CO in Pesos)";
                        }
                    }
                    else
                    {
                        if (tempBox.Value == "0")
                        {
                            textBox2.Value = "ALLOTMENT RELEASE ORDER(FLOAT)";
                            textBox27.Value = "AUTHORIZED APPROPRIATION (in Pesos)";
                        }
                        else if (tempBox.Value == "1")
                        {
                            textBox2.Value = "ALLOTMENT RELEASE ORDER FOR PERSONAL SERVICES(FLOAT)";
                            textBox27.Value = "AUTHORIZED APPROPRIATION(PS in Pesos)";
                        }
                        else if (tempBox.Value == "2")
                        {
                            textBox2.Value = "ALLOTMENT RELEASE ORDER FOR MAINTENANCE AND OTHER OPERATING EXPENSE(FLOAT)";
                            textBox27.Value = "AUTHORIZED APPROPRIATION(MOOE in Pesos)";
                        }
                        else if (tempBox.Value == "3")
                        {
                            textBox2.Value = "ALLOTMENT RELEASE ORDER FOR CAPITAL OUTLAY(FLOAT)";
                            textBox27.Value = "AUTHORIZED APPROPRIATION(CO in Pesos)";
                        }
                    }
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"exec sp_MonthlyRelease_ARONo " + year + "," + OfficeID + " ,'" + bcode.Value + "'", con);
                    con.Open();
                    textBox8.Value = com.ExecuteScalar().ToString();

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    textBox15.Value = dateissue;
                   // textBox15.Value = "09/23/2025";//dateissue;

                }
                if (budgettype == 1)
                {
                    checkBox1.FalseValue = "1";
                    checkBox2.FalseValue = "";
                    checkBox3.FalseValue = "";
                }
                else if (budgettype == 2)
                {
                    checkBox1.FalseValue = "";
                    checkBox2.FalseValue = "1";
                    checkBox3.FalseValue = "";
                }
                else
                {
                    checkBox1.FalseValue = "";
                    checkBox2.FalseValue = "";
                    checkBox3.FalseValue = "1";
                }
                if (is_float == 0)
                {
                    if (expclass == 0)
                    {
                        textBox2.Value = "ALLOTMENT RELEASE ORDER";
                        textBox27.Value = "AUTHORIZED APPROPRIATION (in Pesos)";
                    }
                    else if (expclass == 1)
                    {
                        textBox2.Value = "ALLOTMENT RELEASE ORDER FOR PERSONAL SERVICE";
                        textBox27.Value = "AUTHORIZED APPROPRIATION(PS in Pesos)";
                    }
                    else if (expclass == 2)
                    {
                        textBox2.Value = "ALLOTMENT RELEASE ORDER FOR MAINTENANCE AND OTHER OPERATING EXPENSE";
                        textBox27.Value = "AUTHORIZED APPROPRIATION(MOOE in Pesos)";
                    }
                    else if (expclass == 3)
                    {
                        textBox2.Value = "ALLOTMENT RELEASE ORDER FOR CAPITAL OUTLAY";
                        textBox27.Value = "AUTHORIZED APPROPRIATION(CO in Pesos)";
                    }
                }
                else
                {
                    if (expclass == 0)
                    {
                        textBox2.Value = "ALLOTMENT RELEASE ORDER(FLOAT)";
                        textBox27.Value = "AUTHORIZED APPROPRIATION (in Pesos)";
                    }
                    else if (expclass == 1)
                    {
                        textBox2.Value = "ALLOTMENT RELEASE ORDER FOR PERSONAL SERVICES(FLOAT)";
                        textBox27.Value = "AUTHORIZED APPROPRIATION(PS in Pesos)";
                    }
                    else if (expclass == 2)
                    {
                        textBox2.Value = "ALLOTMENT RELEASE ORDER FOR MAINTENANCE AND OTHER OPERATING EXPENSE(FLOAT)";
                        textBox27.Value = "AUTHORIZED APPROPRIATION(MOOE in Pesos)";
                    }
                    else if (expclass == 3)
                    {
                        textBox2.Value = "ALLOTMENT RELEASE ORDER FOR CAPITAL OUTLAY(FLOAT)";
                        textBox27.Value = "AUTHORIZED APPROPRIATION(CO in Pesos)";
                    }
                }

            }
            

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@" select officename from IFMIS.dbo.tbl_R_BMSOffices where OfficeID = '" + OfficeID + "'", con);
                con.Open();
                txt_Office.Value = com.ExecuteScalar().ToString();

            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select case when [FundFlag]=2 then  101 when [FundFlag]=3 then 119 when [FundFlag]=1 then 201 when [FundFlag]=0 then 118 end [FundFlag] from IFMIS.dbo.tbl_R_BMS_A_Class where [FundFlag] = '" + classtype + "'", con);
                con.Open();
                txt_FundType.Value = com.ExecuteScalar().ToString() ; 

            }

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (sort_ == 0)
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_MonthlyRelease_AROTotal] '" + OfficeID + "','" + year + "','" + month_ + "','" + batch + "'," + expclass + "," + reporthistory + ","+ is_float + "", con);
                    con.Open();
                    textBox17.Value = com.ExecuteScalar().ToString();
                }
                else
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_MonthlyRelease_AROTotal_details] '" + OfficeID + "','" + year + "','" + month_ + "','" + batch + "'," + expclass + "," + reporthistory + "," + is_float + "", con);
                    con.Open();
                    textBox17.Value = com.ExecuteScalar().ToString();
                }

            }
            
            textBox24.Value =Convert.ToString(year);//"FOR THE MONTH OF " + com.ExecuteScalar().ToString();
           
            //}
            if (note_ != "" || purposeid != "")
            {
                textBox1.Value = note_;// == ""? "                            " : note_;
                txt_purpose.Value = purposeid;
            }

            //else
            //{
            //    textBox57.Visible = false;
            //   // txt_notch.Value = "";
            //}
            if (OfficeID == 14 || OfficeID == 49 || OfficeID == 57)
            {
                textBox55.Value = "PATRICIA ANNE B. PLAZA";
                textBox56.Value = "Vice Governor";
               // textBox56.Value = "Acting Governor";
            }
            else
            {
                textBox55.Value = "SANTIAGO B. CANE, JR.";
                textBox56.Value = "Governor";
                //textBox55.Value = "PATRICIA ANNE B. PLAZA";
                //textBox56.Value = "Acting Governor";
            }

            DataTable prepsig = new DataTable();
            string _sqlQuery_prep = "select top 1 [usereid],b.EmpNameFull,b.Position from [IFMIS].[dbo].[tbl_T_BMSARO_xml] as a inner join pmis.dbo.vwMergeAllEmployee_Modified as b on b.eid=a.usereid  where  [arono] ='"+ textBox8.Value + "'";
            prepsig = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery_prep).Tables[0];
            if (prepsig.Rows.Count > 0)
            {
                textBox61.Value = prepsig.Rows[0][1].ToString();
                textBox50.Value = prepsig.Rows[0][2].ToString();
            }

            DataTable _lgusig = new DataTable();
            string _sqlQuery_lgu = "SELECT [lgu],[province],isnull([address],'') address,[budgetofficer],[position],lgu_head,lgu_headposition,tagline FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1";
            _lgusig = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery_lgu).Tables[0];
            if (_lgusig.Rows.Count > 0)
            {
                textBox55.Value= _lgusig.Rows[0][5].ToString();
                textBox56.Value= _lgusig.Rows[0][6].ToString();
                textBox35.Value = _lgusig.Rows[0][3].ToString();
                textBox36.Value = _lgusig.Rows[0][4].ToString();
                textBox10.Value= _lgusig.Rows[0][0].ToString();
                textBox48.Value = _lgusig.Rows[0][0].ToString() +  " " + _lgusig.Rows[0][7].ToString();
            }
        }
    }
}