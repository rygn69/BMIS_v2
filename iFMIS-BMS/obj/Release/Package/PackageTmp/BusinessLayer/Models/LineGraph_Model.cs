using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class LineGraph_Model
    {
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double amount1 { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double amount2 { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double amount3 { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double amount4 { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double amount5 { get; set; }
            
    }
}