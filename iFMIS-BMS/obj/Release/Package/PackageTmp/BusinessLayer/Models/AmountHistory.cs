using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class AmountHistory
    {
        public decimal Amount { get; set; }
        public string Date { get; set; }
        public string UserName { get; set; }
        public int? ProposalID { get; set; }
        public string AccountName { get; set; }
        public int AmountHistoryID { get; set; }
        public float ProposalAmount { get; set; }
    }
}