<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="iFMIS_BMS.Reports.ReportViewer" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=16.1.22.622, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
        <%--<link rel="SHORTCUT ICON" href="~/Images/PPC.png" />--%>
        <link rel="SHORTCUT ICON" href="~/Images/Ph_seal_agusan_del_sur.png" />
        <%--<link rel="SHORTCUT ICON" href="~/Images/pgzn_logo.png" />--%>
         <%--<link rel="SHORTCUT ICON" href="~/Images/tacurong.png" />--%>
    </head>
    <body style="position: fixed; top: 0px; bottom: 0px; right: 0px; left: 0px;">
        <form id="form1" runat="server" style="position: fixed; top: 0px; bottom: 0px; right: 0px; left: 0px;">
            <div style="position: fixed; top: 0px; bottom: 0px; right: 0px; left: 0px;">
                <%--<telerik:ReportViewer ID="RV" runat="server" Style="position: absolute; top: 0px; bottom: 0px; right: 0px; left: 0px; width: auto; height: auto;"></telerik:ReportViewer>--%>
                <telerik:ReportViewer ID="RV" runat="server" Style="position: absolute; top: 0px; bottom: 0px; right: 0px; left: 0px; width: auto; height: auto;"></telerik:ReportViewer>
               

           <%--     <telerkik:ReportViewer ID="RV" runat="server"></telerik:ReportViewer>--%>

               <script type="text/javascript">

                    RV.prototype.PrintReport = function () {

                        switch (this.defaultPrintFormat) {

                            //case "Default":

                            //    this.DefaultPrint();

                            //    break;

                            case "PDF":

                                this.PrintAs("PDF");

                                previewFrame = document.getElementById(this.previewFrameID);

                                previewFrame.onload = function () { previewFrame.contentDocument.execCommand("print", true, null); }

                                break;

                        }

                    };
               </script>
               
            </div>
        </form>
    </body>
</html>