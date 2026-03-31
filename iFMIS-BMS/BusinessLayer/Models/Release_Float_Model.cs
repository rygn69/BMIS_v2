using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class Release_Float_Model
    {
        public Int64 release_float_id { get; set; }
        public Int64 release_id { get; set; }
        public string amount_description { get; set; }
        public decimal Amount_release { get; set; }
        public string MonthOf { get; set; }
        public int YearOf { get; set; }
        public decimal balance_amount { get; set; }
        public int batch { get; set; }
        public int Float_Flag { get; set; }
        public string DateTimeEntered { get; set; }
        public string DateReleased { get; set; }
        public int FMISOfficeCode { get; set; }
        public int FMISProgramCode { get; set; }
        public int FMISAccountCode { get; set; }
        public int MonthOf_ { get; set; }
        public double AmountPS { get; set; }
        public double AmountMOOE { get; set; }
        public double AmountCO { get; set; }

        public Int64 subsidyrelease_float_id { get; set; }
        public int FMISOfficeCode_sub { get; set; }
        public double Amount_sub { get; set; }
        public int MonthOf_sub { get; set; }
        public int YearOf_sub { get; set; }
        public int batch_sub { get; set; }


        public Int64 incomerelease_float_id { get; set; }
        public int FMISOfficeCode_inc { get; set; }
        public double Amount_inc { get; set; }
        public int MonthOf_inc { get; set; }
        public int YearOf_inc { get; set; }
        public int batch_inc { get; set; }
        public int gov_com { get; set; }
        public int comtag { get; set; }
        public string code { get; set; }
        public int officeid { get; set; }
        public int programid { get; set; }
        public long accountid { get; set; }
        public string particular { get; set; }
        public long muncode { get; set; }
        public long brgycode { get; set; }
        public string officename { get; set; }
        public int ooeid { get; set; }
        public string enduser { get; set; }
    }
}