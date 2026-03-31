using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class Monthly_reserve_Model
    {
        public Int64 reserve_id { get; set; }
        public int FMISOfficeCode { get; set; }
        public int FMISProgramCode { get; set; }
        public int FMISAccountCode { get; set; }
        public double ReservePercent { get; set; }
        public int ActionCode { get; set; }
        public int UserID { get; set; }
        public string DateTimeEntered { get; set; }
        public int YearOf { get; set; }
        public double Amount { get; set; }
        public int reserve_flag { get; set; }
        public string account_name { get; set; }

        public double percentInmoney { get; set; }
        public double moneyInpercent { get; set; }


        public int reserve_accounts { get; set; }
        
    }
}