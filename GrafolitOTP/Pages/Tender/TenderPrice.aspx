<%@ Page Title="Pregled cen prevoznikov glede na relacijo" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TenderPrice.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Tender.TenderPrice" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script>
        function Relacija_ValueChanged(s, e) {
            callbackPanelRoutes.PerformCallback('CarrierrChanged');
        }

        var isBtnConfirmIntiated = false;
        function btnConfirm_Click(s, e) {

            if (!isBtnConfirmIntiated) {
                callbackPanelRoutes.PerformCallback('OpenPopupByRoute');
                isBtnConfirmIntiated = true;
            }
        }

        function gridRoutes_DoubleClick(s, e) {
            gridRoutes.GetRowValues(gridRoutes.GetFocusedRowIndex(), 'RelacijaID', OnGetRowValue);
        }

        function OnGetRowValue(value) {
            callbackPanelRoutes.PerformCallback('OpenPopupByRoute;' + value);
        }

        function OnClosePopUpHandler(command, sender, userAction, recallID) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'RoutesDetail':
                            clientPopUproutesDetail.Hide();
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'RoutesDetail':
                            clientPopUproutesDetail.Hide();
                    }
                    break;
            }
        }

        function ShowPrices(s, e) {            
            callbackPanelRoutes.PerformCallback('ShowPrices');
        }

        function CallbackPanelRoutes_EndCallback(s, e) { }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="row small-padding-bottom">
        <div class="col-md-8">
            <div class="row2" style="align-items: center;">
                <div class="col-sm-1 no-padding-right" <%--style="background-color: lavender;"--%>>
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="RELACIJA: "></dx:ASPxLabel>
                </div>
                <div class="col-sm-4 no-padding-left" <%--style="background-color: lavenderblush;"--%>>
                    <dx:ASPxGridLookup ID="ASPxGridLookupRelacija" runat="server" ClientInstanceName="lookUpRelacija"
                        KeyFieldName="RelacijaID" TextFormatString="{1}" CssClass="text-box-input"
                        Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                        OnLoad="ASPxGridLookupLoad_WidthLarge" OnDataBinding="ASPxGridLookupRealacija_DataBinding" IncrementalFilteringMode="Contains">
                        <ClearButton DisplayMode="OnHover" />
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
                        <ClientSideEvents ValueChanged="Relacija_ValueChanged" />
                    </dx:ASPxGridLookup>
                </div>
                <div class="col-sm-6 no-padding-right" <%--style="background-color: lightblue;"--%>>&nbsp</div>
            </div>


            <div class="row2" style="align-items: center; padding-top: 8px">
                <div class="col-sm-1 no-padding-right" <%--style="background-color: lavender;"--%>>
                    <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="DATUM RAZPISA: " Width="80px"></dx:ASPxLabel>
                </div>
                <div class="col-sm-2 no-padding-left">
                    <dx:ASPxDateEdit ID="DateEditDatumRazpisa" runat="server" EditFormat="Date" Width="170" Theme="Moderno"
                        CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="DateEditDatumRazpisa">
                        <FocusedStyle CssClass="focus-text-box-input" />
                        <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                        <DropDownButton Visible="true"></DropDownButton>
                    </dx:ASPxDateEdit>
                </div>
                <div class="col-sm-6 no-padding-left">
                    <dx:ASPxButton ID="btnShowPrices" runat="server" Text="Izpiši cene" AutoPostBack="false"
                        Width="50" ClientInstanceName="btnShowPrices" ClientEnabled="true">
                        <Paddings PaddingLeft="20" PaddingRight="20" />
                        <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                        <ClientSideEvents Click="ShowPrices" />
                    </dx:ASPxButton>
                </div>
            </div>
        </div>
    </div>
    <dx:ASPxCallbackPanel ID="CallbackPanelRoutes" runat="server" ClientInstanceName="callbackPanelRoutes" OnCallback="CallbackPanelRoutes_Callback">
        <ClientSideEvents EndCallback="CallbackPanelRoutes_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxButton ID="btnExportEvents" runat="server" RenderMode="Link" ClientEnabled="true" OnClick="btnExportEvents_Click"
                    AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientbtnExportStranke" ToolTip="Izvozi v excel">
                    <DisabledStyle CssClass="icon-disabled" />
                    <HoverStyle CssClass="icon-hover" />
                    <Image Url="../../Images/pdf-export.png" Width="30px" />
                </dx:ASPxButton>
                <dx:ASPxGridViewExporter ID="ASPxGridViewExporterEvents" GridViewID="ASPxGridViewRoutes" runat="server"></dx:ASPxGridViewExporter>
                <dx:ASPxGridView ID="ASPxGridViewRoutes" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridRoutes"
                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewRoutes_DataBinding"
                    KeyFieldName="RelacijaID" CssClass="gridview-no-header-padding"
                    OnCommandButtonInitialize="ASPxGridViewRoutes_CommandButtonInitialize">
                    <ClientSideEvents RowDblClick="gridRoutes_DoubleClick" />
                    <Paddings Padding="0" />
                    <Settings ShowVerticalScrollBar="True"
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
                    <SettingsText EmptyDataRow="Trenutno ni podatka o relacijah. Izberi prevoznika." />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Stranka Id" FieldName="Stranka.idStranka" Width="80px"
                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Cena"
                            FieldName="Cena" ShowInCustomizationForm="True"
                            Width="10%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Stranka.NazivPrvi" Width="35%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        
                        <dx:GridViewDataTextColumn Caption="Relacija" FieldName="Relacija.Naziv" Width="55%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                       


                    </Columns>
                </dx:ASPxGridView>                
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
