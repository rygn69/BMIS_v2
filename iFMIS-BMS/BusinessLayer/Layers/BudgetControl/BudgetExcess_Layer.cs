using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.BudgetControl;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.BudgetControl
{
    public class BudgetExcess_Layer
    {
        public ExcessModel ExcessComputation(int? FundID, int? AccountID, int? Year)
        {
            ExcessModel data = new ExcessModel();
            if (Common.IsServerConnected())
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var query = @"dbo.sp_BMS_ExcessComputation "+FundID+", "+AccountID+", "+Year+"";
                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();

                    while (reader.Read())
                    {
                        data.ConnectionStatus = 1;
                        data.Appropriation = Convert.ToDouble(reader.GetValue(0));
                        data.Obligation = Convert.ToDouble(reader.GetValue(1));
                        data.Difference = Convert.ToDouble(reader.GetValue(2));
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