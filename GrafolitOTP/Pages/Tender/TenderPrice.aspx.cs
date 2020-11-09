using DatabaseWebService.ModelsOTP.Route;
using DatabaseWebService.ModelsOTP.Tender;
using DevExpress.Data.Mask;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Drawings;
using DevExpress.Web;
using DevExpress.XtraReports.UI;
using Ionic.Zip;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Helpers.Models;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Tender
{
    public partial class TenderPrice :ServerMasterPage
    {
        RouteModel model = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            ASPxGridLookupRelacija.GridView.Settings.GridLines = GridLines.Both;
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

                ASPxGridLookupRelacija.DataBind();
            }
            else
            {                
            }
        }

        protected void btnExportEvents_Click(object sender, EventArgs e)
        {
            //CommonMethods.ExportToPDFFitToPage(ASPxGridViewExporterEvents, this);
            ReportGeneratonHelper generator = new ReportGeneratonHelper();
            generator.CustomizeColumnsCollection += new CustomizeColumnsCollectionEventHandler(generator_CustomizeColumnsCollection);
            generator.CustomizeColumn += new CustomizeColumnEventHandler(generator_CustomizeColumn);

            model = (RouteModel)GetValueFromSession(Enums.RouteSession.RouteModel);

            DataTable dt = new DataTable();
            int idRoute = 0;

            if (model != null)
            {
                idRoute = model.RelacijaID;
            }

            string dtDatumRazpisa = DateEditDatumRazpisa.Value != null ? DateEditDatumRazpisa.Date.ToShortDateString() : "";
            List<TenderPositionModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetTenderListByRouteIDAndTenderDate(idRoute, dtDatumRazpisa));
            XtraReport report = generator.GenerateReport(ASPxGridViewRoutes, list);

            
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
            e.ColumnsInfo[0].ColumnWidth = 200;
            e.ColumnsInfo[1].ColumnWidth = 400;
            e.ColumnsInfo[2].ColumnWidth = 400;
            //e.ColumnsInfo[e.ColumnsInfo.Count - 1].IsVisible = false;

            e.ColumnsInfo[0].ColumnCaption = "Cena";
            e.ColumnsInfo[1].ColumnCaption = "Prevoznik";
        }


        protected void ASPxGridViewRoutes_DataBinding(object sender, EventArgs e)
        {
           

            DataTable dt = new DataTable();
            int idRoute = 0;
            
            if (model != null)
            {
                idRoute = model.RelacijaID;                
            }

            string dtDatumRazpisa = DateEditDatumRazpisa.Value != null ? DateEditDatumRazpisa.Date.ToShortDateString() : "";

            if (idRoute > 0)
            {
                List<TenderPositionModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetTenderListByRouteIDAndTenderDate(idRoute, dtDatumRazpisa));

                if (list == null)
                    list = new List<TenderPositionModel>();

                (sender as ASPxGridView).DataSource = list;
                AddValueToSession(Enums.RouteSession.CarrierListModel, list);
            }
            else
            {
                List<TenderPositionModel> listFromSes = (List<TenderPositionModel>)GetValueFromSession(Enums.RouteSession.CarrierListModel);
                if (listFromSes != null)
                {
                    (sender as ASPxGridView).DataSource = listFromSes;
                    AddValueToSession(Enums.RouteSession.CarrierListModel, listFromSes);
                }
            }
        }

        #region Initialize
        private void Initialize(int routeID)
        {
            PopulateModel(routeID);
            ASPxGridViewRoutes.DataBind();
        }
        #endregion

        private void PopulateModel(int routeID)
        {            
            model = CheckModelValidation(GetDatabaseConnectionInstance().GetRouteByID(routeID));
            AddValueToSession(Enums.RouteSession.RouteModel, model);
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

        protected void ASPxGridLookupRealacija_DataBinding(object sender, EventArgs e)
        {
            List<RouteModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutes());
            (sender as ASPxGridLookup).DataSource = SerializeToDataTable(list);
        }

        protected void CallbackPanelRoutes_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] split = e.Parameter.Split(';');
            if (split[0] == "ShowPrices")
            {
                RemoveSession(Enums.RouteSession.RouteModel);
                int routeID = CommonMethods.ParseInt(ASPxGridLookupRelacija.Value);

                Initialize(routeID);
            }
           
        }

        protected void ASPxPopupControlRoutesDetail_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.RouteSession.RouteIDPopup);
            RemoveSession(Enums.RouteSession.CarrierIDPopup);
            RemoveSession(Enums.RouteSession.RouteNamePopup);
            RemoveSession(Enums.RouteSession.CarrierNamePopup);
            RemoveSession(Enums.RouteSession.RouteModel);

            ASPxGridLookupRelacija.GridView.Settings.GridLines = GridLines.Both;
        }

        protected void btnConfirmDownload_Click(object sender, EventArgs e)
        {

        }
    }
}