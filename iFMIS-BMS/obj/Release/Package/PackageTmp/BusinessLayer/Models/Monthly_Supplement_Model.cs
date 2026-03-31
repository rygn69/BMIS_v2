using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class Monthly_Supplement_Model
    {
        public Int64 supplementalbudget_id { get; set; }
        public Int64 LegalCode { get; set; }
        public int FMISOfficeCode { get; set; }
        public int FMISProgramCode { get; set; }
        public int FMISAccountCode { get; set; }
        public int OOECode { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int MonthOf { get; set; }
        public int YearOf { get; set; }
        public string UserID { get; set; }
        public string DateTimeEntered { get; set; }


        public double AmountPS { get; set; }
        public double AmountMOOE { get; set; }
        public double AmountCO { get; set; }
        public string DateReleased { get; set; }

        public double BalancePS { get; set; }
        public double BalanceMOOE { get; set; }
        public double BalanceCO { get; set; }
        public int Batch { get; set; }
        public int WithSubsidyFlag { get; set; }



        public Int64 supplementaltransfere_id { get; set; }
        public Int64 supplementalreverse_id { get; set; }
      
        public string OfficeAbbrivation { get; set; }
        public string MonthOf_ { get; set; }
        public string AccountName { get; set; }
       

    }
}