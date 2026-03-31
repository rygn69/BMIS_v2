using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class copyProgram
    {
        public int ProgramID { get; set; }
        public int AccountID { get; set; }
        public int ObjectOfExpendetureID { get; set; }
        public int ActionCode { get; set; }
        public int OrderNo { get; set; }
        public string datepickerFROM { get; set; }
        public string datepickerTO { get; set; }
        public int text_office_id { get; set; }

        
    }
}