using DatabaseWebService.ModelsOTP.Client;
using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.ModelsOTP.Recall;
using DatabaseWebService.ModelsOTP.Route;
using DatabaseWebService.ModelsOTP.Tender;
using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.Web.Rendering;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Recall
{
    public partial class RecallBuyerList : ServerMasterPage
    {
        RecallBuyerFullModel model = null;
        List<DisconnectedInvoicesModel> listDisconectedInvoices = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();

                /*if (model != null)
                {
                    GetOrderDataProvider().SetOrderPositions(model);
                }*/                
            }
            else
            {                              
            }
        }
        

        protected void PerformClickNaprej()
        {
            CreateModel();

            if (model != null)
            {
                //RecallFullModel recall = new RecallFullModel();
                //recall.OdpoklicPozicija = new List<RecallPositionModel>();                

                //ClearAllSessions(Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList());
                //GetRecallDataProvider().SetRecallFullModel(recall);
                model.KolicinaSkupno = CommonMethods.ParseDecimal (hfQnt["SelectQnt"]);
                ASPxWebControl.RedirectOnCallback(GenerateURI("RecallBuyerCreate.aspx", (int)Enums.UserAction.Add, -1));
            }
        }

        private void CreateModel()
        {
            model = new RecallBuyerFullModel();
            DisconnectedInvoicesModel obj = null;            
            model.RelacijaID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupRealacija));

            List<object> selectedRows = ASPxGridViewDisconnectedInvoices.GetSelectedFieldValues("TempID");
            int i = 0 ;
            foreach (var item in selectedRows)
            {
                int id = CommonMethods.ParseInt(item);
                i++;

                if (GetRecallDataProvider().GetDisconnectedInvoicesList() != null)
                {
                    listDisconectedInvoices = GetRecallDataProvider().GetDisconnectedInvoicesList();
                }

                obj = listDisconectedInvoices.Where(o => o.TempID == id).FirstOrDefault();

                if (obj != null)
                {
                    RecallBuyerPositionModel recallBuyPos = new RecallBuyerPositionModel();

                    recallBuyPos.ZaporednaStevilka = i;
                    recallBuyPos.Akcija = (int)Enums.UserAction.Add;
                    recallBuyPos.acKey = obj.acKey;
                    recallBuyPos.Kljuc = obj.Kljuc;
                    recallBuyPos.Datum = obj.Datum;
                    recallBuyPos.Kolicina = obj.Kolicina;
                    recallBuyPos.Vrednost = obj.ZnesekFakture;
                    recallBuyPos.Kupec = obj.Kupec;
                    recallBuyPos.Prevzemnik = obj.Prevzemnik;                    

                    if (model.OdpoklicKupecPozicija == null) model.OdpoklicKupecPozicija = new List<RecallBuyerPositionModel>();

                    model.OdpoklicKupecPozicija.Add(recallBuyPos);
                }        
            }

            GetRecallDataProvider().SetRecallBuyerFullModel(model);            
        }




        protected void ASPxGridViewDisconnectedInvoices_DataBinding(object sender, EventArgs e)
        {
            listDisconectedInvoices = CheckModelValidation(GetDatabaseConnectionInstance().GetDisconnectedInvoices());

            GetRecallDataProvider().SetDisconnectedInvoicesList(listDisconectedInvoices);

            (sender as ASPxGridView).DataSource = listDisconectedInvoices;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }



        protected void ASPxGridLookupRealacija_DataBinding(object sender, EventArgs e)
        {
            List<RouteModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutes());
            (sender as ASPxGridLookup).DataSource = SerializeToDataTable(list);
        }
       



        #region Initialize
        private void Initialize()
        {
            ASPxGridLookupRealacija.DataBind();
            ASPxGridViewDisconnectedInvoices.DataBind();
            //ASPxGridLookupZbirnikTon.DataBind();
        }
        #endregion
      
        protected void ASPxGridViewDisconnectedInvoices_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
           
        }

        protected void ASPxGridViewOrder10Positions_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
          
        }

        protected void ASPxGridViewDisconnectedInvoices_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            //object item = ASPxGridViewDisconnectedInvoices.GetRowValues(e.VisibleIndex, "TempID");
            //if (item != null)
            //{

            //    object[] values = (object[])item;
            //    if (values[0] != null)
            //    {
            //        if (values[1] == null)
            //            e.Visible = false;
            //        else if (String.IsNullOrEmpty(values[1].ToString()))
            //            e.Visible = false;
            //    }
            //}
        }

        protected void ASPxGridViewDisconnectedInvoices_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            e.Handled = true;
        }

        protected void RecallCallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
          if (e.Parameter == "ClickNaprej")
            {
                PerformClickNaprej();
            }
          else if (e.Parameter == "CategoryChanged")
            {
                
            }
        }


    }
}