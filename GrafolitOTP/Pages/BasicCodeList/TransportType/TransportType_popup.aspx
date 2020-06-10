<%@ Page Title="Tip prevoza" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="TransportType_popup.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.BasicCodeList.TransportType.TransportType_popup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolderPopup" runat="server">
    <script type="text/javascript">
        function CheckFieldValidation(s, e) {
            var process = false;
            var inputItems = [clientTxtNaziv, clientTxtDovoljenaTeza];

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
        </ul>
        <div class="tab-content">
            <div id="basic" class="tab-pane fade in active">
                <div class="panel panel-default" style="border-top: none; background-color: transparent;">
                    <div class="panel-body">
                        <div class="row small-padding-bottom">
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center">
                                    <div class="col-xs-0 no-padding-right" style="margin-right:90px;">
                                        <dx:ASPxLabel ID="lblRelacija" runat="server" Text="KODA : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-8 no-padding-left">
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
                                    <div class="col-xs-0 no-padding-right" style="margin-right:87px;">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="NAZIV : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-9 no-padding-left">
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
                                    <div class="col-xs-0 no-padding-right big-margin-r">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="DOVOLJENA TEŽA : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-6 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtDovoljenaTeza" ClientInstanceName="clientTxtDovoljenaTeza"
                                            CssClass="text-box-input" Font-Size="14px" AutoCompleteType="Disabled" HorizontalAlign="Right" NullText="0,00">
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
                                    <div class="col-xs-0 no-padding-right big-margin-r" style="margin-right:43px;">
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="SHRANI POZ. : ">                                            
                                        </dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-8 no-padding-left">
                                        <dx:ASPxCheckBox ID="CheckBoxShraniPozicije" runat="server" ToggleSwitchDisplayMode="Always">
                                        </dx:ASPxCheckBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row small-padding-bottom">
                            <div class="col-md-6">
                                <div class="row2" style="align-items: center">
                                    <div class="col-xs-0 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="OPOMBA : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-12 no-padding-left">
                                        <dx:ASPxMemo ID="ASPxMemoOpomba" runat="server" Width="100%" MaxLength="2000" Theme="Moderno"
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
