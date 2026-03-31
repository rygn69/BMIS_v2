using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class Monthly_Edit_Realign_Model
    {

        public int FromOfficeCode { get; set; }
        public int FromProgramCode { get; set; }
        public int FromAccountCode { get; set; }
        public int ToOfficeCode { get; set; }
        public int ToProgramCode { get; set; }
        public int ToAccountCode { get; set; }
        public string Description { get; set; }

        public Int64 supplementalbudget_id { get; set; }
        public double Amount_Sup { get; set; }
        public Int64 SBCode { get; set; }
        public string Description_supp { get; set; }

        public double AmountCodes { get; set; }
        public string UserIDCodes { get; set; }
        public string DateTimeEnteredCodes { get; set; }
        public int yearOfCodes { get; set; }
        


    }
}