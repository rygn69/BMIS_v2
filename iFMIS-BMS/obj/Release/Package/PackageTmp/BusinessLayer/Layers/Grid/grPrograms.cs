using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using iFMIS_BMS.BusinessLayer.Connector;
using System.Data.SqlClient;

namespace iFMIS_BMS.BusinessLayer.Layers.Grid
{
    public class grPrograms
    {
       
        public IEnumerable<programs> grProgram_list(int OfficeID, int YearOf)
        {
            List<programs> ProgramList = new List<programs>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("Select ProgramDescription, ProgramID, OrderNo, FundCode, OfficeProgramID from tbl_R_BMSOfficePrograms where OfficeID=" + OfficeID + " and ProgramYear= " + YearOf + " and ActionCode = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    programs office_list = new programs();
                    office_list.program_name = reader.GetString(0);
                    office_list.program_id = Convert.ToInt32(reader.GetValue(1));
                    office_list.orderBY = Convert.ToInt32(reader.GetValue(2));
                    office_list.fund_id = Convert.ToInt32(reader.GetValue(3));
                    office_list.OfficeProgramID = Convert.ToInt32(reader.GetValue(4));
                    ProgramList.Add(office_list);
                }
            }
            return ProgramList;
        }
        public IEnumerable<programs> grProgram_Info(string DataDropdown)
        {
            List<programs> ProgramList = new List<programs>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand("Select ProgramDescription, OfficeProgramID, OfficeName from tbl_R_BMSOfficePrograms LEFT JOIN tbl_R_BMSOffices ON tbl_R_BMSOfficePrograms.OfficeID = tbl_R_BMSOffices.OfficeID where ProgramYear= YEAR(GETDATE()+1) and tbl_R_BMSOfficePrograms.ActionCode = 1 and tbl_R_BMSOffices.OfficeID = '" + DataDropdown + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    programs office_list = new programs();
                    office_list.program_name = reader.GetString(0);
                    office_list.program_id = Convert.ToInt32(reader.GetValue(1));
                    office_list.OfficeName = reader.GetString(2);
                    ProgramList.Add(office_list);
                }
                return ProgramList;
            }
        }
        public IEnumerable<lbp5> lbp5_info(int? OfficeID)
        {
            List<lbp5> LBP5LIST = new List<lbp5>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT * from tbl_R_LBP5_Objectives WHERE OfficeID = '" + OfficeID + "' AND ActionCode = 1 and TransactionYear = " + (DateTime.Now.Year + 1) + " ORDER BY CAST(OBJ_OrderBy as int)", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while(reader.Read()){
                    lbp5 lbp5_list = new lbp5();
                    lbp5_list.OBJ_ID = Convert.ToInt32(reader.GetValue(0));
                    lbp5_list.OBJ_Description = reader.GetString(1);
                    lbp5_list.OBJ_OrderBy = reader.GetString(2);
                    lbp5_list.OfficeID = Convert.ToInt32(reader.GetValue(3));
                    LBP5LIST.Add(lbp5_list);
                }
                return LBP5LIST;
            }
        }
        public IEnumerable<lbp5> lbp5_fs(int? OfficeID)
        {
            List<lbp5> LBP5LIST = new List<lbp5>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT * from tbl_R_LBP5_FS WHERE OfficeID = '" + OfficeID 
                                                + "' AND ActionCode = 1 and TransactionYear = " + (DateTime.Now.Year + 1) + " ORDER BY FS_OrderBy", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    lbp5 lbp5_list = new lbp5();
                    lbp5_list.FS_ID = Convert.ToInt32(reader.GetValue(0));
                    lbp5_list.FS_DESC = reader.GetString(1);
                    lbp5_list.FS_OrderBy = Convert.ToInt32(reader.GetValue(2));
                    lbp5_list.OfficeID = Convert.ToInt32(reader.GetValue(3));
                    LBP5LIST.Add(lbp5_list);
                }
                return LBP5LIST;
            }
        }
        public IEnumerable<lbp5> grPPA_MFO(int? OfficeID)
        {
            List<lbp5> LBP5LIST = new List<lbp5>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT PPA_MFO_ID, PPA_DESCRIPTION, OfficeID, PPA_MFO_COST, PPA_OrderBy FROM tbl_R_LBP5_PPA_MFO WHERE OfficeID = '" + OfficeID 
                                                    + "' and ActionCode = 1 and TransactionYear = " + (DateTime.Now.Year + 1) + " ORDER BY PPA_OrderBy, PPA_Description", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    lbp5 lbp5_list = new lbp5();
                    lbp5_list.PPA_MFO_ID = Convert.ToInt32(reader.GetValue(0));
                    lbp5_list.PPA_MFO_Description = reader.GetString(1);
                    lbp5_list.OfficeID = Convert.ToInt32(reader.GetValue(2));
                    lbp5_list.MFO_Cost = Convert.ToDouble(reader.GetValue(3));
                    lbp5_list.PPA_MFOOrderBy = Convert.ToInt32(reader.GetValue(4));
                    LBP5LIST.Add(lbp5_list);
                }
                return LBP5LIST;
            }
        }
        public IEnumerable<lbp5> grPPA_MFO_Breakdown(int? PPA_MFO_ID)
        {
            List<lbp5> LBP5LIST = new List<lbp5>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var OfficeID = @Account.UserInfo.Department;
                SqlCommand com = new SqlCommand(@"SELECT PPA_ID, PPA_MFO_ID, ISNULL(PPA_CodeRef, '(NULL)'), ISNULL(PPA_Description, '(NULL)'), ISNULL(COST, 0),
                                                  ISNULL(PPA_Output_Indicator, '(NULL)') ,isnull(PPA_Target, '(NULL)'), ISNULL(PPA_Implement_FROM,
                                                    '(NULL)'), ISNULL(PPA_Implement_TO, '(NULL)'), Sub_PPA, OrderBy 
                                                     FROM tbl_R_LBP5_PPA_Denomination WHERE PPA_MFO_ID = '" + PPA_MFO_ID 
                                                + "' AND ActionCode = 1 and TransactionYear = " + (DateTime.Now.Year + 1) + " ORDER BY OrderBy, PPA_ID", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    lbp5 lbp5_list = new lbp5();
                    lbp5_list.PPA_ID = Convert.ToInt32(reader.GetValue(0));
                    lbp5_list.PPA_MFO_ID = Convert.ToInt32(reader.GetValue(1));
                    lbp5_list.PPA_CodeRef = reader.GetString(2);
                    lbp5_list.PPA_Description = reader.GetString(3);
                    lbp5_list.PPA_Cost = Convert.ToDouble(reader.GetValue(4));
                    lbp5_list.PPA_Output_Indicator = reader.GetString(5);
                    lbp5_list.PPA_Target = reader.GetString(6);
                    lbp5_list.PPA_Implement_FROM = reader.GetString(7);
                    lbp5_list.PPA_Implement_TO = reader.GetString(8);
                    lbp5_list.Sub_PPA = Convert.ToInt32(reader.GetValue(9));
                    lbp5_list.PPA_OrderBy = Convert.ToInt32(reader.GetValue(10));
                    LBP5LIST.Add(lbp5_list);
                }
                return LBP5LIST;
            }
        }
        public double AccountCost(string OfficeID, int AccountID)
        {
            double AmountCost = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT
                        dbo.tbl_T_BMSBudgetProposal.ProposalAmount
                        FROM
                        dbo.tbl_R_BMSOfficePrograms
                        INNER JOIN dbo.tbl_R_BMSProgramAccounts ON dbo.tbl_R_BMSProgramAccounts.ProgramID = dbo.tbl_R_BMSOfficePrograms.ProgramID
                        INNER JOIN dbo.tbl_T_BMSBudgetProposal ON dbo.tbl_R_BMSProgramAccounts.AccountID = dbo.tbl_T_BMSBudgetProposal.AccountID AND dbo.tbl_R_BMSProgramAccounts.ProgramID = dbo.tbl_T_BMSBudgetProposal.ProgramID
                        WHERE
                        dbo.tbl_R_BMSOfficePrograms.OfficeID = '" + OfficeID + "' AND " +
                        "dbo.tbl_T_BMSBudgetProposal.AccountID = '" + AccountID + "' AND " +
                        "dbo.tbl_R_BMSOfficePrograms.ProgramYear = Year(GETDATE())+1 AND " +
                        "dbo.tbl_R_BMSProgramAccounts.ActionCode = 1 AND " +
                        "dbo.tbl_R_BMSOfficePrograms.ActionCode = 1 AND " +
                        "dbo.tbl_R_BMSProgramAccounts.AccountYear = Year(GETDATE())+1 AND " +
                        "dbo.tbl_T_BMSBudgetProposal.ProposalYear = Year(GETDATE())+1 AND " +
                        "dbo.tbl_T_BMSBudgetProposal.ProposalActionCode = 1", con);
                con.Open();
                AmountCost = Convert.ToDouble(com.ExecuteScalar());
                return AmountCost;

            }
        }
        public IEnumerable<programs> grProgramDelete_Info(string DataDropdown)
        {
            List<programs> ProgramList = new List<programs>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand("Select ProgramDescription, OfficeProgramID, OfficeName from tbl_R_BMSOfficePrograms LEFT JOIN tbl_R_BMSOffices ON tbl_R_BMSOfficePrograms.OfficeID = tbl_R_BMSOffices.OfficeID where ProgramYear= YEAR(GETDATE()) and tbl_R_BMSOfficePrograms.ActionCode = 10 and tbl_R_BMSOffices.OfficeID = '" + DataDropdown + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    programs office_list = new programs();
                    office_list.program_name = reader.GetString(0);
                    office_list.program_id = Convert.ToInt32(reader.GetValue(1));
                    office_list.OfficeName = reader.GetString(2);
                    ProgramList.Add(office_list);
                }
            }
            return ProgramList;
        }
    
    }
}