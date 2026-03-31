using iFMIS_BMS.BusinessLayer.Connector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


    public class GlobalFunctions
    {
        public static string CurrencyFormatString(double Amount)
        {
            return "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", Amount);
        }
        public static string CurrencyFormatStringNoSymbol(double Amount)
        {
            return string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", Amount);
        }
        public static string getOfficeHead(int OfficeID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select top 1 OfficeHead from pmis.dbo.m_vwGetOfficehead where OfficeID = (select b.PMISOfficeID from dbo.tbl_R_BMSOffices as b where b.OfficeID = " + OfficeID + ") ORDER BY trnno  ", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception)
            {
                return "No Signatory Found";
            }
                        
        }
        public static string getOfficeHeadDesignation(int OfficeID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select top 1 OfficeDesignation from pmis.dbo.m_vwGetOfficehead where OfficeID = (select b.PMISOfficeID from dbo.tbl_R_BMSOffices as b where b.OfficeID = " + OfficeID + ") ORDER BY trnno  ", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception)
            {
                return "No Designation Found";
            }
            
        }
        public static string QRCodeValue(string PrintedBy, string ComputerIP)
        {
            return "SYSTEM GENERATED DOCUMENT" + Environment.NewLine
                                    + "Printed by : " + PrintedBy + Environment.NewLine
                                    + "Print Date : " + DateTime.Now + Environment.NewLine
                                    + "I.P. Address : " + ComputerIP + Environment.NewLine;
        }
        public static string getReportTextBoxValue(int ReportID,int FieldID) 
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select [Value] from tbl_R_BMSReportTextBoxes where ReportID = " + ReportID + " and ActionCode = 1 and FieldID = " + FieldID + "", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string QR_globalstr { get; set; }
        public static int wfppreparer_sign { get; set; }
        public static int wfpdepthead_sign { get; set; }

}