using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class Release_Float_Edit
    {
        public Int64 release_float_id { get; set; }
        public int FMISOfficeCode { get; set; }
        public int FMISProgramCode { get; set; }
        public decimal Amount_release { get; set; }
        public int MonthOf { get; set; }
        public int YearOf { get; set; }
        public int FMISAccountCode { get; set; }
        public int batch { get; set; }
        public int ooe_ids { get; set; }



        public string TransactionNo { get; set; }
        public string Account_Name { get; set; }



        public double AmountRelease { get; set; }
        public double AmountObligate { get; set; }
        public double AmountRemain { get; set; }




    }
}