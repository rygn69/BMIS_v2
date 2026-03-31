using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class ChangeTransactionCharge
    {
        public int ConnectionStatus { get; set; }
        public int ActionCode { get; set; }
        public string MTitle { get; set; }
        public string MBody { get; set; }
        public string MType { get; set; }
        public int trnno { get; set; }
        public int OfficeID { get; set; }
        public string OfficeName { get; set; }
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public double AmountDummy { get; set; }
        public int TempID { get; set; }
        public int _OldAcctCharge { get; set; }
        public double Amount { get; set; }
        public string NewOBR { get; set; }
        public int IsIncome { get; set; }
        public int ReturnStatus { get; set; }

    }
}