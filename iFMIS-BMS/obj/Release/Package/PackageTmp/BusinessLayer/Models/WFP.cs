using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class WFP
    {
        public long trnno { get; set; }
        public string objectexpenditure { get; set; }
        public double Appropriation { get; set; }
        public double Reserved { get; set; }
        public double Netprogram { get; set; }
        public double Firstmonth { get; set; }
        public double Secondmonth { get; set; }
        public double Thirdmonth { get; set; }
        public string MFO { get; set; }
        public string PTFirstmonth { get; set; }
        public string PTSecondmonth { get; set; }
        public string PTThirdmonth { get; set; }
        public int officeid { get; set; }
        public int programid { get; set; }
        public long accountid { get; set; }
        public string officemfo { get; set; }
        public double AppropriationNet { get; set; }
        public int qtr { get; set; }
        public double firstmonth_rel { get; set; }
        public double secondmonth_rel { get; set; }
        public double thirdmonth_rel { get; set; }
        public long transno { get; set; }
        public int MFOid { get; set; }
        public int fundid { get; set; }
    }
}