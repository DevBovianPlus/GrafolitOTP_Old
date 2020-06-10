using DevExpress.Web;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.BasicCodeList.Route
{
    public partial class Route : ServerMasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ASPxGridViewRoute.DataBind();
                if (ASPxGridViewRoute.VisibleRowCount <= 0)
                {
                    btnEdit.ClientEnabled = false;
                    btnDelete.ClientEnabled = false;
                }
            }
        }

        protected void ASPxGridViewRoute_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutes());
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void CallbackPanelRoute_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshGrid")
            {
                ASPxGridViewRoute.DataBind();
            }
            else
            {
                object valueID = null;
                if (ASPxGridViewRoute.VisibleRowCount > 0)
                    valueID = ASPxGridViewRoute.GetRowValues(ASPxGridViewRoute.FocusedRowIndex, "RelacijaID");

                bool isValid = SetSessionsAndOpenPopUp(e.Parameter, Enums.RouteSession.RouteID, valueID);
                if (isValid)
                    ASPxPopupControlRoute.ShowOnPageLoad = true;
            }
        }

        protected void ASPxPopupControlRoute_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            if (e.Parameter == "ClosePopupButtonClick")//if the user click on close button on popup we have to clear sessions
            {
                RemoveSession(Enums.CommonSession.UserActionPopUp);
                RemoveSession(Enums.RouteSession.RouteModel);
                RemoveSession(Enums.RouteSession.RouteID);
            }
        }
    }
}