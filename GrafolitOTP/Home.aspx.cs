using DatabaseWebService.ModelsOTP;
using Newtonsoft.Json;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov
{
    public partial class Home : ServerMasterPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (Request.IsAuthenticated)
                MasterPageFile = "~/Main.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                ASPxFormLayoutLogin.Visible = false;
                FormLayoutWrap.Style.Add("display", "none");
                MainDashboard.Style.Add("display", "block");

                DashboardDataModel data = CheckModelValidation(GetDatabaseConnectionInstance().GetDashboardData());
                if (data != null)
                {
                    lblAllRecalls.Text = data.AllRecalls.ToString();
                    lblConfirmedRecalls.Text = data.ApprovedRecalls.ToString();
                    lblRejectedRecalls.Text = data.RejectedRecalls.ToString();
                    lblNeedToConfirmRecall.Text = data.NeedsApproval.ToString();
                }
            }
        }

        protected void LoginCallback_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            Authentication auth = new Authentication();
            bool signInSuccess = false;
            string message = "";
            string username = CommonMethods.Trim(txtUsername.Text);
            string password = CommonMethods.Trim(txtPassword.Text);

            try
            {
                if (username != "" && password != "")
                {
                    signInSuccess = auth.Authenticate(username, password);
                }

            }
            catch (Exception ex)
            {
                message = ex.ToString();
            }


            //string url = Session["PreviousPage"].ToString();
            if (signInSuccess)
            {
                //ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopupEventHandler_Prijava('{0}','{1}');", "Potrdi", url), true);
                Session.Remove("PreviousPage");
            }
            else
            {
                //ErrorLabel.ClientVisible = true;
                /*
                ErrorTableRow.Style.Add("display", "block");
                signInBtnWrap.Style.Add("margin-top", "0");*/
                LoginCallback.JSProperties["cpResult"] = "Napačna prijava! Ponovno vnesi geslo in uporabniško ime!  Error:" + message;
            }
        }

        protected void ChartsCallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshCharts" && Request.IsAuthenticated)
            {
                DashboardDataModel data = CheckModelValidation(GetDatabaseConnectionInstance().GetDashboardData());

                ChartsCallbackPanel.JSProperties["cpChartData"] = JsonConvert.SerializeObject(data.CurrentYearRecall);
                ChartsCallbackPanel.JSProperties["cpChartDataEmployees"] = JsonConvert.SerializeObject(data.EmployeesRecallCount);
                ChartsCallbackPanel.JSProperties["cpChartDataTransporters"] = JsonConvert.SerializeObject(data.TransporterRecallCount);
                ChartsCallbackPanel.JSProperties["cpChartDataRoutes"] = JsonConvert.SerializeObject(data.RouteRecallCount);
                ChartsCallbackPanel.JSProperties["cpChartDataSupplier"] = JsonConvert.SerializeObject(data.SupplierRecallCount);
            }
        }
    }
}