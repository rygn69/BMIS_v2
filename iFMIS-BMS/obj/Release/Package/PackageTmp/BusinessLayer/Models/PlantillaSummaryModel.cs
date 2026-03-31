using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class PlantillaSummaryModel
    {
        public int OfficeID { get; set; }
        public string OfficeName { get; set; }
        public int Filled { get; set; }
        public int Vacant { get; set; }
        public int Casual { get; set; }

        public string FundType { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double VacantAmount { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double FilledAmount { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double CasualAmount { get; set; }
        public string PSComponent { get; set; }

        
        
    }
}