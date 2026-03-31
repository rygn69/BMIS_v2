using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class LegalBasis_Model
    {
        public Int64 Legal_id { get; set; }
        public Int64 LegalCode { get; set; }
        public string LegalDescription { get; set; }
        public string UserID { get; set; }
        public int ActionCode { get; set; }
        public string DateTimeEntered { get; set; }
        public int YearOf { get; set; }

        public string LegalDescriptionOther { get; set; }
        public Int32 LegalbasisID { get; set; }



    }
}