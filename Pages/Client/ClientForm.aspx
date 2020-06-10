<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ClientForm.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Client.ClientForm" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script>
        function CheckFieldValidation(s, e) {
            var process = false;

            var inputItems = [clientTxtNaziv];
            var comboBoxItems = [clientComboBoxZaposleni, clientComboBoxTip]
            process = InputFieldsValidation(null, inputItems, null, null, comboBoxItems, null);

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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="small-padding-top">
        <ul class="nav nav-tabs">
            <li class="active"><a data-toggle="tab" href="#basic"><span class="glyphicon glyphicon-import"></span>Osnovni podatki</a></li>
            <li class="hidden"><a data-toggle="tab" href="#tender"><span class="badge" runat="server" id="tenderBadge">0</span> Razpisi</a></li>
            <li class="hidden"><a data-toggle="tab" href="#route"><span class="badge" runat="server" id="routeBadge">0</span> Relacija</a></li>
            <li class="hidden"><a data-toggle="tab" href="#recall"><span class="badge" runat="server" id="recallBadge">0</span> Odpoklic</a></li>
            <li class="hidden"><a data-toggle="tab" href="#contact"><span class="badge" runat="server" id="contactBadge">0</span> Kontaktne osebe</a></li>
        </ul>
        <div class="tab-content">
            <div id="basic" class="tab-pane fade in active">
                <div class="panel panel-default" style="border-top: none; background-color: transparent;">
                    <div class="panel-body">
                        <div class="row small-padding-bottom">
                            <div class="col-md-12">
                                <div class="row2" style="align-items: center">
                                    <div class="col-sm-2 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="NAZIV PRVI : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-10 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtNazivPrvi" ClientInstanceName="clientTxtNaziv"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <ClientSideEvents Init="SetFocus" />
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row medium-padding-bottom">
                            <div class="col-md-12">
                                <div class="row2" style="align-items: center">
                                    <div class="col-sm-2 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="NAZIV DRUGI : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-10 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtNazivDrugi"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row small-padding-bottom">
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center">
                                    <div class="col-sm-4 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="NASLOV : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-8 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtNaslov"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center">
                                    <div class="col-sm-4 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="ŠTEV. POŠTE : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-8 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtStevPoste"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center">
                                    <div class="col-sm-4 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="NAZIV POŠTE : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-8 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtNazivPoste"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row small-padding-bottom">
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center">
                                    <div class="col-sm-4 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="TELEFON : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-8 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtTelefon"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center">
                                    <div class="col-sm-4 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="FAX : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-8 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtFAX"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center">
                                    <div class="col-sm-4 no-padding-right" style="align-items: center;">
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="EMAIL : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-8 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtEmail"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row small-padding-bottom">
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center">
                                    <div class="col-sm-4 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="TIP : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-8 no-padding-left">
                                        <dx:ASPxComboBox ID="ComboBoxTip" runat="server" ValueType="System.String" DropDownStyle="DropDownList"
                                            IncrementalFilteringMode="StartsWith" TextField="Naziv" ValueField="TipStrankaID"
                                            EnableSynchronization="False" Width="100%" OnDataBinding="ComboBoxTip_DataBinding"
                                            Font-Size="14px" Font-Names="Segoe UI" CssClass="text-box-input" ClientInstanceName="clientComboBoxTip">
                                            <ClearButton DisplayMode="OnHover" />
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center">
                                    <div class="col-sm-4 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="SKRBNIK : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-8 no-padding-left">
                                        <dx:ASPxComboBox ID="ComboBoxSkrbnik" runat="server" ValueType="System.String" DropDownStyle="DropDownList"
                                            IncrementalFilteringMode="StartsWith" TextField="CelotnoIme" ValueField="idOsebe"
                                            EnableSynchronization="False" Width="100%" OnDataBinding="ComboBoxSkrbnik_DataBinding"
                                            Font-Size="14px" Font-Names="Segoe UI" CssClass="text-box-input" ClientInstanceName="clientComboBoxZaposleni">
                                            <ClearButton DisplayMode="OnHover" />
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
                            <span class="AddEditButtons">
                                <dx:ASPxButton Theme="Moderno" ID="btnConfirm" runat="server" Text="Shrani" AutoPostBack="false"
                                    Height="25" Width="110" ClientInstanceName="clientBtnConfirm" UseSubmitBehavior="false"
                                    OnClick="btnConfirm_Click">
                                    <Paddings PaddingLeft="10" PaddingRight="10" />
                                    <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                                    <ClientSideEvents Click="CheckFieldValidation" />
                                </dx:ASPxButton>
                            </span>
                            <span class="AddEditButtons">
                                <dx:ASPxButton Theme="Moderno" ID="btnCancel" runat="server" Text="Prekliči" AutoPostBack="false"
                                    Height="25" Width="110" UseSubmitBehavior="false" OnClick="btnCancel_Click">
                                    <Paddings PaddingLeft="10" PaddingRight="10" />
                                    <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                                </dx:ASPxButton>
                            </span>
                        </div>

                    </div>
                </div>
            </div>

            <div id="tender" class="tab-pane fade">
                <div class="panel panel-default" style="border-top: none; background-color: transparent;">
                    <div class="panel-body">
                        d
                    </div>
                </div>
            </div>

            <div id="route" class="tab-pane fade">
                <div class="panel panel-default" style="border-top: none; background-color: transparent;">
                    <div class="panel-body">
                        d
                    </div>
                </div>
            </div>

            <div id="recall" class="tab-pane fade">
                <div class="panel panel-default" style="border-top: none; background-color: transparent;">
                    <div class="panel-body">
                        d
                    </div>
                </div>
            </div>

            <div id="contact" class="tab-pane fade">
                <div class="panel panel-default" style="border-top: none; background-color: transparent;">
                    <div class="panel-body">
                        d
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
