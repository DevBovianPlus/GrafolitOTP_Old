<%@ Page Title="Odpoklici kupcev" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RecallBuyerCreate.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Recall.RecallBuyerCreate" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            $("#modal-btn-delete").on("click", function () {
                $("#warningButtonModal").modal('hide');
                firstShow = false;
                clientLoadingPanel.Show();
                clientCallbackPanelUserInput.PerformCallback('DeleteSelectedPosition');
            });

        });

        var postbackInitiated = false;
        function CheckFieldValidation(s, e) {
            var process = false;
            var lookUpItems = [lookUpRelacija];
            //var comboBoxItems = [clientComboBoxTip];
            var inputItems = null;
            var memoItems = null;


            process = InputFieldsValidation(lookUpItems, inputItems, null, memoItems, /*comboBoxItems*/null, null);

            if (clientBtnConfirm.GetText() == 'Izbriši')
                process = true;

            if (process) {
                e.processOnServer = !postbackInitiated;
                postbackInitiated = true;
            }
            else
                e.processOnServer = false;

            return process;
        }

        function btnAddOrderPosition_Click(s, e) {
            clientLoadingPanel.Show();
            clientCallbackPanelUserInput.PerformCallback("ShowOrderPositionPopUp");
        }

        function ValueChanged_lookUpRelacija(s, e) {
            clientCallbackPanelUserInput.PerformCallback('SelectRelacija');
        }

        function ValueChanged_lookUpZbirnikTon(s, e) {
            clientCallbackPanelUserInput.PerformCallback('SelectZbirnikTon');
        }

        function ValueChanged_lookUpPrevoznik(s, e) {
            lookUpPrevoznik.GetGridView().GetRowValues(lookUpPrevoznik.GetGridView().GetFocusedRowIndex(), 'Cena', OnGetRowValuesPrevoznik);
        }

        function OnGetRowValuesPrevoznik(value) {
            clientTxtNovaCena.SetText(value);
            clientHfCena.Set("IzbrCena", value);
            clientCallbackPanelUserInput.PerformCallback('SelectPrevoznik');
        }

        function btnDeleteSelectedPosition_Click(s, e) {
            $('#warningButtonModal').modal('show');
            $('#modal-btn-confirm').hide();
            $('#modal-btn-delete').show();
            $('#warningButtonModalBody').empty();
            $('#warningButtonModalBody').append("Ali želite izbrisati izbrano pozicijo iz odpoklica?");
            $('#warningButtonModalTitle').empty();
            $('#warningButtonModalTitle').append("Brisanje pozicije odpoklica!");
        }

        function btnDeleteSelectedPosition_Init(s, e) {
            if (gridSelectedBuyersPosition.GetFocusedRowIndex() >= 0)
                s.SetEnabled(true);
        }
        function EndCallback_gridSelectedPositions(s, e) {

            clientLoadingPanel.Show();
            clientCallbackPanelUserInput.PerformCallback('Enable');

        }

        function OnLostFocus(s, e) {
            clientLoadingPanel.Show();
            clientCallbackPanelUserInput.PerformCallback('ChangePrice');
        } 

        function EndCallback_clientCallbackPanelUserInput(s, e) {
            if (s.cpError != null && s.cpError !== undefined) {
                ShowWarningPopUp("Opozorilo!", s.cpError);
                delete (s.cpError);
            }
            else if (s.cpRefreshGrid != null && s.cpRefreshGrid !== undefined) {
                delete (s.cpRefreshGrid);
                gridSelectedBuyersPosition.Refresh();
            }
            clientLoadingPanel.Hide();
        }


        function OnClosePopUpHandler(command, sender, userAction, recallID) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'OrderBuyerPos':
                            clientPopUpOrderPos.Hide();
                            if (userAction !== undefined && recallID !== undefined) {
                                clientLoadingPanel.Show();
                                window.location.replace("RecallBuyerCreate.aspx?action=" + userAction + "&recordId=" + recallID + "&Refresh=1");
                            }
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'OrderBuyerPos':
                            clientPopUpOrderPos.Hide();
                            break;
                    }
                    break;
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="clientLoadingPanel" Modal="true"></dx:ASPxLoadingPanel>
    <dx:ASPxHiddenField runat="server" ClientInstanceName="clientHfCurrentSum" ID="hfCurrentSum">
    </dx:ASPxHiddenField>
    <dx:ASPxHiddenField runat="server" ClientInstanceName="clientHfCena" ID="ASPxHFCena">
    </dx:ASPxHiddenField>
    <dx:ASPxHiddenField runat="server" ClientInstanceName="clientHfCurrentSumPercent" ID="hfCurrentSumPercent">
    </dx:ASPxHiddenField>
    <dx:ASPxHiddenField runat="server" ClientInstanceName="cliAkcija" ID="ASPxHiddenField1">
    </dx:ASPxHiddenField>
    <div class="row2 align-item-centerV-startH" style="margin-bottom: 20px">
        <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 38px">
            <dx:ASPxLabel ID="lblRelacija" runat="server" Text="RELACIJA : "></dx:ASPxLabel>
        </div>
        <div class="col-xs-4 no-padding-left">
            <dx:ASPxGridLookup ID="ASPxGridLookupRealacija" runat="server" ClientInstanceName="lookUpRelacija"
                KeyFieldName="RelacijaID" TextFormatString="{1}" CssClass="text-box-input"
                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                OnLoad="ASPxGridLookupLoad_WidthLarge" OnDataBinding="ASPxGridLookupRealacija_DataBinding" IncrementalFilteringMode="Contains">
                <ClearButton DisplayMode="OnHover" />
                <ClientSideEvents ValueChanged="ValueChanged_lookUpRelacija" />
                <%-- DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Naziv').Focus();}" --%>
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                <GridViewStyles>
                    <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                    <FilterBarClearButtonCell></FilterBarClearButtonCell>
                </GridViewStyles>
                <GridViewProperties>
                    <SettingsBehavior EnableRowHotTrack="True" />
                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                    <SettingsPager ShowSeparators="True" AlwaysShowPager="false" PageSize="200">
                        <PageSizeItemSettings Visible="false"></PageSizeItemSettings>
                    </SettingsPager>
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                        ShowHorizontalScrollBar="true" VerticalScrollableHeight="200" GridLines="Both"></Settings>
                </GridViewProperties>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Relacija Id" FieldName="RelacijaID" Width="80px"
                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="100%"
                        ReadOnly="true" ShowInCustomizationForm="True">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Dolžina"
                        FieldName="Dolzina" ShowInCustomizationForm="True"
                        Width="20%" Visible="false">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Datum"
                        FieldName="Datum" ShowInCustomizationForm="True"
                        Width="15%" Visible="false">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                </Columns>
            </dx:ASPxGridLookup>
        </div>
    </div>
    <dx:ASPxGridView ID="ASPxGridSelectPositions" runat="server" ClientInstanceName="gridSelectedBuyersPosition"
        Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" KeyFieldName="OdpoklicKupecID" CssClass="newFont"
        OnDataBinding="ASPxGridViewSelectedPositions_DataBinding" OnCellEditorInitialize="ASPxGridViewSelectedPositions_CellEditorInitialize"
        OnCommandButtonInitialize="ASPxGridViewSelectedPositions_CommandButtonInitialize"
        OnHtmlRowPrepared="ASPxGridViewSelectedPositions_HtmlRowPrepared" OnDataBound="ASPxGridViewSelectedPositions_DataBound">
        <Paddings Padding="0" />
        <Settings ShowVerticalScrollBar="True" ShowFooter="true" ShowStatusBar="Visible"
            ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400"
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
        <SettingsText EmptyDataRow="Trenutno ni podatka o odpoklicih. Dodaj novega." />
        <Columns>
            <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="80px" SelectAllCheckboxMode="None" Caption="Izberi" ShowClearFilterButton="true" Visible="false" />
            <dx:GridViewDataTextColumn Caption="Ključ" FieldName="Kljuc" Width="8%"
                ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ZaporednaStevilka" FieldName="ZaporednaStevilka" Width="8%"
                ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataDateColumn Caption="Datum" FieldName="Datum" Width="6%"
                ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                <PropertiesDateEdit DisplayFormatString="dd.MM.yyy" EditFormatString="dd.MM.yyyy"></PropertiesDateEdit>
            </dx:GridViewDataDateColumn>



            <dx:GridViewDataTextColumn Caption="Valuta" FieldName="Valuta" Width="5%" Visible="false"
                ReadOnly="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Kupec" FieldName="Kupec" Width="30%"
                ReadOnly="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Prevzemnik" FieldName="Prevzemnik" Width="30%"
                ReadOnly="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Skupna količina"
                FieldName="Kolicina" ShowInCustomizationForm="True"
                Width="7%">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                <PropertiesTextEdit DisplayFormatString="f2"></PropertiesTextEdit>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Skupna vrednost"
                FieldName="Vrednost" ShowInCustomizationForm="True"
                Width="7%">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                <PropertiesTextEdit DisplayFormatString="c"></PropertiesTextEdit>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Procent prevoz"
                FieldName="ProcentPrevoza" ShowInCustomizationForm="True"
                Width="7%">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                <PropertiesTextEdit DisplayFormatString="f2"></PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
        </Columns>
        <ClientSideEvents EndCallback="EndCallback_gridSelectedPositions" />
        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="Kolicina" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="ProcentPrevoza" SummaryType="Sum" />
        </TotalSummary>
        <Templates>
            <FooterRow>
                <div style="margin: 0 auto; text-align: center;">
                    <div>
                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Bold="true" Text="Količina skupaj: "></dx:ASPxLabel>
                        <dx:ASPxLabel ID="SummaryLabel" runat="server" Text='<%# GetTotalSummaryValue() %>' Font-Bold="true" Font-Size="14px" ClientInstanceName="labelSum"></dx:ASPxLabel>
                    </div>
                    <div>
                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Bold="true" Text="Procent prevoza skupaj: "></dx:ASPxLabel>
                        <dx:ASPxLabel ID="lblSumProcentSkupaj" runat="server" Text='<%# GetAvgSummaryTransportPercent() %>' Font-Bold="true" Font-Size="14px" ClientInstanceName="labelSum"></dx:ASPxLabel>
                    </div>
                </div>
            </FooterRow>
            <StatusBar>
                <div class="row">
                    <div class="col-xs-6 align-item-centerV-startH">
                        <span class="AddEditButtons no-margin-top">
                            <dx:ASPxButton ID="btnAddOrderPosition" runat="server" Text="Dodaj pozicijo" AutoPostBack="false" CssClass="statusBarButton"
                                Height="40" Width="110" UseSubmitBehavior="false" ClientInstanceName="clientBtnAddOrderPosition">
                                <ClientSideEvents Click="btnAddOrderPosition_Click" />
                                <DisabledStyle CssClass="statusBarButtonsDisabled"></DisabledStyle>
                            </dx:ASPxButton>
                        </span>
                        <span class="AddEditButtons no-margin-top">
                            <dx:ASPxButton ID="btnDeleteSelectedPosition" runat="server" Text="Izbriši izbrano pozicijo" AutoPostBack="false" CssClass="statusBarButton red"
                                Height="40" Width="110" UseSubmitBehavior="false" ClientEnabled="false" ClientInstanceName="clientBtnDeleteSelectedPosition">
                                <ClientSideEvents Click="btnDeleteSelectedPosition_Click" Init="btnDeleteSelectedPosition_Init" />
                                <DisabledStyle CssClass="statusBarButtonsDisabled"></DisabledStyle>
                            </dx:ASPxButton>
                        </span>
                    </div>
                </div>
            </StatusBar>
        </Templates>
    </dx:ASPxGridView>

    <dx:ASPxCallbackPanel ID="CallbackPanelUserInput" runat="server" Width="100%" ClientInstanceName="clientCallbackPanelUserInput"
        OnCallback="CallbackPanelUserInput_Callback">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="EndCallback_clientCallbackPanelUserInput" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="row small-padding-bottom medium-padding-top">
                    <div class="col-md-3">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 30px;">
                                <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="ODPOKLIC : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-6 no-padding-left">
                                <dx:ASPxTextBox runat="server" ID="txtStOdpoklic" ClientEnabled="false" ClientInstanceName="clientTxtStOdpoklic"
                                    CssClass="text-box-input" Font-Size="14px" ReadOnly="true" BackColor="LightGray" Width="100%" Font-Bold="true">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="row2 align-item-centerV-centerH">
                            <div class="col-xs-0 big-margin-r no-padding-right">
                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="STATUS : ">
                                </dx:ASPxLabel>
                            </div>
                            <div class="col-xs-10 no-padding-left" style="align-items: center">
                                <dx:ASPxTextBox runat="server" ID="txtStatus" ClientEnabled="false" ClientInstanceName="clientTxtStatus"
                                    CssClass="text-box-input" Font-Size="14px" ReadOnly="true" BackColor="LightGray" Width="80%" Font-Bold="true">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="row2 align-item-centerV-centerH">
                            <div class="col-xs-0 big-margin-r no-padding-right">
                                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Št. naročilnice : ">
                                </dx:ASPxLabel>
                            </div>
                            <div class="col-xs-10 no-padding-left" style="align-items: center">
                                <dx:ASPxTextBox runat="server" ID="txtStNarocilnice" ClientEnabled="false" ClientInstanceName="clientTxtStNarocilnice"
                                    CssClass="text-box-input" Font-Size="14px" ReadOnly="true" BackColor="LightGray" Width="60%" Font-Bold="true">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row small-padding-bottom">
                    <div class="col-md-4">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-sm-0 big-margin-r" style="padding-right: 6px">
                                <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Zbirnik Teža : " ClientInstanceName="clientLblZbirnik"></dx:ASPxLabel>
                            </div>
                            <div class="col-sm-8 no-padding-left">
                                <dx:ASPxGridLookup ID="ASPxGridLookupZbirnikTon" runat="server" ClientInstanceName="lookUpZbirnikTon"
                                    KeyFieldName="ZbirnikTonID" TextFormatString="{2}" CssClass="text-box-input" ClientEnabled="true"
                                    Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                    OnLoad="ASPxGridLookupLoad_WidthMedium" OnDataBinding="ASPxGridLookupZbirnikTon_DataBinding" IncrementalFilteringMode="Contains">
                                    <ClearButton DisplayMode="OnHover" />
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                    <ClientSideEvents ValueChanged="ValueChanged_lookUpZbirnikTon" />
                                    <GridViewStyles>
                                        <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                        <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                    </GridViewStyles>
                                    <GridViewProperties>
                                        <SettingsBehavior EnableRowHotTrack="True" />
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                        <SettingsPager ShowSeparators="True" AlwaysShowPager="false" PageSize="200">
                                            <PageSizeItemSettings Visible="false"></PageSizeItemSettings>
                                        </SettingsPager>
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                            ShowHorizontalScrollBar="true" VerticalScrollableHeight="200" GridLines="Both"></Settings>
                                    </GridViewProperties>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="Id" FieldName="ZbirnikTonID" Width="80px"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Koda" FieldName="Koda" Width="80px"
                                            ReadOnly="true" ShowInCustomizationForm="True" VisibleIndex="0">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="20%"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Teža Od" FieldName="TezaOd" Width="20%"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Teža Do" FieldName="TezaDo" Width="20%"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>


                                    </Columns>
                                </dx:ASPxGridLookup>
                            </div>
                        </div>
                    </div>

                </div>

                 <div class="row small-padding-bottom">
                    <div class="col-md-12">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 37px;">
                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="OPOMBA : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-10 no-padding-left">
                                 <dx:ASPxMemo runat="server" Rows="4" Width="60%" NullText="Vpiši opombo..." ClientVisible="true" ID="memOpis" MaxLength="5000"
                                    ClientInstanceName="clientMemoKomentar" />
                            </div>
                        </div>
                    </div>                   
                </div>

                <div class="row small-padding-bottom">
                    <div class="col-md-6">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 22px;">
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="PREVOZNIK : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-10 no-padding-left">
                                <dx:ASPxGridLookup ID="ASPxGridLookupPrevoznik" runat="server" ClientInstanceName="lookUpPrevoznik"
                                    KeyFieldName="RazpisPozicijaID" TextFormatString="{1} - {4} EUR" CssClass="text-box-input"
                                    Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                    OnLoad="ASPxGridLookupLoad_WidthXtraLarge" OnDataBinding="ASPxGridLookupPrevoznik_DataBinding" IncrementalFilteringMode="Contains" SelectionMode="Single">
                                    <ClearButton DisplayMode="OnHover" />
                                    <ClientSideEvents ValueChanged="ValueChanged_lookUpPrevoznik" />
                                    <%-- DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Stranka.NazivPrvi').Focus();}" --%>
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                    <GridViewStyles>
                                        <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                        <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                    </GridViewStyles>
                                    <GridViewProperties>
                                        <SettingsBehavior EnableRowHotTrack="True" />
                                        <SettingsBehavior AllowFocusedRow="True" />
                                        <SettingsPager ShowSeparators="True" AlwaysShowPager="false" ShowNumericButtons="true" NumericButtonCount="3" PageSize="200">
                                            <PageSizeItemSettings Visible="false"></PageSizeItemSettings>
                                        </SettingsPager>
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                            ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                    </GridViewProperties>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="Stranka Id" FieldName="Stranka.idStranka" Width="80px"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Stranka.NazivPrvi" Width="55%"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Email"
                                            FieldName="Stranka.Email" ShowInCustomizationForm="True"
                                            Width="20%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Telefon"
                                            FieldName="Stranka.Telefon" ShowInCustomizationForm="True"
                                            Width="15%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Cena"
                                            FieldName="Cena" ShowInCustomizationForm="True"
                                            Width="10%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                        </dx:GridViewDataTextColumn>

                                    </Columns>
                                    <GridViewProperties>
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" UseSubmitBehavior="false" Text="Potrdi in zapri"
                                                                Image-Url="~/Images/confirm.png" Image-UrlHottracked="~/Images/confirmHover.png" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <Settings ShowFilterRow="True" ShowStatusBar="Visible" />
                                        <SettingsPager EnableAdaptivity="true" />
                                    </GridViewProperties>
                                </dx:ASPxGridLookup>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="row2 align-item-centerV-endH">
                            <div class="col-xs-0 big-margin-r no-padding-right">
                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="CENA : ">
                                </dx:ASPxLabel>
                            </div>
                            <div class="col-xs-3 no-padding-left" style="align-items: flex-end">
                                <dx:ASPxTextBox runat="server" ID="txtNovaCena" ClientEnabled="true" ClientInstanceName="clientTxtNovaCena"
                                    CssClass="text-box-input" Font-Size="14px" AutoCompleteType="Disabled" Width="100%">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                    <ClientSideEvents LostFocus="OnLostFocus" />                                      
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>


                    <div class="col-md-6">
                        <div class="row2 align-item-centerV-endH">
                            <div class="col-xs-12 no-padding-left" style="align-items: flex-end;">
                                <dx:ASPxMemo runat="server" Rows="4" Width="60%" NullText="Vpiši komentar..." ClientVisible="false" ID="memoKomentar" MaxLength="5000"
                                    ClientInstanceName="clientMemoKomentar" />
                            </div>
                        </div>
                    </div>
                </div>

               



                <dx:ASPxPopupControl ID="ASPxPopupControlOrderPos" runat="server" ContentUrl="OrderBuyerPos_popup.aspx"
                    ClientInstanceName="clientPopUpOrderPos" Modal="True" HeaderText="POZICIJE NAROČILNIC"
                    CloseAction="CloseButton" Width="1650px" Height="700px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true"
                    OnWindowCallback="ASPxPopupControlOrderPos_WindowCallback">
                    <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                    <ContentStyle BackColor="#F7F7F7">
                        <Paddings PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px" PaddingTop="0px"></Paddings>
                    </ContentStyle>
                </dx:ASPxPopupControl>


                <dx:ASPxPopupControl ID="ASPxPopupControlCreateOrder" runat="server" ContentUrl="CreateOrder_popup.aspx"
                    ClientInstanceName="clientPopUpCreateOrder" Modal="True" HeaderText="USTVARI NAROČILO"
                    CloseAction="CloseButton" Width="1300px" Height="900px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true"
                    OnWindowCallback="ASPxPopupControlCreateOrder_WindowCallback">
                    <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                    <ContentStyle BackColor="#F7F7F7">
                        <Paddings PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px" PaddingTop="0px"></Paddings>
                    </ContentStyle>
                </dx:ASPxPopupControl>

                <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
                    <span class="AddEditButtons">
                        <dx:ASPxButton Theme="Moderno" ID="btnConfirm" runat="server" Text="Delovna verzija" AutoPostBack="false"
                            Height="25" Width="110" ClientInstanceName="clientBtnConfirm" UseSubmitBehavior="false" OnClick="btnConfirm_Click">
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


                    <span class="AddEditButtons">
                        <dx:ASPxButton ID="btnReopenRecall" runat="server" Text="Odpri odpoklic" AutoPostBack="false"
                            Height="50" Width="110" UseSubmitBehavior="false" ClientVisible="false" ClientInstanceName="clientBtnReopenRecall"
                            OnClick="btnReopenRecall_Click">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../../Images/lock.png" UrlHottracked="../../Images/lockHover.png" />
                        </dx:ASPxButton>
                    </span>

                     <span class="AddEditButtons">
                        <dx:ASPxButton ID="btnStorno" runat="server" ForeColor="Red" Text="Storno" AutoPostBack="false"
                            Height="50" Width="110" UseSubmitBehavior="false" ClientVisible="false" ClientInstanceName="clientbtnStorno"
                            OnClick="btnStorno_Click">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../../Images/storno.png" UrlHottracked="../../Images/storno.png" />
                        </dx:ASPxButton>
                    </span>


                    <div class="AddEditButtonsElements">
                        <span class="AddEditButtons">
                            <dx:ASPxButton Theme="Moderno" ID="btnRecall" runat="server" Text="Izdelaj naročilnico" AutoPostBack="false" UseSubmitBehavior="false"
                                Height="25" Width="130" ClientEnabled="true" ClientInstanceName="clientBtnRecall" OnClick="btnRecall_Click">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                            </dx:ASPxButton>
                        </span>
                    </div>

                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

    <!-- Warning - Modal -->
    <div id="warningModal" class="modal fade" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" id="modal-head" style="background-color: tomato; border-top-left-radius: 6px; border-top-right-radius: 6px;">
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

    <div class="modal fade" role="dialog" data-backdrop="static" data-keyboard="false" id="warningButtonModal">
        <div class="vertical-alignment-helper">
            <div class="modal-dialog modal-sm vertical-align-center">
                <div class="modal-content">
                    <div class="modal-header kvp-model-header">
                        <button type="button" class="close" data-dismiss="modal" style="color: white; opacity: 0.8">&times;</button>
                        <h4 class="modal-title" id="warningButtonModalTitle" style="color: white;"></h4>
                    </div>
                    <div class="modal-body text-center">
                        <div class="row2">
                            <div class="col-xs-2 align-item-centerV-centerH">
                                <i class="fa fa-balance-scale" style="font-size: 30px; color: #3C8DBC;"></i>
                            </div>
                            <div class="col-xs-10 text-right">
                                <p id="warningButtonModalBody"></p>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" id="modal-btn-confirm">Potrdi</button>
                        <button type="button" class="btn btn-default" id="modal-btn-delete">Izbriši</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">Zapri</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
