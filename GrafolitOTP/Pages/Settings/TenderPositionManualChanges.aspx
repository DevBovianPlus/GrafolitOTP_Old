<%@ Page Title="Seznam ročnih sprememb pri razpisih" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TenderPositionManualChanges.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Settings.TenderPositionManualChanges" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxCallbackPanel ID="TPManualChangesCallbackPanel" runat="server" OnCallback="TPManualChangesCallbackPanel_Callback" ClientInstanceName="clientTPManualChangesCallbackPanel">
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxGridView ID="ASPxGridViewTPManualChanges" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridTPManualChanges"
                    OnDataBinding="ASPxGridViewTPManualChanges_DataBinding" Width="100%"
                    KeyFieldName="RazpisPozicijaSpremembeID" CssClass="gridview-no-header-padding">
                    <ClientSideEvents RowDblClick="RowDoubleClick" />
                    <Paddings Padding="0" />
                    <Settings ShowVerticalScrollBar="True"
                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400" AutoFilterCondition="Contains"
                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                        <PageSizeItemSettings Visible="true" Items="50,80,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                        </PageSizeItemSettings>
                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                    </SettingsPager>
                    <SettingsBehavior AllowFocusedRow="true" AllowEllipsisInText="true" />
                    <Styles Header-Wrap="True">
                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                    </Styles>
                    <SettingsText EmptyDataRow="Trenutno ni podatka o ¸zaposlenih. Dodaj novega." />
                    <Columns>

                        <dx:GridViewDataTextColumn Caption="ID" FieldName="RazpisPozicijaSpremembeID" Visible="false" />
                        <dx:GridViewDataTextColumn Caption="Stara cena" FieldName="StaraCena" Width="4%"/>
                        <dx:GridViewDataTextColumn Caption="Nova cena" FieldName="NovaCena" Width="4%" />

                        <dx:GridViewDataTextColumn Caption="Relacija" FieldName="Relacija.Naziv" Width="30%"/>
                        <dx:GridViewDataTextColumn Caption="Dobavitelj" FieldName="Stranka.NazivPrvi" Width="12%" />
                        <dx:GridViewDataTextColumn Caption="Zbirnik ton" FieldName="ZbirnikTon.Naziv" Width="7%" />
                        <dx:GridViewDataTextColumn Caption="Vnos" FieldName="VnosTS" Width="12%"  SortOrder="Descending"/>
                        <dx:GridViewDataTextColumn Caption="Spremembe" FieldName="SpremembeTS" Width="12%"  SortOrder="Descending"/>

                      <%--  <dx:GridViewDataTextColumn Caption="Prejemnik" FieldName="EmailTo" Width="12%" />

                        <dx:GridViewDataTextColumn Caption="Zadeva" FieldName="EmailSubject" Width="20%" />

                        <dx:GridViewDataTextColumn Caption="Telo" FieldName="EmailBody" Width="40%" />--%>

                    </Columns>
                </dx:ASPxGridView>

             

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

</asp:Content>
