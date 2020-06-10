<%@ Page Title="" Language="C#" MasterPageFile="~/Greetings.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="OptimizacijaTransprotov.Home" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">
        function OnClosePopupEventHandler_Prijava(param, url) {
            switch (param) {
                case 'Potrdi':
                    Prijava_Popup.Hide();
                    window.location.assign(url);//"../Default.aspx"
                    break;
                case 'Prekliči':
                    Prijava_Popup.Hide();
                    break;
            }
        }

        $(document).ready(function () {
             $('#ctl00_MainContentPlaceHolder_FormLayoutWrap').keypress(function (event) {
                var key = event.which;
                if (key == 13) {
                    CauseValidation(this, event);
                    clientUsername.GetInputElement().blur();
                    clientPass.GetInputElement().blur();
                    return false;
                }
            });
        });

        function CauseValidation(s, e) {
            var procees = false;
            var inputItems = [clientUsername, clientPass];

            procees = InputFieldsValidation(null, inputItems, null, null);

            if (procees) {
                clientLoadingPanel.Show();
                clientLoginCallback.PerformCallback("Test");
            }
        }

        function EndLoginCallback(s, e) {
            clientLoadingPanel.Hide();
            
            if (s.cpResult != null && s.cpResult !== undefined) {

                ShowErrorPopUp(s.cpResult);
                clientErrorLabel.SetText(s.cpResult);
                clientUsername.SetText("");
                clientPass.SetText("");
                delete (s.cpResult);
            }
            else
                window.location.assign('Home.aspx');//"../Default.aspx"
        }
        function ClearText(s, e) {
            clientErrorLabel.SetText("");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div id="FormLayoutWrap" runat="server" style="display: flex; width: 50%; margin: 0 auto; overflow: hidden; padding: 10px; border: 1px solid #e1e1e1; border-radius: 3px; box-shadow: 5px 10px 18px #e1e1e1; background-color: whitesmoke; margin-top:30px;">
        <dx:ASPxFormLayout ID="ASPxFormLayoutLogin" runat="server">
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="500" />
            <Items>
                <dx:LayoutGroup Name="LOGIN" GroupBoxDecoration="HeadingLine" Caption="Login" UseDefaultPaddings="false" GroupBoxStyle-Caption-BackColor="WhiteSmoke">
                    <Items>
                        <dx:LayoutItem Caption="Error label caption" Name="ErrorLabelCaption" ShowCaption="False"
                            CaptionSettings-VerticalAlign="Middle">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxLabel ID="ErrorLabel" runat="server" Text="" ForeColor="Red"
                                        ClientInstanceName="clientErrorLabel">
                                    </dx:ASPxLabel>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Uporabniško ime" Name="Username" CaptionSettings-VerticalAlign="Middle" Paddings-PaddingBottom="20px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="txtUsername" runat="server" Theme="Moderno"
                                        CssClass="text-box-input" ClientInstanceName="clientUsername"
                                        AutoCompleteType="Disabled">
                                        <FocusedStyle CssClass="focus-text-box-input" />
                                        <ClientSideEvents Init="SetFocus" GotFocus="ClearText" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Geslo" Name="Password" CaptionSettings-VerticalAlign="Middle" Paddings-PaddingBottom="10px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="txtPassword" runat="server" Theme="Moderno"
                                        CssClass="text-box-input" Password="true" ClientInstanceName="clientPass">
                                        <ClientSideEvents GotFocus="ClearText" />
                                        <FocusedStyle CssClass="focus-text-box-input" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Zapomni si geslo" Name="RememberMe" Paddings-PaddingTop="10px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="rememberMeCheckBox" runat="server" ToggleSwitchDisplayMode="Always" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Name="SignUp" HorizontalAlign="Right" ShowCaption="False" Paddings-PaddingTop="20px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxButton ID="ASPxButton1" runat="server" Text="PRIJAVA" Width="100" Theme="Moderno"
                                        AutoPostBack="false" UseSubmitBehavior="false">
                                        <ClientSideEvents Click="CauseValidation" />
                                    </dx:ASPxButton>
                                    <dx:ASPxCallback ID="LoginCallback" runat="server" OnCallback="LoginCallback_Callback"
                                        ClientInstanceName="clientLoginCallback">
                                        <ClientSideEvents EndCallback="EndLoginCallback" />
                                    </dx:ASPxCallback>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
            </Items>
        </dx:ASPxFormLayout>
        <dx:ASPxLoadingPanel ID="LoadingPanel" ClientInstanceName="clientLoadingPanel" runat="server" Modal="true">
        </dx:ASPxLoadingPanel>
    </div>
</asp:Content>
