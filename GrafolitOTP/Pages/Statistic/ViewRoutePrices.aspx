<%@ Page Title="Relacije" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewRoutePrices.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Statistic.ViewRoutePrices" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script>
        function RadioButtonList_ValueChanged(s, e) {
            LoadingPanel.Show();
            clientCallbackPanelRoute.PerformCallback(s.GetValue());
        }

        function CallbackPanelRoute_EndCallback(s, e) {
            LoadingPanel.Hide();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxCallbackPanel ID="CallbackPanelRoute" ClientInstanceName="clientCallbackPanelRoute" runat="server" Width="100%" OnCallback="CallbackPanelRoute_Callback">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="CallbackPanelRoute_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="row m-0 pb-3 justify-content-end">
                    <div class="col-md-6">
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 20px;">
                                <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="DATUM Od : " Width="80px"></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-0 no-padding-left">
                                <dx:ASPxDateEdit ID="DateEditDatumOd" runat="server" EditFormat="Date" Width="170" Theme="Moderno"
                                    CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="DateEditDatumOd">
                                    <ClientSideEvents ValueChanged="RadioButtonList_ValueChanged" />
                                    <FocusedStyle CssClass="focus-text-box-input" />
                                    <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                    <DropDownButton Visible="true"></DropDownButton>
                                </dx:ASPxDateEdit>
                            </div>

                            <div class="col-xs-0" style="margin-right: 20px; margin-left: 20px;">
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="DATUM Do : " Width="80px"></dx:ASPxLabel>
                            </div>
                            <div class="col-xs-0">
                                <dx:ASPxDateEdit ID="DateEditDatumDo" runat="server" EditFormat="Date" Width="170" Theme="Moderno"
                                    CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="DateEditDatumDo">
                                    <ClientSideEvents ValueChanged="RadioButtonList_ValueChanged" />
                                    <FocusedStyle CssClass="focus-text-box-input" />
                                    <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                    <DropDownButton Visible="true"></DropDownButton>
                                </dx:ASPxDateEdit>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row m-0 align-item-centerV-startH">
                    <div class="col-xs-0 big-margin-r" style="margin-right: 20px;">
                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Prevoz organizira : " Width="180px"></dx:ASPxLabel>
                    </div>
                    <div class="col-xs-4 mb-2 mb-lg-0">
                        <dx:ASPxRadioButtonList ID="RadioButtonList" runat="server" ValueType="System.String" RepeatColumns="6" RepeatLayout="Flow">
                            <CaptionSettings Position="Top" />
                            <ClientSideEvents ValueChanged="RadioButtonList_ValueChanged" />
                            <Items>
                                <dx:ListEditItem Text="Vseeno" Value="AllValues" Selected="true" />
                                <dx:ListEditItem Text="Grafolit" Value="GrafolitPrevoz" />
                                <dx:ListEditItem Text="Dobavitelj" Value="Dobavitelj" />
                                <dx:ListEditItem Text="Kupec" Value="Kupec" />
                                <dx:ListEditItem Text="Grafolit - Lastni" Value="GrafolitLastniPrevoz" />
                            </Items>
                        </dx:ASPxRadioButtonList>
                    </div>
                    <div class="col-xs-0 big-margin-r" style="margin-right: 20px;">
                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Teža : " Width="180px"></dx:ASPxLabel>
                    </div>
                    <div class="col-xs-4 mb-2 mb-lg-0">
                        <dx:ASPxRadioButtonList ID="RadioButtonTeza" runat="server" ValueType="System.String" RepeatColumns="4" RepeatLayout="Flow">
                            <CaptionSettings Position="Top" />
                            <ClientSideEvents ValueChanged="RadioButtonList_ValueChanged" />
                            <Items>
                                <dx:ListEditItem Text="Odpoklici nad 20t" Value="1" Selected="true" />
                                <dx:ListEditItem Text="Odpoklici pod 20t" Value="2" />
                            </Items>
                        </dx:ASPxRadioButtonList>
                    </div>
                </div>


                <dx:ASPxButton ID="btnExportTransportPricesCompare" runat="server" RenderMode="Link" ClientEnabled="true" OnClick="btnExportTransportPricesCompare_Click"
                    AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientbtnExportTransportPricesCompare" ToolTip="Izvozi v PDF">
                    <DisabledStyle CssClass="icon-disabled" />
                    <HoverStyle CssClass="icon-hover" />
                    <Image Url="../../Images/pdf-export.png" Width="30px" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnXlsxExport" runat="server" Text="" OnClick="btnXlsxExport_Click" Theme="MetropolisBlue" AutoPostBack="False"
                    EnableTheming="false" RenderMode="Link"
                    Border-BorderStyle="None" EnableViewState="False" BackColor="Transparent">
                    <Image Url="../../Images/export_excel.png" Height="27px" Width="27px"></Image>                    
                    <Border BorderStyle="None"></Border>
                </dx:ASPxButton>

                <dx:ASPxGridViewExporter ID="ASPxGridViewExporterTransportPricesCompare" GridViewID="ASPxGridViewRouteTransportPricesCompare" runat="server"></dx:ASPxGridViewExporter>
                <dx:ASPxGridView ID="ASPxGridViewRouteTransportPricesCompare" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridRoute" OnHtmlRowPrepared="ASPxGridViewRouteTransportPricesCompare_HtmlRowPrepared"
                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewRouteTransportPricesCompare_DataBinding" OnCustomColumnDisplayText="ASPxGridViewRouteTransportPricesCompare_CustomColumnDisplayText"
                    KeyFieldName="RelacijaID" CssClass="gridview-no-header-padding">

                    <Paddings Padding="0" />
                    <Settings ShowVerticalScrollBar="True"
                        ShowFilterBar="Hidden" ShowFilterRow="false" VerticalScrollableHeight="600"
                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                    <SettingsPager PageSize="100" ShowNumericButtons="false" AlwaysShowPager="true">
                        <PageSizeItemSettings Visible="true" Items="100, 200, 300" Caption="Zapisi na stran : " AllItemText="Vsi">
                        </PageSizeItemSettings>
                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                    </SettingsPager>
                    <SettingsBehavior AllowFocusedRow="true" />
                    <Styles Header-Wrap="True">
                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                    </Styles>
                    <SettingsText EmptyDataRow="Trenutno ni podatka o relacijah. Dodaj novo." />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="TempID" Width="80px"
                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Št. odpoklicov / leto" FieldName="RecallCount" Width="5%"
                            ReadOnly="true" ShowInCustomizationForm="True" Visible="true">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Relacija" Width="30%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Prevoznik 1" FieldName="Prevoznik_1" Width="10%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Cena" FieldName="Prevoznik_1_Cena" Width="5%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            <PropertiesTextEdit DisplayFormatString="n2" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Prevoznik 2" FieldName="Prevoznik_2" Width="10%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Cena" FieldName="Prevoznik_2_Cena" Width="5%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            <PropertiesTextEdit DisplayFormatString="n2" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Prevoznik 3" FieldName="Prevoznik_3" Width="10%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Cena" FieldName="Prevoznik_3_Cena" Width="5%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <PropertiesTextEdit DisplayFormatString="n2" />
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Prevoznik 4" FieldName="Prevoznik_4" Width="10%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Cena" FieldName="Prevoznik_4_Cena" Width="5%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <PropertiesTextEdit DisplayFormatString="n2" />
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>



                    </Columns>
                </dx:ASPxGridView>


            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
