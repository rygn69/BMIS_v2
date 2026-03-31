using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class AdditionalRulesModel
    {
        public Int16 canReviewPS { get; set; }
        public Int16 canReviewMOOE { get; set; }
        public Int16 canReviewCO { get; set; }
        public Int16 canReviewFE { get; set; }
    }
}