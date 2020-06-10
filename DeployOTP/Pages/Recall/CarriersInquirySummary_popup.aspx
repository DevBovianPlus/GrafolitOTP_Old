<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="CarriersInquirySummary_popup.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Recall.CarriersInquirySummary_popup" %>

<%@ MasterType VirtualPath="~/Popup.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolderPopup" runat="server">
    <script type="text/javascript">
        var addToListItemsClick = false;

        function OnSelectionChanged_gridCarrierInquiry(s, e) {
            if (s.GetSelectedRowCount() > 0 && s.GetSelectedRowCount() < 2)//za izbiro prevoznika lahko izbere samo enega
            {
                clientBtnSelectCarrier.SetEnabled(true);
                btnDeleteSelectedCarrier.SetEnabled(true);
                btnReSendEmailToSelectedCarriers.SetEnabled(true);
            }
            else {
                 btnDeleteSelectedCarrier.SetEnabled(false);
                clientBtnSelectCarrier.SetEnabled(false);
                btnReSendEmailToSelectedCarriers.SetEnabled(false);
            }

            if (s.GetSelectedRowCount() > 0)
                btnReSendEmailToSelectedCarriers.SetEnabled(true);
        }

        function CloseGridLookup() {
            lookUpStranke.ConfirmCurrentSelection();
            lookUpStranke.HideDropDown();
            lookUpStranke.Focus();
            addToListItemsClick = true;
        }

        function lookUpStranke_ValueChanged(s, e) {
            if (addToListItemsClick) {
                gridCarrierInquiry.PerformCallback('AddNewCarriers');
                lookUpStranke.SetEnabled(false);
            }
        }

        function gridCarrierInquiry_EndCallback(s, e) {
            if (addToListItemsClick) {
                gridCarrierInquiry.Refresh();
                lookUpStranke.GetGridView().Refresh();
                addToListItemsClick = false;
                lookUpStranke.SetEnabled(true);
                lookUpStranke.SetText("");
            }

            LoadingPanel.Hide();
        }

        function btnReSendEmailToSelectedCarriers_Click(s, e) {
            LoadingPanel.Show();

            gridCarrierInquiry.PerformCallback('ReSendEmailToCarriers');
        }

        function btnDeleteSelectedCarrier_Click(s, e) {
            LoadingPanel.Show();
            addToListItemsClick = true;
            gridCarrierInquiry.PerformCallback('DeleteCarrier');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolderPopup" runat="server">
    <div class="row small-padding-bottom">
        <div class="col-xs-12 no-padding-left no-padding-right">
            <dx:ASPxGridView ID="ASPxGridViewCarrierInquiry" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridCarrierInquiry"
                Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewCarrierInquiry_DataBinding"
                KeyFieldName="PrijavaPrevoznikaID" CssClass="gridview-no-header-padding" OnCustomCallback="ASPxGridViewCarrierInquiry_CustomCallback"
                OnHtmlDataCellPrepared="ASPxGridViewCarrierInquiry_HtmlDataCellPrepared" OnDataBound="ASPxGridViewCarrierInquiry_DataBound">
                <ClientSideEvents SelectionChanged="OnSelectionChanged_gridCarrierInquiry" EndCallback="gridCarrierInquiry_EndCallback" />
                <Paddings Padding="0" />
                <Settings ShowVerticalScrollBar="True"
                    ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="300"
                    ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" />
                <SettingsPager PageSize="50" ShowNumericButtons="true">
                    <PageSizeItemSettings Visible="true" Items="50,80,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                    </PageSizeItemSettings>
                    <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                </SettingsPager>
                <SettingsBehavior AllowFocusedRow="true" AllowSelectSingleRowOnly="false" />
                <Styles Header-Wrap="True">
                    <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                    <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                </Styles>
                <SettingsText EmptyDataRow="Trenutno ni podatka o prijavah prevoznika." />
                <Columns>
                    <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="80px" SelectAllCheckboxMode="AllPages" Caption="Izberi" ShowClearFilterButton="true" />
                    <dx:GridViewDataTextColumn Caption="Odpoklic štev." FieldName="Odpoklic.OdpoklicStevilka" Width="9%"
                        ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Prevoznik" FieldName="Prevoznik.NazivPrvi" Width="40%"
                        ReadOnly="true" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Prvotna cena" FieldName="PrvotnaCena" Width="10%"
                        ReadOnly="true" ShowInCustomizationForm="True" PropertiesTextEdit-DisplayFormatString="N2">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Prijavljena cena"
                        FieldName="PrijavljenaCena" ShowInCustomizationForm="True"
                        Width="10%" PropertiesTextEdit-DisplayFormatString="N2">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataDateColumn FieldName="DatumNaklada" Caption="Datum naklada" ShowInCustomizationForm="True" Width="10%">
                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                        </PropertiesDateEdit>
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataDateColumn>

                    <dx:GridViewDataDateColumn FieldName="DatumPosiljanjePrijav" Caption="Datum pošiljanja" ShowInCustomizationForm="True" Width="10%">
                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                        </PropertiesDateEdit>
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataDateColumn>

                    <dx:GridViewDataDateColumn FieldName="DatumPrijave" Caption="Datum prijave" ShowInCustomizationForm="True" Width="10%">
                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                        </PropertiesDateEdit>
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataDateColumn>

                    <dx:GridViewDataTextColumn Caption="Odstopanje v EUR"
                        FieldName="OdstopanjeVEUR" ShowInCustomizationForm="True"
                        Width="10%" PropertiesTextEdit-DisplayFormatString="N2">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </div>
    </div>
    <div class="row small-padding-bottom" style="margin-top: 15px;">
        <div class="col-xs-12 no-padding-right">
            <div class="row2 align-item-centerV-startH">
                <div class="col-sm-12 no-padding-left">
                    <div>
                        <h5 class="no-margin"><em>Dodaj novega prevoznika</em></h5>
                    </div>
                    <div class="panel panel-default" style="margin-top: 2px;">
                        <div class="panel-body">
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 22px;">
                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="PREVOZNIK : "></dx:ASPxLabel>
                                </div>
                                <div class="col-xs-8 no-padding-left">
                                    <dx:ASPxGridLookup ID="ASPxGridLookupStranke" runat="server" ClientInstanceName="lookUpStranke"
                                        KeyFieldName="RazpisPozicijaID" TextFormatString="{1}" CssClass="text-box-input"
                                        Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                        OnLoad="ASPxGridLookupLoad_WidthXtraLarge" OnDataBinding="ASPxGridLookupStranke_DataBinding" IncrementalFilteringMode="Contains"
                                        SelectionMode="Multiple" MultiTextSeparator="; ">
                                        <ClearButton DisplayMode="OnHover" />
                                        <%-- DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Stranka.NazivPrvi').Focus();}"--%>
                                        <ClientSideEvents ValueChanged="lookUpStranke_ValueChanged" />
                                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        <GridViewStyles>
                                            <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                            <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                        </GridViewStyles>
                                        <GridViewProperties>
                                            <SettingsBehavior EnableRowHotTrack="True" />
                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" />
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
                                                                <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" UseSubmitBehavior="false" Text="Dodaj na seznam in pošlji email" ClientSideEvents-Click="CloseGridLookup"
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
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">

        <span class="AddEditButtons">
            <dx:ASPxButton Theme="Moderno" ID="btnReSendEmailToSelectedCarriers" runat="server" Text="Ponovno pošlji email izbranim prevoznikom" AutoPostBack="false"
                Height="25" Width="90" ClientEnabled="false" ClientInstanceName="btnReSendEmailToSelectedCarriers">
                <Paddings PaddingLeft="10" PaddingRight="10" />
                <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                <ClientSideEvents Click="btnReSendEmailToSelectedCarriers_Click" />
            </dx:ASPxButton>
        </span>

        <div class="AddEditButtonsElements">
            <span class="AddEditButtons">
                <dx:ASPxButton Theme="Moderno" ID="btnSelectCarrier" runat="server" Text="Izberi prevoznika" AutoPostBack="false" OnClick="btnConfirm_Click"
                    Height="25" Width="90" ClientEnabled="false" ClientInstanceName="clientBtnSelectCarrier">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                </dx:ASPxButton>
            </span>

            <span class="AddEditButtons">
                <dx:ASPxButton Theme="Moderno" ID="btnDeleteSelectedCarrier" runat="server" Text="Izbriši" AutoPostBack="false"
                    Height="25" Width="90" ClientEnabled="false" ClientVisible="false" ClientInstanceName="btnDeleteSelectedCarrier">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/trash.png" UrlHottracked="../../Images/trashHover.png" />
                    <ClientSideEvents Click="btnDeleteSelectedCarrier_Click" />
                </dx:ASPxButton>
            </span>
        </div>
    </div>
</asp:Content>
