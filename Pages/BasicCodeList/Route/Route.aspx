<%@ Page Title="Relacije" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Route.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.BasicCodeList.Route.Route" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script>
        function ShowPopUp(s, e) {
            parameter = HandleUserActionsOnTabs(gridRoute, clientBtnAdd, clientBtnEdit, clientBtnDelete, s);
            clientCallbackPanelRoute.PerformCallback(parameter);
        }


        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'Route':
                            clientPopUpRoute.Hide();
                            clientCallbackPanelRoute.PerformCallback("RefreshGrid");
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'Route':
                            clientPopUpRoute.Hide();
                    }
                    break;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxCallbackPanel ID="CallbackPanelRoute" ClientInstanceName="clientCallbackPanelRoute" runat="server" Width="100%"
        OnCallback="CallbackPanelRoute_Callback">
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxGridView ID="ASPxGridViewRoute" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridRoute"
                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewRoute_DataBinding"
                    KeyFieldName="RelacijaID" CssClass="gridview-no-header-padding">
                    <ClientSideEvents RowDblClick="ShowPopUp" />
                    <Paddings Padding="0" />
                    <Settings ShowVerticalScrollBar="True"
                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400"
                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                    <SettingsPager PageSize="100" ShowNumericButtons="false" AlwaysShowPager="false" >
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
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="RelacijaID" Width="80px"
                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True" SortOrder="Descending">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Koda" FieldName="Koda" Width="10%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="15%"
                            ReadOnly="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Dolzina"
                            FieldName="Dolzina" ShowInCustomizationForm="True"
                            Width="25%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataDateColumn Caption="Datum" FieldName="Datum" Width="10%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy" />
                        </dx:GridViewDataDateColumn>
                    </Columns>
                </dx:ASPxGridView>

                <dx:ASPxPopupControl ID="ASPxPopupControlRoute" runat="server" ContentUrl="Route_popup.aspx"
                    ClientInstanceName="clientPopUpRoute" Modal="True" HeaderText="RELACIJA"
                    CloseAction="CloseButton" Width="650px" Height="480px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true"
                    OnWindowCallback="ASPxPopupControlRoute_WindowCallback">
                    <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                    <ContentStyle BackColor="#F7F7F7">
                        <Paddings PaddingBottom="0px" PaddingLeft="6px" PaddingRight="6px" PaddingTop="0px"></Paddings>
                    </ContentStyle>
                </dx:ASPxPopupControl>

                <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
                    <div class="DeleteButtonElements">
                        <span class="AddEditButtons">
                            <dx:ASPxButton Theme="Moderno" ID="btnDelete" runat="server" Text="Izbriši" AutoPostBack="false"
                                Height="25" Width="50" ClientInstanceName="clientBtnDelete">
                                <ClientSideEvents Click="ShowPopUp" />
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../../../Images/trash.png" UrlHottracked="../../../Images/trashHover.png" />
                            </dx:ASPxButton>
                        </span>
                    </div>
                    <div class="AddEditButtonsElements">
                        <span class="AddEditButtons">
                            <dx:ASPxButton Theme="Moderno" ID="btnAdd" runat="server" Text="Dodaj" AutoPostBack="false"
                                Height="25" Width="90" ClientInstanceName="clientBtnAdd">
                                <ClientSideEvents Click="ShowPopUp" />
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../../../Images/add.png" UrlHottracked="../../../Images/addHover.png" />
                            </dx:ASPxButton>
                        </span>
                        <span class="AddEditButtons">
                            <dx:ASPxButton Theme="Moderno" ID="btnEdit" runat="server" Text="Spremeni" AutoPostBack="false"
                                Height="25" Width="90" ClientInstanceName="clientBtnEdit">
                                <ClientSideEvents Click="ShowPopUp" />
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../../../Images/edit.png" UrlHottracked="../../../Images/editHover.png" />
                            </dx:ASPxButton>
                        </span>
                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
