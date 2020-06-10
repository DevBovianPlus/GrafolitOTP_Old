    <%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="OrderPos_popup.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Recall.OrderPos_popup" %>

<%@ MasterType VirtualPath="~/Popup.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolderPopup" runat="server">
    <script type="text/javascript">
        function OnSelectionChanged_gridOrdersPositions(s, e) {
            if (s.GetSelectedRowCount() > 0)
                clientBtnConfirm.SetEnabled(true);
            else
                clientBtnConfirm.SetEnabled(false);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolderPopup" runat="server">
    <div class="row small-padding-bottom">
        <div class="col-xs-12 no-padding-left no-padding-right">
            <dx:ASPxGridView ID="ASPxGridViewOrdersPositions" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridOrdersPositions"
                Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewOrdersPositions_DataBinding"
                KeyFieldName="tempID" CssClass="gridview-no-header-padding" OnCustomCallback="ASPxGridViewOrdersPositions_CustomCallback"
                OnHtmlRowPrepared="ASPxGridViewOrdersPositions_HtmlRowPrepared"
                OnCommandButtonInitialize="ASPxGridViewOrdersPositions_CommandButtonInitialize">
                <ClientSideEvents SelectionChanged="OnSelectionChanged_gridOrdersPositions" />
                <Paddings Padding="0" />
                <Settings ShowVerticalScrollBar="True"
                    ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="350"
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
                    <dx:GridViewDataTextColumn Caption="Naročilnica" FieldName="Narocilnica" Width="180px"
                        ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Št. pozicije" FieldName="St_Pozicija" Width="110px"
                        ReadOnly="true" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Potrditev naročila" FieldName="Order_Confirm" Width="180px"
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

                    <dx:GridViewDataDateColumn FieldName="Datum_narocila" Caption="Datum naročila" ShowInCustomizationForm="True" Width="150px">
                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                        </PropertiesDateEdit>
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataDateColumn>

                    <dx:GridViewDataDateColumn FieldName="Datum_Dobave" Caption="Datum dobave" ShowInCustomizationForm="True" Width="150px">
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

                    <dx:GridViewDataTextColumn Caption="Naročeno" FieldName="Naroceno" Width="120px"
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
        </div>
    </div>
    <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
        <div style="margin-top:20px; color:red;">* Opomba: <strong>Sivo</strong> obarvane vrstice ne vsebujejo potrditve naročila</div>
        <div class="AddEditButtonsElements">
            <span class="AddEditButtons">
                <dx:ASPxButton Theme="Moderno" ID="btnConfirm" runat="server" Text="Potrdi izbiro" AutoPostBack="false" OnClick="btnConfirm_Click"
                    Height="25" Width="90" ClientEnabled="false" ClientInstanceName="clientBtnConfirm">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                </dx:ASPxButton>
            </span>
        </div>
    </div>
</asp:Content>
