<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="SystemEmail_popup.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Settings.SystemEmail_popup" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/Popup.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolderPopup" runat="server">
    <script type="text/javascript">
        var postbackInitiated = false;

        function CheckFieldValidation() {
            var process = false;
            var lookUpItems = null;

            var inputItems = [txtEmailTo, txtSubject];
            var dateItems = null;

            if (btnConfirm.GetText() == 'Izbriši')
                process = true;
            else
                process = InputFieldsValidation(/*lookUpItems*/null, inputItems, /*dateItems*/null, /*memoItems*/ null, /*comboBoxItems*/null, null);

            return process;
        }

        function btnCancelPopUp_Click(s, e) {
            postbackInitiated = true;
        }

        function btnConfirmPopUp_Click(s, e) {
            var process = CheckFieldValidation();

            if (process) {
                e.processOnServer = !postbackInitiated;
                postbackInitiated = true;
            }
            else
                e.processOnServer = false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolderPopup" runat="server">
    <div class="card bg-transparent">
        <div class="card-body p-0">
            <div class="row m-0 pb-3 pt-3">
                <div class="col-md-6 mb-2 mb-md-0">
                    <div class="row m-0 align-items-center">
                        <div class="col-0 p-0" style="margin-right: 53px;">
                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="OD : " Font-Size="12px"></dx:ASPxLabel>
                        </div>
                        <div class="col p-0">
                            <dx:ASPxTextBox runat="server" ID="txtEmailFrom" ClientInstanceName="txtEmailFrom" MaxLength="200"
                                CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled" ClientEnabled="false" Font-Bold="true" BackColor="LightGray">
                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 mb-2 mb-md-0">
                    <div class="row m-0 align-items-center justify-content-end">
                        <div class="col-0 p-0 mr-3">
                            <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="ZA : *" Font-Size="12px"></dx:ASPxLabel>
                        </div>
                        <div class="col p-0">
                            <dx:ASPxTextBox runat="server" ID="txtEmailTo" ClientInstanceName="txtEmailTo" MaxLength="200"
                                CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row m-0 pb-3">
                <div class="col-lg-12 mb-2 mb-lg-0">
                    <div class="row m-0 align-items-center">
                        <div class="col-0 p-0 mr-3">
                            <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="NASLOV : *" Font-Size="12px"></dx:ASPxLabel>
                        </div>
                        <div class="col p-0">
                            <dx:ASPxTextBox runat="server" ID="txtSubject" ClientInstanceName="txtSubject" MaxLength="200"
                                CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row m-0 pb-3">
                <div class="col-lg-12 mb-2 mb-lg-0">
                    <dx:ASPxHtmlEditor ID="ASPxHtmlEditorEmailBody" runat="server" Width="100%" Height="570">
                        <Settings AllowContextMenu="true" AllowDesignView="true" AllowHtmlView="false" AllowPreview="true"></Settings>
                    </dx:ASPxHtmlEditor>
                </div>
            </div>

        </div>
    </div>

    <div class="row pt-3">
        <div class="col text-left">
            <dx:ASPxButton ID="btnCancelPopUp" runat="server" Text="Prekliči" AutoPostBack="false"
                ClientInstanceName="clientBtnCancel" OnClick="btnCancelPopUp_Click" UseSubmitBehavior="false"
                Width="100px">
                <ClientSideEvents Click="btnCancelPopUp_Click" />
                <Image Url="../../Images/reject.png" UrlHottracked="../../Images/reject.png" />
            </dx:ASPxButton>
        </div>

        <div class="col text-right">
            <dx:ASPxButton ID="btnConfirmPopUp" runat="server" Text="Pošlji" AutoPostBack="false"
                ClientInstanceName="btnConfirm" OnClick="btnConfirmPopUp_Click" UseSubmitBehavior="false"
                Width="100px">
                <ClientSideEvents Click="btnConfirmPopUp_Click" />
                <Image Url="../../Images/systemEmail.png" UrlHottracked="../../Images/systemEmail.png" />
            </dx:ASPxButton>
        </div>
    </div>

</asp:Content>
