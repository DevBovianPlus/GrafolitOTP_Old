<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RecallForm.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Recall.RecallForm" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script>
        var max = 24500;//max količina!
        var criticalTransportType = 15;
        var newQuantity = 0;//pri tipu prevoza 15 preverjamo optimalno zalogo z trenutno + odpoklicano količino!

        $(document).ready(function () {
            if (parseFloat(labelSum.GetText()) > max)
                labelSum.GetMainElement().style.color = 'Red';

        });
        function OnGetRowValues(Value) {
            alert(Value);
        }
        function OnBatchEditEndEditing(s, e) {

            var originalValue = parseFloat(s.batchEditApi.GetCellValue(e.visibleIndex, "Kolicina").toString().replace(',', '.'));
            var newValue = parseFloat(e.rowValues[(s.GetColumnByField("Kolicina").index)].value.toString().replace(',', '.'));

            var originalValuePalete = parseFloat(s.batchEditApi.GetCellValue(e.visibleIndex, "Palete").toString().replace(',', '.'));
            var newValuePalete = parseFloat(e.rowValues[(s.GetColumnByField("Palete").index)].value.toString().replace(',', '.'));

            var dif = newValue - originalValue;
            var sum = (parseFloat(labelSum.GetValue().replace('.', '').replace(',', '.')) + dif);

            var difPalete = newValuePalete - originalValuePalete;
            var sumPalete = (parseInt(labelSumPalete.GetValue() + difPalete));

            clientHfCurrentSum.Set("CurrenSum", sum);

            clientHfCurrentSumPalete.Set("CurrenSumPalete", sumPalete);

            if (dif != 0) {
                if (sum > max) {

                    //ShowErrorPopUp("Previsoka količina za odpoklic (max 24t)", 0, "Previsoka količina!");
                    ShowWarningPopUp("Previsoka količina!", "Previsoka količina za odpoklic (max 24t)");
                    labelSum.GetMainElement().style.color = 'Red';
                }
                else {
                    labelSum.GetMainElement().style.color = 'Green';

                    newQuantity = newValue.toFixed(2);
                    gridSelectedPositions.GetRowValues(e.visibleIndex, 'TrenutnaZaloga;OptimalnaZaloga;TipID;Kolicina', OnGetRowValues);
                }
            }

            labelSum.SetText(sum.toLocaleString());

            labelSumPalete.SetValue(sumPalete.toFixed(0));
        }

        function roundNumber(rnum, rlength) {
            var newnumber = Math.round(rnum * Math.pow(10, rlength)) / Math.pow(10, rlength);
            return newnumber;
        }


        function OnGetRowValues(values) {
            var currentStock = values[0];
            var optimalStock = values[1];
            var typeID = values[2];

            if (typeID == criticalTransportType) {
                if ((parseFloat(newQuantity) + currentStock) > optimalStock)
                    ShowWarningPopUp("Preseg optimalne zaloge!", "Odpoklicana količina " + newQuantity + " ne sme presegati optimalne zaloge");
                //ShowErrorPopUp("Odpoklicana količina " + newQuantity + " ne sme presegati optimalne zaloge", 0, "Preseg optimalne zaloge!");
            }
        }

        function EndCallback_gridSelectedPositions(s, e) {
            if (gridSelectedPositions.cpQuntityOversize != null && gridSelectedPositions.cpQuntityOversize !== undefined) {
                delete (gridSelectedPositions.cpQuntityOversize);
                //ShowErrorPopUp("Previsoka količina za odpoklic (max 24t)", 0, "Previsoka količina!");
                ShowWarningPopUp("Previsoka količina!", "Previsoka količina za odpoklic (max 24t)");
            }
            else if (gridSelectedPositions.cpQuntityOverflowOptimalStock != null && gridSelectedPositions.cpQuntityOverflowOptimalStock !== undefined) {
                delete (gridSelectedPositions.cpQuntityOverflowOptimalStock);
                //ShowErrorPopUp("Odpoklicana količina ne sme presegati optimalne zaloge", 0, "Preseg optimalne zaloge!");
                ShowWarningPopUp("Preseg optimalne zaloge!", "Odpoklicana količina ne sme presegati optimalne zaloge");
            }
            else if (gridSelectedPositions.cpRecallQuantityOversize != null && gridSelectedPositions.cpRecallQuantityOversize !== undefined)
            {
                ShowWarningPopUp("Previsoka količina!", gridSelectedPositions.cpRecallQuantityOversize);
                delete (gridSelectedPositions.cpRecallQuantityOversize);
            }
            else
                clientCallbackPanelUserInput.PerformCallback('Enable');
        }

        function CheckFieldValidation(s, e) {
            var process = false;
            var lookUpItems = [lookUpStranke, lookUpRelacija];
            //var comboBoxItems = [clientComboBoxTip];
            var inputItems = [clientTxtNovaCena];
            var memoItems = null;

            if (clientMemoKomentar.GetVisible())
                memoItems = [clientMemoKomentar];

            process = InputFieldsValidation(lookUpItems, inputItems, null, memoItems, /*comboBoxItems*/null, null);

            if (clientBtnConfirm.GetText() == 'Izbriši')
                process = true;

            if (process)
                e.processOnServer = true;
            else
                e.processOnServer = false;

            return process;
        }

        function ValueChanged_lookUpStranke(s, e) {
            var key = lookUpStranke.GetGridView().GetRowKey(lookUpStranke.GetGridView().GetFocusedRowIndex());
            if (key != null && key > 0)
                clientCallbackPanelUserInput.PerformCallback('PriceCompare');
        }

        function EndCallback_clientCallbackPanelUserInput(s, e) {
            if (s.cpError != null && s.cpError !== undefined) {
                //ShowErrorPopUp(s.cpError, 0, "Napaka!");
                ShowWarningPopUp("Opozorilo!", s.cpError);
                delete (s.cpError);
            }
        }

        function ShowWarningPopUp(title, message) {
            $('#warningModal').modal('show');
            $('#modalBodyText').empty();
            $('#modalBodyText').append(message);
            $('#myModalTitle').empty();
            $('#myModalTitle').append(title);
        }

        function OnSelectionChanged_gridSelectedPositions(s, e) {

            if (s.GetSelectedRowCount() > 0)
                clientBtnConfirmTakeOver.SetVisible(true);
            else
                clientBtnConfirmTakeOver.SetVisible(false);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxHiddenField runat="server" ClientInstanceName="clientHfCurrentSum" ID="hfCurrentSum">
    </dx:ASPxHiddenField>
    <dx:ASPxHiddenField runat="server" ClientInstanceName="clientHfCurrentSumPalete" ID="hfCurrentSumPalete">
    </dx:ASPxHiddenField>
    <dx:ASPxGridView ID="ASPxGridViewSelectedPositions" runat="server" AutoGenerateColumns="False"
        EnableTheming="True" EnableCallbackCompression="true" ClientInstanceName="gridSelectedPositions"
        Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G"
        KeyFieldName="OdpoklicPozicijaID" Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Paddings-PaddingLeft="3px" Paddings-PaddingRight="3px"
        CssClass="gridview-no-header-padding" OnDataBinding="ASPxGridViewSelectedPositions_DataBinding" OnCellEditorInitialize="ASPxGridViewSelectedPositions_CellEditorInitialize"
        OnBatchUpdate="ASPxGridViewSelectedPositions_BatchUpdate" OnCommandButtonInitialize="ASPxGridViewSelectedPositions_CommandButtonInitialize">
        <ClientSideEvents SelectionChanged="OnSelectionChanged_gridSelectedPositions" />
        <Settings ShowVerticalScrollBar="True"
            VerticalScrollableHeight="250" HorizontalScrollBarMode="Auto"
            VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" ShowFooter="true" />
        <SettingsPager PageSize="10" ShowNumericButtons="true">
            <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
        </SettingsPager>
        <SettingsBehavior AllowFocusedRow="true" AllowGroup="true" AllowSort="true" />
        <Styles Header-Wrap="True">
            <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
            <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
        </Styles>
        <SettingsText EmptyDataRow="Trenutno ni podatka o odpoklicih. Dodaj novega."
            CommandBatchEditUpdate="Spremeni" CommandBatchEditCancel="Prekliči" />
        <SettingsEditing Mode="Batch" BatchEditSettings-StartEditAction="DblClick" />
        <%--<SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />--%>
        <SettingsBehavior AllowEllipsisInText="true" />
        <Columns>
            <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="90px" SelectAllCheckboxMode="None" Caption="Prevzemi" ShowClearFilterButton="true" />

            <dx:GridViewCommandColumn Caption="Briši"
                ShowDeleteButton="true" Width="7%" VisibleIndex="0" Visible="false" />

            <dx:GridViewDataTextColumn Caption="Naročilo ID" FieldName="NarociloID" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True" SortOrder="Ascending" EditFormSettings-Visible="False"
                VisibleIndex="1" Visible="false">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Naročilo pozicije ID" FieldName="NarociloPozicijaID" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True" Visible="false">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Potrditev naročila" FieldName="OC" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Artikel" FieldName="Material" Width="400"
                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Kolicina iz naročila"
                FieldName="KolicinaIzNarocila" ShowInCustomizationForm="True"
                Width="120px" AdaptivePriority="1" EditFormSettings-Visible="False">
                <PropertiesTextEdit DisplayFormatString="n2" DisplayFormatInEditMode="true" />
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Odpoklicana količina iz prejšnjih odpoklicev"
                FieldName="KolicinaOTP" ShowInCustomizationForm="True"
                Width="120px" AdaptivePriority="1" EditFormSettings-Visible="False">
                <PropertiesTextEdit DisplayFormatString="n3" DisplayFormatInEditMode="true" />
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Odpoklicana količina glede naročilnice"
                FieldName="KolicinaOTPPozicijaNarocilnice" ShowInCustomizationForm="True"
                Width="120px" AdaptivePriority="1" EditFormSettings-Visible="False">
                <PropertiesTextEdit DisplayFormatString="n3" DisplayFormatInEditMode="true" />
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Prevzeta količina" FieldName="KolicinaPrevzeta" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                <PropertiesTextEdit DisplayFormatString="n2" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Razlika količina" FieldName="KolicinaRazlika" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                <PropertiesTextEdit DisplayFormatString="n2" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Količina odpoklic"
                FieldName="Kolicina" ShowInCustomizationForm="True"
                Width="120px" AdaptivePriority="1">
                <PropertiesTextEdit DisplayFormatString="n2" DisplayFormatInEditMode="true">
                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                </PropertiesTextEdit>
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Število palet"
                FieldName="Palete" ShowInCustomizationForm="True"
                Width="100px" AdaptivePriority="1">
                <PropertiesTextEdit DisplayFormatString="n" DisplayFormatInEditMode="true">
                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                </PropertiesTextEdit>
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>


            <dx:GridViewDataTextColumn Caption="Tip prevoza" FieldName="TipNaziv" Width="300px"
                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
            </dx:GridViewDataTextColumn>
            <%--<dx:GridViewDataComboBoxColumn Caption="Tip prevoza" FieldName="TipID" Width="200px"></dx:GridViewDataComboBoxColumn>--%>

            <dx:GridViewDataTextColumn Caption="Optimalna zaloga" FieldName="OptimalnaZaloga" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                <PropertiesTextEdit DisplayFormatString="n2" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Trenutna zaloga" FieldName="TrenutnaZaloga" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                <PropertiesTextEdit DisplayFormatString="n2" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Kupec" FieldName="KupecNaziv" Width="300px"
                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataCheckColumn Caption="Kupec viden" FieldName="KupecViden" ShowInCustomizationForm="True" Width="90px">
                <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32"></PropertiesCheckEdit>
            </dx:GridViewDataCheckColumn>

            <dx:GridViewDataTextColumn Caption="Interno" FieldName="Interno" Width="300px"
                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Proizvedeno" FieldName="Proizvedeno" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                <PropertiesTextEdit DisplayFormatString="n3" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Status prevzeto" FieldName="StatusPrevzeto" Width="100px"
                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False" Visible="false">
            </dx:GridViewDataTextColumn>
        </Columns>
        <ClientSideEvents BatchEditEndEditing="OnBatchEditEndEditing" EndCallback="EndCallback_gridSelectedPositions" />
        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="Kolicina" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="Palete" SummaryType="Sum" />
        </TotalSummary>
        <Templates>
            <FooterRow>
                <div style="margin: 0 auto; text-align: center;">
                    <div>
                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Bold="true" Text="Količina skupaj: "></dx:ASPxLabel>
                        <dx:ASPxLabel ID="SummaryLabel" runat="server" Text='<%# GetTotalSummaryValue() %>' Font-Bold="true" Font-Size="14px" ClientInstanceName="labelSum"></dx:ASPxLabel>
                    </div>
                    <div>
                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Bold="true" Text="Palete skupaj: "></dx:ASPxLabel>
                        <dx:ASPxLabel ID="SummaryLabelPalete" runat="server" Text='<%# GetTotalPaleteSummaryValue() %>' Font-Bold="true" Font-Size="14px" ClientInstanceName="labelSumPalete"></dx:ASPxLabel>
                    </div>
                </div>
            </FooterRow>
        </Templates>
    </dx:ASPxGridView>


    <dx:ASPxCallbackPanel ID="CallbackPanelUserInput" runat="server" Width="100%" ClientInstanceName="clientCallbackPanelUserInput"
        OnCallback="CallbackPanelUserInput_Callback">
        <ClientSideEvents EndCallback="EndCallback_clientCallbackPanelUserInput" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="row small-padding-bottom medium-padding-top">
                    <div class="col-md-6">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right:30px">
                                <dx:ASPxLabel ID="lblRelacija" runat="server" Text="RELACIJA : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-10 no-padding-left">
                                <dx:ASPxGridLookup ID="ASPxGridLookupRealacija" runat="server" ClientInstanceName="lookUpRelacija"
                                    KeyFieldName="RelacijaID" TextFormatString="{1}" CssClass="text-box-input"
                                    Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                    OnLoad="ASPxGridLookupLoad_WidthLarge" OnDataBinding="ASPxGridLookupRealacija_DataBinding">
                                    <ClearButton DisplayMode="OnHover" />
                                    <ClientSideEvents Init="SetFocus" DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Naziv').Focus();}"
                                        ValueChanged="function(s,e){clientCallbackPanelUserInput.PerformCallback('DataBindPrevoznik');}" />
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

                                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="80%"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Dolžina"
                                            FieldName="Dolzina" ShowInCustomizationForm="True"
                                            Width="20%">
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
                    </div>
                    <div class="col-md-3">
                        <div class="row2 align-item-centerV-centerH">
                            <div class="col-xs-0 big-margin-r no-padding-right">
                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="STATUS : ">
                                </dx:ASPxLabel>
                            </div>
                            <div class="col-xs-7 no-padding-left" style="align-items: center">
                                <dx:ASPxTextBox runat="server" ID="txtStatus" ClientEnabled="false" ClientInstanceName="clientTxtStatus"
                                    CssClass="text-box-input" Font-Size="14px" ReadOnly="true" BackColor="LightGray" Width="100%" Font-Bold="true">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="row2 align-item-centerV-endH">
                            <div class="col-xs-0 big-margin-r no-padding-right">
                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="CENA : ">
                                </dx:ASPxLabel>
                            </div>
                            <div class="col-xs-6 no-padding-left" style="align-items: flex-end">
                                <dx:ASPxTextBox runat="server" ID="txtNovaCena" ClientEnabled="false" ClientInstanceName="clientTxtNovaCena"
                                    CssClass="text-box-input" Font-Size="14px" AutoCompleteType="Disabled" Width="100%">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                    <ClientSideEvents KeyPress="isNumberKey_decimal" ValueChanged="ValueChanged_lookUpStranke" />
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row small-padding-bottom">
                    <div class="col-md-5">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 big-margin-r no-padding-right">
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="PREVOZNIK : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-9 no-padding-left">
                                <dx:ASPxGridLookup ID="ASPxGridLookupStranke" runat="server" ClientInstanceName="lookUpStranke"
                                    KeyFieldName="RazpisPozicijaID" TextFormatString="{1}" CssClass="text-box-input"
                                    Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                    OnLoad="ASPxGridLookupLoad_WidthLarge" OnDataBinding="ASPxGridLookupStranke_DataBinding">
                                    <ClearButton DisplayMode="OnHover" />
                                    <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Stranka.NazivPrvi').Focus();}"
                                        ValueChanged="ValueChanged_lookUpStranke" />
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                    <GridViewStyles>
                                        <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                        <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                    </GridViewStyles>
                                    <GridViewProperties>
                                        <SettingsBehavior EnableRowHotTrack="True" />
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
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

                                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Stranka.NazivPrvi" Width="65%"
                                            ReadOnly="true" ShowInCustomizationForm="True">
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

                                    </Columns>
                                </dx:ASPxGridLookup>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-7">
                        <div class="row2 align-item-centerV-endH">
                            <div class="col-xs-12 no-padding-left" style="align-items: flex-end;">
                                <dx:ASPxMemo runat="server" Rows="4" Width="50%" NullText="Vpiši komentar..." ClientVisible="false" ID="memoKomentar" MaxLength="500"
                                    ClientInstanceName="clientMemoKomentar" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row medium-padding-bottom">
                    <div class="col-md-6">
                        <div class="row2" style="align-items: center;">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right:45px">
                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="ŠOFER : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-10 no-padding-left">
                                <dx:ASPxTextBox runat="server" ID="txtSofer" ClientInstanceName="clientTxtSofer"
                                    CssClass="text-box-input" Font-Size="14px" Width="100%" MaxLength="300" AutoCompleteType="Disabled">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="row2" style="align-items: center;">
                            <div class="col-xs-2 no-padding-right">
                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="REGISTRACIJA : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-10 no-padding-left">
                                <dx:ASPxTextBox runat="server" ID="txtRegistracija" ClientInstanceName="clientTxtRegistracija"
                                    CssClass="text-box-input" Font-Size="14px" Width="100%" MaxLength="300" AutoCompleteType="Disabled">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row medium-padding-bottom">
                    <div class="col-md-12">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right:30px">
                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="OPOMBA : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-12 no-padding-left">
                                <dx:ASPxMemo runat="server" Rows="4" Width="100%" NullText="Vpiši komentar..." ID="ASPxMemoOpombe"
                                    ClientInstanceName="clientMemoOpomba" MaxLength="500" CssClass="text-box-input">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxMemo>
                            </div>
                        </div>
                    </div>
                </div>

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

                    <span class="AddEditButtons" style="margin-left: 30px;">
                        <dx:ASPxButton ID="btnConfirmRecall" runat="server" Text="Potrdi odpoklic" AutoPostBack="false"
                            Height="25" Width="110" UseSubmitBehavior="false" Font-Bold="true" OnClick="btnConfirmRecall_Click" ClientVisible="false">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../../../Images/confirm.png" UrlHottracked="../../Images/confirmHover.png" />
                        </dx:ASPxButton>
                    </span>

                    <span class="AddEditButtons">
                        <dx:ASPxButton ID="btnRejectRecall" runat="server" Text="Zavrni odpoklic" AutoPostBack="false"
                            Height="25" Width="110" UseSubmitBehavior="false" OnClick="btnRejectRecall_Click" Font-Bold="true" ClientVisible="false">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../../../Images/reject.png" UrlHottracked="../../Images/rejectHover.png" />
                        </dx:ASPxButton>
                    </span>

                    <span class="AddEditButtons">
                        <dx:ASPxButton ID="btnConfirmTakeOver" runat="server" Text="Potrdi prevzem" AutoPostBack="false"
                            Height="50" Width="110" UseSubmitBehavior="false" Font-Bold="true" ClientVisible="false" ClientInstanceName="clientBtnConfirmTakeOver"
                            OnClick="btnConfirmTakeOver_Click">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../../../Images/prevzem.png" UrlHottracked="../../Images/prevzemHover.png" />
                        </dx:ASPxButton>
                    </span>

                    <div class="AddEditButtonsElements">
                        <span class="AddEditButtons">
                            <dx:ASPxButton Theme="Moderno" ID="btnRecall" runat="server" Text="Odpokliči" AutoPostBack="false" UseSubmitBehavior="false"
                                Height="25" Width="130" ClientEnabled="false" ClientInstanceName="clientBtnRecall" OnClick="btnRecall_Click">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <ClientSideEvents Click="CheckFieldValidation" />
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
                <div class="modal-header text-center" style="background-color: tomato; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="fa fa-exclamation-circle" style="font-size: 60px; color: white"></i></div>
                </div>
                <h4 class="modal-title text-center" id="myModalTitle" style="margin-top: 10px; font-weight: 600">Opozorilo!</h4>
                <div class="modal-body text-center" id="modalBodyText">
                </div>
                <%--<div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>--%>
            </div>

        </div>
    </div>
</asp:Content>
