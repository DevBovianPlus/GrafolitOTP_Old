<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="CreateOrder_popup.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Recall.CreateOrder_popup" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/Popup.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolderPopup" runat="server">
    <script type="text/javascript">

        var isPostbackInitiated = false;
        function btnCreateOrder_Click(s, e) {
            if (!isPostbackInitiated) {
                LoadingPanel.Show();
                e.processOnServer = !isPostbackInitiated;
                isPostbackInitiated = true;
            }
            else
                e.processOnServer = false;
        }

        function gridServices_EndCallback(s, e) { }

        function btnAddOrderPosition_Click(s, e) {
            gridServices.UpdateEdit();
            gridServices.PerformCallback("AddServicePosition");
        }

        function gridServices_BatchEditStartEditing(s, e) {
            clientBtnSaveChanges.SetEnabled(true);
            clientBtnCancelChanges.SetEnabled(true);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolderPopup" runat="server">
    <div class="row small-padding-bottom" style="margin-top: 15px;">
        <div class="col-xs-12 no-padding-right">
            <div class="row2 align-item-centerV-startH">
                <div class="col-sm-12 no-padding-left">

                    <div class="row2 align-item-centerV-startH big-margin-b">
                        <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 22px;">
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="OPOMBE : "></dx:ASPxLabel>
                        </div>
                        <div class="col-xs-12 no-padding-left">
                            <dx:ASPxHtmlEditor ID="HtmlEditorNotes" runat="server" Width="100%" CssClass="text-box-input" Font-Size="Medium">
                                <Settings AllowDesignView="false" AllowHtmlView="false" AllowPreview="false"></Settings>
                                <Styles ViewArea-Font-Size="Medium"></Styles>
                            </dx:ASPxHtmlEditor>
                        </div>
                    </div>
                    <div class="row2 align-item-centerV-startH">
                        <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 22px;">
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="STORITVE "></dx:ASPxLabel>
                        </div>
                        <div class="col-xs-12 no-padding-left">
                            <dx:ASPxGridView ID="ASPxGridViewServices" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridServices"
                                Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewServices_DataBinding"
                                KeyFieldName="ServiceID" CssClass="gridview-no-header-padding" OnCustomCallback="ASPxGridViewServices_CustomCallback"
                                OnBatchUpdate="ASPxGridViewServices_BatchUpdate">
                                <ClientSideEvents EndCallback="gridServices_EndCallback" BatchEditStartEditing="gridServices_BatchEditStartEditing" />
                                <Paddings Padding="0" />
                                <Settings ShowVerticalScrollBar="True"
                                    ShowFilterBar="Auto" ShowFilterRow="false" VerticalScrollableHeight="200"
                                    ShowFilterRowMenu="false" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" />
                                <SettingsPager PageSize="50" ShowNumericButtons="true">
                                    <%--<PageSizeItemSettings Visible="true" Items="50,80,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                                            </PageSizeItemSettings>--%>
                                    <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                </SettingsPager>
                                <SettingsBehavior AllowFocusedRow="true" AllowSelectSingleRowOnly="false" AllowEllipsisInText="true" />
                                <Styles Header-Wrap="True">
                                    <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                    <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                </Styles>
                                <SettingsText EmptyDataRow="Trenutno ni podatka o storitvah." />
                                <SettingsEditing Mode="Batch" BatchEditSettings-StartEditAction="DblClick" />
                                <SettingsCommandButton>
                                    <DeleteButton Text="Izbriši"></DeleteButton>
                                </SettingsCommandButton>
                                <Columns>
                                    <%--<dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="80px" SelectAllCheckboxMode="AllPages" Caption="Izberi" ShowClearFilterButton="true" />--%>

                                    <dx:GridViewCommandColumn Caption="Briši"
                                            ShowDeleteButton="true" Width="10%" VisibleIndex="0"  />

                                    <dx:GridViewDataTextColumn Caption="Štev." FieldName="ServiceID" Width="6%"
                                        EditFormSettings-Visible="False">
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Koda" FieldName="Code" Width="14%">
                                        <PropertiesTextEdit DisplayFormatInEditMode="true">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Name" Width="45%">
                                        <PropertiesTextEdit DisplayFormatInEditMode="true">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Količina"
                                        FieldName="Quantity" Width="10%">
                                        <PropertiesTextEdit DisplayFormatString="N3" DisplayFormatInEditMode="true">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="EM" FieldName="UnitOfMeasure" Width="10%">
                                        <PropertiesTextEdit DisplayFormatInEditMode="true">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Cena"
                                        FieldName="Price" Width="10%" PropertiesTextEdit-DisplayFormatString="N3">
                                        <PropertiesTextEdit DisplayFormatString="N3" DisplayFormatInEditMode="true">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                </Columns>

                                <Templates>
                                    <StatusBar>
                                        <div class="row">
                                            <div class="col-xs-6 align-item-centerV-startH">
                                                <span class="AddEditButtons no-margin-top">
                                                    <dx:ASPxButton ID="btnAddOrderPosition" runat="server" Text="Dodaj pozicijo" AutoPostBack="false" CssClass="statusBarButton"
                                                        Height="40" Width="110" UseSubmitBehavior="false" ClientInstanceName="clientBtnAddOrderPosition">
                                                        <ClientSideEvents Click="btnAddOrderPosition_Click" />
                                                        <DisabledStyle CssClass="statusBarButtonsDisabled"></DisabledStyle>
                                                    </dx:ASPxButton>
                                                </span>
                                            </div>
                                            <div class="col-xs-6 text-right">
                                                <span class="AddEditButtons no-margin-top">
                                                    <dx:ASPxButton ID="btnSaveChanges" runat="server" Text="Spremeni" AutoPostBack="false" CssClass="statusBarButton"
                                                        Height="40" Width="110" UseSubmitBehavior="false" ClientEnabled="false" ClientInstanceName="clientBtnSaveChanges">
                                                        <DisabledStyle CssClass="statusBarButtonsDisabled" />
                                                        <ClientSideEvents Click="function(s,e) { gridServices.UpdateEdit(); }" />
                                                    </dx:ASPxButton>
                                                </span>
                                                <span class="AddEditButtons no-margin-top">
                                                    <dx:ASPxButton ID="btnCancelChanges" runat="server" Text="Prekliči" AutoPostBack="false" CssClass="statusBarButton"
                                                        Height="40" Width="110" UseSubmitBehavior="false" ClientEnabled="false" ClientInstanceName="clientBtnCancelChanges">
                                                        <DisabledStyle CssClass="statusBarButtonsDisabled" />
                                                        <ClientSideEvents Click="function(s,e) { gridServices.CancelEdit(); }" />
                                                    </dx:ASPxButton>
                                                </span>
                                            </div>
                                        </div>
                                    </StatusBar>
                                </Templates>
                            </dx:ASPxGridView>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">

        <div class="AddEditButtonsElements">
            <span class="AddEditButtons">
                <dx:ASPxButton Theme="Moderno" ID="btnCreateOrder" runat="server" Text="Ustvari naročilo" AutoPostBack="false" OnClick="btnConfirm_Click"
                    Height="25" Width="90" ClientInstanceName="clientBtnSelectCarrier">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/addPopup.png" UrlHottracked="../../Images/addPopup.png" />
                    <ClientSideEvents Click="btnCreateOrder_Click" />
                </dx:ASPxButton>
            </span>

            <span class="AddEditButtons">
                <dx:ASPxButton Theme="Moderno" ID="btnCancel" runat="server" Text="Prekliči naročilo" AutoPostBack="false"
                    Height="25" Width="90" OnClick="btnCancel_Click" ClientVisible="false">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/cancelPopUp.png" UrlHottracked="../../Images/cancelPopUp.png" />
                </dx:ASPxButton>
            </span>
        </div>

    </div>
</asp:Content>
