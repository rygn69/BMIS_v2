using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class LBPOne_SourceFundsModel
    {
        public int Fund_ID { get; set; }
        public string Fund_Desc { get; set; }
        public string Date_Of { get; set; }
        public int Action_Code { get; set; }
        public int Year_Of { get; set; }
        public int Fund_Desc_ID { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double Year1_Amount { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double Year2_Amount { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double Year3_Amount { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double Difference { get; set; }



        public Int64 supplement_source_ID { get; set; }
        public string supplement_souce { get; set; }



    }
}