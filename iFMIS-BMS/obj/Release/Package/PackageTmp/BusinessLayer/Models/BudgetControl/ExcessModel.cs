
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.BudgetControl
{
    public class ExcessModel
    {
        public int grExcessID { get; set; }
        public string grAccount { get; set; }
        public double grAmount { get; set; }
        public int grYearOf { get; set; }
        public int grFundFlag { get; set; }
        public int? TransactionYear { get; set; }
        public string OBRNo { get; set; }
        public string OBRSeries { get; set; }
        public int PPAID { get; set; }
        public string PPAAccount { get; set; }
        public string grOBRNo { get; set; }
        public string grDescription { get; set; }
        public int grTrnnoID { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int FundFlag { get; set; }
        public string ExcessAppropriationNo { get; set; }
        public double Appropriation { get; set; }
        public double Obligation { get; set; }
        public double Allotment { get; set; }
        public double Balance { get; set; }
        public int Office { get; set; }
        public string Program { get; set; }
        public string NonOfficeAccount { get; set; }
        public int AcctChecker { get; set; }
        public int trnno { get; set; }
        public int ppaid { get; set; }
        public int ExcessAppropriationID { get; set; }
        public string message { get; set; }
        public int ProgramID { get; set; }
        public int AccountID { get; set; }
        // UPDATE
        public int ConnectionStatus { get; set; }
        public string MTitle { get; set; }
        public string MBody { get; set; }
        public string MType { get; set; }
        public double Difference { get; set; }
        public int Count { get; set; }
        public int spoid { get; set; }
        public string obrnotemp { get; set; }
        public Int64 transno { get; set; }
        public string username { get; set; }
        public string seriesno { get; set; }
    }
}