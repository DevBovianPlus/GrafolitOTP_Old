<%@ Page Title="Razpisi" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Tender.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Tender.Tender" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script>
        function ShowPopUp(s, e) {
            parameter = HandleUserActionsOnTabs(gridTender, clientBtnAdd, null, clientBtnDelete, s);
            clientLoadingPanel.Show();
            clientCallbackPanelTender.PerformCallback(parameter);
        }

        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'Tender':
                            clientPopUpTender.Hide();
                            clientLoadingPanel.Show();
                            clientCallbackPanelTender.PerformCallback("RefreshGrid");
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'Tender':
                            clientPopUpTender.Hide();
                    }
                    break;
            }
        }

        function clientBtnUploadExcel_Click(s, e) {
            clientBtnUploadExcel.SetEnabled(false);
            clientLoadingPanel.Show();
            clientCallbackPanelTender.PerformCallback("UploadExcel");
        }

        function clientCallbackPanelTender_EndCallback(s, e) {
            if (s.cpExcelError != "" && s.cpExcelError !== undefined) {

                delete (s.cpExcelError);
            }
            else if (s.cpExcelConfirmImport != "" && s.cpExcelConfirmImport !== undefined) {
                //show pop up on which position have values changed. And than notify user that he has to confirm imported data
                //if he doesn't confirm import on popup than there should be visible buttons on detailRow for import confirmation
                delete (s.cpExcelConfirmImport);
            }

            clientLoadingPanel.Hide();
        }



        var uploadedFiles = [];
        var errorText;
        function onFileUploadComplete(s, e) {
            uploadedFiles.push(e.callbackData);
            errorText = e.errorText;
        }
        function onFileUploadStart(s, e) {
            UploadedFilesTokenBox.SetIsValid(true);
        }
        function onFilesUploadComplete(s, e) {
            for (var i = 0; i < uploadedFiles.length; i++)
                UploadedFilesTokenBox.AddToken(uploadedFiles[i]);
            updateTokenBoxVisibility();
            uploadedFiles = [];

            if (errorText != "" && errorText !== undefined) {
                $('#unhandledExpModalContentBody').append(errorText);
                $('#unhandledExpModal').modal('show');
                UploadedFilesTokenBox.ClearTokenCollection();
            }
            else {
                clientBtnUploadExcel.SetEnabled(true);
                $('#successModal').modal('show');
            }

            gridTenderPosition.Refresh();
        }

        function onTokenBoxValidation(s, e) {
            var isValid = clientDocumentsUploadControl.GetText().length > 0 || UploadedFilesTokenBox.GetText().length > 0;
            e.isValid = isValid;
            if (!isValid) {
                e.errorText = "Naloži datoteko";
            }
        }
        function onTokenBoxValueChanged(s, e) {
            updateTokenBoxVisibility();
        }
        function updateTokenBoxVisibility() {
            var isTokenBoxVisible = UploadedFilesTokenBox.GetTokenCollection().length > 0;
            UploadedFilesTokenBox.SetVisible(isTokenBoxVisible);
        }

        function btnAddNew_Click(s, e) {
            clientLoadingPanel.Show();
            clientCallbackPanelTender.PerformCallback("AddNewTenderPosition");
        }

        function gridTenderPosition_SelectionChanged(s, e) {
            var selectedRowCount = s.GetSelectedRowCount();

            if (selectedRowCount > 0)
                clientBtnDeleteSelected.SetEnabled(true);
            else
                clientBtnDeleteSelected.SetEnabled(false);
        }

        function btnDeleteSelected_Click(s, e) {
            gridTenderPosition.GetSelectedFieldValues('RazpisPozicijaID', OnGetSelectedFieldValues);
        }

        function OnGetSelectedFieldValues(value) {
            clientLoadingPanel.Show();
            clientCallbackPanelTender.PerformCallback("DeleteSelected;" + value);
        }

        function OnDetailRowExpanding(s, e) {
            gridTender.SetFocusedRowIndex(e.visibleIndex);
            clientCallbackPanelTender.PerformCallback("SelectPozicija;" + e.visibleIndex);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="clientLoadingPanel" Modal="true"></dx:ASPxLoadingPanel>
    <dx:ASPxCallbackPanel ID="CallbackPanelTender" runat="server" Width="100%" OnCallback="CallbackPanelTender_Callback"
        ClientInstanceName="clientCallbackPanelTender">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="clientCallbackPanelTender_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="col-md-12" style="margin-bottom: 10px">
                    <div class="row2 align-item-centerV-startH">
                        <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 20px;">
                            <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="DATUM Od : " Width="80px"></dx:ASPxLabel>
                        </div>
                        <div class="col-xs-0 no-padding-left">
                            <dx:ASPxDateEdit ID="DateEditDatumOd" runat="server" EditFormat="Date" Width="170" Theme="Moderno"
                                CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="DateEditDatumOd">

                                <FocusedStyle CssClass="focus-text-box-input" />
                                <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                <DropDownButton Visible="true"></DropDownButton>
                            </dx:ASPxDateEdit>
                        </div>

                        <div class="col-xs-0" style="margin-right: 20px; margin-left: 20px;">
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="DATUM Do : " Width="80px"></dx:ASPxLabel>
                        </div>
                        <div class="col-xs-0" style="margin-right: 30px;">
                            <dx:ASPxDateEdit ID="DateEditDatumDo" runat="server" EditFormat="Date" Width="170" Theme="Moderno"
                                CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="DateEditDatumDo">
                                <FocusedStyle CssClass="focus-text-box-input" />
                                <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                <DropDownButton Visible="true"></DropDownButton>
                            </dx:ASPxDateEdit>
                        </div>
                        <div class="col-xs-0" style="margin-left: 20px; margin-top: -14px">
                            <dx:ASPxButton Theme="Moderno" ID="btnFilter" runat="server" Text="Filter" AutoPostBack="false"
                                Height="15" Width="90" ClientVisible="true" OnClick="btnFilter_Click">
                                <Image Url="../../../Images/magnifier.png" UrlHottracked="../../Images/magnifier.png" />
                            </dx:ASPxButton>
                        </div>
                    </div>
                    <div class="row2 align-item-centerV-startH" style="margin-top: 10px">
                        <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 47px;">
                            <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Relacija : "></dx:ASPxLabel>
                        </div>
                        <div class="col-xs-4 no-padding-left">
                            <dx:ASPxTextBox runat="server" ID="txtRelacija" ClientInstanceName="clientTxtRelacija" CssClass="text-box-input" Font-Size="14px" Width="80%">
                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </div>
                </div>
                <dx:ASPxGridView ID="ASPxGridViewTender" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridTender"
                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewTender_DataBinding" OnSelectionChanged="ASPxGridViewTender_SelectionChanged"
                    KeyFieldName="RazpisID" OnHtmlRowPrepared="ASPxGridViewTender_HtmlRowPrepared" OnCustomColumnDisplayText="ASPxGridViewTender_CustomColumnDisplayText" AllowOnlyOneMasterRowExpanded="true">
                    <Paddings Padding="0" />
                    <ClientSideEvents DetailRowExpanding="OnDetailRowExpanding" />
                    <Settings ShowVerticalScrollBar="True"
                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="600"
                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                        <PageSizeItemSettings Visible="true" Items="50, 70, 100" Caption="Zapisi na stran : " AllItemText="Vsi">
                        </PageSizeItemSettings>
                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                    </SettingsPager>
                    <SettingsBehavior AllowFocusedRow="true" />
                    <Styles Header-Wrap="True">
                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                    </Styles>
                    <SettingsText EmptyDataRow="Trenutno ni podatka o razpisih. Dodaj novega." />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="RazpisID" Width="80px"
                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True" SortOrder="Descending">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Izdelano" FieldName="GeneriranTender" MinWidth="70" MaxWidth="250" Width="2%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="55%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Cena"
                            FieldName="CenaSkupaj" ShowInCustomizationForm="True"
                            Width="25%" PropertiesTextEdit-DisplayFormatString="N3" Visible="false">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataDateColumn Caption="Datum razpisa" FieldName="DatumRazpisa" Width="13%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy HH:mm:ss" />
                        </dx:GridViewDataDateColumn>
                    </Columns>
                    <Templates>
                        <DetailRow>
                            <div class="row2">
                                <div class="col-xs-8">
                                    <dx:ASPxGridView ID="ASPxGridViewTenderPosition" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridTenderPosition"
                                        Width="100%" EnablePagingGestures="False" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewTenderPosition_DataBinding"
                                        KeyFieldName="RazpisPozicijaID" OnBeforePerformDataSelect="ASPxGridViewTenderPosition_BeforePerformDataSelect"
                                        OnHtmlRowPrepared="ASPxGridViewTenderPosition_HtmlRowPrepared" OnBatchUpdate="ASPxGridViewTenderPosition_BatchUpdate">
                                        <ClientSideEvents SelectionChanged="gridTenderPosition_SelectionChanged" />
                                        <Paddings Padding="0" />
                                        <Settings ShowVerticalScrollBar="True"
                                            ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="150"
                                            ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                        <SettingsPager PageSize="10" ShowNumericButtons="true">
                                            <PageSizeItemSettings Visible="true" Items="10,20,30" Caption="Zapisi na stran : " AllItemText="Vsi">
                                            </PageSizeItemSettings>
                                            <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                        </SettingsPager>
                                        <SettingsBehavior AllowFocusedRow="true" AllowEllipsisInText="true" />
                                        <Styles Header-Wrap="True">
                                            <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                            <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                        </Styles>
                                        <SettingsText EmptyDataRow="Trenutno ni podatka o pozicijah razpisa." CommandBatchEditUpdate="Spremeni" CommandBatchEditCancel="Prekliči" />
                                        <SettingsEditing Mode="Batch" BatchEditSettings-StartEditAction="DblClick" />
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowSelectCheckbox="true" Caption="Izberi" Width="60px">
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn Caption="Prevoznik" FieldName="Stranka.NazivPrvi" Width="35%"
                                                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Relacija" FieldName="Relacija.Naziv" Width="35%"
                                                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Teža v t" FieldName="ZbirnikTon.Koda" Width="8%"
                                                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Cena"
                                                FieldName="Cena" ShowInCustomizationForm="True"
                                                Width="15%" PropertiesTextEdit-DisplayFormatString="N3">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                <PropertiesTextEdit DisplayFormatInEditMode="true">
                                                    <ValidationSettings>
                                                        <RegularExpression ErrorText="Vnesi ceno" ValidationExpression="^\d+(.\d+){0,1}$" />
                                                    </ValidationSettings>
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:ASPxGridView>
                                </div>
                                <div class="col-xs-4">
                                    <dx:ASPxFormLayout ID="FormLayout" runat="server" Width="100%" ColCount="2" UseDefaultPaddings="false">
                                        <Items>
                                            <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Width="100%" UseDefaultPaddings="false">
                                                <Items>
                                                    <dx:LayoutGroup Caption="Dokumenti">
                                                        <Items>
                                                            <dx:LayoutItem ShowCaption="False">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer>
                                                                        <div id="dropZone">
                                                                            <dx:ASPxUploadControl runat="server" ID="DocumentsUploadControl" ClientInstanceName="clientDocumentsUploadControl" Width="100%"
                                                                                AutoStartUpload="true" ShowProgressPanel="True" ShowTextBox="false" BrowseButton-Text="Dodaj dokumente" FileUploadMode="OnPageLoad"
                                                                                OnFileUploadComplete="DocumentsUploadControl_FileUploadComplete">
                                                                                <AdvancedModeSettings EnableMultiSelect="true" EnableDragAndDrop="true" ExternalDropZoneID="dropZone" />
                                                                                <ValidationSettings
                                                                                    AllowedFileExtensions=".xls, .xlsx"
                                                                                    MaxFileSize="4194304">
                                                                                </ValidationSettings>
                                                                                <ClientSideEvents
                                                                                    FileUploadComplete="onFileUploadComplete"
                                                                                    FilesUploadComplete="onFilesUploadComplete"
                                                                                    FilesUploadStart="onFileUploadStart" />
                                                                            </dx:ASPxUploadControl>
                                                                            <br />
                                                                            <dx:ASPxTokenBox runat="server" Width="100%" ID="UploadedFilesTokenBox" ClientInstanceName="UploadedFilesTokenBox"
                                                                                NullText="Select the documents to submit" AllowCustomTokens="false" ClientVisible="false">
                                                                                <ClientSideEvents Init="updateTokenBoxVisibility" ValueChanged="onTokenBoxValueChanged" Validation="onTokenBoxValidation" />
                                                                                <ValidationSettings EnableCustomValidation="true" />
                                                                            </dx:ASPxTokenBox>
                                                                            <br />
                                                                            <p class="Note">
                                                                                <dx:ASPxLabel ID="AllowedFileExtensionsLabel" runat="server" Text="Dovoljene datoteke za nalaganje: .xls, .xlsx" Font-Size="8pt" />
                                                                                <br />
                                                                                <dx:ASPxLabel ID="MaxFileSizeLabel" runat="server" Text="Max velikost datoteke: 4 MB." Font-Size="8pt" />
                                                                            </p>
                                                                            <dx:ASPxValidationSummary runat="server" ID="ValidationSummary" ClientInstanceName="ValidationSummary"
                                                                                RenderMode="Table" Width="250px" ShowErrorAsLink="false" />
                                                                        </div>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                        </Items>
                                                    </dx:LayoutGroup>
                                                    <dx:LayoutItem ShowCaption="False" HorizontalAlign="Right">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer>
                                                                <span class="AddEditButtons">
                                                                    <dx:ASPxButton ID="btnUploadExcel" runat="server" Text="Potrdi naložene dokumente" AutoPostBack="false"
                                                                        Height="25" Width="50" ClientInstanceName="clientBtnUploadExcel" ClientEnabled="false">
                                                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                                                        <ClientSideEvents Click="clientBtnUploadExcel_Click" />
                                                                    </dx:ASPxButton>
                                                                </span>

                                                                <span class="AddEditButtons">
                                                                    <dx:ASPxButton ID="btnAddNew" runat="server" Text="Dodaj novo" AutoPostBack="false"
                                                                        Height="25" Width="50" ClientInstanceName="clientBtnAddNew">
                                                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                                                        <ClientSideEvents Click="btnAddNew_Click" />
                                                                    </dx:ASPxButton>
                                                                </span>
                                                                <span class="AddEditButtons">
                                                                    <dx:ASPxButton ID="btnDeleteSelected" runat="server" Text="Izbriši izbrane" AutoPostBack="false"
                                                                        Height="25" Width="50" ClientInstanceName="clientBtnDeleteSelected" ClientEnabled="false">
                                                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                                                        <ClientSideEvents Click="btnDeleteSelected_Click" />
                                                                    </dx:ASPxButton>
                                                                </span>
                                                                <span class="AddEditButtons">
                                                                    <dx:ASPxButton ID="btnDownloadTender" runat="server" Text="Prenos razpisa" AutoPostBack="false"
                                                                        Height="25" Width="50" ClientInstanceName="clientBtnDownloadTender" ClientEnabled="true" OnClick="btnDownloadTender_Click">
                                                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                                                    </dx:ASPxButton>
                                                                </span>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                    </dx:LayoutItem>
                                                    <dx:EmptyLayoutItem Height="5" />
                                                </Items>
                                            </dx:LayoutGroup>
                                        </Items>
                                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
                                    </dx:ASPxFormLayout>
                                </div>
                            </div>
                        </DetailRow>
                    </Templates>
                    <SettingsDetail ShowDetailRow="true" AllowOnlyOneMasterRowExpanded="true" />
                </dx:ASPxGridView>

                <dx:ASPxPopupControl ID="ASPxPopupControlTender" runat="server" ContentUrl="Tender_popup.aspx"
                    ClientInstanceName="clientPopUpTender" Modal="True" HeaderText="RAZPIS"
                    CloseAction="CloseButton" Width="690px" Height="500px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true"
                    OnWindowCallback="ASPxPopupControlTender_WindowCallback">
                    <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                    <ContentStyle BackColor="#F7F7F7">
                        <Paddings PaddingBottom="0px" PaddingLeft="6px" PaddingRight="6px" PaddingTop="0px"></Paddings>
                    </ContentStyle>
                </dx:ASPxPopupControl>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
        <div class="DeleteButtonElements">
            <span class="AddEditButtons"></span>
        </div>
        <div class="AddEditButtonsElements">
            <span class="AddEditButtons">
                <dx:ASPxButton Theme="Moderno" ID="btnPrenosTender" runat="server" Text="Prenos razpisa" AutoPostBack="false"
                    Height="25" Width="90" ClientInstanceName="clientBtnPrenosTender" ClientVisible="true" OnClick="btnPrenosTender_Click">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/prevzem.png" UrlHottracked="../../Images/prevzemHover.png" />
                </dx:ASPxButton>
            </span>
            <span class="AddEditButtons">
                <dx:ASPxButton Theme="Moderno" ID="btnPosljiTender" runat="server" Text="Pošlji razpis" AutoPostBack="false"
                    Height="25" Width="50" ClientInstanceName="clientBtnPosljiTender" ClientVisible="true" OnClick="btnPosljiTender_Click">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/sendMailToCarriers.png" UrlHottracked="../../Images/sendMailToCarriersHover.png" />
                </dx:ASPxButton>
            </span>
        </div>
    </div>

    <!-- Session end Modal -->
    <div id="successModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: yellow; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="material-icons" style="font-size: 48px; color: white">warning</i></div>
                </div>
                <div class="modal-body text-center">
                    <h3>Uspešno nalaganje dokumenta!</h3>
                    <p>Prosim potrdite ali zavrzite naložene cene za uspešno shranjevanje razpisa!</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Zapri</button>
                </div>
            </div>

        </div>
    </div>

    <!-- Unhandled exception Modal -->
    <div id="unhandledExpModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: red; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="material-icons" style="font-size: 48px; color: white">error_outline</i></div>
                </div>
                <div class="modal-body text-center">
                    <h3>Napaka!</h3>
                    <p id="unhandledExpModalContentBody"></p>
                    <dx:ASPxHiddenField ID="HiddenUnhandledExpField" runat="server" ClientInstanceName="clientHiddenUnhandledExpField"></dx:ASPxHiddenField>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Zapri</button>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
