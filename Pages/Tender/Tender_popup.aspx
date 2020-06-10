<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="Tender_popup.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Tender.Tender_popup" %>

<%@ MasterType VirtualPath="~/Popup.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolderPopup" runat="server">
    <script>
        function CheckFieldValidation(s, e) {
            var process = false;
            var lookUpItems = [lookUpStranke, lookUpRelacija];
            var inputItems = [clientTxtCena];

            process = InputFieldsValidation(lookUpItems, inputItems, null, null, null, null);

            if (clientBtnConfirm.GetText() == 'Izbriši')
                process = true;

            if (process)
                e.processOnServer = true;
            else
                e.processOnServer = false;

            return process;
        }

        function OnClosePopUpHandler(command, sender, quickAddRouteID) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'Route':
                            clientPopUpRoute.Hide();
                            clientCllbackPanelQuickAddRoute.PerformCallback(quickAddRouteID);
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

        function addNewRoute(s, e)
        {
            clientCllbackPanelQuickAddRoute.PerformCallback("AddNewRoute");
        }
        function OnEndCallbackQuickAddRoute(s, e)
        {
            if (s.cpNewRouteID != "" && s.cpNewRouteID !== undefined) {
                lookUpRelacija.SetValue(s.cpNewRouteID);
                delete (s.cpNewRouteID);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolderPopup" runat="server">
    <div class="small-padding-top">
        <ul class="nav nav-tabs">
            <li class="active"><a data-toggle="tab" href="#basic"><span class="glyphicon glyphicon-import"></span>Osnovni podatki</a></li>
            <li><a data-toggle="tab" href="#attachments"><span class="badge" runat="server" id="attachmentBadge">0</span> Priloge</a></li>
        </ul>
        <div class="tab-content">
            <div id="basic" class="tab-pane fade in active">
                <div class="panel panel-default" style="border-top: none; background-color: transparent;">
                    <div class="panel-body">
                        <div class="row small-padding-bottom">
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center">
                                    <div class="col-xs-3 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="DATUM : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-9 no-padding-left">
                                        <dx:ASPxDateEdit ID="DateEditDatumRAzpisa" runat="server" EditFormat="Date" Width="170" Theme="Moderno"
                                            CssClass="text-box-input date-edit-padding" Font-Size="13px"
                                            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                                            <FocusedStyle CssClass="focus-text-box-input" />
                                            <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                            <DropDownButton Visible="true"></DropDownButton>
                                        </dx:ASPxDateEdit>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row small-padding-bottom">
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center">
                                    <div class="col-xs-3 no-padding-right">
                                        <dx:ASPxLabel ID="lblRelacija" runat="server" Text="RELACIJA : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-8 no-padding-left">
                                        <dx:ASPxCallbackPanel ID="CllbackPanelQuickAddRoute" runat="server" Width="100%"
                                            ClientInstanceName="clientCllbackPanelQuickAddRoute" OnCallback="CllbackPanelQuickAddRoute_Callback">
                                            <ClientSideEvents EndCallback="OnEndCallbackQuickAddRoute" />
                                            <PanelCollection>
                                                <dx:PanelContent>

                                                    <dx:ASPxGridLookup ID="ASPxGridLookupRelacija" runat="server" ClientInstanceName="lookUpRelacija"
                                                        KeyFieldName="RelacijaID" TextFormatString="{1}" CssClass="text-box-input"
                                                        Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                                        OnDataBinding="ASPxGridLookupRelacija_DataBinding"
                                                        OnLoad="ASPxGridLookupLoad_WidthMedium">
                                                        <ClearButton DisplayMode="OnHover" />
                                                        <ClientSideEvents Init="SetFocus" DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Naziv').Focus();}" />
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
                                                            <dx:GridViewDataTextColumn Caption="Relacija Id" FieldName="RelacijaID" Width="80px"
                                                                ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="40%"
                                                                ReadOnly="true" ShowInCustomizationForm="True">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn Caption="Dolžina"
                                                                FieldName="Dolzina" ShowInCustomizationForm="True"
                                                                Width="35%">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn Caption="Datum"
                                                                FieldName="Datum" ShowInCustomizationForm="True"
                                                                Width="25%">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>

                                                        </Columns>
                                                    </dx:ASPxGridLookup>

                                                    <dx:ASPxPopupControl ID="ASPxPopupControlRoute" runat="server" ContentUrl="../BasicCodeList/Route/Route_popup.aspx"
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

                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxCallbackPanel>
                                    </div>
                                    <div class="col-xs-1 no-padding-left">
                                        <dx:ASPxButton ID="btnQuickaddRoute" runat="server" AutoPostBack="False" AllowFocus="False" RenderMode="Link" EnableTheming="False"
                                            CssClass="add-route" ToolTip="Dodaj novo relacijo" UseSubmitBehavior="false">
                                            <ClientSideEvents Click="addNewRoute" />
                                            <Image>
                                                <SpriteProperties CssClass="add-route" />
                                            </Image>
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row small-padding-bottom">
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center;">
                                    <div class="col-xs-3 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="PREVOZNIK : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-9 no-padding-left">
                                        <dx:ASPxGridLookup ID="ASPxGridLookupStranke" runat="server" ClientInstanceName="lookUpStranke"
                                            KeyFieldName="idStranka" TextFormatString="{1}" CssClass="text-box-input"
                                            Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                            OnDataBinding="ASPxGridLookupStranke_DataBinding"
                                            OnLoad="ASPxGridLookupLoad_WidthMedium">
                                            <ClearButton DisplayMode="OnHover" />
                                            <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Stranka.NazivPrvi').Focus();}" />
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
                                                <dx:GridViewDataTextColumn Caption="Stranka Id" FieldName="idStranka" Width="80px"
                                                    ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Naziv" FieldName="NazivPrvi" Width="40%"
                                                    ReadOnly="true" ShowInCustomizationForm="True">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Email"
                                                    FieldName="Email" ShowInCustomizationForm="True"
                                                    Width="35%">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Telefon"
                                                    FieldName="Telefon" ShowInCustomizationForm="True"
                                                    Width="25%">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>

                                            </Columns>
                                        </dx:ASPxGridLookup>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row small-padding-bottom">
                            <div class="col-md-6">
                                <div class="row2" style="align-items: center">
                                    <div class="col-xs-3 no-padding-right">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="CENA : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-9 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtCena" ClientInstanceName="clientTxtCena"
                                            CssClass="text-box-input" Font-Size="14px">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <ClientSideEvents KeyPress="isNumberKey_decimal" />
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="attachments" class="tab-pane fade">
                <div class="panel panel-default" style="border-top: none; background-color: transparent;">
                    <div class="panel-body">
                        Test
                    </div>
                </div>
            </div>
        </div>
        <div class="AddEditButtonsWrap">
            <div class="AddEditButtonsElements" style="margin-top: 10px; margin-bottom: 15px;">
                <span class="AddEditButtons">
                    <dx:ASPxButton ID="btnCancelPopUp" runat="server" Text="Prekliči" AutoPostBack="false"
                        Height="20" Width="80" OnClick="btnCancelPopUp_Click">
                        <Image Url="../../Images/cancelPopUp.png"></Image>
                    </dx:ASPxButton>
                </span>
                <span class="AddEditButtons">
                    <dx:ASPxButton ID="btnConfirmPopUp" runat="server" Text="Potrdi" AutoPostBack="false"
                        ValidationGroup="Confirm" ClientInstanceName="clientBtnConfirm" OnClick="btnConfirmPopUp_Click">
                        <ClientSideEvents Click="CheckFieldValidation" />
                    </dx:ASPxButton>
                </span>
            </div>
        </div>
    </div>
</asp:Content>
