<%@ Page Title="Sistemska pošta" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SystemEmailTable.aspx.cs" Inherits="OptimizacijaTransprotov.Pages.Settings.SystemEmailTable" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">

        function RowDoubleClick(s, e) {
            gridSystemEmailMessage.GetRowValues(gridSystemEmailMessage.GetFocusedRowIndex(), 'SystemEmailMessageID;EmailBody', OnGetRowValuesAuditorKVPs);
        }

        function OnGetRowValuesAuditorKVPs(value) {
            clientSytemEmailMessageCallbackPanel.PerformCallback('DblClickShowEmailBody|' + value[0] + "|" + value[1]);
        }


        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'EmailList':
                            clientPopUpAuditors.Hide();
                            gridSystemEmailMessage.Refresh();
                            break;
                        case 'EditEmailAndSend':
                            ASPxPopupControlSystemEmail.Hide();
                            gridSystemEmailMessage.Refresh();
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'Employee':
                            clientPopUpAuditors.Hide();
                            break;
                        case 'EditEmailAndSend':
                            ASPxPopupControlSystemEmail.Hide();
                            break;
                    }
                    break;
            }
        }

        function btnEditEmail_Click(s, e) {
            clientSytemEmailMessageCallbackPanel.PerformCallback('ShowAndEditEmail');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxCallbackPanel ID="SytemEmailMessageCallbackPanel" runat="server" OnCallback="SytemEmailMessageCallbackPanel_Callback" ClientInstanceName="clientSytemEmailMessageCallbackPanel">
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxGridView ID="ASPxGridViewEmails" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridSystemEmailMessage"
                    OnDataBinding="ASPxGridViewEmails_DataBinding" Width="100%"
                    KeyFieldName="SystemEmailMessageID" CssClass="gridview-no-header-padding">
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

                        <dx:GridViewDataTextColumn Caption="ID" FieldName="SystemEmailMessageID" Visible="false" />
                        <dx:GridViewDataTextColumn Caption="Datum" FieldName="ts" Width="10%" SortOrder="Descending" />
                        <dx:GridViewDataTextColumn Caption="Pošiljatelj" FieldName="EmailFrom" Width="12%" />

                        <dx:GridViewDataTextColumn Caption="Prejemnik" FieldName="EmailTo" Width="12%" />

                        <dx:GridViewDataTextColumn Caption="Zadeva" FieldName="EmailSubject" Width="20%" />

                        <dx:GridViewDataTextColumn Caption="Telo" FieldName="EmailBody" Width="40%" />

                    </Columns>
                </dx:ASPxGridView>

                <dx:ASPxPopupControl ID="ASPxPopupControlSystemEmailBody" runat="server" ContentUrl="SystemEmailBody_popup.aspx"
                    ClientInstanceName="clientPopUpAuditors" Modal="True" HeaderText="TELO E-POŠTE"
                    CloseAction="CloseButton" Width="750px" Height="510px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true"
                    OnWindowCallback="ASPxPopupControlSystemEmailBody_WindowCallback">
                    <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                    <ContentStyle BackColor="#F7F7F7">
                        <Paddings Padding="0px"></Paddings>
                    </ContentStyle>
                </dx:ASPxPopupControl>

                <dx:ASPxPopupControl ID="ASPxPopupControlSystemEmail" runat="server" ContentUrl="SystemEmail_popup.aspx"
                    ClientInstanceName="ASPxPopupControlSystemEmail" Modal="True" HeaderText="E-POŠTA"
                    CloseAction="CloseButton" Width="850px" Height="830px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true"
                    OnWindowCallback="ASPxPopupControlSystemEmail_WindowCallback">
                    <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                    <ContentStyle BackColor="#F7F7F7">
                        <Paddings Padding="0px"></Paddings>
                    </ContentStyle>
                </dx:ASPxPopupControl>


                <div class="AddEditButtonsWrap medium-margin-l medium-margin-r mt-3">
                    <div class="DeleteButtonElements">
                        <dx:ASPxButton ID="btnCopyEmailAndSend" runat="server" Text="Kopiraj sporočilo in pošlji" AutoPostBack="false" ClientVisible="false"
                            Height="25" Width="110" ClientInstanceName="btnCopyEmailAndSend" UseSubmitBehavior="false" OnClick="btnCopyEmailAndSend_Click">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../../Images/copy.png" UrlHottracked="../../Images/copyHover.png" />
                        </dx:ASPxButton>
                    </div>
                    <div class="AddEditButtonsElements">
                        <span class="AddEditButtons">
                            <dx:ASPxButton ID="btnEditEmail" runat="server" Text="Spremeni pošto" AutoPostBack="false" ClientInstanceName="btnEditEmail"
                                Height="25" Width="90" ClientVisible="false">
                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                <Image Url="../../../Images/edit.png" UrlHottracked="../../Images/editHover.png" />
                                <ClientSideEvents Click="btnEditEmail_Click" />
                            </dx:ASPxButton>
                        </span>
                    </div>
                </div>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

</asp:Content>
