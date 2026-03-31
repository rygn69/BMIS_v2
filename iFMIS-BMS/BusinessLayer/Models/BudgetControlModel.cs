using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class BudgetControlModel
    {
        public string OBRValue { get; set; }
        public int trnno_id { get; set; }
        public string UserID { get; set; }
        public int OBRSeqNo { get; set; }
        public string OBRNo { get; set; }
        public string officeabbr { get; set; }
        public int OfficeID { get; set; }
        public int ProgramID { get; set; }
        public string OOE_Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int ModeOfExpenseID { get; set; }
        public int TransTypeID { get; set; }
        public string FundType { get; set; }
        public string PTOAccountCode { get; set; }
        public int Posted { get; set; }
        public int Income { get; set; }
        public int PaidByPTO { get; set; }
        public int ActionCode { get; set; }
        public string DateTimeEntered { get; set; }
        public string OfficeName { get; set; }
        public string TransTypeName { get; set; }
        public string ModeOfExpenseName { get; set; }
        public int FundTypeID { get; set; }
        public string FundTypeName { get; set; }
        public int OOE_ID { get; set; }
        public int item_ID { get; set; }
        public string item_data { get; set; }
        public int eid { get; set; }
        public string Employee_Name { get; set; }
        public long AccountID { get; set; }
        public string AccountName { get; set; }
        public double TotalObligate { get; set; }
        public double AmountPS { get; set; }
        public double AmountMOOE { get; set; }
        public double AmountCO { get; set; }
        public int grProgramID { get; set; }
        public long grAcctCharge { get; set; }
        public long grAcctCode { get; set; }
        public int grFundCode { get; set; }
        public int grTransType { get; set; }
        public int grModeOfExpense { get; set; }
        public int grOOEID { get; set; }
        public double grAmount { get; set; }
        public string grAccountName { get; set; }
        public string grDateTimeEntered { get; set; }
        public string grOBRNo { get; set; }
        public int grOffice { get; set; }
        public string grDescription { get; set; }
        public double grTotalRelease { get; set; }
        public double grTotalObligated { get; set; }
        public double grTotalAllotmentAvailable { get; set; }
        public double grClaim { get; set; }
        public double grTotalAllotmentBalance { get; set; }
        public double grChargeAllotmentAvailalble { get; set; }
        public double grChargeAmount { get; set; }
        public double grChargeBalance { get; set; }
        public int grAcctChecker { get; set; }
        public int grModePayment { get; set; }

        // Added for Serialize
        public int TransactionType { get; set; }
        public int ModeOfExpense { get; set; }
        public int Office { get; set; }
        public int Program { get; set; }
        public int ModePayment { get; set; }
        public long ObjOfExpenditure { get; set; }
        public string TEVControlNoText { get; set; }
        public int TEVControlNo { get; set; }
        public int PPANonOffice { get; set; }
        public int SusProgram { get; set; }
        public int OOE { get; set; }
        public string ExpenseDescription { get; set; }
        public int SubsidyIncome { get; set; }
        public double AllotedAmountValue { get; set; }
        public double ObligateValue { get; set; }
        public double DiffObliandAllocatedValue { get; set; }
        public double ClaimValue { get; set; }
        public double BalanceAllotmentValue { get; set; }
        public int AccountCharged { get; set; }
        public int ActualAccount { get; set; }
        public double AccntChargeValue { get; set; }
        public double BalanceAllotmentAppointmentValue { get; set; }
        public int FundTypeValue { get; set; }
        public int ActualAmountValue { get; set; }
        public string OBRNoCenter { get; set; }
        public string OOEText { get; set; }
        public int grifExist { get; set; }
        public long grtrnnoID { get; set; }
        public double AmountInputed { get; set; }
        public string grTEVControlNo { get; set; }
        public int greID { get; set; }
        public double grAmountDummy { get; set; }
        public double CurrentAllotment { get; set; }
        public double CurrentAllotmentValue { get; set; }
        public int grNonOffice { get; set; }
        public string Remarks { get; set; }
        public int ORNumber { get; set; }
        public string grRemarks { get; set; }
        public int grORNumber { get; set; }
        public string grClaimant { get; set; }
        public int grSubsidyIncome { get; set; }
        public string TransactionNo { get; set; }
        public string AROBRNo { get; set; }
        public string AROBRSeries { get; set; }
        public int AROffice { get; set; }
        public int ARProgram { get; set; }
        public string ARActualOffice { get; set; }
        public int ARAccount { get; set; }
        public decimal ARAmount { get; set; }
        public int ARIncome { get; set; }
        public int AROOE { get; set; }
        public int ARIfControlExist { get; set; }
        public string grRefNo { get; set; }
        public int PastAccount { get; set; }

        // PPA Control
        public int RootPPA { get; set; }
        public int PPA { get; set; }
        public int PPAOOE { get; set; }
        public string PPADescription { get; set; }
        public double _AllotedAmount { get; set; }
        public double _Obligate { get; set; }
        public double _PPARelease { get; set; }
        public double _PPAObligate { get; set; }
        public double _PPAvailable { get; set; }
        public double _Claim { get; set; }
        public double PPATotalClaim { get; set; }
        public string PPAOBRNo { get; set; }
        public string PPAControlNo { get; set; }
        public int PPATransactionType { get; set; }
        public int PPAModeofExpense { get; set; }
        public string TransOBRNo { get; set; }
        public int _PPAID { get; set; }
        public int _SubsidyID { get; set; }
        public int _RefIdentifer { get; set; }
        public int PPAFundType { get; set; }

        //grid PPA
        public int grRootPPA { get; set; }
        public string grPPAAccountName { get; set; }
        public int grPPA { get; set; }
        public int grPPAModeOfExpense { get; set; }
        public int grPPATransactionType { get; set; }
        public double grPPAClaim { get; set; }
        public int grPPAOOE { get; set; }
        public string grPPADescription { get; set; }
        public int grPPAID { get; set; }
        public string grPPAOBR { get; set; }
        public double grPPATotalClaim { get; set; }
        public int grPPAAcctChecker { get; set; }
        public int grSubsidyID { get; set; }

        // grid PPA

        // End PPA Control

        // grid Non Office Code
        public int grNonOfficeAcctID { get; set; }
        public string grNonOfficeAcctName { get; set; }
        public int grNonOfficeFunctionCode { get; set; }
        // End Non Office Code
        public int? TransactionYear { get; set; }
        public string message { get; set; }
        public int? _RefIdentifier { get; set; }
        public string OBRNoEntry { get; set; }
        public string PPATransOBRNo { get; set; }
        public int ExcessAppropriationID { get; set; }
        public int PPAID { get; set; }
        public int RootPPAID { get; set; }
        public int FromAirMark { get; set; }
        public int FromOBR { get; set; }
        public string type { get; set; }
        public string UserTransactionID { get; set; }
        public decimal AllotedAmount { get; set; }
        public decimal ObligatedAmount { get; set; }
        public int CountRow { get; set; }
        public int IfExist { get; set; }
        public string UserINTimeStamp { get; set; }
        public int ReturnStatus { get; set; }
        public string MTitle { get; set; }
        public string MBody { get; set; }
        public string MType { get; set; }

        //Batch No.
        public long batchno { get; set; }
        public long batchno2 { get; set; }
        public double grossAmount { get; set; }
        public string payee { get; set; }
        public string particular { get; set; }
        public decimal sumTotal { get; set; }
        public string address { get; set; }
        public string date_ { get; set; }
        public string rc { get; set; }
        public string username { get; set; }
        public long transno { get; set; }
        public string datetimein { get; set; }
        public string datetimeout { get; set; }
        public int transmode { get; set; }
        public string enduser { get; set; }
        public string programname { get; set; }
        public int subppa { get; set; }
        public int positionid { get; set; }
        public int scheduleid { get; set; }
        public string posname { get; set; }
        public int sg { get; set; }
        public string appointmentdate { get; set; }
        public int noOfPersonnel { get; set; }
        public double Salary { get; set; }
        public double totalSalary { get; set; }
        public int programidplantilla { get; set; }
        public int accountidplantilla { get; set; }
        public decimal app1 { get; set; }
        public decimal app2 { get; set; }
        public decimal app3 { get; set; }
        public decimal allot1 { get; set; }
        public decimal allot2 { get; set; }
        public decimal allot3 { get; set; }
        public decimal obligate1 { get; set; }
        public decimal obligate2 { get; set; }
        public decimal obligate3 { get; set; }
        public decimal app_per1 { get; set; }
        public decimal app_per2 { get; set; }
        public decimal app_per3 { get; set; }
        public decimal allot_per1 { get; set; }
        public decimal allot_per2 { get; set; }
        public decimal allot_per3 { get; set; }

        public double ooeid_uti { get; set; }
        public double qty { get; set; }
        public double qty2 { get; set; }
        public decimal totalamount { get; set; }
        public int yearacquired { get; set; }
        public double RefAmount { get; set; }
        public string obrseries { get; set; }
        public int functioncode { get; set; }
        public int lguid { get; set; }
        public int PPAOffice { get; set; }
        public int PPAProgram { get; set; }
        public string datetimeverified { get; set; }
        
        //public int NewProgramID { get; set; }
        //public int NewAccountID { get; set; }
        //public int ProposalYear { get; set; }
        //public int NewOffice { get; set; }
    }
}
//Added - Ranel cator