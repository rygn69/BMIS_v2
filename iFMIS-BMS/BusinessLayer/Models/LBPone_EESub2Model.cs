using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class LBPone_EESub2Model
    {
        public int EE_Sub2_ID { get; set; }
        public int EE_Sub2_Desc_ID { get; set; }
        public int EE_Sub1_ID { get; set; }
        public string Date_Of { get; set; }
        public int Action_Code { get; set; }
        public int Year_Of { get; set; }
        public string EE_Sub1_Desc { get; set; }
        public string EE_Sub2_Desc { get; set; }
        public string Account_Code { get; set; }
        public int Class_ID { get; set; }
        

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