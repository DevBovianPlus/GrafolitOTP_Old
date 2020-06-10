using DatabaseWebService.ModelsOTP.Tender;
using DevExpress.Spreadsheet;
using DevExpress.Web;
using Ionic.Zip;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Statistic
{
    public partial class RoutesTransportersRecalls : ServerMasterPage
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
                ASPxGridViewCarrier.DataBind();
                ASPxGridViewRoutes.DataBind();
            }
        }

        protected void ASPxGridViewRoutes_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutes()).OrderByDescending(o=>o.RecallCount);
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridViewCarrier_DataBinding(object sender, EventArgs e)
        {
            string prevoznik = DatabaseWebService.Common.Enums.Enums.TypeOfClient.PREVOZNIK.ToString();
            (sender as ASPxGridView).DataSource = CheckModelValidation(GetDatabaseConnectionInstance().GetAllClients(prevoznik)).OrderByDescending(o => o.RecallCount);
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void CallbackPanelSendTenders_Callback(object sender, CallbackEventArgsBase e)
        {
           
        }

        protected void btnExportToExcelRoutes_Click(object sender, EventArgs e)
        {
            ASPxGridViewRoutesExporter.FileName = "Relacije_" + CommonMethods.GetTimeStamp();
            ASPxGridViewRoutesExporter.WriteXlsxToResponse();
        }

        protected void btnExportToExcelCarriers_Click(object sender, EventArgs e)
        {
            ASPxGridViewCarrierExporter.FileName = "Prevozniki_" + CommonMethods.GetTimeStamp();
            ASPxGridViewCarrierExporter.WriteXlsxToResponse();
        }
    }
}