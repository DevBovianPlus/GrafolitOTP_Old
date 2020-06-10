<%@ Page Title="Odpoklici" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RecallForm.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Recall.RecallForm" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">
        var max = 0;//max količina!
        var criticalTransportType = 15;
        var newQuantity = 0;//pri tipu prevoza 15 preverjamo optimalno zalogo z trenutno + odpoklicano količino!
        var firstShow = true;
        var transportTypeCode = '';
        $(document).ready(function () {

            //pridobimo max količino za odpoklic glede na tip prevoza
            lookUpTipPrevoza_ValueChanged();

            $("#modal-btn-delete").on("click", function () {
                $("#warningButtonModal").modal('hide');
                firstShow = false;
                clientLoadingPanel.Show();
                clientCallbackPanelUserInput.PerformCallback('DeleteSelectedPosition');
            });

            $("#modal-btn-confirm").on("click", function () {
                $("#warningButtonModal").modal('hide');
                firstShow = false;
                clientLoadingPanel.Show();
                clientCallbackPanelUserInput.PerformCallback('SplitSelectedPosition');
            });

        });


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
                    //ShowWarningPopUp("Previsoka količina!", "Previsoka količina za odpoklic (max 24t)");
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
            else if (gridSelectedPositions.cpRecallQuantityOversize != null && gridSelectedPositions.cpRecallQuantityOversize !== undefined) {
                ShowWarningPopUp("Previsoka količina!", gridSelectedPositions.cpRecallQuantityOversize);
                delete (gridSelectedPositions.cpRecallQuantityOversize);
            }
            else {
                clientLoadingPanel.Show();
                clientCallbackPanelUserInput.PerformCallback('Enable');
            }
        }

        var postbackInitiated = false;
        function CheckFieldValidation(s, e) {
            var process = false;
            var lookUpItems = [lookUpStranke, lookUpRelacija, lookUpTipPrevoza];
            //var comboBoxItems = [clientComboBoxTip];
            var inputItems = [clientTxtNovaCena];
            var memoItems = null;

            var codeTypeShip = '<%= DatabaseWebService.Common.Enums.Enums.TransportType.LADJA.ToString() %>';
            var codeTypePlane = '<%= DatabaseWebService.Common.Enums.Enums.TransportType.LETALO.ToString() %>';
            var codeTypeContainer = '<%= DatabaseWebService.Common.Enums.Enums.TransportType.ZBIRNIK.ToString() %>';

            if (transportTypeCode == codeTypeShip || transportTypeCode == codeTypePlane || transportTypeCode == codeTypeContainer) {
                lookUpItems = [lookUpTipPrevoza];
                inputItems = null;
            }

            if (clientMemoKomentar.GetVisible())
                memoItems = [clientMemoKomentar];

            if (lookUpSkladisce.GetEnabled())
                lookUpItems.push(lookUpSkladisce);

            if (clientSupplierArrangesTransportCheckBox.GetChecked()) {
                inputItems = null;
                lookUpItems.shift();
            }

            if (clientBuyerArrangesTransportCheckBox.GetChecked()) {
                lookUpItems = null;
                inputItems = null;
                //inputItems = [clientTxtRegistracija];
            }

            else if (!clientSupplierArrangesTransportCheckBox.GetChecked())
                inputItems = [clientTxtNovaCena];

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

        function ValueChanged_lookUpStranke(s, e) {
            /*var key = lookUpStranke.GetGridView().GetRowKey(lookUpStranke.GetGridView().GetFocusedRowIndex());
            if (key != null && key > 0) {
                clientLoadingPanel.Show();
                clientCallbackPanelUserInput.PerformCallback('PriceCompare');
            }*/

            lookUpStranke.GetGridView().GetSelectedFieldValues("RazpisPozicijaID", GotSelectedValues);
        }

        function GotSelectedValues(value) {
            if (value.length == 1) {
                clientLoadingPanel.Show();
                clientCallbackPanelUserInput.PerformCallback('PriceCompare');
            }
            else if (value.length > 1) {
                clientBtnRecall.SetEnabled(false);

                var allowSending = ('<%= ConfigurationManager.AppSettings["AllowSendingInquirytoCarriers"].ToString() %>' == '1');
                if (allowSending)
                    clientBtnSendInquiry.SetVisible(true);//ko imamo izbranih več prevoznikov omogočimo gumb za pošiljanje mailov vsem izbranim prevoznikom.
            }
        }

        function EndCallback_clientCallbackPanelUserInput(s, e) {
            if (s.cpError != null && s.cpError !== undefined) {
                //ShowErrorPopUp(s.cpError, 0, "Napaka!");
                ShowWarningPopUp("Opozorilo!", s.cpError);
                delete (s.cpError);
            }
            else if (s.cpRefreshGrid != null && s.cpRefreshGrid !== undefined) {
                delete (s.cpRefreshGrid);
                gridSelectedPositions.Refresh();
            }
            clientLoadingPanel.Hide();
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

        function SupplierArrangesTransportCheckBox_CheckChanged(s, e) {
            var value = s.GetChecked();
            if (value) {
                clientLoadingPanel.Show();
                clientCallbackPanelUserInput.PerformCallback('SupplierArrangesTransport');
                clientBuyerArrangesTransportCheckBox.SetChecked(false);
            }
            else {
                EnableUserControls(!value, false);
            }
        }

        function EnableUserControls(value, buyerArrangesTransport) {
            clientTxtNovaCena.SetEnabled(value);
            lookUpStranke.SetEnabled(value);
            clientTxtRegistracija.SetEnabled(value);
            clientTxtSofer.SetEnabled(value);

            if (!value) {
                clientTxtNovaCena.GetMainElement().style.backgroundColor = "LightGray";
                clientTxtNovaCena.GetInputElement().style.backgroundColor = "LightGray";

                lookUpStranke.GetMainElement().style.backgroundColor = "LightGray";
                lookUpStranke.GetInputElement().style.backgroundColor = "LightGray";

                if (!buyerArrangesTransport) {
                    clientTxtRegistracija.GetMainElement().style.backgroundColor = "LightGray";
                    clientTxtRegistracija.GetInputElement().style.backgroundColor = "LightGray";
                    //clientBuyerArrangesTransportCheckBox.SetChecked(value);
                }
                //else
                //clientSupplierArrangesTransportCheckBox.SetEnabled(value);

                clientTxtSofer.GetMainElement().style.backgroundColor = "LightGray";
                clientTxtSofer.GetInputElement().style.backgroundColor = "LightGray";
            }
            else {
                clientTxtNovaCena.GetMainElement().style.backgroundColor = "White";
                clientTxtNovaCena.GetInputElement().style.backgroundColor = "White";

                lookUpStranke.GetMainElement().style.backgroundColor = "White";
                lookUpStranke.GetInputElement().style.backgroundColor = "White";

                clientTxtRegistracija.GetMainElement().style.backgroundColor = "White";
                clientTxtRegistracija.GetInputElement().style.backgroundColor = "White";

                clientTxtSofer.GetMainElement().style.backgroundColor = "White";
                clientTxtSofer.GetInputElement().style.backgroundColor = "White";

                //if (!buyerArrangesTransport)
                //clientBuyerArrangesTransportCheckBox.SetEnabled(value);
                // else
                //clientSupplierArrangesTransportCheckBox.SetEnabled(value);
            }
        }

        function btnAddOrderPosition_Click(s, e) {
            clientLoadingPanel.Show();
            clientCallbackPanelUserInput.PerformCallback("ShowOrderPositionPopUp");
        }

        function OnClosePopUpHandler(command, sender, userAction, recallID) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'OrderPos':
                            clientPopUpOrderPos.Hide();
                            if (userAction !== undefined && recallID !== undefined)
                                window.location.replace("RecallForm.aspx?action=" + userAction + "&recordId=" + recallID);
                            //gridSelectedPositions.Refresh();
                            break;
                        case 'InquirySummary':
                            clientPopUpCarriersInquirySummary.Hide();
                            $('#modal-head').css('background-color', '#88e188');
                            $('#warningModal').modal('show');
                            $('#modalBodyText').empty();
                            $('#modalBodyText').append("Uspešno ste izbrali prevoznika za odpoklic.");
                            $('#myModalTitle').empty();
                            $('#myModalTitle').append("Odlično!");
                            if (userAction !== undefined && recallID !== undefined)
                                window.location.replace("RecallForm.aspx?action=" + userAction + "&recordId=" + recallID);
                            break;
                        case 'CreateOrder':
                            clientPopUpCreateOrder.Hide();
                            $('#modal-head').css('background-color', '#88e188');
                            $('#warningModal').modal('show');
                            $('#modalBodyText').empty();
                            $('#modalBodyText').append("Uspešno ste ustvarili novo naročilo za odpoklic.");
                            $('#myModalTitle').empty();
                            $('#myModalTitle').append("Odlično!");
                            if (recallID !== undefined)
                                window.location.replace("Recall.aspx?recordId=" + recallID);
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'OrderPos':
                            clientPopUpOrderPos.Hide();
                            break;
                        case 'InquirySummary':
                            clientPopUpCarriersInquirySummary.Hide();
                            break;
                        case 'CreateOrder':
                            clientPopUpCreateOrder.Hide();
                            break;
                    }
                    break;
            }
        }

        function gridSelectedPositions_BatchEditStartEditing(s, e) {
            clientBtnSaveChanges.SetEnabled(true);
            clientBtnCancelChanges.SetEnabled(true);
        }

        function btnDeleteSelectedPosition_Init(s, e) {
            if (gridSelectedPositions.GetFocusedRowIndex() >= 0)
                s.SetEnabled(true);
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

        function lookUpTipPrevoza_ValueChanged(s, e) {
            lookUpTipPrevoza.GetGridView().GetRowValues(lookUpTipPrevoza.GetGridView().GetFocusedRowIndex(), 'Koda;DovoljenaTeza', OnGetRowValuesTransportType);
        }
        function OnGetRowValuesTransportType(values) {
            var codeTypePlane = '<%= DatabaseWebService.Common.Enums.Enums.TransportType.LETALO.ToString() %>';
            var codeTypeShip = '<%= DatabaseWebService.Common.Enums.Enums.TransportType.LADJA.ToString() %>';
            transportTypeCode = values[0];

            if ((values[0] == codeTypeShip || values[0] == codeTypePlane) && lookUpTipPrevoza.GetEnabled())
                lookUpSkladisce.SetEnabled(true);
            else
                lookUpSkladisce.SetEnabled(false);

            max = parseInt(values[1]);

            if (parseFloat(labelSum.GetText().replace(".", "")) > max)
                labelSum.GetMainElement().style.color = 'Red';
        }

        function lookUpSkladisce_ValueChanged(s, e) {
            clientLoadingPanel.Show();
            clientCallbackPanelUserInput.PerformCallback('OwnStockWarehouse');
        }

        function btnSplitSelectedPosition_Init(s, e) {
            if (gridSelectedPositions.GetFocusedRowIndex() >= 0)
                s.SetEnabled(true);
        }

        function btnSplitSelectedPosition_Click(s, e) {
            $('#warningButtonModal').modal('show');
            $('#modal-btn-delete').hide();
            $('#modal-btn-confirm').show();
            $('#warningButtonModalBody').empty();
            $('#warningButtonModalBody').append("Izbrano pozicijo naročilnice boste razdelili. Potrdite izbiro!");
            $('#warningButtonModalTitle').empty();
            $('#warningButtonModalTitle').append("Razdeljevanje pozicije!");
        }

        var isBtnConfirmPostbackInitiated = false;
        function btnConfirm_Click(s, e) {
            e.processOnServer = !isBtnConfirmPostbackInitiated;
            isBtnConfirmPostbackInitiated = true;
        }

        var isBtnConfirmRecallPostbackInitiated = false;
        function btnConfirmRecall_Click(s, e) {
            e.processOnServer = !isBtnConfirmRecallPostbackInitiated;
            isBtnConfirmRecallPostbackInitiated = true;
        }

        var isBtnRejectRecallPostbackInitiated = false;
        function btnRejectRecall_Click(s, e) {
            e.processOnServer = !isBtnRejectRecallPostbackInitiated;
            isBtnRejectRecallPostbackInitiated = true;
        }

        var isBtnConfirmTakeOverPostbackInitiated = false;
        function btnConfirmTakeOver_Click(s, e) {
            e.processOnServer = !isBtnConfirmTakeOverPostbackInitiated;
            isBtnConfirmTakeOverPostbackInitiated = true;
        }

        var isBtnReopenRecallPostbackInitiated = false;
        function btnReopenRecall_Click(s, e) {
            e.processOnServer = !isBtnReopenRecallPostbackInitiated;
            isBtnReopenRecallPostbackInitiated = true;
        }

        function lookupRelacija_Valuechanged(s, e) {

            s.GetGridView().GetSelectedFieldValues("ID", GotSelectedValues);
        }

        function CloseGridLookup() {
            lookUpStranke.ConfirmCurrentSelection();
            lookUpStranke.HideDropDown();
            lookUpStranke.Focus();
        }

        function BuyerArrangesTransportCheckBox_CheckChanged(s, e) {
            var value = s.GetChecked();
            if (value) {
                clientLoadingPanel.Show();
                clientCallbackPanelUserInput.PerformCallback('BuyerArrangesTransport');
                clientSupplierArrangesTransportCheckBox.SetChecked(false);
            }
            else {
                EnableUserControls(!value, true);
            }
        }

        var isBtnSendInquiryPostbackInitiated = false;
        function btnSendInquiry_Click(s, e) {
            var process = false;
            var dateItems = [DateEditDatumNaklada];
            var lookUpItems = [lookUpStranke];
            process = InputFieldsValidation(lookUpItems, null, dateItems, null, /*comboBoxItems*/null, null);


            if (process) {
                e.processOnServer = !isBtnSendInquiryPostbackInitiated;
                isBtnSendInquiryPostbackInitiated = true;
            }
            else
                e.processOnServer = false;
        }

        function btnOpenInquirySummaryForCarrier_Click(s, e) {
            clientCallbackPanelUserInput.PerformCallback('OpenCarriersInquiryPopUp');
        }

        function btnCreateOrder_Click(s, e) {
             var process = false;
            var dateItems = [DateEditDatumRazklada];
            var lookUpItems = [lookUpStranke];
            process = InputFieldsValidation(lookUpItems, null, dateItems, null, /*comboBoxItems*/null, null);

            if (process) {
                clientCallbackPanelUserInput.PerformCallback('OpenNewOrderPopUp');
            }
            else
                e.processOnServer = false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="clientLoadingPanel" Modal="true"></dx:ASPxLoadingPanel>
    <dx:ASPxHiddenField runat="server" ClientInstanceName="clientHfCurrentSum" ID="hfCurrentSum">
    </dx:ASPxHiddenField>
    <dx:ASPxHiddenField runat="server" ClientInstanceName="clientHfCurrentSumPalete" ID="hfCurrentSumPalete">
    </dx:ASPxHiddenField>
    <dx:ASPxGridView ID="ASPxGridViewSelectedPositions" runat="server" AutoGenerateColumns="False"
        EnableTheming="True" EnableCallbackCompression="true" ClientInstanceName="gridSelectedPositions"
        Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G"
        KeyFieldName="OdpoklicPozicijaID" Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Paddings-PaddingLeft="3px" Paddings-PaddingRight="3px"
        CssClass="gridview-no-header-padding" OnDataBinding="ASPxGridViewSelectedPositions_DataBinding" OnCellEditorInitialize="ASPxGridViewSelectedPositions_CellEditorInitialize"
        OnBatchUpdate="ASPxGridViewSelectedPositions_BatchUpdate" OnCommandButtonInitialize="ASPxGridViewSelectedPositions_CommandButtonInitialize"
        OnHtmlRowPrepared="ASPxGridViewSelectedPositions_HtmlRowPrepared" OnDataBound="ASPxGridViewSelectedPositions_DataBound">
        <ClientSideEvents SelectionChanged="OnSelectionChanged_gridSelectedPositions" BatchEditStartEditing="gridSelectedPositions_BatchEditStartEditing" />
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

            <dx:GridViewDataTextColumn Caption="Odpoklicana količina iz drugih naročilnic"
                FieldName="KolicinaOTP" ShowInCustomizationForm="True"
                Width="120px" AdaptivePriority="1" EditFormSettings-Visible="False">
                <PropertiesTextEdit DisplayFormatString="n3" DisplayFormatInEditMode="true" />
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Odpoklicana količina na tej naročilnici"
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

         <%--   <dx:GridViewDataTextColumn Caption="Št. palet"
                FieldName="StPalet" ShowInCustomizationForm="True"
                Width="120px" AdaptivePriority="1">
                <PropertiesTextEdit DisplayFormatString="n2" DisplayFormatInEditMode="true">
                    <ValidationSettings Display="Dynamic" />
                </PropertiesTextEdit>
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>--%>

            <dx:GridViewDataTextColumn Caption="Enota mere"
                FieldName="EnotaMere" ShowInCustomizationForm="True"
                Width="80px" EditFormSettings-Visible="False">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Transportna količina"
                FieldName="TransportnaKolicina" ShowInCustomizationForm="True"
                Width="120px" AdaptivePriority="1">
                <PropertiesTextEdit DisplayFormatString="n2" DisplayFormatInEditMode="true">
                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" RegularExpression-ValidationExpression="" />
                </PropertiesTextEdit>
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Število palet"
                FieldName="Palete" ShowInCustomizationForm="True"
                Width="70px" AdaptivePriority="1">
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
                        <span class="AddEditButtons no-margin-top">
                            <dx:ASPxButton ID="btnSplitSelectedPosition" runat="server" Text="Razdeli izbrano pozicijo" AutoPostBack="false" CssClass="statusBarButton orange"
                                Height="40" Width="110" UseSubmitBehavior="false" ClientEnabled="false" ClientInstanceName="clientBtnSplitSelectedPosition">
                                <ClientSideEvents Click="btnSplitSelectedPosition_Click" Init="btnSplitSelectedPosition_Init" />
                                <DisabledStyle CssClass="statusBarButtonsDisabled"></DisabledStyle>
                            </dx:ASPxButton>
                        </span>
                    </div>
                    <div class="col-xs-6 text-right">
                        <span class="AddEditButtons no-margin-top">
                            <dx:ASPxButton ID="btnSaveChanges" runat="server" Text="Spremeni" AutoPostBack="false" CssClass="statusBarButton"
                                Height="40" Width="110" UseSubmitBehavior="false" ClientEnabled="false" ClientInstanceName="clientBtnSaveChanges">
                                <DisabledStyle CssClass="statusBarButtonsDisabled" />
                                <ClientSideEvents Click="function(s,e) { gridSelectedPositions.UpdateEdit(); }" />
                            </dx:ASPxButton>
                        </span>
                        <span class="AddEditButtons no-margin-top">
                            <dx:ASPxButton ID="btnCancelChanges" runat="server" Text="Prekliči" AutoPostBack="false" CssClass="statusBarButton"
                                Height="40" Width="110" UseSubmitBehavior="false" ClientEnabled="false" ClientInstanceName="clientBtnCancelChanges">
                                <DisabledStyle CssClass="statusBarButtonsDisabled" />
                                <ClientSideEvents Click="function(s,e) { gridSelectedPositions.CancelEdit(); }" />
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
                                    CssClass="text-box-input" Font-Size="14px" ReadOnly="true" BackColor="LightGray" Width="100%" Font-Bold="true">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-5">
                        <div class="row2 align-item-centerV-endH">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 30px;">
                                <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="DOBAVITELJ : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-7 no-padding-left">
                                <dx:ASPxTextBox runat="server" ID="txtDobaviteljNaziv" ClientEnabled="false" ClientInstanceName="clientTxtStOdpoklic"
                                    CssClass="text-box-input" Font-Size="14px" ReadOnly="true" BackColor="LightGray" Width="100%" Font-Bold="true">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row small-padding-bottom">
                    <div class="col-md-3">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-sm-0 big-margin-r">
                                <dx:ASPxLabel ID="lblTransportType" runat="server" Text="TIP PREVOZA : " ClientInstanceName="clientLblTransportType"></dx:ASPxLabel>
                            </div>
                            <div class="col-sm-6 no-padding-left">
                                <dx:ASPxGridLookup ID="ASPxGridLookupTipPrevoza" runat="server" ClientInstanceName="lookUpTipPrevoza"
                                    KeyFieldName="TipPrevozaID" TextFormatString="{2}" CssClass="text-box-input" ClientEnabled="true"
                                    Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                    OnLoad="ASPxGridLookupLoad_WidthMedium" OnDataBinding="ASPxGridLookupTipPrevoza_DataBinding" IncrementalFilteringMode="Contains">
                                    <ClearButton DisplayMode="OnHover" />
                                    <%-- DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Naziv').Focus();} --%>
                                    <ClientSideEvents Init="SetFocus" ValueChanged="lookUpTipPrevoza_ValueChanged" />
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
                                        <dx:GridViewDataTextColumn Caption="Id" FieldName="TipPrevozaID" Width="80px"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Koda" FieldName="Koda" Width="80px"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="50%"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Dovoljena teža"
                                            FieldName="DovoljenaTeza" ShowInCustomizationForm="True"
                                            Width="30%" PropertiesTextEdit-DisplayFormatString="N2">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                    </Columns>
                                </dx:ASPxGridLookup>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="row2 align-item-centerV-centerH">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 30px">
                                <dx:ASPxLabel ID="lblSKladisce" runat="server" Text="SKLADIŠČE : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-8 no-padding-left">
                                <dx:ASPxGridLookup ID="ASPxGridLookupSkladisce" runat="server" ClientInstanceName="lookUpSkladisce"
                                    KeyFieldName="idStranka" TextFormatString="{1}" CssClass="text-box-input" ClientEnabled="false"
                                    Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                    OnLoad="ASPxGridLookupLoad_WidthLarge" OnDataBinding="ASPxGridLookupSkladisce_DataBinding" IncrementalFilteringMode="Contains">
                                    <ClearButton DisplayMode="OnHover" />
                                    <%-- DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('NazivPrvi').Focus();}" --%>
                                    <ClientSideEvents Init="SetFocus" ValueChanged="lookUpSkladisce_ValueChanged" />
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
                                        <dx:GridViewDataTextColumn Caption="Id" FieldName="idStranka" Width="80px"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="NazivPrvi" Width="80%"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Naslov"
                                            FieldName="Naslov" ShowInCustomizationForm="True"
                                            Width="20%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Poštna štev."
                                            FieldName="StevPoste" ShowInCustomizationForm="True"
                                            Width="20%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Pošta"
                                            FieldName="NazivPoste" ShowInCustomizationForm="True"
                                            Width="20%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                    </Columns>
                                </dx:ASPxGridLookup>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="row2 align-item-centerV-endH">
                            <div class="col-xs-0 big-margin-r no-padding-right">
                                <dx:ASPxLabel ID="lblSupplierArrangesTransport" runat="server" Text="DOBAVITELJ PREVOZ : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-1 no-padding-left">
                                <dx:ASPxCheckBox ID="SupplierArrangesTransportCheckBox2" runat="server" ClientInstanceName="clientSupplierArrangesTransportCheckBox">
                                    <ClientSideEvents CheckedChanged="SupplierArrangesTransportCheckBox_CheckChanged" />
                                </dx:ASPxCheckBox>
                            </div>

                            <div class="col-xs-0 big-margin-r no-padding-right">
                                <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="LASTEN PREVOZ : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-1 no-padding-left no-padding-right">
                                <dx:ASPxCheckBox ID="CheckBoxLastenPrevoz" runat="server" ClientInstanceName="clientCheckBoxLastnaZaloga">
                                </dx:ASPxCheckBox>
                            </div>
                            <div class="col-xs-0 big-margin-r no-padding-right">
                                <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="KUPEC PREVOZ : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-1 no-padding-left no-padding-right">
                                <dx:ASPxCheckBox ID="BuyerArrangesTransportCheckBox2" runat="server" ClientInstanceName="clientBuyerArrangesTransportCheckBox">
                                    <ClientSideEvents CheckedChanged="BuyerArrangesTransportCheckBox_CheckChanged" />
                                </dx:ASPxCheckBox>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="row small-padding-bottom">
                    <div class="col-md-6">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 38px">
                                <dx:ASPxLabel ID="lblRelacija" runat="server" Text="RELACIJA : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-10 no-padding-left">
                                <dx:ASPxGridLookup ID="ASPxGridLookupRealacija" runat="server" ClientInstanceName="lookUpRelacija"
                                    KeyFieldName="RelacijaID" TextFormatString="{1}" CssClass="text-box-input"
                                    Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                    OnLoad="ASPxGridLookupLoad_WidthLarge" OnDataBinding="ASPxGridLookupRealacija_DataBinding" IncrementalFilteringMode="Contains">
                                    <ClearButton DisplayMode="OnHover" />
                                    <%-- DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Naziv').Focus();}" --%>
                                    <ClientSideEvents Init="SetFocus" ValueChanged="function(s,e){clientLoadingPanel.Show(); clientCallbackPanelUserInput.PerformCallback('DataBindPrevoznik');}" />
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
                    </div>

                    <div class="col-md-6">
                        <div class="row2 align-item-centerV-endH">
                            <div class="col-xs-0 big-margin-r no-padding-right">
                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="CENA : ">
                                </dx:ASPxLabel>
                            </div>
                            <div class="col-xs-3 no-padding-left" style="align-items: flex-end">
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
                    <div class="col-md-6">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 22px;">
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="PREVOZNIK : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-10 no-padding-left">
                                <dx:ASPxGridLookup ID="ASPxGridLookupStranke" runat="server" ClientInstanceName="lookUpStranke"
                                    KeyFieldName="RazpisPozicijaID" TextFormatString="{1}" CssClass="text-box-input"
                                    Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                    OnLoad="ASPxGridLookupLoad_WidthXtraLarge" OnDataBinding="ASPxGridLookupStranke_DataBinding" IncrementalFilteringMode="Contains"
                                    SelectionMode="Multiple" MultiTextSeparator="; ">
                                    <ClearButton DisplayMode="OnHover" />
                                    <%-- DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Stranka.NazivPrvi').Focus();}" --%>
                                    <ClientSideEvents ValueChanged="ValueChanged_lookUpStranke" />
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
                                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="60px" />
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
                                                            <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" UseSubmitBehavior="false" Text="Potrdi in zapri" ClientSideEvents-Click="CloseGridLookup"
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
                            <div class="col-xs-12 no-padding-left" style="align-items: flex-end;">
                                <dx:ASPxMemo runat="server" Rows="4" Width="60%" NullText="Vpiši komentar..." ClientVisible="false" ID="memoKomentar" MaxLength="5000"
                                    ClientInstanceName="clientMemoKomentar" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row small-padding-bottom">
                    <div class="col-md-12">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 30px">
                                <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="OPOMBA POVPRAŠEVANJA ZA PREVOZNIKA: "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-12 no-padding-left">
                                <dx:ASPxMemo runat="server" Rows="4" Width="100%" NullText="Vpiši komentar..." ID="memOpombaPrevoznikov"
                                    ClientInstanceName="clientMemoOpombaPrevoznikov" MaxLength="4000" CssClass="text-box-input">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxMemo>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row small-padding-bottom">
                    <div class="col-md-6">
                        <div class="row2" style="align-items: center;">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 53px;">
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
                    <div class="col-md-6">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 20px;">
                                <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="DATUM NAKLADA : " Width="80px"></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-9 no-padding-left">
                                <dx:ASPxDateEdit ID="DateEditDatumNaklada" runat="server" EditFormat="Date" Width="170" Theme="Moderno"
                                    CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="DateEditDatumNaklada">
                                    <FocusedStyle CssClass="focus-text-box-input" />
                                    <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                    <DropDownButton Visible="true"></DropDownButton>
                                </dx:ASPxDateEdit>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 20px;">
                                <dx:ASPxLabel ID="lblDatRazklada" runat="server" Text="DATUM RAZKLADA : " Width="80px"></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-9 no-padding-left">
                                <dx:ASPxDateEdit ID="DateEditDatumRazklada" runat="server" EditFormat="Date" Width="170" Theme="Moderno"
                                    CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="DateEditDatumRazklada">
                                    <FocusedStyle CssClass="focus-text-box-input" />
                                    <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                    <DropDownButton Visible="true"></DropDownButton>
                                </dx:ASPxDateEdit>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="row2 align-item-centerV-endH">
                            <div class="col-xs-0 big-margin-r no-padding-right">
                                <dx:ASPxLabel ID="lblRazlogOdobritveSistem" runat="server" Text="RAZLOG ZA ODOBRITEV SISTEM : " ClientVisible="false"></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-7 no-padding-left">
                                <dx:ASPxTextBox runat="server" ID="txtRazlogOdobritveSistem" ClientInstanceName="clientTxtRazlogOdobritveSistem"
                                    CssClass="text-box-input" Font-Size="14px" Width="100%" MaxLength="500" AutoCompleteType="Disabled"
                                    BackColor="LightGray" Enabled="false" ClientVisible="false" Font-Bold="true" ForeColor="Tomato">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row medium-padding-bottom">
                    <div class="col-md-12">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 30px">
                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="OPOMBA : "></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-12 no-padding-left">
                                <dx:ASPxMemo runat="server" Rows="4" Width="100%" NullText="Vpiši komentar..." ID="ASPxMemoOpombe"
                                    ClientInstanceName="clientMemoOpomba" MaxLength="5000" CssClass="text-box-input">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxMemo>
                            </div>
                        </div>
                    </div>
                </div>


                <dx:ASPxPopupControl ID="ASPxPopupControlOrderPos" runat="server" ContentUrl="OrderPos_popup.aspx"
                    ClientInstanceName="clientPopUpOrderPos" Modal="True" HeaderText="POZICIJE NAROČILNIC"
                    CloseAction="CloseButton" Width="1600px" Height="610px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true"
                    OnWindowCallback="ASPxPopupControlOrderPos_WindowCallback">
                    <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                    <ContentStyle BackColor="#F7F7F7">
                        <Paddings PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px" PaddingTop="0px"></Paddings>
                    </ContentStyle>
                </dx:ASPxPopupControl>

                <dx:ASPxPopupControl ID="ASPxPopupControlCarriersInquirySummary" runat="server" ContentUrl="CarriersInquirySummary_popup.aspx"
                    ClientInstanceName="clientPopUpCarriersInquirySummary" Modal="True" HeaderText="PREGLED POSLANIH POVPRAŠEVANJ"
                    CloseAction="CloseButton" Width="1400px" Height="700px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true"
                    OnWindowCallback="ASPxPopupControlCarriersInquirySummary_WindowCallback">
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
                            <ClientSideEvents Click="btnConfirm_Click" />
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
                            <ClientSideEvents Click="btnConfirmRecall_Click" />
                        </dx:ASPxButton>
                    </span>

                    <span class="AddEditButtons">
                        <dx:ASPxButton ID="btnRejectRecall" runat="server" Text="Zavrni odpoklic" AutoPostBack="false"
                            Height="25" Width="110" UseSubmitBehavior="false" OnClick="btnRejectRecall_Click" Font-Bold="true" ClientVisible="false">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../../../Images/reject.png" UrlHottracked="../../Images/rejectHover.png" />
                            <ClientSideEvents Click="btnRejectRecall_Click" />
                        </dx:ASPxButton>
                    </span>

                    <span class="AddEditButtons">
                        <dx:ASPxButton ID="btnConfirmTakeOver" runat="server" Text="Potrdi prevzem" AutoPostBack="false"
                            Height="50" Width="110" UseSubmitBehavior="false" Font-Bold="true" ClientVisible="false" ClientInstanceName="clientBtnConfirmTakeOver"
                            OnClick="btnConfirmTakeOver_Click">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../../../Images/prevzem.png" UrlHottracked="../../Images/prevzemHover.png" />
                            <ClientSideEvents Click="btnConfirmTakeOver_Click" />
                        </dx:ASPxButton>
                    </span>

                    <span class="AddEditButtons">
                        <dx:ASPxButton ID="btnReopenRecall" runat="server" Text="Odpri odpoklic" AutoPostBack="false"
                            Height="50" Width="110" UseSubmitBehavior="false" ClientVisible="false" ClientInstanceName="clientBtnReopenRecall"
                            OnClick="btnReopenRecall_Click">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../../Images/lock.png" UrlHottracked="../../Images/lockHover.png" />
                            <ClientSideEvents Click="btnReopenRecall_Click" />
                        </dx:ASPxButton>
                    </span>
                    <span class="AddEditButtons">
                        <dx:ASPxButton ID="btnSendInquiry" runat="server" Text="Pošlji povpraševanje" AutoPostBack="false"
                            Height="50" Width="110" UseSubmitBehavior="false" ClientVisible="false" ClientInstanceName="clientBtnSendInquiry"
                            OnClick="btnSendInquiry_Click">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../../Images/sendMailToCarriers.png" UrlHottracked="../../Images/sendMailToCarriersHover.png" />
                            <ClientSideEvents Click="btnSendInquiry_Click" />
                        </dx:ASPxButton>
                    </span>

                    <span class="AddEditButtons">
                        <dx:ASPxButton ID="btnOpenInquirySummaryForCarrier" runat="server" Text="Preglej razpise prevoznikov" AutoPostBack="false"
                            Height="50" Width="110" UseSubmitBehavior="false" ClientVisible="false" ClientInstanceName="btnOpenInquirySummaryForCarrier">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../../Images/sendMailToCarriers.png" UrlHottracked="../../Images/sendMailToCarriersHover.png" />
                            <ClientSideEvents Click="btnOpenInquirySummaryForCarrier_Click" />
                        </dx:ASPxButton>
                    </span>

                    <span class="AddEditButtons">
                        <dx:ASPxButton ID="btnCreateOrder" runat="server" Text="Ustvari naročilo" AutoPostBack="false"
                            Height="50" Width="110" UseSubmitBehavior="false" ClientVisible="false" ClientInstanceName="btnCreateOrder">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../../Images/createOrder.png" UrlHottracked="../../Images/createOrderHover.png" />
                            <ClientSideEvents Click="btnCreateOrder_Click" />
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
