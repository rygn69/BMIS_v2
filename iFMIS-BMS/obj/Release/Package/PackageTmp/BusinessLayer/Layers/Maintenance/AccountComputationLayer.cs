using iFMIS_BMS.BusinessLayer.Connector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class AccountComputationLayer
    {
        public string SaveNewComputation(int AccountCode, double Amount, int NoOfMonths, double Percentage,
                                        int isRoundOff, double MaxAmount, int EmployeeType, int YearActive)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"insert into tbl_R_BMSAccountComputation values(" + AccountCode + "," + Amount
                                                    + ", " + NoOfMonths + ", " + Percentage + ", " + isRoundOff + "," + MaxAmount
                                                    + "," + EmployeeType + "," + YearActive + ")", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }   
        }
        public string CheckAccountComputation(int AccountCode, int YearActive)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Select AccountCode from tbl_R_BMSAccountComputation where AccountCode = " + AccountCode + "and YearActive = " + YearActive + " + 1", con);
                    con.Open();
                    return Convert.ToString(com.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }  
        }
        public string UpdateComputation(int ComputationID, double Amount, int NoOfMonths, double Percentage,
                                        int isRoundOff, double MaxAmount, int EmployeeType)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSAccountComputation set Amount = " + Amount + ", NoOfMonths =" + NoOfMonths 
                                                    + ", Percentage =" + Percentage + ", isRoundOf = " + isRoundOff + ", MaxAmount = " + MaxAmount + ", EmployeeType = " + EmployeeType + " where ComputationID = " + ComputationID + "", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string DeleteComputation(int ComputationID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Delete from tbl_R_BMSAccountComputation where ComputationID = " + ComputationID + "", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}