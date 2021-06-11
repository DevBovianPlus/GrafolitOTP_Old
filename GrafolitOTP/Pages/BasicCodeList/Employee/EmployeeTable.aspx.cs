using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DatabaseWebService.ModelsPDO;
using DevExpress.Web;
using DatabaseWebService.ModelsPDO.Inquiry;
using DatabaseWebService.Models;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using OptimizacijaTransprotov.Helpers;

namespace OptimizacijaTransprotov.Pages.BasicCodeList.Employee
{
    public partial class EmployeeTable : ServerMasterPage
    {
        int employeeIDFocusedRowIndex = 0;
        int filterType = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!PrincipalHelper.IsUserAdmin() && !PrincipalHelper.IsUserSuperAdmin()) RedirectHome();

            if (!Request.IsAuthenticated) RedirectHome();
            

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
                    ASPxGridViewEmployee.FocusedRowIndex = ASPxGridViewEmployee.FindVisibleIndexByKeyValue(employeeIDFocusedRowIndex);
                    ASPxGridViewEmployee.ScrollToVisibleIndexOnClient = ASPxGridViewEmployee.FindVisibleIndexByKeyValue(employeeIDFocusedRowIndex);
                }

                ASPxGridViewEmployee.DataBind();
                InitializeEditDeleteButtons();
            }
        }

        protected void ASPxGridViewInquiry_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] split = e.Parameters.Split(';');
            if (split[0] == "DblClick" && !String.IsNullOrEmpty(split[1]))
            {                
            }
        }

        protected void ASPxGridViewInquiry_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            if (e.GetValue("StatusPovprasevanja.Koda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.POTRJEN.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#dff0d8");//#
            else if (e.GetValue("StatusPovprasevanja.Koda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.DELOVNA.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFA879");//#fcf8e3

            if (CommonMethods.ParseBool(e.GetValue("Zakleni")))
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2dede");

            if (CommonMethods.ParseInt(e.GetValue("NarociloID")) > 0)
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffbf00");
        }

        protected void ASPxGridViewInquiry_DataBinding(object sender, EventArgs e)
        {
            List<EmployeeFullModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetAllEmployees());

            (sender as ASPxGridView).DataSource = list;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        private void InitializeEditDeleteButtons()
        {
            if (ASPxGridViewEmployee.VisibleRowCount <= 0)
            {
                btnEdit.ClientVisible = false;
                btnDelete.ClientVisible = false;
            }
        }

        protected void EmployeeCallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            object valueID = null;

            ClearAllSessions(Enum.GetValues(typeof(Enums.EmployeeSession)).Cast<Enums.EmployeeSession>().ToList());

            if (CommonMethods.ParseInt(e.Parameter) != (int)Enums.UserAction.Add)
                valueID = ASPxGridViewEmployee.GetRowValues(ASPxGridViewEmployee.FocusedRowIndex, "idOsebe");
            
            
            
            bool isValid = SetSessionsAndOpenPopUp(e.Parameter, Enums.EmployeeSession.EmployeeID, valueID);

            PopupControlEmployee.ShowOnPageLoad = isValid;

        }

        protected void PopupControlEmployee_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.EmployeeSession.EmployeeID);
            RemoveSession(Enums.EmployeeSession.EmployeeModel);
        }
    }
}