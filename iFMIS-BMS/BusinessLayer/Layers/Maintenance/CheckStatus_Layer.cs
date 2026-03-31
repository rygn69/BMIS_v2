using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class CheckStatus_Layer
    {
        public string CheckStatus(checkStatus CheckUserStatus)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var UserTypeDesc = Account.UserInfo.UserTypeDesc.ToString();
                if (UserTypeDesc == "Budget In-Charge")
                {
                    var OfficeID = Account.UserInfo.Department.ToString();
                    return OfficeID;
                   // return "1";
                }
                else
                {
                    return "0";
                }
            }
        }
    }
}