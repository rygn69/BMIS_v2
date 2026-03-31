using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class LBPone_dllEcoEnter_Model
    {
        public int Eco_Desc_ID { get; set; }
        public string Eco_Desc { get; set; }
        public int EE_Sub1_Desc_ID { get; set; }
        public string EE_Sub1_Desc { get; set; }
        public int EE_Sub2_Desc_ID { get; set; }
        public string EE_Sub2_Desc { get; set; }
        public int Eco_Type_Desc_ID { get; set; }
        public string Eco_Type_Desc { get; set; }
        public int Class_ID { get; set; }
        public string Class_Desc { get; set; }


    }
}