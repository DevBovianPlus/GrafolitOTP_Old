using DatabaseWebService.Models.Client;
using DatabaseWebService.ModelsOTP.Client;
using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.ModelsOTP.Recall;
using DatabaseWebService.ModelsOTP.Route;
using DatabaseWebService.ModelsOTP.Tender;
using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.XtraReports.UI;
using Newtonsoft.Json;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Recall
{
    public partial class RecallBuyerCreate : ServerMasterPage
    {
        RecallBuyerFullModel model = null;
        int recallID = -1;
        int action = -1;
        int iRefresh = 0;
        public decimal maxKolicina = 0;
        public string criticalTransportType = "15 - SKLADIŠČE MALOPRODAJA";
        public bool reopenRecall = false;
        private bool recallStatusChanged = false;
        private bool bIsRejectOrAccept = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
            {
                action = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.action.ToString()].ToString());
                recallID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());
                if (Request.QueryString[Enums.QueryStringName.Refresh.ToString()] != null)
                {
                    iRefresh = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.Refresh.ToString()].ToString());
                }
            }

            ASPxGridLookupRealacija.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupPrevoznik.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupZbirnikTon.GridView.Settings.GridLines = GridLines.Both;

            //maxKolicina = GetMaxQuantityForRecall();

            this.Master.DisableNavBar = true;
        }


        protected void ASPxGridViewDisconnectedInvoices_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = model.OdpoklicKupecPozicija.Where(p => p.Akcija != CommonMethods.ParseInt(Enums.UserAction.Delete)).ToList();
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridLookupPrevoznik_DataBinding(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            object id = null;

            object id2 = null;

            if (ASPxGridLookupRealacija.Value != null)
                id = ASPxGridLookupRealacija.Value;
            else if (model != null && model.RelacijaID > 0)
                id = model.RelacijaID;
            id2 = (ASPxGridLookupZbirnikTon.Value == null ? model.ZbirnikTonID : ASPxGridLookupZbirnikTon.Value);

            if (id != null)
            {
                int idRoute = CommonMethods.ParseInt(id);
                int idZbirnikTon = CommonMethods.ParseInt(id2);
                List<TenderPositionModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetTenderListByRouteIDandZbirnikTon(idRoute, idZbirnikTon));

                if (list == null)
                    list = new List<TenderPositionModel>();

                //list.Add(new TenderPositionModel { RazpisID = -1, Stranka = new ClientFullModel { idStranka = -1, NazivPrvi = "Izberi..." } });

                EnableUserControls();

                if (list.Count > 0)
                {
                    GetRecallDataProvider().SetTenderListFromSelectedRoute(list);

                    (sender as ASPxGridLookup).DataSource = list;

                    if (action == (int)Enums.UserAction.Add)
                        ASPxGridLookupPrevoznik.Value = -1;
                }
            }
            //else
            //  ClearSessionsAndRedirect();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                if (action == (int)Enums.UserAction.Edit || action == (int)Enums.UserAction.Delete || action == (int)Enums.UserAction.Storno)
                {
                    if (recallID > 0)
                    {
                        if (GetRecallDataProvider().GetRecallBuyerFullModel() != null)
                            model = GetRecallDataProvider().GetRecallBuyerFullModel();
                        else
                        {
                            model = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallBuyerByID(recallID));
                        }

                        if (model != null)
                        {
                            GetRecallDataProvider().SetRecallBuyerFullModel(model);
                            FillForm();
                        }
                    }

                }
                else if (action == (int)Enums.UserAction.Add)
                {
                    SetFormDefaultValues();
                }
                UserActionConfirmBtnUpdate(btnConfirm, action);

            }
            else
            {
                if (model == null && SessionHasValue(Enums.RecallSession.RecallBuyerFulModel))
                    model = GetRecallDataProvider().GetRecallBuyerFullModel();
                if (model != null)
                {
                    if (model.CenaPrevozaSkupno > 0)
                    {
                        btnRecall.ClientEnabled = true;
                    }
                }

            }
            if (iRefresh == 1)
            {
                FillForm();
            }
        }

        private bool IfRazpisPozicijaExistInList(List<TenderPositionModel> list, int iID)
        {
            bool bReturn = false;
            if (list != null)
            {
                bReturn = list.Exists(s => s.RazpisPozicijaID == iID);
            }

            return bReturn;
        }

        private void FillForm()
        {
            ASPxGridLookupRealacija.Value = model.RelacijaID > 0 ? model.RelacijaID : -1;


            if (model.RazpisPozicijaID != null || model.RazpisPozicijaID > 0)
                if (IfRazpisPozicijaExistInList((List<TenderPositionModel>)ASPxGridLookupPrevoznik.DataSource, model.RazpisPozicijaID))
                {
                    ASPxGridLookupPrevoznik.Value = model.RazpisPozicijaID;//dobvitelja/prevoznika preberemo iz razpisa pozicije
                }
                else
                {
                    if (ASPxGridLookupPrevoznik.DataSource != null)
                    {
                        int iCnt = ((List<TenderPositionModel>)ASPxGridLookupPrevoznik.DataSource).Count;
                        if (iCnt > 0)
                        {
                            ASPxGridLookupPrevoznik.GridView.Selection.SelectRow(0);
                        }
                    }
                }
            else
            {
                ASPxGridLookupPrevoznik.Text = model.PrevoznikNaziv;
            }
            if (CommonMethods.ParseDecimal(txtNovaCena.Text.Length) > 0)
            {
                txtNovaCena.Text = CommonMethods.ParseDecimal(txtNovaCena.Text) != CommonMethods.ParseDecimal(model.CenaPrevozaSkupno) ? CommonMethods.ParseDecimal(txtNovaCena.Text).ToString("N2") : CommonMethods.ParseDecimal(model.CenaPrevozaSkupno).ToString("N2");
            }
            else
            {
                txtNovaCena.Text = CommonMethods.ParseDecimal(model.CenaPrevozaSkupno).ToString("N2");
            }
            //ComboBoxTip.SelectedIndex = model.TipID > 0 ? ComboBoxTip.Items.IndexOfValue(model.TipID.ToString()) : 0;
            txtStatus.Text = model.StatusNaziv != null ? model.StatusNaziv : "";
            txtStNarocilnice.Text = model.StevilkaNarocilnica != null ? model.StevilkaNarocilnica : "";
            memOpis.Text = model.OpisOdpoklicKupec != null ? model.OpisOdpoklicKupec : "";



            model.KolicinaSkupno = CommonMethods.ParseDecimal(GetTotalSummaryValue());

            txtStOdpoklic.Text = model.OdpoklicKupecStevilka.ToString();




            //FuncionalityBasedOnUserRole();
            if (iRefresh != 1)
            {
                if (model.StatusKoda != null && (model.StatusKoda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA.ToString() || model.StatusKoda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POPRAVLJENO_NAROCILO.ToString() || model.StatusKoda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.NAR_BREZ_FAKTURE.ToString()))
                    SetEnabledAllControls(true);
                else
                    SetEnabledAllControls(false);
            }



            ASPxGridLookupZbirnikTon.Value = model.ZbirnikTonID > 0 ? model.ZbirnikTonID : -1;

            // nastavimo zbirnik ton
            if (model.ZbirnikTonID == 0)
            {
                SetZbirnikTonByODpoklicValue();
            }
            if (model != null && model.StatusKoda != null && model.StatusOdpoklica.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.USTVARJENO_NAROCILO.ToString())
                btnReopenRecall.ClientVisible = true;

            if (action == (int)Enums.UserAction.Delete)
            {
                btnConfirm.ClientEnabled = true;
                btnReopenRecall.ClientVisible = false;
                btnStorno.ClientVisible = false;
            }

            if (action == (int)Enums.UserAction.Storno)
            {
                btnConfirm.ClientEnabled = false;
                btnReopenRecall.ClientVisible = false;
                btnStorno.ClientVisible = true;
            }

            if (model.StatusKoda != null && model.StatusKoda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.STORNO.ToString())
            {
                btnStorno.ClientEnabled = false;
            }
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = GetRecallDataProvider().GetRecallBuyerFullModel() != null ? GetRecallDataProvider().GetRecallBuyerFullModel() : new RecallBuyerFullModel();

                model.ts = DateTime.Now;
                model.tsIDOseba = PrincipalHelper.GetUserPrincipal().ID;
                model.UserID = PrincipalHelper.GetUserPrincipal().ID;

                if (model.OdpoklicKupecPozicija != null)//pri dodajanju novega odpoklica imamo v začetku nastavljene začasne id-je. Zato jih je potrebno ob shranjevanju nastavit na 0, da dobijo sql-ove id-je
                {
                    model.OdpoklicKupecPozicija.ForEach(poz => poz.OdpoklicKupecPozicijaID = 0);
                    model.OdpoklicKupecPozicija.ForEach(poz => poz.OdpoklicKupecPozicijaID = 0);
                }
            }
            else if (model == null && !add)
            {
                model = GetRecallDataProvider().GetRecallBuyerFullModel();
            }

            model.RelacijaID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupRealacija));
            model.RelacijaNaziv = ASPxGridLookupRealacija.Text;

            string recallStatusCode = GetRecallDataProvider().GetRecallStatus().ToString();


            if (model.IzdelajNarocilnico == 1)
            {
                model.StatusID = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatuses())
               .Where(rs => rs.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.USTVARJENO_NAROCILO.ToString())
               .FirstOrDefault().StatusOdpoklicaID;
            }
            else
            {
                model.StatusID = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatuses())
               .Where(rs => rs.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA.ToString())
               .FirstOrDefault().StatusOdpoklicaID;
            }

            if (recallStatusCode == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POPRAVLJENO_NAROCILO.ToString())
            {
                model.StatusID = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatuses())
                .Where(rs => rs.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POPRAVLJENO_NAROCILO.ToString())
                .FirstOrDefault().StatusOdpoklicaID;
            }

            if (recallStatusCode == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.STORNO.ToString())
            {
                model.StatusID = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatuses())
                .Where(rs => rs.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.STORNO.ToString())
                .FirstOrDefault().StatusOdpoklicaID;
                model.StatusKoda = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatuses())
                .Where(rs => rs.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.STORNO.ToString())
                .FirstOrDefault().Koda;
                model.StatusKoda = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatuses())
                .Where(rs => rs.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.STORNO.ToString())
                .FirstOrDefault().Naziv;
            }


            if (model.bBrezFakture)
            {
                model.StatusID = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatuses())
                  .Where(rs => rs.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.NAR_BREZ_FAKTURE.ToString())
                  .FirstOrDefault().StatusOdpoklicaID;
            }

            model.KolicinaSkupno = CommonMethods.ParseDecimal(GetTotalSummaryValue());
            model.ZbirnikTonID = CommonMethods.ParseInt(ASPxGridLookupZbirnikTon.Value);
            model.RazpisPozicijaID = CommonMethods.ParseInt(ASPxGridLookupPrevoznik.Value);
            model.CenaPrevozaSkupno = txtNovaCena.Text.Contains(".") ? CommonMethods.ParseDecimal(txtNovaCena.Text.Replace(".", ",")) : CommonMethods.ParseDecimal(txtNovaCena.Text);
            model.StevilkaNarocilnica = txtStNarocilnice.Text;
            model.OpisOdpoklicKupec = memOpis.Text;
            //

            List<TenderPositionModel> tenderList = GetRecallDataProvider().GetTenderListFromSelectedRoute();
            if (tenderList != null)
            {

                model.PrevoznikNaziv = (tenderList.Where(t => t.RazpisPozicijaID == model.RazpisPozicijaID).FirstOrDefault() != null ? tenderList.Where(t => t.RazpisPozicijaID == model.RazpisPozicijaID).FirstOrDefault().Stranka.NazivPrvi : "");

            }


            // preverimo, če je odpoklic zavrnjen ali potrjen, zaradi filtra
            if (recallStatusCode == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POTRJEN.ToString() || recallStatusCode == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.ZAVRNJEN.ToString())
                bIsRejectOrAccept = true;

            RecallBuyerFullModel returnModel = CheckModelValidation(GetDatabaseConnectionInstance().SaveBuyerRecall(model));

            RemoveSession(Enums.RecallSession.RecallStatus);




            if (returnModel != null)
            {
                //this we need if we want to add new client and then go and add new Plan with no redirection to Clients page
                model = returnModel;//if we need updated model in the same request;
                GetRecallDataProvider().SetRecallBuyerFullModel(returnModel);

                //TODO: ADD new item to session and if user has added new client and create data bind.
                return true;
            }
            else
                return false;
        }



        #region Initialization

        private void Initialize()
        {
            PopulateModel();

            ASPxGridLookupRealacija.DataBind();
            ASPxGridLookupZbirnikTon.DataBind();

            if (action != (int)Enums.UserAction.Add)
                ASPxGridLookupPrevoznik.DataBind();

            ASPxGridSelectPositions.DataBind();


            GetRecallDataProvider().SetRecallStatuses(CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatuses()));

            if (model.StatusKoda != null && model.StatusKoda != DatabaseWebService.Common.Enums.Enums.StatusOfRecall.USTVARJENO_NAROCILO.ToString())
            {
                CalculatePercShip();
                if (model.ZbirnikTonID == 0)
                {
                    SetZbirnikTonByODpoklicValue();
                }

                if (iRefresh == 1)
                    SetZbirnikTonByODpoklicValue();
            }

            if (model.StatusKoda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POPRAVLJENO_NAROCILO.ToString())
            {
                btnRecall.Text = "Osveži naročilnico";
            }
        }

        private void PopulateModel()
        {
            if (GetRecallDataProvider().GetRecallBuyerFullModel() != null)
            {
                model = GetRecallDataProvider().GetRecallBuyerFullModel();
            }
            else if (recallID > 0)
            {
                model = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallBuyerByID(recallID));
            }
        }

        protected object GetTotalSummaryValue()
        {
            object sum = null;
            //ASPxSummaryItem summaryItem = ASPxGridSelectPositions.TotalSummary.First(i => i.FieldName == "Kolicina");
            //if (hfCurrentSum.Contains("CurrenSum"))
            //{
            //    decimal sumHidden = CommonMethods.ParseDecimal(hfCurrentSum["CurrenSum"]);
            //    sum = CommonMethods.ParseDecimal(ASPxGridSelectPositions.GetTotalSummaryValue(summaryItem)).ToString("N2");
            //    decimal tSum = CommonMethods.ParseDecimal(sum);
            //    if (tSum > sumHidden)
            //        sum = sumHidden;
            //}
            //else
            //    sum = CommonMethods.ParseDecimal(ASPxGridSelectPositions.GetTotalSummaryValue(summaryItem)).ToString("N2");
            //// = sum;
            if (model != null)
            {
                if (model.OdpoklicKupecPozicija != null)
                {
                    sum = model.OdpoklicKupecPozicija.Where(p => p.Akcija != (int)Enums.UserAction.Delete).Select(s => s.Kolicina).Sum();
                    sum = CommonMethods.ParseDecimal(sum);
                }
            }
            return sum;
        }


        protected object GetAvgSummaryTransportPercent()
        {
            object sum = null;

            if (GetRecallDataProvider().GetRecallBuyerFullModel() != null)
            {
                model = GetRecallDataProvider().GetRecallBuyerFullModel();
            }
            if (model != null)
            {
                sum = CommonMethods.ParseDecimal(model.ProcentPrevozaSkupno).ToString("N2");
            }

            //ASPxSummaryItem summaryItem = ASPxGridSelectPositions.TotalSummary.First(i => i.FieldName == "ProcentPrevoza");
            //if (hfCurrentSumPercent.Contains("CurrenSum"))
            //{
            //    decimal sumHidden = CommonMethods.ParseDecimal(hfCurrentSum["CurrenSum"]);
            //    sum = CommonMethods.ParseDecimal(ASPxGridSelectPositions.GetTotalSummaryValue(summaryItem)).ToString("N2");
            //    decimal tSum = CommonMethods.ParseDecimal(sum);
            //    if (tSum > sumHidden)
            //        sum = sumHidden;
            //}
            //else
            //    sum = CommonMethods.ParseDecimal(ASPxGridSelectPositions.GetTotalSummaryValue(summaryItem)).ToString("N2");
            //// = sum;
            return sum;
        }


        private void SetFormDefaultValues()
        {
            ASPxGridLookupPrevoznik.Value = -1;
            ASPxGridLookupRealacija.Value = -1;
            ASPxGridLookupZbirnikTon.Value = -1;


            ASPxSummaryItem summaryItem = ASPxGridSelectPositions.TotalSummary.First(i => i.FieldName == "Kolicina");
            decimal sum = CommonMethods.ParseDecimal(ASPxGridSelectPositions.GetTotalSummaryValue(summaryItem));


            if (GetRecallDataProvider().GetRecallStatuses() != null)
            {
                string status = DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA.ToString();
                string naziv = GetRecallDataProvider().GetRecallStatuses().Where(r => r.Koda == status)
                    .FirstOrDefault().Naziv;
                txtStatus.Text = naziv;
            }

            decimal dCurrentWeightValue = model.KolicinaSkupno;

            ASPxGridLookupRealacija.Value = model.RelacijaID > 0 ? model.RelacijaID : -1;

            // calculate with current recall values, preverimo kaka je celotna vrednost teže in najdemo ter predlagamo pravi zbirnik            
            ASPxGridLookupZbirnikTon.Value = ReturnZbirnikTonIDByOdpoklicValue(CommonMethods.ParseDecimal(dCurrentWeightValue));

            if (ASPxGridLookupRealacija.Value != null && ASPxGridLookupZbirnikTon.Value != null)
            {
                int routeValueID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupRealacija));
                int ZbirnikTonID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupZbirnikTon));

                decimal lowestPrice = (ZbirnikTonID == 0) ? CheckModelValidation(GetDatabaseConnectionInstance().GetLowestAndMostRecentPriceByRouteID(routeValueID)) : CheckModelValidation(GetDatabaseConnectionInstance().GetLowestAndMostRecentPriceByRouteIDandZbirnikTonsID(routeValueID, ZbirnikTonID));
                txtNovaCena.Text = lowestPrice.ToString("N2");
                model.CenaPrevozaSkupno = lowestPrice;
                ASPxGridLookupPrevoznik.DataBind();
                ASPxGridLookupPrevoznik.GridView.Selection.SelectRow(0);
                CalculatePercShip();
                GetRecallDataProvider().SetRecallBuyerFullModel(model);
                ASPxGridSelectPositions.DataBind();
            }
            else
            {
                btnRecall.ClientEnabled = false;
            }
        }

        private void CalculatePercShip()
        {
            if (model == null)
            {
                if (GetRecallDataProvider().GetRecallBuyerFullModel() != null)
                {
                    model = GetRecallDataProvider().GetRecallBuyerFullModel();
                }
            }

            model = CommonMethods.CalculatePercentageShippingCost(model);

            ASPxLabel lblSumPercent = (ASPxLabel)ASPxGridSelectPositions.FindFooterRowTemplateControl("lblSumProcentSkupaj");

            if (lblSumPercent != null) lblSumPercent.Text = CommonMethods.ParseDecimal(model.ProcentPrevozaSkupno).ToString("N2"); ;

            GetRecallDataProvider().SetRecallBuyerFullModel(model);
        }


        private int ReturnZbirnikTonIDByOdpoklicValue(decimal dWeightValue)
        {
            List<ZbirnikTonModel> lZbirnikTon = GetRecallDataProvider().GetZbirnikTon();

            if (lZbirnikTon != null)
            {
                if (lZbirnikTon.Where(zt => zt.TezaOd <= dWeightValue && zt.TezaDo >= dWeightValue).FirstOrDefault() != null)
                    return lZbirnikTon.Where(zt => zt.TezaOd <= dWeightValue && zt.TezaDo >= dWeightValue).FirstOrDefault().ZbirnikTonID;
            }

            return 10;
        }
        #endregion

        #region DataBindings

        protected void ASPxGridViewSelectedPositions_DataBinding(object sender, EventArgs e)
        {
            if (!model.bBrezFakture)
            {
                if (GetRecallDataProvider().GetRecallBuyerFullModel() != null)
                {
                    model = GetRecallDataProvider().GetRecallBuyerFullModel();
                }

                if (model != null)
                {
                    (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
                    (sender as ASPxGridView).DataSource = model.OdpoklicKupecPozicija.Where(p => p.Akcija != (int)Enums.UserAction.Delete).ToList();

                    GetRecallDataProvider().SetRecallTypes(CheckModelValidation(GetDatabaseConnectionInstance().GetRecallTypes()));

                }
            }
        }

        protected void ASPxGridLookupRealacija_DataBinding(object sender, EventArgs e)
        {
            List<RouteModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutes());
            (sender as ASPxGridLookup).DataSource = SerializeToDataTable(list);
        }

        protected void ComboBoxTip_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).DataSource = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallTypes());
        }

        protected void ASPxGridLookupZbirnikTon_DataBinding(object sender, EventArgs e)
        {
            List<ZbirnikTonModel> zbirnikTon = CheckModelValidation(GetDatabaseConnectionInstance().GetAllZbirnikTon());
            GetRecallDataProvider().SetZbirnikTon(zbirnikTon);
            (sender as ASPxGridLookup).DataSource = SerializeToDataTable(zbirnikTon);
        }

        #endregion

        #region Helper methods

        private bool DeleteObject()
        {
            return CheckModelValidation(GetDatabaseConnectionInstance().DeleteBuyerRecall(recallID));
        }

        private void ProcessUserAction()
        {
            bool isValid = false;
            bool isDeleteing = false;

            switch (action)
            {
                case (int)Enums.UserAction.Add:
                    isValid = AddOrEditEntityObject(true);
                    break;
                case (int)Enums.UserAction.Edit:
                    isValid = AddOrEditEntityObject();
                    break;
                case (int)Enums.UserAction.Delete:
                    isValid = DeleteObject();
                    isDeleteing = true;
                    break;
            }

            if (isValid)
            {
                ClearSessionsAndRedirect(isDeleteing);
            }
        }
        private void ClearSessionsAndRedirect(bool isIDDeleted = false, bool saveAndPrintClick = false)
        {
            string redirectString = "";
            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = recallID.ToString() }
            };

            /*else*/
            if (isIDDeleted)
                redirectString = "RecallBuyer.aspx";
            else
                redirectString = GenerateURI("RecallBuyer.aspx", queryStrings);

            if (bIsRejectOrAccept && PrincipalHelper.IsUserLeader())
            {
                redirectString = "RecallBuyer.aspx?filter=3";
            }


            //RemoveSession(Enums.RecallSession.);
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.OrderSession.CientID);
            RemoveSession(Enums.RecallSession.RecallBuyerFulModel);
            RemoveSession(Enums.RecallSession.DisconnectedInvoicesList);

            List<Enums.RecallSession> list = Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList();
            ClearAllSessions(list, redirectString);
        }

        private bool HasSessionModelStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall status)
        {
            if (model == null) return false;

            if (GetRecallDataProvider().GetRecallStatuses() != null)
            {
                string statusOdp = status.ToString();
                int statusID = GetRecallDataProvider().GetRecallStatuses().Where(os => os.Koda == statusOdp).FirstOrDefault().StatusOdpoklicaID;

                return (model.StatusID == statusID);
            }

            return false;
        }



        private decimal GetLatestPrice()
        {

            int routeValueID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupRealacija));
            int ZbirnikTonID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupZbirnikTon));

            decimal lowestPrice = CheckModelValidation(GetDatabaseConnectionInstance().GetLowestAndMostRecentPriceByRouteIDandZbirnikTonsID(routeValueID, ZbirnikTonID));

            return lowestPrice;

        }


        #endregion

        #region Controls Validation based on User roles

        private void EnableUserControls(bool enable = true, bool buyerArrangesTransport = false)
        {
            ASPxGridLookupRealacija.ClientEnabled = enable;
            ASPxGridLookupRealacija.BackColor = enable ? Color.White : Color.LightGray;
            ASPxGridLookupPrevoznik.ClientEnabled = enable;
            ASPxGridLookupPrevoznik.BackColor = enable ? Color.White : Color.LightGray;
        }



        private void SetEnabledAllControls(bool enabled = true)
        {
            ASPxGridSelectPositions.Settings.ShowStatusBar = enabled ? GridViewStatusBarMode.Visible : GridViewStatusBarMode.Hidden;

            ASPxGridLookupRealacija.ClientEnabled = enabled;
            ASPxGridLookupPrevoznik.ClientEnabled = enabled;
            ASPxGridLookupZbirnikTon.ClientEnabled = enabled;
            txtNovaCena.ClientEnabled = enabled;
            btnRecall.ClientEnabled = enabled;
            btnConfirm.ClientEnabled = enabled;
            memOpis.ClientEnabled = enabled;

        }
        #endregion

        #region Value validation

        #endregion

        #region Button events

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (action == (int)Enums.UserAction.Add)
                GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA);
            else
            { //ko shranjujemo odpoklic ko ni več v delovni verziji
                string code = GetRecallDataProvider().GetRecallStatuses().Where(rs => rs.StatusOdpoklicaID == model.StatusID).FirstOrDefault().Koda;
                DatabaseWebService.Common.Enums.Enums.StatusOfRecall enumValue = DatabaseWebService.Common.Enums.Enums.StatusOfRecall.NEZNAN;
                Enum.TryParse(code, out enumValue);
                GetRecallDataProvider().SetRecallStatus(enumValue);
            }
            ProcessUserAction();
        }

        protected void btnRecall_Click(object sender, EventArgs e)
        {
            if (GetRecallDataProvider().GetRecallBuyerFullModel() != null)
                model = GetRecallDataProvider().GetRecallBuyerFullModel();

            model.IzdelajNarocilnico = 1;

            ProcessUserAction();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearSessionsAndRedirect();
        }

        protected void btnConfirmRecall_Click(object sender, EventArgs e)
        {
            recallStatusChanged = true;
            GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POTRJEN);
            ProcessUserAction();
        }

        protected void btnRejectRecall_Click(object sender, EventArgs e)
        {
            recallStatusChanged = true;
            GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.ZAVRNJEN);
            ProcessUserAction();
        }

        protected void btnConfirmTakeOver_Click(object sender, EventArgs e)
        {
            if (model == null) return;

            List<int> selectedPositions = ASPxGridSelectPositions.GetSelectedFieldValues(ASPxGridSelectPositions.KeyFieldName).OfType<int>().ToList();

            ProcessUserAction();
        }

        protected void btnReopenRecall_Click(object sender, EventArgs e)
        {
            reopenRecall = true;
            GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POPRAVLJENO_NAROCILO);
            model.StatusID = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatuses())
                  .Where(rs => rs.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POPRAVLJENO_NAROCILO.ToString())
                  .FirstOrDefault().StatusOdpoklicaID;
            AddOrEditEntityObject();

            List<Enums.RecallSession> list = Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList();
            ClearAllSessions(list, Request.RawUrl);
        }

        protected void btnStorno_Click(object sender, EventArgs e)
        {
            reopenRecall = true;
            GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.STORNO);
            model.StatusID = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatuses())
                  .Where(rs => rs.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.STORNO.ToString())
                  .FirstOrDefault().StatusOdpoklicaID;
            model.IzdelajNarocilnico = 1;

            AddOrEditEntityObject();

            List<Enums.RecallSession> list = Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList();
            ClearSessionsAndRedirect();
        }

        protected void btnSendInquiry_Click(object sender, EventArgs e)
        {
            recallStatusChanged = true;
            GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.RAZPIS_PREVOZNIK);
            ProcessUserAction();
        }

        #endregion

        #region ASPxGridViewSelectedPositions Events


        protected void ASPxGridViewSelectedPositions_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "TipID")
            {
                ASPxComboBox box = e.Editor as ASPxComboBox;
                box.DataSource = GetRecallDataProvider().GetRecallTypes();
                box.ValueField = "TipOdpoklicaID";
                box.ValueType = typeof(Int32);
                box.TextField = "Naziv";
                box.DataBindItems();
            }
        }

        protected void ASPxGridViewSelectedPositions_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            //if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
            //{
            //    if (action != (int)Enums.UserAction.Add && model != null && (model.StatusKoda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POTRJEN.ToString() || model.StatusKoda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELNO_PREVZET.ToString()))
            //    {
            //        ASPxGridView grid = sender as ASPxGridView;
            //        bool prevzeto = Convert.ToBoolean(grid.GetRowValues(e.VisibleIndex, "StatusPrevzeto"));

            //        if (!prevzeto)
            //            e.Visible = true;
            //        else
            //            e.Visible = false;
            //    }
            //    else
            //    {
            //        e.Visible = false;
            //    }
            //}
        }

        #endregion

        protected void CallbackPanelUserInput_Callback(object sender, CallbackEventArgsBase e)
        {
            if (model == null)
            {
                if (GetRecallDataProvider().GetRecallBuyerFullModel() != null)
                {
                    model = GetRecallDataProvider().GetRecallBuyerFullModel();
                }
            }

            model.KolicinaSkupno = CommonMethods.ParseDecimal(GetTotalSummaryValue());

            model.ZbirnikTonID = CommonMethods.ParseInt(ASPxGridLookupZbirnikTon.Value);
            model.RazpisPozicijaID = CommonMethods.ParseInt(ASPxGridLookupPrevoznik.Value);

            model.CenaPrevozaSkupno = (model.CenaPrevozaSkupno > 0 ? model.CenaPrevozaSkupno : CommonMethods.ParseDecimal(GetLatestPrice()));


            model.StevilkaNarocilnica = txtStNarocilnice.Text;
            model.RelacijaID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupRealacija));
            model.RelacijaNaziv = ASPxGridLookupRealacija.Text;
            model.OpisOdpoklicKupec = memOpis.Text;

            if (e.Parameter == "Enable")
            {
                EnableUserControls();
                ASPxGridSelectPositions.DataBind();
            }
            else if (e.Parameter == "SelectRelacija")//Če uporabnik želi dodati novo pozicijo iz naročila
            {

                ASPxGridLookupPrevoznik.DataBind();

                decimal dCenaPrevoza = GetLatestPrice();

                txtNovaCena.Text = dCenaPrevoza.ToString("N2");
                model.CenaPrevozaSkupno = dCenaPrevoza;
                ASPxHFCena["Cena"] = dCenaPrevoza.ToString("N2");

                if (ASPxGridLookupPrevoznik.GridView.VisibleRowCount > 0)
                    ASPxGridLookupPrevoznik.GridView.Selection.SelectRow(0);

                btnRecall.ClientEnabled = true;
                btnRecall.Enabled = true;

                CalculatePercShip();

                CallbackPanelUserInput.JSProperties["cpRefreshGrid"] = true;
                SetZbirnikTonByODpoklicValue();
            }
            else if (e.Parameter == "SelectZbirnikTon")//Če uporabnik želi dodati novo pozicijo iz naročila
            {

                ASPxGridLookupPrevoznik.DataBind();

                decimal dCenaPrevoza = GetLatestPrice();

                txtNovaCena.Text = dCenaPrevoza.ToString("N2");
                model.CenaPrevozaSkupno = dCenaPrevoza;
                ASPxHFCena["Cena"] = dCenaPrevoza.ToString("N2");

                if (ASPxGridLookupPrevoznik.GridView.VisibleRowCount > 0)
                {
                    ASPxGridLookupPrevoznik.GridView.Selection.SelectRow(0);
                }

                btnRecall.ClientEnabled = true;
                btnRecall.Enabled = true;

                CalculatePercShip();

                CallbackPanelUserInput.JSProperties["cpRefreshGrid"] = true;
                //SetZbirnikTonByODpoklicValue();
            }
            else if (e.Parameter == "ChangePrice")//Če uporabnik želi dodati novo pozicijo iz naročila
            {
                model.CenaPrevozaSkupno = CommonMethods.ParseDecimal(txtNovaCena.Text);

                CalculatePercShip();

                CallbackPanelUserInput.JSProperties["cpRefreshGrid"] = true;
                //SetZbirnikTonByODpoklicValue();
            }
            else if (e.Parameter == "SelectPrevoznik")//Če uporabnik želi dodati novo pozicijo iz naročila
            {
                if (ASPxHFCena.Contains("IzbrCena"))
                {
                    model.CenaPrevozaSkupno = CommonMethods.ParseDecimal(ASPxHFCena["IzbrCena"]);
                    CalculatePercShip();
                    CallbackPanelUserInput.JSProperties["cpRefreshGrid"] = true;
                    txtNovaCena.Text = model.CenaPrevozaSkupno.ToString("N2");
                    btnRecall.ClientEnabled = true;
                    btnRecall.Enabled = true;
                    SetZbirnikTonByODpoklicValue();
                }
            }
            else if (e.Parameter == "ShowOrderPositionPopUp")//Če uporabnik želi dodati novo pozicijo iz naročila
            {
                AddValueToSession(Enums.OrderSession.SupplierID, model.PrevoznikNaziv);
                //AddValueToSession(Enums.OrderSession., model.DobaviteljNaziv);
                AddValueToSession(Enums.CommonSession.UserActionPopUp, action);

                model = GetRecallDataProvider().GetRecallBuyerFullModel();
                model.ZbirnikTonID = CommonMethods.ParseInt(ASPxGridLookupZbirnikTon.Value);
                GetRecallDataProvider().SetRecallBuyerFullModel(model);

                ASPxPopupControlOrderPos.ShowOnPageLoad = true;

                SetZbirnikTonByODpoklicValue();
            }
            else if (e.Parameter == "DeleteSelectedPosition")//Če uporabnik želi izbrisati izbrano pozicijo iz odpoklica
            {
                int iTempID = CommonMethods.ParseInt(ASPxGridSelectPositions.GetRowValues(ASPxGridSelectPositions.FocusedRowIndex, "ZaporednaStevilka"));
                if (iTempID > 0)
                {



                    //CheckModelValidation(GetDatabaseConnectionInstance().DeleteRecallPosition(iTempID));
                    var recallPos = model.OdpoklicKupecPozicija.Where(op => op.ZaporednaStevilka == iTempID).FirstOrDefault();
                    //if (recallPos != null)
                    //    model.OdpoklicKupecPozicija.Remove(recallPos);

                    recallPos.Akcija = (int)Enums.UserAction.Delete;
                    SetZbirnikTonByODpoklicValue();
                    CalculatePercShip();
                    ASPxGridSelectPositions.DataBind();
                    CallbackPanelUserInput.JSProperties["cpRefreshGrid"] = true;
                    hfCurrentSum["CurrenSum"] = GetTotalSummaryValue();

                    GetRecallDataProvider().SetRecallBuyerFullModel(model);
                }
            }

            if (model.RazpisPozicijaID > 0)
            {
                btnRecall.ClientEnabled = true;
            }
        }

        private void SetZbirnikTonByODpoklicValue()
        {
            hfCurrentSum["CurrenSum"] = GetTotalSummaryValue();
            decimal dCurrentWeightValue = CommonMethods.ParseDecimal(hfCurrentSum["CurrenSum"]);

            int iSelectedZbirnik = ReturnZbirnikTonIDByOdpoklicValue(CommonMethods.ParseDecimal(dCurrentWeightValue));
            int iCurrentZbirnikID = CommonMethods.ParseInt(ASPxGridLookupZbirnikTon.Value);
            if (iCurrentZbirnikID == iSelectedZbirnik)
            {
                ASPxGridLookupZbirnikTon.Value = iSelectedZbirnik;
                if (model != null)
                    model.ZbirnikTonID = iSelectedZbirnik;
            }
        }

        protected void ASPxPopupControlOrderPos_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.OrderSession.SupplierID);
            RemoveSession(Enums.OrderSession.CientID);
        }

        protected void ASPxGridViewSelectedPositions_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            var obj = e.KeyValue;

            List<object> optimalStockOverflowIds = GetRecallDataProvider().GetRecallPosIDOptimalStockOverflow();

            if (obj != null && optimalStockOverflowIds.Exists(op => (int)op == (int)obj))
            {
                e.Row.BackColor = Color.Tomato;
                ASPxGridSelectPositions.FocusedRowIndex = -1;
            }
        }

        protected void ASPxGridViewSelectedPositions_DataBound(object sender, EventArgs e)
        {
            hfCurrentSum["CurrenSum"] = GetTotalSummaryValue();
        }

        protected void ASPxPopupControlCarriersInquirySummary_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.RecallSession.InquirySummaryRecallID);
        }

        protected void ASPxPopupControlCreateOrder_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.OrderFromRecallSession.ServiceList);
        }
    }
}