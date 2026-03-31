using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class AccountsModel
    {
        public Int64 ProposalID { get; set; }
        public int AccountID { get; set; }
        public int AccountCode { get; set; }
        public string AccountName { get; set; }
        public string FundName { get; set; }

        public string OOEName { get; set; }

        public int ProposalYear { get; set; }
        public int PastProposalYear { get; set; }
        

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0,1000000000000)]
        public double ProposalAllotedAmount { get; set; }
        public string UserID { get; set; }
        public int ProgramID { get; set; }
        public string ProposalStatusCommittee { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double PastProposalAmmount { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double CurrentProposalAmount { get; set; }

         [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double ProposalAmmount { get; set; }
        public int ProposalStatusHR { get; set; }
        public int ProposalStatusInCharge { get; set; }
        public string ColorCircle { get; set; }
        public int Count { get; set; }
        public int ProposalDenominationCode { get; set; }
        public int ProposalStatusCommitteeINT { get; set; }
        public int OOEID { get; set; }
        public decimal setProposalAllotedAmount { get; set; }
        public int checker { get; set; }
        public double PastYear { get; set; }
        public double SlashAmount { get; set; }
        public int CheckComp { get; set; }
        public int? UpdateProposalAccount { get; set; }
        public int? UpdateProgramID { get; set; }
        public int? UpdateAccountID { get; set; }
        public int? UpdateOOEID { get; set; }
        public string UpdateOfficeName { get; set; }
        public int? UpdateOfficeID { get; set; }
        public int? UpdateTransactionYear { get; set; }
        public int setProgramID { get; set; }
        public string setProgramDesc { get; set; }
        public int DenominationCode { get; set; }
        public string ProgramDescription { get; set; }
        public int isProposed { get; set; }
        public int SelectedOfficeID { get; set; }
        public string setRemarks { get; set; }
        public double ApprovedYear { get; set; }
        public int AmountStatus { get; set; }
        public double Difference { get; set; }
        public int WithCheckBox { get; set; }
        public long NewAccountID { get; set; }
        public long setAccountID { get; set; }
        public string setAccountname { get; set; }
        public int NewProgramID { get; set; }
        public int OldOffice { get; set; }
        public int NewOffice { get; set; }
        public double supplemental_amount { get; set; }
        public int aipversion { get; set; }
        public double appropriation { get; set; }
    }
}