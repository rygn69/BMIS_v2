using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class accChart_Model
    {
        public string name_type { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double mounts { get; set; }
        public decimal percentage { get; set; }

    }
}