<%@ Page Title="Izdelava odpoklice za kupce" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RecallBuyerList.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Recall.RecallBuyerList" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script>
        function OnSelectionChanged_gridDisconnectedInvoices(s, e) {
            if (s.GetSelectedRowCount() > 0)
                clientBtnConfirm.SetEnabled(true);
            else
                clientBtnConfirm.SetEnabled(false);
            s.GetSelectedFieldValues("Kolicina", GetSelectedFieldValuesCallback);
        }

        function GetSelectedFieldValuesCallback(values) {
            var cntKolicina = 0;
            
            for (var i = 0; i < values.length; i++) {
              
                cntKolicina += values[i];              
            }

            clientQnt.Set("SelectQnt", cntKolicina);            



            clientKolicina.SetText(parseFloat(cntKolicina).toFixed(2) + " kg");

            //SetZbirnikByQnt(cntKolicina)
        }

        //function SetZbirnikByQnt(cntKolicina)
        //{
        //    var grid = lookUpZbirnikTon.GetGridView(); 

        //    for (var index = grid.GetTopVisibleIndex(); index < grid.GetVisibleRowsOnPage(); index++) {               
        //        //alert(index);
        //    }

        //    //var rowCount = lookUpZbirnikTon.GetVisibleRowsOnPage();
        //    //alert(rowCount);
        //    //for (let i = 0; i < rowCount; i++) {
        //    //    //lookUpZbirnikTon.batchEditApi.SetCellValue(i, column, value);
        //    //}  
        //}

        function btnConfirm_Click(s, e) {
            //e.processOnServer = !isBtnConfirmIntiated;
            //isBtnConfirmIntiated = true;
            LoadingPanel.Show();
            RecallCalbackPanel.PerformCallback("ClickNaprej");
        }

        function RecallCallbackPanel_EndCallback(s, e) {
            LoadingPanel.Hide();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <style type="text/css">
        .newFont, .newFont .dxeEditAreaSys, .newFont .dxeListBoxItem {
            font-size: 12px !important;
        }
    </style>
    <div class="row small-padding-bottom">
        <div class="col-md-4">
            <div class="row2 align-item-centerV-startH">
                <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 38px">
                    <dx:ASPxLabel ID="lblRelacija" runat="server" Text="RELACIJA : "></dx:ASPxLabel>
                </div>
                <div class="col-xs-10 no-padding-left">
                    <dx:ASPxGridLookup ID="ASPxGridLookupRealacija" runat="server" ClientInstanceName="lookUpRelacija"
                        KeyFieldName="RelacijaID" TextFormatString="{1}" CssClass="text-box-input"
                        Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                        OnLoad="ASPxGridLookupLoad_WidthLarge" OnDataBinding="ASPxGridLookupRealacija_DataBinding" IncrementalFilteringMode="Contains">
                        <ClearButton DisplayMode="OnHover" />
                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                        <GridViewStyles>
                            <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                            <FilterBarClearButtonCell></FilterBarClearButtonCell>
                        </GridViewStyles>
                        <GridViewProperties>
                            <SettingsBehavior EnableRowHotTrack="True" />
                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                            <SettingsPager ShowSeparators="True" AlwaysShowPager="false" PageSize="200">
                                <PageSizeItemSettings Visible="false"></PageSizeItemSettings>
                            </SettingsPager>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                ShowHorizontalScrollBar="true" VerticalScrollableHeight="200" GridLines="Both"></Settings>
                        </GridViewProperties>
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Relacija Id" FieldName="RelacijaID" Width="80px"
                                ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="100%"
                                ReadOnly="true" ShowInCustomizationForm="True">
                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn Caption="Dolžina"
                                FieldName="Dolzina" ShowInCustomizationForm="True"
                                Width="20%" Visible="false">
                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn Caption="Datum"
                                FieldName="Datum" ShowInCustomizationForm="True"
                                Width="15%" Visible="false">
                                
                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            </dx:GridViewDataTextColumn>

                        </Columns>
                    </dx:ASPxGridLookup>
                </div>
            </div>
        </div>
    </div>

    <dx:ASPxCallbackPanel ID="RecallCallbackPanel" ClientInstanceName="RecallCalbackPanel" runat="server" OnCallback="RecallCallbackPanel_Callback">
        <ClientSideEvents EndCallback="RecallCallbackPanel_EndCallback" />
        <SettingsLoadingPanel Enabled="false" />
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxHiddenField runat="server" ClientInstanceName="clientQnt" ID="hfQnt">
                </dx:ASPxHiddenField>
                <dx:ASPxGridView ID="ASPxGridViewDisconnectedInvoices" runat="server" ClientInstanceName="gridDisconnectedInvoices"
                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewDisconnectedInvoices_DataBinding"
                    KeyFieldName="TempID" CssClass="newFont">
                    <Paddings Padding="0" />
                    <Settings ShowVerticalScrollBar="True" ShowFooter="true" ShowStatusBar="Visible"
                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="550"
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
                    <ClientSideEvents SelectionChanged="OnSelectionChanged_gridDisconnectedInvoices"/>
                    <SettingsText EmptyDataRow="Trenutno ni podatka o odpoklicih. Dodaj novega."/>                    
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="80px" SelectAllCheckboxMode="None" Caption="Izberi" ShowClearFilterButton="true" />
                        <dx:GridViewDataTextColumn Caption="Ključ" FieldName="Kljuc" Width="8%"
                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataDateColumn Caption="Datum" FieldName="Datum" Width="5%"
                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                            <PropertiesDateEdit DisplayFormatString="dd.MM.yyy" EditFormatString="dd.MM.yyyy"></PropertiesDateEdit>  
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
                            Width="5%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            <PropertiesTextEdit DisplayFormatString="f2"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Skupna vrednost"
                            FieldName="ZnesekFakture" ShowInCustomizationForm="True"
                            Width="5%">
                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                            <PropertiesTextEdit DisplayFormatString="c"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:ASPxGridView>

                <div class="row">
                    <div class="col-md-1" style="padding-right: 30px">
                        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" ClientInstanceName="roundPanel" HeaderText="Izbrana kolicina" runat="server" Width="150" View="GroupBox">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <dx:ASPxLabel Text="0 kg" ID="lblKolicina" runat="server" ClientInstanceName="clientKolicina"></dx:ASPxLabel>
                                    <br />
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </div>



                    <div class="col-md-2">
                        <span class="AddEditButtons">
                            <dx:ASPxButton Theme="Moderno" ID="btnConfirm" runat="server" Text="Naprej" AutoPostBack="false"
                                Height="25" Width="90" ClientEnabled="false" ClientInstanceName="clientBtnConfirm">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../../../Images/forward.png" UrlHottracked="../../Images/forwardHoover.png" />
                                <ClientSideEvents Click="btnConfirm_Click" />
                            </dx:ASPxButton>
                        </span>
                    </div>

                </div>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

</asp:Content>
