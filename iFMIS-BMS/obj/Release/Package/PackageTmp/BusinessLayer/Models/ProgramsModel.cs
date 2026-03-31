using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class ProgramsModel
    {

        public string ProgramID { get; set; }
        public int OfficeID { get; set; }
        public string ProgramDescription { get; set; }
        public string ProgramYear { get; set; }

    }
}