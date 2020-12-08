using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.ModelsOTP.Recall;
using DevExpress.Web;
using DevExpress.Web.Rendering;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
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
    public partial class OrderPos_popup :  ServerMasterPage 
    {
        List<OrderPositionModelNew> model;
        string supplierID;
        RecallFullModel fullRecall;
        int recallAction = -1;
        int clientID = 0;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            recallAction = CommonMethods.ParseInt(GetStringValueFromSession(Enums.CommonSession.UserActionPopUp));
            supplierID = GetStringValueFromSession(Enums.OrderSession.SupplierID);
            clientID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.OrderSession.CientID));
            fullRecall = GetRecallDataProvider().GetRecallFullModel();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize(supplierID);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.OrderSession.OrdersPositionsList))
                    model = GetOrderDataProvider().GetOrderPositions();
            }
        }


        #region Initialize
        private void Initialize(string supplier)
        {
            if (model == null)
                PopulateModel(supplier);

            //TODO: Pobriši ven tiste pozicije katere že obstajajo v odpoklicu.
            //model = model.TakeWhile(x => !fullRecall.OdpoklicPozicija.Any(rp => rp.NarociloID == x.Narocilnica && rp.NarociloPozicijaID == x.St_Pozicija)).ToList();

            ASPxGridViewOrdersPositions.DataBind();
        }

        private void PopulateModel(string supplier)
        {
            model = CheckModelValidation(GetDatabaseConnectionInstance().GetOrderPositionsBySupplier(supplier, clientID));

            //TODO: pobrišemo pozicije iz seznama ki so že na dopoklicu
            if (fullRecall != null && model != null)
            {
                foreach (var item in fullRecall.OdpoklicPozicija)
                {
                    var obj = model.Where(m => m.Narocilnica == item.NarociloID && m.St_Pozicija == item.NarociloPozicijaID).FirstOrDefault();
                    if (obj != null) model.Remove(obj);
                }
            }

            GetOrderDataProvider().SetOrderPositions(model);
        }
        #endregion

        #region Helper methods
        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.OrderSession.SupplierID);
            RemoveSession(Enums.OrderSession.CientID);

            /*if (recallAction == (int)Enums.UserAction.Add)
            {*/
            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}', '{2}', '{3}');", confirmCancelAction, "OrderPos", (int)Enums.UserAction.Edit, fullRecall.OdpoklicID), true);
                RemoveSession(Enums.RecallSession.RecallFulModel);
            /*}
            else
                ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "OrderPos"), true);*/

        }
        #endregion

        protected void ASPxGridViewOrdersPositions_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = model;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridViewOrdersPositions_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void ASPxGridViewOrdersPositions_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
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

        protected void ASPxGridViewOrdersPositions_CommandButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCommandButtonEventArgs e)
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

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            List<object> selectedRows = ASPxGridViewOrdersPositions.GetSelectedFieldValues("tempID");
            OrderPositionModelNew obj = null;
            List<OrderPositionModelNew> selectedList = new List<OrderPositionModelNew>();
            
            int sequentialNum = 0;
            if (fullRecall.OdpoklicPozicija.Count > 0)
                sequentialNum = fullRecall.OdpoklicPozicija.Max(p => p.ZaporednaStevilka) + 1;
            else
                sequentialNum = 1;

            if (model == null) model = GetOrderDataProvider().GetOrderPositions();

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

                    fullRecall.OdpoklicPozicija.Add(new RecallPositionModel
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
                        OdpoklicPozicijaID = fullRecall.OdpoklicPozicija.Count > 0 ? fullRecall.OdpoklicPozicija.Max(op => op.OdpoklicPozicijaID) + 1 : obj.tempID,
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
                        EnotaMere = obj.EnotaMere,
                        TransportnaKolicina = ReturnEnotaMere(obj.EnotaMere) == Enums.UnitsFromOrder.KG.ToString() ? (obj.Razlika - obj.OdpoklicKolicinaOTP) < 0 ? 0 : (obj.Razlika - obj.OdpoklicKolicinaOTP) : 0,
                        addedFromPopUp = true
                    });
                }
            }

            if (recallAction == (int)Enums.UserAction.Add)
            {
                // TODO: Nastavi status odpoklica če je odpoklic v dodajanju
                fullRecall.StatusID = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatuses())
                    .Where(rs => rs.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA.ToString())
                    .FirstOrDefault().StatusOdpoklicaID;

                fullRecall.OdpoklicID = 0;
                fullRecall.ts = DateTime.Now;
                fullRecall.tsIDOseba = PrincipalHelper.GetUserPrincipal().ID;
                fullRecall.UserID = PrincipalHelper.GetUserPrincipal().ID;

                SetIDsAndBoolProperty();
            }
            else
            {
                SetIDsAndBoolProperty(true);
            }

            fullRecall = CheckModelValidation(GetDatabaseConnectionInstance().SaveRecall(fullRecall));
            GetRecallDataProvider().SetRecallFullModel(fullRecall);
            RemoveSessionsAndClosePopUP(true);
        }

        private string ReturnEnotaMere(string enotaMere)
        {
            if (!String.IsNullOrEmpty(enotaMere))
                return enotaMere.Trim();

            return "";
        }

        private void SetIDsAndBoolProperty(bool resetAddedFromPopUp = false)
        {
            if (fullRecall.OdpoklicPozicija != null)//pri dodajanju novega odpoklica imamo v začetku nastavljene začasne id-je. Zato jih je potrebno ob shranjevanju nastavit na 0, da dobijo sql-ove id-je
            {
                foreach (var item in fullRecall.OdpoklicPozicija)
                {

                    if (resetAddedFromPopUp && item.addedFromPopUp)
                    {
                        item.OdpoklicPozicijaID = 0;
                        item.addedFromPopUp = false;
                    }
                    else if(!resetAddedFromPopUp)
                    {
                        item.OdpoklicPozicijaID = 0;
                    }
                }
            }
        }
    }
}