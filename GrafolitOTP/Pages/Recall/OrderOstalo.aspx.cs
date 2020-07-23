using DatabaseWebService.ModelsOTP.Client;
using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.ModelsOTP.Recall;
using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.Web.Rendering;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Recall
{
    public partial class OrderOstalo : ServerMasterPage
    {
        RecallFullModel modelSelectedRecals;
        List<OrderPositionModelNew> model = null;
        List<OrderPositionModelNew> modelOrder10 = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            

            if (modelSelectedRecals == null && SessionHasValue(Enums.RecallSession.RecallFulModel))
                modelSelectedRecals = GetRecallDataProvider().GetRecallFullModel();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (modelSelectedRecals != null)
                { 
                    txtDobaviteljNaziv.Text = modelSelectedRecals.DobaviteljNaziv;                    
                    lblPDO.Text = "Naročilnice NOZ: " + modelSelectedRecals.IzbranaKolicinaPDO + " kg";
                    lblNoz.Text = "Naročilnice PDO: " + modelSelectedRecals.IzbranaKolicinaNOZ + " kg";
                    lblSUM.Text = "SKUPAJ: " + modelSelectedRecals.SkupajNOZPDO + " kg";

                    lblNOZPDOOStaloZaOdpoklic.Text = "NOZ + PDO : " + modelSelectedRecals.SkupajNOZPDO + " kg";
                    hfCurrentSumPDONOZ["SumPDONOZ"] = modelSelectedRecals.SkupajNOZPDO;
                }

                modelOrder10 = CheckModelValidation(GetDatabaseConnectionInstance().GetListOfOrderNumber10());
                GetRecallDataProvider().SetOrder10Positions(modelOrder10);
                ASPxGridViewOrder10Positions.DataBind();

                Initialize(txtDobaviteljNaziv.Text);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.OrderSession.OrdersPositionsList))
                    model = GetOrderDataProvider().GetOrderPositions();
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {

            if (modelSelectedRecals == null && SessionHasValue(Enums.RecallSession.RecallFulModel))
                modelSelectedRecals = GetRecallDataProvider().GetRecallFullModel();

            if (modelSelectedRecals != null)
            {
                               
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

                        modelSelectedRecals.OdpoklicPozicija.Add(new RecallPositionModel
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
                            OdpoklicIzLastneZaloge = modelSelectedRecals.Dobavitelj != null ? true : false,
                            EnotaMere = obj.EnotaMere,
                            TransportnaKolicina = ReturnEnotaMere(obj.EnotaMere) == Enums.UnitsFromOrder.KG.ToString() ? (obj.Razlika - izbranaKolicina) < 0 ? 0 : (obj.Razlika - izbranaKolicina) : 0
                        });
                    }
                }

                ClearAllSessions(Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList());
                GetRecallDataProvider().SetRecallFullModel(modelSelectedRecals);
                RedirectWithCustomURI("RecallForm.aspx", (int)Enums.UserAction.Add, -1);
            }
        }

        private string ReturnEnotaMere(string enotaMere)
        {
            if (!String.IsNullOrEmpty(enotaMere))
                return enotaMere.Trim();

            return "";
        }

        protected void ASPxGridViewOrdersPositions_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = model;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridViewOrder10Positions_DataBinding(object sender, EventArgs e)
        {
            //modelOrder10 = GetRecallDataProvider().GetOrder10Positions();


            (sender as ASPxGridView).DataSource = modelOrder10;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        #region Initialize
        private void Initialize(string supplier, int strankaSkladisceID = 0)
        {
            PopulateModel(supplier, strankaSkladisceID);
            ASPxGridViewOrdersPositions.DataBind();
            ASPxGridViewOrder10Positions.DataBind();
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
            if (model != null) model = model.Where(opm => opm.TipAplikacije.Length==0).ToList();
            modelOrder10 = GetRecallDataProvider().GetOrder10Positions();
            if ((supplier != null) && (supplier.Length > 0))
            {
                if (modelOrder10 != null) modelOrder10 = modelOrder10.Where(opm => opm.Dobavitelj.Trim() == txtDobaviteljNaziv.Text).ToList();
            }


            ASPxGridViewOrder10Positions.DataBind();
            GetOrderDataProvider().SetOrderPositions(model);
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

        protected void ASPxGridViewOrder10Positions_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (model != null)
            {
                int keyValue = CommonMethods.ParseInt(e.KeyValue);
                var item = model.Where(poz => poz.tempID == keyValue).FirstOrDefault();
                if (item != null && String.IsNullOrEmpty(item.Order_Confirm) && e.Row.Cells[0].GetType() == typeof(GridViewTableCommandCell))
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
                if (values[0] != null)
                {
                    if (values[1] == null)
                        e.Visible = false;
                    else if (String.IsNullOrEmpty(values[1].ToString()))
                        e.Visible = false;
                }
            }
        }

        protected void ASPxGridViewOrdersPositions_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {

            if (model != null)
            {
                List<OrderPositionModelNew> updateList = model.ToList() ?? new List<OrderPositionModelNew>();
                OrderPositionModelNew opm = null;

                Type myType = typeof(OrderPositionModelNew);
                List<PropertyInfo> myPropInfo = myType.GetProperties().ToList();

                //Spreminjanje zapisov v gridu
                foreach (ASPxDataUpdateValues item in e.UpdateValues)
                {
                    opm = new OrderPositionModelNew();

                    foreach (DictionaryEntry obj in item.Keys)//we set table ID
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                        {
                            opm = updateList.Where(ips => ips.tempID == (int)obj.Value).FirstOrDefault();
                            break;
                        }
                    }

                    foreach (DictionaryEntry obj in item.NewValues)
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                        {
                            info.SetValue(opm, obj.Value);
                        }
                    }
                }


            }

            e.Handled = true;
        }

        
    }
}