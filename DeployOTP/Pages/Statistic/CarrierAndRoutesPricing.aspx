<%@ Page Title="Pregled cen glede na prevoznika in relacijo" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CarrierAndRoutesPricing.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Statistic.CarrierAndRoutesPricing" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script>
        function Prevoznik_ValueChanged(s, e) {
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

        function CallbackPanelRoutes_EndCallback(s, e) { }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="row small-padding-bottom">
        <div class="col-md-4">
            <div class="row2" style="align-items: center;">
                <div class="col-xs-4 no-padding-right">
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="PREVOZNIK : "></dx:ASPxLabel>
                </div>
                <div class="col-xs-8 no-padding-left">
                    <dx:ASPxGridLookup ID="ASPxGridLookupPrevoznik" runat="server" ClientInstanceName="lookUpPrevoznik"
                        KeyFieldName="idStranka" TextFormatString="{0}" CssClass="text-box-input"
                        Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                        OnLoad="ASPxGridLookupLoad_WidthSmall" OnDataBinding="ASPxGridLookupPrevoznik_DataBinding" IncrementalFilteringMode="Contains">
                        <ClearButton DisplayMode="OnHover" />
                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                        <GridViewStyles>
                            <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                            <FilterBarClearButtonCell></FilterBarClearButtonCell>
                        </GridViewStyles>
                        <GridViewProperties>
                            <SettingsBehavior EnableRowHotTrack="True" />
                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                            <SettingsPager ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                        </GridViewProperties>
                        <Columns>

                            <dx:GridViewDataTextColumn Caption="Naziv" FieldName="NazivPrvi" Width="100%"
                                ReadOnly="true" ShowInCustomizationForm="True">
                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            </dx:GridViewDataTextColumn>

                        </Columns>
                        <ClientSideEvents ValueChanged="Prevoznik_ValueChanged" />
                    </dx:ASPxGridLookup>
                </div>
            </div>
        </div>
    </div>
    <dx:ASPxCallbackPanel ID="CallbackPanelRoutes" runat="server" ClientInstanceName="callbackPanelRoutes" OnCallback="CallbackPanelRoutes_Callback">
        <ClientSideEvents EndCallback="CallbackPanelRoutes_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
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
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="RelacijaID" Visible="false">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Naziv relacije" FieldName="Naziv" Width="70%" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Zadnja cena" FieldName="Cena" Width="10%" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataDateColumn FieldName="Datum" Caption="Datum razpisa" ShowInCustomizationForm="True" Width="20%">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                            </PropertiesDateEdit>
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataDateColumn>
                    </Columns>
                </dx:ASPxGridView>

                <dx:ASPxPopupControl ID="ASPxPopupControlRoutesDetail" runat="server" ContentUrl="RoutesDetail_popup.aspx"
                    ClientInstanceName="clientPopUproutesDetail" Modal="True" HeaderText="CENE RELACIJE GLEDE NA PREVOZNIKA"
                    CloseAction="CloseButton" Width="800px" Height="650px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true"
                    OnWindowCallback="ASPxPopupControlRoutesDetail_WindowCallback">
                    <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                    <ContentStyle BackColor="#F7F7F7">
                        <Paddings PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px" PaddingTop="0px"></Paddings>
                    </ContentStyle>
                </dx:ASPxPopupControl>

                <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
                    <div class="AddEditButtonsElements">
                        <span class="AddEditButtons">
                            <dx:ASPxButton Theme="Moderno" ID="btnConfirm" runat="server" Text="Preveri cene za relacijo" AutoPostBack="false"
                                Height="25" Width="90" ClientInstanceName="clientBtnConfirm" ClientVisible="false">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../../../Images/edit.png" UrlHottracked="../../Images/editHover.png" />
                                <ClientSideEvents Click="btnConfirm_Click" />
                            </dx:ASPxButton>
                        </span>
                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
