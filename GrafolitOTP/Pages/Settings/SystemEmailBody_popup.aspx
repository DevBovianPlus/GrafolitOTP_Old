<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="SystemEmailBody_popup.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Settings.SystemEmailBody_popup" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/Popup.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolderPopup" runat="server">
    <script type="text/javascript">
        function btnConfirmPopUp_Click(s, e) {            
            e.processOnServer = true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolderPopup" runat="server">

    <div class="row" style="align-items: center">
        <div class="col">
            <dx:ASPxHtmlEditor ID="ASPxHtmlEditorEmailBody" runat="server" Width="100%">
                <Settings AllowContextMenu="False" AllowDesignView="false" AllowHtmlView="false" AllowPreview="true"></Settings>
            </dx:ASPxHtmlEditor>
        </div>
    </div>

    <div class="row">
       
        <div class="col-2" style="align-items: flex-end">
            <dx:ASPxButton ID="btnConfirmPopUp" runat="server" Text="Potrdi" AutoPostBack="false"
                ClientInstanceName="clientBtnConfirm" OnClick="btnConfirmPopUp_Click" UseSubmitBehavior="false"
                Width="100px">
                <ClientSideEvents Click="btnConfirmPopUp_Click" />                
            </dx:ASPxButton>
        </div>
    </div>
  
</asp:Content>
