using DatabaseWebService.ModelsOTP.Client;
using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.ModelsOTP.Recall;
using DevExpress.Web;
using DevExpress.Web.Rendering;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Recall
{
    public partial class Order : ServerMasterPage
    {
        List<OrderPositionModelNew> model = null;
        
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            ASPxGridLookupDobavitelj.GridView.Settings.GridLines = GridLines.Both;
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
                ASPxGridLookupDobavitelj.DataBind();
            }
            else
            {
                if (model == null && SessionHasValue(Enums.OrderSession.OrdersPositionsList))
                    model = GetOrderDataProvider().GetOrderPositions();
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (model != null)
            {
                RecallFullModel recall = new RecallFullModel();
                recall.OdpoklicPozicija = new List<RecallPositionModel>();
                SupplierModel supplier = null;

                if (GetRecallDataProvider().GetSuppliersList() != null)
                {
                    supplier = GetRecallDataProvider().GetSuppliersList().Where(su => su.Dobavitelj == ASPxGridLookupDobavitelj.Value.ToString()).FirstOrDefault();
                    if (supplier != null)
                    {
                        recall.DobaviteljNaziv = supplier.Dobavitelj.Trim();
                        recall.DobaviteljNaslov = supplier.Naslov.Trim();
                        recall.DobaviteljPosta = supplier.Posta.Trim();
                        recall.DobaviteljKraj = supplier.Kraj.Trim();
                    }
                }
                
                List<object> selectedRows = ASPxGridViewOrdersPositions.GetSelectedFieldValues("tempID");
                OrderPositionModelNew obj = null;
                List<OrderPositionModelNew> selectedList = new List<OrderPositionModelNew>();
                int sequentialNum = 1;
                decimal maxRecallQuantity = 0;

                foreach (var item in selectedRows)
                {
                    int id = CommonMethods.ParseInt(item);
                    obj = model.Where(o => o.tempID == id).FirstOrDefault();
                    if (obj != null)
                    {
                        decimal izbranaKolicina = obj.Prevzeto <= 0 ? obj.OdpoklicKolicinaOTP : obj.Prevzeto;

                        if (obj.Proizvedeno > 0)
                            maxRecallQuantity = obj.Proizvedeno - izbranaKolicina;
                        else
                            maxRecallQuantity = obj.Razlika - izbranaKolicina;

                        recall.OdpoklicPozicija.Add(new RecallPositionModel
                        {
                            Kolicina = maxRecallQuantity < 0 ? 0 : maxRecallQuantity,//nastavimo trenutno odpoklicano količino razliko med količino iz naročila in prevzeto količino
                            KolicinaIzNarocila = obj.Naroceno,
                            Material = obj.Artikel,
                            NarociloID = obj.Narocilnica,
                            NarociloPozicijaID = obj.St_Pozicija,
                            OC = obj.Order_Confirm,
                            KolicinaPrevzeta = obj.Prevzeto,
                            KolicinaRazlika = obj.Razlika,
                            KupecNaziv = obj.Kupec,
                            OdpoklicPozicijaID = obj.tempID,
                            TrenutnaZaloga = obj.Zaloga,
                            OptimalnaZaloga = obj.Dovoljeno_Odpoklicati,
                            TipNaziv = obj.Tip,
                            Interno = obj.Interno,
                            Proizvedeno = obj.Proizvedeno,
                            MaterialIdent = obj.Ident,
                            KolicinaOTP = obj.VsotaOdpoklicKolicinaOTP,
                            ZaporednaStevilka = sequentialNum++,
                            KolicinaOTPPozicijaNarocilnice = obj.OdpoklicKolicinaOTP,
                            KupecNaslov = obj.Kupec_Naslov,
                            KupecKraj = obj.Kupec_Kraj,
                            KupecPosta = obj.Kupec_Posta,
                            OdpoklicIzLastneZaloge = supplier != null ? (supplier.StrankaSkladisceID > 0 ? true : false) : false,
                            EnotaMere = obj.EnotaMere,
                            TransportnaKolicina = ReturnEnotaMere(obj.EnotaMere) == Enums.UnitsFromOrder.KG.ToString() ? (obj.Razlika - izbranaKolicina) < 0 ? 0 : (obj.Razlika - izbranaKolicina) : 0
                        });
                    }
                }

                ClearAllSessions(Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList());
                GetRecallDataProvider().SetRecallFullModel(recall);
                RedirectWithCustomURI("RecallForm.aspx", (int)Enums.UserAction.Add, -1);
            }
        }

        private string ReturnEnotaMere(string enotaMere)
        {
            if (!String.IsNullOrEmpty(enotaMere))
                return enotaMere.Trim();

            return "";
        }

        protected void ASPxGridView1_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = model;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        #region Initialize
        private void Initialize(string supplier, int strankaSkladisceID = 0)
        {
            PopulateModel(supplier, strankaSkladisceID);
            ASPxGridViewOrdersPositions.DataBind();
        }
        #endregion

        private void PopulateModel(string supplier, int strankaSkladisceID = 0)
        {
           /* if (GetOrderDataProvider().GetOrderPositions() != null)
            {
                model = GetOrderDataProvider().GetOrderPositions();
            }
            else
            {
                model = CheckModelValidation(GetDatabaseConnectionInstance().GetOrdersPositions());
            }*/
            
            supplier = supplier.Replace("&", "|");
            model = CheckModelValidation(GetDatabaseConnectionInstance().GetOrderPositionsBySupplier(supplier, strankaSkladisceID));
            
            GetOrderDataProvider().SetOrderPositions(model);
        }

        protected void ASPxGridViewOrdersPositions_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "SupplierChanged")
            {
                RemoveSession(Enums.OrderSession.CientID);
                string supplier = ASPxGridLookupDobavitelj.Value != null ? ASPxGridLookupDobavitelj.Value.ToString() : "";
                var dobavitelj = GetRecallDataProvider().GetSuppliersList().Where(x => x.Dobavitelj == supplier).FirstOrDefault();
                
                //Če je bilo izbrano skladišče kot dobavitelj (Iz tabele Stranka_OTP)
                int strankaSkladisceID = 0;
                if (dobavitelj != null)
                {
                    strankaSkladisceID = dobavitelj.StrankaSkladisceID;
                    if (dobavitelj.StrankaSkladisceID > 0) AddValueToSession(Enums.OrderSession.CientID, dobavitelj.StrankaSkladisceID);
                }

                Initialize(supplier, strankaSkladisceID);
            }
        }

        protected void ASPxGridLookupDobavitelj_DataBinding(object sender, EventArgs e)
        {
            GetRecallDataProvider().SetSuppliersList(CheckModelValidation(GetDatabaseConnectionInstance().GetAllSuppliers()));
            (sender as ASPxGridLookup).DataSource = GetRecallDataProvider().GetSuppliersList();
        }

        protected void ASPxGridViewOrdersPositions_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (model != null)
            { 
                int keyValue = CommonMethods.ParseInt(e.KeyValue);
                var item = model.Where(poz => poz.tempID == keyValue).FirstOrDefault();
                if (item != null && String.IsNullOrEmpty(item.Order_Confirm))
                {
                    GridViewTableCommandCell cell = (GridViewTableCommandCell)e.Row.Cells[0];
                    if (cell.Column.ShowSelectCheckbox)
                    {
                        e.Row.BackColor = Color.LightGray;
                    }
                }
            }
            
        }

        protected void ASPxGridViewOrdersPositions_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            object item = ASPxGridViewOrdersPositions.GetRowValues(e.VisibleIndex, "tempID", "Order_Confirm");
            if (item != null)
            {
                object[] values = (object[])item;
                if (values[1] == null)
                    e.Visible = false;
                else if (String.IsNullOrEmpty(values[1].ToString()))
                    e.Visible = false;
            }
        }
    }
}