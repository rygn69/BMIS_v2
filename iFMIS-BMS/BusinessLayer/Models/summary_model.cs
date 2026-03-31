using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class summary_model
    {

        public int summary_id { get; set; }
        [Display(Name = "www")]
        public string summary_group { get; set; }
        public string summary_desc { get; set; }
        public int summary_count { get; set; }

    }
}