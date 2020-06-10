<%@ Page Title="Prijava prevoznika" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SubmitToTender.aspx.cs" Inherits="GrafolitOTPRazpis.SubmitToTender" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">

        var firstLoadCheck = false;
        $(document).ready(function () {

            if (!firstLoadCheck) {
                LoadingPanel.Show();
                CallbackPanel.PerformCallback('CheckAuthentication');
            }
        });

        function CallbackPanel_EndCallback(s, e) {
            LoadingPanel.Hide();
            if (s.cpNoQueryStringError != null && s.cpNoQueryStringError !== undefined) {
                delete (s.cpNoQueryStringError);

                ShowWarningPopUp("<%=Resources.Prevodi.res_25 %>", "<%=Resources.Prevodi.res_25 %>");//Napačna zahteva   ----  Poslana zahteva ne ustreza!

                setTimeout(function () { window.location.replace("Home.aspx"); }, 6000);
            }
            else if (s.cpQueryStringValidationError != null && s.cpQueryStringValidationError !== undefined) {
                delete (s.cpQueryStringValidationError);

                ShowWarningPopUp("<%=Resources.Prevodi.res_25 %>", "<%=Resources.Prevodi.res_27 %>");//Napačna zahteva  ----  Poslana zahteva ne ustreza predvideni!

                setTimeout(function () { window.location.replace("Home.aspx"); }, 6000);
            }
            else if (s.cpTimeForSubmittingPriceExpired != null && s.cpTimeForSubmittingPriceExpired !== undefined) {
                delete (s.cpTimeForSubmittingPriceExpired);

                ShowWarningPopUp("<%=Resources.Prevodi.res_28 %>", "<%=Resources.Prevodi.res_29 %>");//Prijava potekla!  ----  Žal je prijava za izbrano relacijo potekla.

                setTimeout(function () { window.location.replace("Home.aspx"); }, 6000);
            }
            else if (s.cpRedirectToThankYouPage != null && s.cpRedirectToThankYouPage !== undefined) {
                delete (s.cpRedirectToThankYouPage);

                setTimeout(function () { window.location.replace("ThankYou.aspx"); }, 200);
            }
        }

        function ShowWarningPopUp(title, message) {
            $('#warningModal').modal('show');
            $('#modalBodyText').empty();
            $('#modalBodyText').append(message);
            $('#myModalTitle').empty();
            $('#myModalTitle').append(title);
        }

        function isNumberKey_int(s, e) {
            var charCode = e.htmlEvent.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                ASPxClientUtils.PreventEvent(e.htmlEvent);

            return true;
        }

        function isNumberKey_decimal(s, e) {
            var charCode = e.htmlEvent.keyCode;
            if (charCode != 44 && charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57))
                ASPxClientUtils.PreventEvent(e.htmlEvent);

            return true;
        }

        function btnSubmitPrice_Click() {
            var inputItems = [txtSubmitPrice];
            var checkItems = [CheckBoxBusinessConditions];
            var process = InputFieldsValidation(inputItems, checkItems);

            if (CheckBoxBusinessConditions.GetChecked() && process) {
                LoadingPanel.Show();
                CallbackPanel.PerformCallback('SubmitPrice');
            }
        }


        function InputFieldsValidation(inputFields, checkBoxItems) {
            var procees = true;

            if (inputFields != null) {
                for (var i = 0; i < inputFields.length; i++) {

                    var item = inputFields[i];
                    if (item.GetText() == "") {
                        $(item.GetInputElement()).parent().parent().parent().addClass("focus-text-box-input-error");
                        procees = false;
                    }
                    else
                        $(item.GetInputElement()).parent().parent().parent().removeClass("focus-text-box-input-error");
                }
            }

            if (checkBoxItems != null) {
                for (var i = 0; i < checkBoxItems.length; i++) {
                    var item = checkBoxItems[i];
                    if (item.GetChecked() == null || !item.GetChecked()) {
                        $(item.GetInputElement()).parent().parent().addClass("focus-text-box-input-error");
                        procees = false;
                    }
                    else
                        $(item.GetInputElement()).parent().parent().removeClass("focus-text-box-input-error");
                }
            }

            return procees;
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxCallbackPanel ID="CallbackPanel" runat="server" ClientInstanceName="CallbackPanel" OnCallback="CallbackPanel_Callback">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="CallbackPanel_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="container-fluid p-0">
                    <div class="banner overlay">
                        <img src="http://grafolit.si/images/logo.png" alt="logo" width="180" height="70" style="display: block; position: absolute; left: 10px; top: 10px;" />
                        <h1><%= Resources.Prevodi.res_11 %>
                            <br />
                            <small><%= Resources.Prevodi.res_12 %></small><%-- Premagajte konkurenco --%>
                        </h1>
                        <%-- Prijava prevoznika --%>
                    </div>
                    <div class="container mt-5">
                        <div class="row mb-5">
                            <div class="col-lg-5 text-left">
                                <h3>
                                    <dx:ASPxLabel Font-Size="26px" ID="lblPozdrav" runat="server" />
                                </h3>
                                <p class="mt-4">
                                    <%= Resources.Prevodi.res_13 %>
                                </p>
                                <%-- Prosimo vas, da na desni strani, v polje PRIJAVNA CENA vpišete ceno, za katero ste pripravljeni peljati blago na relaciji, ki je navedena na desni strani. Ostali podatki o prevozu, so prav tako zapisani na desni strani. 
Opozarjamo, da z oddajo cene, jamčite za ceno in se obvežete, da boste zagotovili prevoz na zapisani relaciji in na zapisan datum (DATUM NAKLADA), v kolikor boste izbrani za prevoz. --%>
                                <br />
                                <p class="mt-4">
                                    <%= Resources.Prevodi.res_14 %>
                                </p>
                                <%-- Opozarjamo, da z oddajo cene, jamčite za ceno in se obvežete, da boste zagotovili prevoz na zapisani relaciji in na zapisan datum (DATUM NAKLADA), v kolikor boste izbrani za prevoz. --%>
                                <%--<div class="mt-4">
                                    <button type="button" class="btn btn-primary">Več informacij</button>
                                </div>--%>
                            </div>
                            <div class="col-lg-7">
                                <div class="row mb-4">
                                    <div class="col-lg-12">
                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="<%$Resources:Prevodi, res_15 %>" />
                                        <%--  ŠT. ODPOKLICA --%>
                                        <dx:ASPxTextBox ID="txtStOdpoklica" runat="server" Width="100%" BackColor="White" ReadOnly="true" Font-Bold="true" ClientEnabled="false">
                                            <FocusedStyle CssClass="focus-text-box-input" />
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="row mb-4">
                                    <div class="col-lg-12">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="<%$Resources:Prevodi, res_16 %>" />
                                        <%-- RELACIJA --%>
                                        <dx:ASPxTextBox ID="txtRoute" runat="server" Width="100%" BackColor="White" ReadOnly="true" Font-Bold="true" ClientEnabled="false">
                                            <FocusedStyle CssClass="focus-text-box-input" />
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>

                                <div class="row mb-4">
                                    <div class="col-lg-12">
                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="<%$Resources:Prevodi, res_17 %>" />
                                        <%-- PODATKI O BLAGU --%>
                                        <dx:ASPxMemo runat="server" Rows="4" Width="100%" NullText="Ni dodatnih podatkov o blagu..." ID="memOpombaPrevoznikov"
                                            ClientInstanceName="clientMemoOpombaPrevoznikov" MaxLength="4000" CssClass="text-box-input" ReadOnly="true">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxMemo>
                                    </div>
                                </div>

                                <div class="row mb-4">
                                    <div class="col-lg-6">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="<%$Resources:Prevodi, res_18 %>" />
                                        <%-- DATUM NAKLADA --%>
                                        <dx:ASPxDateEdit ID="DateEditDatumNaklada" runat="server" EditFormat="Date" Width="100%" Theme="Moderno"
                                            BackColor="White" ReadOnly="true" Font-Bold="true" ClientEnabled="false" Font-Size="13px" ClientInstanceName="DateEditDatumNaklada">
                                            <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                            <DropDownButton Visible="true"></DropDownButton>
                                        </dx:ASPxDateEdit>
                                    </div>
                                    <div class="col-lg-6">
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="<%$Resources:Prevodi, res_19 %>" />
                                        <%-- PREDLAGANA CENA --%>
                                        <dx:ASPxTextBox ID="txtPrice" runat="server" Width="100%" BackColor="White" ReadOnly="true" Font-Bold="true" ClientEnabled="false">
                                            <FocusedStyle CssClass="focus-text-box-input" />
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>

                                <div class="row mb-4">
                                    <div class="col-lg-5">
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="<%$Resources:Prevodi, res_20 %>" />
                                        <%-- PRIJAVNA CENA --%>
                                        <dx:ASPxTextBox ID="txtSubmitPrice" runat="server" Width="100%" CssClass="text-box-input" Font-Bold="true" ClientInstanceName="txtSubmitPrice">
                                            <FocusedStyle CssClass="focus-text-box-input" />
                                            <ClientSideEvents KeyPress="isNumberKey_decimal" />
                                        </dx:ASPxTextBox>
                                    </div>
                                    <div class="col-lg-7">
                                    </div>
                                </div>

                                <div class="row mb-4">
                                    <div class="col-lg-12">
                                        <div class="row d-flex align-items-center">
                                            <div class="col-1">
                                                <dx:ASPxCheckBox ID="CheckBoxBusinessConditions" runat="server" ClientInstanceName="CheckBoxBusinessConditions"></dx:ASPxCheckBox>
                                            </div>
                                            <div class="col-11 pl-0">
                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="<%$Resources:Prevodi, res_21 %>" />
                                                <%-- STRINJAM SE S POGOJI PRIJAVE --%>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row mb-4">
                                    <div class="col-lg-4">
                                        <dx:ASPxButton ID="btnSubmitPrice" runat="server" ForeColor="#007bff" UseSubmitBehavior="false" AutoPostBack="false" Text="<%$Resources:Prevodi, res_22 %>"
                                            ClientInstanceName="btnSubmitPrice">
                                            <%-- Oddaj ceno --%>
                                            <ClientSideEvents Click="btnSubmitPrice_Click" />
                                        </dx:ASPxButton>

                                    </div>
                                </div>

                            </div>
                        </div>

                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title"><%= Resources.Prevodi.res_23 %></h4>
                                <%-- Pogoji prijave --%>
                                <p class="card-text"><%= Resources.Prevodi.res_24 %></p>
                                <%-- Opozarjamo, da z oddajo cene, jamčite za ceno in se obvežete, da boste zagotovili prevoz na zapisani relaciji in na zapisan datum (DATUM NAKLADA), v kolikor boste izbrani za prevoz. --%>
                                <%--<a href="#" class="card-link">Kontakt</a>--%>
                            </div>
                        </div>

                        <br />

                        <div class="row mb-4" id="contact">
                            <div class="col-lg-4">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-3 d-flex align-items-center justify-content-center">
                                                <i class="fa fa-address-card-o" style="font-size: 50px; color: #007bff;"></i>
                                            </div>
                                            <div class="col-9">
                                                <h5 class="card-title"><%= Resources.Prevodi.res_06 %></h5>
                                                <%-- Kje nas najdete? --%>
                                                <p class="card-text mb-0"><%= Resources.Prevodi.res_07 %></p>
                                                <%-- Vrbje 80A, --%>
                                                <p class="card-text mt-0"><%= Resources.Prevodi.res_08 %></p>
                                                <%-- 3310 Žalec, Slovenija --%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-3 d-flex align-items-center justify-content-center">
                                                <i class="fa fa-phone" style="font-size: 50px; color: #007bff;"></i>
                                            </div>
                                            <div class="col-9">
                                                <h5 class="card-title"><%= Resources.Prevodi.res_09 %></h5>
                                                <%-- Telefon & FAX --%>
                                                <p class="card-text mb-0">tel: +386 (0)3 227 77 10 Željka</p>
                                                <p class="card-text mb-0">tel: +386 (0)3 227 77 00 Milena</p>
                                                <p class="card-text mb-0">tel: +386 (0)3 713 68 28 Boris</p>

                                                <p class="card-text mt-0">fax: +386 (0)3 7136830</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-3 d-flex align-items-center justify-content-center">
                                                <i class="fa fa-envelope-o" style="font-size: 50px; color: #007bff;"></i>
                                            </div>
                                            <div class="col-9">
                                                <h5 class="card-title"><%= Resources.Prevodi.res_10 %></h5>
                                                <%-- Elektronska pošta --%>
                                                <p class="card-text mb-0">zeljka.bikic@grafolit.si</p>
                                                <p class="card-text mb-0">milena.prec@grafolit.si</p>
                                                <p class="card-text mb-0">boris.galic@grafolit.si</p>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>


                </div>



                <!-- Warning - Modal -->
                <div id="warningModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="warningModalLabel" aria-hidden="true" data-keyboard="false">
                    <div class="modal-dialog modal-sm" role="document">

                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header text-center" style="background-color: tomato; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <div><i class="fa fa-exclamation-circle" style="font-size: 60px; color: white"></i></div>
                            </div>
                            <h4 class="modal-title text-center" id="myModalTitle" style="margin-top: 10px; font-weight: 600">Opozorilo!</h4>
                            <div class="modal-body text-center" id="modalBodyText">
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-default" data-dismiss="modal">Zapri</button>
                            </div>
                        </div>

                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
