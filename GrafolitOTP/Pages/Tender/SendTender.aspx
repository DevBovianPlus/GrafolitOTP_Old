<%@ Page Title="Pošiljanje razpisov" Async="true" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SendTender.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Tender.SendTender" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script>
        var firstShow = true;

        $(document).ready(function () {
            clientLoadingPanel.Hide();

            $("#modal-btn-download").on("click", function () {
                $("#warningButtonModal").modal('hide');
                firstShow = false;
                btnConfirmDownload.DoClick();
            });
        });

        function OnSelectionChanged(s, e) {
            if (gridRoutes.GetSelectedRowCount() > 0 && gridCarrier.GetSelectedRowCount() > 0) {
                btnSend.SetEnabled(true);
            }
            else
                btnSend.SetEnabled(false);
        }
        function EndCallback_CallbackPanelSendTenders(s, e) {
            if (s.cpSendTender != null && s.cpSendTender !== undefined) {
                ShowErrorPopUp("Ustvarjenih je bilo " + s.cpSendTender + " novih razpisov, ki še niso dopolnjeni z cenami", 0, "Razpisi");
                gridRoutes.UnselectRows();
                gridCarrier.UnselectRows();
                delete (s.cpSendTender);
            }

            clientLoadingPanel.Hide();
        }

        function SendTenders(s, e) {
            clientPopupCompleteTenderData.Show();
            //$("#saveKVPModal").modal('show');
            //clientCallbackPanelSendTenders.PerformCallback('');
        }

        
        function SaveTender(s, e)
        {
            var inputItems = [clientTxtTenderName];
            var dateEditItems = [clientDateEditTenderDate];

            process = InputFieldsValidation(null, inputItems, dateEditItems, null, null, null);

            if (btnSave.GetText() == 'Izbriši')
                process = true;

            if (process) {
                clientPopupCompleteTenderData.Hide();
                clientLoadingPanel.Show();
                e.processOnServer = true;

                //clientCallbackPanelSendTenders.PerformCallback('');
            }
            else
                e.processOnServer = false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="clientLoadingPanel" Modal="true"></dx:ASPxLoadingPanel>
    <dx:ASPxCallbackPanel ID="CallbackPanelSendTenders" runat="server" Width="100%" ClientInstanceName="clientCallbackPanelSendTenders"
        OnCallback="CallbackPanelSendTenders_Callback">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="EndCallback_CallbackPanelSendTenders" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="row2">
                    <div class="col-md-6">
                        <h2 class="text-center">Relacije</h2>
                        <hr />
                        <dx:ASPxGridView ID="ASPxGridViewRoutes" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridRoutes"
                            Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewRoutes_DataBinding"
                            KeyFieldName="RelacijaID" CssClass="gridview-no-header-padding">
                            <ClientSideEvents SelectionChanged="OnSelectionChanged" />
                            <Paddings Padding="0" />
                            <Settings ShowVerticalScrollBar="True"
                                ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400"
                                ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                            <SettingsPager PageSize="50" ShowNumericButtons="true">
                                <PageSizeItemSettings Visible="false" Items="10,20,30" Caption="Zapisi na stran : " AllItemText="Vsi">
                                </PageSizeItemSettings>
                                <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                            </SettingsPager>
                            <SettingsBehavior AllowFocusedRow="true" />
                            <Styles Header-Wrap="True">
                                <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                            </Styles>
                            <SettingsText EmptyDataRow="Trenutno ni podatka o relacijah." />
                            <Columns>
                                <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="60px" SelectAllCheckboxMode="AllPages" ShowClearFilterButton="true" />
                                <dx:GridViewDataTextColumn Caption="ID" FieldName="RelacijaID" Width="80px"
                                    ReadOnly="true" Visible="false">
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="50%">
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Odpoklici v prejšnjem letu"
                                    FieldName="RecallCount" Width="25%">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Dolžina"
                                    FieldName="Dolzina" Width="15%" Visible="false">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataDateColumn Caption="Datum" FieldName="Datum" Width="10%" Visible="false">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                    <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy" />
                                </dx:GridViewDataDateColumn>

                            </Columns>
                        </dx:ASPxGridView>
                    </div>
                    <div class="col-md-6">
                        <h2 class="text-center">Prevozniki</h2>
                        <hr />
                        <dx:ASPxGridView ID="ASPxGridViewCarrier" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridCarrier"
                            Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewCarrier_DataBinding"
                            KeyFieldName="idStranka" CssClass="gridview-no-header-padding">
                            <ClientSideEvents SelectionChanged="OnSelectionChanged" />
                            <Paddings Padding="0" />
                            <Settings ShowVerticalScrollBar="True"
                                ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400"
                                ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                            <SettingsPager PageSize="50" ShowNumericButtons="true" NumericButtonCount="1" PageNumberFormat="1" Visible="true">
                                <PageSizeItemSettings Visible="false" Items="10,20,30" Caption="Zapisi na stran : " AllItemText="Vsi">
                                </PageSizeItemSettings>
                                <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                            </SettingsPager>
                            <SettingsBehavior AllowFocusedRow="true" />
                            <Styles Header-Wrap="True">
                                <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                            </Styles>
                            <SettingsText EmptyDataRow="Trenutno ni podatka o prevoznikih." />
                            <Columns>
                                <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="80px" SelectAllCheckboxMode="AllPages" ShowClearFilterButton="true" />
                                <dx:GridViewDataTextColumn Caption="ID" FieldName="idStranka" Width="80px"
                                    ReadOnly="true" Visible="false">
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Naziv" FieldName="NazivPrvi">
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Naslov"
                                    FieldName="Naslov">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Email"
                                    FieldName="Email" Width="15%" Visible="false">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Telefon"
                                    FieldName="Telefon" Width="15%" Visible="false">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                            </Columns>
                        </dx:ASPxGridView>
                    </div>
                </div>
                <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
                    <div class="DeleteButtonElements">
                        <span class="AddEditButtons">
                            <dx:ASPxButton ID="btnSend" runat="server" Text="Generiraj razpise" AutoPostBack="false"
                                Height="25" Width="50" ClientInstanceName="btnSend" ClientEnabled="false">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                                <ClientSideEvents Click="SendTenders" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btnConfirmDownload" runat="server" Text="Prenesi" AutoPostBack="false"
                                Height="25" Width="50" ClientInstanceName="btnConfirmDownload" ClientVisible="false" OnClick="btnConfirmDownload_Click">
                            </dx:ASPxButton>
                        </span>
                    </div>
                </div>

                <dx:ASPxPopupControl ID="ASPxPopupCompleteTenderData" runat="server"
                    ClientInstanceName="clientPopupCompleteTenderData" Modal="True" HeaderText="Dodajanje razpisa (manjkajoči podatki)"
                    CloseAction="CloseButton" Width="690px" Height="150px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Slide" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true"
                    OnWindowCallback="ASPxPopupCompleteTenderData_WindowCallback">
                    <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                    <ContentStyle BackColor="#F7F7F7">
                        <Paddings PaddingBottom="0px" PaddingLeft="6px" PaddingRight="6px" PaddingTop="0px"></Paddings>
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl>
                            <div class="row small-padding-bottom small-padding-top">
                                <div class="col-md-12">
                                    <div class="row2">
                                        <div class="col-xs-2 align-item-centerV-centerH">
                                            <i class="fa fa-balance-scale" style="font-size: 30px; color: #3C8DBC;"></i>
                                        </div>
                                        <div class="col-xs-10 text-right">
                                            <div class="row2 align-item-centerV-startH small-padding-bottom">
                                                <div class="col-sm-0 big-margin-r">
                                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Font-Bold="true" Text="DATUM RAZPISA : "></dx:ASPxLabel>
                                                </div>
                                                <div class="col-sm-5 no-padding-left">
                                                    <dx:ASPxDateEdit ID="DateEditDatumRazpisa" runat="server" EditFormat="Date" Width="100%" ClientInstanceName="clientDateEditTenderDate"
                                                        CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientEnabled="false" BackColor="LightGray">
                                                        <FocusedStyle CssClass="focus-text-box-input" />
                                                        <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                                        <DropDownButton Visible="true"></DropDownButton>
                                                        <ClientSideEvents GotFocus="FocusClearValidationIfAny" />
                                                    </dx:ASPxDateEdit>
                                                </div>
                                            </div>

                                            <div class="row2 align-item-centerV-startH">
                                                <div class="col-sm-0 big-margin-r" style="margin-right: 74px;">
                                                    <dx:ASPxLabel ID="ASPxLabel18" runat="server" Font-Size="12px" Font-Bold="true" Text="NAZIV : "></dx:ASPxLabel>
                                                </div>
                                                <div class="col-sm-9 no-padding-left">
                                                    <dx:ASPxTextBox runat="server" ID="txtTenderName" ClientInstanceName="clientTxtTenderName"
                                                        CssClass="text-box-input" Font-Size="13px" Width="100%">
                                                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                        <ClientSideEvents GotFocus="FocusClearValidationIfAny" />
                                                    </dx:ASPxTextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row small-padding-bottom">
                                <div class="col-xs-12">
                                    <div class="AddEditButtonsElements">
                                        <span class="AddEditButtons">
                                            <dx:ASPxButton ID="btnConfirm" runat="server" Text="Shrani" AutoPostBack="false"
                                                Height="25" Width="50" ClientInstanceName="btnSave" OnClick="btnConfirm_Click">
                                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                                <ClientSideEvents Click="SaveTender" />
                                            </dx:ASPxButton>
                                        </span>
                                        <span class="AddEditButtons">
                                            <dx:ASPxButton ID="btnCancel" runat="server" Text="Prekliči" AutoPostBack="false"
                                                Height="25" Width="50" ClientInstanceName="btnCancel">
                                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                                <ClientSideEvents Click="function(s,e){clientPopupCompleteTenderData.Hide();}" />
                                            </dx:ASPxButton>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

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
                        <button type="button" class="btn btn-default" id="modal-btn-download">Prenesi</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">Zapri</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
