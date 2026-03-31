using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class LBPOne_SUB1Model
    {

        public int Sub1_ID { get; set; }
        public string Sub1_Desc { get; set; }
        public int Type_ID { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double Year1_Amount { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double Year2_Amount { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double Year3_Amount { get; set; }
        public string Date_Of { get; set; }
        public int Action_Code { get; set; }
        public int Year_Of { get; set; }
        public int Sub1_Desc_ID { get; set; }
    }
}