using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.ModelsOTP.Recall;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DatabaseWebService.ModelsOTP;
using DevExpress.Web;
using DatabaseWebService.ModelsOTP.Tender;

namespace OptimizacijaTransprotov.Pages.Settings
{
    public partial class TenderPositionManualChanges : ServerMasterPage
    {
        int employeeIDFocusedRowIndex = 0;
        int filterType = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!PrincipalHelper.IsUserAdmin() && !PrincipalHelper.IsUserSuperAdmin()) RedirectHome();

            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
                employeeIDFocusedRowIndex = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());

            if (Request.QueryString[Enums.QueryStringName.filter.ToString()] != null)
                filterType = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.filter.ToString()].ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (employeeIDFocusedRowIndex > 0)
                {
                    ASPxGridViewTPManualChanges.FocusedRowIndex = ASPxGridViewTPManualChanges.FindVisibleIndexByKeyValue(employeeIDFocusedRowIndex);
                    ASPxGridViewTPManualChanges.ScrollToVisibleIndexOnClient = ASPxGridViewTPManualChanges.FindVisibleIndexByKeyValue(employeeIDFocusedRowIndex);
                }

                ASPxGridViewTPManualChanges.DataBind();
                InitializeEditDeleteButtons();
            }
        }

        protected void ASPxGridViewTPManualChanges_DataBinding(object sender, EventArgs e)
        {
            List<TenderPositionChangeModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetTenderListPositionChanges());

            (sender as ASPxGridView).DataSource = list;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        private void InitializeEditDeleteButtons()
        {

        }


        protected void PopupControlEmployee_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
        }



        protected void btnExportManualChanges_Click(object sender, EventArgs e)
        {
            CommonMethods.ExportToPDFFitToPage(ASPxGridViewExporterManualChanges, this);
        }


        protected void TPManualChangesCallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
        }

        protected void ASPxPopupControlSystemEmail_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.SystemEmailMessageSession.SystemMessageModel);
            RemoveSession(Enums.SystemEmailMessageSession.SystemMessageID);
        }
    }
}