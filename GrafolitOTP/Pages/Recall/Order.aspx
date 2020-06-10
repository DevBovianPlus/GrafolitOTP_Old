<%@ Page Title="Postavke naročilnic" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Recall.Order" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script>
        function OnSelectionChanged_gridOrdersPositions(s, e) {
            if (s.GetSelectedRowCount() > 0)
                clientBtnConfirm.SetEnabled(true);
            else
                clientBtnConfirm.SetEnabled(false);
        }

        function Dobavitelj_ValueChanged(s, e) {
            gridOrdersPositions.PerformCallback('SupplierChanged');
        }

        var isBtnConfirmIntiated = false;
        function btnConfirm_Click(s, e) {
            e.processOnServer = !isBtnConfirmIntiated;
            isBtnConfirmIntiated = true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="row small-padding-bottom">
        <div class="col-md-4">
            <div class="row2" style="align-items: center;">
                <div class="col-xs-4 no-padding-right">
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="DOBAVITELJ : "></dx:ASPxLabel>
                </div>
                <div class="col-xs-8 no-padding-left">
                    <dx:ASPxGridLookup ID="ASPxGridLookupDobavitelj" runat="server" ClientInstanceName="lookUpDobavitelj"
                        KeyFieldName="Dobavitelj" TextFormatString="{0}" CssClass="text-box-input"
                        Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                        OnLoad="ASPxGridLookupLoad_WidthSmall" OnDataBinding="ASPxGridLookupDobavitelj_DataBinding" IncrementalFilteringMode="Contains">
                        <ClearButton DisplayMode="OnHover" />
                        <ClientSideEvents ValueChanged="ValueChanged_lookUpStranke" />
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

                            <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Dobavitelj" Width="100%"
                                ReadOnly="true" ShowInCustomizationForm="True">
                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            </dx:GridViewDataTextColumn>

                        </Columns>
                        <ClientSideEvents ValueChanged="Dobavitelj_ValueChanged" />
                    </dx:ASPxGridLookup>
                </div>
            </div>
        </div>
    </div>
    <dx:ASPxGridView ID="ASPxGridViewOrdersPositions" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridOrdersPositions"
        Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridView1_DataBinding"
        KeyFieldName="tempID" CssClass="gridview-no-header-padding" OnCustomCallback="ASPxGridViewOrdersPositions_CustomCallback"
        OnHtmlRowPrepared="ASPxGridViewOrdersPositions_HtmlRowPrepared"
        OnCommandButtonInitialize="ASPxGridViewOrdersPositions_CommandButtonInitialize">
        <ClientSideEvents SelectionChanged="OnSelectionChanged_gridOrdersPositions" />
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
        <SettingsText EmptyDataRow="Trenutno ni podatka o naročilih. Dodaj novega." />
        <Columns>
            <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="80px" SelectAllCheckboxMode="None" Caption="Izberi" ShowClearFilterButton="true" />
            <dx:GridViewDataTextColumn Caption="Apl" FieldName="TipAplikacije" Width="70px"
                ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Naročilnica" FieldName="Narocilnica" Width="160px"
                ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>

            

            <dx:GridViewDataTextColumn Caption="Št. pozicije" FieldName="St_Pozicija" Width="110px" Visible="false"
                ReadOnly="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Potrditev naročila" FieldName="Order_Confirm" Width="100px"
                ReadOnly="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>

            <%--<dx:GridViewDataTextColumn Caption="Tovarna" FieldName="Tovarna" Width="15%"
                ReadOnly="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>--%>

            <dx:GridViewDataTextColumn Caption="Artikel"
                FieldName="Artikel" ShowInCustomizationForm="True"
                Width="400px">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataDateColumn FieldName="Datum_narocila" Caption="Datum naročila" ShowInCustomizationForm="True" Width="100px">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                </PropertiesDateEdit>
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataDateColumn FieldName="Datum_Dobave" Caption="Datum dobave" ShowInCustomizationForm="True" Width="100px">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                </PropertiesDateEdit>
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataDateColumn>

            <%--<dx:GridViewDataTextColumn Caption="Dobavitelj" FieldName="Dobavitelj" Width="15%"
                ReadOnly="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>--%>

            <%-- <dx:GridViewDataTextColumn Caption="Kupec" FieldName="Kupec" Width="15%"
                ReadOnly="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>--%>

            <dx:GridViewDataTextColumn Caption="Naročeno" FieldName="Naroceno" Width="100px"
                ReadOnly="true" ShowInCustomizationForm="True">
                <PropertiesTextEdit DisplayFormatString="N3" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Odpoklicana količina" FieldName="OdpoklicKolicinaOTP" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True">
                <PropertiesTextEdit DisplayFormatString="N3" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Prevzeto" FieldName="Prevzeto" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True">
                <PropertiesTextEdit DisplayFormatString="N3" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Razlika" FieldName="Razlika" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True">
                <PropertiesTextEdit DisplayFormatString="N3" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Zaloga" FieldName="Zaloga" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True">
                <PropertiesTextEdit DisplayFormatString="N3" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Optimalna zaloga" FieldName="Dovoljeno_Odpoklicati" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True">
                <PropertiesTextEdit DisplayFormatString="N0" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Tip" FieldName="Tip" Width="300px"
                ReadOnly="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Interno" FieldName="Interno" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Proizvedeno" FieldName="Proizvedeno" Width="120px"
                ReadOnly="true" ShowInCustomizationForm="True">
                <PropertiesTextEdit DisplayFormatString="N3" />
            </dx:GridViewDataTextColumn>

        </Columns>
    </dx:ASPxGridView>
    <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
        <div style="margin-top: 20px; color: red;">* Opomba: <strong>Sivo</strong> obarvane vrstice ne vsebujejo potrditve naročila</div>
        <div class="AddEditButtonsElements">
            <span class="AddEditButtons">
                <dx:ASPxButton Theme="Moderno" ID="btnConfirm" runat="server" Text="Naprej" AutoPostBack="false" OnClick="btnConfirm_Click"
                    Height="25" Width="90" ClientEnabled="false" ClientInstanceName="clientBtnConfirm">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                    <ClientSideEvents Click="btnConfirm_Click" />
                </dx:ASPxButton>
            </span>
        </div>
    </div>

</asp:Content>
