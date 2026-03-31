using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class PlantillaModel
    {
        public Int64 SeriesID { get; set; }
        public int? OfficeID { get; set; }
        public int DivID { get; set; }
        public string DivName { get; set; }
        public string ItemNo { get; set; }
        public string ItemNoNew { get; set; }
        public int PositionID { get; set; }
        public string Position { get; set; }
        public string sg { get; set; }
        public int step { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double Salary { get; set; }
        public string EmployeeName { get; set; }
        public int EmploymentStatusID { get; set; }
        public string EmploymentStatus { get; set; }
        public int StepNew { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double ProposedSalary { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double YearlySalary { get; set; }
        public int yearOf { get; set; }
        public string AppointmentDate { get; set; }
        public DateTime Date { get; set; }
        public int ForStepIncrement { get; set; }
        public string strSalary { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double HazardPay { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double Subsistence { get; set; }
        public int  NoOfMonthsWithStep { get; set; }
        public int NoOfMonthsWithOutStep { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double Increase { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double IncreaseWithStep { get; set; }

        public string StepIncrementEffectivityDate { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double SalaryWithStep { get; set; }

        public int GroupOrder { get; set; }
        public int PlantillaOrdering { get; set; }
        public int oldSG { get; set; }
        public string rate { get; set; }

    }
}