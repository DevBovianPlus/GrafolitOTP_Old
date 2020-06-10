using DatabaseWebService.ModelsOTP.Recall;
using DevExpress.Web;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Recall
{
    public partial class RecallTakeOverOnly : ServerMasterPage
    {
        int recallIDFocusedRowIndex = 0;
        
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
                recallIDFocusedRowIndex = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (recallIDFocusedRowIndex > 0)
                {
                    ASPxGridViewRecall.FocusedRowIndex = ASPxGridViewRecall.FindVisibleIndexByKeyValue(recallIDFocusedRowIndex);
                    ASPxGridViewRecall.ScrollToVisibleIndexOnClient = ASPxGridViewRecall.FindVisibleIndexByKeyValue(recallIDFocusedRowIndex);
                }

                ASPxGridViewRecall.DataBind();
                InitializeEditDeleteButtons();
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            object valueID = ASPxGridViewRecall.GetRowValues(ASPxGridViewRecall.FocusedRowIndex, "OdpoklicID");

            ClearAllSessions(Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList());
            RedirectWithCustomURI("RecallForm.aspx", (int)Enums.UserAction.Edit, valueID);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearAllSessions(Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList());
            Response.Redirect("Order.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            object valueID = ASPxGridViewRecall.GetRowValues(ASPxGridViewRecall.FocusedRowIndex, "OdpoklicID");
            ClearAllSessions(Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList());

            RedirectWithCustomURI("RecallForm.aspx", (int)Enums.UserAction.Delete, valueID);
        }

        private void InitializeEditDeleteButtons()
        {
            //Check to enable Edit and Delete button for Tab PLAN
            /*if (ASPxGridViewRecall.VisibleRowCount <= 0)
            {
                EnabledDeleteAndEditBtnPopUp(btnEdit, btnDelete);
            }
            else if (!btnEdit.Enabled && !btnDelete.Enabled)
            {
                EnabledDeleteAndEditBtnPopUp(btnEdit, btnDelete, false);
            }*/
        }

        protected void ASPxGridViewRecall_DataBinding(object sender, EventArgs e)
        {
            List<RecallModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetAllTakeOverRecalls());

            (sender as ASPxGridView).DataSource = list;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridViewRecall_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] split = e.Parameters.Split(';');
            if (split[0] == "DblClick" && !String.IsNullOrEmpty(split[1]))
            {
                ClearAllSessions(Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("RecallForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
        }

        protected void ASPxGridViewRecall_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID != "Print") return;

            object valueID = ASPxGridViewRecall.GetRowValues(e.VisibleIndex, "OdpoklicID");

            ASPxGridViewRecall.JSProperties["cpPrintID"] = ConcatenateURLForPrint(valueID, "Recall", true);
        }

        protected void ASPxGridViewRecall_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
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
        }
    }
}