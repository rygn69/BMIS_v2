using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Budget_Preparation
{
    public class LBPForm4Model
    {
        public long SeriesID { get; set; }
        public long AIPID { get; set; }
        public long MainAIPID { get; set; }
        public string AIPRefCode { get; set; }
        public string PPADesc { get; set; }
        public string MajorFinalOutput { get; set; }
        public string PerformanceIndicator { get; set; }
        public string TargetForTheBudgetYear { get; set; }
        public double PSAmount { get; set; }
        public string PSAmountStr { get; set; }
        public string MOOEAmount { get; set; }
        public string COAmount { get; set; }
        public string Total { get; set; }
        public int isBold { get; set; }
        public int isNonOffice { get; set; }
        public int OrderNo { get; set; }
        public double PSMax { get; set; }
        public double MOOEMax { get; set; }
        public double COMax { get; set; }
        public int transno { get; set; }
        public int aiptag { get; set; }
        public int initiativeid { get; set; }
        public string initiative { get; set; }
        public double appropriation { get; set; }
        public double breakdownamount { get; set; }
        public double balance { get; set; }
        public long aipid { get; set; }
        public int parentaipid { get; set; }
        public int mainaipid { get; set; }
        public long parent_trnno { get; set; }
        public string target { get; set; }
        public double ChildTotAmount { get; set; }

    }
    public class LBPForm4OtherDataModel {
        public int SeriesID { get; set; }
        public string Description { get; set; }
        public int OrderNo { get; set; }
        public int DataType { get; set; }
        public string DataTypeDesc { get; set; }
    }
}