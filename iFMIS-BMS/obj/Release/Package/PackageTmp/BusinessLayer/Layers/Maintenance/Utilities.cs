using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using iFMIS_BMS.Classes;
using System.Data.SqlClient;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class Utilities
    {
        public void UpdateLogTrans(IEnumerable<account_code> LoggerTransaction)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                var userID = Account.UserInfo.eid;
                var UserType = Account.UserInfo.UserTypeDesc;
                foreach (account_code Data in LoggerTransaction)
                {
                    SqlCommand updateLogtrans = new SqlCommand(@"exec sp_BMS_EditLogger " + Data.TransactionNo + ",'" + Data.obrno +"','"+ Data.NonOfficeTransNo +"','"+ Data.ReferenceNo +"'", con);
                    updateLogtrans.ExecuteNonQuery();
                }
            }
        }
        public void deleteCurrentCtrl(IEnumerable<account_code> CurrentTransaction)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                foreach (account_code Data in CurrentTransaction)
                {
                    SqlCommand updateLogtrans = new SqlCommand(@"exec sp_BMS_UpdateCurrentCtrl '" + Data.obrno + "',"+ Data.actioncode +",'"+ Data.datetimeentered + "',"+ Data.trnno + "", con);
                    updateLogtrans.ExecuteNonQuery();
                }
            }
        }
    }
}