using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.ModelsOTP.Recall;
using DatabaseWebService.ModelsOTP.Route;
using DevExpress.Web;
using DevExpress.Web.Rendering;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Statistic
{
    public partial class RoutesDetail_popup : ServerMasterPage
    {
        List<RouteModel> model;
        int carrierID = 0;
        int routeID = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            carrierID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.RouteSession.CarrierIDPopup));
            routeID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.RouteSession.RouteIDPopup));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize(carrierID, routeID);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.RouteSession.RouteDetailList))
                    model = GetRouteDataProvider().GetRoutesByCarrierIDAndRouteID();
            }
        }


        #region Initialize
        private void Initialize(int carrierID, int routeID)
        {
            txtPrevoznik.Text = GetStringValueFromSession(Enums.RouteSession.CarrierNamePopup);
            txtRelacija.Text = GetStringValueFromSession(Enums.RouteSession.RouteNamePopup);

            if (model == null)
                PopulateModel(carrierID, routeID);

            ASPxGridViewRoutesDetail.DataBind();
        }

        private void PopulateModel(int carrierID, int routeID)
        {
            model = CheckModelValidation(GetDatabaseConnectionInstance().GetRoutesByCarrierIDAndRouteID(carrierID, routeID));

            GetRouteDataProvider().SetRoutesByCarrierIDAndRouteID(model);
        }
        #endregion

        #region Helper methods
        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.RouteSession.CarrierIDPopup);
            RemoveSession(Enums.RouteSession.RouteIDPopup);
            RemoveSession(Enums.RouteSession.RouteNamePopup);
            RemoveSession(Enums.RouteSession.CarrierNamePopup);

            /*if (recallAction == (int)Enums.UserAction.Add)
            {*/
            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "RoutesDetail"), true);
                RemoveSession(Enums.RecallSession.RecallFulModel);
            /*}
            else
                ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "OrderPos"), true);*/

        }
        #endregion

        protected void ASPxGridViewRoutesDetails_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = model;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RemoveSessionsAndClosePopUP(true);
        }
    }
}