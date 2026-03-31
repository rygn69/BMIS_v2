using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class Monthly_DD_Model
    {
        public int account_id { get; set; }
        public string account_name { get; set; }
        public int month_id { get; set; }
        
        public Int64 supplementalbudget_id { get; set; }
        public string LegalDescription { get; set; }
        public double Amountfrom { get; set; }
        public double Amount { get; set; }
        public double AmountApprove { get; set; }
        public Int64 supplementaltransfere_id { get; set; }
        public Int64 supplementalreverse_id { get; set; }
        public int FMISOfficeCode { get; set; }
        public int FMISProgramCode { get; set; }
        public int FMISAccountCode { get; set; }
        public string OfficeAbbrivation { get; set; }
        public string MonthOf { get; set; }
        public string AccountName { get; set; }
        public int YearOf { get; set; }
        public int ooeid { get; set; }
        public int type_ { get; set; }
        public string dtetime { get; set; }
        public Int64 releasereversion_id { get; set; }
        public int reltag { get; set; }
        public int modev2 { get; set; }
        public int legalbasisid { get; set; }
        public int isfloat { get; set; }
        public int realign_tag { get; set; }
        public int subaccount { get; set; }
        public int mon { get; set; }
        public long userid { get; set; }
        public string subaccountname { get; set; }
        public string purpose { get; set; }
        public int status { get; set; }
        public long realignmentid { get; set; }
        public string accountfrom { get; set; }
        public string ppafrom { get; set; }
        public string accountto { get; set; }
        public string ppato { get; set; }
        public int fromprogram { get; set; }
        public int fromaccount { get; set; }
        public int toprogram { get; set; }
        public int toaccount { get; set; }
        public int tooffice { get; set; }
        public int fromoffice { get; set; }
        public string datetimesubmit { get; set; }
        public string datetimeapprove { get; set; }
        public string datetimeposted { get; set; }
        public double Amount_orig { get; set; }
        public Int64 reversion_id { get; set; }
        public Int32 saipno { get; set; }
        public string purposereturn { get; set; }
        public string mode { get; set; }
        public string officename { get; set; }
        public int posted { get; set; }
        public int trnno { get; set; }
        public string officefullname { get; set; }
        public string mafno { get; set; }
        public long docid { get; set; }

    }
}