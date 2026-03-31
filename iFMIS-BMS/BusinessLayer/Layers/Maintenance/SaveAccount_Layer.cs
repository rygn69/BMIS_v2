using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class SaveAccount_Layer
    {
        public string SaveNewAccount(account_code Account_info)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var UserInfo = Account.UserInfo.eid.ToString();

                SqlCommand com = new SqlCommand(@"sp_BMS_CreateUpdateAccount " + Account_info.FMISAccountCode + ",'"
                    + (Account_info.ThirdLevelGroup == null ? "" : Account_info.ThirdLevelGroup) + "', "
                    + (Account_info.AccountCode == 0 ? "NULL" : Account_info.AccountCode.ToString()) + ", '"
                    + (Account_info.child_series_no == null ? "": Account_info.child_series_no.Replace("'", "''")) + "', '"
                    + (Account_info.Account_Name == null ? "" : Account_info.Account_Name.Replace("'", "''")) + "', '"
                    + (Account_info.FundType == null ? "" : Account_info.FundType.Replace("'", "''")) + "', " + UserInfo + "", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();  
            }
        }
    }
}