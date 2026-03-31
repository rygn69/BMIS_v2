using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.PPMP;
using System.Data;
using Kendo.Mvc.Extensions;
using iFMIS_BMS.BusinessLayer.Models;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Connector;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{

    public class PPMPdata_Layer
    {
        serviceSoapClient PPMPdata = new serviceSoapClient();
        public IEnumerable<ppmp> PPMP_data(int year, int officeid, int programid, int accountid)
        {
            year = year == 0 ? 0 : year;
            officeid = officeid == 0 ? 0 : officeid;
            programid = programid == 0 ? 0 : programid;
            accountid = accountid == 0 ? 0 : accountid;

            List<ppmp> dataTable_list = new List<ppmp>();
            //    var programID = 0;
            //  var AccountID = 0;
            //var try_list = "";
            // var item_list = PPMPdata.PPMPItems(2016, 72, 4, 969);
            //// var item_list = PPMPdata.POItems(72);
            //// var item_list = PPMPdata.PPMPLumpsum(2016, 72, 4, 969);
            // //item_list.
            // item_list.TableName = "PPMP DataTable";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select isnull(b.Programid,0),isnull(a.AccountID,0) from tbl_R_BMSProgramAccounts as a
                                                  inner JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID and a.ActionCode = b.ActionCode and a.AccountYear = b.ProgramYear
                                                  where b.OfficeID = 43 and a.AccountYear = " + year + " and a.ActionCode = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    //programID = reader.GetInt32(0);
                    //AccountID = reader.GetInt32(1);
                    if (Convert.ToInt32(reader.GetValue(0)) != 0)
                    {
                        DataTable dt = PPMPdata.PPMPItems(year, 21, Convert.ToInt32(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)));
                        //  DataTable dt = PPMPdata.PPMPNonOffice(2017, 21, 2563);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ppmp dataTable = new ppmp();
                            dataTable.itemname = dt.Rows[i]["itemname"].ToString();
                            dataTable.unit = dt.Rows[i]["unit"].ToString();
                            dataTable.unitcost = Convert.ToInt32(dt.Rows[i]["unitcost"]);
                            dataTable.qty1 = Convert.ToInt32(dt.Rows[i]["qty1"]);
                            dataTable.qty2 = Convert.ToInt32(dt.Rows[i]["qty2"]);
                            dataTable.qty3 = Convert.ToInt32(dt.Rows[i]["qty3"]);
                            dataTable.qty4 = Convert.ToInt32(dt.Rows[i]["qty4"]);
                            dataTable.amount = Convert.ToDouble(dt.Rows[i]["amount"]);
                            dataTable_list.Add(dataTable);
                        }

                        //DataTable dt = PPMPdata.PPMPLumpsum();
                        DataTable dtlumpsum = PPMPdata.PPMPLumpsum(year, 21, Convert.ToInt32(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)));
                        for (int i = 0; i < dtlumpsum.Rows.Count; i++)
                        {
                            ppmp dataTable = new ppmp();
                            dataTable.itemname = dtlumpsum.Rows[i]["itemname"].ToString();
                            dataTable.amount = dtlumpsum.Rows[i]["amount"] == DBNull.Value ? 0 : Convert.ToDouble(dtlumpsum.Rows[i]["amount"]);

                            dataTable_list.Add(dataTable);
                        }
                    }
                }

                //DataTable dt = PPMPdata.PPMPItems(ProposalYearParam, OfficeDataIDParam, ProgramID, AccountID);


            }


            //dt.TableName = "myTable";
            //data.TableName = "MyTable";
            //data.Columns.Add("Data_1");
            //data.Columns.Add("Data_2");
            //data.Columns.Add("Data_3");
            //data.Rows.Add("Test 1");
            //data.Rows.Add("Test 2");
            //data.Rows.Add("Test 3");

            //DataRow dr = new DataRow();
            //while (item_list != null)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr[0] = item_list;
            //}
            //foreach (DataRow a in dt.Rows)
            //{
            //    dataTable_list.Add(data);
            //}
            //dt.Load(item_list);
            //while (item_list != null)
            //{
            //    try_list = "This is the Result" + item_list.ToString();
            //}
            return dataTable_list;
        }
        public string UpdatePPMPAmount(int ProposalYearParam, int OfficeDataIDParam)
        {
            if (Convert.ToInt32(Account.UserInfo.Department.ToString()) == 43)
            {
                return "Unable to Copy Data From PPMP. For Non-Office Budget In-Charge Please Use Lump sum Instead";
            }
            else
            {
                DataTable dt = PPMPdata.AllPPMPItems(OfficeDataIDParam, ProposalYearParam);

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand updatePPMPStats = new SqlCommand(@"UPDATE tbl_T_BMSAccountDenomination set ActionCode = 2 WHERE OfficeID = '" + OfficeDataIDParam + "' and TransactionYear = '" + ProposalYearParam + "' and isPPMP = 1", con);
                    con.Open();
                    updatePPMPStats.ExecuteNonQuery();
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ppmp dataTable = new ppmp();
                    dataTable.itemname = dt.Rows[i]["itemname"].ToString().Replace("'", "''");
                    dataTable.amount = dt.Rows[i]["amount"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["amount"]);
                    dataTable.unitcost = Convert.ToInt32(dt.Rows[i]["unitcost"]);
                    var qty1 = dt.Rows[i]["qty1"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty1"]);
                    var qty2 = dt.Rows[i]["qty2"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty2"]);
                    var qty3 = dt.Rows[i]["qty3"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty3"]);
                    var qty4 = dt.Rows[i]["qty4"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty4"]);
                    double Total = dt.Rows[i]["totalqty"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["totalqty"]);
                    var ProgramID = dt.Rows[i]["programid"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["programid"]);
                    var AccountID = dt.Rows[i]["accountid"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["accountid"]);
                    //String singleQuotes = dt.Rows[i]["itemname"].ToString();
                    //String withDoubleQuotes = singleQuotes.Replace("'", "''");
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand insertItem = new SqlCommand(@"insert into tbl_T_BMSAccountDenomination values('" + dataTable.itemname + "'," + (dataTable.unitcost == 0 ? dataTable.amount : dataTable.unitcost) + ", '" + DateTime.Now + "'," + Account.UserInfo.eid
                            + ",1," + ProposalYearParam + "," + ProgramID + "," + AccountID + " ,1, " + (Total == 0 ? 0 : Total) + "," + dataTable.amount + "," + OfficeDataIDParam + ",1)", con);
                        con.Open();
                        insertItem.ExecuteNonQuery();
                        con.Close();
                    }

                }


                return "success";
            }
        }
        public string ProcessPPMPNonOffice(int TempAccountID, int TempOfficeID, int ProposalYearParam)
        {
            var NonOfficeprogramID = 0;
            var AccountID = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select isnull(b.Programid,0),isnull(a.AccountID,0) from tbl_R_BMSProgramAccounts as a
                                                  inner JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID and a.ActionCode = b.ActionCode and a.AccountYear = b.ProgramYear
                                                  where b.OfficeID = 43 and a.AccountYear = " + ProposalYearParam + " and a.ActionCode = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    NonOfficeprogramID = Convert.ToInt32(reader.GetValue(0));
                    AccountID = Convert.ToInt32(reader.GetValue(1));
                    GetPPMPItemsNonOffice(ProposalYearParam, TempOfficeID, NonOfficeprogramID, AccountID);
                }
            }
            return "Status Ok!";
        }
        public void GetPPMPItemsNonOffice(int ProposalYearParam, int TempOfficeID, int NonOfficeprogramID, int AccountID)
        {
            if (NonOfficeprogramID != 0)
            {
                DataTable dt = PPMPdata.PPMPItems(ProposalYearParam, TempOfficeID, NonOfficeprogramID, AccountID);
                //DataTable dt = PPMPdata.PPMPNonOffice(2017, 31, 342);
                var userID = Account.UserInfo.eid;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ppmp dataTable = new ppmp();
                    dataTable.itemname = dt.Rows[i]["itemname"].ToString();
                    dataTable.amount = dt.Rows[i]["amount"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["amount"]);
                    dataTable.unitcost = Convert.ToInt32(dt.Rows[i]["unitcost"]);
                    var qty1 = dt.Rows[i]["qty1"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty1"]);
                    var qty2 = dt.Rows[i]["qty2"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty2"]);
                    var qty3 = dt.Rows[i]["qty3"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty3"]);
                    var qty4 = dt.Rows[i]["qty4"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty4"]);
                    double Total = Convert.ToDouble(Convert.ToDouble(qty1) + Convert.ToDouble(qty2) + Convert.ToDouble(qty3) + Convert.ToDouble(qty4));
                    String singleQuotes = dt.Rows[i]["itemname"].ToString();
                    String withDoubleQuotes = singleQuotes.Replace("'", "''");
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand insertItem = new SqlCommand(@"SELECT * FROM tbl_T_BMSAccountDenomination WHERE DenominationName = '" + withDoubleQuotes + "' and ProgramID = " + NonOfficeprogramID + " and AccountID = '" + AccountID + "' and OfficeID = '" + TempOfficeID + "'" +
                            "IF @@ROWCOUNT=0 insert into tbl_T_BMSAccountDenomination values('" + withDoubleQuotes + "', '" + dataTable.unitcost + "', GETDATE(), '" + userID + "', 1, '" + ProposalYearParam + "', " + NonOfficeprogramID + ", '" + AccountID + "', 1, '" + Total + "', '" + dataTable.amount + "', '" + TempOfficeID + "','1')" +
                            "ELSE Update tbl_T_BMSAccountDenomination set DenominationName = '" + withDoubleQuotes + "', DenominationAmount = '" + dataTable.unitcost + "', QuantityOrPercentage = '" + Total + "', TotalAmount = '" + dataTable.amount + "', ActionCode=1 WHERE DenominationName = '" + withDoubleQuotes + "' and ProgramID = " + NonOfficeprogramID + " and AccountID = '" + AccountID + "'  and OfficeID = '" + TempOfficeID + "'", con);
                        //SqlCommand insertItem = new SqlCommand(@"INSERT into tbl_T_BMSAccountDenomination values('" + withDoubleQuotes + "', '" + dataTable.amount + "', GETDATE(), '" + userID + "', 1, '" + ProposalYearParam + "', 43, '" + TempAccountID + "')", con);
                        con.Open();
                        insertItem.ExecuteNonQuery();
                        con.Close();
                    }

                }
            }
        }
        public string ProcessPPMPitems(int TempProgramID, int TempAccountID, int TempOfficeID, int ProposalYearParam)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                DataTable dt = PPMPdata.PPMPItems(ProposalYearParam, TempOfficeID, TempProgramID, TempAccountID);
                //DataTable dt = PPMPdata.PPMPLumpsum();
                var userID = Account.UserInfo.eid;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ppmp dataTable = new ppmp();
                    dataTable.itemname = dt.Rows[i]["itemname"].ToString();
                    dataTable.amount = dt.Rows[i]["amount"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["amount"]);
                    dataTable.unitcost = Convert.ToInt32(dt.Rows[i]["unitcost"]);
                    var qty1 = dt.Rows[i]["qty1"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty1"]);
                    var qty2 = dt.Rows[i]["qty2"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty2"]);
                    var qty3 = dt.Rows[i]["qty3"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty3"]);
                    var qty4 = dt.Rows[i]["qty4"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty4"]);
                    double Total = Convert.ToDouble(Convert.ToDouble(qty1) + Convert.ToDouble(qty2) + Convert.ToDouble(qty3) + Convert.ToDouble(qty4));
                    String singleQuotes = dt.Rows[i]["itemname"].ToString();
                    String withDoubleQuotes = singleQuotes.Replace("'", "''");
                    SqlCommand insertItem = new SqlCommand(@"SELECT * FROM tbl_T_BMSAccountDenomination WHERE DenominationName = '" + withDoubleQuotes + "' and ProgramID = '" + TempProgramID + "' and AccountID = '" + TempAccountID + "'  and OfficeID = '" + TempOfficeID + "'" +
                            "IF @@ROWCOUNT=0 insert into tbl_T_BMSAccountDenomination values('" + withDoubleQuotes + "', '" + dataTable.unitcost + "', GETDATE(), '" + userID + "', 1, '" + ProposalYearParam + "', '" + TempProgramID + "', '" + TempAccountID + "', 1, '" + Total + "', '" + dataTable.amount + "','" + TempOfficeID + "','1')" +
                            "ELSE Update tbl_T_BMSAccountDenomination set DenominationName = '" + withDoubleQuotes + "', DenominationAmount = '" + dataTable.unitcost + "', QuantityOrPercentage = '" + Total + "', TotalAmount = '" + dataTable.amount + "', ActionCode=1 WHERE DenominationName = '" + withDoubleQuotes + "' and ProgramID = '" + TempProgramID + "' and AccountID = '" + TempAccountID + "'  and OfficeID = '" + TempOfficeID + "'", con);
                    //SqlCommand insertItem = new SqlCommand(@"INSERT into tbl_T_BMSAccountDenomination values('" + dataTable.itemname + "', '" + dataTable.amount + "', GETDATE(), '"+userID+"', 1, '" + ProposalYearParam + "', '" + TempProgramID + "', '" + TempAccountID + "')", con);
                    con.Open();
                    insertItem.ExecuteNonQuery();
                    con.Close();
                }

            }
            return "Status Ok!";
        }
        public string ProcessPPMPLumpsum(int TempProgramID, int TempAccountID, int TempOfficeID, int ProposalYearParam)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                DataTable dt = PPMPdata.PPMPLumpsum(ProposalYearParam, TempOfficeID, TempProgramID, TempAccountID);
                //DataTable dt = PPMPdata.PPMPLumpsum();
                var userID = Account.UserInfo.eid;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ppmp dataTable = new ppmp();
                    dataTable.itemname = dt.Rows[i]["itemname"].ToString();
                    dataTable.amount = dt.Rows[i]["amount"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["amount"]);

                    String singleQuotes = dt.Rows[i]["itemname"].ToString();
                    String withDoubleQuotes = singleQuotes.Replace("'", "''");
                    SqlCommand insertItem = new SqlCommand(@"SELECT * FROM tbl_T_BMSAccountDenomination WHERE DenominationName = '" + withDoubleQuotes + "' and ProgramID = '" + TempProgramID + "' and AccountID = '" + TempAccountID + "'  and OfficeID = '" + TempOfficeID + "'" +
                            "IF @@ROWCOUNT=0 insert into tbl_T_BMSAccountDenomination values('" + withDoubleQuotes + "', '" + dataTable.amount + "', GETDATE(), '" + userID + "', 1, '" + ProposalYearParam + "', '" + TempProgramID + "', '" + TempAccountID + "', 1, 1, '" + dataTable.amount + "', '" + TempOfficeID + "','1')" +
                            "ELSE Update tbl_T_BMSAccountDenomination set DenominationName = '" + withDoubleQuotes + "', DenominationAmount = '" + dataTable.amount + "', QuantityOrPercentage = 1, TotalAmount = '" + dataTable.amount + "', ActionCode=1 WHERE DenominationName = '" + withDoubleQuotes + "' and ProgramID = '" + TempProgramID + "' and AccountID = '" + TempAccountID + "'  and OfficeID = '" + TempOfficeID + "'", con);

                    //SqlCommand insertItem = new SqlCommand(@"INSERT into tbl_T_BMSAccountDenomination values('" + dataTable.itemname + "', '" + dataTable.amount + "', GETDATE(), '" + userID + "', 1, '" + ProposalYearParam + "', '" + TempProgramID + "', '" + TempAccountID + "')", con);
                    con.Open();
                    insertItem.ExecuteNonQuery();
                    con.Close();
                }

            }
            return "Status Ok!";
        }
        public string ProcessPPMPNonOfficeLumpsum(int TempProgramID, int TempAccountID, int TempOfficeID, int ProposalYearParam)
        {
            var NonOfficeprogramID = 0;
            var AccountID = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select isnull(b.Programid,0),isnull(a.AccountID,0) from tbl_R_BMSProgramAccounts as a
                                                  inner JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID and a.ActionCode = b.ActionCode and a.AccountYear = b.ProgramYear
                                                  where b.OfficeID = 43 and a.AccountYear = " + ProposalYearParam + " and a.ActionCode = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    NonOfficeprogramID = Convert.ToInt32(reader.GetValue(0));
                    AccountID = Convert.ToInt32(reader.GetValue(1));
                    getPPMPItemsNonOfficeLumpsum(ProposalYearParam, TempOfficeID, NonOfficeprogramID, AccountID);
                }
            }
            return "Status Ok!";
        }
        public void getPPMPItemsNonOfficeLumpsum(int ProposalYearParam, int TempOfficeID, int NonOfficeprogramID, int AccountID)
        {
            if (NonOfficeprogramID != 0)
            {
                DataTable dt = PPMPdata.PPMPLumpsum(ProposalYearParam, TempOfficeID, NonOfficeprogramID, AccountID);
                //DataTable dt = PPMPdata.PPMPLumpsum();
                var userID = Account.UserInfo.eid;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ppmp dataTable = new ppmp();
                    dataTable.itemname = dt.Rows[i]["itemname"].ToString();
                    dataTable.amount = dt.Rows[i]["amount"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["amount"]);

                    String singleQuotes = dt.Rows[i]["itemname"].ToString();
                    String withDoubleQuotes = singleQuotes.Replace("'", "''");
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand insertItem = new SqlCommand(@"SELECT * FROM tbl_T_BMSAccountDenomination WHERE DenominationName = '" + withDoubleQuotes + "' and ProgramID = " + NonOfficeprogramID + " and AccountID = '" + AccountID + "'  and OfficeID = '" + TempOfficeID + "'" +
                                "IF @@ROWCOUNT=0 insert into tbl_T_BMSAccountDenomination values('" + withDoubleQuotes + "', '" + dataTable.amount + "', GETDATE(), '" + userID + "', 1, '" + ProposalYearParam + "', " + NonOfficeprogramID + ", '" + AccountID + "', 1, 1, '" + dataTable.amount + "', '" + TempOfficeID + "',1)" +
                                "ELSE Update tbl_T_BMSAccountDenomination set DenominationName = '" + withDoubleQuotes + "', DenominationAmount = '" + dataTable.amount + "', QuantityOrPercentage = 1, TotalAmount = '" + dataTable.amount + "', ActionCode=1 WHERE DenominationName = '" + withDoubleQuotes + "' and ProgramID = " + NonOfficeprogramID + " and AccountID = '" + AccountID + "'  and OfficeID = '" + TempOfficeID + "'", con);

                        //SqlCommand insertItem = new SqlCommand(@"INSERT into tbl_T_BMSAccountDenomination values('" + dataTable.itemname + "', '" + dataTable.amount + "', GETDATE(), '" + userID + "', 1, '" + ProposalYearParam + "', '" + TempProgramID + "', '" + TempAccountID + "')", con);
                        con.Open();
                        insertItem.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }
        public string DownloadPPMP(int officeid, int transyear)
        {
            if (Convert.ToInt32(Account.UserInfo.Department.ToString()) == 43)
            {
                return "Unable to download Data From PPMP. For Non-Office Budget In-Charge Please Use Lump sum Instead";
            }
            else
            {
                DataTable dt = PPMPdata.AllPPMPItems(officeid, transyear);

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand updatePPMPStats = new SqlCommand(@"UPDATE tbl_T_BMSPPMP set ActionCode = 2 WHERE OfficeID = '" + officeid + "' and TransactionYear = '" + transyear + "' and isPPMP = 1", con);
                    con.Open();
                    updatePPMPStats.ExecuteNonQuery();


                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ppmp dataTable = new ppmp();
                    dataTable.itemname = dt.Rows[i]["itemname"].ToString().Replace("'", "''");
                    dataTable.amount = dt.Rows[i]["amount"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["amount"]);
                    dataTable.unitcost = Convert.ToInt32(dt.Rows[i]["unitcost"]);
                    var qty1 = dt.Rows[i]["qty1"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty1"]);
                    var qty2 = dt.Rows[i]["qty2"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty2"]);
                    var qty3 = dt.Rows[i]["qty3"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty3"]);
                    var qty4 = dt.Rows[i]["qty4"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty4"]);
                    double Total = dt.Rows[i]["totalqty"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["totalqty"]);
                    var ProgramID = dt.Rows[i]["programid"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["programid"]);
                    var AccountID = dt.Rows[i]["accountid"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["accountid"]);
                    //String singleQuotes = dt.Rows[i]["itemname"].ToString();
                    //String withDoubleQuotes = singleQuotes.Replace("'", "''");
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand insertItem = new SqlCommand(@"insert into tbl_T_BMSPPMP values('" + dataTable.itemname + "'," + (dataTable.unitcost == 0 ? dataTable.amount : dataTable.unitcost) + ", '" + DateTime.Now + "'," + Account.UserInfo.eid
                            + ",1," + transyear + "," + ProgramID + "," + AccountID + " ,1, " + (Total == 0 ? 0 : Total) + "," + dataTable.amount + "," + officeid + ",1)", con);
                        con.Open();
                        insertItem.ExecuteNonQuery();
                        con.Close();
                    }

                }


                return "success";
            }
        }
    }
}