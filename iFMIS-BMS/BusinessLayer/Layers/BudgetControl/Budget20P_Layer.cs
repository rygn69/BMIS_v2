using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.BudgetControl;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.BudgetControl
{
    public class Budget20P_Layer
    {
        public PPAModel PPAComputation(int? Year, int? AccountID, int? RootPPA, int? PPAID,string ControlNo,int? ProgramID)
        {
            PPAModel data = new PPAModel();
            if (Common.IsServerConnected())
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    AccountID = AccountID == null ? 0 : AccountID;
                    var query = @"dbo.sp_BMS_20PComputation " + Year + ", " + AccountID + ", " + RootPPA + ", " + PPAID + ",'"+ ControlNo + "',"+ ProgramID + "";
                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();
                    try
                    {
                        SqlDataReader reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            data.ConnectionStatus = 1;
                            data.AcctRelease = Convert.ToDouble(reader.GetValue(0));
                            data.AcctObligation = Convert.ToDouble(reader.GetValue(1));
                            data.AcctDifference = Convert.ToDouble(reader.GetValue(2));
                            data.RootPPARelease = Convert.ToDouble(reader.GetValue(3));
                            data.RootPPAObligation = Convert.ToDouble(reader.GetValue(4));
                            data.RootPPADifference = Convert.ToDouble(reader.GetValue(5));
                            data.PPADifference = Convert.ToDouble(reader.GetValue(6));
                            data.obrno = Convert.ToString(reader.GetValue(7));
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
                data.MBody = "Please contact your System Adminstrator.";
                data.MType = "error";
            }
            return data;
        }
    }
}