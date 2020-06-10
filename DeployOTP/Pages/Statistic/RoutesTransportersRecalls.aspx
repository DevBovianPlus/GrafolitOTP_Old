<%@ Page Title="Pregled relacij in prevoznikov na odpoklicih" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RoutesTransportersRecalls.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Statistic.RoutesTransportersRecalls" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            $("#modal-btn-save").on("click", function () {
                $("#saveKVPModal").modal('hide');
            });

            $("#modal-btn-submit").on("click", function () {
                $("#saveKVPModal").modal('hide');
            });

            clientLoadingPanel.Hide();
        });

        function EndCallback_CallbackPanelSendTenders(s, e) {
            if (s.cpSendTender != null && s.cpSendTender !== undefined) {
                ShowErrorPopUp("Ustvarjenih je bilo " + s.cpSendTender + " novih razpisov, ki še niso dopolnjeni z cenami", 0, "Razpisi");
                gridRoutes.UnselectRows();
                gridCarrier.UnselectRows();
                delete (s.cpSendTender);
            }

            clientLoadingPanel.Hide();
        }


        function gridRoutes_Init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelRoutes);
        }

        function gridCarrier_init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelCarriers);
        }

        function SetEnableExportBtn(sender, button) {
            if (sender.GetVisibleRowsOnPage() > 0)
                button.SetEnabled(true);
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
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 text-left">
                                <dx:ASPxButton ID="btnExportToExcelRoutes" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExportToExcelRoutes_Click"
                                    AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelRoutes" ToolTip="Izvozi v excel">
                                    <DisabledStyle CssClass="icon-disabled" />
                                    <HoverStyle CssClass="icon-hover" />
                                    <Image Url="../../Images/export_excel.png" Width="20px" />
                                </dx:ASPxButton>
                            </div>
                        </div>
                        <div class="row2">
                            <div class="col-xs-12 small-margin-t" style="padding: 0;">
                                <dx:ASPxGridViewExporter ID="ASPxGridViewRoutesExporter" GridViewID="ASPxGridViewRoutes" runat="server">
                                    <Styles>
                                        <Header Wrap="True" />
                                    </Styles>
                                </dx:ASPxGridViewExporter>

                                <dx:ASPxGridView ID="ASPxGridViewRoutes" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridRoutes"
                                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewRoutes_DataBinding"
                                    KeyFieldName="RelacijaID" CssClass="gridview-no-header-padding">
                                    <ClientSideEvents Init="gridRoutes_Init" />
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

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="RelacijaID" Width="80px"
                                            ReadOnly="true" Visible="false">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="50%">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Vsi odpoklici v prejšnjem letu"
                                            FieldName="RecallCount" Width="12%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Naši odpoklici v prejšnjem letu"
                                            FieldName="SupplierArrangesTransportRecallCount" Width="13%">
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
                        </div>
                    </div>
                    <div class="col-md-6">
                        <h2 class="text-center">Prevozniki</h2>
                        <hr />
                        <div class="row2 align-item-centerV-startH">
                            <div class="col-xs-0 text-left">
                                <dx:ASPxButton ID="btnExportToExcelCarriers" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExportToExcelCarriers_Click"
                                    AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelCarriers" ToolTip="Izvozi v excel">
                                    <DisabledStyle CssClass="icon-disabled" />
                                    <HoverStyle CssClass="icon-hover" />
                                    <Image Url="../../Images/export_excel.png" Width="20px" />
                                </dx:ASPxButton>
                            </div>
                        </div>

                        <div class="row2">
                            <div class="col-xs-12 small-margin-t" style="padding: 0;">
                                <dx:ASPxGridViewExporter ID="ASPxGridViewCarrierExporter" GridViewID="ASPxGridViewCarrier" runat="server">
                                    <Styles>
                                        <Header Wrap="True" />
                                    </Styles>
                                </dx:ASPxGridViewExporter>

                                <dx:ASPxGridView ID="ASPxGridViewCarrier" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridCarrier"
                                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewCarrier_DataBinding"
                                    KeyFieldName="idStranka" CssClass="gridview-no-header-padding">
                                    <ClientSideEvents Init="gridCarrier_init" />
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

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="idStranka" Width="80px"
                                            ReadOnly="true" Visible="false">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="NazivPrvi">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Naslov"
                                            FieldName="Naslov">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Odpoklici v prejšnjem letu"
                                            FieldName="RecallCount" Width="25%">
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
                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
