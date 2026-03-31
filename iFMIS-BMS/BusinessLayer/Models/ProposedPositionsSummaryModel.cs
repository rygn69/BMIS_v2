using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class ProposedPositionsSummaryModel
    {
        public Int64 SeriesID { get; set; }
        public string OfficeName { get; set; }
        public string Position { get; set; }
        public string FundType { get; set; }
        public string StartDate { get; set; }
        public int SalaryGrade { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double YearlySalary { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double YearEndBonus { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double MidYearBonus { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double Philhealth { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double ecc { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double GSIS { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double PERA { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double PagIbig { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double Clothing { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double CashGift { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double  Subsistence { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double Hazard { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double PBB { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double  PEI { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double RepresentationAllowance { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double TransportationAllowance { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double Total { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double TotalProposed { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double TotalApproved { get; set; }
        public string Status { get; set; }
    }
}