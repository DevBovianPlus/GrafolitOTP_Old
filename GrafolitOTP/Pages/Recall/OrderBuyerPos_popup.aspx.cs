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
    public partial class OrderBuyerPos_popup : ServerMasterPage
    {
        RecallBuyerFullModel model = null;
        List<DisconnectedInvoicesModel> listDisconectedInvoices = null;

        string supplierID;
        //RecallFullModel fullRecall;
        int recallAction = -1;
        int clientID = 0;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            recallAction = CommonMethods.ParseInt(GetStringValueFromSession(Enums.CommonSession.UserActionPopUp));
            supplierID = GetStringValueFromSession(Enums.OrderSession.SupplierID);
            clientID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.OrderSession.CientID));
            model = GetRecallDataProvider().GetRecallBuyerFullModel();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
            else
            {
                if (GetRecallDataProvider().GetRecallBuyerFullModel() != null)
                    model = GetRecallDataProvider().GetRecallBuyerFullModel();

                if (model != null)
                {
                    GetRecallDataProvider().SetRecallBuyerFullModel(model);
                }
                PopulateModel();
            }
        }


        #region Initialize
        private void Initialize()
        {
            if (listDisconectedInvoices == null)
                PopulateModel();

            //TODO: Pobriši ven tiste pozicije katere že obstajajo v odpoklicu.
            //model = model.OdpoklicKupecPozicija.TakeWhile(x => !fullRecall.OdpoklicPozicija.Any(rp => rp.NarociloID == x.Narocilnica && rp.NarociloPozicijaID == x.St_Pozicija)).ToList();

            ASPxGridViewOrdersBuyerPositions.DataBind();
        }

        private void PopulateModel()
        {
            if (listDisconectedInvoices == null)
            {
                if (GetRecallDataProvider().GetDisconnectedInvoicesList() != null)
                {
                    listDisconectedInvoices = GetRecallDataProvider().GetDisconnectedInvoicesList();
                }
                else
                {
                    listDisconectedInvoices = CheckModelValidation(GetDatabaseConnectionInstance().GetDisconnectedInvoices());
                }
            }

            //TODO: pobrišemo pozicije iz seznama ki so že na dopoklicu
            if (listDisconectedInvoices != null && model != null)
            {
                foreach (var item in model.OdpoklicKupecPozicija)
                {
                    var obj = listDisconectedInvoices.Where(pos => pos.acKey == item.acKey).FirstOrDefault();
                    if (obj != null) listDisconectedInvoices.Remove(obj);
                }
            }

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
            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}', '{2}', '{3}');", confirmCancelAction, "OrderBuyerPos", (int)Enums.UserAction.Edit, model.OdpoklicKupecID), true);
            RemoveSession(Enums.RecallSession.DisconnectedInvoicesList);
            /*}
            else
                ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "OrderPos"), true);*/

        }
        #endregion

        protected void ASPxGridViewOrdersBuyerPositions_DataBinding(object sender, EventArgs e)
        {
                if (GetRecallDataProvider().GetDisconnectedInvoicesList() != null)
            {
                listDisconectedInvoices = GetRecallDataProvider().GetDisconnectedInvoicesList();
            }

            (sender as ASPxGridView).DataSource = listDisconectedInvoices;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }


        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            List<object> selectedRows = ASPxGridViewOrdersBuyerPositions.GetSelectedFieldValues("acKey");
            DisconnectedInvoicesModel obj = null;
            

            if (listDisconectedInvoices == null)
            {
                if (GetRecallDataProvider().GetDisconnectedInvoicesList() != null)
                {
                    listDisconectedInvoices = GetRecallDataProvider().GetDisconnectedInvoicesList();
                }
                else
                {
                    listDisconectedInvoices = CheckModelValidation(GetDatabaseConnectionInstance().GetDisconnectedInvoices());
                }
            }

            int sequentialNum = 0;
            if (model.OdpoklicKupecPozicija.Count > 0)
                sequentialNum = model.OdpoklicKupecPozicija.Max(p => p.ZaporednaStevilka) + 1;
            else
                sequentialNum = 1;

            if (model == null) model = GetRecallDataProvider().GetRecallBuyerFullModel();


            foreach (var item in selectedRows)
            {
                string id = item.ToString();
                obj = listDisconectedInvoices.Where(o => o.acKey == id).FirstOrDefault();
                if (obj != null)
                {
                    model.OdpoklicKupecPozicija.Add(new RecallBuyerPositionModel
                    {
                        acKey = obj.acKey,
                        Kljuc = obj.Kljuc,
                        Datum = obj.Datum,
                        Kupec = obj.Kupec,
                        Prevzemnik = obj.Prevzemnik,
                        Vrednost = obj.ZnesekFakture,
                        Kolicina = obj.Kolicina,
                        Akcija = (int)Enums.UserAction.Add,
                        addedFromPopUp = true
                    }) ; 
                }
            }

            if (recallAction == (int)Enums.UserAction.Add)
            {


                // TODO: Nastavi status odpoklica če je odpoklic v dodajanju
                model.StatusID = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatuses())
                    .Where(rs => rs.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA.ToString())
                    .FirstOrDefault().StatusOdpoklicaID;

                model.OdpoklicKupecID = 0;
                model.ts = DateTime.Now;
                model.tsIDOseba = PrincipalHelper.GetUserPrincipal().ID;

                SetIDsAndBoolProperty();
            }
            else
            {
                SetIDsAndBoolProperty(true);
            }

            model = CommonMethods.CalculatePercentageShippingCost(model);
            GetRecallDataProvider().SetRecallBuyerFullModel(model);
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
            if (model.OdpoklicKupecPozicija != null)//pri dodajanju novega odpoklica imamo v začetku nastavljene začasne id-je. Zato jih je potrebno ob shranjevanju nastavit na 0, da dobijo sql-ove id-je
            {
                foreach (var item in model.OdpoklicKupecPozicija)
                {

                    if (resetAddedFromPopUp && item.addedFromPopUp)
                    {
                        item.OdpoklicKupecPozicijaID = 0;
                        item.addedFromPopUp = false;
                    }
                    else if (!resetAddedFromPopUp)
                    {
                        item.OdpoklicKupecPozicijaID = 0;
                    }
                }
            }
        }
    }
}