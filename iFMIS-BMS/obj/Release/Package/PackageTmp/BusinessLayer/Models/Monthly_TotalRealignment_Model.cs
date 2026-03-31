using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class Monthly_TotalRealignment_Model
    {
        public Int64 totalrealign_id { get; set; }
        public int UserID { get; set; }
        public double Amount { get; set; }
        public int ActionCode { get; set; }
        public int YearOf { get; set; }
        public string DateTimeEntered { get; set; }
        public int ToOfficeCode { get; set; }
        public int ToProgramCode { get; set; }
        public int ToAccountCode { get; set; }
        public double ToAmount { get; set; }
        public string Description { get; set; }
        public string officename { get; set; }
    }
}