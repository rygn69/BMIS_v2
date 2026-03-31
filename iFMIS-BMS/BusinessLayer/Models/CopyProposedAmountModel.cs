using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class CopyProposedAmountModel
    {
        public int ProposalID { get; set; }
        public int OfficeID { get; set; }
        public int OOEID { get; set; }
        public string OfficeName { get; set; }
        public string OOEName { get; set; }
        public string AccountName { get; set; }
        public double ProposedAmount { get; set; }
        public double ApprovedAmount { get; set; }

    }
}