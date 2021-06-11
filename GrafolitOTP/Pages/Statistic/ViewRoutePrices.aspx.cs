using DatabaseWebService.Models;
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

        hlpViewRoutePricesModel helperRPModel = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            DateTime dateStart = DateTime.Now.AddYears(-1).Date;
            DateTime dateEnd = DateTime.Now.Date;

            DateEditDatumOd.Date = dateStart;
            DateEditDatumDo.Date = dateEnd;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (helperRPModel == null) helperRPModel = new hlpViewRoutePricesModel();
                helperRPModel.DateFrom = DateEditDatumOd.Date;
                helperRPModel.DateTo = DateEditDatumDo.Date;
                helperRPModel.iWeightType = 1;
                helperRPModel.iViewType = 1;

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

        protected void btnXlsxExport_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporterTransportPricesCompare.WriteXlsxToResponse();
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

            model = (model == null) ? CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutesTransportPricesByViewType(helperRPModel)).lRouteTransporterPriceModel : GetRouteDataProvider().GetRouteTransportPrices();

            (sender as ASPxGridView).DataSource = model;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;

            GetRouteDataProvider().SetRouteTransportPrices(model);
        }

        protected void CallbackPanelRoute_Callback(object sender, CallbackEventArgsBase e)
        {
            int iWeight = Convert.ToInt32(RadioButtonTeza.Value);

            string sViewType = RadioButtonList.Value.ToString();

            if (helperRPModel == null) helperRPModel = new hlpViewRoutePricesModel();

            helperRPModel.DateFrom = DateEditDatumOd.Date;
            helperRPModel.DateTo = DateEditDatumDo.Date;
            helperRPModel.iWeightType = iWeight;

            if (sViewType == "AllValues")
            {
                helperRPModel.iViewType = 1;
            }
            else if (sViewType == "GrafolitPrevoz")
            {
                helperRPModel.iViewType = 2;
            }
            else if (sViewType == "Dobavitelj")
            {
                helperRPModel.iViewType = 3;
            }
            else if (sViewType == "Kupec")
            {
                helperRPModel.iViewType = 4;
            }
            else if (sViewType == "GrafolitLastniPrevoz")
            {
                helperRPModel.iViewType = 5;
            }

            model = CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutesTransportPricesByViewType(helperRPModel)).lRouteTransporterPriceModel;


            GetRouteDataProvider().SetRouteTransportPrices(model);
            ASPxGridViewRouteTransportPricesCompare.DataBind();
        }

        protected void ASPxGridViewRouteTransportPricesCompare_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.Caption != "Cena") return;
            if (Convert.ToInt32(e.Value) == 0)
                e.DisplayText = " ";
        }

        protected void ASPxGridViewRouteTransportPricesCompare_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            if (CommonMethods.ParseBool(e.GetValue("IsRoute").ToString()))
            {
                e.Row.Font.Bold = true;
                e.Row.Font.Underline = true;
            }
        }
    }
}