using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class StepIncrementLayer
    {
        public StepIncrement getActualSalary(int? EmployeeID, int? OfficeID)
        {
            StepIncrement StepIncrement = new StepIncrement();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.eid, a.BasicNew ,a.SG, a.Step, b.BasicNew, b.SG, b.Step
                                                from pmis.dbo.vw_RGPermanentAndCasual as a 
                                                LEFT JOIN dbo.tbl_R_BMSStepIncrement as b 
                                                on b.eid = a.eid and b.OfficeID = a.officeID
                                                WHERE b.DateCopied like CONCAT('%',year(getdate()),'%') and b.ActionCode = 1
                                                and a.OfficeID='" + getPmisOfficeID(OfficeID) + "' and a.eid='" + EmployeeID + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    StepIncrement.eid = reader.GetInt64(0);
                    StepIncrement.ActualSalary = "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", Convert.ToDouble(reader.GetValue(1)));
                    StepIncrement.SalaryGrade = Convert.ToInt16(reader.GetValue(2));
                    StepIncrement.Step = Convert.ToInt16(reader.GetValue(3));
                    StepIncrement.BudgetSalary = "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", Convert.ToDouble(reader.GetValue(4)));
                    StepIncrement.SalaryGradeBudgeted = Convert.ToInt16(reader.GetValue(5));
                    StepIncrement.StepBudgeted = Convert.ToInt16(reader.GetValue(6));
                }
            }
            
            return StepIncrement;
        }
        public int? getPmisOfficeID(int? OfficeID)
        {
            var PMISOfficeID = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT PMISOfficeID from tbl_R_BMSOffices where OfficeID='" + OfficeID + "'", con);
                con.Open();
                PMISOfficeID = Convert.ToInt32(com.ExecuteScalar().ToString());
            }
            return PMISOfficeID;
        }
        public string getBudgetSalary(int SalaryGrade, string Step)
        {
            var BudgetSalary = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select isNull(" + Step + ",0) from pmis.dbo.sgTrance_refs where Salary_Grade =" + SalaryGrade + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    BudgetSalary = "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", Convert.ToDouble(reader.GetValue(0)));
                }
            }

            return BudgetSalary;
        }
        public string UpdateSalary(int SalaryGrade, string Step, int eid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update dbo.tbl_R_BMSStepIncrement 
                                                    set SG=" + SalaryGrade + ",Step=" + Step + ",BasicNew = (SELECT Step"+ Step
                                                    + " from pmis.dbo.sgTrance_refs where Salary_Grade = "+SalaryGrade
                                                    + ") where eid = " + eid + " and DateCopied like CONCAT('%',year(getdate()),'%')", con);
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