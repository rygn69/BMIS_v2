using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class Monthly_Income
    {

        public Int64 income_id { get; set; }
        public int FMISOfficeCode { get; set; }
        public int MonthOf { get; set; }
        public double Amount { get; set; }
        public int YearOf { get; set; }
        public string UserID { get; set; }
        public string DateTimeEntered { get; set; }
        public string Months_ { get; set; }

    }
}