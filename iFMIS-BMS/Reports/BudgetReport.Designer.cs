namespace iFMIS_BMS.Reports
{
    partial class BudgetReport
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Drawing.FormattingRule formattingRule1 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Barcodes.QRCodeEncoder qrCodeEncoder1 = new Telerik.Reporting.Barcodes.QRCodeEncoder();
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group2 = new Telerik.Reporting.Group();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.groupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.textBox27 = new Telerik.Reporting.TextBox();
            this.textBox28 = new Telerik.Reporting.TextBox();
            this.textBox29 = new Telerik.Reporting.TextBox();
            this.textBox30 = new Telerik.Reporting.TextBox();
            this.groupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.chAccounts = new Telerik.Reporting.TextBox();
            this.chAccountCode = new Telerik.Reporting.TextBox();
            this.chPastYear = new Telerik.Reporting.TextBox();
            this.chCurrentYear = new Telerik.Reporting.TextBox();
            this.chBudgetYear = new Telerik.Reporting.TextBox();
            this.groupFooterSection1 = new Telerik.Reporting.GroupFooterSection();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.groupHeaderSection1 = new Telerik.Reporting.GroupHeaderSection();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.textBox17 = new Telerik.Reporting.TextBox();
            this.textBox18 = new Telerik.Reporting.TextBox();
            this.textBox20 = new Telerik.Reporting.TextBox();
            this.textBox21 = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.textBox23 = new Telerik.Reporting.TextBox();
            this.textBox24 = new Telerik.Reporting.TextBox();
            this.textBox25 = new Telerik.Reporting.TextBox();
            this.textBox26 = new Telerik.Reporting.TextBox();
            this.textBox22 = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.QRCode = new Telerik.Reporting.Barcode();
            this.txtPageAndYear = new Telerik.Reporting.HtmlTextBox();
            this.txtpage = new Telerik.Reporting.TextBox();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.txtYear = new Telerik.Reporting.HtmlTextBox();
            this.txtProgram = new Telerik.Reporting.HtmlTextBox();
            this.txtOfficeDepartment = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox1 = new Telerik.Reporting.HtmlTextBox();
            this.txtTitle = new Telerik.Reporting.HtmlTextBox();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.htmlTextBox3 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox4 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox5 = new Telerik.Reporting.HtmlTextBox();
            this.txtOfficehead = new Telerik.Reporting.TextBox();
            this.txtDesignation = new Telerik.Reporting.TextBox();
            this.txtBudgetHead = new Telerik.Reporting.TextBox();
            this.txtBudgetHeadDesignation = new Telerik.Reporting.TextBox();
            this.txtProvincialGovernorDesignatiom = new Telerik.Reporting.TextBox();
            this.txtProvincialGovernor = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // groupFooterSection
            // 
            this.groupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.2853902280330658D);
            this.groupFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox19,
            this.textBox27,
            this.textBox28,
            this.textBox29,
            this.textBox30});
            this.groupFooterSection.Name = "groupFooterSection";
            // 
            // textBox19
            // 
            this.textBox19.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(9.2435665130615234D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376823425292969D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox19.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox19.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox19.Style.Font.Bold = true;
            this.textBox19.Style.Font.Name = "Calibri";
            this.textBox19.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox19.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox19.Value = "=MyFormat(Sum(CDbl(Fields.BudgetYearApproved)))";
            // 
            // textBox27
            // 
            this.textBox27.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox27.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.5059661865234375D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.textBox27.Name = "textBox27";
            this.textBox27.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376823425292969D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox27.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox27.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox27.Style.Font.Bold = true;
            this.textBox27.Style.Font.Name = "Calibri";
            this.textBox27.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox27.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox27.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox27.Value = "=MyFormat(Sum(CDbl(Fields.CurrentYearApproved)))";
            // 
            // textBox28
            // 
            this.textBox28.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox28.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.7683615684509277D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox28.Name = "textBox28";
            this.textBox28.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7375246286392212D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox28.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox28.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox28.Style.Font.Bold = true;
            this.textBox28.Style.Font.Name = "Calibri";
            this.textBox28.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox28.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox28.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox28.Value = "=MyFormat(Sum(CDbl(Fields.PastYearApproved)))";
            // 
            // textBox29
            // 
            this.textBox29.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox29.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.0306000709533691D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.textBox29.Name = "textBox29";
            this.textBox29.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376823425292969D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox29.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox29.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox29.Style.Font.Bold = true;
            this.textBox29.Style.Font.Name = "Calibri";
            this.textBox29.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox29.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox29.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox29.Value = "";
            // 
            // textBox30
            // 
            this.textBox30.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox30.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9736431062920019E-05D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.textBox30.Name = "textBox30";
            this.textBox30.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.0304813385009766D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox30.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox30.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox30.Style.Font.Bold = true;
            this.textBox30.Style.Font.Name = "Calibri";
            this.textBox30.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox30.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox30.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox30.Value = "Total";
            // 
            // groupHeaderSection
            // 
            this.groupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.41460990905761719D);
            this.groupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.chAccounts,
            this.chAccountCode,
            this.chPastYear,
            this.chCurrentYear,
            this.chBudgetYear});
            this.groupHeaderSection.Name = "groupHeaderSection";
            this.groupHeaderSection.PrintOnEveryPage = true;
            // 
            // chAccounts
            // 
            this.chAccounts.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9696693420410156E-05D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.chAccounts.Name = "chAccounts";
            this.chAccounts.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.0304813385009766D), Telerik.Reporting.Drawing.Unit.Inch(0.41457033157348633D));
            this.chAccounts.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.chAccounts.Style.Font.Bold = true;
            this.chAccounts.Style.Font.Name = "Calibri";
            this.chAccounts.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.chAccounts.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.chAccounts.Value = "Accounts\r\n(1)";
            // 
            // chAccountCode
            // 
            this.chAccountCode.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.0306000709533691D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.chAccountCode.Name = "chAccountCode";
            this.chAccountCode.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376827001571655D), Telerik.Reporting.Drawing.Unit.Inch(0.41457033157348633D));
            this.chAccountCode.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.chAccountCode.Style.Font.Bold = true;
            this.chAccountCode.Style.Font.Name = "Calibri";
            this.chAccountCode.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.chAccountCode.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.chAccountCode.Value = "Account Code\r\n(2)";
            // 
            // chPastYear
            // 
            this.chPastYear.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.7682037353515625D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.chPastYear.Name = "chPastYear";
            this.chPastYear.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376827001571655D), Telerik.Reporting.Drawing.Unit.Inch(0.41457033157348633D));
            this.chPastYear.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.chPastYear.Style.Font.Bold = true;
            this.chPastYear.Style.Font.Name = "Calibri";
            this.chPastYear.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.chPastYear.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.chPastYear.Value = "Past Year\r\n(3)";
            // 
            // chCurrentYear
            // 
            this.chCurrentYear.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.5059661865234375D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.chCurrentYear.Name = "chCurrentYear";
            this.chCurrentYear.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376827001571655D), Telerik.Reporting.Drawing.Unit.Inch(0.41457033157348633D));
            this.chCurrentYear.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.chCurrentYear.Style.Font.Bold = true;
            this.chCurrentYear.Style.Font.Name = "Calibri";
            this.chCurrentYear.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.chCurrentYear.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.chCurrentYear.StyleName = "";
            this.chCurrentYear.Value = "Current Year\r\n(4)";
            // 
            // chBudgetYear
            // 
            this.chBudgetYear.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(9.2435684204101562D), Telerik.Reporting.Drawing.Unit.Inch(3.9577484130859375E-05D));
            this.chBudgetYear.Name = "chBudgetYear";
            this.chBudgetYear.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376827001571655D), Telerik.Reporting.Drawing.Unit.Inch(0.41457033157348633D));
            this.chBudgetYear.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.chBudgetYear.Style.Font.Bold = true;
            this.chBudgetYear.Style.Font.Name = "Calibri";
            this.chBudgetYear.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.chBudgetYear.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.chBudgetYear.StyleName = "";
            this.chBudgetYear.Value = "Budget Year\r\n(5)";
            // 
            // groupFooterSection1
            // 
            this.groupFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.2853902280330658D);
            this.groupFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2,
            this.textBox4,
            this.textBox6,
            this.textBox8,
            this.textBox10});
            this.groupFooterSection1.Name = "groupFooterSection1";
            // 
            // textBox2
            // 
            this.textBox2.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.0304813385009766D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Font.Name = "Calibri";
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox2.Value = "Total {Fields.OOEName}";
            // 
            // textBox4
            // 
            this.textBox4.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.0306000709533691D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376823425292969D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox4.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox4.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox4.Style.Font.Bold = true;
            this.textBox4.Style.Font.Name = "Calibri";
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox4.Value = "";
            // 
            // textBox6
            // 
            this.textBox6.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.7682037353515625D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376823425292969D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox6.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox6.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox6.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.Style.Font.Name = "Calibri";
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox6.Value = "=MyFormat(Sum(CDbl(Fields.PastYearApproved)))";
            // 
            // textBox8
            // 
            this.textBox8.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.5059661865234375D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376823425292969D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox8.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox8.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox8.Style.Font.Bold = true;
            this.textBox8.Style.Font.Name = "Calibri";
            this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox8.Value = "=MyFormat(Sum(CDbl(Fields.CurrentYearApproved)))";
            // 
            // textBox10
            // 
            this.textBox10.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(9.2435665130615234D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376823425292969D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox10.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox10.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox10.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox10.Style.Font.Bold = true;
            this.textBox10.Style.Font.Name = "Calibri";
            this.textBox10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox10.Value = "=MyFormat(Sum(CDbl(Fields.BudgetYearApproved)))";
            // 
            // groupHeaderSection1
            // 
            this.groupHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.2853902280330658D);
            this.groupHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox16,
            this.textBox17,
            this.textBox18,
            this.textBox20,
            this.textBox21});
            this.groupHeaderSection1.Name = "groupHeaderSection1";
            // 
            // textBox16
            // 
            this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9736431062920019E-05D), Telerik.Reporting.Drawing.Unit.Inch(7.8678131103515625E-05D));
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.0304813385009766D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox16.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox16.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox16.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox16.Style.Font.Bold = true;
            this.textBox16.Style.Font.Name = "Calibri";
            this.textBox16.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox16.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox16.Value = "=Fields.OOEName";
            // 
            // textBox17
            // 
            this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.0305995941162109D), Telerik.Reporting.Drawing.Unit.Inch(7.8678131103515625E-05D));
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376829385757446D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox17.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox17.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox17.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox17.Style.Font.Bold = true;
            this.textBox17.Style.Font.Name = "Calibri";
            this.textBox17.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox17.Value = "";
            // 
            // textBox18
            // 
            this.textBox18.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.7682037353515625D), Telerik.Reporting.Drawing.Unit.Inch(7.8678131103515625E-05D));
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376823425292969D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox18.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox18.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox18.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox18.Style.Font.Bold = true;
            this.textBox18.Style.Font.Name = "Calibri";
            this.textBox18.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox18.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox18.Value = "";
            // 
            // textBox20
            // 
            this.textBox20.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.5059661865234375D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376823425292969D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox20.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox20.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox20.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox20.Style.Font.Bold = true;
            this.textBox20.Style.Font.Name = "Calibri";
            this.textBox20.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox20.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox20.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox20.Value = "";
            // 
            // textBox21
            // 
            this.textBox21.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(9.2435693740844727D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376804351806641D), Telerik.Reporting.Drawing.Unit.Inch(0.28531137108802795D));
            this.textBox21.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox21.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox21.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox21.Style.Font.Bold = true;
            this.textBox21.Style.Font.Name = "Calibri";
            this.textBox21.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox21.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox21.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox21.Value = "";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(0.26257926225662231D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox23,
            this.textBox24,
            this.textBox25,
            this.textBox26,
            this.textBox22,
            this.textBox1});
            this.detail.Name = "detail";
            this.detail.PageBreak = Telerik.Reporting.PageBreak.None;
            this.detail.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.detail.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.detail.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            // 
            // textBox23
            // 
            this.textBox23.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            formattingRule1.Filters.Add(new Telerik.Reporting.Filter("=Fields.Indicator", Telerik.Reporting.FilterOperator.Equal, "1"));
            formattingRule1.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.textBox23.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1});
            this.textBox23.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.0306000709533691D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.textBox23.Name = "textBox23";
            this.textBox23.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376824617385864D), Telerik.Reporting.Drawing.Unit.Inch(0.26249977946281433D));
            this.textBox23.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox23.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox23.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox23.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox23.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox23.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox23.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox23.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox23.TextWrap = true;
            this.textBox23.Value = "=Fields.AccountCode";
            // 
            // textBox24
            // 
            this.textBox24.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox24.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1});
            this.textBox24.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.7682037353515625D), Telerik.Reporting.Drawing.Unit.Inch(7.8996024967636913E-05D));
            this.textBox24.Name = "textBox24";
            this.textBox24.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376824617385864D), Telerik.Reporting.Drawing.Unit.Inch(0.26249977946281433D));
            this.textBox24.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox24.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox24.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox24.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox24.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox24.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox24.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox24.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox24.TextWrap = true;
            this.textBox24.Value = "=MyFormat(CDbl(Fields.PastYearApproved))";
            // 
            // textBox25
            // 
            this.textBox25.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox25.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1});
            this.textBox25.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.5059661865234375D), Telerik.Reporting.Drawing.Unit.Inch(7.915496826171875E-05D));
            this.textBox25.Name = "textBox25";
            this.textBox25.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376824617385864D), Telerik.Reporting.Drawing.Unit.Inch(0.26249977946281433D));
            this.textBox25.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox25.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox25.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox25.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox25.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox25.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox25.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox25.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox25.TextWrap = true;
            this.textBox25.Value = "=MyFormat(CDbl(Fields.CurrentYearApproved))";
            // 
            // textBox26
            // 
            this.textBox26.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox26.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1});
            this.textBox26.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(9.2435684204101562D), Telerik.Reporting.Drawing.Unit.Inch(7.9313911555800587E-05D));
            this.textBox26.Name = "textBox26";
            this.textBox26.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7376805543899536D), Telerik.Reporting.Drawing.Unit.Inch(0.26249977946281433D));
            this.textBox26.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox26.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox26.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox26.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox26.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox26.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox26.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox26.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox26.TextWrap = true;
            this.textBox26.Value = "=MyFormat(CDbl(Fields.BudgetYearApproved))";
            // 
            // textBox22
            // 
            this.textBox22.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Bottom)));
            this.textBox22.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1});
            this.textBox22.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.3305206298828125D), Telerik.Reporting.Drawing.Unit.Inch(7.9472862125840038E-05D));
            this.textBox22.Name = "textBox22";
            this.textBox22.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.7000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.26249977946281433D));
            this.textBox22.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox22.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox22.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox22.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox22.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox22.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox22.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox22.TextWrap = true;
            this.textBox22.Value = "=Fields.AccountName";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.040437221527099609D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2999998331069946D), Telerik.Reporting.Drawing.Unit.Inch(0.19385473430156708D));
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox1.Style.Visible = false;
            this.textBox1.Value = "=Fields.Indicator";
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.0844950675964356D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.QRCode,
            this.txtPageAndYear,
            this.txtpage});
            this.pageFooterSection1.Name = "pageFooterSection1";
            this.pageFooterSection1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // QRCode
            // 
            this.QRCode.Encoder = qrCodeEncoder1;
            this.QRCode.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(9.89984130859375D), Telerik.Reporting.Drawing.Unit.Inch(0.18449497222900391D));
            this.QRCode.Name = "QRCode";
            this.QRCode.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1000804901123047D), Telerik.Reporting.Drawing.Unit.Inch(0.75999975204467773D));
            this.QRCode.Stretch = true;
            this.QRCode.Value = "http://192.168.2.104/eportal";
            // 
            // txtPageAndYear
            // 
            this.txtPageAndYear.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.4444945752620697D));
            this.txtPageAndYear.Name = "txtPageAndYear";
            this.txtPageAndYear.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.999920845031738D), Telerik.Reporting.Drawing.Unit.Inch(0.19996063411235809D));
            this.txtPageAndYear.Style.Font.Bold = false;
            this.txtPageAndYear.Style.Font.Name = "Calibri";
            this.txtPageAndYear.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.txtPageAndYear.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtPageAndYear.Value = "Annual Budget Calendar Year 2016 . . . . LBP Form No. 3 . . . Page";
            // 
            // txtpage
            // 
            this.txtpage.CanGrow = false;
            this.txtpage.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.5D), Telerik.Reporting.Drawing.Unit.Inch(0.45558610558509827D));
            this.txtpage.Name = "txtpage";
            this.txtpage.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1999998092651367D), Telerik.Reporting.Drawing.Unit.Inch(0.17777785658836365D));
            this.txtpage.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.txtpage.Style.Visible = true;
            this.txtpage.Value = "= PageNumber+ \' of \' + PageCount";
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.1000000238418579D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txtYear,
            this.txtProgram,
            this.txtOfficeDepartment,
            this.htmlTextBox1,
            this.txtTitle});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // txtYear
            // 
            this.txtYear.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.4479166567325592D));
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.999920845031738D), Telerik.Reporting.Drawing.Unit.Inch(0.19996063411235809D));
            this.txtYear.Style.Font.Bold = false;
            this.txtYear.Style.Font.Name = "Calibri";
            this.txtYear.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.txtYear.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtYear.Value = "of Year <b>2016</b>";
            // 
            // txtProgram
            // 
            this.txtProgram.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.90000009536743164D));
            this.txtProgram.Name = "txtProgram";
            this.txtProgram.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.999920845031738D), Telerik.Reporting.Drawing.Unit.Inch(0.19996063411235809D));
            this.txtProgram.Style.Font.Bold = false;
            this.txtProgram.Style.Font.Name = "Calibri";
            this.txtProgram.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.txtProgram.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.txtProgram.Value = "Fund/Special Account : <b>Sample</b>";
            // 
            // txtOfficeDepartment
            // 
            this.txtOfficeDepartment.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.70000004768371582D));
            this.txtOfficeDepartment.Name = "txtOfficeDepartment";
            this.txtOfficeDepartment.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.999920845031738D), Telerik.Reporting.Drawing.Unit.Inch(0.19996063411235809D));
            this.txtOfficeDepartment.Style.Font.Bold = false;
            this.txtOfficeDepartment.Style.Font.Name = "Calibri";
            this.txtOfficeDepartment.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.txtOfficeDepartment.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.txtOfficeDepartment.Value = "Office/Department : <b>Sample</b>";
            // 
            // htmlTextBox1
            // 
            this.htmlTextBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.25D));
            this.htmlTextBox1.Name = "htmlTextBox1";
            this.htmlTextBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.999921798706055D), Telerik.Reporting.Drawing.Unit.Inch(0.19996063411235809D));
            this.htmlTextBox1.Style.Font.Bold = true;
            this.htmlTextBox1.Style.Font.Name = "Calibri";
            this.htmlTextBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.htmlTextBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.htmlTextBox1.Value = "BY OBJECT OF EXPENDITURES";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(10.999921798706055D), Telerik.Reporting.Drawing.Unit.Inch(0.19996063411235809D));
            this.txtTitle.Style.Font.Bold = true;
            this.txtTitle.Style.Font.Name = "Calibri";
            this.txtTitle.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(13D);
            this.txtTitle.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtTitle.Value = "PROGRAMMED APPROPRIATION AND OBLIGATION";
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.3288387060165405D);
            this.reportFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.htmlTextBox3,
            this.htmlTextBox4,
            this.htmlTextBox5,
            this.txtOfficehead,
            this.txtDesignation,
            this.txtBudgetHead,
            this.txtBudgetHeadDesignation,
            this.txtProvincialGovernorDesignatiom,
            this.txtProvincialGovernor});
            this.reportFooterSection1.Name = "reportFooterSection1";
            this.reportFooterSection1.PageBreak = Telerik.Reporting.PageBreak.None;
            this.reportFooterSection1.PrintAtBottom = true;
            this.reportFooterSection1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.reportFooterSection1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            // 
            // htmlTextBox3
            // 
            this.htmlTextBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.3305206298828125D), Telerik.Reporting.Drawing.Unit.Inch(0.085351310670375824D));
            this.htmlTextBox3.Name = "htmlTextBox3";
            this.htmlTextBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.96947938203811646D), Telerik.Reporting.Drawing.Unit.Inch(0.19996063411235809D));
            this.htmlTextBox3.Style.Font.Bold = true;
            this.htmlTextBox3.Style.Font.Name = "Calibri";
            this.htmlTextBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.htmlTextBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.htmlTextBox3.Value = "Prepared By:";
            // 
            // htmlTextBox4
            // 
            this.htmlTextBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.2000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.085351310670375824D));
            this.htmlTextBox4.Name = "htmlTextBox4";
            this.htmlTextBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0674291849136353D), Telerik.Reporting.Drawing.Unit.Inch(0.19996063411235809D));
            this.htmlTextBox4.Style.Font.Bold = true;
            this.htmlTextBox4.Style.Font.Name = "Calibri";
            this.htmlTextBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.htmlTextBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.htmlTextBox4.Value = "Reviewed By :";
            // 
            // htmlTextBox5
            // 
            this.htmlTextBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(8.1999998092651367D), Telerik.Reporting.Drawing.Unit.Inch(0.085351310670375824D));
            this.htmlTextBox5.Name = "htmlTextBox5";
            this.htmlTextBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.93477880954742432D), Telerik.Reporting.Drawing.Unit.Inch(0.19996063411235809D));
            this.htmlTextBox5.Style.Font.Bold = true;
            this.htmlTextBox5.Style.Font.Name = "Calibri";
            this.htmlTextBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.htmlTextBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.htmlTextBox5.Value = "Approved :";
            // 
            // txtOfficehead
            // 
            this.txtOfficehead.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.73689967393875122D));
            this.txtOfficehead.Name = "txtOfficehead";
            this.txtOfficehead.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8305206298828125D), Telerik.Reporting.Drawing.Unit.Inch(0.19385473430156708D));
            this.txtOfficehead.Style.Font.Bold = true;
            this.txtOfficehead.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtOfficehead.Value = "";
            // 
            // txtDesignation
            // 
            this.txtDesignation.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.93083316087722778D));
            this.txtDesignation.Name = "txtDesignation";
            this.txtDesignation.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8305206298828125D), Telerik.Reporting.Drawing.Unit.Inch(0.19385473430156708D));
            this.txtDesignation.Style.Font.Bold = false;
            this.txtDesignation.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtDesignation.Value = "";
            // 
            // txtBudgetHead
            // 
            this.txtBudgetHead.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.5D), Telerik.Reporting.Drawing.Unit.Inch(0.73689967393875122D));
            this.txtBudgetHead.Name = "txtBudgetHead";
            this.txtBudgetHead.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8999998569488525D), Telerik.Reporting.Drawing.Unit.Inch(0.19385473430156708D));
            this.txtBudgetHead.Style.Font.Bold = true;
            this.txtBudgetHead.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtBudgetHead.Value = "";
            // 
            // txtBudgetHeadDesignation
            // 
            this.txtBudgetHeadDesignation.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.5D), Telerik.Reporting.Drawing.Unit.Inch(0.950567901134491D));
            this.txtBudgetHeadDesignation.Name = "txtBudgetHeadDesignation";
            this.txtBudgetHeadDesignation.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8999998569488525D), Telerik.Reporting.Drawing.Unit.Inch(0.19385473430156708D));
            this.txtBudgetHeadDesignation.Style.Font.Bold = false;
            this.txtBudgetHeadDesignation.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtBudgetHeadDesignation.Value = "";
            // 
            // txtProvincialGovernorDesignatiom
            // 
            this.txtProvincialGovernorDesignatiom.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(8.9813308715820312D), Telerik.Reporting.Drawing.Unit.Inch(0.9452330470085144D));
            this.txtProvincialGovernorDesignatiom.Name = "txtProvincialGovernorDesignatiom";
            this.txtProvincialGovernorDesignatiom.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.999921441078186D), Telerik.Reporting.Drawing.Unit.Inch(0.19385473430156708D));
            this.txtProvincialGovernorDesignatiom.Style.Font.Bold = false;
            this.txtProvincialGovernorDesignatiom.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtProvincialGovernorDesignatiom.Value = "";
            // 
            // txtProvincialGovernor
            // 
            this.txtProvincialGovernor.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(8.9813308715820312D), Telerik.Reporting.Drawing.Unit.Inch(0.73689967393875122D));
            this.txtProvincialGovernor.Name = "txtProvincialGovernor";
            this.txtProvincialGovernor.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.999921441078186D), Telerik.Reporting.Drawing.Unit.Inch(0.19385473430156708D));
            this.txtProvincialGovernor.Style.Font.Bold = true;
            this.txtProvincialGovernor.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtProvincialGovernor.Value = "";
            // 
            // BudgetReport
            // 
            group1.GroupFooter = this.groupFooterSection;
            group1.GroupHeader = this.groupHeaderSection;
            group1.Name = "group3";
            group2.GroupFooter = this.groupFooterSection1;
            group2.GroupHeader = this.groupHeaderSection1;
            group2.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.OOEID"));
            group2.Name = "group4";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1,
            group2});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.groupHeaderSection,
            this.groupFooterSection,
            this.groupHeaderSection1,
            this.groupFooterSection1,
            this.detail,
            this.pageFooterSection1,
            this.reportHeaderSection1,
            this.reportFooterSection1});
            this.Name = "BudgetReport";
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.PageSettings.PaperSize = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.5D), Telerik.Reporting.Drawing.Unit.Inch(13D));
            this.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(11D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.Barcode QRCode;
        private Telerik.Reporting.HtmlTextBox txtPageAndYear;
        private Telerik.Reporting.TextBox txtpage;
        private Telerik.Reporting.ReportHeaderSection reportHeaderSection1;
        private Telerik.Reporting.ReportFooterSection reportFooterSection1;
        private Telerik.Reporting.HtmlTextBox txtYear;
        private Telerik.Reporting.HtmlTextBox txtProgram;
        private Telerik.Reporting.HtmlTextBox txtOfficeDepartment;
        private Telerik.Reporting.HtmlTextBox htmlTextBox1;
        private Telerik.Reporting.HtmlTextBox txtTitle;
        private Telerik.Reporting.HtmlTextBox htmlTextBox3;
        private Telerik.Reporting.HtmlTextBox htmlTextBox4;
        private Telerik.Reporting.HtmlTextBox htmlTextBox5;
        private Telerik.Reporting.TextBox txtOfficehead;
        private Telerik.Reporting.TextBox txtDesignation;
        private Telerik.Reporting.TextBox txtBudgetHead;
        private Telerik.Reporting.TextBox txtBudgetHeadDesignation;
        private Telerik.Reporting.TextBox txtProvincialGovernorDesignatiom;
        private Telerik.Reporting.TextBox txtProvincialGovernor;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox23;
        private Telerik.Reporting.TextBox textBox24;
        private Telerik.Reporting.TextBox textBox25;
        private Telerik.Reporting.TextBox textBox26;
        private Telerik.Reporting.TextBox textBox22;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection;
        private Telerik.Reporting.TextBox chAccounts;
        private Telerik.Reporting.TextBox chAccountCode;
        private Telerik.Reporting.TextBox chPastYear;
        private Telerik.Reporting.TextBox chCurrentYear;
        private Telerik.Reporting.TextBox chBudgetYear;
        private Telerik.Reporting.GroupFooterSection groupFooterSection;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox textBox27;
        private Telerik.Reporting.TextBox textBox28;
        private Telerik.Reporting.TextBox textBox29;
        private Telerik.Reporting.TextBox textBox30;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection1;
        private Telerik.Reporting.TextBox textBox16;
        private Telerik.Reporting.TextBox textBox17;
        private Telerik.Reporting.TextBox textBox18;
        private Telerik.Reporting.TextBox textBox20;
        private Telerik.Reporting.TextBox textBox21;
        private Telerik.Reporting.GroupFooterSection groupFooterSection1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox10;

    }
}