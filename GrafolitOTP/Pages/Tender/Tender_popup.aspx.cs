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

namespace OptimizacijaTransprotov.Pages.Tender
{
    public partial class Tender_popup : ServerMasterPage
    {
        TenderPositionModel model = null;
        int tenderID = -1;
        int action = -1;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            action = CommonMethods.ParseInt(GetStringValueFromSession(Enums.CommonSession.UserActionPopUp));
            tenderID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.TenderSession.TenderID));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                if (action == (int)Enums.UserAction.Edit || action == (int)Enums.UserAction.Delete)
                {
                    /*if (tenderID > 0)
                    {
                        if (GetTenderDataProvider().GetTenderFullModel() != null)
                            model = GetTenderDataProvider().GetTenderFullModel();
                        else
                        {
                            model = CheckModelValidation(GetDatabaseConnectionInstance().GetTenderByID(tenderID));
                        }

                        if (model != null)
                        {
                            GetTenderDataProvider().SetTenderFullModel(model);
                            FillForm();
                        }
                    }*/
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
            /*ASPxGridLookupStranke.Value = model.StrankaID;
            ASPxGridLookupRelacija.Value = model.RelacijaID;
            DateEditDatumRAzpisa.Date = model.DatumRazpisa;
            txtCena.Text = model.Cena.ToString("N3");*/
        }

        private bool AddOrEditEntityObject(bool add = false)
        {

            if (add)
            {
                model = new TenderPositionModel();

                model.RazpisPozicijaID = 0;
                model.IDOseba = PrincipalHelper.GetUserPrincipal().ID;
            }
            else if (model == null && !add)
            {
                //model = GetTenderDataProvider().GetTenderFullModel();
            }
            model.RazpisID = tenderID;
            model.StrankaID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupStranke));
            model.RelacijaID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupRelacija));
            
            model.Cena = CommonMethods.ParseDecimal(txtCena.Text);
            

            TenderPositionModel newModel = CheckModelValidation(GetDatabaseConnectionInstance().SaveTenderPosition(model));

            if (newModel != null)//If new record is added we need to refresh aspxgridview. We add new record to session model.
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool DeleteObject()
        {
            return CheckModelValidation(GetDatabaseConnectionInstance().DeleteTender(tenderID));
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
            ASPxGridLookupRelacija.DataBind();
            ASPxGridLookupStranke.DataBind();
        }

        private void SetFromDefaultValues()
        {
            DateEditDatumRAzpisa.Date = DateTime.Now;
        }
        #endregion

        #region DataBindings
        protected void ASPxGridLookupRelacija_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridLookup).DataSource = CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutes());
            (sender as ASPxGridLookup).GridView.Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridLookupStranke_DataBinding(object sender, EventArgs e)
        {
            string prevoznik = DatabaseWebService.Common.Enums.Enums.TypeOfClient.PREVOZNIK.ToString();
            (sender as ASPxGridLookup).DataSource = CheckModelValidation(GetDatabaseConnectionInstance().GetAllClients(prevoznik));
            (sender as ASPxGridLookup).GridView.Settings.GridLines = GridLines.Both;
        }
        #endregion

        #region Helper methods
        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.TenderSession.TenderID);
            RemoveSession(Enums.TenderSession.TenderFullModel);

            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "Tender"), true);

        }
        #endregion

        protected void CllbackPanelQuickAddRoute_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "AddNewRoute")
            {
                AddValueToSession(Enums.CommonSession.UserActionNestedPopUp, 1);
                ASPxPopupControlRoute.ShowOnPageLoad = true;
            }
            else
            {
                ASPxGridLookupRelacija.DataBind();
                RemoveSession(Enums.CommonSession.UserActionNestedPopUp);
                CllbackPanelQuickAddRoute.JSProperties["cpNewRouteID"] = e.Parameter;
            }
        }

        protected void ASPxPopupControlRoute_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionNestedPopUp);
            RemoveSession(Enums.RouteSession.RouteModel);
            RemoveSession(Enums.RouteSession.RouteID);
        }
    }
}