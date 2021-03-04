<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="OrderBuyerPos_popup.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Recall.OrderBuyerPos_popup" %>

<%@ MasterType VirtualPath="~/Popup.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolderPopup" runat="server">
    <script type="text/javascript">
        function OnSelectionChanged_gridOrdersPositions(s, e) {
            if (s.GetSelectedRowCount() > 0)
                clientBtnConfirm.SetEnabled(true);
            else
                clientBtnConfirm.SetEnabled(false);
        }

        function btnConfirm_Click(s, e) {
            clientLoadingPanel.Show();            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolderPopup" runat="server">
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="clientLoadingPanel" Modal="true"></dx:ASPxLoadingPanel>
    <div class="row small-padding-bottom">
        <div class="col-xs-12 no-padding-left no-padding-right">
            <dx:ASPxGridView ID="ASPxGridViewOrdersBuyerPositions" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridOrdersPositions"
                Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewOrdersBuyerPositions_DataBinding"
                KeyFieldName="TempID" CssClass="gridview-no-header-padding">
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
                    <dx:GridViewDataTextColumn Caption="acKey" FieldName="acKey" Width="8%"
                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Ključ" FieldName="Kljuc" Width="8%"
                        ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataDateColumn Caption="Datum" FieldName="Datum" Width="8%"
                        ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                        <PropertiesDateEdit DisplayFormatString="dd.MM.yyyy"></PropertiesDateEdit>
                    </dx:GridViewDataDateColumn>



                    <dx:GridViewDataTextColumn Caption="Valuta" FieldName="Valuta" Width="5%" Visible="false"
                        ReadOnly="true" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Kupec" FieldName="Kupec" Width="30%"
                        ReadOnly="true" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Prevzemnik" FieldName="Prevzemnik" Width="30%"
                        ReadOnly="true" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Skupna količina"
                        FieldName="Kolicina" ShowInCustomizationForm="True"
                        Width="8%">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        <PropertiesTextEdit DisplayFormatString="f2"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Skupna vrednost"
                        FieldName="ZnesekFakture" ShowInCustomizationForm="True"
                        Width="8%">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        <PropertiesTextEdit DisplayFormatString="c"></PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>

                </Columns>
            </dx:ASPxGridView>
        </div>
    </div>
    <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
        <div style="margin-top: 20px; color: red;">* Opomba: <strong>Sivo</strong> obarvane vrstice ne vsebujejo potrditve naročila</div>
        <div class="AddEditButtonsElements">
            <span class="AddEditButtons">
                <dx:ASPxButton Theme="Moderno" ID="btnConfirm" runat="server" Text="Potrdi izbiro" AutoPostBack="false" OnClick="btnConfirm_Click"
                    Height="25" Width="90" ClientEnabled="false" ClientInstanceName="clientBtnConfirm">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                    <ClientSideEvents Click="btnConfirm_Click" />
                </dx:ASPxButton>
            </span>
        </div>
    </div>
</asp:Content>
