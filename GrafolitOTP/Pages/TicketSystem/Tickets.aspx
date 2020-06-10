<%@ Page Title="Ticket sistem" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Tickets.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.TicketSystem.Tickets" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">
        function DoubleClick(s, e)
        {
            gridTickets.GetRowValues(gridTickets.GetFocusedRowIndex(), 'TicketID', OnGetRowValues);
        }

        function OnGetRowValues(value) {
            gridTickets.PerformCallback('DblClick;' + value);
        }

        function gridTickets_EndCallback(s, e)
        { }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxGridView ID="ASPxGridViewTickets" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridTickets"
        Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewTickets_DataBinding"
        KeyFieldName="TicketID" CssClass="gridview-no-header-padding" OnCustomCallback="ASPxGridViewTickets_CustomCallback"
        OnHtmlRowPrepared="ASPxGridViewTickets_HtmlRowPrepared" OnCommandButtonInitialize="ASPxGridViewTickets_CommandButtonInitialize">
        <ClientSideEvents RowDblClick="DoubleClick" EndCallback="gridTickets_EndCallback" />
        <Paddings Padding="0" />
        <Settings ShowVerticalScrollBar="True"
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
        <SettingsText EmptyDataRow="Trenutno ni podatka o težavah. Dodaj novo." />
        <Columns>
            <dx:GridViewDataTextColumn Caption="ID" FieldName="TicketID" Width="80px"
                ReadOnly="true" ShowInCustomizationForm="True" SortOrder="Descending">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Prioriteta" FieldName="Prioriteta" Width="7%"
                ReadOnly="true" >
                <DataItemTemplate>
                    <dx:ASPxImage runat="server" ID="priorityImage" OnInit="priorityImage_Init"></dx:ASPxImage>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Tip" FieldName="Tip" Width="16%"
                ReadOnly="true" ShowInCustomizationForm="True">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="25%"
                ReadOnly="true" ShowInCustomizationForm="True">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <%--<dx:GridViewDataTextColumn Caption="Tip"
                FieldName="TipNaziv" ShowInCustomizationForm="True"
                Width="25%">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>--%>

            <dx:GridViewDataTextColumn Caption="Status"
                FieldName="StatusNaziv" ShowInCustomizationForm="True"
                Width="10%">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Uporabnik"
                FieldName="User" ShowInCustomizationForm="True"
                Width="10%">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Uporabnik"
                FieldName="User" ShowInCustomizationForm="True"
                Width="10%">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>
    <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
        <div class="DeleteButtonElements">
            <span class="AddEditButtons">
                <dx:ASPxButton Theme="Moderno" ID="btnDelete" runat="server" Text="Izbriši" AutoPostBack="false" OnClick="btnDelete_Click"
                    Height="25" Width="50">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/trash.png" UrlHottracked="../../Images/trashHover.png" />
                </dx:ASPxButton>
            </span>
        </div>
        <div class="AddEditButtonsElements">
            <span class="AddEditButtons">
                <dx:ASPxButton Theme="Moderno" ID="btnAdd" runat="server" Text="Dodaj" AutoPostBack="false" OnClick="btnAdd_Click"
                    Height="25" Width="90">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                </dx:ASPxButton>
            </span>
            <span class="AddEditButtons">
                <dx:ASPxButton Theme="Moderno" ID="btnEdit" runat="server" Text="Spremeni" AutoPostBack="false" OnClick="btnEdit_Click"
                    Height="25" Width="90">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/edit.png" UrlHottracked="../../Images/editHover.png" />
                </dx:ASPxButton>
            </span>
        </div>
    </div>
</asp:Content>
