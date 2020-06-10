using DatabaseWebService.ModelsOTP.Client;
using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.ModelsOTP.Recall;
using DatabaseWebService.ModelsOTP.Route;
using DevExpress.Web;
using DevExpress.Web.Rendering;
using OptimizacijaTransprotov.Common;
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
    public partial class CarrierAndRoutesPricing : ServerMasterPage
    {
        List<RouteModel> model = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            ASPxGridLookupPrevoznik.GridView.Settings.GridLines = GridLines.Both;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Initialize();
                
                /*if (model != null)
                {
                    GetOrderDataProvider().SetOrderPositions(model);
                }*/
            }
            else
            {
                if (model == null && SessionHasValue(Enums.RouteSession.RouteList))
                    model = GetRouteDataProvider().GetRoutesByCarrierID();
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            
        }


        protected void ASPxGridViewRoutes_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = model;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        #region Initialize
        private void Initialize(int carrierID, int routeID)
        {
            PopulateModel(carrierID, routeID);
            ASPxGridViewRoutes.DataBind();
        }
        #endregion

        private void PopulateModel(int carrierID, int routeID)
        {
            model = CheckModelValidation(GetDatabaseConnectionInstance().GetRoutesByCarrierID(carrierID));
            GetRouteDataProvider().SetRoutesByCarrierID(model);
        }

        

        protected void ASPxGridViewRoutes_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
           /* object item = ASPxGridViewOrdersPositions.GetRowValues(e.VisibleIndex, "tempID", "Order_Confirm");
            if (item != null)
            {
                object[] values = (object[])item;
                if (values[1] == null)
                    e.Visible = false;
                else if (String.IsNullOrEmpty(values[1].ToString()))
                    e.Visible = false;
            }*/
        }

        protected void ASPxGridLookupPrevoznik_DataBinding(object sender, EventArgs e)
        {
            string prevoznik = DatabaseWebService.Common.Enums.Enums.TypeOfClient.PREVOZNIK.ToString();
            (sender as ASPxGridLookup).DataSource = CheckModelValidation(GetDatabaseConnectionInstance().GetAllClients(prevoznik));
            (sender as ASPxGridLookup).GridView.Settings.GridLines = GridLines.Both;
        }

        protected void CallbackPanelRoutes_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] split = e.Parameter.Split(';');
            if (split[0] == "CarrierrChanged")
            {
                int carrierID = CommonMethods.ParseInt(ASPxGridLookupPrevoznik.Value);
                object valueID = ASPxGridViewRoutes.GetRowValues(ASPxGridViewRoutes.FocusedRowIndex, "RelacijaID");
                int routeID = CommonMethods.ParseInt(valueID);

                Initialize(carrierID, routeID);
            }
            else if (split[0] == "OpenPopupByRoute")
            {
                object valueName= ASPxGridViewRoutes.GetRowValues(ASPxGridViewRoutes.FocusedRowIndex, "Naziv");

                AddValueToSession(Enums.RouteSession.CarrierNamePopup, ASPxGridLookupPrevoznik.Text);
                AddValueToSession(Enums.RouteSession.RouteNamePopup, valueName);

                AddValueToSession(Enums.RouteSession.CarrierIDPopup, ASPxGridLookupPrevoznik.Value);
                AddValueToSession(Enums.RouteSession.RouteIDPopup, split[1]);

                ASPxPopupControlRoutesDetail.ShowOnPageLoad = true;
                ASPxGridLookupPrevoznik.GridView.Settings.GridLines = GridLines.Both;
            }
        }

        protected void ASPxPopupControlRoutesDetail_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.RouteSession.RouteIDPopup);
            RemoveSession(Enums.RouteSession.CarrierIDPopup);
            RemoveSession(Enums.RouteSession.RouteNamePopup);
            RemoveSession(Enums.RouteSession.CarrierNamePopup);

            ASPxGridLookupPrevoznik.GridView.Settings.GridLines = GridLines.Both;
        }
    }
}