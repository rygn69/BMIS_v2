using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class ForFundingModel
    {
        public string Remark { get; set; }
        public string SG { get; set; }
        public int ProposedItemID { get; set; }
        public string Position { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double Salary { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double yearlySalary { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double pera { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double clothing { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double cashgift { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double liferetirement { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double pagibig { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double philhealth { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double eccContributions { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double yearendbonus { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double PersonalityBasedBonus { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double HazardPay { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double Subsistence { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double Total { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double PEI { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double RepresentationAllowance { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double TransportationAllowance { get; set; }
        public string GroupBY { get; set; }
        public string GroupTag { get; set; }
        public string SalaryEffectivityDate { get; set; }
        
    }
}