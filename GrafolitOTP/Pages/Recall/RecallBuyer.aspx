<%@ Page Title="Odpoklici kupcev" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RecallBuyer.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Recall.RecallBuyer" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            var filterType = GetUrlQueryStrings()['filter'];

            //we delete successMessage query string so we show modal only once!
            var params = QueryStringsToObject();
            delete params.filter;i
            var path = window.location.pathname + '?' + SerializeQueryStrings(params);
            history.pushState({}, document.title, path);
        });

        function DoubleClick(s, e) {
            gridRecallBuyer.GetRowValues(gridRecallBuyer.GetFocusedRowIndex(), 'OdpoklicKupecID', OnGetRowValues);
        }

        function OnGetRowValues(value) {
            gridRecallBuyer.PerformCallback('DblClick;' + value);
        }

        function gridRecallBuyer_EndCallback(s, e) {
            if (s.cpPrintID != "" && s.cpPrintID != undefined) {
                //    window.open(s.cpPrintID, '_blank');
                //    delete (s.cpPrintID);

                clientPopupPrintSelection.Show();
            }
        }

        function OnSelectionChanged_gridRecallBuyer(s, e) {
            if (s.GetSelectedRowCount() > 0) {
                var role = '<%= OptimizacijaTransprotov.Helpers.PrincipalHelper.IsUserAdmin()%>';
                var role2 = '<%= OptimizacijaTransprotov.Helpers.PrincipalHelper.IsUserSuperAdmin()%>';
                btnClearStatus.SetVisible(true);
                btnSendOrder.SetVisible(true);

                if ((role == "False") && (role2 == "False")) {
                    btnClearStatus.SetEnabled(false);
                }


            }

            else {
                btnClearStatus.SetVisible(false);
                btnSendOrder.SetVisible(false);
            }
        }

        function OnFocusedRowChanged_gridRecall(s, e) {
            gridRecallBuyer.GetRowValues(gridRecall.GetFocusedRowIndex(), 'StatusKoda', OnGetRowValuesFocusedChanged);
        }
        function OnGetRowValuesFocusedChanged(value) {

            var status = '<%= DatabaseWebService.Common.Enums.Enums.StatusOfRecall.RAZPIS_PREVOZNIK.ToString()%>';
            if (status == value)
                btnOpenInquirySummaryForCarrier.SetVisible(true);
            else
                btnOpenInquirySummaryForCarrier.SetVisible(false);
        }

        function btnOpenInquirySummaryForCarrier_Click(s, e) {
            gridRecallBuyer.GetRowValues(gridRecall.GetFocusedRowIndex(), 'OdpoklicKupecID', OnGetFocuesdRowValue);
        }

        function OnGetFocuesdRowValue(value) {
            RecallCalbackPanel.PerformCallback('InquirySummary;' + value);
        }

        function IzpisOdpoklica(s, e) {

            clientPopupPrintSelection.Hide();            
            e.processOnServer = true;

        }

        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'InquirySummary':
                            clientPopUpCarriersInquirySummary.Hide();
                            $('#warningModal').modal('show');
                            $('#modalBodyText').empty();
                            $('#modalBodyText').append("Uspešno ste izbrali prevoznika za odpoklic.");
                            $('#myModalTitle').empty();
                            $('#myModalTitle').append("Odlično!");
                            gridRecall.Refresh();
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'InquirySummary':
                            clientPopUpCarriersInquirySummary.Hide();
                    }
                    break;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxCallbackPanel ID="RecallCallbackPanel" ClientInstanceName="RecallCalbackPanel" runat="server"
        OnCallback="RecallCallbackPanel_Callback">
        <PanelCollection>
            <dx:PanelContent>

                <dx:ASPxGridView ID="ASPxGridViewRecallBuyer" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridRecallBuyer"
                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewRecallBuyer_DataBinding"
                    KeyFieldName="OdpoklicKupecID" CssClass="gridview-no-header-padding-medium-fonts" OnCustomCallback="ASPxGridViewRecallBuyer_CustomCallback"
                    OnCustomButtonCallback="ASPxGridViewRecallBuyer_CustomButtonCallback" OnHtmlRowPrepared="ASPxGridViewRecallBuyer_HtmlRowPrepared" OnCommandButtonInitialize="ASPxGridViewRecallBuyer_CommandButtonInitialize">
                    <ClientSideEvents RowDblClick="DoubleClick" EndCallback="gridRecallBuyer_EndCallback" SelectionChanged="OnSelectionChanged_gridRecallBuyer" />
                    <%-- FocusedRowChanged="OnFocusedRowChanged_gridRecall" --%>
                    <Paddings Padding="0" />
                    <Settings ShowVerticalScrollBar="True"
                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400"
                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
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
                        <dx:GridViewCommandColumn ButtonRenderMode="Button" Width="5%" Caption="Dokument">
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="Print">
                                </dx:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="OdpoklicKupecID" Width="1%"
                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True" SortOrder="Descending">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Naročilnica št." FieldName="StevilkaNarocilnica" Width="6%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Odpoklic št." FieldName="OdpoklicKupecStevilka" Width="3%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Prevoznik" FieldName="PrevoznikNaziv" Width="14%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Relacija" FieldName="RelacijaNaziv" Width="25%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Cena prevoza"
                            FieldName="CenaPrevoza" ShowInCustomizationForm="True"
                            Width="5%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            <PropertiesTextEdit DisplayFormatString="c"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Skupna količina"
                            FieldName="KolicinaSkupno" ShowInCustomizationForm="True"
                            Width="4%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Dobavitelj kraj" FieldName="DobaviteljKraj"
                            ReadOnly="true" ShowInCustomizationForm="True" Visible="false">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataDateColumn Caption="Datum odpoklica" FieldName="ts" CellStyle-HorizontalAlign="Right" Width="7%">
                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy"></PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>

                        <dx:GridViewDataTextColumn Caption="Status Koda" FieldName="StatusKoda" Width="10%"
                            Visible="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:ASPxGridView>
                <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
                    <div class="DeleteButtonElements">
                        <span class="AddEditButtons">
                            <dx:ASPxButton Theme="Moderno" ID="btnDelete" runat="server" Text="Izbriši" AutoPostBack="false" OnClick="btnDelete_Click"
                                Height="25" Width="50">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../../../Images/trash.png" UrlHottracked="../../Images/trashHover.png" />
                            </dx:ASPxButton>
                        </span>

                        <span class="AddEditButtons">
                            <dx:ASPxButton Theme="Moderno" ForeColor="Red" ID="btnStorno" runat="server" Text="Storno" AutoPostBack="false" OnClick="btnStorno_Click"
                                Height="47" Width="50" ClientInstanceName="clientbtnStorno" ClientVisible="true">
                                <Paddings PaddingLeft="10" PaddingRight="10" />                                
                                <Image Url="../../../Images/storno.png" UrlHottracked="../../Images/storno.png" />
                            </dx:ASPxButton>
                        </span>

                        <span class="AddEditButtons">
                            <dx:ASPxButton Theme="Moderno" ID="btnOpenInquirySummaryForCarrier" runat="server" Text="Preglej razpise prevoznikov" AutoPostBack="false"
                                Height="47" Width="50" ClientInstanceName="btnOpenInquirySummaryForCarrier" ClientVisible="false">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <ClientSideEvents Click="btnOpenInquirySummaryForCarrier_Click" />
                                <%--<Image Url="../../../Images/trash.png" UrlHottracked="../../Images/trashHover.png" />--%>
                            </dx:ASPxButton>
                        </span>
                    </div>
                    <div class="AddEditButtonsElements">
                        <span class="AddEditButtons">
                            <dx:ASPxButton ID="btnClearStatus" runat="server" Text="Resetiraj" AutoPostBack="false"
                                Height="43" Width="90" ClientInstanceName="btnClearStatus" ClientVisible="false" OnClick="btnClearStatus_Click">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../../../Images/lock.png" UrlHottracked="../../Images/lockHover.png" />
                            </dx:ASPxButton>
                        </span>
                        <span class="AddEditButtons">
                            <dx:ASPxButton ID="btnSendOrder" runat="server" Text="Pošlji naročilnico" AutoPostBack="false"
                                Height="43" Width="90" ClientInstanceName="btnSendOrder" ClientVisible="false" OnClick="btnSendOrder_Click">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../../../Images/sendMailToCarriers.png" UrlHottracked="../../Images/sendMailToCarriersHover.png" />
                            </dx:ASPxButton>
                        </span>
                        <span class="AddEditButtons">
                            <dx:ASPxButton Theme="Moderno" ID="btnAdd" runat="server" Text="Dodaj" AutoPostBack="false" OnClick="btnAdd_Click"
                                Height="25" Width="90">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                            </dx:ASPxButton>
                        </span>
                        <span class="AddEditButtons">
                            <dx:ASPxButton Theme="Moderno" ID="btnEdit" runat="server" Text="Spremeni" AutoPostBack="false" OnClick="btnEdit_Click"
                                Height="25" Width="90">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../../../Images/edit.png" UrlHottracked="../../Images/editHover.png" />
                            </dx:ASPxButton>
                        </span>
                    </div>
                </div>


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

                <dx:ASPxPopupControl ID="ASPxPopupPrintSelection" runat="server"
                    ClientInstanceName="clientPopupPrintSelection" Modal="True" HeaderText="Izberite način izpisa"
                    CloseAction="CloseButton" Width="400px" Height="150px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Slide" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true">
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
                                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Font-Bold="true" Text="Prikaz vrednost transporta : "></dx:ASPxLabel>
                                                </div>
                                                <div class="col-xs-1 no-padding-left">
                                                    <dx:ASPxCheckBox ID="chkPrikazVrednosti" runat="server" Checked="true">
                                                    </dx:ASPxCheckBox>
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
                                            <dx:ASPxButton ID="btnConfirm" runat="server" Text="Izpiši" AutoPostBack="false"
                                                Height="25" Width="50" ClientInstanceName="btnIzpis" OnClick="btnConfirm_Click">
                                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                                <ClientSideEvents Click="IzpisOdpoklica" />
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

    <!-- Warning - Modal -->
    <div id="warningModal" class="modal fade" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: #88e188; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="fa fa-check-circle-o" style="font-size: 60px; color: white"></i></div>
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
