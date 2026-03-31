using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Models.BudgetControl;
using iFMIS_BMS.BusinessLayer.Connector;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using iFMIS_BMS.Base;

namespace iFMIS_BMS.BusinessLayer.Layers.BudgetControl
{
    public class CurrentControl_Layer
    {
        // Checking Connection
        //if (Common.IsServerConnected())
        //    {
        //    }
        //    else
        //    {
        //        data.MTitle = "Server Connection Lost!";
        //        data.MBody = "Please contact your System Adminstrator.";
        //        data.MType = "error";
        //    }
        //Update on 4/18/2018 - xXx
        //public CurrentControl_Model CurrentComputation(int? OfficeID, int? AccountID, int? ProgramID, int? YearOf, int? OOE, int? WithSubsidiaryFlag, int? param)
        //{
        //    CurrentControl_Model data = new CurrentControl_Model();
        //    if (Common.IsServerConnected())
        //    {
        //        using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //        {
        //            AccountID = AccountID == null ? 0 : AccountID;
        //            var query = @"dbo.sp_BMS_CurrentComputation "+OfficeID+", "+AccountID+", "+ProgramID+", "+YearOf+", "+OOE+", "+WithSubsidiaryFlag+", "+param+"";
        //            SqlCommand com = new SqlCommand(query, con);
        //            con.Open();
        //            com.CommandTimeout = 0;
        //            try
        //            {
        //                SqlDataReader reader = com.ExecuteReader();
                        
        //                while (reader.Read())
        //                {
        //                    data.ConnectionStatus = 1;
        //                    data.AllotedAmount = Convert.ToDouble(reader.GetValue(0));
        //                    data.ObligatedAmount = Convert.ToDouble(reader.GetValue(1));
        //                    data.BalanceAllotment = Convert.ToDouble(reader.GetValue(2));
        //                }

        //            }
        //            catch (SqlException ex)
        //            {
        //                data.ConnectionStatus = 1;
        //                data.MTitle = ex.Message;
        //                data.MType = "error";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        data.ConnectionStatus = 0;
        //        data.MTitle = "Server Connection Lost!";
        //        data.MBody = "Please contact your System Adminstrator.";
        //        data.MType = "error";
        //    }
        //    return data;
        //}
        //Update on 4/18/2018 - xXx
        public CurrentControl_Model CurrentComputation(int? OfficeID=0, int? AccountID=0, int? ProgramID=0, int? YearOf=0, int? OOE=0, int? WithSubsidiaryFlag=0, int? param=0,string refno="")
        {
            CurrentControl_Model data = new CurrentControl_Model();
            DataTable _dt = new DataTable();
           // var nullooe=FUNC.LongDBNull
            string _sqlQuery = "dbo.sp_BMS_CurrentComputation " + OfficeID + ", " + AccountID + ", " + ProgramID + ", " + YearOf + ", " + OOE + ", " + WithSubsidiaryFlag + ", " + param + ",'"+ refno + "'";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            data.ConnectionStatus = 1;
            data.AllotedAmount = Convert.ToDouble(_dt.Rows[0][0]);
            data.ObligatedAmount = Convert.ToDouble(_dt.Rows[0][1]);
            data.BalanceAllotment = Convert.ToDouble(_dt.Rows[0][2]);
            data.Appropriation = Convert.ToDouble(_dt.Rows[0][3]);
            return data;

        }
        public CurrentControl_Model CheckIfAirmarkExist(string ControlNo)
        {
            CurrentControl_Model data = new CurrentControl_Model();
            if (Common.IsServerConnected())
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    var query = @"dbo.sp_BMS_CheckIfAirmarkExist '" + ControlNo + "'";
                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();
                    try
                    {
                        SqlDataReader reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            data.ConnectionStatus = 1;
                            data.ActionCode = Convert.ToInt32(reader.GetValue(0));
                            data.MTitle = Convert.ToString(reader.GetValue(1));
                            data.MBody = Convert.ToString(reader.GetValue(2));
                            data.MType = Convert.ToString(reader.GetValue(3));
                        }

                    }
                    catch (SqlException ex)
                    {
                        data.ConnectionStatus = 1;
                        data.MTitle = ex.Message;
                        data.MBody = "";
                        data.MType = "error";
                    }
                }
            }
            else
            {
                data.ConnectionStatus = 0;
                data.MTitle = "Server Connection Lost!";
                data.MBody = "Please contact your System Adminstrator.";
                data.MType = "error";
            }
            return data;
        }
        public CurrentControl_Model CheckDvNoStatus(string OBRNO) 
        {
            
            CurrentControl_Model data = new CurrentControl_Model();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var query = @"SELECT c.status
                        FROM fmis.dbo.tblAMIS_IncomingDVTrns as a
                        LEFT JOIN fmis.dbo.tblAMIS_Logtrans as b ON a.DVNo = b.Dvno
                        LEFT JOIN fmis.dbo.tblCMS_TransStatusMap as c ON c.code = b.status
                        WHERE a.OBRNo = '" + OBRNO + "' and b.ActionCode = 1";
                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();
                    try
                    {
                        SqlDataReader reader = com.ExecuteReader();

                        while (reader.Read())
                        {

                            data.MBody = Convert.ToString(reader.GetValue(0));
                            data.ORNumber = Convert.ToString(reader.GetValue(1));
                        }

                    }
                    catch (SqlException ex)
                    {
                        
                        data.MBody = ex.Message;
                        data.ORNumber = "0";

                    }           
            return data;
            }
        }
        public CurrentControl_Model SPOComputation(int? ProgramID, int? AccountID, int? YearOF, int? IsIncome, int? SPO_ID)
        {
            CurrentControl_Model data = new CurrentControl_Model();
            if (Common.IsServerConnected())
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    AccountID = AccountID == null ? 0 : AccountID;
                    var query = @"dbo.sp_BMS_NonOfficeSPOComputation " + ProgramID + ", " + AccountID + ", " + YearOF + ", " + IsIncome + ", " + SPO_ID + "";
                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();
                    try
                    {
                        SqlDataReader reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            data.ConnectionStatus = 1;
                            data.Amount = Convert.ToDouble(reader.GetValue(0));
                            data.MTitle = Convert.ToString(reader.GetValue(1));
                            data.MType = "success";
                        }

                    }
                    catch (SqlException ex)
                    {
                        data.ConnectionStatus = 1;
                        data.MTitle = ex.Message;
                        data.MType = "error";
                    }
                }
            }
            else
            {
                data.ConnectionStatus = 0;
                data.MTitle = "Server Connection Lost!";
                data.MBody = "Please contact your System Administrator.";
                data.MType = "error";
            }
            return data;
        }
    }
}