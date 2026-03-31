using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Email
{
    public class EmailModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}