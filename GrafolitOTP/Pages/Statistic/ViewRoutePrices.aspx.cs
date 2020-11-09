using DatabaseWebService.ModelsOTP.Route;
using DatabaseWebService.ModelsOTP.Tender;
using DevExpress.Web;
using DevExpress.XtraReports.UI;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Statistic
{
    public partial class ViewRoutePrices : ServerMasterPage
    {
        List<RouteTransporterPricesModel> model = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ASPxGridViewRouteTransportPricesCompare.DataBind();

            }
            else
            {
                if (model == null && SessionHasValue(Enums.RouteSession.RouteTransportPriceModel))
                    model = GetRouteDataProvider().GetRouteTransportPrices();
            }
        }

        protected void btnExportTransportPricesCompare_Click(object sender, EventArgs e)
        {
            //CommonMethods.ExportToPDFFitToPage(ASPxGridViewExporterEvents, this);
            ReportGeneratonHelper generator = new ReportGeneratonHelper();
            generator.CustomizeColumnsCollection += new CustomizeColumnsCollectionEventHandler(generator_CustomizeColumnsCollection);
            generator.CustomizeColumn += new CustomizeColumnEventHandler(generator_CustomizeColumn);

            model = (List<RouteTransporterPricesModel>)GetValueFromSession(Enums.RouteSession.RouteTransportPriceModel);

            DataTable dt = new DataTable();
            XtraReport report = generator.GenerateReport(ASPxGridViewRouteTransportPricesCompare, model);


            generator.WritePdfToResponse(Response, "Relacije.pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());

        }


        void generator_CustomizeColumn(object source, ControlCustomizationEventArgs e)
        {
            //if (e.FieldName == "Stranka.NazivPrvi")
            //{             
            //    e.IsModified = true;
            //}
        }

        void control_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToBoolean(((XRShape)sender).Report.GetCurrentColumnValue("Discontinued")) == true)
                ((XRShape)sender).FillColor = Color.Yellow;
            else
                ((XRShape)sender).FillColor = Color.White;
        }

        void generator_CustomizeColumnsCollection(object source, ColumnsCreationEventArgs e)
        {
            e.ColumnsInfo[0].ColumnWidth = 80;
            e.ColumnsInfo[1].ColumnWidth = 300;
            e.ColumnsInfo[2].ColumnWidth = 100;
            e.ColumnsInfo[3].ColumnWidth = 50;
            e.ColumnsInfo[4].ColumnWidth = 100;
            e.ColumnsInfo[5].ColumnWidth = 50;
            e.ColumnsInfo[6].ColumnWidth = 100;
            e.ColumnsInfo[7].ColumnWidth = 50;
            e.ColumnsInfo[8].ColumnWidth = 100;
            e.ColumnsInfo[9].ColumnWidth = 50;



            e.ColumnsInfo[0].ColumnCaption = "Št. prevozov";
            e.ColumnsInfo[1].ColumnCaption = "Relacija";
        }

        protected void ASPxGridViewRouteTransportPricesCompare_DataBinding(object sender, EventArgs e)
        {
            model = (model == null) ? CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutesTransportPricesByViewType(1,1)) : GetRouteDataProvider().GetRouteTransportPrices();

            (sender as ASPxGridView).DataSource = model;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;

            GetRouteDataProvider().SetRouteTransportPrices(model);
        }

        protected void CallbackPanelRoute_Callback(object sender, CallbackEventArgsBase e)
        {
            int iWeight = Convert.ToInt32(RadioButtonTeza.Value);

            string sViewType = (CommonMethods.ParseInt(e.Parameter) > 0) ? RadioButtonList.Value.ToString() : e.Parameter;

            

            if (sViewType == "AllValues")
            {
                model = CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutesTransportPricesByViewType(1, iWeight));
            }
            else if (sViewType == "LastniPrevoz")
            {
                model = CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutesTransportPricesByViewType(2, iWeight));
            }
            else if (sViewType == "OstaliPrevoz")
            {
                model = CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutesTransportPricesByViewType(3, iWeight));
            }
                       
            GetRouteDataProvider().SetRouteTransportPrices(model);
            ASPxGridViewRouteTransportPricesCompare.DataBind();
        }

    }
}