using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Budget_Preparation
{
    public class Cancelled
    {
        public int ProposalYear { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double ProposalAmount { get; set; }
        public string Remarks { get; set; }
        public string AccountName { get; set; }
        public string OOE { get; set; }
        public string OfficeLevel { get; set; }
        public string OfficeName { get; set; }
        public Int64 ProposalID { get; set; }
        public string EmpName { get; set; }
        public int ProgramID { get; set; }
        public int AccountID { get; set; }
        public int TransactionYear { get; set; }
        public int ProposalDenominationCode { get; set; }
        public int CanPS { get; set; }
        public int CanMOOE { get; set; }
        public int CanCO { get; set; }
        public int CanFE { get; set; }


    }
}