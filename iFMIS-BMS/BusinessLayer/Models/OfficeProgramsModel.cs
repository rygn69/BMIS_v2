using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class OfficeProgramsModel
    {
        public Int64 OfficeProgramID { get; set; }
        public int OfficeID { get; set; }
        public Int64 ProgramID { get; set; }
        public Int16 ActionCode { get; set; }
        public string DateTimeEntered { get; set; }
        public string UserID { get; set; }
        public string ProgramDescription { get; set; }
        public int OrderNo { get; set; }
        public int ProgramYear { get; set; }
    }
}