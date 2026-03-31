using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.BudgetControl
{
    public class OBRLogger
    {
        public string grtrnno { get; set; }
        public string grOBRNo { get; set; }
        public string grOBRSeries { get; set; }
        public int grUserIDIn { get; set; }
        public string grUserIDOut { get; set; }
        public string TransactionNo { get; set; }
        public string OBRNo { get; set; }
        public string DateTimeStamp { get; set; }
        public string Description { get; set; }
        public string ReferenceNo { get; set; }
        public string ClaimantEmployee { get; set; }
        public double Amount { get; set; }
        public int FundID { get; set; }
        public string DateTimeIN { get; set; }
        public string DateTimeOut { get; set; }
        public int UserIDOut { get; set; }
        public string OBRNowithFnCode { get; set; }
        public int YearOf { get; set; }
        public int grOBRID { get; set; }
        public int? trnno { get; set; }
        public int id { get; set; }
        public string UserINTimeStamp { get; set; }
        public string UserOutTimeStamp { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public string title { get; set; }
        public string cttsno { get; set; }
        public int? office { get; set; }
        public string claimant { get; set; }
        public string TAmount { get; set; }
        public int? verify_tag { get; set; }
        public string account { get; set; }
        public int? officeassign { get; set; }
        public string program { get; set; }
        public int? employeeassign { get; set; }
        public string otherindividual { get; set; }
        public string datetimeverified { get; set; }
        public long? approveby { get; set; }
    }
}