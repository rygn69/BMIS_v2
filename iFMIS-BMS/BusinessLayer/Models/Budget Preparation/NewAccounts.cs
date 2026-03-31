using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Budget_Preparation
{
    public class NewAccounts
    {
        public double ProposalID { get; set; }
        public double ProposalAmount { get; set; }
        public string AccountName { get; set; }
        public string OOE { get; set; }
        public int ProposalYear { get; set; }
        public string ProposalDateTime { get; set; }
        public int ProposalStatusHR { get; set; }
        public int? ProgramID { get; set; }
    }
}