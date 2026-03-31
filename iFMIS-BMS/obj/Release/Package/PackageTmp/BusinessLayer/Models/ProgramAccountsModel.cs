using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class ProgramAccountsModel
    {
        public Int64 ProgramAccountID { get; set; }
        public int OfficeProgramID { get; set; }
        public int AccountID { get; set; }
        public int ObjectOfExpendetureID { get; set; }
        public Int16 ActionCode { get; set; }
        public string DateTimeEntered { get; set; }
        public string UserID { get; set; }
        public string AccountDescripttion { get; set; }
        public int OrderNo { get; set; }
        public int AccountYear { get; set; }
        public string ProgramDescription { get; set; }
        public int isProposed { get; set; }
        public string OOEDesciption { get; set; }
    }
}