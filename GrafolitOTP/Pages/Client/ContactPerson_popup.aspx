<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="ContactPerson_popup.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Client.ContactPerson_popup" %>

<%@ MasterType VirtualPath="~/Popup.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolderPopup" runat="server">
    <script>
        var postbackInitiated = false;

        function CheckFieldValidation() {
            var process = false;
            var lookUpItems = [];

            var inputItems = [txtName, txtEmail];
            var dateItems = null;

            process = InputFieldsValidation(lookUpItems, inputItems, dateItems, /*memoItems*/ null, /*comboBoxItems*/null, null);

            if (btnConfirm.GetText() == 'Izbriši')
                process = true;

            return process;
        }

        function ActionButton_Click(s, e) {
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
    <div class="small-padding-top">
        <ul class="nav nav-tabs">
            <li class="active"><a data-toggle="tab" href="#basic"><span class="glyphicon glyphicon-import"></span>Osnovni podatki</a></li>
        </ul>
        <div class="tab-content">
            <div id="basic" class="tab-pane fade in active">
                <div class="panel panel-default" style="border-top: none; background-color: transparent;">
                    <div class="panel-body">
                        <div class="row small-padding-bottom">
                            <div class="col-md-12">
                                <div class="row2" style="align-items: center">
                                    <div class="col-xs-1 no-padding-right no-padding-left">
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="NAZIV : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-11 no-padding-left" style="padding-left: 34px !important;">
                                        <dx:ASPxTextBox runat="server" ID="txtName" ClientInstanceName="txtName" MaxLength="300"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                       
                        <div class="row small-padding-bottom">
                            <div class="col-sm-6">
                                <div class="row2" style="align-items: center;">
                                    <div class="col-xs-3 no-padding-right no-padding-left">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="TELEFON : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-9 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtPhone" ClientInstanceName="txtPhone" MaxLength="50"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-6">
                                <div class="row2" style="align-items: center; justify-content:flex-end">
                                    <div class="col-xs-2 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="GSM : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-9 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtPhoneGSM" ClientInstanceName="txtPhoneGSM" MaxLength="50"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row small-padding-bottom">
                            <div class="col-sm-6">
                                <div class="row2" style="align-items: center">
                                    <div class="col-xs-3 no-padding-right no-padding-left">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="FAX : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-9 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtFax" ClientInstanceName="txtFax" MaxLength="30"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-6">
                                <div class="row2" style="align-items: center; justify-content:flex-end;">
                                    <div class="col-xs-2 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="EMAIL : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-9 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtEmail" ClientInstanceName="txtEmail" MaxLength="50"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row small-padding-bottom">
                            <div class="col-md-6">
                                <div class="row2" style="align-items: center">
                                    <div class="col-xs-1 no-padding-right no-padding-left">
                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="PODPIS : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-11 no-padding-left" style="padding-left: 34px !important;">
                                        <dx:ASPxTextBox runat="server" ID="txtSignature" ClientInstanceName="txtSignature" MaxLength="30"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row small-padding-bottom">
                            <div class="col-md-6">
                                <div class="row2" style="align-items: center">
                                    <div class="col-xs-1 no-padding-right no-padding-left">
                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="OPOMBE : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-11 no-padding-left" style="padding-left: 34px !important;">
                                        <dx:ASPxMemo ID="MemoNotes" runat="server" Width="100%" Rows="8" MaxLength="1000" CssClass="text-box-input" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxMemo>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

        </div>
        <div class="AddEditButtonsWrap">
            <div class="AddEditButtonsElements" style="margin-top: 10px; margin-bottom: 15px;">
                <span class="AddEditButtons">
                    <dx:ASPxButton ID="btnCancelPopUp" runat="server" Text="Prekliči" AutoPostBack="false"
                        Height="20" Width="80" OnClick="btnCancel_Click">
                        <Image Url="../../Images/cancelPopUp.png"></Image>
                    </dx:ASPxButton>
                </span>
                <span class="AddEditButtons">
                    <dx:ASPxButton ID="btnConfirm" runat="server" Text="Potrdi" AutoPostBack="false"
                        ValidationGroup="Confirm" ClientInstanceName="btnConfirm" OnClick="btnConfirm_Click">
                        <ClientSideEvents Click="ActionButton_Click" />
                    </dx:ASPxButton>
                </span>
            </div>
        </div>
    </div>
</asp:Content>
