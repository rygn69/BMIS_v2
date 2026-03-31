namespace iFMIS_BMS.Reports
{
    using iFMIS_BMS.BusinessLayer.Connector;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Linq;
    using System.Configuration;
    using System.Data.SqlTypes;
    using iFMIS_BMS.Base;
    using iFMIS_BMS.Classes;

    /// <summary>
    /// Summary description for LBEF.
    /// </summary>
    public partial class Supplemental : Telerik.Reporting.Report
    {
        public Supplemental(int? OfficeID, int? classtype, int? year, int? month_, int? batch, int? sort_, string note_, int? expclass, string purposeid, int? Fundtype, int? packet, int? reporthistory, string ComputerIP, int? is_float, int? budgettype, string dateissue)
        {

            InitializeComponent();


        }
    }
}