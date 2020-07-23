<%@ Page Title="Postavke naročilnic PDO, NOZ" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="OrderOstalo.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Recall.OrderOstalo" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script>
        function OnSelectionChanged_gridOrdersPositions(s, e) {
            if (s.GetSelectedRowCount() > 0)
                clientBtnConfirm.SetEnabled(true);
            else
                clientBtnConfirm.SetEnabled(false);
            s.GetSelectedFieldValues("TipAplikacije;OdpoklicKolicinaOTP", GetSelectedFieldValuesCallback);
        }

        function GetSelectedFieldValuesCallback(values) {
            var cntKolicina = 0;
            var cntSkupaj = 0;
            var sumSkupajNOZPDO = 0;
            var sumZaOdpoklic = 0;

            for (var i = 0; i < values.length; i++) {
                cntKolicina += values[i][1];
            }
            
            sumSkupajNOZPDO = clientHfSumPDONOZ.Get("SumPDONOZ");
            sumZaOdpoklic = sumSkupajNOZPDO + cntKolicina;     

            cntKolicina = "Ostale naročilnice: " + cntKolicina + " kg";
                 

            clientOstaleNarocilnice.SetText(cntKolicina);
            clientNarocilniceOstaleZaOdpoklic.SetText(cntKolicina);

            clientSkupajZaOdpoklic.SetText("Za odpoklic: " + sumZaOdpoklic + " kg");
        }

        function Dobavitelj_ValueChanged(s, e) {
            clientCallbackPanelRefreshSupplier.PerformCallback("SupplierChanged");
        }

        function ASPxGridViewOrdersPositions_EndCallBack(s, e) {
            //alert("Spremeni");
            s.GetSelectedFieldValues("TipAplikacije;OdpoklicKolicinaOTP", GetSelectedFieldValuesCallback);
        }

        var isBtnConfirmIntiated = false;
        function btnConfirm_Click(s, e) {
            e.processOnServer = !isBtnConfirmIntiated;
            isBtnConfirmIntiated = true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <style type="text/css">
        .newFont, .newFont .dxeEditAreaSys, .newFont .dxeListBoxItem {
            font-size: 12px !important;
        }
    </style>
    <div class="row small-padding-bottom">
        <div class="col-md-4">
            <div class="row2" style="align-items: center;">
                <div class="col-xs-4 no-padding-right">
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="DOBAVITELJ : "></dx:ASPxLabel>
                </div>
                <div class="col-xs-8 no-padding-left">
                    <dx:ASPxTextBox runat="server" ID="txtDobaviteljNaziv" ClientEnabled="false" ClientInstanceName="clientTxtStOdpoklic"
                        CssClass="text-box-input" Font-Size="14px" ReadOnly="true" BackColor="LightGray" Width="100%" Font-Bold="true">
                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                    </dx:ASPxTextBox>
                </div>
            </div>
        </div>
    </div>
    <dx:ASPxCallbackPanel runat="server" ID="cbpRefreshsUpplier" ClientInstanceName="clientCallbackPanelRefreshSupplier">
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxHiddenField runat="server" ClientInstanceName="clientHfSumPDONOZ" ID="hfCurrentSumPDONOZ">
                </dx:ASPxHiddenField>

                <dx:ASPxGridView ID="ASPxGridViewOrdersPositions" runat="server" ClientInstanceName="gridOrdersPositions"
                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewOrdersPositions_DataBinding"
                    KeyFieldName="tempID" CssClass="newFont"
                    OnHtmlRowPrepared="ASPxGridViewOrdersPositions_HtmlRowPrepared" OnBatchUpdate="ASPxGridViewOrdersPositions_BatchUpdate"
                    OnCommandButtonInitialize="ASPxGridViewOrdersPositions_CommandButtonInitialize">
                    <ClientSideEvents SelectionChanged="OnSelectionChanged_gridOrdersPositions" EndCallback="ASPxGridViewOrdersPositions_EndCallBack" />
                    <Paddings Padding="0" />
                    <Settings ShowVerticalScrollBar="True" ShowFooter="true" ShowStatusBar="Visible"
                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="300"
                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" />
                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                        <PageSizeItemSettings Visible="true" Items="50,80,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                        </PageSizeItemSettings>
                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                    </SettingsPager>
                    <SettingsBehavior AllowFocusedRow="true" />
                    <Styles Header-Wrap="True">
                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                    </Styles>
                    <SettingsText EmptyDataRow="Trenutno ni podatka o odpoklicih. Dodaj novega."
                        CommandBatchEditUpdate="Spremeni" CommandBatchEditCancel="Prekliči" />
                    <SettingsEditing Mode="Batch" BatchEditSettings-StartEditAction="DblClick" BatchEditSettings-KeepChangesOnCallbacks="False" />
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="80px" SelectAllCheckboxMode="None" Caption="Izberi" ShowClearFilterButton="true" />
                        <dx:GridViewDataTextColumn Caption="Apl" FieldName="TipAplikacije" Width="70px"
                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Naročilnica" FieldName="Narocilnica" Width="160px"
                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>



                        <dx:GridViewDataTextColumn Caption="Št. pozicije" FieldName="St_Pozicija" Width="110px" Visible="false"
                            ReadOnly="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Potrditev naročila" FieldName="Order_Confirm" Width="100px"
                            ReadOnly="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                        <%--<dx:GridViewDataTextColumn Caption="Tovarna" FieldName="Tovarna" Width="15%"
                ReadOnly="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>--%>

                        <dx:GridViewDataTextColumn Caption="Artikel"
                            FieldName="Artikel" ShowInCustomizationForm="True"
                            Width="400px">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataDateColumn FieldName="Datum_narocila" Caption="Datum naročila" ShowInCustomizationForm="True" Width="100px">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                            </PropertiesDateEdit>
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataDateColumn>

                        <dx:GridViewDataDateColumn FieldName="Datum_Dobave" Caption="Datum dobave" ShowInCustomizationForm="True" Width="100px">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                            </PropertiesDateEdit>
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataDateColumn>

                        <%--<dx:GridViewDataTextColumn Caption="Dobavitelj" FieldName="Dobavitelj" Width="15%"
                ReadOnly="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>--%>

                        <%-- <dx:GridViewDataTextColumn Caption="Kupec" FieldName="Kupec" Width="15%"
                ReadOnly="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>--%>

                        <dx:GridViewDataTextColumn Caption="Naročeno" FieldName="Naroceno" Width="100px"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <PropertiesTextEdit DisplayFormatString="N3" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Odpoklicana količina" FieldName="OdpoklicKolicinaOTP" Width="120px"
                            ShowInCustomizationForm="True">

                            <PropertiesTextEdit DisplayFormatString="n2" DisplayFormatInEditMode="true">
                                <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                            </PropertiesTextEdit>
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />

                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Prevzeto" FieldName="Prevzeto" Width="120px"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <PropertiesTextEdit DisplayFormatString="N3" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Razlika" FieldName="Razlika" Width="120px"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <PropertiesTextEdit DisplayFormatString="N3" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Zaloga" FieldName="Zaloga" Width="120px"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <PropertiesTextEdit DisplayFormatString="N3" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Optimalna zaloga" FieldName="Dovoljeno_Odpoklicati" Width="120px"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <PropertiesTextEdit DisplayFormatString="N0" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Tip" FieldName="Tip" Width="300px"
                            ReadOnly="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Interno" FieldName="Interno" Width="120px"
                            ReadOnly="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Proizvedeno" FieldName="Proizvedeno" Width="120px"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <PropertiesTextEdit DisplayFormatString="N3" />
                        </dx:GridViewDataTextColumn>

                    </Columns>
                    <%--<SettingsResizing ColumnResizeMode="NextColumn" Visualization="Live" />--%>
                </dx:ASPxGridView>
                <dx:ASPxLabel runat="server" Text="Seznam potrjenih naročilnic:"></dx:ASPxLabel>
                <dx:ASPxGridView ID="ASPxGridViewOrder10Positions" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridOrders10Positions"
                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewOrder10Positions_DataBinding"
                    KeyFieldName="tempID" CssClass="newFont" OnHtmlRowPrepared="ASPxGridViewOrder10Positions_HtmlRowPrepared">
                    <Paddings Padding="0" />
                    <Settings ShowVerticalScrollBar="True"
                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="160"
                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" />
                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                        <PageSizeItemSettings Visible="true" Items="50,80,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                        </PageSizeItemSettings>
                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                    </SettingsPager>
                    <SettingsBehavior AllowFocusedRow="true" />
                    <Styles Header-Wrap="True">
                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                    </Styles>
                    <SettingsText EmptyDataRow="Trenutno ni podatka o naročilih. Dodaj novega." />
                    <%--<SettingsEditing Mode="Batch" BatchEditSettings-StartEditAction="DblClick"  />--%>
                    <Columns>
                        <dx:GridViewDataDateColumn FieldName="ZeljeniRokDobave" Caption="Datum naročila" ShowInCustomizationForm="True" Width="8%">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                            </PropertiesDateEdit>
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataDateColumn>



                        <dx:GridViewDataTextColumn Caption="Št. dokumenta" FieldName="Narocilnica" Width="10%"
                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Stranka" FieldName="Kupec" Width="15%"
                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Ident" FieldName="Ident" Width="8%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Artikel" FieldName="Artikel" ShowInCustomizationForm="True" Width="20%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Količina" FieldName="Naroceno" Width="10%"
                            ShowInCustomizationForm="True">
                            <PropertiesTextEdit DisplayFormatString="N3" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="EM" FieldName="EnotaMere" Width="10%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>


                        <dx:GridViewDataDateColumn FieldName="PotrjeniRokDobave" Caption="Potrjeni rok dobave" ShowInCustomizationForm="True" Width="10%">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                            </PropertiesDateEdit>
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataDateColumn>

                        <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" Width="5%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Dobavitelj" FieldName="Dobavitelj" Width="15%"
                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                    </Columns>
                    <SettingsResizing ColumnResizeMode="NextColumn" Visualization="Live" />
                </dx:ASPxGridView>
                <div class="row">
                    <div class="col-md-2">
                        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" ClientInstanceName="roundPanel" HeaderText="PDO, NOZ naročilnice" runat="server" Width="270" View="GroupBox">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <dx:ASPxLabel Text="PDO Naračilnice" ID="lblPDO" runat="server" ClientInstanceName="clientNarocilnicePDO"></dx:ASPxLabel>
                                    <br />
                                    <dx:ASPxLabel Text="NOZ Naračilnice" ID="lblNoz" runat="server" ClientInstanceName="clientNarocilniceNOZ"></dx:ASPxLabel>
                                    <hr style="width: 100%; padding-bottom: 0px" />
                                    <dx:ASPxLabel Font-Bold="true" Text="Skupaj" ID="lblSUM" runat="server" ClientInstanceName="clientSkupaj"></dx:ASPxLabel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                    </div>
                    <div class="col-md-2">
                        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" ClientInstanceName="roundPanel" HeaderText="Ostale naročilnice" runat="server" Width="270" View="GroupBox">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <dx:ASPxLabel Text="Izbrane Naročilnice" ID="lblOstaleNarocilnice" runat="server" ClientInstanceName="clientOstaleNarocilnice"></dx:ASPxLabel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </div>
                    <div class="col-md-8">
                        <div class="row2 align-item-centerV-endH">
                            <div class="col-md-3">
                                <dx:ASPxRoundPanel ID="ASPxRoundPanel3" ClientInstanceName="roundPanel" HeaderText="Ostale naročilnice" runat="server" Width="270" View="GroupBox">
                                    <PanelCollection>
                                        <dx:PanelContent>
                                            <dx:ASPxLabel Text="PDO + NOZ Naročilnice:" ID="lblNOZPDOOStaloZaOdpoklic" runat="server" ClientInstanceName="clientNarocilnicePDONOZOstaloZaOdpoklic"></dx:ASPxLabel>
                                            <br />
                                            <dx:ASPxLabel Text="Ostale naročilnice" ID="lblOstaleNarocilniceZaOdpoklic" runat="server" ClientInstanceName="clientNarocilniceOstaleZaOdpoklic"></dx:ASPxLabel>
                                            <hr style="width: 100%; padding-bottom: 0px" />
                                            <dx:ASPxLabel Font-Bold="true" Text="Za odpoklic" ID="lblSkupajZaOdpoklic" runat="server" ClientInstanceName="clientSkupajZaOdpoklic"></dx:ASPxLabel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxRoundPanel>
                            </div>
                            <div class="col-md-2 text-right" style="display: block;">
                                <dx:ASPxButton Theme="Moderno" ID="btnBack" runat="server" Text="Nazaj" AutoPostBack="false" OnClick="btnConfirm_Click"
                                    Height="25" Width="120" ClientEnabled="false" ClientInstanceName="clientBtnBack">
                                    <Paddings PaddingLeft="10" PaddingRight="10" />
                                    <Image Url="../../../Images/back.png" UrlHottracked="../../Images/backHoover.png" />
                                    <ClientSideEvents Click="btnConfirm_Click" />
                                </dx:ASPxButton>
                            </div>
                            <div class="col-md-2">
                                <dx:ASPxButton Theme="Moderno" ID="btnConfirm" runat="server" Text="Naprej" AutoPostBack="false" OnClick="btnConfirm_Click"
                                    Height="25" Width="120" ClientEnabled="false" ClientInstanceName="clientBtnConfirm">
                                    <Paddings PaddingLeft="10" PaddingRight="10" />
                                    <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                                    <ClientSideEvents Click="btnConfirm_Click" />
                                </dx:ASPxButton>
                            </div>
                        </div>
                    </div>
                </div>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>


</asp:Content>
