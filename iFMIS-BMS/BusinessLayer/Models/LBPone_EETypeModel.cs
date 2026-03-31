using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class LBPone_EETypeModel
    {
        public int Eco_Type_ID { get; set; }
        public int Eco_Type_Desc_ID { get; set; }
        public int Eco_ID { get; set; }
        public string Date_Of { get; set; }
        public int Action_Code { get; set; }
        public int Year_Of { get; set; }
        public string Eco_Type_Desc { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double Year1_AmountEE { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double Year2_AmountEE { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double Year3_AmountEE { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double DifferenceEE { get; set; }

    }
}