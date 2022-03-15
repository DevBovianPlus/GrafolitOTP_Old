<%@ Page Title="Postavke naročilnic PDO, NOZ" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="OrderNOZPDO.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Recall.OrderNOZPDO" %>

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
            var cntNOZKolicina = 0;
            var cntPDOKolicina = 0;
            var cntOstaleKolicina = 0;
            var cntSkupaj = 0;

            for (var i = 0; i < values.length; i++) {
                if (values[i][0] == 'PDO') {
                    cntPDOKolicina += values[i][1];
                }

                if (values[i][0] == 'NOZ')
                {
                    cntNOZKolicina += values[i][1];
                }
                if (values[i][0] == '')
                {
                    cntOstaleKolicina += values[i][1];
                }

            }
            cntSkupaj = cntPDOKolicina + cntNOZKolicina + cntOstaleKolicina;
            cntSkupaj = "SKUPAJ za odpoklic:" + parseFloat(cntSkupaj).toFixed(2) + " kg";

            clientHfSumNOZ.Set("CurrenSumNOZ", cntNOZKolicina);
            clientHfSumPDO.Set("CurrenSumPDO", cntPDOKolicina);

            cntNOZKolicina = "Naročilnice NOZ: " + parseFloat(cntNOZKolicina).toFixed(2)  + " kg";
            cntPDOKolicina = "Naročilnice PDO: " + parseFloat(cntPDOKolicina).toFixed(2)  + " kg";
            cntOstaleKolicina = "Naročilnice ostale: " + parseFloat(cntOstaleKolicina).toFixed(2)  + " kg";



            clientNarocilniceNOZ.SetText(cntNOZKolicina);
            clientNarocilnicePDO.SetText(cntPDOKolicina);
            clientNarocilniceOstale.SetText(cntOstaleKolicina);
            clientSkupaj.SetText(cntSkupaj);

        }

        function Dobavitelj_ValueChanged(s, e) {
            LoadingPanel.Show();
            clientCallbackPanelRefreshSupplier.PerformCallback("SupplierChanged");
        }


        function ASPxGridViewOrdersPositions_EndCallBack(s, e) {
            //alert("Spremeni");
            s.GetSelectedFieldValues("TipAplikacije;OdpoklicKolicinaOTP", GetSelectedFieldValuesCallback);
        }

        var isBtnConfirmIntiated = false;
        function btnConfirm_Click(s, e) {
            //e.processOnServer = !isBtnConfirmIntiated;
            //isBtnConfirmIntiated = true;
            LoadingPanel.Show();
            clientBtnConfirm.SetEnabled(false);
            clientCallbackPanelRefreshSupplier.PerformCallback("ClickNaprej");
        }

        function CloseGridLookup(s, e) {
            lookUpCategory.ConfirmCurrentSelection();
            lookUpCategory.HideDropDown();
            lookUpCategory.Focus();
        }

        function cbpRefreshsUpplier_EndCallback(s, e) {
            LoadingPanel.Hide();
        }

        function lookUpCategory_Valuechanged(s, e) {
            LoadingPanel.Show();
            clientCallbackPanelRefreshSupplier.PerformCallback('CategoryChanged');
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
        <div class="col-md-6">
            <div class="row2" style="align-items: center;">
                <div class="col-md-0">
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="DOBAVITELJ : "></dx:ASPxLabel>
                </div>
                <div class="col-md-3">
                    <dx:ASPxGridLookup ID="ASPxGridLookupDobavitelj" runat="server" ClientInstanceName="lookUpDobavitelj"
                        KeyFieldName="Dobavitelj" TextFormatString="{0}" CssClass="text-box-input"
                        Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                        OnLoad="ASPxGridLookupLoad_WidthSmall" OnDataBinding="ASPxGridLookupDobavitelj_DataBinding" IncrementalFilteringMode="Contains">
                        <ClearButton DisplayMode="OnHover" />
                        <ClientSideEvents ValueChanged="ValueChanged_lookUpStranke" />
                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                        <GridViewStyles>
                            <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                            <FilterBarClearButtonCell></FilterBarClearButtonCell>
                        </GridViewStyles>
                        <GridViewProperties>
                            <SettingsBehavior EnableRowHotTrack="True" />
                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                            <SettingsPager ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                        </GridViewProperties>
                        <Columns>

                            <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Dobavitelj" Width="100%"
                                ReadOnly="true" ShowInCustomizationForm="True">
                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            </dx:GridViewDataTextColumn>

                        </Columns>
                        <ClientSideEvents ValueChanged="Dobavitelj_ValueChanged" />
                    </dx:ASPxGridLookup>
                </div>
                <div class="col-md-0">
                    <dx:ASPxLabel ID="ASPxLabel19" runat="server" Font-Size="12px" Text="KATEGORIJA : "></dx:ASPxLabel>
                </div>
                <div class="col-md-3">
                    <dx:ASPxGridLookup ID="GridLookupCategory" runat="server" ClientInstanceName="lookUpCategory"
                        KeyFieldName="TempID" TextFormatString="{0}" CssClass="text-box-input"
                        Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                        OnLoad="ASPxGridLookupLoad_WidthSmall" OnDataBinding="GridLookupCategory_DataBinding" IncrementalFilteringMode="Contains"
                        SelectionMode="Multiple" MultiTextSeparator=", ">
                        <ClientSideEvents ValueChanged="lookUpCategory_Valuechanged" />
                        <ClearButton DisplayMode="OnHover" />
                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                        <GridViewStyles>
                            <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                        </GridViewStyles>
                        <GridViewProperties>
                            <SettingsBehavior EnableRowHotTrack="True" AllowEllipsisInText="true" AllowDragDrop="false" />
                            <SettingsPager ShowSeparators="True" NumericButtonCount="3" EnableAdaptivity="true" />
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowVerticalScrollBar="True"
                                ShowHorizontalScrollBar="true" VerticalScrollableHeight="200" ShowStatusBar="Visible"></Settings>
                        </GridViewProperties>
                        <SettingsAdaptivity Mode="OnWindowInnerWidth" ModalDropDownCaption="Kategorija" SwitchToModalAtWindowInnerWidth="650" />
                        <Columns>
                            <dx:GridViewCommandColumn ShowSelectCheckbox="True" />
                            <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv"
                                ReadOnly="true" MinWidth="230" MaxWidth="400" Width="100%">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <GridViewProperties>
                            <Templates>
                                <StatusBar>
                                    <table class="OptionsTable" style="float: right">
                                        <tr>
                                            <td>
                                                <dx:ASPxButton ID="btnConfirmAndClose" runat="server" AutoPostBack="false" Text="Potrdi in zapri" ClientSideEvents-Click="CloseGridLookup" />
                                            </td>
                                        </tr>
                                    </table>
                                </StatusBar>
                            </Templates>
                            <Settings ShowFilterRow="True" ShowStatusBar="Visible" />
                            <SettingsPager PageSize="7" EnableAdaptivity="true" />
                        </GridViewProperties>
                    </dx:ASPxGridLookup>
                </div>
            </div>
        </div>
    </div>
    <dx:ASPxCallbackPanel runat="server" ID="cbpRefreshsUpplier" ClientInstanceName="clientCallbackPanelRefreshSupplier" OnCallback="cbpRefreshsUpplier_Callback">
        <ClientSideEvents EndCallback="cbpRefreshsUpplier_EndCallback" />
        <SettingsLoadingPanel  Enabled="false"/>
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxHiddenField runat="server" ClientInstanceName="clientHfSumPDO" ID="hfCurrentSumPDO">
                </dx:ASPxHiddenField>
                <dx:ASPxHiddenField runat="server" ClientInstanceName="clientHfSumNOZ" ID="hfCurrentSumNOZ">
                </dx:ASPxHiddenField>
                <dx:ASPxGridView ID="ASPxGridViewOrdersPositions" runat="server" ClientInstanceName="gridOrdersPositions"
                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewOrdersPositions_DataBinding"
                    KeyFieldName="tempID" CssClass="newFont"
                    OnHtmlRowPrepared="ASPxGridViewOrdersPositions_HtmlRowPrepared" OnBatchUpdate="ASPxGridViewOrdersPositions_BatchUpdate"
                    OnCommandButtonInitialize="ASPxGridViewOrdersPositions_CommandButtonInitialize">
                    <ClientSideEvents SelectionChanged="OnSelectionChanged_gridOrdersPositions" EndCallback="ASPxGridViewOrdersPositions_EndCallBack" />
                    <Paddings Padding="0" />
                    <Settings ShowVerticalScrollBar="True" ShowFooter="true" ShowStatusBar="Visible"
                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="250"
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
                        <dx:GridViewDataTextColumn Caption="Status" FieldName="StatusPozicije" Width="70px"
                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>
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

                       <dx:GridViewDataTextColumn Caption="Kategorija" FieldName="Kategorija" Width="100px"
                            ReadOnly="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Ident" FieldName="Ident" Width="100px"
                            ReadOnly="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

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

                        <dx:GridViewDataTextColumn Caption="Kategorija" FieldName="Kategorija" Width="10%"
                            ReadOnly="true" ShowInCustomizationForm="True">
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
                                    <br />
                                    <dx:ASPxLabel Text="Ostale naročilnice" ID="lblOstale" runat="server" ClientInstanceName="clientNarocilniceOstale"></dx:ASPxLabel>
                                    <hr style="width: 100%; padding-bottom: 0px" />
                                    <dx:ASPxLabel Font-Bold="true" Text="Skupaj" ID="lblSUM" runat="server" ClientInstanceName="clientSkupaj"></dx:ASPxLabel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </div>
                    <div class="col-md-2">
                        <span class="AddEditButtons">
                            <dx:ASPxButton Theme="Moderno" ID="btnConfirm" runat="server" Text="Naprej" AutoPostBack="false"
                                Height="25" Width="90" ClientEnabled="false" ClientInstanceName="clientBtnConfirm">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../../../Images/forward.png" UrlHottracked="../../Images/forwardHoover.png" />
                                <ClientSideEvents Click="btnConfirm_Click" />
                            </dx:ASPxButton>
                        </span>
                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>


</asp:Content>
