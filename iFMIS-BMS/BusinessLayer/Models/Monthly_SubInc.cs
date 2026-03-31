using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class Monthly_SubInc
    {
        //----Subsidy
        public Int64 subsidyrelease_float_id { get; set; }
        public int FMISOfficeCode { get; set; }
        public double Amount { get; set; }
        public int MonthOf { get; set; }
        public int YearOf { get; set; }
        public int Batch { get; set; }
        public int Float_Flag { get; set; }

        public string amount_description { get; set; }
        public double balance_amount { get; set; }

        //----Income
        public Int64 incomerelease_float_id { get; set; }
        public string MonthOf_D { get; set; }

        public double sub_available { get; set; }
        public double inc_available { get; set; }

        public Int64 release_float_id { get; set; }

        public double AmountPS { get; set; }
        public double AmountMOOE { get; set; }
        public double AmountCO { get; set; }
        public string DateTimeEntered { get; set; }

    }
}