using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class TransFundsModel
    {
        public int TransAmount_ID { get; set; }
        public int Trans_ID { get; set; }
        public string Trans_name { get; set; }
        public double Trans_Amount { get; set; }
        public int OfficeID { get; set; }
        public int Year { get; set; }
    }
}