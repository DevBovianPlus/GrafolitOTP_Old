<%@ Page Title="Administrator" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AdminOverview.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Admin.AdminOverview" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">
        function ShowWarningPopUp(title, message, icon, backgroundHeaderColor) {
            $('#resultModal').modal('show');
            $('#modalBodyText').empty();
            $('#modalBodyText').append(message);
            $('#myModalTitle').empty();
            $('#myModalTitle').append(title);

            $('#modalIcon').empty();
            $('#modalIcon').append(icon);

            $('#modalHeader').css('background-color', backgroundHeaderColor);
        }

        function EndCallback_clientCallbackPanelUserInput(s, e) {
            clientLoadingPanel.Hide();
            $('[data-toggle="popover"]').popover();

            if (s.cpResetZaporednaStevilka != "" && s.cpResetZaporednaStevilka !== undefined) {
                ShowWarningPopUp('Odlično!', s.cpResetZaporednaStevilka, '<i class="fa fa-check-square-o" style="font-size: 60px; color: white"></i>', '#47c9a2');

                delete (s.cpResetZaporednaStevilka);
            }
            else if (s.cpPrevzemiPotrjeneOdpoklice != "" && s.cpPrevzemiPotrjeneOdpoklice !== undefined) {
                ShowWarningPopUp('Odlično!', s.cpPrevzemiPotrjeneOdpoklice, '<i class="fa fa-check-square-o" style="font-size: 60px; color: white"></i>', '#47c9a2');

                delete (s.cpPrevzemiPotrjeneOdpoklice);
            }
            else if (s.cpPreveriPovprasevanjeZaOdpoklice != "" && s.cpPreveriPovprasevanjeZaOdpoklice !== undefined) {
                ShowWarningPopUp('Odlično!', s.cpPreveriPovprasevanjeZaOdpoklice, '<i class="fa fa-check-square-o" style="font-size: 60px; color: white"></i>', '#47c9a2');

                delete (s.cpPreveriPovprasevanjeZaOdpoklice);
            }
            else if (s.cpError != "" && s.cpError !== undefined) {

                ShowWarningPopUp('Opozorilo!', s.cpError, '<i class="fa fa-exclamation-circle" style="font-size: 60px; color: white"></i>', 'tomato');

                delete (s.cpError);
            }
        }

        $(document).ready(function () {
            $('[data-toggle="popover"]').popover();
        });

        function btnResetZaporednsStevilka_Click(s, e) {
            clientLoadingPanel.Show();
            clientCallbackPanelUserInput.PerformCallback('ResetZaporednaStevilka');
        }

        function btnPrevzemiOdpoklice_Click(s, e) {
            clientLoadingPanel.Show();
            clientCallbackPanelUserInput.PerformCallback('PrevzemiPotrjeneOdpoklice');
        }

        function btnPreveriPovprasevanjaZaOdpoklice_Click(s, e) {
            clientLoadingPanel.Show();
            clientCallbackPanelUserInput.PerformCallback('PreveriPovprasevanjeZaOdpoklice');
        }

        function btnOdpokliciBrezPonudb_Click(s, e) {
            clientLoadingPanel.Show();
            clientCallbackPanelUserInput.PerformCallback('OdpokliciBrezPonudb');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxLoadingPanel ID="LoadingPanel" ClientInstanceName="clientLoadingPanel" runat="server" Modal="true" Width="100px" Text="Nalagam">
    </dx:ASPxLoadingPanel>

    <dx:ASPxCallbackPanel ID="CallbackPanelUserInput" runat="server" Width="100%" ClientInstanceName="clientCallbackPanelUserInput"
        OnCallback="CallbackPanelUserInput_Callback">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="EndCallback_clientCallbackPanelUserInput" />
        <PanelCollection>
            <dx:PanelContent>

                <div class="panel panel-default" style="margin-top: 10px;">
                    <div class="panel-heading">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-6 no-padding-left">
                                <h4 class="panel-title">Odpoklici</h4>
                            </div>

                            <div class="col-xs-6 no-padding-right">
                                <div class="row2 align-item-centerV-endH">
                                    <div class="col-xs-0">
                                        <a data-toggle="collapse" style="display: inline-block;" href="#recalls"></a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="recalls" class="panel-collapse collapse in">
                        <div class="panel-body">
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-sm-3 no-padding-left">
                                    <div>
                                        <h5 class="no-margin"><em>Odpoklici pozicija</em></h5>
                                    </div>
                                    <div class="panel panel-default" style="margin-top: 2px;">
                                        <div class="panel-body">
                                            <div style="display: inline-block;" title="Restiraj zaporedno številko" data-toggle="popover" data-trigger="hover" data-content="Restiraj poje ZaporednaStevilka v tabeli OdpoklicPozicija">
                                                <dx:ASPxButton ID="btnResetZaporednsStevilka" runat="server" Text="Resetiraj polje" Width="100" Theme="Moderno"
                                                    AutoPostBack="false" UseSubmitBehavior="false">
                                                    <ClientSideEvents Click="btnResetZaporednsStevilka_Click" />
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-3 no-padding-left">
                                    <div>
                                        <h5 class="no-margin"><em>Prevzemanje odpoklicev</em></h5>
                                    </div>
                                    <div class="panel panel-default" style="margin-top: 2px;">
                                        <div class="panel-body">
                                            <div style="display: inline-block;" title="Zaključi odpoklice" data-toggle="popover" data-trigger="hover" data-content="Zaključi potrjene odpoklice ki so bili prevzeti">
                                                <dx:ASPxButton ID="btnPrevzemiOdpoklice" runat="server" Text="Prevzemi odpoklice" Width="100" Theme="Moderno"
                                                    AutoPostBack="false" UseSubmitBehavior="false">
                                                    <ClientSideEvents Click="btnPrevzemiOdpoklice_Click" />
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-3 no-padding-left">
                                    <div>
                                        <h5 class="no-margin"><em>Povpraševanja za prevoznike - odpoklici</em></h5>
                                    </div>
                                    <div class="panel panel-default" style="margin-top: 2px;">
                                        <div class="panel-body">
                                            <div style="display: inline-block;" title="Potrdi odpoklice" data-toggle="popover" data-trigger="hover" data-content="Potrdi odpoklice ki imajo najugodnejšega prevoznika glede na prijavljeno ceno.">
                                                <dx:ASPxButton ID="btnPreveriPovprasevanjaZaOdpoklice" runat="server" Text="Preveri povpraševanja za odpoklice" Width="100" Theme="Moderno"
                                                    AutoPostBack="false" UseSubmitBehavior="false">
                                                    <ClientSideEvents Click="btnPreveriPovprasevanjaZaOdpoklice_Click" />
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-3 no-padding-left">
                                    <div>
                                        <h5 class="no-margin"><em>Odpoklici kjer se prevozniki niso prijavili</em></h5>
                                    </div>
                                    <div class="panel panel-default" style="margin-top: 2px;">
                                        <div class="panel-body">
                                            <div style="display: inline-block;" title="Poplji mail logistiki" data-toggle="popover" data-trigger="hover" data-content="Preveri in pošlji mail logistiki kjer ni bilo nobenih ponudb za prevoz.">
                                                <dx:ASPxButton ID="btnOdpokliciBrezPonudb" runat="server" Text="Preveri odpoklice brez ponudb" Width="100" Theme="Moderno"
                                                    AutoPostBack="false" UseSubmitBehavior="false">
                                                    <ClientSideEvents Click="btnOdpokliciBrezPonudb_Click" />
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-sm-3 no-padding-left">
                                    <div>
                                        <h5 class="no-margin"><em>XML Generate Naročilnica odpoklica - Prevoznik</em></h5>
                                    </div>
                                    <div class="panel panel-default" style="margin-top: 2px;">
                                        <div class="panel-body">
                                            <div style="display: inline-block;" title="Generiraj XML - prevoznik" data-toggle="popover" data-trigger="hover" data-content="Generiraj XML za potrjenega prevoznika">
                                                <dx:ASPxButton ID="btnbtnGenerateXMLPrevoznik" runat="server" Text="Generiraj XML - prevoznik" Width="100" Theme="Moderno"
                                                    AutoPostBack="false" UseSubmitBehavior="false" OnClick="btnbtnGenerateXMLPrevoznik_Click">
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-3 no-padding-left">
                                    <div>
                                        <h5 class="no-margin"><em>Launch create PDF Pantheon</em></h5>
                                    </div>
                                    <div class="panel panel-default" style="margin-top: 2px;">
                                        <div class="panel-body">
                                            <div style="display: inline-block;" title="Zaključi odpoklice" data-toggle="popover" data-trigger="hover" data-content="Zaženi pantheon program za kreiranje PDF">
                                                <dx:ASPxButton ID="btnLaunchPDFPantheon" runat="server" Text="PDF Pantheon" Width="100" Theme="Moderno"
                                                    AutoPostBack="false" UseSubmitBehavior="false" OnClick="btnLaunchPDFPantheon_Click">
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-3 no-padding-left">
                                    <div>
                                        <h5 class="no-margin"><em>Preveri če obstaja PDF</em></h5>
                                    </div>
                                    <div class="panel panel-default" style="margin-top: 2px;">
                                        <div class="panel-body">
                                            <div style="display: inline-block;" title="Preveri PDF" data-toggle="popover" data-trigger="hover" data-content="Preveri ali obstaja PDF">
                                                <dx:ASPxTextBox runat="server" ID="txtPath" CssClass="text-box-input" Font-Size="14px" Width="100%" Font-Bold="true">
                                                </dx:ASPxTextBox>
                                                <dx:ASPxButton ID="btnCheckPDFExist" runat="server" Text="Preveri ali obstaja PDF" Width="100" Theme="Moderno"
                                                    AutoPostBack="false" UseSubmitBehavior="false" OnClick="btnCheckPDFExist_Click">                                                    
                                                </dx:ASPxButton>
                                                <dx:ASPxLabel ID="lblRezultat" runat="server" Font-Bold="true" Text=""></dx:ASPxLabel>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-3 no-padding-left">
                                    <div>
                                        <h5 class="no-margin"><em>Klic Utility funkcije - Check for Order2</em></h5>
                                    </div>
                                    <div class="panel panel-default" style="margin-top: 2px;">
                                        <div class="panel-body">
                                            <div style="display: inline-block;" title="Poplji mail logistiki" data-toggle="popover" data-trigger="hover" data-content="Preveri ali obstajajo odpoklici ki niso prevzeti">
                                                <dx:ASPxButton ID="btnNoRecall" runat="server" Text="Še ne prevzeti odpoklici" Width="100" Theme="Moderno"
                                                    AutoPostBack="false" UseSubmitBehavior="false" OnClick="btnNoRecall_Click">                                                    
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-sm-3 no-padding-left">
                                    <div>
                                        <h5 class="no-margin"><em>Avtomatska izbira prevoznika</em></h5>
                                    </div>
                                    <div class="panel panel-default" style="margin-top: 2px;">
                                        <div class="panel-body">
                                            <div style="display: inline-block;" title="Avtomatska izbira prevoznika" data-toggle="popover" data-trigger="hover" data-content="Avtomatska izbira prevoznika">
                                                <dx:ASPxButton ID="btnAvtomatskaIzbira" runat="server" Text="Avtomatska izbira" Width="100" Theme="Moderno"
                                                    AutoPostBack="false" UseSubmitBehavior="false" OnClick="btnAvtomatskaIzbira_Click">
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-3 no-padding-left">
                                    <div>
                                        <h5 class="no-margin"><em>Avtomatska izbira prevoznika 2</em></h5>
                                    </div>
                                    <div class="panel panel-default" style="margin-top: 2px;">
                                        <div class="panel-body">
                                            <div style="display: inline-block;" title="Zaključi odpoklice" data-toggle="popover" data-trigger="hover" data-content="Avtomatska izbira prevoznika">
                                                <dx:ASPxButton ID="btnAvtomatskaIzbira2" runat="server" Text="PDF Pantheon" Width="100" Theme="Moderno"
                                                    AutoPostBack="false" UseSubmitBehavior="false" OnClick="btnAvtomatskaIzbira2_Click">
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-3 no-padding-left">
                                    <div>
                                        <h5 class="no-margin"><em>Preveri vse odpoklice ali so kreirane in poslane naročilnice</em></h5>
                                    </div>
                                    <div class="panel panel-default" style="margin-top: 2px;">
                                        <div class="panel-body">
                                            <div style="display: inline-block;" title="Preveri PDF" data-toggle="popover" data-trigger="hover" data-content="Izdalaj in pošlji PDF naročilnico prevozniku">
                                                <dx:ASPxTextBox runat="server" ID="ASPxTextBox1" CssClass="text-box-input" Font-Size="14px" Width="100%" Font-Bold="true">
                                                </dx:ASPxTextBox>
                                                <dx:ASPxButton ID="btnCreadeAndSendOrders" runat="server" Text="Izdela in pošlji" Width="100" Theme="Moderno"
                                                    AutoPostBack="false" UseSubmitBehavior="false" OnClick="btnCreadeAndSendOrders_Click">                                                    
                                                </dx:ASPxButton>
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Bold="true" Text=""></dx:ASPxLabel>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-3 no-padding-left">
                                    <div>
                                        <h5 class="no-margin"><em>Klic Utility funkcije - CreatePDFAndSendPDOOrdersMultiple</em></h5>
                                    </div>
                                    <div class="panel panel-default" style="margin-top: 2px;">
                                        <div class="panel-body">
                                            <div style="display: inline-block;" title="Poplji mail logistiki" data-toggle="popover" data-trigger="hover" data-content="Preveri ali obstajajo odpoklici ki niso prevzeti">
                                                <dx:ASPxButton ID="ASPxButton4" runat="server" Text="Še ne prevzeti odpoklici" Width="100" Theme="Moderno"
                                                    AutoPostBack="false" UseSubmitBehavior="false" OnClick="btnCreatePDFAndSendPDOOrdersMultiple_Click">                                                    
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="row2 align-item-centerV-startH">
                                <div class="col-sm-3 no-padding-left">
                                    <div>
                                        <h5 class="no-margin"><em>Log datoteke</em></h5>
                                    </div>
                                    <div class="panel panel-default" style="margin-top: 2px;">
                                        <div class="panel-body">
                                            <div style="display: inline-block;" title="Prenos log datotek iz web service-a in aplikacije" data-toggle="popover" data-trigger="hover" data-content="Preveri ali obstajajo odpoklici ki niso prevzeti">
                                                <dx:ASPxButton ID="btnGetLogs" runat="server" Text="Prenesi log datoteke" Width="100" Theme="Moderno"
                                                    AutoPostBack="false" UseSubmitBehavior="false" OnClick="btnGetLogs_Click">                                                    
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-3 no-padding-left">
                                <div>
                                    <h5 class="no-margin"><em>Change DB config</em></h5>
                                </div>
                                <div class="panel panel-default" style="margin-top: 2px;">
                                    <div class="panel-body">
                                        <div style="display: inline-block;" title="Change DB config" data-toggle="popover" data-trigger="hover" data-content="Change DB config">
                                            <dx:ASPxTextBox runat="server" ID="txtConfigName" CssClass="text-box-input" Font-Size="14px" Width="100%" Font-Bold="true">
                                                </dx:ASPxTextBox>
                                            <dx:ASPxTextBox runat="server" ID="txtConfigValue" CssClass="text-box-input" Font-Size="14px" Width="100%" Font-Bold="true">
                                                </dx:ASPxTextBox>
                                            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Zamenjaj vrednost" Width="100" Theme="Moderno" OnClick="btnChangeConfig_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false">                                                
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                            </div>

                                <div class="col-sm-3 no-padding-left">
                                <div>
                                    <h5 class="no-margin"><em>Get config Value by name</em></h5>
                                </div>
                                <div class="panel panel-default" style="margin-top: 2px;">
                                    <div class="panel-body">
                                        <div style="display: inline-block;" title="Change DB config" data-toggle="popover" data-trigger="hover" data-content="Change DB config">
                                            <dx:ASPxTextBox runat="server" ID="txtConfigNameRet" CssClass="text-box-input" Font-Size="14px" Width="100%" Font-Bold="true">
                                                </dx:ASPxTextBox>
                                           
                                            <dx:ASPxButton ID="btnGetConfigValue" runat="server" Text="Dobi vrednost" Width="100" Theme="Moderno" OnClick="btnGetConfigVal_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false">                                                
                                            </dx:ASPxButton>

                                            <dx:ASPxLabel ID="lblRezultat2" runat="server" Font-Bold="true" Text=""></dx:ASPxLabel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            </div>


                        </div>
                    </div>
                </div>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

    <!-- Result - Modal -->
    <div id="resultModal" class="modal fade" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div id="modalHeader" class="modal-header text-center" style="background-color: tomato; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div id="modalIcon"></div>
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
</asp:Content>
