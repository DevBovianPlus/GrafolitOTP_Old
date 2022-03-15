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
    public partial class RecallForm : ServerMasterPage
    {
        RecallFullModel model = null;
        int recallID = -1;
        int action = -1;
        public decimal maxKolicina = 0;
        public string criticalTransportType = "15 - SKLADIŠČE MALOPRODAJA";
        public bool reopenRecall = false;
        private bool recallStatusChanged = false;
        private bool bIsRejectOrAccept = false;
        private bool bIsKos = false;

        protected void Page_Init(object sender, EventArgs e)
        {

            if (CheckIfAuthorised(Request.IsAuthenticated)) RedirectHome();

            //if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
            {
                action = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.action.ToString()].ToString());
                recallID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());
            }

            ASPxGridLookupRealacija.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupStranke.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridViewSelectedPositions.Settings.GridLines = GridLines.Both;
            ASPxGridLookupSkladisce.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupTipPrevoza.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupZbirnikTon.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridViewSelectedPositions.SettingsDataSecurity.AllowReadUnlistedFieldsFromClientApi = DevExpress.Utils.DefaultBoolean.True;

            //maxKolicina = GetMaxQuantityForRecall();

            this.Master.DisableNavBar = true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                if (action == (int)Enums.UserAction.Edit || action == (int)Enums.UserAction.Delete)
                {
                    if (recallID > 0)
                    {
                        if (GetRecallDataProvider().GetRecallFullModel() != null)
                            model = GetRecallDataProvider().GetRecallFullModel();
                        else
                        {
                            model = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallByID(recallID));
                        }

                        if (model != null)
                        {
                            CommonMethods.LogThis("Page_Load.Dobavitelj 1 :" + model.DobaviteljNaziv == null ? "" : model.DobaviteljNaziv);

                            GetRecallDataProvider().SetRecallFullModel(model);
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
                if (model == null && SessionHasValue(Enums.RecallSession.RecallFulModel))
                    model = GetRecallDataProvider().GetRecallFullModel();
            }
        }

        private void FillForm()
        {
            ASPxGridLookupRealacija.Value = model.RelacijaID > 0 ? model.RelacijaID : -1;
            txtNovaCena.Text = model.CenaPrevoza.ToString("N3");
            bool disableRecallBtnBasedOnTransporter = false;

            if (model.Dobavitelj != null || model.RazpisPozicijaID > 0)
                ASPxGridLookupStranke.Value = model.RazpisPozicijaID;//dobvitelja/prevoznika preberemo iz razpisa pozicije
            else
            {
                disableRecallBtnBasedOnTransporter = true;
                ASPxGridLookupStranke.Text = model.Prevozniki;
            }


            //ComboBoxTip.SelectedIndex = model.TipID > 0 ? ComboBoxTip.Items.IndexOfValue(model.TipID.ToString()) : 0;
            txtStatus.Text = model.StatusOdpoklica != null ? model.StatusOdpoklica.Naziv : "";

            if ((model.StatusOdpoklica.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.USTVARJENO_NAROCILO.ToString()) && (model.P_TransportOrderPDFName != null))
            {
                txtStatus.Text += " ŠT: " + model.P_TransportOrderPDFName;
            }

            model.KolicinaSkupno = CommonMethods.ParseDecimal(GetTotalSummaryValue());

            hfCurrentSum["CurrenSum"] = model.KolicinaSkupno;
            hfCurrentSumPalete["CurrenSumPalete"] = model.PaleteSkupaj;



            ASPxMemoOpombe.Text = model.Opis;
            txtSofer.Text = model.SoferNaziv;
            txtRegistracija.Text = model.Registracija;
            memOpombaPrevoznikov.Text = model.OpombaZaPovprasevnjePrevoznikom;
            DateEditDatumNaklada.Date = model.DatumNaklada.Value;
            DateEditDatumRazklada.Date = model.DatumRazklada.Value > DateTime.MinValue ? model.DatumRazklada.Value : DateTime.MinValue;

            SupplierArrangesTransportCheckBox2.Checked = model.DobaviteljUrediTransport;
            CheckBoxLastenPrevoz.Checked = model.LastenPrevoz;

            if (!String.IsNullOrEmpty(model.OdobritevKomentar))
            {
                memoKomentar.ClientVisible = true;
                memoKomentar.Text = model.OdobritevKomentar;
            }

            //če status ni delovna verzija ptoem blokiramo shranjvanje odpoklica
            if (model.StatusOdpoklica != null && model.StatusOdpoklica.Koda != DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA.ToString() &&
                action != (int)Enums.UserAction.Delete)
            {
                //btnConfirm.ClientEnabled = false;
                btnRecall.ClientEnabled = false;
            }

            //če status delovna verzija odblokiramo shranjvanje odpoklica
            if (model.StatusOdpoklica != null && model.StatusOdpoklica.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA.ToString() &&
                action != (int)Enums.UserAction.Delete)
            {
                //btnConfirm.ClientEnabled = false;
                btnRecall.ClientEnabled = true;
            }



            FuncionalityBasedOnUserRole();

            if (model.StatusOdpoklica != null && (model.StatusOdpoklica.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA.ToString()))
                SetEnabledAllControls();
            else
                SetEnabledAllControls(false);

            //Id supplier Arranges transport
            if (model.DobaviteljUrediTransport || model.KupecUrediTransport)
            {
                EnableUserControls(false, model.KupecUrediTransport);
                ASPxGridLookupStranke.Value = null;
                txtNovaCena.Text = "";
                if (model.StatusOdpoklica != null && (model.StatusOdpoklica.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA.ToString()))
                {
                    ASPxGridLookupRealacija.ClientEnabled = true;
                    ASPxGridLookupRealacija.BackColor = Color.White;
                }
            }

            if ((model != null && model.StatusOdpoklica.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.V_ODOBRITEV.ToString()) ||
                (model != null && model.StatusOdpoklica.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POTRJEN.ToString()))
                btnReopenRecall.ClientVisible = true;

            txtRazlogOdobritveSistem.Text = model.RazlogOdobritveSistem;

            ASPxGridLookupTipPrevoza.Value = model.TipPrevozaID > 0 ? model.TipPrevozaID : -1;
            ASPxGridLookupZbirnikTon.Value = model.ZbrirnikTonID > 0 ? model.ZbrirnikTonID : -1;

            ASPxGridLookupSkladisce.Value = model.LastnoSkladisceID > 0 ? model.LastnoSkladisceID : -1;

            // nastavimo zbirnik ton
            SetZbirnikTonByODpoklicValue();

            if (model.ZbrirnikTonID != null && model.StatusOdpoklica.Koda != DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA.ToString())
            {
                ASPxGridLookupZbirnikTon.Value = model.ZbrirnikTonID;
            }

            //Ko odpremo odpoklic je potrebno prevertiti če je dobavitelj Skladišče. Če je res potem je potrebno poiskati ID dobavitelja (tabela Stranke_OTP)
            ClientFullModel clientSupplier = CheckModelValidation(GetDatabaseConnectionInstance().GetClientByName(model.DobaviteljNaziv));
            if (clientSupplier != null) AddValueToSession(Enums.OrderSession.CientID, clientSupplier.idStranka);

            txtStOdpoklic.Text = model.OdpoklicStevilka.ToString();

            if ((model.DobaviteljNaziv == null) || (model.DobaviteljNaziv.Length == 0))
            {
                ShowClientWarningPopUp("DOBAVITELJ ni izbran, NAPAKA - Odpoklic ni mogoč ");
            }
            txtDobaviteljNaziv.Text = model.DobaviteljNaziv;

            if (disableRecallBtnBasedOnTransporter && (!model.DobaviteljUrediTransport && !model.KupecUrediTransport))
                btnRecall.ClientEnabled = false;

            if (disableRecallBtnBasedOnTransporter && ASPxGridLookupStranke.Text.Contains(";"))
                btnSendInquiry.ClientVisible = true;

            BuyerArrangesTransportCheckBox2.Checked = model.KupecUrediTransport;



            //Iz odpoklica omogočimo da lahko uporabnik samo enkrat pošlje povpraševanje. Drugače ga lahko ureja na Recall.aspx strani kot popup.
            if (model.PovprasevanjePoslanoPrevoznikom)
            {
                btnOpenInquirySummaryForCarrier.ClientVisible = true;
                btnSendInquiry.ClientVisible = false;
            }

            if (model.StatusOdpoklica != null && (model.StatusOdpoklica.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POTRJEN.ToString()) && (!model.DobaviteljUrediTransport && !model.KupecUrediTransport))
            {
                btnCreateOrder.ClientVisible = true;
            }

            if (PrincipalHelper.IsUserLogistics())
            {
                ASPxGridLookupStranke.ClientEnabled = true;
                txtNovaCena.ClientEnabled = true;
            }

            if (model.StatusOdpoklica.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.PREVZET.ToString() && (PrincipalHelper.IsUserCarrierSpecialist() || PrincipalHelper.IsUserSuperAdmin()))
            {
                btnReopenRecall.ClientVisible = true;
                ASPxGridLookupStranke.ClientEnabled = true;
                txtNovaCena.ClientEnabled = true;
            }

        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            CommonMethods.LogThis("recallStatusChanged :" + recallStatusChanged.ToString());

            if (add)
            {
                model = GetRecallDataProvider().GetRecallFullModel() != null ? GetRecallDataProvider().GetRecallFullModel() : new RecallFullModel();

                model.OdpoklicID = 0;
                model.ts = DateTime.Now;
                model.tsIDOseba = PrincipalHelper.GetUserPrincipal().ID;
                model.UserID = PrincipalHelper.GetUserPrincipal().ID;

                if (model.OdpoklicPozicija != null)//pri dodajanju novega odpoklica imamo v začetku nastavljene začasne id-je. Zato jih je potrebno ob shranjevanju nastavit na 0, da dobijo sql-ove id-je
                    model.OdpoklicPozicija.ForEach(poz => poz.OdpoklicPozicijaID = 0);

            }
            else if (model == null && !add)
            {
                model = GetRecallDataProvider().GetRecallFullModel();
            }

            if (model != null)
            {
                CommonMethods.LogThis("0-model.Dobavitelj :" + model.DobaviteljNaziv == null ? "" : model.DobaviteljNaziv);
            }
            else
            {
                CommonMethods.LogThis("recall je NULL :");
            }

            model.RelacijaID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupRealacija));

            if (SupplierArrangesTransportCheckBox2.Checked || BuyerArrangesTransportCheckBox2.Checked)
            {
                model.RazpisPozicijaID = 0;
                model.DobaviteljID = 0;
                model.Prevozniki = "";
            }
            else
            {
                //če je izbrano več prevoznikov potem ne dovolimo izpeljati odpoklica do konca.
                int len = ASPxGridLookupStranke.Text.Split(';').Length;
                if (len > 1)
                {
                    model.Prevozniki = ASPxGridLookupStranke.Text;
                    model.DobaviteljID = 0;
                    model.RazpisPozicijaID = 0;
                }
                else
                {
                    int tenderPosID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupStranke));
                    if (tenderPosID > 0)
                    {
                        model.DobaviteljID = GetRecallDataProvider().GetTenderListFromSelectedRoute().Where(t => t.RazpisPozicijaID == tenderPosID).FirstOrDefault().StrankaID;
                        model.RazpisPozicijaID = tenderPosID;
                    }
                }
            }
            //model.TipID = CommonMethods.ParseInt(ComboBoxTip.Value.ToString());



            string recallStatusCode = GetRecallDataProvider().GetRecallStatus().ToString();
            model.StatusID = GetRecallDataProvider().GetRecallStatuses() != null ? GetRecallDataProvider().GetRecallStatuses().Where(rs => rs.Koda == recallStatusCode).FirstOrDefault().StatusOdpoklicaID : 0;

            // preverimo ali je status še vedno 0
            if (model.StatusID == 0)
            {
                // get from database
                RecallStatus st = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatusByCode(recallStatusCode));
                if (st != null)
                {
                    model.StatusID = st.StatusOdpoklicaID;
                }
            }

            CommonMethods.LogThis("recallStatusCode :" + recallStatusCode.ToString());
            CommonMethods.LogThis(" model.StatusID :" + model.StatusID.ToString());

            model.CenaPrevoza = CommonMethods.ParseDecimal(txtNovaCena.Text);
            model.KolicinaSkupno = CommonMethods.ParseDecimal(GetTotalSummaryValue());
            model.PaleteSkupaj = CommonMethods.ParseDecimal(hfCurrentSumPalete["CurrenSumPalete"]);
            model.OdobritevKomentar = memoKomentar.Text;
            model.Opis = ASPxMemoOpombe.Text;
            model.SoferNaziv = txtSofer.Text;
            model.Registracija = txtRegistracija.Text;
            model.OpombaZaPovprasevnjePrevoznikom = memOpombaPrevoznikov.Text;

            model.DatumNaklada = (DateEditDatumNaklada.Date > DateTime.MinValue) ? DateEditDatumNaklada.Date : (DateTime?)null;
            model.DatumRazklada = (DateEditDatumRazklada.Date > DateTime.MinValue) ? DateEditDatumRazklada.Date : (DateTime?)null;
            model.DobaviteljUrediTransport = SupplierArrangesTransportCheckBox2.Checked;
            model.LastenPrevoz = CheckBoxLastenPrevoz.Checked;

            model.TipPrevozaID = CommonMethods.ParseInt(ASPxGridLookupTipPrevoza.Value);
            model.ZbrirnikTonID = CommonMethods.ParseInt(ASPxGridLookupZbirnikTon.Value);

            CommonMethods.LogThis("1-model.TipPrevozaID :" + CommonMethods.Parse(model.TipPrevozaID.ToString()));
            CommonMethods.LogThis("1-model.ZbrirnikTonID :" + CommonMethods.ParseInt(model.ZbrirnikTonID.ToString()));

            CommonMethods.LogThis("1-model.Dobavitelj :" + model.DobaviteljNaziv == null ? "" : model.DobaviteljNaziv);



            if (model.DobaviteljNaziv == null || model.DobaviteljNaziv.Length == 0)
            {
                CommonMethods.LogThis("1 - dobavitelj ni izbran");
                if (GetRecallDataProvider().GetSelectSupplier() != null)
                {
                    CommonMethods.LogThis("2 - dobavitelj se potegne iz RAMA");
                    SupplierModel suppmodel = GetRecallDataProvider().GetSelectSupplier();

                    model.DobaviteljNaziv = suppmodel.Dobavitelj.Trim();
                    model.DobaviteljNaslov = suppmodel.Naslov.Trim();
                    model.DobaviteljPosta = suppmodel.Posta.Trim();
                    model.DobaviteljKraj = suppmodel.Kraj.Trim();
                }
                else
                {
                    CommonMethods.LogThis("3 - dobavitelja ni v RAM-u");
                    if (GetRecallDataProvider().GetSelectSupplierValue() != null)
                    {
                        CommonMethods.LogThis("4 - Samo vrednost : " + GetRecallDataProvider().GetSelectSupplierValue());
                    }
                    else
                    {
                        CommonMethods.LogThis("5 - Ni tudi te vrednosti GetSelectSupplierValue()");
                    }
                }
            }
            else
            {
                CommonMethods.LogThis("4 - dobavitelja ni prazen");
            }



            if (SessionHasValue(Enums.RecallSession.ArgumentsOfApprovalToDB) && GetValueFromSession(Enums.RecallSession.ArgumentsOfApprovalToDB).ToString().Length > 0)
            {
                model.RazlogOdobritveSistem = GetValueFromSession(Enums.RecallSession.ArgumentsOfApprovalToDB).ToString();
                RemoveSession(Enums.RecallSession.ArgumentsOfApprovalToDB);
            }

            model.LastnoSkladisceID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupSkladisce));

            //če smo razdelili kakšno pozicijo je potrebno novim pozicijam nastaviti OdpoklicPozicijaID na 0
            if (model.OdpoklicPozicija != null)
            {
                model.OdpoklicPozicija.Where(op => op.childSplit).ToList().ForEach(op => op.OdpoklicPozicijaID = 0);
            }
            model.RecallStatusChanged = recallStatusChanged;

            //ob pošiljanju razpisov cen prevoznikom moramo nastaviti polje na true da bomo vedeli katere odpoklice iskati
            if (model.RecallStatusChanged && recallStatusCode == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.RAZPIS_PREVOZNIK.ToString())
                model.PovprasevanjePoslanoPrevoznikom = true;

            // preverimo, če je odpoklic zavrnjen ali potrjen, zaradi filtra
            if (recallStatusCode == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POTRJEN.ToString() || recallStatusCode == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.ZAVRNJEN.ToString())
                bIsRejectOrAccept = true;

            model.KupecUrediTransport = BuyerArrangesTransportCheckBox2.Checked;

            if (model.RazlogOdobritveSistem == null) model.RazlogOdobritveSistem = "";

            if (model.RazlogOdobritveSistem.Length == 0 && model.StatusID == Convert.ToInt32(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.V_ODOBRITEV))
            {
                CommonMethods.LogThis("NAPAKA V odobritev brez razloga :" + model.OdpoklicStevilka.ToString() + ": model.RazlogOdobritveSistem :" + model.RazlogOdobritveSistem);
                model.StatusID = 4;
            }

            CommonMethods.LogThis("RazlogOdobritveSistem :" + model.RazlogOdobritveSistem);
            CommonMethods.LogThis("recallStatusCode :" + recallStatusCode);
            CommonMethods.LogThis("StatusID :" + model.StatusID.ToString());

            CommonMethods.LogThis("2-model.TipPrevozaID :" + model.TipPrevozaID.ToString());
            CommonMethods.LogThis("2-model.ZbrirnikTonID :" + model.ZbrirnikTonID.ToString());
            CommonMethods.LogThis("2-model.DobaviteljID :" + model.DobaviteljID == null ? "" : model.DobaviteljID.ToString());
            CommonMethods.LogThis("2-model.Dobavitelj :" + model.DobaviteljNaziv == null ? "" : model.DobaviteljNaziv);

            RecallFullModel returnModel = CheckModelValidation(GetDatabaseConnectionInstance().SaveRecall(model));

            RemoveSession(Enums.RecallSession.RecallStatus);




            if (returnModel != null)
            {
                //this we need if we want to add new client and then go and add new Plan with no redirection to Clients page
                model = returnModel;//if we need updated model in the same request;
                GetRecallDataProvider().SetRecallFullModel(returnModel);

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
            if ((model != null && action == (int)Enums.UserAction.Add) || (model != null && model.StatusOdpoklica.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.V_ODOBRITEV.ToString()))
            {
                List<MaterialModel> materials = GetMaterialValues();
                string errorMessage = "";
                //odpoklic pozicij preverjamo smo če imajo transport tipa 15
                if (CheckForOptimalStockOverflowFromPrevRecalls(model.OdpoklicPozicija, materials, out errorMessage))
                    ShowClientWarningPopUp(errorMessage);
                else if (CheckForOptimalStockOverflow(model.OdpoklicPozicija, false))
                    ShowClientWarningPopUp("Odpoklicana količina presega optimalno!");
                else if (!isTransportType15Valid(model.OdpoklicPozicija))
                    ShowClientWarningPopUp("Odpoklicana količina presega optimalno zaradi tipa transporta (15) ");

                if ((model.DobaviteljNaziv == null) || (model.DobaviteljNaziv.Length == 0))
                {
                    ShowClientWarningPopUp("DOBAVITELJ ni izbran, NAPAKA - Odpoklic ni mogoč ");
                }
                txtDobaviteljNaziv.Text = model.DobaviteljNaziv;
            }

            ASPxGridViewSelectedPositions.DataBind();
            ASPxGridLookupRealacija.DataBind();

            if (action != (int)Enums.UserAction.Add)
                ASPxGridLookupStranke.DataBind();

            ASPxGridLookupTipPrevoza.DataBind();
            ASPxGridLookupZbirnikTon.DataBind();
            ASPxGridLookupSkladisce.DataBind();
            GetRecallDataProvider().SetRecallStatuses(CheckModelValidation(GetDatabaseConnectionInstance().GetRecallStatuses()));
        }

        private void PopulateModel()
        {
            if (GetRecallDataProvider().GetRecallFullModel() != null)
            {
                model = GetRecallDataProvider().GetRecallFullModel();
            }
            else if (recallID > 0)
            {
                model = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallByID(recallID));
            }
        }

        protected object GetTotalSummaryValue()
        {
            object sum = null;
            ASPxSummaryItem summaryItem = ASPxGridViewSelectedPositions.TotalSummary.First(i => i.FieldName == "Kolicina");
            if (hfCurrentSum.Contains("CurrenSum"))
            {
                decimal sumHidden = CommonMethods.ParseDecimal(hfCurrentSum["CurrenSum"]);
                sum = CommonMethods.ParseDecimal(ASPxGridViewSelectedPositions.GetTotalSummaryValue(summaryItem)).ToString("N2");
                decimal tSum = CommonMethods.ParseDecimal(sum);
                if (tSum > sumHidden)
                    sum = sumHidden;
            }
            else
                sum = CommonMethods.ParseDecimal(ASPxGridViewSelectedPositions.GetTotalSummaryValue(summaryItem)).ToString("N2");
            // = sum;
            return sum;
        }




        protected object GetTotalPaleteSummaryValue()
        {
            ASPxSummaryItem summaryItem = ASPxGridViewSelectedPositions.TotalSummary.First(i => i.FieldName == "Palete");
            object sum = CommonMethods.ParseDecimal(ASPxGridViewSelectedPositions.GetTotalSummaryValue(summaryItem)).ToString("N2");
            hfCurrentSumPalete["CurrenSumPalete"] = sum;
            return sum;
        }

        private void SetFormDefaultValues()
        {
            ASPxGridLookupStranke.Value = -1;
            ASPxGridLookupRealacija.Value = -1;
            ASPxGridLookupZbirnikTon.Value = -1;

            string truck = DatabaseWebService.Common.Enums.Enums.TransportType.KAMION.ToString();
            ASPxGridLookupTipPrevoza.Value = GetRecallDataProvider().GetTransportTypes().Where(tt => tt.Koda == truck).FirstOrDefault().TipPrevozaID;



            ASPxGridLookupSkladisce.Value = -1;

            ASPxSummaryItem summaryItem = ASPxGridViewSelectedPositions.TotalSummary.First(i => i.FieldName == "Kolicina");
            decimal sum = CommonMethods.ParseDecimal(ASPxGridViewSelectedPositions.GetTotalSummaryValue(summaryItem));

            if (sum <= GetMaxQuantityForRecall())
                EnableUserControls();
            else if (bIsKos)
            {
                EnableUserControls();
            }
            else
            {
                EnableUserControls(false);
            }


            if (GetRecallDataProvider().GetRecallStatuses() != null)
            {
                string status = DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA.ToString();
                string naziv = GetRecallDataProvider().GetRecallStatuses().Where(r => r.Koda == status)
                    .FirstOrDefault().Naziv;
                txtStatus.Text = naziv;
            }

            decimal dCurrentWeightValue = CommonMethods.ParseDecimal(hfCurrentSum["CurrenSum"]);
            hfCurrentSum["CurrenSum"] = GetTotalSummaryValue();

            // calculate with current recall values, preverimo kaka je celotna vrednost teže in najdemo ter predlagamo pravi zbirnik
            //ASPxGridLookupZbirnikTon.Value = GetRecallDataProvider().GetZbirnikTon().Where(tt => tt.Koda == truck).FirstOrDefault().ZbirnikTonID;

            if (GetRecallDataProvider().GetRecallFullModel() != null)
                model = GetRecallDataProvider().GetRecallFullModel();

            bIsKos = CheckIfKosPosition();
            if (!bIsKos)
            {
                ASPxGridLookupZbirnikTon.Value = ReturnZbirnikTonIDByOdpoklicValue(CommonMethods.ParseDecimal(dCurrentWeightValue));
            }
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

        private bool CheckIfKosPosition()
        {
            bool bRet = false;
            string kos = Enums.UnitsFromOrder.KOS.ToString();

            if (model != null)
            {
                bRet = model.OdpoklicPozicija.Exists(m => m.EnotaMere == kos);
            }
            return bRet;
        }
        #endregion

        #region DataBindings

        protected void ASPxGridViewSelectedPositions_DataBinding(object sender, EventArgs e)
        {
            if (model != null)
            {
                (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
                (sender as ASPxGridView).DataSource = model.OdpoklicPozicija != null ? model.OdpoklicPozicija : new List<RecallPositionModel>();
                string kos = Enums.UnitsFromOrder.KOS.ToString();
                bIsKos = false;
                hfIsKos["IsKos"] = false.ToString();
                if (model.OdpoklicPozicija.Exists(m => m.EnotaMere == kos))
                {
                    bIsKos = true;
                    hfIsKos["IsKos"] = true.ToString();
                    var column = (GridViewDataTextColumn)ASPxGridViewSelectedPositions.Columns["TransportnaKolicina"];
                    column.PropertiesTextEdit.ValidationSettings.RequiredField.IsRequired = true;
                    column.PropertiesTextEdit.ValidationSettings.RegularExpression.ValidationExpression = "^[1-9]\\d*$";
                    column.PropertiesTextEdit.ValidationSettings.RegularExpression.ErrorText = "Vpiši količino, ki je večja od 0";
                }

                GetRecallDataProvider().SetRecallTypes(CheckModelValidation(GetDatabaseConnectionInstance().GetRecallTypes()));

            }
        }
        protected void ASPxGridLookupStranke_DataBinding(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            object id = null;

            object id2 = null;

            if (ASPxGridLookupRealacija.Value != null)
                id = ASPxGridLookupRealacija.Value;
            else if (model != null && model.RelacijaID > 0)
                id = model.RelacijaID;
            if (ASPxGridLookupZbirnikTon.Value != null)
            {
                id2 = ASPxGridLookupZbirnikTon.Value;
            }
            id2 = id2 == null ? model.ZbrirnikTonID : id2;
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
                        ASPxGridLookupStranke.Value = -1;
                }
            }
            //else
            //  ClearSessionsAndRedirect();
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

        protected void ASPxGridLookupStranke_DataBound(object sender, EventArgs e)
        {
            if (action != (int)Enums.UserAction.Add)
                ASPxGridLookupStranke.Value = model.DobaviteljID > 0 ? model.DobaviteljID : -1;
        }

        protected void ASPxGridLookupSkladisce_DataBinding(object sender, EventArgs e)
        {
            string codeType = DatabaseWebService.Common.Enums.Enums.TypeOfClient.SKLADISCE.ToString();
            List<ClientSimpleModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetAllClients(codeType));
            (sender as ASPxGridLookup).DataSource = SerializeToDataTable(list, "idStranka", "NazivPrvi");
        }

        protected void ASPxGridLookupTipPrevoza_DataBinding(object sender, EventArgs e)
        {
            List<ClientTransportType> types = CheckModelValidation(GetDatabaseConnectionInstance().GetAllTransportTypes());
            GetRecallDataProvider().SetTransportTypes(types);
            (sender as ASPxGridLookup).DataSource = SerializeToDataTable(types, "TipPrevozaID", "Naziv");
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
            return CheckModelValidation(GetDatabaseConnectionInstance().DeleteRecall(recallID));
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
                redirectString = "Recall.aspx";
            else
                redirectString = GenerateURI("Recall.aspx", queryStrings);

            if (bIsRejectOrAccept && PrincipalHelper.IsUserLeader())
            {
                redirectString = "Recall.aspx?filter=3";
            }


            //RemoveSession(Enums.RecallSession.);
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.OrderSession.CientID);

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

        private List<MaterialModel> GetMaterialValues()
        {
            if (model == null) return null;

            List<MaterialModel> materials = new List<MaterialModel>();
            foreach (var item in model.OdpoklicPozicija)
            {
                materials.Add(new MaterialModel { Ident = item.MaterialIdent });
            }
            materials = CheckModelValidation(GetDatabaseConnectionInstance().GetLatestQuantityForProduct(materials));

            return materials;
        }

        private bool isNewPriceHigherFromTender()
        {
            //če je obkljukan dobavitelj priskrbi prevoz ali kupec priskrbi prevoz potem ne smemo preverjati cene
            if (SupplierArrangesTransportCheckBox2.Checked || BuyerArrangesTransportCheckBox2.Checked) return false;

            if (!GetRecallDataProvider().HasTenderListValues()) return true;

            decimal newPrice = CommonMethods.ParseDecimal(txtNovaCena.Text);

            decimal latestPrice = GetLatestPrice();
            if (latestPrice == -1)
            {//Če vrne funkcija -1 potem vemo da prevoznik ni izbran
                return false;
            }
            else if (newPrice > latestPrice)
            {
                return true;
            }

            return false;
        }

        private decimal GetLatestPrice()
        {//Nova zahteva (3.9.2019): Ne pridobivamo več zadnje cene iz razpisa za izbranega prevoznika, ampak najnižjo ceno za izbrano relacijo ne glede na prevoznika

            /*int tenderPosID = CommonMethods.ParseInt(ASPxGridLookupStranke.Value);

            if (tenderPosID == 0) return -1;

            List<TenderPositionModel> tenderList = GetRecallDataProvider().GetTenderListFromSelectedRoute();

            TenderPositionModel selectedTenderPos = null;

            if (tenderList != null)
                selectedTenderPos = tenderList.Where(t => t.RazpisPozicijaID == tenderPosID).FirstOrDefault();*/

            int routeValueID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupRealacija));
            int ZbirnikTonID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupZbirnikTon));

            decimal lowestPrice = CheckModelValidation(GetDatabaseConnectionInstance().GetLowestAndMostRecentPriceByRouteIDandZbirnikTonsID(routeValueID, ZbirnikTonID));

            return lowestPrice;

            //return selectedTenderPos != null ? selectedTenderPos.Cena : 0;
        }

        private decimal GetMaxQuantityForRecall()
        {
            return CommonMethods.ParseDecimal(ASPxGridLookupTipPrevoza.GridView.GetRowValues(ASPxGridLookupTipPrevoza.GridView.FocusedRowIndex, "DovoljenaTeza"));
        }
        private string GetSelecctedTransportTypeCode()
        {
            if (ASPxGridLookupTipPrevoza.GridView.FocusedRowIndex < 0) return "";

            return ASPxGridLookupTipPrevoza.GridView.GetRowValues(ASPxGridLookupTipPrevoza.GridView.FocusedRowIndex, "Koda").ToString();
        }

        #endregion

        #region Controls Validation based on User roles

        private void EnableUserControls(bool enable = true, bool buyerArrangesTransport = false)
        {
            ASPxGridLookupRealacija.ClientEnabled = enable;
            ASPxGridLookupRealacija.BackColor = enable ? Color.White : Color.LightGray;
            ASPxGridLookupStranke.ClientEnabled = enable;
            ASPxGridLookupStranke.BackColor = enable ? Color.White : Color.LightGray;
            memOpombaPrevoznikov.ClientEnabled = enable;
            memOpombaPrevoznikov.BackColor = enable ? Color.White : Color.LightGray;
            txtNovaCena.ClientEnabled = enable;
            txtNovaCena.BackColor = enable ? Color.White : Color.LightGray;
            //btnRecall.ClientEnabled = enable;
            txtSofer.ClientEnabled = enable;
            txtSofer.BackColor = enable ? Color.White : Color.LightGray;

            if (!buyerArrangesTransport)
            {
                txtRegistracija.ClientEnabled = enable;
                txtRegistracija.BackColor = enable ? Color.White : Color.LightGray;
                //BuyerArrangesTransportCheckBox2.ClientEnabled = enable;
            }
            //else
            // SupplierArrangesTransportCheckBox2.ClientEnabled = enable;

            //ASPxMemoOpombe.ClientEnabled = enable;
            //ASPxMemoOpombe.BackColor = enable ? Color.White : Color.LightGray;

            string transportTypeCode = model != null ? (model.TipPrevoza != null ? model.TipPrevoza.Koda : GetSelecctedTransportTypeCode()) : GetSelecctedTransportTypeCode();

            if (transportTypeCode == DatabaseWebService.Common.Enums.Enums.TransportType.LADJA.ToString() || transportTypeCode == DatabaseWebService.Common.Enums.Enums.TransportType.LETALO.ToString())
                ASPxGridLookupSkladisce.ClientEnabled = true;
            else
                ASPxGridLookupSkladisce.ClientEnabled = false;
        }

        private void FuncionalityBasedOnUserRole()
        {
            bool visible = false;
            bool isRecallVOdobritvi = (model.StatusOdpoklica.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.V_ODOBRITEV.ToString());

            if (PrincipalHelper.IsUserAdmin())
            {
                if (isRecallVOdobritvi)
                    visible = true;
            }
            else if (PrincipalHelper.IsUserLeader())
            {
                if (isRecallVOdobritvi)
                    visible = true;
            }
            else if (PrincipalHelper.IsUserWarehouseKeeper())
            {
                visible = false;
            }
            else
            {
                if (isRecallVOdobritvi)
                    visible = true;
            }

            btnConfirmRecall.ClientVisible = visible;
            btnRejectRecall.ClientVisible = visible;
            lblRazlogOdobritveSistem.ClientVisible = visible;
            txtRazlogOdobritveSistem.ClientVisible = visible;
        }

        private void SetEnabledAllControls(bool enabled = true)
        {
            ASPxGridViewSelectedPositions.Settings.ShowStatusBar = enabled ? GridViewStatusBarMode.Visible : GridViewStatusBarMode.Hidden;
            ASPxGridLookupRealacija.ClientEnabled = enabled;
            ASPxGridLookupStranke.ClientEnabled = enabled;
            memOpombaPrevoznikov.ClientEnabled = enabled;
            txtNovaCena.ClientEnabled = enabled;
            SupplierArrangesTransportCheckBox2.ClientEnabled = enabled;
            BuyerArrangesTransportCheckBox2.ClientEnabled = enabled;
            CheckBoxLastenPrevoz.ClientEnabled = enabled;
            ASPxGridLookupTipPrevoza.ClientEnabled = enabled;
            ASPxGridLookupZbirnikTon.ClientEnabled = enabled;
            ASPxGridLookupSkladisce.ClientEnabled = enabled;
            //txtRegistracija.ClientEnabled = enabled;
            //txtSofer.ClientEnabled = enabled;
            //ASPxMemoOpombe.ClientEnabled = enabled;
            memoKomentar.ClientEnabled = enabled;
        }
        #endregion

        #region Value validation

        private bool isTransportType15Valid(object recallPositionID, object newQuantity)
        {
            object value = ASPxGridViewSelectedPositions.GetRowValuesByKeyValue(recallPositionID, "TipNaziv", "OptimalnaZaloga", "TrenutnaZaloga", "Kolicina", "EnotaMere");
            object[] obj = (object[])value;

            if (newQuantity == null)
                newQuantity = obj[3];

            string transportType = obj[0] != null ? obj[0].ToString() : "";

            if (CommonMethods.Trim(transportType) == criticalTransportType && CommonMethods.Trim(obj[4].ToString()) == Enums.UnitsFromOrder.KG.ToString())
            {
                if ((CommonMethods.ParseDecimal(newQuantity) + CommonMethods.ParseDecimal(obj[2])) > CommonMethods.ParseDecimal(obj[1]))
                    return false;
            }

            return true;
        }

        private bool isTransportType15Valid(List<RecallPositionModel> updatedValues)
        {

            if (BuyerArrangesTransportCheckBox2.Checked) return true;

            foreach (var item in updatedValues)
            {
                if (CommonMethods.Trim(item.TipNaziv) == criticalTransportType && item.EnotaMere == Enums.UnitsFromOrder.KG.ToString())
                {
                    if ((CommonMethods.ParseDecimal(item.Kolicina) + CommonMethods.ParseDecimal(item.TrenutnaZaloga) > CommonMethods.ParseDecimal(item.OptimalnaZaloga)))
                        return false;
                }
            }

            return true;
        }

        private List<RecallPositionModel> SelectOnlyRecallPosWithTransportType15(List<RecallPositionModel> recallPos)
        {
            List<RecallPositionModel> model = new List<RecallPositionModel>();

            foreach (var item in recallPos)
            {
                if (CommonMethods.Trim(item.TipNaziv) == criticalTransportType && CommonMethods.Trim(item.EnotaMere) == Enums.UnitsFromOrder.KG.ToString())
                {
                    model.Add(item);
                }
            }

            return model;
        }

        private bool CheckForOptimalStockOverflow(List<RecallPositionModel> recallPos, bool setCallbackResult = true)
        {
            if (BuyerArrangesTransportCheckBox2.Checked) return false;

            recallPos = SelectOnlyRecallPosWithTransportType15(recallPos);

            List<object> optimalStockOverflowIds = GetRecallDataProvider().GetRecallPosIDOptimalStockOverflow();

            var groupedRecallPos = recallPos.GroupBy(r => r.MaterialIdent).Select(rs => new
            {
                OdpoklicPozicijaID = rs.Select(sr => sr.OdpoklicPozicijaID).FirstOrDefault(),
                OdkpoklicVsotaKolicina = rs.Sum(sr => sr.Kolicina),
                OptimalnaZaloga = rs.Select(sr => sr.OptimalnaZaloga).FirstOrDefault(),
                TrenutnaZaloga = rs.Select(sr => sr.TrenutnaZaloga).FirstOrDefault(),
                TipTransportaNaziv = rs.Select(sr => sr.TipNaziv).FirstOrDefault(),
                EnotaMere = rs.Select(sr => sr.EnotaMere).FirstOrDefault()
            }).ToList();

            foreach (var item in groupedRecallPos)
            {
                if ((item.OdkpoklicVsotaKolicina + item.TrenutnaZaloga) > item.OptimalnaZaloga && CommonMethods.Trim(item.TipTransportaNaziv) == criticalTransportType && CommonMethods.Trim(item.EnotaMere) == Enums.UnitsFromOrder.KG.ToString())
                {
                    if (!optimalStockOverflowIds.Exists(op => (int)op == item.OdpoklicPozicijaID))
                        optimalStockOverflowIds.Add(item.OdpoklicPozicijaID);

                    if (setCallbackResult)
                        ASPxGridViewSelectedPositions.JSProperties["cpQuntityOverflowOptimalStock"] = "Preseg optimalne zaloge";
                }
            }

            GetRecallDataProvider().SetRecallPosIDOptimalStockOverflow(optimalStockOverflowIds);

            if (optimalStockOverflowIds.Count > 0 && recallPos.Count > 0)//ali obstajajo takšni id-ji ki imajo preseg optimalne količine in imajo transport tip 15 (tipPrevoza)
                return true;
            else
                return false;
        }

        private bool CheckForOptimalStockOverflowFromPrevRecalls(List<RecallPositionModel> recallPos, List<MaterialModel> previousMaterials, out string errorMsg)
        {
            recallPos = SelectOnlyRecallPosWithTransportType15(recallPos);

            List<object> optimalStockOverflowIds = GetRecallDataProvider().GetRecallPosIDOptimalStockOverflow();

            var groupedRecallPos = recallPos.GroupBy(r => r.MaterialIdent).Select(rs => new
            {
                OdpoklicPozicijaID = rs.Select(sr => sr.OdpoklicPozicijaID).FirstOrDefault(),
                OdkpoklicVsotaKolicina = rs.Sum(sr => sr.Kolicina),
                OptimalnaZaloga = rs.Select(sr => sr.OptimalnaZaloga).FirstOrDefault(),
                TrenutnaZaloga = rs.Select(sr => sr.TrenutnaZaloga).FirstOrDefault(),
                Ident = rs.Select(sr => sr.MaterialIdent).FirstOrDefault(),
                KolicinaOTP = rs.Select(sr => sr.KolicinaOTP).FirstOrDefault(),
                VsotaKolicinaOTPPozicijaNarocilnice = rs.Sum(sr => sr.KolicinaOTPPozicijaNarocilnice),
                TipTransportaNaziv = rs.Select(sr => sr.TipNaziv).FirstOrDefault(),
                EnotaMere = rs.Select(sr => sr.EnotaMere).FirstOrDefault()
            }).ToList();

            //preverimo na posameznih pozicijah če je kje preseg optimalne količine
            foreach (var item in recallPos)
            {
                if ((item.KolicinaOTP + item.KolicinaOTPPozicijaNarocilnice + item.Kolicina + item.TrenutnaZaloga) > item.OptimalnaZaloga &&
                    CommonMethods.Trim(item.TipNaziv) == criticalTransportType && item.EnotaMere == Enums.UnitsFromOrder.KG.ToString())
                {
                    if (!optimalStockOverflowIds.Exists(op => (int)op == item.OdpoklicPozicijaID))
                        optimalStockOverflowIds.Add(item.OdpoklicPozicijaID);

                    errorMsg = "Preseg optimalne zaloge na posamezni poziciji odpoklica";
                    //return true;
                }
            }

            foreach (var item in groupedRecallPos)
            {
                if ((item.KolicinaOTP + item.VsotaKolicinaOTPPozicijaNarocilnice + item.OdkpoklicVsotaKolicina + item.TrenutnaZaloga) > item.OptimalnaZaloga &&
                    CommonMethods.Trim(item.TipTransportaNaziv) == criticalTransportType && item.EnotaMere == Enums.UnitsFromOrder.KG.ToString())
                {
                    if (!optimalStockOverflowIds.Exists(op => (int)op == item.OdpoklicPozicijaID))
                        optimalStockOverflowIds.Add(item.OdpoklicPozicijaID);

                    errorMsg = "Preseg optimalne zaloge na podlagi prejšnjih odpoklicev";
                    //return true;
                }
            }


            GetRecallDataProvider().SetRecallPosIDOptimalStockOverflow(optimalStockOverflowIds);

            errorMsg = "";
            if (optimalStockOverflowIds.Count > 0 && recallPos.Count > 0)//ali obstajajo takšni id-ji ki imajo preseg optimalne količine in imajo transport tip 15 (tipPrevoza)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CheckRecallForAnomalies(bool isSupplierArrangesTransport = false, bool isBuyerArrangesTransport = false)
        {
            //decimal kolicinaVsota = CommonMethods.ParseDecimal(hfCurrentSum["CurrenSum"]);
            decimal kolicinaVsota = CommonMethods.ParseDecimal(GetTotalSummaryValue());
            string sArgumentOfApproval = "";
            bool bOptimalnaPrekoracena = false;
            bIsKos = CheckIfKosPosition();

            if (model == null)
            {
                if (GetRecallDataProvider().GetRecallFullModel() != null)
                {
                    model = GetRecallDataProvider().GetRecallFullModel();
                }
                else
                {
                    CommonMethods.LogThis("Recall model is null:");
                    return;
                }

            }

            CommonMethods.LogThis("Log CheckRecallForAnomalies 1 - ŠT :" + model.OdpoklicStevilka.ToString());
            string memKomentarOdobritve = "";

            if (kolicinaVsota > GetMaxQuantityForRecall() && !bIsKos)
            {
                CommonMethods.LogThis("Log CheckRecallForAnomalies 2 - ŠT :" + model.OdpoklicStevilka.ToString());

                CallbackPanelUserInput.JSProperties["cpError"] = "Previsoka količina";
                btnRecall.ClientEnabled = false;

                //če imamo previsoko količino in obkljukano Dobavitelj priskrbi prevoz
                if (isSupplierArrangesTransport)
                    EnableUserControls(false);
                else if (isBuyerArrangesTransport)
                    EnableUserControls(false, true);

                return;
            }

            if (model == null) return;

            if (GetRecallDataProvider().HasTenderListValues())
            {
                CommonMethods.LogThis("Log CheckRecallForAnomalies 3 - ŠT :" + model.OdpoklicStevilka.ToString());

                string errorMessage = "";
                EnableUserControls();
                decimal newPrice = CommonMethods.ParseDecimal(txtNovaCena.Text);
                if (newPrice <= 0 && (!isSupplierArrangesTransport && !isBuyerArrangesTransport))
                {
                    CallbackPanelUserInput.JSProperties["cpError"] = "Vnesi ceno prevoza!";
                    return;
                }

                bool needToConfirmRecall = false;
                bOptimalnaPrekoracena = false;

                //odpoklic pozicij preverjamo smo če imajo transport tipa 15

                if (isNewPriceHigherFromTender())
                {
                    model.LowestPrice = model.LowestPrice > 0 ? model.LowestPrice : GetLatestPrice();

                    CommonMethods.LogThis("Log CheckRecallForAnomalies 4 - ŠT :" + model.OdpoklicStevilka.ToString());
                    sArgumentOfApproval += "Cena je višja od najcenejšega razpisa za izbrano relacijo (" + model.LowestPrice.ToString("N2") + "). | ";
                    bool optimalStockOverFlow = CheckForOptimalStockOverflow(model.OdpoklicPozicija);//preverimo če je cena napačna in preverimo še če je optimalna zaloga prekoračena.
                    needToConfirmRecall = true;
                    memKomentarOdobritve += "Cena je višja od najcenejšega razpisa za izbrano relacijo (" + model.LowestPrice.ToString("N2") + "). " + (optimalStockOverFlow ? "\r\n Preseg optimalne količine. |" : " (" + GetLatestPrice().ToString("N2") + ")") + " |";
                    //AddValueToSession(Enums.RecallSession.ArgumentsOfApproval, "Cena je višja od zadnjega razpisa izbranega prevoznika. (" + GetLatestPrice().ToString("N2") + ")");
                }

                if (CheckForOptimalStockOverflow(model.OdpoklicPozicija))//ob vnosu cene še preverimo če je preseg optimalne količine
                {
                    CommonMethods.LogThis("Log CheckRecallForAnomalies 5 - ŠT :" + model.OdpoklicStevilka.ToString());
                    sArgumentOfApproval += "Odpoklicana količina je višja od optimalne. | ";
                    needToConfirmRecall = true;
                    memKomentarOdobritve += "Odpoklicana količina je višja od optimalne." + " | ";
                    //AddValueToSession(Enums.RecallSession.ArgumentsOfApproval, "Odpoklicana količina višja od optimalne");

                }
                if (!isTransportType15Valid(model.OdpoklicPozicija))//preverimo če smo prekoračili optimalno zalogo za transport 15
                {
                    CommonMethods.LogThis("Log CheckRecallForAnomalies 6 - ŠT :" + model.OdpoklicStevilka.ToString());

                    needToConfirmRecall = true;
                    memKomentarOdobritve += "Odpoklicana količina je višja od optimalne (tip transporta 15)." + " | ";
                    bOptimalnaPrekoracena = true;
                    //AddValueToSession(Enums.RecallSession.ArgumentsOfApproval, "Odpoklicana količina višja od optimalne");
                }

                if (CheckForOptimalStockOverflowFromPrevRecalls(model.OdpoklicPozicija, GetMaterialValues(), out errorMessage))//preverimi če smo prekoračili optimalno zalogo na podlagi prejšnjih odpoklicev
                {
                    CommonMethods.LogThis("Log CheckRecallForAnomalies 7 - ŠT :" + model.OdpoklicStevilka.ToString());
                    sArgumentOfApproval += "Prekoračena optimalna količina. V prejšnjih odpoklicih | ";
                    needToConfirmRecall = true;
                    memKomentarOdobritve += "Prekoračena optimalna količina." + " | ";
                    bOptimalnaPrekoracena = true;
                    //AddValueToSession(Enums.RecallSession.ArgumentsOfApproval, "Odpoklicana količina višja od optimalne");
                }

                if (!needToConfirmRecall)
                {
                    CommonMethods.LogThis("Log CheckRecallForAnomalies 8 - ŠT :" + model.OdpoklicStevilka.ToString());

                    btnRecall.ForeColor = Color.Green;
                    //memoKomentar.ClientVisible = false;
                    btnRecall.Text = "Odpokliči";
                    GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.NEZNAN);
                }


                if (needToConfirmRecall)
                {
                    CommonMethods.LogThis("Log CheckRecallForAnomalies 9 - ŠT :" + model.OdpoklicStevilka.ToString());

                    memoKomentar.ClientVisible = true;
                    btnRecall.Text = "V odobritev";
                    btnRecall.ForeColor = Color.Tomato;
                    GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.V_ODOBRITEV);
                }


                btnRecall.ClientEnabled = true;
            }

            if (isSupplierArrangesTransport)
            {
                CommonMethods.LogThis("Log CheckRecallForAnomalies 10 - ŠT :" + model.OdpoklicStevilka.ToString() + " Optimalna prekoračena: " + bOptimalnaPrekoracena);
                btnRecall.Text = bOptimalnaPrekoracena == true ? "V odobritev" : "Odpokliči";
                EnableUserControls(false);
                btnRecall.ClientEnabled = true;
                btnRecall.ForeColor = bOptimalnaPrekoracena == true ? Color.Red : Color.Green;
                sArgumentOfApproval += "";
            }
            else if (isBuyerArrangesTransport)
            {
                CommonMethods.LogThis("Log CheckRecallForAnomalies 11 - ŠT :" + model.OdpoklicStevilka.ToString());
                btnRecall.Text = bOptimalnaPrekoracena == true ? "V odobritev" : "Odpokliči";
                EnableUserControls(false, true);
                btnRecall.ClientEnabled = true;
                btnRecall.ForeColor = bOptimalnaPrekoracena == true ? Color.Red : Color.Green;
                sArgumentOfApproval += "";
                GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.NEZNAN);//nastavimo status nazaj na neznano če je slučajno prišlo vmes do spreminjnja checkbox-a (dobavitelj ali kupec priskrbi prevoz)
            }

            //preverimo če je tip prevoza kamion - če vsota količine večja od 20000 potem more biti v mejah med 24000 in 24500 drugače gre v odobritev.
            //To ne preverjamo če imamo obkljukano dobavitelj priskrbi prevoz ali kupec priskrbi prevoz.
            if (GetSelecctedTransportTypeCode() == DatabaseWebService.Common.Enums.Enums.TransportType.KAMION.ToString() &&
               (kolicinaVsota >= 20000 && !(kolicinaVsota >= 24500 && kolicinaVsota <= 26000) && !bIsKos) && String.IsNullOrEmpty(memoKomentar.Text) &&
               (!SupplierArrangesTransportCheckBox2.Checked && !BuyerArrangesTransportCheckBox2.Checked))
            {
                CommonMethods.LogThis("Log CheckRecallForAnomalies 12 - ŠT :" + model.OdpoklicStevilka.ToString());
                memKomentarOdobritve += "Skupna količina presega 20000kg in ni v mejah med 24500kg in 26000kg.";
                sArgumentOfApproval += "Skupna količina presega 20000kg in ni v mejah med 24500kg in 26000kg. | ";
                AddValueToSession(Enums.RecallSession.ArgumentsOfApproval, "Skupna količina presega 20000kg in ni v mejah med 24500kg in 26000kg.");
                memoKomentar.ClientVisible = true;
                btnRecall.Text = "V odobritev";
                btnRecall.ForeColor = Color.Tomato;
                GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.V_ODOBRITEV);
            }

            CommonMethods.LogThis("Log CheckRecallForAnomalies 13 - ŠT :" + model.OdpoklicStevilka.ToString());
            CommonMethods.LogThis("btnRecall.Text :" + btnRecall.Text);
            CommonMethods.LogThis("memKomentarOdobritve :" + memKomentarOdobritve);
            CommonMethods.LogThis("isSupplierArrangesTransport :" + isSupplierArrangesTransport);
            CommonMethods.LogThis("isBuyerArrangesTransport :" + isBuyerArrangesTransport);


            SetZbirnikTonByODpoklicValue();
            AddValueToSession(Enums.RecallSession.ArgumentsOfApproval, memKomentarOdobritve);
            AddValueToSession(Enums.RecallSession.ArgumentsOfApprovalToDB, sArgumentOfApproval);
            memKomentarOdobritve += " Vpiši komentar";
            memoKomentar.NullText = memKomentarOdobritve;
            //memoKomentar.Text = "";
        }

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
            CommonMethods.LogThis("Recall click");
            bool isValid = true;
            string sArgumentOfApproval = "";

            decimal kolicinaVsota = CommonMethods.ParseDecimal(GetTotalSummaryValue());
            bIsKos = CheckIfKosPosition();

            CommonMethods.LogThis("Status: " + GetRecallDataProvider().GetRecallStatus().ToString());
            CommonMethods.LogThis("model.StatusID: " + model.StatusID.ToString());

            //odpoklic pozicij preverjamo smo če imajo transport tipa 15
            if (CheckForOptimalStockOverflow(model.OdpoklicPozicija))
            {
                CommonMethods.LogThis("Log 1 - ŠT :" + model.OdpoklicStevilka.ToString());
                ShowClientWarningPopUp("Odpoklicana količina presega optimalno!");
                sArgumentOfApproval += "Odpoklicana količina presega optimalno! | ";
                AddValueToSession(Enums.RecallSession.ArgumentsOfApproval, "Odpoklicana količina višja od optimalne");
                isValid = false;
            }
            else if (!isTransportType15Valid(model.OdpoklicPozicija))
            {
                CommonMethods.LogThis("Log 2 - ŠT :" + model.OdpoklicStevilka.ToString());
                ShowClientWarningPopUp("Odpoklicana količina presega optimalno zaradi tipa transporta (15).");
                sArgumentOfApproval += "Odpoklicana količina presega optimalno zaradi tipa transporta (15). |";
                AddValueToSession(Enums.RecallSession.ArgumentsOfApproval, "Odpoklicana količina višja od optimalne");
                isValid = false;
            }
            else if (ASPxGridLookupStranke.Text.Split(';').Length > 1)//če je izbrano več prevoznikov potem ne dovolimo odpoklica odpoklicati
            {
                CommonMethods.LogThis("Log 3 - ŠT :" + model.OdpoklicStevilka.ToString());
                ShowClientWarningPopUp("Ni možno odpoklicati odpoklica zaradi prevoznikov!");
                return;
            }

            //preverimo če je vnsenea cena večja kot tista iz razpisa
            if (isNewPriceHigherFromTender() && (GetRecallDataProvider().GetRecallStatus() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.NEZNAN && HasSessionModelStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA)))
            {
                CommonMethods.LogThis("Log 4 - ŠT :" + model.OdpoklicStevilka.ToString());

                memoKomentar.ClientVisible = true;
                btnRecall.Text = "V odobritev";
                btnRecall.ForeColor = Color.Tomato;
                GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.V_ODOBRITEV);
                ShowClientWarningPopUp("Cena višja od zadnjega razpisa (" + GetLatestPrice().ToString("N2") + "). Odpoklic je potrebno odobriti!");
                AddValueToSession(Enums.RecallSession.ArgumentsOfApproval, "Cena je višja od zadnjega razpisa izbranega prevoznika. (" + GetLatestPrice().ToString("N2") + ")");
            }
            else if (!isValid && (GetRecallDataProvider().GetRecallStatus() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.NEZNAN && HasSessionModelStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA)))
            {
                CommonMethods.LogThis("Log 5 - ŠT :" + model.OdpoklicStevilka.ToString());

                memoKomentar.ClientVisible = true;
                btnRecall.Text = "V odobritev";
                btnRecall.ForeColor = Color.Tomato;
                GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.V_ODOBRITEV);
            }
            //preverimo če je tip prevoza kamion - če vsota količine večja od 20000 potem more biti v mejah med 24000 in 24500 drugače gre v odobritev.
            //To ne preverjamo če imamo obkljukano dobavitelj priskrbi prevoz ali kupec priskrbi prevoz.
            else if (GetSelecctedTransportTypeCode() == DatabaseWebService.Common.Enums.Enums.TransportType.KAMION.ToString() &&
               (kolicinaVsota >= 20000 && !(kolicinaVsota >= 24000 && kolicinaVsota <= 26000) && !bIsKos) && (String.IsNullOrEmpty(memoKomentar.Text) ||
               GetRecallDataProvider().GetRecallStatus() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.NEZNAN && !String.IsNullOrEmpty(model.RazlogOdobritveSistem)) && //ta del pogoja zajema primer, ko uporabnik da odpoklic v odobritev in ga potem nazaj odpre
               (!SupplierArrangesTransportCheckBox2.Checked && !BuyerArrangesTransportCheckBox2.Checked))//če je delovna verzija in če ima razlog odobritev sistem polje vrednost potem moremo priti v ta if
            {
                CommonMethods.LogThis("Log 6 - ŠT :" + model.OdpoklicStevilka.ToString());

                memoKomentar.NullText = "Skupna količina presega 20000kg in ni v mejah med 24000kg in 26000kg. Zapiši komentar!";
                AddValueToSession(Enums.RecallSession.ArgumentsOfApproval, "Skupna količina presega 20000kg in ni v mejah med 24000kg in 26000kg.");
                memoKomentar.ClientVisible = true;
                btnRecall.Text = "V odobritev";
                btnRecall.ForeColor = Color.Tomato;
                GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.V_ODOBRITEV);
            }
            else
            {

                CommonMethods.LogThis("Log 7 - ŠT :" + model.OdpoklicStevilka.ToString());
                //TODO : Status se ni spremenil v času vpisovanja nove cene zato ima status privzeto vrednost
                if (GetRecallDataProvider().GetRecallStatus() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.NEZNAN)
                {
                    CommonMethods.LogThis("Log 7.1 - ŠT :" + model.OdpoklicStevilka.ToString());
                    GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POTRJEN);
                }
                else if (GetRecallDataProvider().GetRecallStatus() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.V_ODOBRITEV)
                {
                    CommonMethods.LogThis("Log 7.2 - ŠT :" + model.OdpoklicStevilka.ToString());
                    recallStatusChanged = true;
                }

                CommonMethods.LogThis("Status: " + GetRecallDataProvider().GetRecallStatus().ToString());
                CommonMethods.LogThis("model.StatusID: " + model.StatusID.ToString());



                ProcessUserAction();
            }

            AddValueToSession(Enums.RecallSession.ArgumentsOfApprovalToDB, sArgumentOfApproval);
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

            List<int> selectedPositions = ASPxGridViewSelectedPositions.GetSelectedFieldValues(ASPxGridViewSelectedPositions.KeyFieldName).OfType<int>().ToList();

            foreach (var item in selectedPositions)
            {
                RecallPositionModel obj = model.OdpoklicPozicija.Where(op => op.OdpoklicPozicijaID == item).FirstOrDefault();
                if (obj != null)
                {
                    obj.StatusPrevzeto = true;
                }
            }

            // če je število obkljukanih pozicij enako kot v seji potem vemo da je odpoklic v celoti prevzet
            if (model.OdpoklicPozicija.Where(op => op.StatusPrevzeto).Count() == model.OdpoklicPozicija.Count)
                GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.PREVZET);
            else
                GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELNO_PREVZET);

            ProcessUserAction();
        }

        protected void btnReopenRecall_Click(object sender, EventArgs e)
        {
            reopenRecall = true;
            if (model.StatusOdpoklica.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.PREVZET.ToString() && (PrincipalHelper.IsUserCarrierSpecialist() || PrincipalHelper.IsUserSuperAdmin()))
            {
                GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.PREVZET);
            }
            else
            {
                GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA);
                AddOrEditEntityObject();
            }

            List<Enums.RecallSession> list = Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList();
            ClearAllSessions(list, Request.RawUrl);
        }

        protected void btnSendInquiry_Click(object sender, EventArgs e)
        {
            recallStatusChanged = true;
            GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.RAZPIS_PREVOZNIK);
            ProcessUserAction();
        }

        #endregion

        #region ASPxGridViewSelectedPositions Events

        protected void ASPxGridViewSelectedPositions_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            bool isError = false;
            List<object> optimalStockOverflowIds = GetRecallDataProvider().GetRecallPosIDOptimalStockOverflow();
            model = GetRecallDataProvider().GetRecallFullModel();
            if (model == null) return;

            decimal kolicinaVsota = CommonMethods.ParseDecimal(hfCurrentSum["CurrenSum"]);

            if (kolicinaVsota > GetMaxQuantityForRecall())
            {
                ASPxGridViewSelectedPositions.JSProperties["cpQuntityOversize"] = "Previsoka kolicina";
                isError = true;
                //e.Handled = true;
                //return;
            }

            RecallPositionModel recallPosModel = null;
            List<RecallPositionModel> updatedList = model.OdpoklicPozicija;

            Type myType = typeof(RecallPositionModel);
            List<PropertyInfo> myPropInfo = myType.GetProperties().ToList();

            foreach (ASPxDataUpdateValues item in e.UpdateValues)
            {
                recallPosModel = new RecallPositionModel();

                foreach (DictionaryEntry obj in item.Keys)//we set table ID
                {
                    PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                    if (info != null)
                    {
                        recallPosModel = updatedList.Where(r => r.OdpoklicPozicijaID == (int)obj.Value).FirstOrDefault();
                        object kolicinaValue = 0;
                        foreach (DictionaryEntry inst in item.NewValues)
                        {
                            if (inst.Key.ToString().Equals("Kolicina"))
                            {
                                kolicinaValue = inst.Value;
                                break;
                            }
                        }

                        //prvi del maksimalno odpoklicane količine izračunamo iz polja KolicinaPrevzeta če je večja kot 0, drugače vzamemo odpoklcano količino za pozicijo naročilnice
                        //drugi del dobimo iz proizvedene količine če je večja kot 0, drugače vzamemo KolicinaRazlika
                        //Maksimalno količino dobimo iz razlike med spremenljivko izbrana kolicina in proizvedeno ali KolicinaRazlika
                        decimal maxRecallQuantity = 0;
                        decimal izbranaKolicina = recallPosModel.KolicinaPrevzeta <= 0 ? recallPosModel.KolicinaOTPPozicijaNarocilnice : recallPosModel.KolicinaPrevzeta;

                        // 28.07.2020 - popravek
                        //if (recallPosModel.Proizvedeno > 0)
                        //{
                        //    maxRecallQuantity = (recallPosModel.Proizvedeno - izbranaKolicina);
                        //}
                        //else
                        //    maxRecallQuantity = (recallPosModel.KolicinaRazlika - izbranaKolicina);

                        if (recallPosModel.Proizvedeno > 0)
                        {
                            maxRecallQuantity = recallPosModel.Proizvedeno;
                        }
                        else
                            maxRecallQuantity = izbranaKolicina;

                        //Če je odpoklicana količina previsoka uporabnika obvestimo in dodamo to pozicijo v seznam s keterim označimo pozicijo v gridview-u rdeče
                        if (maxRecallQuantity < CommonMethods.ParseDecimal(kolicinaValue))
                        {
                            ASPxGridViewSelectedPositions.JSProperties["cpRecallQuantityOversize"] = "Previsoka odpoklicana količina! " + (maxRecallQuantity > 0 ? "Max: " + maxRecallQuantity.ToString("N2") : "");
                            //e.Handled = true;
                            //return;
                            isError = true;

                            if (!optimalStockOverflowIds.Exists(op => (int)op == (int)obj.Value))
                                optimalStockOverflowIds.Add(obj.Value);
                        }

                        //Če je preseg optimalne količine obvestimo uporabnika in dodamo pozicijo v seznam s katerim označimo pozicije rdečo ki presegajo optimalno količino
                        if (!isTransportType15Valid(obj.Value, kolicinaValue))
                        {
                            ASPxGridViewSelectedPositions.JSProperties["cpQuntityOverflowOptimalStock"] = "Preseg optimalne zaloge";
                            //e.Handled = true;
                            //return;
                            isError = true;

                            if (!optimalStockOverflowIds.Exists(op => (int)op == (int)obj.Value))
                                optimalStockOverflowIds.Add(obj.Value);
                        }

                        if (!isError)
                        {
                            optimalStockOverflowIds.Remove(obj.Value);
                        }

                        break;
                    }
                }

                SetValuesToRecallPosList(item.NewValues, myPropInfo, recallPosModel);
            }


            model.OdpoklicPozicija = updatedList;
            GetRecallDataProvider().SetRecallPosIDOptimalStockOverflow(optimalStockOverflowIds);//nastavimo pozicije v sejo ki niso v mejah pravil

            ASPxGridViewSelectedPositions.DataBind();
            GetRecallDataProvider().SetRecallFullModel(model);//nastavimo celoten model + pozicije z novimi vrednostmi

            if (isError)
            {
                e.Handled = true;
                return;
            }
            else if (CheckForOptimalStockOverflow(updatedList))//še enkrat preverimo če katera pozicija v seznamu presega optimalno količino
            {
                ASPxGridViewSelectedPositions.JSProperties["cpQuntityOverflowOptimalStock"] = "Preseg optimalne zaloge";
                e.Handled = true;
                return;
            }

            e.Handled = true;
        }

        private void SetValuesToRecallPosList(OrderedDictionary newValues, List<PropertyInfo> myPropInfo, RecallPositionModel recallPosModel)
        {
            foreach (DictionaryEntry obj in newValues)
            {
                PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                if (info != null)
                    info.SetValue(recallPosModel, obj.Value);
            }
        }

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
            if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
            {
                if (action != (int)Enums.UserAction.Add && model != null && (model.StatusOdpoklica.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POTRJEN.ToString() || model.StatusOdpoklica.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELNO_PREVZET.ToString()))
                {
                    ASPxGridView grid = sender as ASPxGridView;
                    bool prevzeto = Convert.ToBoolean(grid.GetRowValues(e.VisibleIndex, "StatusPrevzeto"));

                    if (!prevzeto)
                        e.Visible = true;
                    else
                        e.Visible = false;
                }
                else
                {
                    e.Visible = false;
                }
            }
        }

        #endregion

        protected void CallbackPanelUserInput_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "Enable")
            {
                EnableUserControls();
                if (SupplierArrangesTransportCheckBox2.Checked || BuyerArrangesTransportCheckBox2.Checked)
                {
                    //preverimo odpoklicane količine, cene,...
                    CheckRecallForAnomalies(SupplierArrangesTransportCheckBox2.Checked, BuyerArrangesTransportCheckBox2.Checked);
                    ASPxGridLookupRealacija.ClientEnabled = true;
                    ASPxGridLookupRealacija.BackColor = Color.White;
                    ASPxMemoOpombe.ClientEnabled = true;
                    ASPxMemoOpombe.BackColor = Color.White;
                }
                else
                    CheckRecallForAnomalies();
            }
            else if (e.Parameter == "DataBindPrevoznik")//Ko uporabnik izbere relacijo je potrebno poiskati vse prevoznike za to relacijo
            {

                if (ASPxGridLookupRealacija.Value != null && (!SupplierArrangesTransportCheckBox2.Checked && !BuyerArrangesTransportCheckBox2.Checked))
                {
                    int routeValueID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupRealacija));
                    int ZbirnikTonID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupZbirnikTon));

                    decimal lowestPrice = (ZbirnikTonID == 0) ? CheckModelValidation(GetDatabaseConnectionInstance().GetLowestAndMostRecentPriceByRouteID(routeValueID)) : CheckModelValidation(GetDatabaseConnectionInstance().GetLowestAndMostRecentPriceByRouteIDandZbirnikTonsID(routeValueID, ZbirnikTonID));
                    txtNovaCena.Text = lowestPrice.ToString("N2");
                    model.LowestPrice = (model != null) ? lowestPrice : 0;


                    ASPxGridLookupStranke.DataBind();
                }

                if (SupplierArrangesTransportCheckBox2.Checked || BuyerArrangesTransportCheckBox2.Checked)
                {
                    //preverimo odpoklicane količine,...
                    CheckRecallForAnomalies(SupplierArrangesTransportCheckBox2.Checked, BuyerArrangesTransportCheckBox2.Checked);
                    ASPxGridLookupRealacija.ClientEnabled = true;
                    ASPxGridLookupRealacija.BackColor = Color.White;
                    ASPxMemoOpombe.ClientEnabled = true;
                    ASPxMemoOpombe.BackColor = Color.White;
                }
                else
                    CheckRecallForAnomalies();
            }
            else if (e.Parameter == "PriceCompare")//Ko uporabnik izbere prevoznika ali vnese ceno je potrebno preveriti vnešeno ceno in tisto ki jo ima prevoznik iz razpisa
            {

                if (model.StatusOdpoklica != null && model.StatusOdpoklica.Koda != DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELOVNA.ToString())
                    return;

                string komentarTemp = memoKomentar.Text;
                memoKomentar.Text = "";

                //preverimo če obstaja pozicija z tip prevoza 15 in če je vsota odpoklicane količine in trenutne zaloge večja od optimalne zaloge
                if (model.OdpoklicPozicija != null)
                {
                    foreach (var item in model.OdpoklicPozicija)
                    {
                        if (!isTransportType15Valid(item.OdpoklicPozicijaID, null))
                        {
                            CallbackPanelUserInput.JSProperties["cpError"] = "Preseg optimalne zaloge!";
                            break;
                            //return;
                        }
                    }
                }
                EnableUserControls();
                //preverimo odpoklicane količine, cene,...
                CheckRecallForAnomalies();

                if (!String.IsNullOrEmpty(komentarTemp))
                    memoKomentar.Text = komentarTemp;
            }
            else if (e.Parameter == "SupplierArrangesTransport")//Če uporabnik obkljuka Dobavitelj priskrbi prevoz
            {
                //preverimo odpoklicane količine,...
                CheckRecallForAnomalies(true);
                EnableUserControls(false);
                ASPxGridLookupRealacija.ClientEnabled = true;
                ASPxGridLookupRealacija.BackColor = Color.White;
                ASPxMemoOpombe.ClientEnabled = true;
                ASPxMemoOpombe.BackColor = Color.White;
                txtNovaCena.Text = "";
                ASPxGridLookupStranke.Value = null;
                //SupplierArrangesTransportCheckBox2.ClientEnabled = true;
                //ASPxGridLookupStranke.Value = null;

                model.DobaviteljUrediTransport = SupplierArrangesTransportCheckBox2.Checked;
                model.KupecUrediTransport = BuyerArrangesTransportCheckBox2.Checked;

                GetRecallDataProvider().SetRecallFullModel(model);
                BuyerArrangesTransportCheckBox2.Checked = false;
            }
            else if (e.Parameter == "BuyerArrangesTransport")// Če uporabnik obkljuka kupec priskrbi prevoz
            {
                //preverimo odpoklicane količine,...
                CheckRecallForAnomalies(false, true);
                EnableUserControls(false, true);
                ASPxGridLookupRealacija.ClientEnabled = true;
                ASPxGridLookupRealacija.BackColor = Color.White;
                ASPxMemoOpombe.ClientEnabled = true;
                ASPxMemoOpombe.BackColor = Color.White;
                txtNovaCena.Text = "";
                ASPxGridLookupStranke.Value = null;
                memoKomentar.ClientVisible = false;

                txtRegistracija.ClientEnabled = true;
                txtRegistracija.BackColor = Color.White;

                BuyerArrangesTransportCheckBox2.ClientEnabled = true;

                model.DobaviteljUrediTransport = SupplierArrangesTransportCheckBox2.Checked;
                model.KupecUrediTransport = BuyerArrangesTransportCheckBox2.Checked;

                GetRecallDataProvider().SetRecallFullModel(model);
                SupplierArrangesTransportCheckBox2.Checked = false;
            }
            else if (e.Parameter == "ShowOrderPositionPopUp")//Če uporabnik želi dodati novo pozicijo iz naročila
            {
                AddValueToSession(Enums.OrderSession.SupplierID, model.DobaviteljNaziv);
                //AddValueToSession(Enums.OrderSession., model.DobaviteljNaziv);
                AddValueToSession(Enums.CommonSession.UserActionPopUp, action);

                model = GetRecallDataProvider().GetRecallFullModel();
                model.TipPrevozaID = CommonMethods.ParseInt(ASPxGridLookupTipPrevoza.Value);
                model.ZbrirnikTonID = CommonMethods.ParseInt(ASPxGridLookupTipPrevoza.Value);
                GetRecallDataProvider().SetRecallFullModel(model);

                ASPxPopupControlOrderPos.ShowOnPageLoad = true;
            }
            else if (e.Parameter == "DeleteSelectedPosition")//Če uporabnik želi izbrisati izbrano pozicijo iz odpoklica
            {
                int odpoklicPozicijaID = CommonMethods.ParseInt(ASPxGridViewSelectedPositions.GetRowValues(ASPxGridViewSelectedPositions.FocusedRowIndex, "OdpoklicPozicijaID"));
                if (odpoklicPozicijaID > 0)
                {

                    SetZbirnikTonByODpoklicValue();

                    CheckModelValidation(GetDatabaseConnectionInstance().DeleteRecallPosition(odpoklicPozicijaID));
                    var recallPos = model.OdpoklicPozicija.Where(op => op.OdpoklicPozicijaID == odpoklicPozicijaID).FirstOrDefault();
                    if (recallPos != null)
                        model.OdpoklicPozicija.Remove(recallPos);

                    CallbackPanelUserInput.JSProperties["cpRefreshGrid"] = true;
                    hfCurrentSum["CurrenSum"] = GetTotalSummaryValue();
                }
            }
            else if (e.Parameter == "OwnStockWarehouse")//Če uproabnik spremeni tip prevoza na ladjo ali letalo potem izbere v katero skladišče želi narediti odpoklic.
            {
                //preverimo odpoklicane količine, cene,...
                bool needToConfirmRecall = false;
                string errorMessage = "";
                //odpoklic pozicij preverjamo smo če imajo transport tipa 15
                if (CheckForOptimalStockOverflow(model.OdpoklicPozicija))//ob vnosu cene še preverimo če je preseg optimalne količine
                {
                    needToConfirmRecall = true;
                    memoKomentar.NullText = "Odpoklicana količina je višja od optimalne. Zapiši komentar!";
                    AddValueToSession(Enums.RecallSession.ArgumentsOfApproval, "Odpoklicana količina višja od optimalne");

                }
                else if (!isTransportType15Valid(model.OdpoklicPozicija))//preverimo če smo prekoračili optimalno zalogo za transport 15
                {
                    needToConfirmRecall = true;
                    memoKomentar.NullText = "Odpoklicana količina je višja od optimalne (tip transporta 15). Zapiši komentar!";
                    AddValueToSession(Enums.RecallSession.ArgumentsOfApproval, "Odpoklicana količina višja od optimalne");
                }
                else if (CheckForOptimalStockOverflowFromPrevRecalls(model.OdpoklicPozicija, GetMaterialValues(), out errorMessage))//preverimi če smo prekoračili optimalno zalogo na podlagi prejšnjih odpoklicev
                {
                    needToConfirmRecall = true;
                    memoKomentar.NullText = "Prekoračena optimalna količina. Zapiši komentar!";
                    AddValueToSession(Enums.RecallSession.ArgumentsOfApproval, "Odpoklicana količina višja od optimalne");
                }
                else
                {
                    btnRecall.ForeColor = Color.Green;
                    memoKomentar.ClientVisible = false;
                    btnRecall.Text = "Odpokliči";
                    GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.NEZNAN);
                    EnableUserControls();
                }

                if (needToConfirmRecall)
                {
                    memoKomentar.ClientVisible = true;
                    btnRecall.Text = "V odobritev";
                    btnRecall.ForeColor = Color.Tomato;
                    GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.V_ODOBRITEV);
                }

                btnRecall.ClientEnabled = true;

                /*if (SupplierArrangesTransportCheckBox2.Checked)
                {
                    EnableUserControls(false);
                    btnRecall.ClientEnabled = true;
                    btnRecall.ForeColor = Color.Green;
                }*/
            }
            else if (e.Parameter == "SplitSelectedPosition")//Če uporabnik želi razdeliti izbrano pozicijo
            {
                int odpoklicPozID = CommonMethods.ParseInt(ASPxGridViewSelectedPositions.GetRowValues(ASPxGridViewSelectedPositions.FocusedRowIndex, "OdpoklicPozicijaID"));
                if (odpoklicPozID <= 0) return;

                var item = model.OdpoklicPozicija.Where(op => op.OdpoklicPozicijaID == odpoklicPozID).FirstOrDefault();
                if (item != null)
                {
                    RecallPositionModel splitRecallPos = new RecallPositionModel();
                    item.KolicinaIzNarocila = item.KolicinaIzNarocila / 2;
                    item.KolicinaPrevzeta = item.KolicinaPrevzeta / 2;
                    item.KolicinaRazlika = item.KolicinaRazlika / 2;
                    item.Kolicina = item.Kolicina > 0 ? item.Kolicina / 2 : 0;
                    item.TransportnaKolicina = item.TransportnaKolicina > 0 ? item.TransportnaKolicina / 2 : 0;
                    PropertyCopyHelper<RecallPositionModel, RecallPositionModel>.Copy(item, splitRecallPos);

                    //po končanem kopiranje je potrebno novi poziciji nastaviti OdpoklicPozicijaID na 0
                    splitRecallPos.OdpoklicPozicijaID = model.OdpoklicPozicija.Max(op => op.OdpoklicPozicijaID) + 1;
                    splitRecallPos.ZaporednaStevilka = model.OdpoklicPozicija.Max(m => m.ZaporednaStevilka) + 1;
                    splitRecallPos.childSplit = true;
                    splitRecallPos.Split = true;
                    item.Split = true;
                    model.OdpoklicPozicija.Add(splitRecallPos);
                    SetZbirnikTonByODpoklicValue();
                    CallbackPanelUserInput.JSProperties["cpRefreshGrid"] = true;
                }
            }
            else if (e.Parameter == "OpenCarriersInquiryPopUp")
            {
                bool open = SetSessionsAndOpenPopUp("2", Enums.RecallSession.InquirySummaryRecallID, recallID);
                ASPxPopupControlCarriersInquirySummary.ShowOnPageLoad = open;
            }
            else if (e.Parameter == "OpenNewOrderPopUp")
            {
                GetRecallDataProvider().SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POTRJEN);
                AddOrEditEntityObject();
                bool open = SetSessionsAndOpenPopUp("2", Enums.RecallSession.InquirySummaryRecallID, recallID);
                ASPxPopupControlCreateOrder.ShowOnPageLoad = open;
            }
        }

        private void SetZbirnikTonByODpoklicValue()
        {
            hfCurrentSum["CurrenSum"] = GetTotalSummaryValue();
            decimal dCurrentWeightValue = CommonMethods.ParseDecimal(hfCurrentSum["CurrenSum"]);
            bIsKos = CheckIfKosPosition();
            if (!bIsKos)
            {
                ASPxGridLookupZbirnikTon.Value = ReturnZbirnikTonIDByOdpoklicValue(CommonMethods.ParseDecimal(dCurrentWeightValue));
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
                ASPxGridViewSelectedPositions.FocusedRowIndex = -1;
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