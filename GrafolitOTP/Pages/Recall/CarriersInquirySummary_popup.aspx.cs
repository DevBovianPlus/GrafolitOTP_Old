using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.ModelsOTP.Recall;
using DatabaseWebService.ModelsOTP.Tender;
using DevExpress.Web;
using DevExpress.Web.Rendering;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Recall
{
    public partial class CarriersInquirySummary_popup : ServerMasterPage
    {
        List<CarrierInquiryModel> model;
        int recallAction = -1;
        int recallID = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            recallAction = CommonMethods.ParseInt(GetStringValueFromSession(Enums.CommonSession.UserActionPopUp));
            recallID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.RecallSession.InquirySummaryRecallID));

            ASPxGridLookupStranke.GridView.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize(recallID);
            }
            else
            {
                if (model == null && SessionHasValue(Enums.RecallSession.InquirySummaries))
                    model = GetCarriersInquiryDataProvider().GetCarrierInquiryModel();
            }
        }


        #region Initialize
        private void Initialize(int recall)
        {
            /*if (model == null)
                PopulateModel(recall);*/


            ASPxGridViewCarrierInquiry.DataBind();
            ASPxGridLookupStranke.DataBind();
        }

        private void PopulateModel(int recall)
        {
            model = CheckModelValidation(GetDatabaseConnectionInstance().GetCarriersInquiry(recall));

            GetCarriersInquiryDataProvider().SetCarrierInquiryModel(model);
        }
        #endregion

        #region Helper methods
        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.RecallSession.InquirySummaryRecallID);

            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}', '{2}', '{3}');", confirmCancelAction, "InquirySummary", recallAction, recallID), true);
            RemoveSession(Enums.RecallSession.RecallFulModel);
        }
        #endregion

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            //tukaj bo vedno izbran samo en prevoznik (na client side preprečujemo da bi izbrav več kot enega in stisnil na gumb izberi prevoznika)
            List<int> selectedItems = ASPxGridViewCarrierInquiry.GetSelectedFieldValues("PrijavaPrevoznikaID").OfType<int>().ToList();

            CheckModelValidation(GetDatabaseConnectionInstance().ManualSelectCarrierForTransport(selectedItems[0]));

            RemoveSessionsAndClosePopUP(true);
        }

        protected void ASPxGridViewCarrierInquiry_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            model = GetModel();
            if (model != null && model.Count > 0)
            {
                if (e.Parameters == "AddNewCarriers")
                {
                    RecallFullModel recallFullModel = new RecallFullModel();
                    recallFullModel.Prevozniki = ASPxGridLookupStranke.Text;
                    recallFullModel.RelacijaID = model[0].Odpoklic.RelacijaID;
                    recallFullModel.CenaPrevoza = model[0].Odpoklic.CenaPrevoza;
                    recallFullModel.DatumNaklada = model[0].DatumNaklada;
                    recallFullModel.OdpoklicID = model[0].OdpoklicID;

                    CheckModelValidation(GetDatabaseConnectionInstance().SaveNewAddedCarrierForInquiry(recallFullModel));
                }
                else if (e.Parameters == "ReSendEmailToCarriers")
                {
                    List<int> selectedItems = ASPxGridViewCarrierInquiry.GetSelectedFieldValues("PrijavaPrevoznikaID").OfType<int>().ToList();
                    List<CarrierInquiryModel> items = model.Where(cim => selectedItems.Any(si => si == cim.PrijavaPrevoznikaID)).ToList();

                    CheckModelValidation(GetDatabaseConnectionInstance().ReSendEmailToCarriers(items));
                }
                else if (e.Parameters == "DeleteCarrier")
                {
                    //tukaj bo vedno izbran samo en prevoznik (na client side preprečujemo da bi izbrav več kot enega in stisnil na gumb izberi prevoznika)
                    List<int> selectedItems = ASPxGridViewCarrierInquiry.GetSelectedFieldValues("PrijavaPrevoznikaID").OfType<int>().ToList();

                    CheckModelValidation(GetDatabaseConnectionInstance().DeleteCarrierInquiry(selectedItems[0]));
                }
            }
        }

        protected void ASPxGridViewCarrierInquiry_DataBinding(object sender, EventArgs e)
        {
            PopulateModel(recallID);

            (sender as ASPxGridView).DataSource = model;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridLookupStranke_DataBinding(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            int idRoute = 0;
            int recallID = 0;
            if (model != null && model.Count > 0)
            {
                idRoute = model[0].Odpoklic.RelacijaID;
                recallID = model[0].Odpoklic.OdpoklicID;
            }
            else if (SessionHasValue(Enums.RecallSession.InquirySummaries))
            {
                bool hasItems = GetCarriersInquiryDataProvider().GetCarrierInquiryModel().Count > 0;
                idRoute = hasItems ? GetCarriersInquiryDataProvider().GetCarrierInquiryModel()[0].Odpoklic.RelacijaID : 0;
                recallID = hasItems ? GetCarriersInquiryDataProvider().GetCarrierInquiryModel()[0].Odpoklic.OdpoklicID : 0;
            }

            if (idRoute > 0)
            {
                List<TenderPositionModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetTenderListByRouteIDAndRecallID(idRoute, recallID));

                if (list == null)
                    list = new List<TenderPositionModel>();

                (sender as ASPxGridLookup).DataSource = list;
            }
        }

        private List<CarrierInquiryModel> GetModel()
        {
            if (model == null)
                return GetCarriersInquiryDataProvider().GetCarrierInquiryModel();

            return model;
        }

        protected void ASPxGridViewCarrierInquiry_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName != "OdstopanjeVEUR") return;

            decimal value = CommonMethods.ParseDecimal(e.CellValue);
            if (value > 0)
                e.Cell.BackColor = ColorTranslator.FromHtml("#88e188");
            else
                e.Cell.BackColor = ColorTranslator.FromHtml("#ff726f");
        }

        protected void ASPxGridViewCarrierInquiry_DataBound(object sender, EventArgs e)
        {
            string potrjen = DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POTRJEN.ToString();
            string prevzet = DatabaseWebService.Common.Enums.Enums.StatusOfRecall.PREVZET.ToString();
            string kreiranPoslan = DatabaseWebService.Common.Enums.Enums.StatusOfRecall.KREIRAN_POSLAN_PDF.ToString();
            string ustvnarocilo = DatabaseWebService.Common.Enums.Enums.StatusOfRecall.USTVARJENO_NAROCILO.ToString();
            string errAdminMail = DatabaseWebService.Common.Enums.Enums.StatusOfRecall.ERR_ADMIN_MAIL.ToString();
            string errOrderNoSend = DatabaseWebService.Common.Enums.Enums.StatusOfRecall.ERR_ORDER_NO_SEND.ToString();

            if (model.Count > 0 && (model[0].Odpoklic.StatusKoda == potrjen || model[0].Odpoklic.StatusKoda == prevzet || model[0].Odpoklic.StatusKoda == kreiranPoslan || model[0].Odpoklic.StatusKoda == ustvnarocilo || model[0].Odpoklic.StatusKoda == errAdminMail || model[0].Odpoklic.StatusKoda == errOrderNoSend ))
                ASPxGridViewCarrierInquiry.Columns["Izberi"].Visible = false;
        }
    }
}