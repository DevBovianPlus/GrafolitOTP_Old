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

                <dx:ASPxButton ID="btnExportManualChanges" runat="server" RenderMode="Link" ClientEnabled="true" OnClick="btnExportManualChanges_Click"
                    AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientbtnExportManualChanges" ToolTip="Izvozi v excel">
                    <DisabledStyle CssClass="icon-disabled" />
                    <HoverStyle CssClass="icon-hover" />
                    <Image Url="../../Images/pdf-export.png" Width="30px" />
                </dx:ASPxButton>
                <dx:ASPxGridViewExporter ID="ASPxGridViewExporterManualChanges" GridViewID="ASPxGridViewTPManualChanges" runat="server"></dx:ASPxGridViewExporter>

                <dx:ASPxGridView ID="ASPxGridViewTPManualChanges" runat="server" Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewTPManualChanges_DataBinding" KeyFieldName="RazpisPozicijaSpremembeID" CssClass="gridview-no-header-padding">                    
                    <%-- FocusedRowChanged="OnFocusedRowChanged_gridRecall" --%>
                    <Paddings Padding="0" />
                    <Settings ShowVerticalScrollBar="True" ShowHeaderFilterButton ="true"
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
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="RazpisPozicijaSpremembeID" Width="1%"
                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True" SortOrder="Descending">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Stara cena"
                            FieldName="StaraCena" ShowInCustomizationForm="True"
                            Width="5%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            <PropertiesTextEdit DisplayFormatString="c"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Nova cena"
                            FieldName="NovaCena" ShowInCustomizationForm="True"
                            Width="5%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            <PropertiesTextEdit DisplayFormatString="c"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Relacija" FieldName="Relacija.Naziv" Width="30%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Dobavitelj" FieldName="Stranka.NazivPrvi" Width="15%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Zbirnik ton" FieldName="ZbirnikTon.Naziv" Width="7%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>                        

                        <dx:GridViewDataDateColumn Caption="Vnos" FieldName="VnosTS" CellStyle-HorizontalAlign="Right" Width="7%">
                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy hh:mm:ss"></PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>

                        <dx:GridViewDataTextColumn Caption="Vnos oseba" FieldName="VnosOseba.UporabniskoIme" Width="7%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>                        

                         <dx:GridViewDataDateColumn Caption="Spremembe" FieldName="SpremembeTS" CellStyle-HorizontalAlign="Right" Width="7%">
                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy hh:mm:ss"></PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>

                        <dx:GridViewDataTextColumn Caption="Spremembe oseba" FieldName="SpremembeOseba.UporabniskoIme" Width="7%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>                        
                    </Columns>
                </dx:ASPxGridView>



            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

</asp:Content>
