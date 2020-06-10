<%@ Page Title="Odpoklici" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RecallTakeOverOnly.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Recall.RecallTakeOverOnly" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            var filterType = GetUrlQueryStrings()['filter'];

            //we delete successMessage query string so we show modal only once!
            var params = QueryStringsToObject();
            delete params.filter;
            var path = window.location.pathname + '?' + SerializeQueryStrings(params);
            history.pushState({}, document.title, path);
        });

        function DoubleClick(s, e) {            
            gridRecall.GetRowValues(gridRecall.GetFocusedRowIndex(), 'OdpoklicID', OnGetRowValues);
        }

        function OnGetRowValues(value) {
            gridRecall.PerformCallback('DblClick;' + value);
        }

        function gridRecall_EndCallback(s, e) {
            if (s.cpPrintID != "" && s.cpPrintID != undefined)
            {
                window.open(s.cpPrintID, '_blank');
                delete (s.cpPrintID);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxGridView ID="ASPxGridViewRecall" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridRecall"
        Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G" OnDataBinding="ASPxGridViewRecall_DataBinding"
        KeyFieldName="OdpoklicID" CssClass="gridview-no-header-padding" OnCustomCallback="ASPxGridViewRecall_CustomCallback"
        OnCustomButtonCallback="ASPxGridViewRecall_CustomButtonCallback" OnHtmlRowPrepared="ASPxGridViewRecall_HtmlRowPrepared">
        <ClientSideEvents RowDblClick="DoubleClick" EndCallback="gridRecall_EndCallback" />
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
        <SettingsText EmptyDataRow="Trenutno ni podatka o odpoklicih. Dodaj novega." />
        <Columns>
            <dx:GridViewCommandColumn ButtonRenderMode="Button" Width="6%" Caption="Dokument">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="Print">
                        
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="ID" FieldName="OdpoklicID" Width="80px"
                ReadOnly="true" Visible="false" ShowInCustomizationForm="True" SortOrder="Descending">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Odpoklic številka" FieldName="OdpoklicStevilka" Width="5%"
                ReadOnly="true" ShowInCustomizationForm="True">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Dobavitelj" FieldName="DobaviteljNaziv" Width="16%"
                ReadOnly="true" ShowInCustomizationForm="True">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Relacija" FieldName="RelacijaNaziv" Width="25%"
                ReadOnly="true" ShowInCustomizationForm="True">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Status Koda" FieldName="StatusKoda" Width="10%"
                Visible="false" ShowInCustomizationForm="True">
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

            <dx:GridViewDataTextColumn Caption="Cena prevoza"
                FieldName="CenaPrevoza" ShowInCustomizationForm="True"
                Width="8%">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Skupna količina"
                FieldName="KolicinaSkupno" ShowInCustomizationForm="True"
                Width="8%">
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Dobavitelj kraj" FieldName="DobaviteljKraj" 
                ReadOnly="true" ShowInCustomizationForm="True" Visible="false">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Dobavitelj pošta" FieldName="DobaviteljPosta"
                ReadOnly="true" ShowInCustomizationForm="True" Visible="false">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Dobavitelj naslov" FieldName="DobaviteljNaslov"
                ReadOnly="true" ShowInCustomizationForm="True" Visible="false">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn Caption="Datum odpoklica" FieldName="ts" CellStyle-HorizontalAlign="Right" Width="7%">
                <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy"></PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
        </Columns>
    </dx:ASPxGridView>
    <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
        <div class="DeleteButtonElements">
            <span class="AddEditButtons">
                <dx:ASPxButton Theme="Moderno" ID="btnDelete" runat="server" Text="Izbriši" AutoPostBack="false" OnClick="btnDelete_Click"
                    Height="25" Width="50" ClientVisible="false">
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

    <!-- Warning - Modal -->
    <div id="warningModal" class="modal fade" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: tomato; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="fa fa-exclamation-circle" style="font-size: 60px; color: white"></i></div>
                </div>
                <h4 class="modal-title text-center" id="myModalTitle" style="margin-top: 10px; font-weight: 600">Opozorilo!</h4>
                <div class="modal-body text-center" id="modalBodyText">
                </div>
                <%--<div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>--%>
            </div>

        </div>
    </div>
</asp:Content>
