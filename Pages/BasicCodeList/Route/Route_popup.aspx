<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="Route_popup.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.BasicCodeList.Route.Route_popup" %>

<%@ MasterType VirtualPath="~/Popup.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolderPopup" runat="server">
    <script>
        function CheckFieldValidation(s, e) {
            var process = false;
            var inputItems = [clientTxtNaziv];

            process = InputFieldsValidation(null, inputItems, null, null, null, null);

            if (clientBtnConfirm.GetText() == 'Izbriši')
                process = true;

            if (process)
                e.processOnServer = true;
            else
                e.processOnServer = false;

            return process;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolderPopup" runat="server">
    <div class="small-padding-top">
        <ul class="nav nav-tabs">
            <li class="active"><a data-toggle="tab" href="#basic"><span class="glyphicon glyphicon-import"></span>Osnovni podatki</a></li>
            <li class="hidden"><a data-toggle="tab" href="#attachments"><span class="badge" runat="server" id="attachmentBadge">0</span> Priloge</a></li>
        </ul>
        <div class="tab-content">
            <div id="basic" class="tab-pane fade in active">
                <div class="panel panel-default" style="border-top: none; background-color: transparent;">
                    <div class="panel-body">
                        <div class="row small-padding-bottom">
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center">
                                    <div class="col-xs-2 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="DATUM : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-10 no-padding-left">
                                        <dx:ASPxDateEdit ID="DateEditDatum" runat="server" EditFormat="Date" Width="170" Theme="Moderno"
                                            CssClass="text-box-input date-edit-padding" Font-Size="13px"
                                            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                                            <FocusedStyle CssClass="focus-text-box-input" />
                                            <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                            <DropDownButton Visible="true"></DropDownButton>
                                        </dx:ASPxDateEdit>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row small-padding-bottom">
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center">
                                    <div class="col-xs-2 no-padding-right">
                                        <dx:ASPxLabel ID="lblRelacija" runat="server" Text="KODA : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-10 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtKoda" ClientInstanceName="clientTxtKoda"
                                            CssClass="text-box-input" Font-Size="14px" Width="60%" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row small-padding-bottom">
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center;">
                                    <div class="col-xs-2 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="NAZIV : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-10 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtNaziv" ClientInstanceName="clientTxtNaziv"
                                            CssClass="text-box-input" Font-Size="14px" Width="100%" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row small-padding-bottom">
                            <div class="col-md-6">
                                <div class="row2" style="align-items: center">
                                    <div class="col-xs-2 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="DOLŽINA : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-10 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtDolzina" ClientInstanceName="clientTxtDolzina"
                                            CssClass="text-box-input" Font-Size="14px" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <ClientSideEvents KeyPress="isNumberKey_decimal" />
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row small-padding-bottom">
                            <div class="col-md-6">
                                <div class="row2" style="align-items: center">
                                    <div class="col-xs-2 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="OPOMBA : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-10 no-padding-left">
                                        <dx:ASPxMemo ID="ASPxMemoOpomba" runat="server" Width="100%" MaxLength="300" Theme="Moderno"
                                        NullText="Opomba..." Rows="3" HorizontalAlign="Left" BackColor="White"
                                        CssClass="text-box-input" Font-Size="14px" AutoCompleteType="Disabled">
                                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                    </dx:ASPxMemo>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="attachments" class="tab-pane fade hidden">
                <div class="panel panel-default" style="border-top: none; background-color: transparent;">
                    <div class="panel-body">
                        Test
                    </div>
                </div>
            </div>
        </div>
        <div class="AddEditButtonsWrap">
            <div class="AddEditButtonsElements" style="margin-top: 10px; margin-bottom:15px;">
                <span class="AddEditButtons">
                    <dx:ASPxButton ID="btnCancelPopUp" runat="server" Text="Prekliči"  AutoPostBack="false"
                        Height="20" Width="80" OnClick="btnCancelPopUp_Click">
                        <Image Url="../../../Images/cancelPopUp.png"></Image>
                    </dx:ASPxButton>
                </span>
                <span class="AddEditButtons">
                    <dx:ASPxButton ID="btnConfirmPopUp" runat="server" Text="Potrdi"  AutoPostBack="false"
                        ValidationGroup="Confirm" ClientInstanceName="clientBtnConfirm" OnClick="btnConfirmPopUp_Click">
                        <ClientSideEvents Click="CheckFieldValidation" />
                    </dx:ASPxButton>
                </span>
            </div>
        </div>
    </div>
</asp:Content>
