using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class Monthly_Realignment_Model
    {
        public Int64 realignment_id { get; set; }
        public int FromOfficeCode { get; set; }
        public int FromProgramCode { get; set; }
        public int FromAccountCode { get; set; }
        public int ToOfficeCode { get; set; }
        public int ToProgramCode { get; set; }
        public int ToAccountCode { get; set; }
        public double Amount { get; set; }
        public int UserID { get; set; }
        public string DateTimeEntered { get; set; }
        public int ActionCode { get; set; }
        public int YearOf { get; set; }
        public string Description { get; set; }
        public string officename { get; set; }
        public string user_name { get; set; }

        public Int64 realignment_id_EDIT { get; set; }
        public double Amount_EDIT { get; set; }

        public Int64 supplementalbudget_id { get; set; }
        public double Amount_Sup { get; set; }

        public double balance_amount_ps { get; set; }
        public double balance_amount_mooe { get; set; }
        public double balance_amount_co { get; set; }
        public double available_amount { get; set; }

        public double debayd_PS { get; set; }
        public double debayd_MOOE { get; set; }
        public double debayd_CO { get; set; }
        public double total_debayd { get; set; }
        public double total_debaydm { get; set; }
        public double total_debaydc { get; set; }
        public double s1 { get; set; }
        public double s2 { get; set; }
        public double s3 { get; set; }


        public double amount_totalss { get; set; }
        public Int64 total_rel_id { get; set; }
        public Int64 release_float_id { get; set; }
        public int trnno { get; set; }

        public int type_ { get; set; }


        public string FromAccountName { get; set; }
        public string ToAccountName { get; set; }

        public double ToAmount { get; set; }
    }
}