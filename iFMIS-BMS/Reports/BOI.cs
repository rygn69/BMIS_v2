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
    /// <summary>
    /// Summary description for BOI.
    /// </summary>
    public partial class BOI : Telerik.Reporting.Report
    {
        public BOI(int? OfficeID, int? year, int? month_,  int? type_pgo)
        {
            
            InitializeComponent();

            DataTable dt = new DataTable();
            if (type_pgo == 1)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                { //Update on 1/24/2019 all add with actioncode=1
                    SqlCommand com = new SqlCommand(@"select c.OfficeID,a.OBRNo,b.TransTypeName,a.[Description]
                                                  ,a.Amount,c.OfficeName
                                                  ,CONCAT('Sub-Total 'COLLATE Latin1_General_CI_AS
                                                  , c.OfficeAbbrivation COLLATE Latin1_General_CI_AS ) as OfficeAbbrivation
                                                  ,a.OOE_Name,d.OOEID, CONCAT('Sub-Total ' COLLATE Latin1_General_CI_AS
                                                  ,d.OOEAbrevation COLLATE Latin1_General_CI_AS) as OOEAbrevation 
                                                  FROM IFMIS.dbo.tbl_T_BMSCurrentControl as a
                                                  left join IFMIS.dbo.tbl_R_BMSTransType as b 
                                                  on a.TransTypeID = b.TransTypeID
                                                  left join IFMIS.dbo.tbl_R_BMSOffices as c
                                                  on a.OfficeID = c.OfficeID 
                                                  left join IFMIS.dbo.tbl_R_BMSObjectOfExpenditure as d
                                                  on a.OOE_Name = d.OOEName " +
                                                     " WHERE  case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201' " +
                                                      "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101'  " +
                                                     " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119'" +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127' " +
                                                     " then cast('20' + SUBSTRING([OBRNo], 10, 2) as varchar ) " +
                                                     " else cast('20' + SUBSTRING([OBRNo], 1, 2) as varchar ) " +
                                                      "end like '" + year + "' and case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127' " +
                                                      "then  cast(SUBSTRING([OBRNo], 13, 2) as int ) " +
                                                      "else  cast(SUBSTRING([OBRNo], 3, 2) as int ) " +
                                                      "end = '" + month_ + "' and LEN(a.OBRNo) != 16 and a.actioncode=1" +
                                                      " order by d.OOEID", con);
                    con.Open();
                    dt.Load(com.ExecuteReader());

                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select c.OfficeID,a.OBRNo,b.TransTypeName,a.[Description]
                                                  ,a.Amount,c.OfficeName
                                                  ,CONCAT('Sub-Total 'COLLATE Latin1_General_CI_AS
                                                  , c.OfficeAbbrivation COLLATE Latin1_General_CI_AS ) as OfficeAbbrivation
                                                  ,a.OOE_Name,d.OOEID, CONCAT('Sub-Total ' COLLATE Latin1_General_CI_AS
                                                  ,d.OOEAbrevation COLLATE Latin1_General_CI_AS) as OOEAbrevation 
                                                  FROM IFMIS.dbo.tbl_T_BMSCurrentControl as a
                                                  left join IFMIS.dbo.tbl_R_BMSTransType as b 
                                                  on a.TransTypeID = b.TransTypeID
                                                  left join IFMIS.dbo.tbl_R_BMSOffices as c
                                                  on a.OfficeID = c.OfficeID 
                                                  left join IFMIS.dbo.tbl_R_BMSObjectOfExpenditure as d
                                                  on a.OOE_Name = d.OOEName " +
                                                     " WHERE  case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201' " +
                                                      "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101'  " +
                                                     " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119'" +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127' " +
                                                     " then cast('20' + SUBSTRING([OBRNo], 10, 2) as varchar ) " +
                                                     " else cast('20' + SUBSTRING([OBRNo], 1, 2) as varchar ) " +
                                                      "end like '" + year + "' and case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127' " +
                                                      "then  cast(SUBSTRING([OBRNo], 13, 2) as int ) " +
                                                      "else  cast(SUBSTRING([OBRNo], 3, 2) as int ) " +
                                                      "end = '" + month_ + "' and a.actioncode=1" +
                                                      " order by d.OOEID", con);
                    con.Open();
                    dt.Load(com.ExecuteReader());

                }
            }


            if (type_pgo == 1)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@" select sum(a.Amount) as Amount
                                                      FROM IFMIS.dbo.tbl_T_BMSCurrentControl as a
                                                      left join IFMIS.dbo.tbl_R_BMSTransType as b 
                                                      on a.TransTypeID = b.TransTypeID 
                                                      left join IFMIS.dbo.tbl_R_BMSOffices as c
                                                      on a.OfficeID = c.OfficeID 
                                                      left join IFMIS.dbo.tbl_R_BMSObjectOfExpenditure as d
                                                       on a.OOE_Name = d.OOEName " +
                                                           " WHERE  case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201' " +
                                                      "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101'  " +
                                                     " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119'" +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127' " +
                                                     " then cast('20' + SUBSTRING([OBRNo], 10, 2) as varchar ) " +
                                                     " else cast('20' + SUBSTRING([OBRNo], 1, 2) as varchar ) " +
                                                      "end like '" + year + "' and case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127' " +
                                                      "then  cast(SUBSTRING([OBRNo], 13, 2) as int ) " +
                                                      "else  cast(SUBSTRING([OBRNo], 3, 2) as int ) " +
                                                      "end = '" + month_ + "' and LEN(a.OBRNo) != 16 and a.actioncode=1", con);
                    con.Open();
                    txt_todaydate.Value = "Date Printed : " + com.ExecuteScalar().ToString();

                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@" select sum(a.Amount) as Amount
                                                      FROM IFMIS.dbo.tbl_T_BMSCurrentControl as a
                                                      left join IFMIS.dbo.tbl_R_BMSTransType as b 
                                                      on a.TransTypeID = b.TransTypeID 
                                                      left join IFMIS.dbo.tbl_R_BMSOffices as c
                                                      on a.OfficeID = c.OfficeID 
                                                      left join IFMIS.dbo.tbl_R_BMSObjectOfExpenditure as d
                                                       on a.OOE_Name = d.OOEName " +
                                                           " WHERE  case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201' " +
                                                      "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101'  " +
                                                     " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119'" +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127' " +
                                                     " then cast('20' + SUBSTRING([OBRNo], 10, 2) as varchar ) " +
                                                     " else cast('20' + SUBSTRING([OBRNo], 1, 2) as varchar ) " +
                                                      "end like '" + year + "' and case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118' " +
                                                      " or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127' " +
                                                      "then  cast(SUBSTRING([OBRNo], 13, 2) as int ) " +
                                                      "else  cast(SUBSTRING([OBRNo], 3, 2) as int ) " +
                                                      "end = '" + month_ + "' and a.actioncode=1", con);
                    con.Open();
                    txt_todaydate.Value = "Date Printed : " + com.ExecuteScalar().ToString();

                }
            }
           


            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DATENAME(month, DATEADD(month, '" + month_ + "'-1, CAST('2008-01-01' AS datetime)))", con);
                con.Open();
                txt_asOf.Value = "For The Month Of " + com.ExecuteScalar().ToString();

            }

            this.table1.DataSource = dt;

            DataTable _dt4 = new DataTable();
            string _sqlQuery4 = "SELECT [lgu],[province],isnull([address],'') address FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1";
            _dt4 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery4).Tables[0];
            if (_dt4.Rows.Count > 0)
            {
                textBox13.Value = _dt4.Rows[0][0].ToString();
                textBox19.Value = _dt4.Rows[0][2].ToString();
                textBox40.Value = _dt4.Rows[0][0].ToString() + " - Making your task easier...";
            }

            txt_todaydate.Value = "Date Printed : " + DateTime.Now.ToString();
            txt_user.Value = "Printed by : " + Account.UserInfo.empName;

        }
    }
}