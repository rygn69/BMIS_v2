using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class saveAccount
    {
        public string account_desc { get; set; }
        public string code { get; set; }
        public string child_series_no { get; set; }
        public string child_account_code { get; set; }
        public string fund_id { get; set; }
        public int x { get; set; }
        public int datepicker { get; set; }
        public string ThirdLevelGroup { get; set; }
        public int AccountCode { get; set; }
        public string ChildSeriesNo { get; set; }
        public string FundType { get; set; }
        public string ChildAccountCode { get; set; }
        public int FMISAccountCode { get; set; }
        public int ProposalID { get; set; }
    }
}