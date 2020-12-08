using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.ModelsOTP.Recall;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using DevExpress.Web;
using System;
using OptimizacijaTransprotov.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Settings
{
    public partial class SystemEmailBody_popup : ServerMasterPage
    {
        int systemEmailMessageID = -1;
        string emailBody = "";
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            systemEmailMessageID = CommonMethods.ParseInt(GetStringValueFromSession("SystemEmailMessageID"));
            emailBody = GetStringValueFromSession("SystemEmailMessageBody");            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxHtmlEditorEmailBody.Html = emailBody;
        }

        protected void btnConfirmPopUp_Click(object sender, EventArgs e)
        {
            RemoveSessionsAndClosePopUP(true);
        }

        protected void btnCancelPopUp_Click(object sender, EventArgs e)
        {
            RemoveSessionsAndClosePopUP(false);
        }


        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";



            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "EmailList"), true);

        }
    }
}