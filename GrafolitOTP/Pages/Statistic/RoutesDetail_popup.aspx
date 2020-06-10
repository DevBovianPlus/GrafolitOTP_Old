<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="RoutesDetail_popup.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Statistic.RoutesDetail_popup" %>

<%@ MasterType VirtualPath="~/Popup.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolderPopup" runat="server">
    <script type="text/javascript">

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolderPopup" runat="server">
    
    <div class="row small-padding-bottom">
        <div class="col-xs-12">
            <div class="row2 align-item-centerV-startH">
                <div class="col-xs-0 big-margin-r no-padding-right">
                    <dx:ASPxLabel ID="lblRazlogOdobritveSistem" runat="server" Text="PREVOZNIK : "></dx:ASPxLabel>
                </div>
                <div class="col-xs-10 no-padding-left">
                    <dx:ASPxTextBox runat="server" ID="txtPrevoznik"
                        CssClass="text-box-input" Font-Size="14px" Width="100%" BackColor="LightGray" ReadOnly="true" Font-Bold="true">
                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                    </dx:ASPxTextBox>
                </div>
            </div>
        </div>
    </div>

    <div class="row small-padding-bottom">
        <div class="col-xs-12">
            <div class="row2 align-item-centerV-startH">
                <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 30px;">
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="RELACIJA : "></dx:ASPxLabel>
                </div>
                <div class="col-xs-10 no-padding-left">
                    <dx:ASPxTextBox runat="server" ID="txtRelacija"
                        CssClass="text-box-input" Font-Size="14px" Width="100%" BackColor="LightGray" ReadOnly="true" Font-Bold="true">
                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                    </dx:ASPxTextBox>
                </div>
            </div>
        </div>
    </div>

    <div class="row2 small-padding-bottom align-item-centerV-centerH">
        <div class="col-xs-12 no-padding-left no-padding-right text-center">
            <dx:ASPxGridView ID="ASPxGridViewRoutesDetail" runat="server" EnableCallbackCompression="true"
                Theme="Moderno" Width="80%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewRoutesDetails_DataBinding"
                KeyFieldName="TempID" CssClass="gridview-no-header-padding center-margin">

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

                    <dx:GridViewDataTextColumn Caption="Cena" FieldName="Cena" Width="40%"
                        ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataDateColumn FieldName="Datum" Caption="Datum razpisa" ShowInCustomizationForm="True" Width="60%">
                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                        </PropertiesDateEdit>
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataDateColumn>
                </Columns>
            </dx:ASPxGridView>
        </div>
    </div>
    <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
        <div class="AddEditButtonsElements">
            <span class="AddEditButtons">
                <dx:ASPxButton Theme="Moderno" ID="btnCancel" runat="server" Text="Prekliči" AutoPostBack="false" OnClick="btnCancel_Click"
                    Height="25" Width="90" ClientEnabled="false" ClientInstanceName="clientBtnConfirm" ClientVisible="false">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                </dx:ASPxButton>
            </span>
        </div>
    </div>
</asp:Content>
