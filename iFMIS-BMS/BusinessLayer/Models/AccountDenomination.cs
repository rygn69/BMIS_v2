using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class AccountDenomination
    {
        public int? AccountDenominationID { get; set; }
        public string DenominationName { get; set; }
        public decimal DenominationAmount { get; set; }
        public decimal DenominationMonth { get; set; }
        public string DateTimeEntered { get; set; }
        public int UserID { get; set; }
        public int ProposalID { get; set; }
        public int ActionCode { get; set; }
        public int TransactionYear { get; set; }
        public int? ProgramID { get; set; }
        public int? AccountID {get; set;}
        public int? ProposalYear { get; set; }
        public string AccountDenominationAmountConverted { get; set; }
        public int isPPMP { get; set; }
        public decimal TotalDenominationAmount { get; set; }

        public int? OfficeID { get; set; }
        public int? EditQuantityPercentage { get; set;}
        public decimal EditDenominationAmount { get; set; }
        public decimal EditDenominationMonth { get; set; }
        public decimal EditTotalDenominationAmount { get; set; }
        public decimal QuantityPercentage { get; set; }
        public decimal QuantityPercentageHistory { get; set; }
        public double TotalDenominationAmountHistory { get; set; }
        public double Total { get; set; }
        public string Remarks { get; set; }
        public int Office { get; set; }
        public decimal OriginalAmount { get; set; }
        public string specificactivity { get; set; }
        //check in QuantityPercentage
    }
}