using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.BudgetControl
{
    public class PPAModel
    {
        public int ConnectionStatus { get; set; }
        public double AcctRelease { get; set; }
        public double AcctObligation { get; set; }
        public double AcctDifference { get; set; }
        public double RootPPARelease { get; set; }
        public double RootPPAObligation { get; set; }
        public double RootPPADifference { get; set; }
        public double PPADifference { get; set; }
        public string MTitle { get; set; }
        public string MBody { get; set; }
        public string MType { get; set; }
        public string obrno { get; set; }
    }
}