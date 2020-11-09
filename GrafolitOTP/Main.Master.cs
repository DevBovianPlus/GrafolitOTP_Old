using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov
{
    public partial class Main : System.Web.UI.MasterPage
    {
        private bool disableNavBar;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                Session["MainMenuSaleAnalysis"] = AppDomain.CurrentDomain.BaseDirectory + "Navigation\\MainMenu.xml";
                UsernameLabel.Text = PrincipalHelper.GetUserPrincipal().firstName + " " + PrincipalHelper.GetUserPrincipal().lastName;
                SignedInHelloLabel.Visible = true;
                UserRoleLabel.Visible = true;
                SignedInAsLabel.Visible = true;
                UserRoleLabel.Text = PrincipalHelper.GetUserPrincipal().Role;
                NavBarMainMenu.Visible = true;
                SetMainMenuBySignInRole();

                if (!String.IsNullOrEmpty(PrincipalHelper.GetUserPrincipal().ProfileImage))
                    headerProfileImage.Src = PrincipalHelper.GetUserPrincipal().ProfileImage.Replace(AppDomain.CurrentDomain.BaseDirectory, "/");
                else
                    headerProfileImage.Src = "/Images/defaultPerson.png";


                InfrastructureHelper.SetCookieValue(Enums.Cookies.UserLastRequest.ToString(), DateTime.Now.ToString("dd M yyyy HH mm ss"));
            }
            else
            {
                NavBarMainMenu.Visible = false;
                UsernameLabel.Text = "";
                UserRoleLabel.Visible = false;
                SignedInAsLabel.Visible = false;
                SignedInHelloLabel.Visible = false;


                Session["PreviousPage"] = Request.RawUrl;
                //ASPxPopupControl_PonovnaPrijava.ShowOnPageLoad = true;

                InfrastructureHelper.SetCookieValue(Enums.Cookies.UserLastRequest.ToString(), "STOP");
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
            }

            var version = Assembly.GetExecutingAssembly().GetName().Version;

            var buildDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);

            var displayableVersion = $"{version} ({buildDate})";

            Console.WriteLine("Current version (inc. build date) = " + displayableVersion);

            lblVerzija.Text = "Ver.: " + displayableVersion;

            NavBarMainMenu.DataBind();
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();

            InfrastructureHelper.SetCookieValue(Enums.Cookies.UserLastRequest.ToString(), "STOP");

            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Session["MainMenuSaleAnalysis"] = null;
            Response.Redirect("~/Home.aspx");
        }

        private void SetMainMenuBySignInRole()
        {
            if (PrincipalHelper.IsUserSuperAdmin())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.SuperAdmin.ToString());
            }
            else if (PrincipalHelper.IsUserAdmin())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.Admin.ToString());
            }
            else if (PrincipalHelper.IsUserLeader())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.Leader.ToString());
            }
            else if (PrincipalHelper.IsUserWarehouseKeeper())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.Leader.ToString());
            }
            else if (PrincipalHelper.IsUserLogistics())
            {
                SetXmlDataSourceSetttings(Enums.UserRole.Logistics.ToString());
            }
            else
            {
                SetXmlDataSourceSetttings(Enums.UserRole.Warehouse.ToString());
            }
        }

        private void SetXmlDataSourceSetttings(string userRole)
        {
            MainMenuDataSource.DataFile = Session["MainMenuSaleAnalysis"].ToString();
            MainMenuDataSource.XPath = "MainMenu/" + userRole + "/Group";
            
            if (!DisableNavBar)
                NavBarMainMenu.Enabled = true;
            else
                NavBarMainMenu.Enabled = false;

        }

        public bool DisableNavBar
        {
            get { return disableNavBar; }
            set { disableNavBar = value; }
        }

        public string PageHeadlineTitle
        {
            get { return PageHeadline.HeaderText; }
            set { PageHeadline.HeaderText = value; }
        }

        public string StyleDisplayHeader
        {
            get { return masterHeader.Style["display"]; }
            set { masterHeader.Style.Add("display", value); }
        }
    }
}