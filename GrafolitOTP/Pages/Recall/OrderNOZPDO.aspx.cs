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
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Recall
{
    public partial class OrderNOZPDO : ServerMasterPage
    {
        List<OrderPositionModelNew> model = null;
        List<OrderPositionModelNew> modelOrder10 = null;
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
                modelOrder10 = CheckModelValidation(GetDatabaseConnectionInstance().GetListOfOrderNumber10());
                GetRecallDataProvider().SetOrder10Positions(modelOrder10);
                ASPxGridViewOrder10Positions.DataBind();
                GridLookupCategory.DataBind();
            }
            else
            {
                if (model == null && SessionHasValue(Enums.OrderSession.OrdersPositionsList))
                    model = GetOrderDataProvider().GetOrderPositions();
            }
        }

        private RecallFullModel SetSelectedQnt(RecallFullModel recall)
        {


            decimal dSumPDO = CommonMethods.ParseDecimal(hfCurrentSumPDO["CurrenSumPDO"].ToString());
            decimal dSumNOZ = CommonMethods.ParseDecimal(hfCurrentSumNOZ["CurrenSumNOZ"].ToString());

            decimal dSumPDONOZ = dSumNOZ + dSumPDO;

            recall.IzbranaKolicinaPDO = dSumPDO;
            recall.IzbranaKolicinaNOZ = dSumNOZ;
            recall.SkupajNOZPDO = dSumPDONOZ;

            return recall;
        }

        protected void PerformClickNaprej()
        {
            if (model != null)
            {
                RecallFullModel recall = new RecallFullModel();
                recall.OdpoklicPozicija = new List<RecallPositionModel>();
                SupplierModel supplier = null;


                SetSelectedQnt(recall);

                if (GetRecallDataProvider().GetSuppliersList() != null)
                {
                    supplier = GetRecallDataProvider().GetSuppliersList().Where(su => su.Dobavitelj == ASPxGridLookupDobavitelj.Value.ToString()).FirstOrDefault();
                    if (supplier != null)
                    {

                        recall.DobaviteljNaziv = supplier.Dobavitelj.Trim();
                        recall.DobaviteljNaslov = supplier.Naslov.Trim();
                        recall.DobaviteljPosta = supplier.Posta.Trim();
                        recall.DobaviteljKraj = supplier.Kraj.Trim();


                        GetRecallDataProvider().SetSelectSupplier(supplier);

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

                        maxRecallQuantity = maxRecallQuantity == 0 ? obj.Razlika : maxRecallQuantity;

                        recall.OdpoklicPozicija.Add(new RecallPositionModel
                        {
                            Kolicina = maxRecallQuantity < 0 ? (obj.Razlika < 0 ? 0 : obj.Razlika) : maxRecallQuantity,//nastavimo trenutno odpoklicano količino razliko med količino iz naročila in prevzeto količino
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
                ASPxWebControl.RedirectOnCallback(GenerateURI("RecallForm.aspx", (int)Enums.UserAction.Add, -1));
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

        protected void GridLookupCategory_DataBinding(object sender, EventArgs e)
        {
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

        private List<OrderPositionModelNew> FilterOrderPositionsByCategory(List<OrderPositionModelNew> model, string selCategories)
        {
            var predicate = PredicateBuilder.True<OrderPositionModelNew>();
            selCategories = selCategories.Replace(", ", ",");

            var catg = selCategories.Split(',').ToList();

            // where in 
            model = model.Where(e => catg.Any(ee => ee == e.Kategorija)).ToList();



            return model;
        }

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
            string selCategory = GridLookupCategory.Text.Trim();

            supplier = supplier.Replace("&", "|");
            model = CheckModelValidation(GetDatabaseConnectionInstance().GetOrderPositionsBySupplier(supplier, strankaSkladisceID));
            modelOrder10 = GetRecallDataProvider().GetOrder10Positions();
            if (selCategory.Length > 0)
            {
                model = FilterOrderPositionsByCategory(model, selCategory);
                modelOrder10 = FilterOrderPositionsByCategory(modelOrder10, selCategory);
            }
            //if (model != null) model = model.Where(opm => opm.TipAplikacije == "PDO" || opm.TipAplikacije == "NOZ").ToList();

            if ((supplier != null) && (supplier.Length > 0))
            {
                if (modelOrder10 != null) modelOrder10 = modelOrder10.Where(opm => opm.Dobavitelj == supplier).ToList();
            }


            ASPxGridViewOrder10Positions.DataBind();
            GetOrderDataProvider().SetOrderPositions(model);
        }

        //protected void ASPxGridViewOrdersPositions_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    if (e.Parameters == "SupplierChanged")
        //    {
        //        RemoveSession(Enums.OrderSession.CientID);
        //        string supplier = ASPxGridLookupDobavitelj.Value != null ? ASPxGridLookupDobavitelj.Value.ToString() : "";
        //        var dobavitelj = GetRecallDataProvider().GetSuppliersList().Where(x => x.Dobavitelj == supplier).FirstOrDefault();

        //        //Če je bilo izbrano skladišče kot dobavitelj (Iz tabele Stranka_OTP)
        //        int strankaSkladisceID = 0;
        //        if (dobavitelj != null)
        //        {
        //            strankaSkladisceID = dobavitelj.StrankaSkladisceID;
        //            if (dobavitelj.StrankaSkladisceID > 0) AddValueToSession(Enums.OrderSession.CientID, dobavitelj.StrankaSkladisceID);
        //        }

        //        Initialize(supplier, strankaSkladisceID);
        //    }
        //}



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

        protected void cbpRefreshsUpplier_Callback(object sender, CallbackEventArgsBase e)
        {
            string supplier = ASPxGridLookupDobavitelj.Value != null ? ASPxGridLookupDobavitelj.Value.ToString() : "";

            if (e.Parameter == "SupplierChanged")
            {
                RemoveSession(Enums.OrderSession.CientID);
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
            else if (e.Parameter == "ClickNaprej")
            {
                PerformClickNaprej();
            }
            else if (e.Parameter == "CategoryChanged")
            {
                Initialize(supplier, 0);
            }
        }
    }
}