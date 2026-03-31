using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Budget_Preparation
{
    public class AIPItemsModel
    {
        public long AIPID { get; set; }
        public string MotherAIPID { get; set; }
        public string EmplementingOffice { get; set; }
        public string FundingSource { get; set; }
        public string AIPRefCode { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string CompletionDate { get; set; }
        public string ExpectedOutput { get; set; }
        public string PSAmount { get; set; }
        public string MOOEAmount { get; set; }
        public string COAmount { get; set; }
        public string TotalAmount { get; set; }
        public string CCType { get; set; }
        public string CCAmount { get; set; }
        public string CCTypologyCode { get; set; }
        public int OrderNo { get; set; }
        public int isNonOffice { get; set; }
    }
}