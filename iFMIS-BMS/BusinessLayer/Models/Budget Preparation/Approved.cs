using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Budget_Preparation
{
    public class Approved
    {
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double ProposalAllotedAmount { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double ProposalAmount { get; set; }
        public string OOE { get; set; }
        public int ProposalYear { get; set; }
        public string AccountName { get; set; }
        public int AccountID { get; set; }
        public int ProgramID { get; set; }
        public int ProposalDenominationCode { get; set; }
        public int ProposalID { get; set; }
        public int? BudgetYear { get; set; }
        public int? OfficeID { get; set; }
        public int? ProgramIDparams { get; set; }
        public int AccountCode { get; set; }
        public decimal Current_Year { get; set; }
        public decimal Budget_Year { get; set; }
        public double Actual_Year { get; set; }
        public double SlashAmount { get; set; }
        public double ApprovedYear { get; set; }
    }
}