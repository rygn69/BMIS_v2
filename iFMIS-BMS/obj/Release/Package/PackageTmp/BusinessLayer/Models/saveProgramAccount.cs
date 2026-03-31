using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class saveProgramAccount
    {
        public int office_id { get; set; }
        public int program_id { get; set; }
        public int ref_account_id { get; set; }
        public int ooe_id { get; set; }
        public int orderBy { get; set; }
        public int text_office_id { get; set; }
        public string account_name { get; set; }
        public string ChildAccountCode { get; set; }
        public string text_AccountName { get; set; }
        public int text_CAC { get; set; }

        // NEW
        public int AccountID { get; set; }
        public int AccountCode { get; set; }
        public int ProposalYear { get; set; }
        public int isProposed { get; set; }


    }
}