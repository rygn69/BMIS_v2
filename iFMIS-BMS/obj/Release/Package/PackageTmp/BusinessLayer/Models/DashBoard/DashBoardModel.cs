using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace iFMIS_BMS.BusinessLayer.Models.DashBoard
{
    public class DashBoardModel
    {
        public string BudgetYear { get; set; }
        public double ProjectedAmount { get; set; }
        public double ProposedAmount { get; set; }
        public string ProjectedVsProposedCategory { get; set; }
        public int ProjectedVsProposedYear { get; set; }
        public string UserColor { get; set; }
        public int OOEIDPie { get; set; }
        public double TotalPSMooeCA { get; set; }
        public double TotalPS { get; set; }
        public double TotalMOOE { get; set; }
        public double TotalCA { get; set; }
        public string LegendPiePSMooeCA { get; set; }
        public string OfficeLegend { get; set; }
        public double TotalAmountOffice { get; set; }
        public int OfficeID { get; set; }
        public string SourceOfFundsID { get; set; }
        public double SourceOfFundsAmount { get; set; }
        public string LegendSourceOfFunds { get; set; }
        public double OriginalProposedAmount { get; set; }
        public double DiffirenceProposed { get; set; }
        public double DifferenceApproved { get; set; }
       
       // public double TotalPSMooeCAPercent { get; set; }

        public double col1 { get; set; }
        public double col2 { get; set; }
        public double col3 { get; set; }
        public double col4 { get; set; }
        public double col5 { get; set; }
        public double col6 { get; set; }
        public double col7 { get; set; }
        public double col8 { get; set; }
        public double col9 { get; set; }
        public double col10 { get; set; }
        public double col11 { get; set; }
        public double col12 { get; set; }
        
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double amount1 { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double amount2 { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        public double amount3 { get; set; }

        public int trnno { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int Sg_new { get; set; }
        public int id { get; set; }
        public string AccountName { get; set; }
        public double Amount { get; set; }
        public double TotalAmount { get; set; }
        public decimal TotalFilledVacant { get; set; }
        public int qty { get; set; }
        public double appropriation { get; set; }
        public double allotment { get; set; }
        public double obligation { get; set; }
        public double Allotment_balance { get; set; }
        public double Appropriation_balance { get; set; }
        public string Programname { get; set; }
        public decimal appropriationps { get; set; }
        public decimal appropriationmooe { get; set; }
        public decimal appropriationco { get; set; }
        public decimal obligationps { get; set; }
        public decimal obligationmooe { get; set; }
        public decimal obligationco { get; set; }
        public decimal allotmentu { get; set; }
        public decimal appropriationlabel { get; set; }
        public decimal allotmentlabel { get; set; }
        public decimal preobligationlabel { get; set; }
        public decimal obligationlabel { get; set; }
        public decimal disbursementlabel { get; set; }
        public decimal consumatedlabel { get; set; }
        public int fundid { get; set; }
        public string fundname { get; set; }
        public double finamount { get; set; }
        public string dateandtime {get;set;}
        public double allpct { get; set; }
        public double pre_oblpct { get; set; }
        public double oblpct { get; set; }
        public double dispct { get; set; }
        public double utipct { get; set; }
        public string office { get; set; }
        public double preobligation { get; set; }
        public double disbursement { get; set; }
        public double accounted { get; set; }
        public string program { get; set; }
        public string account { get; set; }
        public int yearof { get; set; }
        public double utilize { get; set; }
        public double unutilize { get; set; }
        public int accountid { get; set; }
        public int num { get; set; }
        public int prcent { get; set; }
        public int catid { get; set; }
        public int perclass { get; set; }
        public int peroffice { get; set; }
        public double amount_pr { get; set; }
        public double amount_ob { get; set; }
        public double utilize_pr { get; set; }
        public double utilize_ob { get; set; }
        public double appropriation_pr { get; set; }
        public double appropriation_ob { get; set; }
    }
    public class OfficeListModel
    {
        public string OfficeName { get; set; }
        public string NoofAccountsSubmitted { get; set; }
        public string DateTimeSubmitted { get; set; }
        public int isComplete { get; set; }
    }
}