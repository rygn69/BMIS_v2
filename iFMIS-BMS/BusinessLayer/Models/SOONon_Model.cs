using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class SOONon_Model
    {
        public Int64 SOOid { get; set; }
        public Int64 trnno { get; set; }
        public int FMISProgramCode { get; set; }
        public int OOECode { get; set; }
        public int FMISAccountCode { get; set; }
        public string BudgetAcctName { get; set; }
        public int Yearof { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double AllotedAmount { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double SooAmount { get; set; }


    }
}