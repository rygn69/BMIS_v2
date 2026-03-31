using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.Models
{
	public class CurrentControl
	{
        public long batchno { get; set; }
        public string payee { get; set; }
        public string particular { get; set; }
        public decimal sumTotal { get; set; }
        public string address { get; set; }
        public string date_ { get; set; }
        public string rc { get; set; }
	}
   
}