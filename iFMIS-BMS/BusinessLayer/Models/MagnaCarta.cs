using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class MagnaCarta
    {
        public int OfficeID { get; set; }
        public int Account { get; set; }
        public int ProgramID { get; set; }
        public string type { get; set; }
        public int BudgetYear { get; set; }
        public string AccountName { get; set; }
    }
}