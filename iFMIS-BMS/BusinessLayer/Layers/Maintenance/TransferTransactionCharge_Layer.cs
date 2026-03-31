using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class TransferTransactionCharge_Layer
    {
        public ChangeTransactionCharge searchOBR(string OBRNo)
        {
            ChangeTransactionCharge data = new ChangeTransactionCharge();
            if (Common.IsServerConnected())
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    var query = @"dbo.sp_BMS_ExecutionUtilities_SearchOBR '"+OBRNo+"'";
                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();
                    try
                    {
                        SqlDataReader reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            data.ConnectionStatus = 1;
                            data.trnno = Convert.ToInt32(reader.GetValue(0));
                            data.IsIncome = Convert.ToInt32(reader.GetValue(1));
                            data.OfficeID = Convert.ToInt32(reader.GetValue(2));
                            data.Amount = Convert.ToDouble(reader.GetValue(3));
                            data.MTitle = Convert.ToString(reader.GetValue(4));
                            data.MBody = Convert.ToString(reader.GetValue(5));
                            data.MType = Convert.ToString(reader.GetValue(6));
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
        public ChangeTransactionCharge searchOBRExcess(string OBRNo)
        {
            ChangeTransactionCharge data = new ChangeTransactionCharge();
            if (Common.IsServerConnected())
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    var query = @"dbo.sp_BMS_ExecutionUtilitiesExcess_SearchOBR '" + OBRNo + "'";
                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();
                    try
                    {
                        SqlDataReader reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            data.ConnectionStatus = 1;
                            data.trnno = Convert.ToInt32(reader.GetValue(0));
                            data.OfficeID = Convert.ToInt32(reader.GetValue(1));
                            data.Amount = Convert.ToDouble(reader.GetValue(2));
                            data.MTitle = Convert.ToString(reader.GetValue(3));
                            data.MBody = Convert.ToString(reader.GetValue(4));
                            data.MType = Convert.ToString(reader.GetValue(5));
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
        public ChangeTransactionCharge AddTransferTransaction(int? OfficeID, int? ProgramID, int? AccountID, int? trnno, int? _Office, int? _Program, int? _Accounts, int? IsIncome)
        {
            ChangeTransactionCharge data = new ChangeTransactionCharge();
            if (Common.IsServerConnected())
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    var query = @"dbo.sp_BMS_ExecutionUtilities_AddTransferTransaction  " + OfficeID + ", " + ProgramID + ", " + AccountID + ", " + trnno + ", " + _Office + ", " + _Program + ", " + _Accounts + ", " + Account.UserInfo.eid + ", " + IsIncome + "";
                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();
                    try
                    {
                        SqlDataReader reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            data.ConnectionStatus = 1;
                            data.NewOBR = Convert.ToString(reader.GetValue(0));
                            data.Amount = Convert.ToDouble(reader.GetValue(1));
                            data.TempID = Convert.ToInt32(reader.GetValue(2));

                            
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
        public ChangeTransactionCharge AddTransferTransaction_Excess(int? OfficeID, int? AcctCharge, int? PPA, int? NewOffice, int? NewAcctCharge, int? NewPPA, int? Claim)
        {
            ChangeTransactionCharge data = new ChangeTransactionCharge();
            if (Common.IsServerConnected())
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    var query = @"dbo.sp_BMS_ExecutionUtilities_AddTransferTransaction_Excess  " + OfficeID + ", " + AcctCharge + ", " + PPA + ", " + NewOffice + ", " + NewAcctCharge + ", " + NewPPA + ", " + Claim + "";
                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();
                    try
                    {
                        SqlDataReader reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            data.ConnectionStatus = 1;
                            data.NewOBR = Convert.ToString(reader.GetValue(0));
                            data.Amount = Convert.ToDouble(reader.GetValue(1));
                            data.TempID = Convert.ToInt32(reader.GetValue(2));


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
        public ChangeTransactionCharge SaveTransfer(string OBRNO, int? Years)
        {
            ChangeTransactionCharge data = new ChangeTransactionCharge();
            if (Common.IsServerConnected())
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    var query = @"dbo.sp_BMS_ExecutionUtilities_SaveTransfer  '" + OBRNO + "', "+Account.UserInfo.eid+", " + Years + "";
                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();
                    try
                    {
                        SqlDataReader reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            data.ConnectionStatus = 1;
                            data.MTitle = Convert.ToString(reader.GetValue(0));
                            data.MBody = Convert.ToString(reader.GetValue(1));
                            data.MType = Convert.ToString(reader.GetValue(2));
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
        public ChangeTransactionCharge DeleteTransfer(int? TempID)
        {
            ChangeTransactionCharge data = new ChangeTransactionCharge();
            if (Common.IsServerConnected())
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    var query = @"dbo.sp_BMS_ExecutionUtilities_DeleteTransfer " + TempID + "";
                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();
                    try
                    {
                        SqlDataReader reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            data.ConnectionStatus = 1;
                            data.MTitle = Convert.ToString(reader.GetValue(0));
                            data.MBody = Convert.ToString(reader.GetValue(1));
                            data.MType = Convert.ToString(reader.GetValue(2));
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
        public IEnumerable<ChangeTransactionCharge> grTransactionCharge(int? trnno, int? Year)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_TransactionCharge_Grid "+trnno+", "+Year+"", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.AccountName = Convert.ToString(reader.GetValue(0));
                    value.AmountDummy = Convert.ToDouble(reader.GetValue(1));
                    value.TempID = Convert.ToInt32(reader.GetValue(2));
                    value._OldAcctCharge = Convert.ToInt32(reader.GetValue(3));
                    data.Add(value);
                }
            }
            return data;
        }
        public double SetAmountCharge(int? _trnno, int? AcctCharge)
        {
            double data = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"SELECT dbo.fn_BMS_ExecutionUtilities_SetAmountCharge(" + _trnno + ", " + AcctCharge + ", 1)", con);
                con.Open();
                data = Convert.ToDouble(com.ExecuteScalar());
            }
            return data;
        }
        public double SetAmountCharge_Excess(int? _trnno, int? AcctCharge)
        {
            double data = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"SELECT dbo.fn_BMS_ExecutionUtilities_SetAmountCharge(" + _trnno + ", " + AcctCharge + ", 2)", con);
                con.Open();
                data = Convert.ToDouble(com.ExecuteScalar());
            }
            return data;
        }
        public double CheckRemainingBalance(int? OfficeID, int? ProgramID, int? Account, int? Year, int? IsIncome)
        {
            double data = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"SELECT dbo.fn_BMS_ExecutionUtilities_CheckRemainingBalance(" + OfficeID + ", " + ProgramID + ", " + Account + ", " + Year + ", " + IsIncome + ")", con);
                con.Open();
                data = Convert.ToDouble(com.ExecuteScalar());
            }
            return data;
        }
        public double CheckRemainingBalance_Excess(int? Account, int? Year)
        {
            double data = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"SELECT dbo.fn_BMS_ExecutionUtilities_CheckRemainingBalance_Excess(" + Account + ", " + Year + ")", con);
                con.Open();
                data = Convert.ToDouble(com.ExecuteScalar());
            }
            return data;
        }
    }
}