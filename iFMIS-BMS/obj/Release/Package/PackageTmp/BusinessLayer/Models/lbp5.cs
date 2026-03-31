using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class lbp5
    {
        public int OBJ_ID { get; set; }
        public string OBJ_Description { get; set; }
        public string OBJ_OrderBy { get; set; }
        public int OfficeID { get; set; }
        public int PPA_MFO_ID { get; set; }
        public string PPA_MFO_Description { get; set; }
        public int PPA_ID { get; set; }
        public string PPA_CodeRef { get; set; }
        public string PPA_Description { get; set; }
        public double PPA_Cost { get; set; }
        public string PPA_Output_Indicator { get; set; }
        public string PPA_Target { get; set; }
        public string PPA_Implement_FROM { get; set; }
        public string PPA_Implement_TO { get; set; }
        public string AccountName { get; set; }
        public int AccountID { get; set; }
        public double AccountAmount { get; set; }
        public string AmountAccountString { get; set; }
        public int? Send_MFO_ID { get; set; }
        public string OrderOBJ_param { get; set; }
        public string DescriptionOBJ_param { get; set; }
        public int OBJID_param { get; set; }
        public int MFOID_param { get; set; }
        public string MFO_DESC { get; set; }
        public int FS_OrderBy { get; set; }
        public int FS_ID { get; set; }
        public string FS_DESC { get; set; }
        public int FS_OrderByparam { get; set; }
        public int FS_IDparam { get; set; }
        public string FS_DESCparam { get; set; }
        public int PPA_IDparam { get; set; }
        public string PPA_CodeRefparam { get; set; }
        public string PPA_Descriptionparam { get; set; }
        public double PPA_Costparam { get; set; }
        public string PPA_Output_Indicatorparam { get; set; }
        public string PPA_Targetparam { get; set; }
        public string PPA_Implement_FROMparam { get; set; }
        public string PPA_Implement_TOparam { get; set; }
        public int Sub_PPAparam { get; set; }
        public int Sub_PPA { get; set; }
        public double MFO_Cost { get; set; }
        public int PPA_OrderBy { get; set; }
        public int? PPA_MFOOrderBy { get; set; }
    }
}