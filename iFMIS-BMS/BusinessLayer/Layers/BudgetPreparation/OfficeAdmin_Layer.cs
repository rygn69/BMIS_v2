using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Budget_Preparation;
using iFMIS_BMS.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using iFMIS_BMS.FMIS;
using System.Web.Mvc;
using System.Data;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;

using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
using iFMIS_BMS.BusinessLayer.Layers.Maintenance;

namespace iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation
{
    public class OfficeAdmin_Layer
    {
        int progIDV = 0;
        //decimal ProposalAllotedAmount = 0;
        int ooek = 0;
        ServiceSoapClient FMIS = new ServiceSoapClient();

        public IEnumerable<Approved> OfficeApprovedAccounts(int? prog_ID, int? proy_ID, int? ooe_id)
        {
            List<Approved> Accounts = new List<Approved>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                prog_ID = prog_ID == null ? 0 : prog_ID;
                proy_ID = proy_ID == null ? 0 : proy_ID;
                ooe_id = ooe_id == null ? 0 : ooe_id;

                SqlCommand query_account = new SqlCommand(@"SELECT DISTINCT
                        dbo.tbl_T_BMSBudgetProposal.ProposalAllotedAmount,
                        dbo.tbl_T_BMSBudgetProposal.ProposalAmount,
                        dbo.tbl_R_BMSObjectOfExpenditure.OOEAbrevation,
                        dbo.tbl_T_BMSBudgetProposal.ProposalYear,
                        dbo.tbl_R_BMSProgramAccounts.AccountName,
                        dbo.tbl_T_BMSBudgetProposal.AccountID,
                        dbo.tbl_R_BMSProgramAccounts.ProgramID,
                        dbo.tbl_T_BMSBudgetProposal.ProposalDenominationCode,
						dbo.tbl_R_BMSAccounts.AccountCode
                        FROM
                        dbo.tbl_T_BMSBudgetProposal
                        INNER JOIN dbo.tbl_R_BMSProgramAccounts ON dbo.tbl_T_BMSBudgetProposal.AccountID = dbo.tbl_R_BMSProgramAccounts.AccountID
                        INNER JOIN dbo.tbl_R_BMSObjectOfExpenditure ON dbo.tbl_R_BMSProgramAccounts.ObjectOfExpendetureID = dbo.tbl_R_BMSObjectOfExpenditure.OOEID
                        INNER JOIN dbo.tbl_R_BMSOfficePrograms ON dbo.tbl_R_BMSOfficePrograms.ProgramID = dbo.tbl_R_BMSProgramAccounts.ProgramID
						INNER JOIN dbo.tbl_R_BMSAccounts ON  dbo.tbl_R_BMSProgramAccounts.AccountID=dbo.tbl_R_BMSAccounts.AccountID
                        WHERE
                        dbo.tbl_R_BMSOfficePrograms.ActionCode = 1 and dbo.tbl_T_BMSBudgetProposal.ProposalStatusCommittee = 1 and dbo.tbl_R_BMSObjectOfExpenditure.OOEID = '" + ooe_id + "' and dbo.tbl_R_BMSOfficePrograms.OfficeID = '" + Account.UserInfo.Department.ToString() + "' and dbo.tbl_T_BMSBudgetProposal.ProposalYear = '" + proy_ID + "' and dbo.tbl_R_BMSProgramAccounts.ActionCode = 1 and dbo.tbl_T_BMSBudgetProposal.ProgramID = " + prog_ID + " and dbo.tbl_R_BMSProgramAccounts.AccountYear = '" + proy_ID + "' ORDER BY AccountName", con);

                con.Open();
                SqlDataReader reader_account = query_account.ExecuteReader();
                while (reader_account.Read())
                {
                    Approved data = new Approved();
                    data.ProposalAllotedAmount = Convert.ToDouble(reader_account.GetValue(0));
                    data.ProposalAmount = Convert.ToDouble(reader_account.GetValue(1));
                    data.OOE = reader_account.GetString(2);
                    data.ProposalYear = Convert.ToInt32(reader_account.GetValue(3));
                    data.AccountName = reader_account.GetString(4);
                    data.AccountID = Convert.ToInt32(reader_account.GetValue(5));
                    data.ProgramID = Convert.ToInt32(reader_account.GetValue(6));
                    data.ProposalDenominationCode = Convert.ToInt32(reader_account.GetValue(7));
                    data.AccountCode = Convert.ToInt32(reader_account.GetValue(8));
                    Accounts.Add(data);
                }
            }
            return Accounts;
        }
        public IEnumerable<ProposedPositionsSummaryModel> getProposedPositionsOfficeLevel(int? YearOf)
        {
            List<ProposedPositionsSummaryModel> lst = new List<ProposedPositionsSummaryModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand query_account = new SqlCommand(@"sp_bms_OfficeAdminGetProposedPositions " + YearOf + "," + Account.UserInfo.Department + "", con);

                con.Open();
                SqlDataReader reader_account = query_account.ExecuteReader();
                while (reader_account.Read())
                {
                    ProposedPositionsSummaryModel data = new ProposedPositionsSummaryModel();
                    data.Position = reader_account.GetValue(0).ToString();
                    data.Status = reader_account.GetValue(1).ToString();
                    data.TotalProposed = Convert.ToDouble(reader_account.GetValue(2));
                    data.TotalApproved = Convert.ToDouble(reader_account.GetValue(3));
                   

                    lst.Add(data);
                }
            }
            return lst;
        }
        public string MarkForCreation(int ProposedItemID, string Remark)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSSubmittedForFundingData set GroupTag='For Creation',StatusHR=1,Remark = '" + Remark.ToString().Replace("'", "''") + "' where SeriesID = " + ProposedItemID + "", con);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
       
