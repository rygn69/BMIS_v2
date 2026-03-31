using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation
{
    public class HRBudget_Layer
    {
        public IEnumerable<ProgramsModel> grSearchOfficeProgram(int? off_ID, int? propYear)
        {
            List<ProgramsModel> pross = new List<ProgramsModel>();
            var query = "";
            if (off_ID == 43)
            {
                query = "select ProgramID,ProgramDescription from tbl_R_BMSOfficePrograms where ProgramYear = '" + propYear + "' and OfficeID = '" + off_ID + "' and programID = 43 and actioncode=1";
            }
            else
            {
                query = "select ProgramID,ProgramDescription from tbl_R_BMSOfficePrograms where ProgramYear = '" + propYear + "' and OfficeID = '" + off_ID + "' and actioncode=1";
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProgramsModel app = new ProgramsModel();
                    app.ProgramID = reader.GetValue(0).ToString();
                    app.ProgramDescription = reader.GetString(1);
                    pross.Add(app);
                }
            }
            return pross;
        }
        public void UpdateDenomination(IEnumerable<AccountDenomination> Denomination)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                
                foreach(AccountDenomination data in Denomination){
                    decimal QuantityOrPercentage = data.QuantityPercentageHistory * data.DenominationMonth * data.DenominationAmount;
                    if (Convert.ToDouble(QuantityOrPercentage) == Convert.ToDouble(data.TotalDenominationAmountHistory))
                    {
                        QuantityOrPercentage = data.QuantityPercentageHistory;
                    }
                    else
                    {
                        QuantityOrPercentage = 0;
                    }
                    
                    var query = "";
                    if (data.AccountDenominationID == 0)
                    {
                        query = "Insert into tbl_R_BMSDenominationHistory values ('" + data.OfficeID + data.AccountID + "', '" + data.QuantityPercentageHistory + "', '" + data.TotalDenominationAmountHistory + "', '" + Account.UserInfo.eid + "', GETDATE(), 43)";
                    }
                    else
                    {
                        query = "Insert into tbl_R_BMSDenominationHistory values ('" + data.AccountDenominationID + "', '" + QuantityOrPercentage + "', '" + data.TotalDenominationAmountHistory + "', '" + Account.UserInfo.eid + "', GETDATE(), 1)";
                    }
                    SqlCommand UpdateDenominationRemark = new SqlCommand(@"sp_BMS_SaveOrUpdateRemarkDenomination " + data.AccountID + ", " + data.ProgramID + ", " + data.TransactionYear + ", " + data.OfficeID + ", '" + data.Remarks + "'", con);
                    UpdateDenominationRemark.ExecuteNonQuery();
                    SqlCommand update_DenominationHistory = new SqlCommand(@"UPDATE tbl_R_BMSDenominationHistory set ActionCode  = 2 WHERE AccountDenominationID = '" + data.AccountDenominationID + "'", con);
                    update_DenominationHistory.ExecuteNonQuery();
                    
                    SqlCommand insert_DenominationHistory = new SqlCommand(query, con);
                    insert_DenominationHistory.ExecuteNonQuery();
                }
                SqlCommand proposalID = new SqlCommand(@"SELECT ProposalID FROM tbl_T_BMSBudgetProposal WHERE ProgramID = '" + Denomination.ElementAt(0).ProgramID + "' and AccountID = '" + Denomination.ElementAt(0).AccountID + "' and ProposalYear = '" + Denomination.ElementAt(0).TransactionYear + "' and ProposalActionCode = 1 ", con);
                var proposedID = Convert.ToInt32(proposalID.ExecuteScalar());
                SqlCommand GetDenominationTotal = new SqlCommand(@"sp_BMS_GetDenominationTotal " + Denomination.ElementAt(0).AccountID + "," + Denomination.ElementAt(0).ProgramID + "," + Denomination.ElementAt(0).TransactionYear + "," + (Denomination.ElementAt(0).AccountDenominationID == 0 ? 43 : Denomination.ElementAt(0).OfficeID)  + "", con);
                var Amount = Convert.ToDouble(GetDenominationTotal.ExecuteScalar());
                SqlCommand insert_ProposalAmount = new SqlCommand(@"insert into tbl_R_AmountHistory values('" + proposedID + "', '" + Account.UserInfo.eid + "', '" + Amount + "', GETDATE())", con);
                insert_ProposalAmount.ExecuteNonQuery();

                SqlCommand updateConsol = new SqlCommand(@"Update [IFMIS].[dbo].[tbl_R_BMSConsoliditedTotal] set [BudgetYearAmount]=" + Amount + " where [YearOf]=" + Denomination.ElementAt(0).TransactionYear + " and [ProgramID]=" +
                               Denomination.ElementAt(0).ProgramID + " and [AccountID]=" + Denomination.ElementAt(0).AccountID + "", con);
                updateConsol.ExecuteNonQuery();

            }
        }
        public double SumDenomination(int? ProgramID, int? AccountID, int? TransactionYear, int? OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand sum = new SqlCommand(@"Select SUM(TotalAmount)
                            from tbl_T_BMSAccountDenomination as a
                            where a.ProgramID = '"+ProgramID+"' and a.AccountID = '"+AccountID+"' and a.TransactionYear = '"+TransactionYear+"' and a.ActionCode = 1 and a.OfficeID = '"+OfficeID+"'" +
                            "and (a.AccountDenominationID NOT in (SELECT AccountDenominationID FROM tbl_R_BMSDenominationHistory))", con);

                return Convert.ToDouble(sum.ExecuteScalar());
            }
        }
        public double SumDenominationHistory(int? ProgramID, int? AccountID, int? TransactionYear, int? OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand sum = new SqlCommand(@"Select isnull(SUM(b.TotalAmount),0)
                            from tbl_T_BMSAccountDenomination as a
                            LEFT JOIN tbl_R_BMSDenominationHistory as b ON b.AccountDenominationID = a.AccountDenominationID
                            where a.ProgramID = '"+ProgramID+"' and a.AccountID = '"+AccountID+"' and a.TransactionYear = '"+TransactionYear+"' and a.ActionCode = 1 and a.OfficeID = '"+OfficeID+"' and b.ActionCode = 1", con);
                return Convert.ToDouble(sum.ExecuteScalar());
            }
        }
        public string ZimbraEmailNotif(int? ProgramID, int? OfficeID, int? BudgetYear, int? ooe_id)
        {
            var AccountName = "";
            var AccountCode = 0;
            var ProposalAmount = 0;
            var ProposalAllotedAmount = 0;
            var UserID = 0;
            var table = "";
            var EmailAdd = "";
            var ProgramDescription = "";
            var OOEAbrevation = "";
            var OOEName = "";
            

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_email = new SqlCommand(@"SELECT DISTINCT 
                        a.ProposalID,
                        b.AccountName,
                        d.AccountCode,
                        a.ProposalAmount,
                        a.ProposalAllotedAmount,
                        a.UserID,
                        c.ProgramDescription,
                        e.OOEAbrevation,
                        e.OOEName
                        FROM
                        dbo.tbl_T_BMSBudgetProposal as a
                        INNER JOIN dbo.tbl_R_BMSProgramAccounts as b ON a.AccountID = b.AccountID
                        INNER JOIN dbo.tbl_R_BMSOfficePrograms as c ON b.ProgramID = c.ProgramID
                        INNER JOIN dbo.tbl_R_BMSAccounts as d ON b.AccountID = d.FMISAccountCode
                        INNER JOIN dbo.tbl_R_BMSObjectOfExpenditure AS e ON b.ObjectOfExpendetureID = e.OOEID
                        WHERE a.ProgramID = '" + ProgramID + "' and c.OfficeID = '" + OfficeID + "' and a.ProposalYear = '" + BudgetYear + "' and a.ProposalActionCode = 1 and a.ProposalStatusCommittee = 1 AND c.ActionCode = 1 AND c.ProgramYear = '" + BudgetYear + "'  ORDER BY e.OOEAbrevation DESC", con);
                con.Open();
                SqlDataReader reader_email = query_email.ExecuteReader();
                var OOE = "";
                var header_table = "";
                var data_table = "";
                var data = "";
                while(reader_email.Read()){
                    AccountName = reader_email.GetString(1);
                    AccountCode = Convert.ToInt32(reader_email.GetValue(2));
                    ProposalAmount = Convert.ToInt32(reader_email.GetValue(3));
                    ProposalAllotedAmount = Convert.ToInt32(reader_email.GetValue(4));
                    UserID = Convert.ToInt32(reader_email.GetValue(5));
                    ProgramDescription = reader_email.GetString(6);
                    OOEAbrevation = reader_email.GetString(7);
                    OOEName = reader_email.GetString(8);

                    if(OOE != OOEName){
                        header_table = "<tr> <td colspan = 4> "+OOEName +"</td> </tr>";
                    }
                    else
                    {
                        header_table = "";
                    }
                    table = "<tr ><td style='padding:10px;'> " + AccountName + "</td> <td style='padding:10px;'>" + AccountCode + "</td> <td style='padding:10px;'> ₱ " + ProposalAmount + " </td> <td style='padding:10px;'> ₱ " + ProposalAllotedAmount + " </td></tr>";
                    data_table = header_table + table;
                    data = data + data_table;
                    OOE = OOEName;
                }
                reader_email.Close();
                SqlCommand query_UserEmail = new SqlCommand(@"SELECT pmis.dbo.vwMergeAllEmployee.EmailAdd FROM pmis.dbo.vwMergeAllEmployee WHERE pmis.dbo.vwMergeAllEmployee.eid = '" + UserID + "'", con);
                SqlDataReader read_UserEmail = query_UserEmail.ExecuteReader();
                while (read_UserEmail.Read())
                {
                    EmailAdd = read_UserEmail.GetString(0);
                }
                read_UserEmail.Close();

                if (table != "")
                {
                    string sender = "ranel.cator@pgas.ph";
                    MailMessage mail = new MailMessage();
                    //mail.To.Add("ranel.cator@pgas.gov");
                    mail.To.Add(EmailAdd);
                    mail.From = new MailAddress(sender, "iFMIS - BMS");
                    mail.Subject = "Notice Changes in Amount";
                    mail.Priority = MailPriority.High;
                    mail.Body = @"
                        <style>
                        table {
                            font-family:Tahoma;
                            border-collapse:collapse;
                        }
                        th {
                            padding: 15px;
                            font-weight:500;
                            background-color:#CEE9E0;
                        }
                        </style>
                        <p style='font-family:Tahoma;size:18px;'> <b> Program: </b> "+ProgramDescription+"</p>" +
                        "<p style='font-family:Tahoma;size:18px;'> <b> Budget Year: </b>"+BudgetYear+"</p> "+
                        "<p style='font-family:Tahoma;size:12px;'>The Following Amount of listed Account is Changed " +
                            "<br /> <table border='1'> " +
                            "<tr>" +
                            "<th> Account Name </th> " +
                            "<th> Account Code </th>" +
                            "<th> Proposed Amount </th> " +
                            "<th> Alloted Amount </th> " +
                            "</tr> " +
                            "" + data + "</table> <p> <i> This is a system generated email </i> </p>";
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "192.168.2.101";
                    smtp.Send(mail);
                }

            }
            return "success";
        }
        public string ApproveNewAccount(int Yearof, int OfficeID, int AccountCode, string AccountName, double ApprovedAmount, int ProposalID, int ProgramID, int OOEID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_ApproveNewProposedAccounts '" + AccountName + "'," + AccountCode + ", " + ProgramID 
                    + ", " + OOEID + ", " + Account.UserInfo.eid + ", " + Yearof + "," + ApprovedAmount + "," + ProposalID + " ", con);
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
        public IEnumerable<AccountsModel> grSearchOfficeAccounts(int? prog_ID, int? off_ID, int? propYear)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DISTINCT e.AccountID,  e.AccountName,  f.OOEAbrevation,a.ProposalYear,a.ProposalAllotedAmount,e.AccountYear,e.ProgramID FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                                                    WHERE c.OfficeID = '" + off_ID + "' and  c.ProgramID = '" + prog_ID + "' and a.ProposalYear = " + propYear
                                                    + "-1 and a.ProposalActionCode = '1' and a.ProposalStatusCommittee='1' and e.AccountYear='" + propYear + "' and (e.AccountID  NOT in(select AccountID from tbl_T_BMSBudgetProposal where ProposalYear = '" + propYear + "' )) ORDER BY AccountName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.AccountID = Convert.ToInt32(reader.GetValue(0));
                    emp.AccountName = reader.GetString(1);
                    // emp.FundName = reader.GetString(2);
                    emp.OOEName = reader.GetString(2);
                    emp.PastProposalYear = reader.GetInt32(3);
                    emp.PastProposalAmmount = Convert.ToDouble(reader.GetValue(4));
                    //emp.UserID = reader.GetString(6);
                    emp.ProposalYear = reader.GetInt32(5);
                    emp.ProgramID = reader.GetInt32(6);

                    prog.Add(emp);
                }
            }
            return prog;
        }
        public void UpdateSelectedOfficeAccount(IEnumerable<AccountsModel> Accounts)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                foreach (AccountsModel Accountss in Accounts)
                {

                    SqlCommand query_time = new SqlCommand(@"SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);

                    SqlDataReader reader = query_time.ExecuteReader();
                    var timeDate = "";
                    while (reader.Read())
                    {
                        timeDate = reader.GetString(0);
                    }

                    reader.Close();
                    SqlCommand com = new SqlCommand("update a set ProposalActionCode= 2  FROM dbo.tbl_T_BMSBudgetProposal AS a left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode WHERE  a.AccountID = " + Accountss.AccountID + "and a.ProposalYear = " + Accountss.ProposalYear, con);
                    com.ExecuteNonQuery();

                    SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode) values (" + Accountss.AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + Accountss.ProgramID + "," + Accountss.ProposalYear + ",'" + timeDate + "',1,1,1, " + Accountss.ProposalAllotedAmount + "," + Accountss.ProposalAllotedAmount + ",1)", con);
                    query_program.ExecuteNonQuery();
                }
            }
        }
        public void UpdateProposedAccount(IEnumerable<AccountsModel> ProposedAccounts)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                var userID = Account.UserInfo.eid;
                var UserType = Account.UserInfo.UserTypeDesc;
                foreach (AccountsModel Data in ProposedAccounts)
                {
                    SqlCommand updateRemarks = new SqlCommand(@"SELECT * FROM tbl_R_BMSProposalRemark WHERE ProposalID = '"+Data.ProposalID+"'"+
                                "IF @@ROWCOUNT=0 insert into tbl_R_BMSProposalRemark values('" + Data.ProposalID + "', '" + userID + "', '" + Data.setRemarks + "', '" + UserType + "')" +
                                "ELSE Update tbl_R_BMSProposalRemark set Remarks = '"+Data.setRemarks+"' WHERE ProposalID = '"+Data.ProposalID+"'", con);
                    updateRemarks.ExecuteNonQuery();

                    SqlCommand updateBudget = new SqlCommand(@"INSERT into tbl_R_AmountHistory values('" + Data.ProposalID + "', '" + userID + "', '" + Data.SlashAmount + "', GETDATE())", con);
                    updateBudget.ExecuteNonQuery();

                    SqlCommand updateProp = new SqlCommand(@"Update [IFMIS].[dbo].[tbl_T_BMSProposalAmount] set [amount]=" + Data.SlashAmount + " where [actioncode]=1 and [yearof]=" + Data.ProposalYear + " and [programid]=" +
                                Data.NewProgramID + " and [accountid]=" + Data.NewAccountID + "", con);
                    updateProp.ExecuteNonQuery();

                    SqlCommand updateConsol = new SqlCommand(@"Update [IFMIS].[dbo].[tbl_R_BMSConsoliditedTotal] set [BudgetYearAmount]=" + Data.SlashAmount + " where [YearOf]=" + Data.ProposalYear + " and [ProgramID]=" +
                                Data.NewProgramID + " and [AccountID]=" + Data.NewAccountID + "", con);
                    updateConsol.ExecuteNonQuery();

                    if (Account.UserInfo.lgu == 1)
                    {
                        //SqlCommand insertCurryear = new SqlCommand(@"INSERT into [IFMIS].[dbo].[tbl_R_BMSCurrentYearAppropriation] ([Appropriations],[AccountID],[AccountName],[ProgramID],[OfficeID],[YearOf]) 
                        //                                            values(" + Data.PastProposalAmmount + "," + Data.NewAccountID + ",'"+ Data.AccountName +"'," + Data.NewProgramID + ","+ Data.NewOffice + ","+ (Data.ProposalYear -1) + ")", con);
                        //insertCurryear.ExecuteNonQuery();

                        SqlCommand updatePastyear = new SqlCommand(@"Update [IFMIS].[dbo].[tbl_R_BMSObligatedAccounts] set ActionCode=2 where [actioncode]=1 and ([YearOf]=" + (Data.ProposalYear -1) + " or [YearOf]=" + (Data.ProposalYear - 2) + ") and [fmisProgramCode]=" +
                               Data.NewProgramID + " and [fmisAccountCode]=" + Data.NewAccountID + "", con);
                        updatePastyear.ExecuteNonQuery();

                        SqlCommand insertCurryear = new SqlCommand(@"INSERT into [IFMIS].[dbo].[tbl_R_BMSObligatedAccounts] ([fmisOfficeID],[fmisProgramCode],[fmisAccountCode],[Appropriations],[YearOf],ActionCode) 
                                                                    values(" + Data.NewOffice + "," + Data.NewProgramID + "," + Data.NewAccountID + "," + Data.PastProposalAmmount + "," + (Data.ProposalYear - 1) + ",1)", con);
                        insertCurryear.ExecuteNonQuery();

                        SqlCommand insertPastyear = new SqlCommand(@"INSERT into [IFMIS].[dbo].[tbl_R_BMSObligatedAccounts] ([fmisOfficeID],[fmisProgramCode],[fmisAccountCode],[Obligation],[YearOf],ActionCode) 
                                                                    values(" + Data.NewOffice + "," + Data.NewProgramID + "," + Data.NewAccountID + "," + Data.PastYear + "," + (Data.ProposalYear - 2) + ",1)", con);
                        insertPastyear.ExecuteNonQuery();

                    }
                }
            }
        }
        public string RemoveProposal(int? ProposalID=0,int? office=0,int? proyear=0, int? supplementalaipid = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    con.Open();
                    if (supplementalaipid == 0)
                    {
                        SqlCommand com = new SqlCommand("sp_BMS_ReturnProposal " + ProposalID + "," + office + "," + proyear + "," + Account.UserInfo.eid + "", con); // e update ra ni ug 0 ang actioncode kung ok na tanan                                                                                                                                            //SqlCommand com = new SqlCommand("update tbl_T_BMSBudgetProposal set ProposalActionCode= 9 where ProposalID = '" + ProposalID + "'", con); // e update ra ni ug 0 ang actioncode kung ok na tanan
                        com.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand("sp_BMS_ReturnProposal_Supplemental " + ProposalID + "," + office + "," + proyear + "," + Account.UserInfo.eid + "", con); // e update ra ni ug 0 ang actioncode kung ok na tanan                                                                                                                                        //SqlCommand com = new SqlCommand("update tbl_T_BMSBudgetProposal set ProposalActionCode= 9 where ProposalID = '" + ProposalID + "'", con); // e update ra ni ug 0 ang actioncode kung ok na tanan
                        com.ExecuteNonQuery();
                    }
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public void UpdateDenomination_propose(IEnumerable<BudgetControlModel> Denomination)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();

                foreach (BudgetControlModel data in Denomination)
                {
                    decimal totalAmount = Convert.ToDecimal(data.qty2) * data.Amount;
                    string remark = data.Remarks == null? "" : data.Remarks.Replace("'", "''");
                    //if (Convert.ToDouble(QuantityOrPercentage) == Convert.ToDouble(data.TotalDenominationAmountHistory))
                    //{
                    //    QuantityOrPercentage = data.QuantityPercentageHistory;
                    //}
                    //else
                    //{
                    //    QuantityOrPercentage = 0;
                    //}

                    var query = "";
                 
                    query = "Insert into ifmis.dbo.tbl_R_BMSDenominationRemark_v2 values ('" + data.trnno_id + "',1,year(GETDATE()), '" + Account.UserInfo.eid + "','"+ remark + "')";
                    SqlCommand insert_DenominationHistory = new SqlCommand(query, con);
                    insert_DenominationHistory.ExecuteNonQuery();
                    SqlCommand UpdateDenominationLog = new SqlCommand(@"sp_BMS_ProposalUpdatePerPIMO " + data.trnno_id + ", " + Account.UserInfo.eid + "", con);
                    UpdateDenominationLog.ExecuteNonQuery();
                    SqlCommand update_DenominationHistory = new SqlCommand(@"UPDATE [IFMIS].[dbo].[tbl_T_BMSAccountDenomination] set DenominationAmount="+ data.Amount + ",QuantityOrPercentage="+ data.qty2 + ",TotalAmount="+ totalAmount + "" +
                                        "WHERE AccountDenominationID = '" + data.trnno_id + "' and actioncode=1", con);
                    update_DenominationHistory.ExecuteNonQuery();

                }
                //SqlCommand proposalID = new SqlCommand(@"SELECT ProposalID FROM tbl_T_BMSBudgetProposal WHERE ProgramID = '" + Denomination.ElementAt(0).ProgramID + "' and AccountID = '" + Denomination.ElementAt(0).AccountID + "' and ProposalYear = '" + Denomination.ElementAt(0).TransactionYear + "' and ProposalActionCode = 1 ", con);
                //var proposedID = Convert.ToInt32(proposalID.ExecuteScalar());
                //SqlCommand GetDenominationTotal = new SqlCommand(@"sp_BMS_GetDenominationTotal " + Denomination.ElementAt(0).AccountID + "," + Denomination.ElementAt(0).ProgramID + "," + Denomination.ElementAt(0).TransactionYear + "," + (Denomination.ElementAt(0).AccountDenominationID == 0 ? 43 : Denomination.ElementAt(0).OfficeID) + "", con);
                //var Amount = Convert.ToDouble(GetDenominationTotal.ExecuteScalar());
                //SqlCommand insert_ProposalAmount = new SqlCommand(@"insert into tbl_R_AmountHistory values('" + proposedID + "', '" + Account.UserInfo.eid + "', '" + Amount + "', GETDATE())", con);
                //insert_ProposalAmount.ExecuteNonQuery();
            }
        }
    }
}