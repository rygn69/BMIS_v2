using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class LbpForm1Model
    {
        public int SeriesID { get; set; }
        public int OrderNo { get; set; }
        public string Particular { get; set; }
        public string AccountCode { get; set; }
        public string IncomeClassification { get; set; }
        public string PastYear { get; set; }
        public string CurrentYear { get; set; }
        public string CurrentYearActual { get; set; }
        public string BudgetYear { get; set; }
        public int isBold { get; set; }
        public int useAmount { get; set; }
        public int hasFooterTotal { get; set; }
        public int useInGraph { get; set; }
        public string RowNo { get; set; }
    }
}