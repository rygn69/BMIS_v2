using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class Monthly_Annuals
    {

        public Int64 Binding_ID { get; set; }
        public int OfficeCode { get; set; }
        public int OfficeCodeBind { get; set; }
        public int FmisProgramCode { get; set; }
        public int AccountCodeBind { get; set; }
        public int YearOf { get; set; }
        public double ProposalAllotedAmount { get; set; }
        public string AccountName { get; set; }


        public Int64 income_id { get; set; }
        public int FMISOfficeCode { get; set; }
        public string MonthOf { get; set; }
        public double Amount_inc { get; set; }
      
        
    

    }
}   