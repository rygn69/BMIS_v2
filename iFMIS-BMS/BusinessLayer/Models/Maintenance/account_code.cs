using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class account_code
    {
        public int code_id { get; set; }
        public string code_desc { get; set; }
        public string code { get; set; }
        public string account_desc { get; set; }
        public int ref_account_id { get; set; }
        public int fund_id { get; set; }
        public string child_series_no { get; set; }
        public string office_desc { get; set; }
        public string program_desc { get; set; }
        public int account_id { get; set; }
        public int program_id { get; set; }
        public int office_id { get; set; }
        public int officeID { get; set; }
        public int programID { get; set; }
        public int ref_accountID { get; set; }
        public string ThirdLevelGroup { get; set; }
        public int AccountCode { get; set; }
        public string ChildSeriesNo { get; set; }
        public string FundType { get; set; }
        public string ChildAccountCode { get; set; }
        public int FMISAccountCode { get; set; }
        public int OrderNo { get; set; }
        public string Account_Name { get; set; }
        public string AccountName { get; set; }
        public int ProposalID { get; set; }
        public string ProposalDate { get; set; }
        public int OOEid { get; set; }
        public string OOEName { get; set; }
        public long PayrollBatchNo { get; set; }
        public string obrno { get; set; }
        public int location { get; set; }
        public DateTime DTE { get; set; }
        public long trnno { get; set; }
        public string obrseries { get; set; }
        public string NonOfficeTransNo { get; set; }
        public string ReferenceNo { get; set; }
        public long TransactionNo { get; set; }
        public int actioncode { get; set; }
        public string datetimeentered { get; set; }
        //public long useridout { get; set; }
        //public string datetimeout { get; set; }
    }
}