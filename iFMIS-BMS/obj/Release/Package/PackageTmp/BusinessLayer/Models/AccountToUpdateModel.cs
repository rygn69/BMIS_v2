using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class AccountToUpdateModel
    {
        public Int64 PrimaryKey { get; set; }
        public string AccountName { get; set; }
        public string AccountID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectTittle { get; set; }
        public int OOEID { get; set; }
        public int ProgramID { get; set; }
        public int isNewProposed { get; set; }
        public int OfficeID { get; set; }
    }
}