using DatabaseWebService.ModelsOTP;
using DatabaseWebService.ModelsOTP.Recall;
using DevExpress.Web;
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

                ASPxGridViewRecallBuyer.DataBind();
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

        #region "Datagridview events"
        protected void ASPxGridViewRecallBuyer_DataBinding(object sender, EventArgs e)
        {
            List<RecallBuyerModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetAllBuyersRecalls());

            int iStatusID = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatuses())
              .Where(rs => rs.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.NAR_BREZ_FAKTURE.ToString())
              .FirstOrDefault().StatusOdpoklicaID;

            list = list.Where(r => r.StatusID == iStatusID).ToList();

            (sender as ASPxGridView).DataSource = list;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridViewRecallBuyer_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] split = e.Parameters.Split(';');
            if (split[0] == "DblClick" && !String.IsNullOrEmpty(split[1]))
            {
                ClearAllSessions(Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList());
                string redPage = "~/Pages/Recall/" + GenerateURI("RecallBuyerCreate.aspx", (int)Enums.UserAction.Edit, split[1]);
                ASPxWebControl.RedirectOnCallback(redPage);
            }
        }

        protected void ASPxGridViewRecallBuyer_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID != "Print") return;

            object valueID = ASPxGridViewRecallBuyer.GetRowValues(e.VisibleIndex, "OdpoklicKupecID");

            AddValueToSession(Enums.RecallSession.SelectedBuyerRecallID, valueID);


            ASPxGridViewRecallBuyer.JSProperties["cpPrintID"] = ConcatenateURLForPrint(valueID, "RecallBuyer", true);
        }

        protected void ASPxGridViewRecallBuyer_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            if (e.GetValue("StatusKoda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POTRJEN.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#dff0d8");
            else if (e.GetValue("StatusKoda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.V_ODOBRITEV.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#fcf8e3");
            else if (e.GetValue("StatusKoda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.ZAVRNJEN.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2dede");
            else if (e.GetValue("StatusKoda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.PREVZET.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffbf00");
            else if (e.GetValue("StatusKoda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ERR_ADMIN_MAIL.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFA879");
            else if (e.GetValue("StatusKoda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.USTVARJENO_NAROCILO.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#dff0d8");
        }

        protected void ASPxGridViewRecallBuyer_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {

            object order = ASPxGridViewRecallBuyer.GetRowValues(e.VisibleIndex, "OdpoklicKupecID");




            object item = ASPxGridViewRecallBuyer.GetRowValues(e.VisibleIndex, "StatusKoda");
            if (item != null)
            {
                bool isinquiryNotSubmited = item != null ? (item.ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ERR_ADMIN_MAIL.ToString() ? true : false) : false;
                bool orderExist = order != null ? (CommonMethods.ParseInt(order) > 0 ? true : false) : false;

                //if (item.ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELNO_PREVZET.ToString() || (item.ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.ERR_ADMIN_MAIL.ToString()) || (item.ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.USTVARJENO_NAROCILO.ToString()) || (item.ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.ERR_ORDER_NO_SEND.ToString()) && (orderExist))
                //    e.Visible = true;
                //else
                e.Visible = false;
            }
        }
        #endregion
    }
}