using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class BudgetProposalModel
    {
        public Int64 ProposalID { get; set; }
        public int AccountID { get; set; }
        public string UserID { get; set; }
        public int ProgramID { get; set; }
        public int ProposalYear { get; set; }
        public string ProposalDateTime { get; set; }
        public Int16 ProposalStatusHR { get; set; }
        public Int16 ProposalStatusInCharge { get; set; }
        public Int16 ProposalStatusCommittee { get; set; }
        public float ProposalAmount { get; set; }
        public float ProposalAllotedAmount { get; set; }
        public Int16 ProposalActionCode { get; set; }

    }
}