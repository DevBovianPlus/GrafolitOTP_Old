<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ReportPreview.aspx.cs" Inherits="OptimizacijaTransprotov.Reports.ReportPreview" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<%@ Register Assembly="DevExpress.XtraReports.v19.2.Web.WebForms, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">
        function customizeActions(s, e) {
            e.Actions.push({
                text: "Izhod",
                imageClassName: "exit-logo-html5",
                disabled: ko.observable(false),
                visible: true,
                clickAction: function () {
                    history.back();
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" ShowCollapseButton="true" Width="100%" ShowHeader="false">
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxWebDocumentViewer ID="ASPxWebDocumentViewer" runat="server" ColorScheme="dark" Width="100%">
                    <ClientSideEvents Init="function(s, e) {s.previewModel.reportPreview.zoom(1);}" CustomizeMenuActions="customizeActions" />
                </dx:ASPxWebDocumentViewer>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</asp:Content>
