<%@ Page Title="Tip prevoza" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TransportType.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.BasicCodeList.TransportType.TransportType" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">
        function ShowPopUp(s, e) {
            parameter = HandleUserActionsOnTabs(gridTransportTypes, clientBtnAdd, clientBtnEdit, clientBtnDelete, s);
            clientTransportTypeCallbackPanel.PerformCallback(parameter);
        }

        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'TransportType':
                            clientPopUpTransportType.Hide();
                            gridTransportTypes.Refresh();
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'TransportType':
                            clientPopUpTransportType.Hide();
                    }
                    break;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxCallbackPanel runat="server" ID="TransportTypeCallbackPanel" OnCallback="TransportTypeCallbackPanel_Callback" ClientInstanceName="clientTransportTypeCallbackPanel">
        <PanelCollection>
            <dx:PanelContent>
                <div class="row2">
                    <div class="col-md-12 no-padding-left no-padding-right">
                        <dx:ASPxGridView ID="ASPxGridViewTransportTypes" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridTransportTypes"
                            Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewTransportTypes_DataBinding"
                            KeyFieldName="TipPrevozaID" CssClass="gridview-no-header-padding" OnCustomColumnDisplayText="ASPxGridViewTransportTypes_CustomColumnDisplayText">
                            <ClientSideEvents RowDblClick="ShowPopUp" />
                            <Paddings Padding="0" />
                            <Settings ShowVerticalScrollBar="True"
                                ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400"
                                ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                            <SettingsPager PageSize="10" ShowNumericButtons="true">
                                <PageSizeItemSettings Visible="false" Items="10,20,30" Caption="Zapisi na stran : " AllItemText="Vsi">
                                </PageSizeItemSettings>
                                <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                            </SettingsPager>
                            <SettingsBehavior AllowFocusedRow="true" />
                            <Styles Header-Wrap="True">
                                <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                            </Styles>
                            <SettingsText EmptyDataRow="Trenutno ni podatka o tipih prevoza. Dodaj novega." />
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="ID" FieldName="TipPrevozaID" Width="80px"
                                    ReadOnly="true" Visible="false">
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Koda" FieldName="Koda" Width="20%">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="50%">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Dovoljena teža" CellStyle-HorizontalAlign="Right"
                                    FieldName="DovoljenaTeza" Width="15%">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Shrani pozicije"
                                    FieldName="ShranjevanjePozicij" Width="15%">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Opombe"
                                    FieldName="Opombe" Width="30%">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                            </Columns>
                        </dx:ASPxGridView>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="DeleteButtonElements">
                            <span class="AddEditButtons">
                                <dx:ASPxButton Theme="Moderno" ID="btnDelete" runat="server" Text="Izbriši" AutoPostBack="false"
                                    Height="25" Width="50" ClientInstanceName="clientBtnDelete">
                                    <Paddings PaddingLeft="10" PaddingRight="10" />
                                    <ClientSideEvents Click="ShowPopUp" />
                                    <Image Url="../../../Images/trash.png" UrlHottracked="../../../Images/trashHover.png" />
                                </dx:ASPxButton>
                            </span>
                        </div>
                        <div class="AddEditButtonsElements">
                            <span class="AddEditButtons">
                                <dx:ASPxButton Theme="Moderno" ID="btnAdd" runat="server" Text="Dodaj" AutoPostBack="false"
                                    Height="25" Width="90" ClientInstanceName="clientBtnAdd">
                                    <Paddings PaddingLeft="10" PaddingRight="10" />
                                    <ClientSideEvents Click="ShowPopUp" />
                                    <Image Url="../../../Images/add.png" UrlHottracked="../../../Images/addHover.png" />
                                </dx:ASPxButton>
                            </span>
                            <span class="AddEditButtons">
                                <dx:ASPxButton Theme="Moderno" ID="btnEdit" runat="server" Text="Spremeni" AutoPostBack="false"
                                    Height="25" Width="90" ClientInstanceName="clientBtnEdit">
                                    <Paddings PaddingLeft="10" PaddingRight="10" />
                                    <ClientSideEvents Click="ShowPopUp" />
                                    <Image Url="../../../Images/edit.png" UrlHottracked="../../../Images/editHover.png" />
                                </dx:ASPxButton>
                            </span>
                        </div>
                    </div>
                </div>

                <dx:ASPxPopupControl ID="ASPxPopupControlTransportType" runat="server" ContentUrl="TransportType_popup.aspx"
                    ClientInstanceName="clientPopUpTransportType" Modal="True" HeaderText="TIP PREVOZA"
                    CloseAction="CloseButton" Width="700px" Height="500px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true"
                    OnWindowCallback="ASPxPopupControlTransportType_WindowCallback">
                    <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                    <ContentStyle BackColor="#F7F7F7">
                        <Paddings PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px" PaddingTop="0px"></Paddings>
                    </ContentStyle>
                </dx:ASPxPopupControl>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
