using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class saveProgram
    {
        public int program_id { get; set; }
        public string program_desc { get; set; }
        public int office_id { get; set; }
        public int fund_id { get; set; }
        public int OfficeProgramID { get; set; }
        public string isSuccess { get; set; }
        public int orderBy { get; set; }
        public string datepicker { get; set; }
        public int ooe_id { get; set; }
        public int text_office_id { get; set; }
        
    }
}