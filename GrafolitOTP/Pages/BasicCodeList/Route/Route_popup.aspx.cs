using DatabaseWebService.ModelsOTP.Route;
using DatabaseWebService.ModelsOTP.Tender;
using DevExpress.Web;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.BasicCodeList.Route
{
    public partial class Route_popup : ServerMasterPage
    {
        RouteModel model = null;
        int routeID = -1;
        int action = -1;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if(SessionHasValue(Enums.CommonSession.UserActionNestedPopUp))
                action = CommonMethods.ParseInt(GetStringValueFromSession(Enums.CommonSession.UserActionNestedPopUp));
            else
                action = CommonMethods.ParseInt(GetStringValueFromSession(Enums.CommonSession.UserActionPopUp));

            routeID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.RouteSession.RouteID));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                if (action == (int)Enums.UserAction.Edit || action == (int)Enums.UserAction.Delete)
                {
                    if (routeID > 0)
                    {
                        if (GetRouteDataProvider().GetRouteModel() != null)
                            model = GetRouteDataProvider().GetRouteModel();
                        else
                        {
                            model = CheckModelValidation(GetDatabaseConnectionInstance().GetRouteByID(routeID));
                        }

                        if (model != null)
                        {
                            GetRouteDataProvider().SetRouteModel(model);
                            FillForm();
                        }
                    }
                }
                else if (action == (int)Enums.UserAction.Add)
                {
                    SetFromDefaultValues();
                }
                UserActionConfirmBtnUpdate(btnConfirmPopUp, action, true);
            }
        }

        private void FillForm()
        {
            DateEditDatum.Date = model.Datum;
            txtKoda.Text = model.Koda;
            txtNaziv.Text = model.Naziv;
            txtDolzina.Text = model.Dolzina.ToString("N2");
            ASPxMemoOpomba.Text = model.Opomba;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {

            if (add)
            {
                model = new RouteModel();

                model.RelacijaID = 0;
                model.tsIDOsebe = PrincipalHelper.GetUserPrincipal().ID;
            }
            else if (model == null && !add)
            {
                model = GetRouteDataProvider().GetRouteModel();
            }

            model.Datum = DateEditDatum.Date;
            model.Koda = txtKoda.Text;
            model.Naziv = txtNaziv.Text;
            model.Dolzina = CommonMethods.ParseDecimal(txtDolzina.Text);
            model.Opomba = ASPxMemoOpomba.Text;

            RouteModel newModel = CheckModelValidation(GetDatabaseConnectionInstance().SaveRoute(model));

            if (newModel != null)//If new record is added we need to refresh aspxgridview. We add new record to session model.
            {
                model = newModel;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool DeleteObject()
        {
            return CheckModelValidation(GetDatabaseConnectionInstance().DeleteRoute(routeID));
        }

        protected void btnConfirmPopUp_Click(object sender, EventArgs e)
        {
            ProcessUserAction();
        }

        protected void btnCancelPopUp_Click(object sender, EventArgs e)
        {
            RemoveSessionsAndClosePopUP();
        }

        private void ProcessUserAction()
        {
            bool isValid = false;
            bool confirm = false;

            switch (action)
            {
                case (int)Enums.UserAction.Add:
                    isValid = AddOrEditEntityObject(true);
                    confirm = true;
                    break;
                case (int)Enums.UserAction.Edit:
                    isValid = AddOrEditEntityObject();
                    confirm = true;
                    break;
                case (int)Enums.UserAction.Delete:
                    isValid = DeleteObject();
                    confirm = true;
                    break;
            }

            if (isValid)
            {
                RemoveSessionsAndClosePopUP(confirm);
            }
            else
                ShowClientPopUp("Something went wrong. Contact administrator", 1);
        }

        #region Initialization
        private void Initialize()
        {
            
        }

        private void SetFromDefaultValues()
        {
            DateEditDatum.Date = DateTime.Now;
        }
        #endregion

        #region Helper methods
        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            if (model != null && model.RelacijaID > 0)
                routeID = model.RelacijaID;

            if (SessionHasValue(Enums.CommonSession.UserActionNestedPopUp))
                RemoveSession(Enums.CommonSession.UserActionNestedPopUp);
            else
                RemoveSession(Enums.CommonSession.UserActionPopUp);
            
            RemoveSession(Enums.RouteSession.RouteID);
            RemoveSession(Enums.RouteSession.RouteModel);

            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}', '{2}');", confirmCancelAction, "Route", routeID), true);

        }
        #endregion
    }
}