using DevExpress.Web;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.BasicCodeList.TransportType
{
    public partial class TransportType : ServerMasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            ASPxGridViewTransportTypes.Settings.GridLines = GridLines.Both;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ASPxGridViewTransportTypes.DataBind();
            }
        }

        protected void TransportTypeCallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshGrid")
            {
                ASPxGridViewTransportTypes.DataBind();
            }
            else
            {
                object valueID = null;
                if (ASPxGridViewTransportTypes.VisibleRowCount > 0)
                    valueID = ASPxGridViewTransportTypes.GetRowValues(ASPxGridViewTransportTypes.FocusedRowIndex, "TipPrevozaID");

                bool isValid = SetSessionsAndOpenPopUp(e.Parameter, Enums.ClientSession.ClientTransportTypeID, valueID);
                if (isValid)
                    ASPxPopupControlTransportType.ShowOnPageLoad = true;
            }
        }

        protected void ASPxGridViewTransportTypes_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = CheckModelValidation(GetDatabaseConnectionInstance().GetAllTransportTypes());
        }

        protected void ASPxGridViewTransportTypes_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName != "ShranjevanjePozicij") return;

            if (CommonMethods.ParseBool(e.Value))
                e.DisplayText = Enums.CustomDisplayText.DA.ToString();
            else
                e.DisplayText = Enums.CustomDisplayText.NE.ToString();
        }

        protected void ASPxPopupControlTransportType_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.ClientSession.ClientTransportTypeID);
            RemoveSession(Enums.ClientSession.ClientTransportTypeModel);
        }
    }
}