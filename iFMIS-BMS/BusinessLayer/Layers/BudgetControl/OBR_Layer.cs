using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.BudgetControl;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.BudgetControl
{
    public class OBR_Layer
    {
        public OBRLogger CheckInOBR(int? FundID, string UserInTimeStamp, int? UserID, string RefNo, int? isPastYear , string cttsno,string obrno,int? chkcafoa,int? tyear,int? officeassign)
        {
            OBRLogger data = new OBRLogger();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                isPastYear = isPastYear == null ? 0 : isPastYear;
                RefNo = RefNo == "" ? null : RefNo;

                if (Account.UserInfo.lgu == 0) //PGAS
                {
                    SqlCommand query = new SqlCommand(@"dbo.sp_BMS_CheckINOBR " + FundID + ", " + Account.UserInfo.eid + ", '" + RefNo + "', " + isPastYear + ",'" + cttsno + "','" + obrno + "'," + chkcafoa + "", con);
                    con.Open();
                    SqlDataReader reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        data.TransactionNo = Convert.ToString(reader.GetValue(0));
                        data.OBRNo = Convert.ToString(reader.GetValue(1));
                        data.UserINTimeStamp = Convert.ToString(reader.GetValue(3));
                    }
                }
                else //OTHER LGU's
                {
                    SqlCommand query = new SqlCommand(@"dbo.sp_BMS_CheckINOBR " + FundID + ", " + Account.UserInfo.eid + ", '" + RefNo + "', " + isPastYear + ",'" + cttsno + "','" + obrno + "'," + chkcafoa + ","+ officeassign + "", con);
                    con.Open();
                    SqlDataReader reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        data.TransactionNo = Convert.ToString(reader.GetValue(0));
                        data.OBRNo = Convert.ToString(reader.GetValue(1));
                        data.UserINTimeStamp = Convert.ToString(reader.GetValue(3));
                    }
                }
            }
            return data;
        }
    }
}