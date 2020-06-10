using DatabaseWebService.ModelsOTP.Client;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.BasicCodeList.TransportType
{
    public partial class TransportType_popup : ServerMasterPage
    {
        ClientTransportType model = null;
        int transportTypeID = -1;
        int action = -1;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();


            action = CommonMethods.ParseInt(GetStringValueFromSession(Enums.CommonSession.UserActionPopUp));

            transportTypeID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.ClientSession.ClientTransportTypeID));
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                if (action == (int)Enums.UserAction.Edit || action == (int)Enums.UserAction.Delete)
                {
                    if (transportTypeID > 0)
                    {
                        if (GetRouteDataProvider().GetRouteModel() != null)
                            model = (ClientTransportType)GetValueFromSession(Enums.ClientSession.ClientTransportTypeModel);
                        else
                        {
                            model = CheckModelValidation(GetDatabaseConnectionInstance().GetTransportTypeByID(transportTypeID));
                        }

                        if (model != null)
                        {
                            AddValueToSession(Enums.ClientSession.ClientTransportTypeModel, model);
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

        protected void btnCancelPopUp_Click(object sender, EventArgs e)
        {
            RemoveSessionsAndClosePopUP();
        }

        protected void btnConfirmPopUp_Click(object sender, EventArgs e)
        {
            ProcessUserAction();
        }

        private void FillForm()
        {
            txtKoda.Text = model.Koda;
            txtNaziv.Text = model.Naziv;
            txtDovoljenaTeza.Text = model.DovoljenaTeza.ToString("N2");
            CheckBoxShraniPozicije.Checked = model.ShranjevanjePozicij;
            ASPxMemoOpomba.Text = model.Opombe;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new ClientTransportType();

                model.TipPrevozaID = 0;
                model.tsIDPrijave = PrincipalHelper.GetUserPrincipal().ID;
            }
            else if (model == null && !add)
            {
                model = (ClientTransportType)GetValueFromSession(Enums.ClientSession.ClientTransportTypeModel);
            }

            model.Koda = txtKoda.Text;
            model.Naziv = txtNaziv.Text;
            model.DovoljenaTeza = CommonMethods.ParseDecimal(txtDovoljenaTeza.Text);
            model.ShranjevanjePozicij = CheckBoxShraniPozicije.Checked;
            model.Opombe = ASPxMemoOpomba.Text;

            ClientTransportType newModel = CheckModelValidation(GetDatabaseConnectionInstance().SaveTransportType(model));

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
            return CheckModelValidation(GetDatabaseConnectionInstance().DeleteTransportType(transportTypeID));
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
            txtNaziv.Focus();
        }
        #endregion

        #region Helper methods
        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";


            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.ClientSession.ClientTransportTypeModel);
            RemoveSession(Enums.ClientSession.ClientTransportTypeID);

            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}', '{2}');", confirmCancelAction, "TransportType", transportTypeID), true);

        }
        #endregion
    }
}