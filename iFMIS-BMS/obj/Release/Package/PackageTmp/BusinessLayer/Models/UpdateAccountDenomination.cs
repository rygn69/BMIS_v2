using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class UpdateAccountDenomination
    {
        public int? AccountDenominationID { get; set; }
        public string DenominationName { get; set; }
        public decimal DenominationAmount { get; set; }
        public string DateTimeEntered { get; set; }
        public int UserID { get; set; }
        public int ProposalID { get; set; }
        public int ActionCode { get; set; }
        public int TransactionYear { get; set; }
        public int? ProgramID { get; set; }
        public int? AccountID {get; set;}
        public int? ProposalYear { get; set; }
        public decimal TotalAmount { get; set; }
        public double DenominationMonth { get; set; }
        public double QuantityPercentage { get; set; }
        public int isPPMP { get; set; }
        // checkin
    }
}