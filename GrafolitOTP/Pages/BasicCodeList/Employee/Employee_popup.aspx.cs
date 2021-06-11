using DatabaseWebService.Models;
using DatabaseWebService.Models.Employee;
using DevExpress.Web;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Helpers.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.BasicCodeList.Employee
{
    public partial class Employee_popup : ServerMasterPage
    {
        int employeeAction = -1;
        int employeeID = 0;
        EmployeeFullModel model;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!PrincipalHelper.IsUserAdmin() && !PrincipalHelper.IsUserSuperAdmin()) RedirectHome();

            if (!Request.IsAuthenticated) RedirectHome();

            employeeAction = CommonMethods.ParseInt(GetStringValueFromSession(Enums.CommonSession.UserActionPopUp));
            employeeID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.EmployeeSession.EmployeeID));

            

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                if (employeeAction == (int)Enums.UserAction.Edit || employeeAction == (int)Enums.UserAction.Delete)
                {
                    if (employeeID > 0)
                    {
                        if (GetEmployeeDataProvider().GetEmployeeFullModel() != null)
                            model = GetEmployeeDataProvider().GetEmployeeFullModel();
                        else
                        {
                            model = CheckModelValidation(GetDatabaseConnectionInstance().GetEmployeeByID(employeeID));
                        }

                        if (model != null)
                        {
                            GetEmployeeDataProvider().SetEmployeeFullModel(model);
                            FillForm();
                        }
                    }
                }
                else if (employeeAction == (int)Enums.UserAction.Add)
                {
                    SetFromDefaultValues();
                }
                UserActionConfirmBtnUpdate(btnConfirm, employeeAction, true);
            }
        }

        private void Initialize()
        {
            GridLookupRole.DataBind();
            GridLookupPantheonUsers.DataBind();
        }

        private void FillForm()
        {
            txtFirstname.Text = model.Ime;
            txtLastname.Text = model.Priimek;
            txtAddress.Text = model.Naslov;
            DateEditBirthDate.Date = model.DatumRojstva;
            txtEmail.Text = model.Email;
            txtPhone.Text = model.TelefonGSM;

            txtUsername.Text = model.UporabniskoIme;
            txtPassword.Text = model.Geslo;
            CheckBoxAllowSignIn.Checked = model.PDODostop;

            txtEmailPassword.Text = model.EmailGeslo;
            txtSmtpServer.Text = model.EmailStreznik;
            CheckBoxSSLEncrypting.Checked = model.EmailSifriranjeSSL;
            txtPort.Text = model.EmailVrata.ToString();

            

            string sUserID = model.OTPPantheonUsrChar != null ? model.OTPPantheonUsrChar : "";

            GridLookupPantheonUsers.Value = GetTempIDPantheonUserByUserID(sUserID);
            GridLookupRole.Value = model.idVloga > 0 ? model.idVloga : (int?)null;

            HtmlPodpis.Html = model.Podpis;             

        }

        private int GetTempIDPantheonUserByUserID(string sUserID)
        {
            List<PantheonUsers> list = CheckModelValidation(GetDatabaseConnectionInstance().GetPantheonUsers());
            PantheonUsers usr = list.Where(pu => pu.acUserId == sUserID).FirstOrDefault();

            return (usr != null && usr.TempID > 0) ? usr.TempID : 0;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new EmployeeFullModel();

                model.idOsebe = 0;

                model.tsIDOsebe = PrincipalHelper.GetUserPrincipal().ID;
            }
            else if (model == null && !add)
            {
                model = GetEmployeeDataProvider().GetEmployeeFullModel();
            }

            model.Ime = txtFirstname.Text;
            model.Priimek = txtLastname.Text;
            model.Naslov = txtAddress.Text;
            model.DatumRojstva = DateEditBirthDate.Date;
            model.Email = txtEmail.Text;
            model.TelefonGSM = txtPhone.Text;

            model.UporabniskoIme = txtUsername.Text;
            model.Geslo = txtPassword.Text;

            model.PDODostop = CheckBoxAllowSignIn.Checked;
            model.idVloga = CommonMethods.ParseInt(GetGridLookupValue(GridLookupRole));

            model.OTPPantheonUsrID = CommonMethods.ParseInt(GridLookupPantheonUsers.GridView.GetRowValues(GridLookupPantheonUsers.GridView.FocusedRowIndex, "anUserID"));
            model.OTPPantheonUsrChar = GridLookupPantheonUsers.Text.ToString();

            EmployeeFullModel newModel = CheckModelValidation(GetDatabaseConnectionInstance().SaveEmployeeOTP(model));

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

        private void SetFromDefaultValues()
        { }

        protected void GridLookupSupervisor_DataBinding(object sender, EventArgs e)
        {
            List<EmployeeFullModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetAllEmployees());
            (sender as ASPxGridLookup).DataSource = list;
        }

        #region Helper methods
        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.EmployeeSession.EmployeeID);
            RemoveSession(Enums.EmployeeSession.EmployeeModel);

            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "Employee"), true);
        }
        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RemoveSessionsAndClosePopUP();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            ProcessUserAction();
        }

        private bool DeleteObject()
        {
            var isDeleted = CheckModelValidation(GetDatabaseConnectionInstance().DeleteEmployee(employeeID));

            return isDeleted;
        }

        private void ProcessUserAction()
        {
            bool isValid = false;
            bool confirm = false;

            switch (employeeAction)
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


        protected void GridLookupPantheonUsers_DataBinding(object sender, EventArgs e)
        {
            List<PantheonUsers> list = CheckModelValidation(GetDatabaseConnectionInstance().GetPantheonUsers());
            (sender as ASPxGridLookup).DataSource = list;
        }

        protected void GridLookupRole_DataBinding(object sender, EventArgs e)
        {
            List<RoleModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetRolesOTP());
            (sender as ASPxGridLookup).DataSource = list;
        }
    }
}