        public string MarkForFunding(int ProposedItemID, string Remark)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSSubmittedForFundingData set GroupTag='For Funding',StatusHR=1,Remark = '" + Remark.ToString().Replace("'", "''") + "' where SeriesID = " + ProposedItemID + "", con);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        public string ApproveProposedPositions(int Yearof, int OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_bms_ApproveProposedPositions "+Yearof+","+OfficeID+"", con);
                try
                {
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string MarkVerified(int ProposedItemID, string Remark)
        {
            var OfficeLevel = "";
            if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                OfficeLevel = "StatusHR";
            }
            else if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                OfficeLevel = "StatusBudget";
            }
            else
            {
                OfficeLevel = "StatusLFC";
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSSubmittedForFundingData set " + OfficeLevel + "=1,Remark = '" + Remark.ToString().Replace("'", "''") + "' where SeriesID = " + ProposedItemID + "", con);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string MarkRemove(int ProposedItemID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand com = new SqlCommand(@"Update tbl_R_BMSProposedNewItem set ActionCode=0 where ProposedItemID = (Select ProposedItemID from tbl_R_BMSSubmittedForFundingData where SeriesID =" + ProposedItemID + ") "+""
                //                                +" Update tbl_R_BMSSubmittedForFundingData set Grouptag = 'Inactive' where SeriesID = " + ProposedItemID + " "+""
                //                                +"", con);
                
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSProposedNewItem set ActionCode=0 where ProposedItemID = (Select ProposedItemID from tbl_R_BMSSubmittedForFundingData where SeriesID =" + ProposedItemID + ")", con);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();

                    //SqlCommand updateProp = new SqlCommand(@"exec sp_bms_UpdateConsolidated_PerOffice " + ProposedItemID + "," + Account.UserInfo.eid + "", con);
                    //updateProp.ExecuteNonQuery();

                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public double GetProposedTotalAmount(int Yearof, int OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_bms_ProposedTotalAllOffice "+Yearof+","+OfficeID+"", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }
        public double GetProjectedTotalAmount(int Yearof, int OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_bms_ProjectedTotalAllOffice " + Yearof + "," + OfficeID + "", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar().ToString());
            }
        }
        
        
        public string MarkUnVerified(int ProposedItemID, string Remark)
        {
            var OfficeLevel = "";
            if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                OfficeLevel = "StatusHR = 2, GroupTag = ''";
            }
            else if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                OfficeLevel = "StatusBudget  = 2";
            }
            else
            {
                OfficeLevel = "StatusLFC = 2";
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSSubmittedForFundingData set " + OfficeLevel + " where SeriesID = " + ProposedItemID + "", con);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string CheckIfVerifiedByOtherLevel(int ProposedItemID, string Remark)
        {
            var OfficeLevel = "";
            if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                OfficeLevel = "StatusBudget";
            }
            else if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                OfficeLevel = "StatusLFC";
            }
            else
            {
                return "0";
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select " + OfficeLevel + " from tbl_R_BMSSubmittedForFundingData where SeriesID = " + ProposedItemID + "", con);
                    con.Open();
                    com.ExecuteScalar();
                    return com.ExecuteScalar().ToString();
            }
        }
        
        public string UpdateProposedDate(string DateValue, int SeriesID, int OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"UPDATE tbl_R_BMSProposedNewItem SET AppointmentDateEffectivity='" + DateValue
                                            + "' WHERE ProposedItemID =(Select ProposedItemId from tbl_R_BMSSubmittedForFundingData where SeriesID = " + SeriesID + ")", con);
                //SqlCommand com2 = new SqlCommand(@"sp_BMS_getProposedItemForUpdate " + DateTime.Now.Year + "," + OfficeID + "," + SeriesID + "", con);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    //com2.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string UpdateProposedDateVacant(string DateValue, int SeriesID, int OfficeID,string isCasual)
        {
            var ApprovedProposedPositionID = "";
            var ReturnMessage = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select SeriesID from tbl_R_BMSSubmittedForFundingData where StatusLFC = 1 and OfficeID = " + OfficeID + "", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        if (ApprovedProposedPositionID == "")
                        {
                            ApprovedProposedPositionID = ApprovedProposedPositionID + reader.GetValue(0).ToString();
                        }
                        else
                        {
                            ApprovedProposedPositionID = ApprovedProposedPositionID + "," + reader.GetValue(0).ToString();
                        }
                    }
                    if (ApprovedProposedPositionID == "")
                    {
                        ApprovedProposedPositionID = "''";
                    }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand comUpdateStatus = new SqlCommand(@"Update tbl_R_BMSSubmittedForFundingData set StatusLFC = 2 where StatusLFC = 1 and OfficeID = " + OfficeID + "", con);
                con.Open();
                comUpdateStatus.ExecuteNonQuery();
            }

            if (isCasual == "No")
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"IF EXISTS (SELECT PlantillaItemNoID FROM tbl_R_BMSVacantApprovedEffectivityDate WHERE PlantillaItemNoID = " + SeriesID + ")" + ""
                                                    + " BEGIN" + ""
                                                    + " Update tbl_R_BMSVacantApprovedEffectivityDate set ApprovedEffectivityDate = '" + DateValue + "' where PlantillaItemNoID = " + SeriesID
                                                    + " END " + ""
                                                    + "ELSE " + ""
                                                    + "BEGIN " + ""
                                                    + " Insert into tbl_R_BMSVacantApprovedEffectivityDate values(" + SeriesID + ",'" + DateValue + "')" + ""
                                                    + " END", con);
                    try
                    {
                        con.Open();
                        com.ExecuteNonQuery();
                        ReturnMessage = UpdatePSAmountHistory(OfficeID);
                    }
                    catch (Exception ex)
                    {
                        ReturnMessage = ex.Message;
                    }
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"UPDATE tbl_R_BMSVacantAndTransferedCasual SET SalaryEffectivityDate='" + DateValue + "' WHERE SeriesID=' " + SeriesID + "'", con);
                    try
                    {
                        con.Open();
                        com.ExecuteNonQuery();
                        ReturnMessage = UpdatePSAmountHistory(OfficeID);
                    }
                    catch (Exception ex)
                    {
                        ReturnMessage = ex.Message;
                    }
                }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand comUpdateStatus = new SqlCommand(@"Update tbl_R_BMSSubmittedForFundingData set StatusLFC = 1 where SeriesID in(" + ApprovedProposedPositionID + ")", con);
                con.Open();
                comUpdateStatus.ExecuteNonQuery();
            }
            return ReturnMessage;
        }
        public string UpdatePSAmountHistory(int OfficeID)
        {
            try
            {
                List<CopyProposedAmountModel> prog = new List<CopyProposedAmountModel>();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select a.ProposalID,d.OfficeID,c.OOEID,c.AccountName,a.ProposalAmount,E.OOEAbrevation, d.OfficeName 
                                                    from tbl_T_BMSBudgetProposal as a 
                                                    INNER JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID and b.ProgramYear = a.ProposalYear and b.ActionCode = ProposalActionCode
                                                    INNER JOIN tbl_R_BMSProposedAccounts as c on c.ProgramID = a.ProgramID and c.AccountID = a.AccountID and C.ProposalYear = a.ProposalYear and C.ActionCode = a.ProposalActionCode
                                                    INNER JOIN tbl_R_BMSObjectOfExpenditure as e on e.OOEID = c.OOEID
                                                    INNER JOIN tbl_R_BMSOffices as d on d.OfficeID = b.OfficeID
                                                    where a.ProposalActionCode = 1 and a.ProposalYear = year(getdate()) + 1 and d.OfficeID = '" + OfficeID + "' ORDER BY d.OfficeID, e.OOEID", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CopyProposedAmountModel emp = new CopyProposedAmountModel();
                        emp.ProposalID = Convert.ToInt32(reader.GetValue(0));
                        emp.OfficeID = reader.GetInt32(1);
                        emp.OOEID = reader.GetInt32(2);
                        emp.AccountName = reader.GetValue(3).ToString();
                        emp.ProposedAmount = Convert.ToDouble(reader.GetValue(4).ToString());
                        emp.ApprovedAmount = getApprovedAmount(Convert.ToInt32(reader.GetValue(0)));
                        emp.OOEName = reader.GetValue(5).ToString();
                        emp.OfficeName = reader.GetValue(6).ToString();

                        prog.Add(emp);
                    }
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select a.ProposalID,d.OfficeID,c.ObjectOfExpendetureID,c.AccountName,a.ProposalAmount,E.OOEAbrevation, d.OfficeName from tbl_T_BMSBudgetProposal as a 
                                                    INNER JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID and b.ProgramYear = a.ProposalYear and b.ActionCode = ProposalActionCode
                                                    INNER JOIN tbl_R_BMSProgramAccounts as c on c.ProgramID = a.ProgramID and c.AccountID = a.AccountID and C.AccountYear = a.ProposalYear and C.ActionCode = a.ProposalActionCode
                                                    INNER JOIN tbl_R_BMSObjectOfExpenditure as e on e.OOEID = c.ObjectOfExpendetureID
                                                    INNER JOIN tbl_R_BMSOffices as d on d.OfficeID = b.OfficeID
                                                    where a.ProposalActionCode = 1 and a.ProposalYear = year(getdate()) + 1 and d.OfficeID = '" + OfficeID + "' ORDER BY OfficeID, OOEID", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CopyProposedAmountModel emp = new CopyProposedAmountModel();
                        emp.ProposalID = Convert.ToInt32(reader.GetValue(0));
                        emp.OfficeID = reader.GetInt32(1);
                        emp.OOEID = reader.GetInt32(2);
                        emp.AccountName = reader.GetValue(3).ToString();
                        emp.ProposedAmount = Convert.ToDouble(reader.GetValue(4).ToString());
                        emp.ApprovedAmount = getApprovedAmount(Convert.ToInt32(reader.GetValue(0)));
                        emp.OOEName = reader.GetValue(5).ToString();
                        emp.OfficeName = reader.GetValue(6).ToString();

                        prog.Add(emp);
                    }
                }
                foreach (var item in prog)
                {
                    if (item.ApprovedAmount != 0)
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand(@"insert into tbl_R_AmountHistory values('" + item.ProposalID + "','" + Account.UserInfo.eid + "','" + item.ApprovedAmount + "',GETDATE())", con);
                            con.Open();
                            com.ExecuteNonQuery();
                        }
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }
        public double getApprovedAmount(int ProposalID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"dbo.sp_bms_getComputationTotalAdminView " + ProposalID + "", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar());
            }
        }
        
        public string MarkForFunding(int ProposedItemID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSSubmittedForFundingData set GroupTag='For Creation',StatusHR=1 where SeriesID = " + ProposedItemID + "", con);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }


        public IEnumerable<ForFundingModel> grGetProposedVerifiedPosition(int? YearOf, int? OfficeID)
        {
            var OfficeLevel = "";
            if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                OfficeLevel = "StatusHR";
            }
            else if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                OfficeLevel = "StatusBudget";
            }
            else
            {
                OfficeLevel = "StatusLFC";
            }
            List<ForFundingModel> ForFundingList = new List<ForFundingModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_account = new SqlCommand(@"select a.SeriesID, a.Position,a.Salary, c.sg,a.Remark,a.GroupTag, b.AppointmentDateEffectivity from tbl_R_BMSSubmittedForFundingData as a 
                                                            inner	 join tbl_R_BMSProposedNewItem as b on b.ProposedItemID = a.ProposedItemID and b.ActionCode = 2
                                                            INNER JOIN pmis.dbo.RefsPositions as c on c.PositionCode = b.PositionID
                                                            where a.OfficeID = '" + OfficeID + "' and a.yearof = '" + YearOf + "' and a." + OfficeLevel + " = 1", con);

                con.Open();
                SqlDataReader reader = query_account.ExecuteReader();
                while (reader.Read())
                {
                    ForFundingModel ForFunding = new ForFundingModel();
                    ForFunding.ProposedItemID = Convert.ToInt32(reader.GetValue(0));
                    ForFunding.Position = reader.GetValue(1).ToString();
                    ForFunding.Salary = Convert.ToDouble(reader.GetValue(2));
                    ForFunding.SG = reader.GetValue(3).ToString();
                    ForFunding.Remark = reader.GetValue(4).ToString();
                    ForFunding.GroupTag = reader.GetValue(5).ToString();
                    ForFunding.SalaryEffectivityDate = reader.GetDateTime(6).ToShortDateString();
                    ForFunding.GroupBY = "Proposed Position";
                    ForFundingList.Add(ForFunding);
                }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_account = new SqlCommand(@"select a.SeriesID, a.Position,a.Salary,a.Remark,a.GroupTag, b.AppointmentDateEffectivity from tbl_R_BMSSubmittedForFundingData as a 
                                                            inner join tbl_R_BMSProposedNewItem as b on b.ProposedItemID = a.ProposedItemID and b.ActionCode = 2 and b.PositionID is Null
                                                            where a.OfficeID = '" + OfficeID + "' and a.yearof = '" + YearOf + "' and a." + OfficeLevel + " = 1", con);

                con.Open();
                SqlDataReader reader = query_account.ExecuteReader();
                while (reader.Read())
                {
                    ForFundingModel ForFunding = new ForFundingModel();
                    ForFunding.ProposedItemID = Convert.ToInt32(reader.GetValue(0));
                    ForFunding.Position = reader.GetValue(1).ToString();
                    ForFunding.Salary = Convert.ToDouble(reader.GetValue(2));
                    ForFunding.Remark = reader.GetValue(3).ToString();
                    ForFunding.GroupTag = reader.GetValue(4).ToString();
                    ForFunding.SalaryEffectivityDate = reader.GetDateTime(5).ToShortDateString();
                    ForFunding.SG = "";
                    ForFunding.GroupBY = "Proposed Casual";
                    ForFundingList.Add(ForFunding);
                }
            }
            return ForFundingList;
        }
        public IEnumerable<ForFundingModel> grGetProposedPosition(int? YearOf, int? OfficeID, string isCasual)
        {
            var OfficeLevel = "";
            if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                OfficeLevel = "a.StatusHR = 2";
            }
            else if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                OfficeLevel = "a.StatusHR = 1 and a.StatusBudget = 2";
            }
            else
            {
                OfficeLevel = "a.StatusHR = 1 and a.StatusBudget = 1 and StatusLFC = 2";
            }
            List<ForFundingModel> ForFundingList = new List<ForFundingModel>();
            if (isCasual == "Yes")
            {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                    SqlCommand query_account = new SqlCommand(@"select a.SeriesID, a.Position,a.Salary,a.Remark,GroupTag, b.AppointmentDateEffectivity from tbl_R_BMSSubmittedForFundingData as a 
                                                            inner join tbl_R_BMSProposedNewItem as b on b.ProposedItemID = a.ProposedItemID and b.ActionCode = 2 and b.PositionID is Null
                                                            where a.OfficeID = '" + OfficeID + "' and a.yearof = '" + YearOf + "' and " + OfficeLevel + "", con);

                con.Open();
                SqlDataReader reader = query_account.ExecuteReader();
                while (reader.Read())
                {
                    ForFundingModel ForFunding = new ForFundingModel();
                    ForFunding.ProposedItemID = Convert.ToInt32(reader.GetValue(0));
                    ForFunding.Position = reader.GetValue(1).ToString();
                    ForFunding.Salary = Convert.ToDouble(reader.GetValue(2));
                        ForFunding.Remark = reader.GetValue(3).ToString();
                        ForFunding.GroupTag = reader.GetValue(4).ToString();
                        ForFunding.SalaryEffectivityDate = reader.GetDateTime(5).ToShortDateString();
                        ForFunding.SG = "";
                        ForFunding.GroupBY = "Proposed Casual";
                    ForFundingList.Add(ForFunding);
                }
            }
            }
            else
            {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                    SqlCommand query_account = new SqlCommand(@"select a.SeriesID, a.Position,a.Salary, c.sg,a.Remark,a.GroupTag, b.AppointmentDateEffectivity from tbl_R_BMSSubmittedForFundingData as a 
                                                            inner	 join tbl_R_BMSProposedNewItem as b on b.ProposedItemID = a.ProposedItemID and b.ActionCode = 2
                                                            INNER JOIN pmis.dbo.RefsPositions as c on c.PositionCode = b.PositionID
                                                            where a.OfficeID = '" + OfficeID + "' and a.GroupTag = 'For Funding' and  a.yearof = '" + YearOf + "' and " + OfficeLevel + " ORDER BY C.sg DESC", con);

                con.Open();
                SqlDataReader reader = query_account.ExecuteReader();
                while (reader.Read())
                {
                    ForFundingModel ForFunding = new ForFundingModel();
                    ForFunding.ProposedItemID = Convert.ToInt32(reader.GetValue(0));
                    ForFunding.Position = reader.GetValue(1).ToString();
                    ForFunding.Salary = Convert.ToDouble(reader.GetValue(2));
                        ForFunding.SG = reader.GetValue(3).ToString();
                        ForFunding.Remark = reader.GetValue(4).ToString();
                        ForFunding.GroupTag = reader.GetValue(5).ToString();
                        ForFunding.SalaryEffectivityDate = reader.GetDateTime(6).ToShortDateString();
                        ForFunding.GroupBY = "Proposed Position";
                    ForFundingList.Add(ForFunding);
                }
            }
            }
            return ForFundingList;
        }
        
        public decimal ActualAmount(int? prog_id, int AccountCode, int? proposalYear)
        {
            decimal Actual_Year = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand actual = new SqlCommand(@"SELECT isnull(SUM(h.amount), 0) as Actual_Year FROM fmis.dbo.tblAMIS_SE_byYearOffice as h WHERE h.rcenter = " + Account.UserInfo.Department.ToString() + " AND h.AccountCode = '" + AccountCode + "' and h.year_ = '" + proposalYear + "' - 2", con);
                con.Open();
                SqlDataReader actual_reader = actual.ExecuteReader();
                while (actual_reader.Read())
                {
                    Actual_Year = Convert.ToDecimal(actual_reader.GetValue(0));
                }
            }
            return Actual_Year;
        }
        public IEnumerable<Utilization> Utilization(int? ProgramID, int? ProposalYear, int? OfficeID, int? OOEID)
        {
            ProgramID = ProgramID == null ? 0 : ProgramID;
            ProposalYear = ProposalYear == null ? 0 : ProposalYear;
            OfficeID = OfficeID == null ? 0 : OfficeID;
            OOEID = OOEID == null ? 0 : OOEID;

            List<Utilization> UtilizationList = new List<Utilization>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            { //Stored proc update on 7/20/2018 - xXx
                SqlCommand com = new SqlCommand(@" exec sp_BMS_Utilization " + OfficeID + "," + ProgramID + "," + ProposalYear + "," + OOEID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while(reader.Read()){
                    Utilization data = new Utilization();
                    data.ProposalID = Convert.ToInt32(reader.GetValue(1));
                    data.AccountID = Convert.ToInt32(reader.GetValue(0));
                    data.AccountCode = Convert.ToInt32(reader.GetValue(3));
                    data.AccountName = reader.GetString(2);
                    data.Actual5 = Convert.ToDouble(reader.GetValue(4));
                    data.Approve5 = Convert.ToDouble(reader.GetValue(5));
                    data.Actual4 = Convert.ToDouble(reader.GetValue(6));
                    data.Approve4 = Convert.ToDouble(reader.GetValue(7));
                    data.Actual3 = Convert.ToDouble(reader.GetValue(8));
                    data.Approve3 = Convert.ToDouble(reader.GetValue(9));
                    data.Actual2 = Convert.ToDouble(reader.GetValue(10));
                    data.Approve2 = Convert.ToDouble(reader.GetValue(11));
                    data.Actual1 = Convert.ToDouble(reader.GetValue(12));
                    data.Approve1 = Convert.ToDouble(reader.GetValue(13));
                    data.ProposalAmount = Convert.ToDouble(reader.GetValue(14));
                    data.ProgramID = Convert.ToInt32(reader.GetValue(15));
                    UtilizationList.Add(data);
                }
                return UtilizationList;
            }
        }
        public IEnumerable<SummaryAllOffices> SelectAllOffice(int? ProposalYear)
        {
            List<SummaryAllOffices> data = new List<SummaryAllOffices>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_SummaryofAllOffices "+ProposalYear+"", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SummaryAllOffices dataList = new SummaryAllOffices();
                    dataList.ProposalID = Convert.ToInt32(reader.GetValue(1).ToString() + "" + reader.GetValue(2).ToString());
                    dataList.OfficeName = Convert.ToString(reader.GetValue(0));
                    dataList.OfficeID = Convert.ToInt32(reader.GetValue(1));
                    dataList.OOEID = Convert.ToInt32(reader.GetValue(2));
                    dataList.OOEFullName = Convert.ToString(reader.GetValue(3));
                    dataList.PastYear = Convert.ToDouble(reader.GetValue(4));
                    dataList.TotalCost = Convert.ToDouble(reader.GetValue(5));
                    //dataList.OOEFullName = Convert.ToString(reader.GetValue(5));                    
                    var diff = dataList.TotalCost - dataList.PastYear;
                    var div = diff / dataList.PastYear;
                    dataList.PercentageIncreaseDecrease = div;

                    data.Add(dataList);
                }
            }
            return data;
        }
        #region Old Office Level Approved Tab Unused
        //        public IEnumerable<Approved> BudgetInCharge_ApproveAccounts(int? prog_ID, int? OOE, int? proposalYear)
        //        {
        //            prog_ID = prog_ID == null ? 0 : prog_ID;
        //            OOE = OOE == null ? 0 : OOE;
        //            proposalYear = proposalYear == null ? 0 : proposalYear;

        //            List<Approved> Accounts = new List<Approved>();
        //            using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //            {
        //                SqlCommand com = new SqlCommand(@"WITH x AS
        //( SELECT DISTINCT a.AccountID, a.ProposalAllotedAmount as Current_Year FROM dbo.tbl_T_BMSBudgetProposal AS a
        //				LEFT JOIN dbo.tbl_R_BMSOfficePrograms as b ON b.ProgramID = a.ProgramID
        //				LEFT JOIN dbo.tbl_R_BMSAccounts as c ON c.FMISAccountCode = a.AccountID
        //				WHERE b.OfficeID = '" + Account.UserInfo.Department.ToString() + "' and a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proposalYear + "' - 1 and a.ProposalActionCode = '1' " +
        //") SELECT DISTINCT d.ProposalID, d.AccountID, d.ProgramID, d.ProposalYear, isnull(x.Current_Year, 0) as Current_Year, d.ProposalAllotedAmount as Budget_Year, isnull(d.ProposalDenominationCode, 0) as ProposalDenominationCode, f.AccountName, e.AccountCode, d.ProposalAmount " +
        //                "FROM dbo.tbl_T_BMSBudgetProposal as d" +
        //                " LEFT JOIN dbo.tbl_R_BMSAccounts as e ON e.FMISAccountCode = d.AccountID" +
        //                " LEFT JOIN dbo.tbl_R_BMSProgramAccounts as f ON f.AccountID = d.AccountID" +
        //                " LEFT JOIN x as x ON x.AccountID = d.AccountID" +
        //                " WHERE d.ProposalStatusCommittee = 1 and d.ProposalYear = '" + proposalYear + "' and d.ProposalActionCode = '1' and d.ProgramID = '" + prog_ID + "' and f.ActionCode = '1' and f.AccountYear = '" + proposalYear + "' and f.ObjectOfExpendetureID ='" + OOE + "'", con);

        //                con.Open();

        //                SqlDataReader reader = com.ExecuteReader();
        //                while (reader.Read())
        //                {
        //                    Approved data = new Approved();
        //                    data.ProposalID = Convert.ToInt32(reader.GetValue(0));
        //                    data.AccountID = Convert.ToInt32(reader.GetValue(1));
        //                    data.ProgramID = Convert.ToInt32(reader.GetValue(2));
        //                    data.ProposalYear = Convert.ToInt32(reader.GetValue(3));
        //                    data.Current_Year = Convert.ToDecimal(reader.GetValue(4));
        //                    data.Budget_Year = Convert.ToDecimal(reader.GetValue(5));
        //                    data.ProposalDenominationCode = Convert.ToInt32(reader.GetValue(6));
        //                    data.AccountName = reader.GetString(7);
        //                    data.AccountCode = Convert.ToInt32(reader.GetValue(8));
        //                    //data.Actual_Year = ActualAmount(prog_ID, data.AccountCode, proposalYear);
        //                    data.ProposalAmount = Convert.ToInt32(reader.GetValue(9));
        //                    Accounts.Add(data);

        //                }
        //            }
        //            return Accounts;
        //        } 
        #endregion
        public IEnumerable<Approved> BudgetInCharge_ApproveAccounts(int? prog_ID, int? OOE, int? proposalYear)
        {
            prog_ID = prog_ID == null ? 0 : prog_ID;
            OOE = OOE == null ? 0 : OOE;
            proposalYear = proposalYear == null ? 0 : proposalYear;

            List<Approved> Accounts = new List<Approved>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_getDataApprovedTabOfficeAdmin " + proposalYear + ", " + Account.UserInfo.Department + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Approved data = new Approved();
                    data.AccountID = Convert.ToInt32(reader.GetValue(6));
                    data.ProgramID = Convert.ToInt32(reader.GetValue(8));
                    data.Current_Year = Convert.ToDecimal(reader.GetValue(2));
                    data.Budget_Year = Convert.ToDecimal(reader.GetValue(3));
                    data.AccountName = reader.GetString(0);
                    data.AccountCode = reader.GetValue(1)== DBNull.Value ? 0 : Convert.ToInt32(reader.GetValue(1));
                    data.ProposalAmount = Convert.ToInt32(reader.GetValue(4));
                    data.OOE = reader.GetValue(7).ToString();
                    Accounts.Add(data);
                }
            }
            return Accounts;
        }
        public IEnumerable<Approved> BudgetCommittee_ApproveAccounts(int? prog_ID, int? OOE, int? proposalYear, int? OfficeID, string OfficeLevel,int? regularaipid)

        {
            prog_ID = prog_ID == null ? 0 : prog_ID;
            OOE = OOE == null ? 0 : OOE;
            proposalYear = proposalYear == null ? 0 : proposalYear;
            OfficeID = OfficeID == null ? 0 : OfficeID;
            List<Approved> Accounts = new List<Approved>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (regularaipid == 1)//annual budget
                {
                    SqlCommand com = new SqlCommand(@"WITH x AS
                ( SELECT DISTINCT a.AccountID, a.ProposalAllotedAmount as Current_Year, b.OfficeID FROM dbo.tbl_T_BMSBudgetProposal AS a
				LEFT JOIN dbo.tbl_R_BMSOfficePrograms as b ON b.ProgramID = a.ProgramID
				LEFT JOIN dbo.tbl_R_BMSAccounts as c ON c.FMISAccountCode = a.AccountID
				WHERE b.OfficeID = '" + OfficeID + "' and a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proposalYear + "' - 1 and a.ProposalActionCode = '1' " +
                    ") SELECT DISTINCT d.ProposalID, d.AccountID, d.ProgramID, d.ProposalYear, isnull(x.Current_Year, 0) as Current_Year, isnull(d.ProposalAmount,0) as Budget_Year, isnull(d.ProposalDenominationCode, 0) as ProposalDenominationCode, f.AccountName, isnull(e.AccountCode,0), d.ProposalAllotedAmount as Approved_Budget, x.OfficeID as OfficeID " +
                    "FROM dbo.tbl_T_BMSBudgetProposal as d" +
                    " LEFT JOIN dbo.tbl_R_BMSAccounts as e ON e.FMISAccountCode = d.AccountID" +
                    " LEFT JOIN dbo.tbl_R_BMSProgramAccounts as f ON f.ProgramID = d.ProgramID and f.AccountID = d.AccountID" +
                    " LEFT JOIN x as x ON x.AccountID = d.AccountID" +
                    " WHERE d." + OfficeLevel + " = 1 and d.ProposalYear = '" + proposalYear + "' and d.ProposalActionCode = '1' and d.ProgramID = '" + prog_ID + "' and f.ActionCode = '1' and f.AccountYear = '" + proposalYear + "' and f.ObjectOfExpendetureID ='" + OOE + "' and [ProposalStatusCommittee]=1", con);

                    con.Open();

                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Approved data = new Approved();
                        data.ProposalID = Convert.ToInt32(reader.GetValue(0));
                        data.AccountID = Convert.ToInt32(reader.GetValue(1));
                        data.ProgramID = Convert.ToInt32(reader.GetValue(2));
                        data.ProposalYear = Convert.ToInt32(reader.GetValue(3));
                        data.Current_Year = Convert.ToDecimal(reader.GetValue(4));
                        data.Budget_Year = Convert.ToDecimal(reader.GetValue(5));
                        data.ProposalDenominationCode = Convert.ToInt32(reader.GetValue(6));
                        data.AccountName = reader.GetString(7);
                        data.AccountCode = Convert.ToInt32(reader.GetValue(8));
                        data.ApprovedYear = Convert.ToDouble(reader.GetValue(9));
                        //data.OfficeID = Convert.ToInt32(reader.GetValue(10));
                        String AccountString = Convert.ToInt32(reader.GetValue(1)).ToString();
                        String programID = prog_ID.ToString();
                        data.Actual_Year = getPastYearAmount(proposalYear, AccountString, OfficeID, programID, prog_ID);
                        //data.Actual_Year = ActualAmount(prog_ID, data.AccountCode, proposalYear);
                        //data.SlashAmount = getSlashAmount(reader.GetInt64(0));
                        //if (data.SlashAmount == 0)
                        //{
                        //    data.SlashAmount = Convert.ToInt32(reader.GetValue(5));
                        //}
                        Accounts.Add(data);
                    }
                    reader.Close();
                }
                else //supplemental budget
                {
                    SqlCommand com = new SqlCommand(@"WITH x AS
                ( SELECT DISTINCT a.AccountID, a.ProposalAllotedAmount as Current_Year, b.OfficeID FROM dbo.tbl_T_BMSBudgetProposal AS a
				LEFT JOIN dbo.tbl_R_BMSOfficePrograms as b ON b.ProgramID = a.ProgramID
				LEFT JOIN dbo.tbl_R_BMSAccounts as c ON c.FMISAccountCode = a.AccountID
				WHERE b.OfficeID = '" + OfficeID + "' and a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proposalYear + "' - 1 and a.ProposalActionCode = '1' " +
                   ") SELECT DISTINCT d.ProposalID, d.AccountID, d.ProgramID, d.ProposalYear, isnull(x.Current_Year, 0) as Current_Year, isnull(d.ProposalAmount,0) as Budget_Year, isnull(d.ProposalDenominationCode, 0) as ProposalDenominationCode, f.AccountName, isnull(e.AccountCode,0), d.[SupplementalAmount_approve] as Approved_Budget, x.OfficeID as OfficeID " +
                   "FROM dbo.tbl_T_BMSBudgetProposal as d" +
                   " LEFT JOIN dbo.tbl_R_BMSAccounts as e ON e.FMISAccountCode = d.AccountID" +
                   " LEFT JOIN dbo.tbl_R_BMSProgramAccounts as f ON f.ProgramID = d.ProgramID and f.AccountID = d.AccountID" +
                   " LEFT JOIN x as x ON x.AccountID = d.AccountID" +
                   " WHERE d." + OfficeLevel + " = 1 and d.ProposalYear = '" + proposalYear + "' and d.ProposalActionCode = '1' and d.ProgramID = '" + prog_ID + "' and f.ActionCode = '1' and f.AccountYear = '" + proposalYear + "' and f.ObjectOfExpendetureID ='" + OOE + "' and [ProposalStatusCommittee]=1  and isnull([SupplementalAmount_approve],0) != 0", con);

                    con.Open();

                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Approved data = new Approved();
                        data.ProposalID = Convert.ToInt32(reader.GetValue(0));
                        data.AccountID = Convert.ToInt32(reader.GetValue(1));
                        data.ProgramID = Convert.ToInt32(reader.GetValue(2));
                        data.ProposalYear = Convert.ToInt32(reader.GetValue(3));
                        data.Current_Year = Convert.ToDecimal(reader.GetValue(4));
                        data.Budget_Year = Convert.ToDecimal(reader.GetValue(5));
                        data.ProposalDenominationCode = Convert.ToInt32(reader.GetValue(6));
                        data.AccountName = reader.GetString(7);
                        data.AccountCode = Convert.ToInt32(reader.GetValue(8));
                        data.ApprovedYear = Convert.ToDouble(reader.GetValue(9));
                        //data.OfficeID = Convert.ToInt32(reader.GetValue(10));
                        String AccountString = Convert.ToInt32(reader.GetValue(1)).ToString();
                        String programID = prog_ID.ToString();
                        data.Actual_Year = getPastYearAmount(proposalYear, AccountString, OfficeID, programID, prog_ID);
                        //data.Actual_Year = ActualAmount(prog_ID, data.AccountCode, proposalYear);
                        //data.SlashAmount = getSlashAmount(reader.GetInt64(0));
                        //if (data.SlashAmount == 0)
                        //{
                        //    data.SlashAmount = Convert.ToInt32(reader.GetValue(5));
                        //}
                        Accounts.Add(data);
                    }
                    reader.Close();
                }
                SqlCommand PNA = new SqlCommand(@"SELECT a.AccountID, a.AccountName, 
                    a.AccountCode, b.ProposalID, b.ProposalAmount, b.ProposalAllotedAmount,
                    b.ProposalStatusHR, b.ProposalStatusInCharge, b.ProposalStatusCommittee, 
                    b.ProposalDenominationCode, a.OOEID, c.Remarks  FROM tbl_R_BMSProposedAccounts as a " +
                    "LEFT JOIN tbl_T_BMSBudgetProposal as b ON a.AccountID = b.AccountID and a.ProposalYear=b.ProposalYear and b.ProgramID=a.ProgramID " +
                    "LEFT JOIN tbl_R_BMSProposalRemark as c ON c.ProposalID = b.ProposalID " +
                    "WHERE a.OfficeID = '" + OfficeID + "'and a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proposalYear + "' and a.ActionCode = 1  and b.ProposalActionCode = 1 and [ProposalStatusCommittee] = 1", con);
                SqlDataReader reader_PNA = PNA.ExecuteReader();
                while (reader_PNA.Read())
                {
                    Approved PNAlist = new Approved();
                    PNAlist.AccountID = Convert.ToInt32(reader_PNA.GetValue(0));
                    PNAlist.AccountName = reader_PNA.GetString(1);
                    PNAlist.AccountCode = Convert.ToInt32(reader_PNA.GetValue(2));
                    PNAlist.ProposalID = Convert.ToInt32(reader_PNA.GetValue(3));
                    PNAlist.Actual_Year = 0;
                    PNAlist.Current_Year = 0;
                    PNAlist.Budget_Year = Convert.ToDecimal(reader_PNA.GetValue(4));
                    PNAlist.ApprovedYear = Convert.ToDouble(reader_PNA.GetValue(5));
                    //PNAlist.ProposalAmmount = Convert.ToDouble(reader_PNA.GetValue(4));
                    //PNAlist.ProposalStatusHR = Convert.ToInt32(reader_PNA.GetValue(5));
                    //PNAlist.ProposalStatusInCharge = Convert.ToInt32(reader_PNA.GetValue(6));
                    //PNAlist.ProposalStatusCommitteeINT = Convert.ToInt32(reader_PNA.GetValue(7));
                    PNAlist.ProposalDenominationCode = Convert.ToInt32(reader_PNA.GetValue(9));
                    //PNAlist.OOEID = Convert.ToInt32(reader_PNA.GetValue(9));
                    
                    Accounts.Add(PNAlist);
                }
                reader_PNA.Close();
                
            }
            return Accounts;
        }
        public double getSlashAmount(Int64? ProposalID)
        {
            double SlashingAmount = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_Slashing = new SqlCommand(@"SELECT Top 1 ISNULL(a.Amount, 0) FROM tbl_R_AmountHistory as a WHERE ProposalID = '" + ProposalID + "' ORDER BY AmountHistoryID DESC", con);
                con.Open();
                SqlDataReader reader_Slashing = query_Slashing.ExecuteReader();
                while (reader_Slashing.Read())
                {
                    SlashingAmount = Convert.ToDouble(reader_Slashing.GetValue(0));
                }
            }
            return SlashingAmount;
        } 
        public IEnumerable<AmountHistory> AccountHistoryAmount(int? ProposalID)
        {
            List<AmountHistory> amountHistory = new List<AmountHistory>();
            ProposalID = ProposalID == null ? 0 : ProposalID;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_HistoryAmount = new SqlCommand(@"sp_BMS_ProposeHistory "+ ProposalID  + "", con);
                con.Open();
                SqlDataReader reader_HistoryAccounts = query_HistoryAmount.ExecuteReader();
                while (reader_HistoryAccounts.Read())
                {
                    AmountHistory data_history = new AmountHistory();
                    data_history.Amount = Convert.ToDecimal(reader_HistoryAccounts.GetValue(0));
                    data_history.Date = reader_HistoryAccounts.GetString(1);
                    data_history.UserName = reader_HistoryAccounts.GetString(2);
                    data_history.AmountHistoryID = Convert.ToInt32(reader_HistoryAccounts.GetValue(3));
                    amountHistory.Add(data_history);
                }
            }
            return amountHistory;
        }
        public IEnumerable<NewAccounts> newAccount(int? prog_ID, int? proy_ID)
        {
            List<NewAccounts> NewAccounts = new List<NewAccounts>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_NewAccounts = new SqlCommand(@"SELECT DISTINCT
                    dbo.tbl_T_BMSBudgetProposal.ProposalID,
                    dbo.tbl_T_BMSBudgetProposal.ProposalAmount,
                    dbo.tbl_R_BMSProgramAccounts.AccountName,
                    dbo.tbl_R_BMSObjectOfExpenditure.OOEAbrevation,
                    dbo.tbl_T_BMSBudgetProposal.ProposalYear,
                    dbo.tbl_T_BMSBudgetProposal.ProposalDateTime,
                    dbo.tbl_T_BMSBudgetProposal.ProposalStatusHR
                    FROM
                    dbo.tbl_T_BMSBudgetProposal
                    INNER JOIN dbo.tbl_R_BMSProgramAccounts ON dbo.tbl_T_BMSBudgetProposal.ProgramID = dbo.tbl_R_BMSProgramAccounts.ProgramID AND dbo.tbl_T_BMSBudgetProposal.AccountID = dbo.tbl_R_BMSProgramAccounts.AccountID
                    INNER JOIN dbo.tbl_R_BMSOfficePrograms ON dbo.tbl_R_BMSProgramAccounts.ProgramID = dbo.tbl_R_BMSOfficePrograms.ProgramID
                    INNER JOIN dbo.tbl_R_BMSObjectOfExpenditure ON dbo.tbl_R_BMSProgramAccounts.ObjectOfExpendetureID = dbo.tbl_R_BMSObjectOfExpenditure.OOEID
                    WHERE dbo.tbl_T_BMSBudgetProposal.ProgramID = " + prog_ID + " and dbo.tbl_T_BMSBudgetProposal.ProposalYear = '" + proy_ID + "' and dbo.tbl_R_BMSProgramAccounts.AccountYear = '" + proy_ID + "' and dbo.tbl_T_BMSBudgetProposal.ProposalActionCode = 99 and dbo.tbl_R_BMSOfficePrograms.OfficeID = '" + Account.UserInfo.Department.ToString() + "' ORDER BY AccountName", con);

                con.Open();
                SqlDataReader reader_NewAccounts = query_NewAccounts.ExecuteReader();
                while (reader_NewAccounts.Read())
                {
                    NewAccounts account = new NewAccounts();
                    account.ProposalID = Convert.ToDouble(reader_NewAccounts.GetValue(0));
                    account.ProposalAmount = Convert.ToDouble(reader_NewAccounts.GetValue(1));
                    account.AccountName = reader_NewAccounts.GetString(2);
                    account.OOE = reader_NewAccounts.GetString(3);
                    account.ProposalYear = Convert.ToInt32(reader_NewAccounts.GetValue(4));
                    account.ProposalDateTime = reader_NewAccounts.GetString(5);
                    account.ProposalStatusHR = Convert.ToInt32(reader_NewAccounts.GetValue(6));
                    NewAccounts.Add(account);
                }

            }
            return NewAccounts;
        }
        public void UpdateNewAccounts(IEnumerable<NewAccounts> Accounts)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                foreach (NewAccounts Account in Accounts)
                {
                    SqlCommand updateAmount = new SqlCommand(@"UPDATE tbl_T_BMSBudgetProposal set ProposalAmount = " + Account.ProposalAmount + " WHERE ProposalID = " + Account.ProposalID + "", con);
                    updateAmount.ExecuteNonQuery();
                }
            }
        }
        public string UpdateAmount(double? Budget, int? ProposalID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                if (Budget == null || Budget == 0 || ProposalID == null || ProposalID == 0)
                {
                    return "All Fields are Required!";
                }
                else
                {
                    SqlCommand select_officelevel = new SqlCommand(@"SELECT OfficeLevel FROM tbl_R_BMSProposalRemark WHERE ProposalID = " + ProposalID, con);
                    con.Open();
                    SqlDataReader reader_officelevel = select_officelevel.ExecuteReader();
                    var OfficeLevel = "";
                    while (reader_officelevel.Read())
                    {
                        OfficeLevel = reader_officelevel.GetString(0);
                    }
                    reader_officelevel.Close();
                    if (OfficeLevel == "HRMO In-Charge")
                    {

                        SqlCommand query_updateBudget = new SqlCommand(@"Update tbl_T_BMSBudgetProposal set ProposalAmount =" + Budget + ", ProposalStatusHR = 2, ProposalStatusInCharge = 2, ProposalStatusCommittee = 2, ProposalActionCode = 1 where ProposalID = " + ProposalID, con);
                        query_updateBudget.ExecuteNonQuery();


                    }
                    else if (OfficeLevel == "Budget Office")
                    {

                        SqlCommand query_updateBudget = new SqlCommand(@"Update tbl_T_BMSBudgetProposal set ProposalAmount =" + Budget + ", ProposalStatusHR = 1, ProposalStatusInCharge = 2, ProposalStatusCommittee = 2, ProposalActionCode = 1 where ProposalID = " + ProposalID, con);
                        query_updateBudget.ExecuteNonQuery();

                    }
                    else if (OfficeLevel == "Budget Committee / Local Finance Committee")
                    {

                        SqlCommand query_updateBudget = new SqlCommand(@"Update tbl_T_BMSBudgetProposal set ProposalAmount =" + Budget + ", ProposalStatusHR = 1, ProposalStatusInCharge = 1, ProposalStatusCommittee = 2, ProposalActionCode = 1 where ProposalID = " + ProposalID, con);
                        query_updateBudget.ExecuteNonQuery();

                    }
                    else
                    {
                        return "Something went Wrong";
                    }
                    SqlCommand query_Delete = new SqlCommand(@"Update tbl_R_BMSProposalRemark set Remarks = 'Approved' WHERE ProposalId =" + ProposalID, con);
                    query_Delete.ExecuteNonQuery();
                    return "success";
                }
            }

        }
        public string UpdateCopyPreviousAmount(double? PreviousAmount, int? AccountID, int? ProgramID, int? ProposalYear, int OfficeID, string AccountName)
        {
            //AdditionalRulesCaseStatement AdditionalRulesCaseStatement = new AdditionalRulesCaseStatement();
            //var caseStatement = AdditionalRulesCaseStatement.CaseStatement();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (PreviousAmount == null)
                {
                    return "Something went Wrong!";
                }
                else
                {
                    //SqlCommand query_UpdateCopyPreviousAmount = new SqlCommand(@"Update ")
                    var userID = Account.UserInfo.eid;
                    SqlCommand query_time = new SqlCommand(@"SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' 
                    + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' 
                    + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' 
                    + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' 
                    + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);
                    con.Open();
                    SqlDataReader reader = query_time.ExecuteReader();
                    var timeDate = "";
                    while (reader.Read())
                    {
                        timeDate = reader.GetString(0);
                    }
                    reader.Close();
                    SqlCommand com = new SqlCommand(@"
                        SELECT AccountDenominationID FROM tbl_T_BMSAccountDenomination WHERE DenominationName = '" + AccountName.Replace("'", "''") + "' and ProgramID = '" + ProgramID + "' and AccountID = '" + AccountID + "' and TransactionYear = '" + ProposalYear + "' " +
                        "IF @@ROWCOUNT=0 insert into tbl_T_BMSAccountDenomination values('" + AccountName.Replace("'", "''") + "', '" + PreviousAmount + "', '" + timeDate + "', '" + userID + "', 1, '" + ProposalYear + "', '" + ProgramID + "', '" + AccountID + "', 0, 1, '" + PreviousAmount + "', '" + OfficeID + "',1)" +
                        "ELSE Update tbl_T_BMSAccountDenomination set DenominationAmount = '" + PreviousAmount + "', ActionCode = 1 WHERE DenominationName = '" + AccountName.Replace("'", "''") + "' and ProgramID = '" + ProgramID + "' and AccountID = '" + AccountID + "' ", con);
                    //con.Open();
                    com.ExecuteNonQuery();
                    return "success";
                }
            }
        }
        public OfficeDataModel getOfficeData(int OfficeID)
        {
            OfficeDataModel OfficeDataModel = new OfficeDataModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select OfficeID , CONCAT(OfficeName,' (',REPLACE(OfficeAbbr, ' ', ''),')') 
                                                from pmis.dbo.OfficeDescription where OfficeID = 
                                                (Select MainOfficeIDPMIS from IFMIS.dbo.tbl_R_BMSMainAndSubOfficesCasual 
                                                where SubofficeIDIFMIS = " + OfficeID + " )", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OfficeDataModel.PMISOfficeID = Convert.ToInt32(reader.GetValue(0));
                    OfficeDataModel.PMISOfficeDesc = reader.GetValue(1).ToString();
                }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select OfficeID , CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') 
                                                from ifmis.dbo.tbl_R_BMSOffices where OfficeID = "+ OfficeID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OfficeDataModel.IFMISOfficeID = Convert.ToInt32(reader.GetValue(0));
                    OfficeDataModel.IFMISOfficeDesc = reader.GetValue(1).ToString();
                }
            }
            return OfficeDataModel;
        }
        public IEnumerable<ProgramsModel> grSearchOfficeProgram(int? propYear1)
        {
            List<ProgramsModel> pross = new List<ProgramsModel>();
            
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select ProgramID,ProgramDescription from tbl_R_BMSOfficePrograms where ProgramYear = '" + propYear1 + "' and OfficeID = '" + Account.UserInfo.Department.ToString() + "' and ActionCode = 1 order by cast([OrderNo] as int),[ProgramDescription] ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProgramsModel app = new ProgramsModel();
                    app.ProgramID = reader.GetValue(0).ToString();
                    app.ProgramDescription = reader.GetString(1);

                    pross.Add(app);
                }
                //ProgramsModel NonOffice = new ProgramsModel();
                //NonOffice.ProgramID = "43";
                //NonOffice.ProgramDescription = "Non-Office";
                //pross.Add(NonOffice);
            }
            return pross;
        }
        public int getAccountCode(int AccountID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select AccountCode from tbl_R_BMSaccounts where fmisaccountcode = " + AccountID + "", con);
                con.Open();
                return Convert.ToInt32(com.ExecuteScalar());
            }
        }

        public IEnumerable<PlantillaModel> grGetCasualToTransfer(int? PMISOfficeID, int? IFMISOfficeID)
        {
            List<PlantillaModel> PlantillaList = new List<PlantillaModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select eid,Firstname + ' ' + left(mi,1) + '. ' + Lastname + ' ' + Suffix from pmis.dbo.vw_CasualListWithRate
                                                where eid not in (select eid from IFMIS.dbo.tbl_R_BMSVacantAndTransferedCasual
                                                where OfficeID = "+ IFMISOfficeID +" and Yearof = " + DateTime.Now.Year + " + 1 and eid is not null and ActionCode = 1) "+""
                                                +"and Office = " + PMISOfficeID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PlantillaModel PlantillaModel = new PlantillaModel();
                    PlantillaModel.SeriesID = Convert.ToInt64(reader.GetValue(0));
                    PlantillaModel.EmployeeName = reader.GetValue(1).ToString();

                    PlantillaList.Add(PlantillaModel);

                }
            }
            return PlantillaList;


        }
        public IEnumerable<AccountsModel>  grOfficeProgram(int? prog_ID, int? propYear1, int? officeID, int? ExpenseClassID, int? suppletag)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            #region Comment Code
            #endregion
            var Query = "";
            //if (officeID == 43 && Account.UserInfo.UserTypeDesc != "Budget In-Charge")
            //{
            //    Query = @"SELECT DISTINCT e.AccountID,  e.AccountName,  f.OOEAbrevation,a.ProposalYear,ISNULL(a.ProposalAllotedAmount,0)ProposalAllotedAmount,e.AccountYear,e.ProgramID, 
            //            isnull(b.AccountCode,0), e.ObjectOfExpendetureID,c.ProgramDescription,0 FROM dbo.tbl_T_BMSBudgetProposal AS a
            //            left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID
            //            left JOIN dbo.tbl_R_BMSAccounts AS b ON b.FMISAccountCode = a.AccountID 
            //            left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
            //            left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
            //            LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
            //            LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
            //            WHERE c.OfficeID = '43' and  c.ProgramID = " + ExpenseClassID 
            //            +" and a.ProposalYear = "+ propYear1 +"-1 and e.ActionCode = 1 and a.ProposalActionCode = '1' and a.ProposalStatusCommittee='1' and e.AccountYear='"+propYear1+"' and c.ProgramYear = '"+propYear1
            //            +"' and a.AccountID not in(select AccountID from tbl_T_BMSBudgetProposal where ProgramID in(SELECT ProgramID from tbl_R_BMSOfficePrograms "+""
            //            + " where OfficeID = 43 and ActionCode = 1 and ProgramYear = " + propYear1 + ") and ProposalYear = " + propYear1 + " and ProposalActionCode = 1) and e.AccountName is not null";
            //}
            //else
            //{
                Query = @"exec sp_bms_PreparationLBP2 "+ officeID  + ","+ prog_ID + ","+ ExpenseClassID  + ","+ propYear1 + ","+ suppletag + "";
            //}
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(Query, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.AccountCode = Convert.ToInt32(reader.GetValue(7));
                    emp.AccountID = Convert.ToInt32(reader.GetValue(0));
                    emp.AccountName = reader.GetString(1);
                    // emp.FundName = reader.GetString(2);
                    emp.OOEName = reader.GetString(2);
                    emp.PastProposalYear = reader.GetInt32(3);
                    emp.CurrentProposalAmount = Convert.ToDouble(reader.GetValue(4));
                    //emp.UserID = reader.GetString(6);
                    emp.ProposalYear = reader.GetInt32(5);
                    emp.ProgramID = reader.GetInt32(6);
                    emp.OOEID = Convert.ToInt32(reader.GetValue(8));
                    emp.ProgramDescription = reader.GetString(9);
                    emp.CheckComp = Convert.ToInt32(reader.GetValue(10));
                    emp.NewAccountID= Convert.ToInt64(reader.GetValue(11));
                    emp.NewProgramID =Convert.ToInt32(reader.GetValue(12));
                    emp.OldOffice = Convert.ToInt32(reader.GetValue(13));
                    emp.isProposed = 0;
                    if ((emp.OOEID == 1 || emp.OOEID == 2 || emp.OOEID == 3 || emp.OOEID == 4) && suppletag == 0)
                    {
                        emp.setProposalAllotedAmount = setProposalAllotedAmount(emp.AccountCode, officeID, propYear1, emp.OOEID, emp.NewProgramID, emp.NewAccountID);
                    }
                    else
                    {
                        emp.setProposalAllotedAmount = setProposalAllotedAmount_Supple(officeID, emp.NewAccountID, propYear1);
                    }
                    if (emp.setProposalAllotedAmount == 0)
                    {
                        emp.checker = 0;
                    }
                    else
                    {
                        emp.checker = 1;
                    }
                    emp.PastProposalAmmount = getPastYearAmount(propYear1, reader.GetValue(0).ToString(), emp.OldOffice, reader.GetValue(6).ToString(), prog_ID);
                    //if (emp.OOEID == 1)
                    //{
                      
                    //    ooek = emp.OOEID;
                    //    ProposalAllotedAmount = emp.setProposalAllotedAmount + ProposalAllotedAmount;
                    //}
                    if(emp.ProgramID==43){
                        progIDV = 43;
                    }
                    prog.Add(emp);
                    
                }
                if (ExpenseClassID == 1 && prog_ID != 43)
                {
                    AccountsModel ForFundingAccount = new AccountsModel();
                    ForFundingAccount.AccountCode = 0;
                    ForFundingAccount.AccountID = 0;
                    ForFundingAccount.AccountName = "Proposed Positions For Funding";
                    // emp.FundName = reader.GetString(2);
                    ForFundingAccount.OOEName = "PS";
                    ForFundingAccount.PastProposalYear = 0;
                    ForFundingAccount.CurrentProposalAmount = 0;
                    //emp.UserID = reader.GetString(6);
                    ForFundingAccount.ProposalYear = Convert.ToInt32(propYear1);
                    ForFundingAccount.ProgramID = Convert.ToInt32(prog_ID);
                    ForFundingAccount.CheckComp = 1;
                    ForFundingAccount.OOEID = 1;

                    if (ForFundingAccount.OOEID == 1 || ForFundingAccount.OOEID == 2 || ForFundingAccount.OOEID == 3)
                    {
                        ForFundingAccount.setProposalAllotedAmount = Convert.ToDecimal(getforFundingTotalOfficeLevel(propYear1, officeID) + getforFundingCasualTotal(propYear1, officeID));
                    }
                    else
                    {
                        ForFundingAccount.setProposalAllotedAmount = 0;
                    }
                    if (ForFundingAccount.setProposalAllotedAmount == 0)
                    {
                        ForFundingAccount.checker = 0;
                    }
                    else
                    {
                        ForFundingAccount.checker = 1;
                    }
                    ForFundingAccount.PastProposalAmmount = 0;
                    if (ForFundingAccount.setProposalAllotedAmount != 0)
                    {
                        prog.Add(ForFundingAccount);
                    }
                    //if (ForFundingAccount.OOEID == 1)
                    //{
                    //    ProposalAllotedAmount = ForFundingAccount.setProposalAllotedAmount + ProposalAllotedAmount;
                    //}
                }
            }

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand com = new SqlCommand(@"SELECT e_AccountID ,e_AccountName , b.OOEAbrevation ,a_ProposalYear ,a_ProposalAllotedAmount ,e_AccountYear ,e_ProgramID ,b_AccountCode ,e_ObjectOfExpendetureID ,opis FROM dbo.tbl_R_BMSdummyonly as a
                //                                inner join dbo.tbl_R_BMSObjectOfExpenditure as b on a.e_ObjectOfExpendetureID = b.OOEID where opis = " + officeID + "", con);

                var ProposedAccountQuery = "";
                if (officeID == 43)
                {
                    ProposedAccountQuery = @"SELECT a.ProposedID,
                        a.AccountID, a.AccountName, c.OOEAbrevation, a.ProposalYear, a.ProgramID, a.AccountCode, a.OOEID, a.OfficeID, a.ActionCode, b.ProgramDescription 
                        FROM tbl_R_BMSProposedAccounts as a 
                        INNER JOIN dbo.tbl_R_BMSOfficePrograms AS b ON a.ProgramID = b.ProgramID and a.ProposalYear = b.ProgramYear
                        INNER JOIN dbo.tbl_R_BMSObjectOfExpenditure as c ON a.OOEID = c.OOEID
                        WHERE a.OfficeID = '" + officeID + "' and a.ProgramID = " + (prog_ID == 43 ? ExpenseClassID : prog_ID) + " and b.ProgramYear = '" + propYear1 + "' and  a.ActionCode = 1 and b.ActionCode = 1 and (a.AccountID  NOT in(select AccountID from tbl_T_BMSBudgetProposal where ProposalYear = '" + propYear1 + "'and ProposalActionCode = 1 and [ProgramID]= " + (prog_ID == 43 ? ExpenseClassID : prog_ID) + "))";
                }
                else
                {
                    ////Changed on 6/29/2020 - xXx
                    //ProposedAccountQuery = @"SELECT a.ProposedID,
                    //    a.AccountID, a.AccountName, c.OOEAbrevation, a.ProposalYear, a.ProgramID, a.AccountCode, a.OOEID, a.OfficeID, a.ActionCode, b.ProgramDescription 
                    //    FROM tbl_R_BMSProposedAccounts as a     
                    //    INNER JOIN dbo.tbl_R_BMSOfficePrograms AS b ON a.ProgramID = b.ProgramID and a.ProposalYear = b.ProgramYear
                    //    INNER JOIN dbo.tbl_R_BMSObjectOfExpenditure as c ON a.OOEID = c.OOEID
                    //    WHERE a.OfficeID = '" + (prog_ID == 43 ? 43 : officeID) + "' " + (prog_ID == 43 ? null : " and  a.OOEID = " + ExpenseClassID + "") + " and a.ProgramID = '" + (prog_ID == 43 ? ExpenseClassID : prog_ID) + "' and b.ProgramYear = '" + propYear1 + "' and  a.ActionCode = 1 and b.ActionCode = 1 and (a.AccountID  NOT in(select AccountID from tbl_T_BMSBudgetProposal where ProposalYear = '" + propYear1 + "'and ProgramID = " + (prog_ID == 43 ? ExpenseClassID : prog_ID) + " and ProposalActionCode = 1))";
                    ProposedAccountQuery = @"exec sp_BMS_ProposedAccount "+ officeID  + ","+ prog_ID + ","+ ExpenseClassID + ","+ propYear1 + "," + suppletag + "";
                }
                SqlCommand com = new SqlCommand(ProposedAccountQuery, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    
                    emp.AccountID = Convert.ToInt32(reader.GetValue(1));
                    emp.AccountName = reader.GetString(2);
                    // emp.FundName = reader.GetString(2);
                    emp.OOEName = reader.GetString(3);
                    emp.ProposalYear = reader.GetInt32(4);
                    emp.ProgramID = reader.GetInt32(5);
                    emp.AccountCode = Convert.ToInt32(reader.GetValue(6));
                    emp.OOEID = Convert.ToInt32(reader.GetValue(7));
                    emp.PastProposalYear = 0;
                    emp.CurrentProposalAmount = Convert.ToDouble(reader.GetValue(11)); 
                    emp.ProgramDescription = reader.GetString(10);
                    emp.NewAccountID = Convert.ToInt64(reader.GetValue(1));
                    emp.NewProgramID = Convert.ToInt32(reader.GetValue(5));
                    //emp.UserID = reader.GetString(6);




                    //if (emp.OOEID == 1 || emp.OOEID == 2 || emp.OOEID == 3)
                    //{
                    emp.setProposalAllotedAmount = setProposalAllotedAmount(emp.AccountCode, officeID, propYear1, emp.OOEID, emp.ProgramID, emp.AccountID);
                    //}
                    //else
                    //{
                    //    emp.setProposalAllotedAmount = 0;
                    //}
                    //if (emp.setProposalAllotedAmount == 0)
                    //{
                    //    emp.checker = 0;
                    //}
                    //else
                    //{
                    //    emp.checker = 1;
            //}
                    emp.checker = 0;
                    emp.PastProposalAmmount = getPastYearAmount(propYear1, reader.GetValue(13).ToString(), officeID, reader.GetValue(12).ToString(),0); ;
                    emp.isProposed = 1;
                    //if (emp.OOEID == 1)
                    //{
                    //    ooek = emp.OOEID;
                    //    ProposalAllotedAmount = emp.setProposalAllotedAmount + ProposalAllotedAmount;
                    //}
                    prog.Add(emp);
                }
            }





            //using (SqlConnection con = new SqlConnection(Common.MyConn()))
            //{
            //    if (progIDV == 43)
            //    {

            //    }
            //    else if(ooek == 1)
            //    {
            //        SqlCommand com = new SqlCommand("delete from tbl_R_BMS_PSAmount_View where OfficeID = '" + Account.UserInfo.Department + "'and [year]='" + propYear1 + "' and ooe='" + ooek + "' and ProgsIDV = 0", con);
            //        con.Open();
            //        com.ExecuteNonQuery();
            //        SqlCommand com2 = new SqlCommand("insert into tbl_R_BMS_PSAmount_View (ProposalAllotedAmount,OfficeID,ooe,year,ProgsIDV) values ('" + ProposalAllotedAmount + "','" + Account.UserInfo.Department + "','" + ooek + "','" + propYear1 + "','0')", con);
            //        com2.ExecuteNonQuery();
            //    }
            //}

            //ProposalAllotedAmount = 0;
            return prog;


        }
        //public void grOfficeProgram2(int? prog_ID, int? propYear1, int? officeID, int ProgramID)
        //{
        //    using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //    {
        //        SqlCommand com = new SqlCommand("delete from tbl_R_BMS_PSAmount_View where OfficeID = '" + Account.UserInfo.Department + "'", con);
        //        con.Open();
        //        com.ExecuteNonQuery();
        //        SqlCommand com2 = new SqlCommand("insert into tbl_R_BMS_PSAmount_View values ('" + ProposalAllotedAmount + "','" + Account.UserInfo.Department + "')", con);
        //        com2.ExecuteNonQuery();

        //    }
        
        //}
        public double getforFundingTotal(int? ProposalYear, int? OfficeID)
        {
            var total = 0.0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_bms_getForFundingData " + ProposalYear + "," + OfficeID + ",0,1", con);
                con.Open();
                total = Convert.ToDouble(com.ExecuteScalar());
            }
            if (total == 0)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"dbo.sp_bms_getForFundingData " + ProposalYear + "," + OfficeID + ",0,2", con);
                    con.Open();
                    total = Convert.ToDouble(com.ExecuteScalar());
                }
            }
            return total;
        }
        public double getforFundingTotalOfficeLevel(int? ProposalYear, int? OfficeID)
        {
            var total = 0.0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_bms_getForFundingData " + ProposalYear + "," + OfficeID + ",0,1", con);
                con.Open();
                total = Convert.ToDouble(com.ExecuteScalar());
            }
            return total;
        }
        
        public double getforFundingCasualTotal(int? ProposalYear, int? OfficeID)
        {
            var total = 0.0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_bms_getForFundingDataCasual " + ProposalYear + "," + OfficeID + ",0,1", con);
                con.Open();
                total = Convert.ToDouble(com.ExecuteScalar());
            }
            if (total == 0)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"dbo.sp_bms_getForFundingDataCasual " + ProposalYear + "," + OfficeID + ",0,2", con);
                    con.Open();
                    total = Convert.ToDouble(com.ExecuteScalar());
                }
            }
            return total;
        }
        public IEnumerable<ForFundingModel> grGetForFundingDetails(int? ProposalYear, int? officeID)
        {
            List<ForFundingModel> ForFundingList = new List<ForFundingModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("dbo.sp_bms_getForFundingData " + ProposalYear + "," + officeID + ",1,1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
            { 
                    ForFundingModel Forfunding = new ForFundingModel();
                    Forfunding.ProposedItemID = reader.GetInt32(0);
                    Forfunding.Position = reader.GetString(1);
                    Forfunding.Salary = Convert.ToDouble(reader.GetValue(2));
                    Forfunding.yearlySalary = Convert.ToDouble(reader.GetValue(3));
                    Forfunding.pera = Convert.ToDouble(reader.GetValue(4));
                    Forfunding.clothing = Convert.ToDouble(reader.GetValue(5));
                    Forfunding.cashgift = Convert.ToDouble(reader.GetValue(6));
                    Forfunding.liferetirement = Convert.ToDouble(reader.GetValue(7));
                    Forfunding.pagibig = Convert.ToDouble(reader.GetValue(8));
                    Forfunding.philhealth = Convert.ToDouble(reader.GetValue(9));
                    Forfunding.eccContributions = Convert.ToDouble(reader.GetValue(10));
                    Forfunding.yearendbonus = Convert.ToDouble(reader.GetValue(11));
                    Forfunding.PersonalityBasedBonus = Convert.ToDouble(reader.GetValue(12));
                    Forfunding.HazardPay = Convert.ToDouble(reader.GetValue(13));
                    Forfunding.Subsistence = Convert.ToDouble(reader.GetValue(14));
                    Forfunding.GroupBY = "Proposed Positions";
                    Forfunding.Total = Convert.ToDouble(reader.GetValue(15));
                    Forfunding.PEI = Convert.ToDouble(reader.GetValue(16));
                    Forfunding.RepresentationAllowance = Convert.ToDouble(reader.GetValue(17));
                    Forfunding.TransportationAllowance = Convert.ToDouble(reader.GetValue(18));
                    
                    


                    ForFundingList.Add(Forfunding);
            }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("dbo.sp_bms_getForFundingDataCasual " + ProposalYear + "," + officeID + ",1,1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ForFundingModel Forfunding = new ForFundingModel();
                    Forfunding.ProposedItemID = reader.GetInt32(0);
                    Forfunding.Position = reader.GetString(1);
                    Forfunding.Salary = Convert.ToDouble(reader.GetValue(2));
                    Forfunding.yearlySalary = Convert.ToDouble(reader.GetValue(3));
                    Forfunding.pera = Convert.ToDouble(reader.GetValue(4));
                    Forfunding.clothing = Convert.ToDouble(reader.GetValue(5));
                    Forfunding.cashgift = Convert.ToDouble(reader.GetValue(6));
                    Forfunding.liferetirement = Convert.ToDouble(reader.GetValue(7));
                    Forfunding.pagibig = Convert.ToDouble(reader.GetValue(8));
                    Forfunding.philhealth = Convert.ToDouble(reader.GetValue(9));
                    Forfunding.eccContributions = Convert.ToDouble(reader.GetValue(10));
                    Forfunding.yearendbonus = Convert.ToDouble(reader.GetValue(11));
                    Forfunding.PersonalityBasedBonus = Convert.ToDouble(reader.GetValue(12));
                    Forfunding.HazardPay = Convert.ToDouble(reader.GetValue(13));
                    Forfunding.Subsistence = Convert.ToDouble(reader.GetValue(14));
                    Forfunding.GroupBY = "Proposed Casual";
                    Forfunding.Total = Convert.ToDouble(reader.GetValue(15));
                    
                    


                    ForFundingList.Add(Forfunding);
                }
            }
            return ForFundingList;
        }
        public IEnumerable<ForFundingModel> grGetForFundingDetailsCasual(int? ProposalYear, int? officeID)
        {
            List<ForFundingModel> ForFundingList = new List<ForFundingModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("dbo.sp_bms_getForFundingDataCasual " + ProposalYear + "," + officeID + ",1,1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ForFundingModel Forfunding = new ForFundingModel();
                    Forfunding.ProposedItemID = reader.GetInt32(0);
                    Forfunding.Position = reader.GetString(1);
                    Forfunding.Salary = Convert.ToDouble(reader.GetValue(2));
                    Forfunding.yearlySalary = Convert.ToDouble(reader.GetValue(3));
                    Forfunding.pera = Convert.ToDouble(reader.GetValue(4));
                    Forfunding.clothing = Convert.ToDouble(reader.GetValue(5));
                    Forfunding.cashgift = Convert.ToDouble(reader.GetValue(6));
                    Forfunding.liferetirement = Convert.ToDouble(reader.GetValue(7));
                    Forfunding.pagibig = Convert.ToDouble(reader.GetValue(8));
                    Forfunding.philhealth = Convert.ToDouble(reader.GetValue(9));
                    Forfunding.eccContributions = Convert.ToDouble(reader.GetValue(10));
                    Forfunding.yearendbonus = Convert.ToDouble(reader.GetValue(11));
                    Forfunding.PersonalityBasedBonus = Convert.ToDouble(reader.GetValue(12));
                    Forfunding.Total = Convert.ToDouble(reader.GetValue(13));


                    ForFundingList.Add(Forfunding);
                }
            }
            return ForFundingList;
        }

        public decimal setProposalAllotedAmount(long? AccountID, int? officeID, int? propYear1, int? OOEID, int? ProgramID, long? FMISAccountCode)
        {

            decimal result = 0;
           // var PMISOfficeID = 0;

            if (officeID == 43 && Account.UserInfo.UserTypeDesc != "Budget In-Charge" && Account.UserInfo.lgu==0)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com2 = new SqlCommand(@"dbo.sp_bms_getNonOfficeTotal " + AccountID + "," + getPmisOfficeID(Convert.ToInt32(officeID)) + "," + propYear1 + ", " + OOEID + ", " + ProgramID + ", 0," + FMISAccountCode + "," + Account.UserInfo.eid + "", con);
                    con.Open();
                    result = Convert.ToDecimal(com2.ExecuteScalar());
                }
            }
            else
            {
            var MotherOfficeID = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT isnull(MainOfficeID_PMIS,0) 
                FROM [dbo].[tbl_R_BMSMainAndSubOffices] where SubOfficeID_IFMIS =" + officeID + "", con);
                con.Open();
                try
                {
                    MotherOfficeID = Convert.ToInt32(com.ExecuteScalar());
                }
                catch (Exception)
                {
                    MotherOfficeID = 0;
                }
                if (MotherOfficeID == 0)
                {
                    SqlCommand com2 = new SqlCommand(@"dbo.SP_bms_getComputationTotal " + AccountID + "," + getPmisOfficeID(Convert.ToInt32(officeID)) + "," + propYear1 + ", " + OOEID + ", " + ProgramID + ", 0," + FMISAccountCode + "," + Account.UserInfo.eid + "", con);
                    result = Convert.ToDecimal(com2.ExecuteScalar());
                }
                else 
                {
                        SqlCommand com3 = new SqlCommand(@"dbo.sp_BMS_getComputation_Transfered " + AccountID + "," + MotherOfficeID + "," + propYear1 + "," + OOEID + "," + ProgramID + ", 0," + officeID + "," + FMISAccountCode + "," + Account.UserInfo.eid + "", con);
                    result = Convert.ToDecimal(com3.ExecuteScalar());
                }
            }
            }
            return result;
        }

        public decimal setProposalAllotedAmount_Supple(int? officeID, long? NewAccountID,int? propYear1)
        {
            decimal result = 0;
            using (SqlConnection con_supp = new SqlConnection(Common.MyConn()))
            {
               
                SqlCommand supple = new SqlCommand(@"ifmis.dbo.sp_BMS_ProposalSupplemental "+ officeID + ","+ NewAccountID + ","+ propYear1 + "", con_supp);
                con_supp.Open();
                result = Convert.ToDecimal(supple.ExecuteScalar());
            }

            return result;

        }
        public double getPastYearAmount(int? BudgetYear, string AccountID, int? officeID, string ProgramID, int? prog_ID)
        {
            double PastAmount = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand com = new SqlCommand(@"SELECT isnull(sum(amount),0)  from fmis.dbo.tblAMIS_SE_byYearOffice where rcenter = '" + officeID + "'  and year_ = '" + BudgetYear + "' -2 and AccountCode = '" + AccountID + "'", con);
                //SqlCommand com = new SqlCommand(@"SELECT Obligation FROM tbl_R_BMSObligatedAccounts WHERE " +
                //        "fmisOfficeID = '" + (prog_ID == 43 ? prog_ID : officeID) + "' And fmisProgramCode = '" + ProgramID + "' and fmisAccountCode = '" + AccountID + "' and YearOf = '" + BudgetYear + "' - 2 and ActionCode = 1", con);
                //SqlCommand com = new SqlCommand(@"select isnull(c.Appropriations,a.ProposalAllotedAmount) ProposalAllotedAmount  from dbo.tbl_T_BMSBudgetProposal as a
                //LEFT JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID and a.ProposalActionCode = b.ActionCode and b.ProgramYear = a.ProposalYear
                //left join [IFMIS].[dbo].[tbl_R_BMSCurrentYearAppropriation] as c on c.ProgramID=a.ProgramID and c.AccountID=a.AccountID and c.YearOf=a.ProposalYear
                //where b.OfficeID = " + officeID+" and a.ProposalActionCode = 1 and a.ProposalYear = "+BudgetYear+" - 2 and a.ProposalStatusCommittee = 1 and a.AccountID = "+AccountID+" and b.ProgramID = "+ProgramID+"", con);
                SqlCommand com = new SqlCommand(@"select  dbo.fn_BMS_PriorYearsObligation ("+ officeID  + ",'"+ ProgramID + "',"+ BudgetYear + ","+ AccountID  + ")", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PastAmount = Convert.ToDouble(reader.GetValue(0));
                }
            }
            return PastAmount;
        }
        //        public IEnumerable<AccountsModel> grOfficeProgramTEST()
        //        {
        //            List<AccountsModel> prog = new List<AccountsModel>();
        //            using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //            {
        //                var propYear1 = 2017;
        //                SqlCommand com = new SqlCommand(@"SELECT DISTINCT e.AccountID,  e.AccountName,  f.OOEAbrevation,a.ProposalYear,a.ProposalAllotedAmount,e.AccountYear,e.ProgramID FROM dbo.tbl_T_BMSBudgetProposal AS a
        //                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
        //                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID 
        //                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
        //                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
        //                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
        //                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
        //                                                    WHERE c.OfficeID = '" + Account.UserInfo.Department.ToString() + "' and  c.ProgramID = '111' and a.ProposalYear = " + propYear1
        //                                                    + "-1 and a.ProposalActionCode = '1' and a.ProposalStatusCommittee='1' and e.AccountYear='" + propYear1 + "' and (e.AccountID  NOT in(select AccountID from tbl_T_BMSBudgetProposal where ProposalYear = '" + propYear1 + "' ))", con);
        //                con.Open();
        //                SqlDataReader reader = com.ExecuteReader();
        //                while (reader.Read())
        //                {
        //                    AccountsModel emp = new AccountsModel();
        //                    emp.AccountID = Convert.ToInt32(reader.GetValue(0));
        //                    emp.AccountName = reader.GetString(1);
        //                    // emp.FundName = reader.GetString(2);
        //                    emp.OOEName = reader.GetString(2);
        //                    emp.PastProposalYear = reader.GetInt32(3);
        //                    emp.PastProposalAmmount = Convert.ToDouble(reader.GetValue(4));
        //                    //emp.UserID = reader.GetString(6);
        //                    emp.ProposalYear = reader.GetInt32(5);
        //                    emp.ProgramID = reader.GetInt32(6);

        //            prog.Add(emp);
        //        }
        //    }
        //    return prog;
        //}
        public IEnumerable<AccountDenomination> grAccountDenomination(int? ProgramID=0, int? AccountID=0, int? ProposalYear=0, int? OfficeID=0,int? suppletag=0)
        {
            List<AccountDenomination> prog = new List<AccountDenomination>();
            if (OfficeID == 43 && Account.UserInfo.UserTypeDesc != "Budget In-Charge")
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand query = new SqlCommand(@"Select c.AccountName + ' (' + REPLACE(b.OfficeAbbrivation, ' ', '') + ')' ,sum(a.TotalAmount) 
                                                        from tbl_T_BMSAccountDenomination as a
                                                        inner JOIN tbl_R_BMSOffices as b on b.OfficeID = a.OfficeID
                                                        INNER JOIN tbl_R_BMSAccounts as c on c.FMISAccountCode = a.AccountID and ActionCode = 1
                                                        where a.ProgramID = '"+ ProgramID +"' and a.AccountID = '"+ AccountID +"' and a.TransactionYear = '"+ProposalYear
                                                        +"' and a.ActionCode = 1 "+""
                                                        + " and a.AccountID not in(select AccountID from tbl_R_BMSProposedAccounts where ProgramID = " + ProgramID + " and ProposalYear = " + ProposalYear + " and ActionCode = 1)" + ""
                                                        +" GROUP BY c.AccountName + ' (' + REPLACE(b.OfficeAbbrivation, ' ', '') + ')' ", con);
                    con.Open();
                    SqlDataReader reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        AccountDenomination emp = new AccountDenomination();
                        emp.DenominationName = reader.GetString(0);
                        emp.DenominationAmount = 0;
                        emp.AccountDenominationID = 0;
                        emp.isPPMP = 2;
                        emp.QuantityPercentage =0;
                        emp.DenominationMonth = 0;
                        emp.TotalDenominationAmount = Convert.ToDecimal(reader.GetValue(1));
                        prog.Add(emp);
                    } 
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand query = new SqlCommand(@"Select replace(c.AccountName,'''','') + ' (' + REPLACE(b.OfficeAbbrivation, ' ', '') + ')' ,sum(a.TotalAmount) 
                                                        from tbl_T_BMSAccountDenomination as a
                                                        inner JOIN tbl_R_BMSOffices as b on b.OfficeID = a.OfficeID
                                                        INNER JOIN tbl_R_BMSProposedAccounts as c on c.AccountID = a.AccountID and c.ActionCode = 1
                                                        where a.ProgramID = "+ProgramID+" and a.AccountID = "+AccountID+" and a.TransactionYear = "+ProposalYear+""  +""
                                                        +" and a.AccountID in(select AccountID from tbl_R_BMSProposedAccounts as d where d.ProgramID = "+ProgramID+" and d.ProposalYear = "+ProposalYear+" and d.ActionCode = 1) "+""
                                                        +" and a.ActionCode = 1 GROUP BY c.AccountName + ' (' + REPLACE(b.OfficeAbbrivation, ' ', '') + ')' ", con);
                    con.Open();
                    SqlDataReader reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        AccountDenomination emp = new AccountDenomination();
                        emp.DenominationName = reader.GetString(0);
                        emp.DenominationAmount = 0;
                        emp.AccountDenominationID = 0;
                        emp.isPPMP = 2;
                        emp.QuantityPercentage = 0;
                        emp.DenominationMonth = 0;
                        emp.TotalDenominationAmount = Convert.ToDecimal(reader.GetValue(1));
                        prog.Add(emp);
                    }
                    
                }
            }
            else
            {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                    //SqlCommand query = new SqlCommand(@"Select AccountDenominationID,replace(replace(DenominationName,'''',''),'""','in') DenominationName,DenominationAmount,isPPMP,QuantityOrPercentage,TotalAmount,[Month] from tbl_T_BMSAccountDenomination where ProgramID = '" + ProgramID 
                    //    + "' and AccountID = '" + AccountID + "' and TransactionYear = '" + ProposalYear
                    //    + "' and ActionCode = 1 and OfficeID = '" + OfficeID + "' order by AccountDenominationID ", con);
                    SqlCommand query = new SqlCommand(@"sp_BMS_LBP2Preparation "+ OfficeID + "," + ProgramID + "," + AccountID + "," + ProposalYear + ","+ suppletag + "", con);
                    con.Open();
                    SqlDataReader reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        AccountDenomination emp = new AccountDenomination();

                        emp.AccountDenominationID = Convert.ToInt32(reader.GetValue(0));
                        emp.DenominationName = reader.GetString(1);
                        emp.DenominationAmount = Convert.ToDecimal(reader.GetValue(2));
                        emp.isPPMP = Convert.ToInt32(reader.GetValue(3));
                        emp.QuantityPercentage = Convert.ToDecimal(reader.GetValue(4));
                        emp.TotalDenominationAmount = Convert.ToDecimal(reader.GetValue(5));
                        emp.DenominationMonth = Convert.ToDecimal(reader.GetValue(6));
                        emp.specificactivity = Convert.ToString(reader.GetValue(8));
                        prog.Add(emp);
                    }
                   
                }
         
            }
            
        
            return prog;
        }
        
        public IEnumerable<AccountDenomination> grAccountDenominationBudgetApproval(int? ProgramID, int? AccountID, int? ProposalYear, int? OfficeID,int? aipversion)
        {
            List<AccountDenomination> prog = new List<AccountDenomination>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (OfficeID == 43)
                {
                    SqlCommand select_NonOffice = new SqlCommand(@"sp_BMS_GetDenominationNonOffice " + AccountID + ", " + ProgramID + ", " + ProposalYear + "", con);
                    con.Open();
                    SqlDataReader reader_NonOffice = select_NonOffice.ExecuteReader();
                    while (reader_NonOffice.Read())
                    {
                        AccountDenomination emp = new AccountDenomination();
                        emp.DenominationName = reader_NonOffice.GetString(0);
                        emp.DenominationAmount = 0;
                        emp.AccountDenominationID = 0;
                        emp.isPPMP = 0;
                        emp.QuantityPercentage = 0;
                        emp.DenominationMonth = 0;
                        emp.TotalDenominationAmount = Convert.ToDecimal(reader_NonOffice.GetValue(1));
                        emp.Remarks = reader_NonOffice.GetValue(2).ToString();
                        emp.OfficeID = Convert.ToInt32(reader_NonOffice.GetValue(3));
                        emp.AccountID = AccountID;
                        emp.ProgramID = ProgramID;
                        emp.TotalDenominationAmountHistory = Convert.ToDouble(reader_NonOffice.GetValue(4));
                        emp.TransactionYear = Convert.ToInt32(ProposalYear);
                        emp.QuantityPercentageHistory = Convert.ToDecimal(reader_NonOffice.GetValue(5));
                        prog.Add(emp);
                    }

                }
                else
                {
                    //SqlCommand query = new SqlCommand(@"Select * from tbl_T_BMSAccountDenomination where ProgramID = '" + ProgramID + "' and AccountID = '" + AccountID + "' and TransactionYear = '" + ProposalYear + "' and ActionCode = 1 and OfficeID = '" + OfficeID + "'", con);
                    if (aipversion == 1) //annual budget
                    {
                        SqlCommand query = new SqlCommand(@"sp_bms_getDenomination " + AccountID + "," + ProgramID + "," + ProposalYear + "," + OfficeID + "", con);
                        con.Open();
                        query.CommandTimeout = 0;
                        SqlDataReader reader = query.ExecuteReader();
                        //double compute = 0;
                        while (reader.Read())
                        {
                            AccountDenomination emp = new AccountDenomination();
                            emp.DenominationName = reader.GetString(1);
                            emp.DenominationAmount = Convert.ToDecimal(reader.GetValue(2));
                            emp.AccountDenominationID = Convert.ToInt32(reader.GetValue(0));
                            emp.TransactionYear = Convert.ToInt32(reader.GetValue(6));
                            emp.isPPMP = Convert.ToInt32(reader.GetValue(9));
                            emp.ProgramID = Convert.ToInt32(reader.GetValue(7));
                            emp.AccountID = Convert.ToInt32(reader.GetValue(8));
                            emp.QuantityPercentage = Convert.ToDecimal(reader.GetValue(10));
                            emp.OfficeID = Convert.ToInt32(reader.GetValue(12));
                            emp.DenominationMonth = Convert.ToDecimal(reader.GetValue(13));
                            emp.TotalDenominationAmount = Convert.ToDecimal(reader.GetValue(11));
                            emp.Remarks = reader.GetValue(14).ToString();
                            decimal QuantityPercentageHistoryParam = 0;
                            double TotalDenominationAmountParam = 0;
                            int AccountDenominationIDParam = Convert.ToInt32(reader.GetValue(0));
                            DenominationHistory(ref QuantityPercentageHistoryParam, ref TotalDenominationAmountParam, ref AccountDenominationIDParam);

                            if (QuantityPercentageHistoryParam == 0 && TotalDenominationAmountParam == 0)
                            {
                                emp.QuantityPercentageHistory = Convert.ToDecimal(reader.GetValue(10));
                                emp.TotalDenominationAmountHistory = Convert.ToDouble(reader.GetValue(11));
                            }
                            else
                            {
                                emp.QuantityPercentageHistory = QuantityPercentageHistoryParam;
                                emp.TotalDenominationAmountHistory = TotalDenominationAmountParam;
                            }
                            // compute = compute + emp.TotalDenominationAmountHistory;
                            //emp.QuantityPercentageHistory = QuantityPercentageHistory(reader.GetValue(0));
                            //emp.QuantityPercentageHistory
                            prog.Add(emp);
                        }
                    }
                    else //supplemental
                    {
                        SqlCommand query = new SqlCommand(@"sp_bms_getDenomination_supplemental " + AccountID + "," + ProgramID + "," + ProposalYear + "," + OfficeID + "", con);
                        con.Open();
                        query.CommandTimeout = 0;
                        SqlDataReader reader = query.ExecuteReader();
                        //double compute = 0;
                        while (reader.Read())
                        {
                            AccountDenomination emp = new AccountDenomination();
                            emp.DenominationName = reader.GetString(1);
                            emp.DenominationAmount = Convert.ToDecimal(reader.GetValue(2));
                            emp.AccountDenominationID = Convert.ToInt32(reader.GetValue(0));
                            emp.TransactionYear = Convert.ToInt32(reader.GetValue(6));
                            emp.isPPMP = Convert.ToInt32(reader.GetValue(9));
                            emp.ProgramID = Convert.ToInt32(reader.GetValue(7));
                            emp.AccountID = Convert.ToInt32(reader.GetValue(8));
                            emp.QuantityPercentage = Convert.ToDecimal(reader.GetValue(10));
                            emp.OfficeID = Convert.ToInt32(reader.GetValue(12));
                            emp.DenominationMonth = Convert.ToDecimal(reader.GetValue(13));
                            emp.TotalDenominationAmount = Convert.ToDecimal(reader.GetValue(11));
                            emp.Remarks = reader.GetValue(14).ToString();
                            decimal QuantityPercentageHistoryParam = 0;
                            double TotalDenominationAmountParam = 0;
                            int AccountDenominationIDParam = Convert.ToInt32(reader.GetValue(0));
                            DenominationHistory(ref QuantityPercentageHistoryParam, ref TotalDenominationAmountParam, ref AccountDenominationIDParam);

                            if (QuantityPercentageHistoryParam == 0 && TotalDenominationAmountParam == 0)
                            {
                                emp.QuantityPercentageHistory = Convert.ToDecimal(reader.GetValue(10));
                                emp.TotalDenominationAmountHistory = Convert.ToDouble(reader.GetValue(11));
                            }
                            else
                            {
                                emp.QuantityPercentageHistory = QuantityPercentageHistoryParam;
                                emp.TotalDenominationAmountHistory = TotalDenominationAmountParam;
                            }
                            // compute = compute + emp.TotalDenominationAmountHistory;
                            //emp.QuantityPercentageHistory = QuantityPercentageHistory(reader.GetValue(0));
                            //emp.QuantityPercentageHistory
                            prog.Add(emp);
                        }
                    }
                }

            }
            
            
            return prog;
        }

        public void DenominationHistory(ref decimal QuantityPercentageHistoryParam, ref double TotalDenominationAmountParam, ref int AccountDenominationIDParam)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand select_DenominationHistory = new SqlCommand(@"SELECT QuantityOrPercentage, TotalAmount FROM tbl_R_BMSDenominationHistory WHERE AccountDenominationID = '"+AccountDenominationIDParam+"' and ActionCode = 1", con);
                con.Open();
                SqlDataReader reader_DenominationHistory = select_DenominationHistory.ExecuteReader();
                while(reader_DenominationHistory.Read()){
                    QuantityPercentageHistoryParam = Convert.ToDecimal(reader_DenominationHistory.GetValue(0));
                    TotalDenominationAmountParam = Convert.ToDouble(reader_DenominationHistory.GetValue(1));
                }
            }
        }
        public void UpdateAccountHome2(IEnumerable<AccountDenomination> Accounts)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                foreach (AccountDenomination accounts in Accounts)
                {

                    SqlCommand query_time = new SqlCommand(@"SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);

                    SqlDataReader reader = query_time.ExecuteReader();
                    var timeDate = "";
                    while (reader.Read())
                    {
                        timeDate = reader.GetString(0);
                    }

                    reader.Close();

                    //SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode) values (" + accounts.AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + accounts.ProgramID + "," + accounts.ProposalYear + ",'" + timeDate + "',2,2,2," + accounts.ProposalAllotedAmount + ",0.00,1)", con);
                    //query_program.ExecuteNonQuery();
                    SqlCommand query_insert = new SqlCommand(@"insert into tbl_T_BMSAccountDenomination values('" + accounts.DenominationName + "', '" + accounts.DenominationAmount + "', '" + timeDate + "', '" + Account.UserInfo.eid + "', '111', 1, 2016)", con);
                    query_insert.ExecuteNonQuery();
                }
            }
        }
        public string CopyAllAmounts(int ProgramID, int Year, int OfficeID)
        {
            try
            {
                AdditionalRulesCaseStatement AdditionalRulesCaseStatement = new AdditionalRulesCaseStatement();
                var caseStatement = AdditionalRulesCaseStatement.CaseStatement();
                var timeDate = "";
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand query_time = new SqlCommand(@"SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);
                    con.Open();
                    SqlDataReader reader = query_time.ExecuteReader();
                    while (reader.Read())
                    {
                        timeDate = reader.GetString(0);
                    }
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal
                                                           SELECT DISTINCT e.AccountID, " + Account.UserInfo.eid + "," + ProgramID
                                                               + ", " + Year + ",'" + timeDate
                                                               + "' ," + caseStatement + ",2,2," + ""
                                                               + "a.ProposalAllotedAmount,0,1,0 FROM dbo.tbl_T_BMSBudgetProposal AS a " + ""
                                                               + "left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID and e.ActionCode = a.ProposalActionCode " + ""
                                                               + "left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID " + ""
                                                               + "left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID " + ""
                                                               + "left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID  " + ""
                                                               + "LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID " + ""
                                                               + "LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode " + ""
                                                               + "WHERE c.OfficeID = '" + OfficeID + "' and  c.ProgramID = '" + ProgramID
                                                               + "' and a.ProposalYear = " + Year + "-1 and a.ProposalActionCode = '1' and a.ProposalStatusCommittee='1' " + ""
                                                               + "and e.AccountYear='" + Year + "' and (e.AccountID  NOT in(select AccountID from tbl_T_BMSBudgetProposal where ProposalYear = '" + Year + "' and ProgramID = " + ProgramID + "))", con);
                    con.Open();
                    query_program.ExecuteNonQuery();
                    return "1";
                }
//                using (SqlConnection con = new SqlConnection(Common.MyConn()))
//                {
//                    SqlCommand query_program = new SqlCommand(@"insert into tbl_R_BMSCopiedFrom 
//                                                                select proposalID, " + Year + " - 2  from tbl_T_BMSBudgetProposal where ProposalYear = " + Year
//                                                                + " and ProgramID = " + ProgramID + " and UserID = " + Account.UserInfo.eid + " and ProposalID NOT in (SELECT ProposalID FROM tbl_R_BMSCopiedFrom)", con);
//                    con.Open();
//                    query_program.ExecuteNonQuery();
                    
//                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public string CheckAccountComputation(int AccountID, int refYear)
        {
            var AccountCode = "";

            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select isnull((select cast(accountcode as VARCHAR(MAX)) from tbl_R_BMSAccountComputation 
                                                    where AccountCode = " + AccountID + " and YearActive=" + refYear + "),'NO DATA')", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        AccountCode = reader.GetValue(0).ToString();
                    }
                }
                return AccountCode;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public AccountComputationModel getAccountComputation(int AccountID, int OfficeID, int refYear)
        {
            AccountComputationModel AccountComputationModel = new AccountComputationModel();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select top 1 isnull(b.AccountCode,0) AccountCode, a.Amount, a.NoOfMonths, a.Percentage, 
                                                  a.isRoundof, a.MaxAmount,a.EmployeeType, b.AccountName from tbl_R_BMSAccountComputation as a
                                                  LEFT JOIN  tbl_R_BMSAccounts as b on b.FMISAccountCode = a.AccountCode
                                                  where a.AccountCode = '" + AccountID + "' and a.YearActive = '" + refYear + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountComputationModel.AccountCode = reader.GetInt32(0);
                    AccountComputationModel.Amount = Convert.ToDouble(reader.GetValue(1));
                    AccountComputationModel.Month = reader.GetInt32(2);
                    AccountComputationModel.Percentage = Convert.ToDouble(reader.GetValue(3));
                    AccountComputationModel.isRoundOf = reader.GetInt16(4);
                    AccountComputationModel.MaxAmount = Convert.ToDouble(reader.GetValue(5));
                    AccountComputationModel.EmployeeType = reader.GetInt16(6);
                    AccountComputationModel.AccountName = reader.GetValue(7).ToString();
                    AccountComputationModel.ComputationID = AccountID;

                }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand comgetComputation = new SqlCommand(@"sp_BudgetYearComputation", con);
                comgetComputation.CommandType = System.Data.CommandType.StoredProcedure;
                comgetComputation.Parameters.Add(new SqlParameter("@AccountCode", AccountID));
                comgetComputation.Parameters.Add(new SqlParameter("@OfficeID", getPmisOfficeID(OfficeID)));
                comgetComputation.Parameters.Add(new SqlParameter("@refYear", refYear));
                con.Open();
                SqlDataReader reader2 = comgetComputation.ExecuteReader();
                while (reader2.Read())
                {
                    AccountComputationModel.ComputedAmount = Convert.ToDouble(reader2.GetValue(0));
                    AccountComputationModel.noOfEmployees = reader2.GetInt32(1);
                    AccountComputationModel.totalBasicSalary = Convert.ToDouble(reader2.GetValue(2));

                }
            }
            return AccountComputationModel;
        }
        public IEnumerable<EmployeeSalaryModel> getComputationDetails(int AccountID, int OfficeID, int refYear)
        {
            List<EmployeeSalaryModel> EmployeeSalaryList = new List<EmployeeSalaryModel>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand comgetComputation = new SqlCommand(@"sp_ComputationDetails", con);
                comgetComputation.CommandType = System.Data.CommandType.StoredProcedure;
                comgetComputation.Parameters.Add(new SqlParameter("@AccountCode", AccountID));
                comgetComputation.Parameters.Add(new SqlParameter("@OfficeID", getPmisOfficeID(OfficeID)));
                comgetComputation.Parameters.Add(new SqlParameter("@refYear", refYear));
                con.Open();
                SqlDataReader reader = comgetComputation.ExecuteReader();
                while (reader.Read())
                {
                    EmployeeSalaryModel EmployeeSalaryModel = new EmployeeSalaryModel();
                    EmployeeSalaryModel.EmployeeID = reader.GetValue(0).ToString();
                    EmployeeSalaryModel.EmployeeName = reader.GetValue(1).ToString();
                    EmployeeSalaryModel.BasicSalary = Convert.ToDouble(reader.GetValue(2));
                    EmployeeSalaryModel.AmountUsed = Convert.ToDouble(reader.GetValue(3));
                    EmployeeSalaryModel.GivenAmount = Convert.ToDouble(reader.GetValue(4));
                    EmployeeSalaryModel.EmployeeStatus = reader.GetValue(5).ToString();

                    EmployeeSalaryList.Add(EmployeeSalaryModel);
                }
            }
            return EmployeeSalaryList;
        }
        public int getPmisOfficeID(int OfficeID)
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
        public int getrefYear(int ProposalID)
        {
            var refYear = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select isnull(CopiedFrom,0) from tbl_R_BMSCopiedFrom where ProposalID = '" + ProposalID + "'", con);
                    con.Open();
                    refYear = Convert.ToInt32(com.ExecuteScalar().ToString());
                }
                return refYear;
            }
            catch (Exception)
            {
                return refYear;
            }
        }
        public string UpdateEmployeeOffice(int? eid, int? selectedYear, int officeID, string OfficeName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSStepIncrement set officeID = " + getPmisOfficeID(officeID)
                                                    + ", OfficeName = '" + OfficeName + "' where eid ='" + eid
                                                    + "' and DateCopied like '% " + selectedYear + " %'", con);
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
        public IEnumerable<PlantillaModel> grGetComputationDetailsAdminView(int OfficeID, int PropYear, string isCasual)
        {
            List<PlantillaModel> PlantillaList = new List<PlantillaModel>();
            if (isCasual == "Yes")
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com2 = new SqlCommand(@"dbo.sp_get_data_from_PlantillaAdminViewCasual " + PropYear + "," + getPmisOfficeID(OfficeID) + "", con);
                    con.Open();
                    SqlDataReader reader = com2.ExecuteReader();
                    while (reader.Read())
                    {
                        PlantillaModel Plantilla = new PlantillaModel();
                        Plantilla.DivName = reader.GetValue(0).ToString();
                        Plantilla.EmployeeName = reader.GetValue(1).ToString();
                        Plantilla.AppointmentDate = reader.GetDateTime(2).ToShortDateString();
                        try
                        {
                            Plantilla.StepIncrementEffectivityDate = reader.GetValue(3).ToString();
                        }
                        catch (Exception)
                        {
                            Plantilla.StepIncrementEffectivityDate = "";
                        }
                        Plantilla.sg = reader.GetValue(4).ToString();
                        Plantilla.step = reader.GetInt32(5);
                        Plantilla.StepNew = reader.GetInt32(6);
                        Plantilla.Salary = Convert.ToDouble(reader.GetValue(7));
                        Plantilla.SalaryWithStep = Convert.ToDouble(reader.GetValue(8));
                        if (reader.GetValue(1).ToString() == "Vacant")
                        {
                            Plantilla.Increase = Convert.ToDouble(reader.GetValue(11));
                        }
                        else
                        {
                            Plantilla.Increase = Convert.ToDouble(reader.GetValue(9));
                        }
                        Plantilla.IncreaseWithStep = Convert.ToDouble(reader.GetValue(10));
                        Plantilla.YearlySalary = Convert.ToDouble(reader.GetValue(11));
                        Plantilla.SeriesID = Convert.ToInt64(reader.GetValue(12));

                        PlantillaList.Add(Plantilla);
                    }
                }
            }
            else
            {
                var MotherOfficeID = 0;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT isnull(MainOfficeID_PMIS,0) 
                    FROM [dbo].[tbl_R_BMSMainAndSubOffices] where SubOfficeID_IFMIS =" + OfficeID + "", con);
                    con.Open();
                    MotherOfficeID = Convert.ToInt32(com.ExecuteScalar());

                    var Query = "";
                    if (MotherOfficeID == 0)
                    {
                        Query = @"dbo.sp_get_data_from_PlantillaAdminView " + PropYear + "," + getPmisOfficeID(OfficeID) + ",0,0";
                    }
                    else
                    {
                        Query = @"dbo.sp_get_data_from_PlantillaAdminView " + PropYear + "," + MotherOfficeID + "," + OfficeID + ",1";
                    }
                    SqlCommand com2 = new SqlCommand(Query, con);
                    SqlDataReader reader = com2.ExecuteReader();
                    while (reader.Read())
                    {
                        PlantillaModel Plantilla = new PlantillaModel();
                        Plantilla.DivName = reader.GetValue(0).ToString();
                        Plantilla.EmployeeName = reader.GetValue(1).ToString();
                        Plantilla.AppointmentDate = reader.GetDateTime(2).ToShortDateString();
                        Plantilla.StepIncrementEffectivityDate = reader.GetValue(3).ToString();
                        //try
                        //{
                        //    Plantilla.StepIncrementEffectivityDate = reader.GetDateTime(3).ToShortDateString();
                        //}
                        //catch (Exception)
                        //{
                        //    Plantilla.StepIncrementEffectivityDate = "";
                        //}
                        Plantilla.sg = reader.GetValue(4).ToString();
                        Plantilla.step = reader.GetInt32(5);
                        Plantilla.StepNew = reader.GetInt32(6);
                        Plantilla.Salary = Convert.ToDouble(reader.GetValue(7));
                        Plantilla.SalaryWithStep = Convert.ToDouble(reader.GetValue(8));
                        if (reader.GetValue(1).ToString() == "Vacant")
                        {
                            Plantilla.Increase = Convert.ToDouble(reader.GetValue(11));
                        }
                        else
                        {
                            Plantilla.Increase = Convert.ToDouble(reader.GetValue(9));
                        }
                        Plantilla.IncreaseWithStep = Convert.ToDouble(reader.GetValue(10));
                        Plantilla.YearlySalary = Convert.ToDouble(reader.GetValue(11));
                        Plantilla.SeriesID = Convert.ToInt64(reader.GetValue(12));

                        PlantillaList.Add(Plantilla);
                    }
                }
            }
            return PlantillaList;
        }
        public IEnumerable<PlantillaModel> grGetComputationDetails(int AccountCode, int OfficeID, int PropYear, int AccountID)
        {
            List<PlantillaModel> PlantillaList = new List<PlantillaModel>();
            var MotherOfficeID = 0;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                SqlCommand com = new SqlCommand(@"SELECT isnull(MainOfficeID_PMIS,0) 
                FROM [dbo].[tbl_R_BMSMainAndSubOffices] where SubOfficeID_IFMIS =" + OfficeID + "", con);
                    con.Open();
                MotherOfficeID = Convert.ToInt32(com.ExecuteScalar());

                if (MotherOfficeID == 0)
                {
                    SqlCommand com2 = new SqlCommand(@"dbo.sp_bms_getComputationTotal " + AccountCode + "," + getPmisOfficeID(OfficeID) + "," + PropYear + ",0,0,1," + AccountID + "," + Account.UserInfo.eid + "", con);
                    SqlDataReader reader = com2.ExecuteReader();
                    while (reader.Read())
                    {
                        PlantillaModel Plantilla = new PlantillaModel();
                        Plantilla.EmployeeName = reader.GetValue(0).ToString();
                        Plantilla.YearlySalary = Convert.ToDouble(reader.GetValue(1));
                        Plantilla.DivName = reader.GetValue(2).ToString();

                        PlantillaList.Add(Plantilla);
                    }
                }
            else
            {
                SqlCommand com2 = new SqlCommand(@"dbo.sp_BMS_getComputation_Transfered " + AccountCode + "," + MotherOfficeID + "," + PropYear + ",0,0, 1," + OfficeID + "," + AccountID + "," + Account.UserInfo.eid + "", con);
                    SqlDataReader reader = com2.ExecuteReader();
                while (reader.Read())
                {
                    PlantillaModel Plantilla = new PlantillaModel();
                    Plantilla.EmployeeName = reader.GetValue(0).ToString();
                    Plantilla.YearlySalary = Convert.ToDouble(reader.GetValue(1));
                        Plantilla.DivName = reader.GetValue(2).ToString();

                    PlantillaList.Add(Plantilla);
                }
            }
            }




            //List<PlantillaModel> PlantillaList = new List<PlantillaModel>();
            //var MotherOfficeID = 0;
            //if (OfficeID == 31 || OfficeID == 32 || OfficeID == 33 || OfficeID == 36)
            //{
            //    if (OfficeID == 31 || OfficeID == 32)
            //    {
            //        MotherOfficeID = 71;
            //    }
            //    else if (OfficeID == 33 || OfficeID == 36)
            //    {
            //        MotherOfficeID = 641;
            //    }
            //    using (SqlConnection con = new SqlConnection(Common.MyConn()))
            //    {
            //        SqlCommand com = new SqlCommand(@"dbo.sp_BMS_getComputation_Transfered " + AccountCode + "," + MotherOfficeID + "," + PropYear + ",0,0, 1," + OfficeID + ",0", con);
            //        con.Open();
            //        SqlDataReader reader = com.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            PlantillaModel Plantilla = new PlantillaModel();
            //            Plantilla.EmployeeName = reader.GetValue(0).ToString();
            //            Plantilla.YearlySalary = Convert.ToDouble(reader.GetValue(1));

            //            PlantillaList.Add(Plantilla);
            //        }
            //    }
            //}
            //else
            //{
            //using (SqlConnection con = new SqlConnection(Common.MyConn()))
            //{
            //    SqlCommand com = new SqlCommand(@"dbo.sp_bms_getComputationTotal " + AccountCode + "," + getPmisOfficeID(OfficeID) + "," + PropYear + ",0,0,1,0", con);
            //    con.Open();
            //    SqlDataReader reader = com.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        PlantillaModel Plantilla = new PlantillaModel();
            //        Plantilla.EmployeeName = reader.GetValue(0).ToString();
            //        Plantilla.YearlySalary = Convert.ToDouble(reader.GetValue(1));

            //        PlantillaList.Add(Plantilla);
            //    }
            //}
            //}
            return PlantillaList;
        }
        public string updateAccount_NewAmount(int? eid, int? selectedYear, int CurrentOfficeID, int ProposalYear)
        {
            List<int> ProposalCode = new List<int>();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_T_BMSBudgetProposal set ProposalStatusCommittee = 2 where ProposalID in(
                                                    select a.proposalID from tbl_T_BMSBudgetProposal as a 
                                                    LEFT JOIN tbl_R_BMSProgramAccounts as b on b.AccountID = a.AccountID 
                                                    and b.ProgramID =a.ProgramID 
                                                    and b.AccountYear = a.ProposalYear and b.ActionCode = a.ProposalActionCode
                                                    LEFT JOIN tbl_R_BMSOfficePrograms as c on c.ProgramID = b.ProgramID and c.ProgramYear = a.ProposalYear and c.ActionCode = a.ProposalActionCode
                                                    LEFT JOIN tbl_R_BMSAccounts as d on d.FMISAccountCode = a.AccountID 
                                                    LEFT JOIN tbl_R_BMSCopiedFrom as f on f.ProposalID = a.ProposalID
                                                    LEFT JOIN tbl_R_BMSAccountComputation as e on e.AccountCode = d.AccountCode and e.YearActive = f.CopiedFrom
                                                    where c.OfficeID = " + CurrentOfficeID + " and a.ProposalYear = " + ProposalYear + " and e.AccountCode != 0 AND f.CopiedFrom = " + selectedYear + ")", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();

                    SqlCommand com2 = new SqlCommand(@"select d.AccountCode from tbl_T_BMSBudgetProposal as a 
                                                    LEFT JOIN tbl_R_BMSProgramAccounts as b on b.AccountID = a.AccountID 
                                                    and b.ProgramID =a.ProgramID 
                                                    and b.AccountYear = a.ProposalYear and b.ActionCode = a.ProposalActionCode
                                                    LEFT JOIN tbl_R_BMSOfficePrograms as c on c.ProgramID = b.ProgramID and c.ProgramYear = a.ProposalYear and c.ActionCode = a.ProposalActionCode
                                                    LEFT JOIN tbl_R_BMSAccounts as d on d.FMISAccountCode = a.AccountID 
                                                    LEFT JOIN tbl_R_BMSCopiedFrom as f on f.ProposalID = a.ProposalID
                                                    LEFT JOIN tbl_R_BMSAccountComputation as e on e.AccountCode = d.AccountCode and e.YearActive =f.CopiedFrom
                                                    where c.OfficeID = " + CurrentOfficeID + " and a.ProposalYear = " + ProposalYear + " and e.AccountCode != 0 and f.CopiedFrom= " + selectedYear + "", con);
                    con.Open();
                    SqlDataReader reader2 = com2.ExecuteReader();
                    while (reader2.Read())
                    {
                        ProposalCode.Add(reader2.GetInt32(0));
                    }
                    con.Close();
                    foreach (var AccountCode in ProposalCode)
                    {
                        var Amount = 0;
                        SqlCommand com3 = new SqlCommand(@"sp_BudgetYearComputation " + AccountCode + ", " + getPmisOfficeID(CurrentOfficeID) + ", " + selectedYear + "", con);
                        con.Open();
                        Amount = Convert.ToInt32(com3.ExecuteScalar());
                        con.Close();

                        SqlCommand com4 = new SqlCommand(@"Update tbl_T_BMSBudgetProposal set ProposalAmount = " + Amount
                                                         + " where ProposalID = (select a.proposalID from tbl_T_BMSBudgetProposal as a " + ""
                                                         + "LEFT JOIN tbl_R_BMSProgramAccounts as b on b.AccountID = a.AccountID  " + ""
                                                         + " and b.ProgramID =a.ProgramID and b.AccountYear = a.ProposalYear and b.ActionCode = a.ProposalActionCode " + ""
                                                         + " LEFT JOIN tbl_R_BMSOfficePrograms as c on c.ProgramID = b.ProgramID and c.ProgramYear = a.ProposalYear and c.ActionCode = a.ProposalActionCode " + ""
                                                         + " LEFT JOIN tbl_R_BMSAccounts as d on d.FMISAccountCode = a.AccountID LEFT JOIN tbl_R_BMSCopiedFrom as f on f.ProposalID = a.ProposalID " + ""
                                                         + " LEFT JOIN tbl_R_BMSAccountComputation as e on e.AccountCode = d.AccountCode and e.YearActive =f.CopiedFrom " + ""
                                                         + " where c.OfficeID = " + CurrentOfficeID + " and a.ProposalYear = " + ProposalYear + " and e.AccountCode = " + AccountCode + ")", con);
                        con.Open();
                        com4.ExecuteNonQuery();
                        con.Close();

                        SqlCommand com5 = new SqlCommand(@"insert into tbl_R_AmountHistory 
                                                         select a.proposalID,'" + Account.UserInfo.eid.ToString() + "'," + Amount + ", GETDATE() from tbl_T_BMSBudgetProposal as a " + ""
                                                         + "LEFT JOIN tbl_R_BMSProgramAccounts as b on b.AccountID = a.AccountID  " + ""
                                                         + " and b.ProgramID =a.ProgramID and b.AccountYear = a.ProposalYear and b.ActionCode = a.ProposalActionCode " + ""
                                                         + " LEFT JOIN tbl_R_BMSOfficePrograms as c on c.ProgramID = b.ProgramID and c.ProgramYear = a.ProposalYear and c.ActionCode = a.ProposalActionCode " + ""
                                                         + " LEFT JOIN tbl_R_BMSAccounts as d on d.FMISAccountCode = a.AccountID LEFT JOIN tbl_R_BMSCopiedFrom as f on f.ProposalID = a.ProposalID " + ""
                                                         + " LEFT JOIN tbl_R_BMSAccountComputation as e on e.AccountCode = d.AccountCode and e.YearActive =f.CopiedFrom " + ""
                                                         + " where c.OfficeID = " + CurrentOfficeID + " and a.ProposalYear = " + ProposalYear + " and e.AccountCode = " + AccountCode + "", con);
                        con.Open();
                        com5.ExecuteNonQuery();
                        con.Close();

                    }



                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string SubmitProposal(double? Amount, int? AccountID, int? ProgramID, int? ProposalYear, int OfficeID, int regularaipid)
        {
            var retstr = "";
            AdditionalRulesCaseStatement AdditionalRulesCaseStatement = new AdditionalRulesCaseStatement();
            var caseStatement = AdditionalRulesCaseStatement.CaseStatement();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (Amount == null)
                {
                    return "Something went Wrong!";
                }
                else
                {
                    SqlCommand query_time = new SqlCommand(@"SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' 
                    + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' 
                    + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' 
                    + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' 
                    + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);
                    con.Open();
                    SqlDataReader reader = query_time.ExecuteReader();
                    var timeDate = "";
                    while (reader.Read())
                    {
                        timeDate = reader.GetString(0);
                    }
                    reader.Close();
                    if (Account.UserInfo.lgu == 0)
                    {
                        con.Close();
                        SqlCommand query_program = new SqlCommand(@"exec sp_BMS_SubmitProposed_v2 '" + AccountID + "', " + Account.UserInfo.eid + "," + ProgramID + ", " + ProposalYear + "  ," + caseStatement + "," + Amount + "," + regularaipid + "", con);
                        con.Open();
                        retstr = query_program.ExecuteScalar().ToString();
                        return retstr;
                    }
                    else
                    {
                        con.Close();
                        SqlCommand query_program = new SqlCommand(@"exec sp_BMS_SubmitProposed_v2 '" + AccountID + "', " + Account.UserInfo.eid + "," + ProgramID + ", " + ProposalYear + "  ," + caseStatement + "," + Amount + "", con);
                        con.Open();
                        retstr = query_program.ExecuteScalar().ToString();
                        return retstr;
                    }
                    //query_program.ExecuteNonQuery();
                    //return "success";

                    //var data = "";
                    //SqlCommand com = new SqlCommand(@"sp_bms_WFPTrasnferPreparer " + docid + "," + eid + "," + eid_dh + "", con);
                    //con.Open();
                    //data = Convert.ToString(com.ExecuteScalar());
                    //return data;
                }
            }
        }
        public string SubmitProposalForHR(int? ProposalYear, int OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("INSERT into tbl_R_BMSSubmittedForFundingData EXEC dbo.sp_bms_getForFundingData " + ProposalYear + "," + OfficeID + ",2,1", con);
                SqlCommand comUpdate = new SqlCommand("Update tbl_R_BMSProposedNewItem set ActionCode = 2 where OfficeID = " + OfficeID + " and YearOf =  " + ProposalYear + " and ActionCode = 1 ", con);
                con.Open();
                com.ExecuteNonQuery();
                comUpdate.ExecuteNonQuery();

                return "success";
            }
        }
        public string GetComputationDescription(int OfficeID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select b.Description from tbl_R_BMSOfficesWithHazardAndSubsistence as a LEFT JOIN
                                                    tbl_R_BMSHazardComputation as b on b.SeriesID = a.HazardFormulaUsed and a.ActionCode = b.ActionCode
                                                    where a.ifmisOfficeID = " + OfficeID + " and a.ActionCode = 1 and a.hasHazard = 1", con);
                    con.Open();

                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        
        public string RemoveProposal(int? AccountID,int? ProgramID, int? ProposalYear)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSProposedAccounts set ActionCode = 2 WHERE AccountID = '"+AccountID+"'", con);
                con.Open();
                try
                {
                    SqlCommand parti = new SqlCommand(@"Update [IFMIS].[dbo].[tbl_T_BMSAccountDenomination] set ActionCode  = 2, dateTimeEntered = dateTimeEntered + ','+ (
                            SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)
                            ) WHERE [ProgramID]= "+ ProgramID  + " and AccountID ='" + AccountID+ "' and actioncode=1 and [TransactionYear] =" + ProposalYear + "", con);
                    com.ExecuteNonQuery();
                    parti.ExecuteNonQuery();
                    return "success";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string SubmitAllNonOffice(int? ProposalYearParam)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                try
                {
                    SqlCommand com = new SqlCommand(@"dbo.sp_bms_SubmitAllNonOfficeAccounts " + ProposalYearParam + ", " + Account.UserInfo.eid + "", con);
                    con.Open();
                    com.ExecuteScalar().ToString();
                    return com.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string TransferSelectedCasual(int? OfficeID, int? eid)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                try
                {
                    SqlCommand com = new SqlCommand(@"insert into tbl_R_BMSVacantAndTransferedCasual values("+eid+", NULL, NULL,1," + DateTime.Now.Year + " + 1, " +  OfficeID+ ")", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string GetAccountsToCombine() 
        {
            var AccountList = "";
            
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select AccountID from tbl_R_BMSAccountsToCombine where ActionCode = 1 and isCombineWithNonOffice = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    var Comma = AccountList == "" ? "" : ",";
                    AccountList = AccountList + Comma + reader.GetValue(0).ToString();
                }
            }
            return AccountList;
        }
    }
}