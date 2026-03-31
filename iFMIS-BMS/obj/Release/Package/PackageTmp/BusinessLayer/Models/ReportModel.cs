using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class ReportModel
    {
        public string UserTypeDesc { get; set; }
        public string OfficeID { get; set; }
        public string OfficeName { get; set; }
        public int ObjectOfExpendetureID { get; set; }
        public string OfficeAbbrivation { get; set; }
        public int pmisofficeid { get; set; }
        public long accountid { get; set; }
        public string accountname { get; set; }
        public double appropriation { get; set; }
        public double proportion { get; set; }
        public int trnno { get; set; }
        public string articlename { get; set; }
        public string enduser { get; set; }
    }
}