using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class SummaryAllOffices
    {
        public int ProposalID { get; set; }
        public int OfficeID { get; set; }
        public int OOEID { get; set; }
        public string OfficeName { get; set; }
        public string OOEName { get; set; }
        public double TotalCost { get; set; }
        public string OOEFullName { get; set; }
        public double PastYear { get; set; }
        public double PercentageIncreaseDecrease { get; set; }
    }
}