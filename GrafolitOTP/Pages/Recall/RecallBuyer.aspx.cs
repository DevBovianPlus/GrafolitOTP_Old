using DatabaseWebService.ModelsOTP.Recall;
using DevExpress.Web;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Recall
{
    public partial class RecallBuyer : ServerMasterPage
    {
        int recallIDFocusedRowIndex = 0;
        int filterType = 0;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
                recallIDFocusedRowIndex = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());

            if (Request.QueryString[Enums.QueryStringName.filter.ToString()] != null)
                filterType = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.filter.ToString()].ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (recallIDFocusedRowIndex > 0)
                {
                    ASPxGridViewRecall.FocusedRowIndex = ASPxGridViewRecall.FindVisibleIndexByKeyValue(recallIDFocusedRowIndex);
                    ASPxGridViewRecall.ScrollToVisibleIndexOnClient = ASPxGridViewRecall.FindVisibleIndexByKeyValue(recallIDFocusedRowIndex);
                }

                ASPxGridViewRecall.DataBind();
                InitializeEditDeleteButtons();
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            object valueID = ASPxGridViewRecall.GetRowValues(ASPxGridViewRecall.FocusedRowIndex, "OdpoklicID");

            ClearAllSessions(Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList());
            RedirectWithCustomURI("RecallForm.aspx", (int)Enums.UserAction.Edit, valueID);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearAllSessions(Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList());
            Response.Redirect("OrderNOZPDO.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            object valueID = ASPxGridViewRecall.GetRowValues(ASPxGridViewRecall.FocusedRowIndex, "OdpoklicID");
            ClearAllSessions(Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList());

            RedirectWithCustomURI("RecallForm.aspx", (int)Enums.UserAction.Delete, valueID);
        }

        private void InitializeEditDeleteButtons()
        {
            //Check to enable Edit and Delete button for Tab PLAN
            /*if (ASPxGridViewRecall.VisibleRowCount <= 0)
            {
                EnabledDeleteAndEditBtnPopUp(btnEdit, btnDelete);
            }
            else if (!btnEdit.Enabled && !btnDelete.Enabled)
            {
                EnabledDeleteAndEditBtnPopUp(btnEdit, btnDelete, false);
            }*/
        }

        protected void ASPxGridViewRecall_DataBinding(object sender, EventArgs e)
        {
            List<RecallModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetAllNoneTakeOverRecalls());

            if (filterType > 0)
            {
                string statusKoda = "";
                switch (filterType)
                {
                    case 1:
                        statusKoda = DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POTRJEN.ToString();
                        list = list.Where(l => l.StatusKoda == statusKoda).ToList();
                        break;
                    case 2:
                        statusKoda = DatabaseWebService.Common.Enums.Enums.StatusOfRecall.ZAVRNJEN.ToString();
                        list = list.Where(l => l.StatusKoda == statusKoda).ToList();
                        break;
                    case 3:
                        statusKoda = DatabaseWebService.Common.Enums.Enums.StatusOfRecall.V_ODOBRITEV.ToString();
                        list = list.Where(l => l.StatusKoda == statusKoda).ToList();
                        break;
                }
            }


            (sender as ASPxGridView).DataSource = list;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridViewRecall_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] split = e.Parameters.Split(';');
            if (split[0] == "DblClick" && !String.IsNullOrEmpty(split[1]))
            {
                ClearAllSessions(Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("RecallForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
        }

        protected void ASPxGridViewRecall_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID != "Print") return;

            object valueID = ASPxGridViewRecall.GetRowValues(e.VisibleIndex, "OdpoklicID");

            ASPxGridViewRecall.JSProperties["cpPrintID"] = ConcatenateURLForPrint(valueID, "Recall", true);
        }

        protected void ASPxGridViewRecall_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            if (e.GetValue("StatusKoda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.POTRJEN.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#dff0d8");
            else if (e.GetValue("StatusKoda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.V_ODOBRITEV.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#fcf8e3");
            else if (e.GetValue("StatusKoda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.ZAVRNJEN.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2dede");
            else if (e.GetValue("StatusKoda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.PREVZET.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffbf00");
            else if (e.GetValue("StatusKoda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ERR_ADMIN_MAIL.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFA879");
            else if (e.GetValue("StatusKoda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.USTVARJENO_NAROCILO.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#dff0d8");
        }

        protected void ASPxGridViewRecall_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {

            object order = ASPxGridViewRecall.GetRowValues(e.VisibleIndex, "OdpoklicID");




            object item = ASPxGridViewRecall.GetRowValues(e.VisibleIndex, "StatusKoda");
            if (item != null)
            {
                bool isinquiryNotSubmited = item != null ? (item.ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ERR_ADMIN_MAIL.ToString() ? true : false) : false;
                bool orderExist = order != null ? (CommonMethods.ParseInt(order) > 0 ? true : false) : false;

                //if (item.ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.DELNO_PREVZET.ToString() || (item.ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.ERR_ADMIN_MAIL.ToString()) || (item.ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.USTVARJENO_NAROCILO.ToString()) || (item.ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfRecall.ERR_ORDER_NO_SEND.ToString()) && (orderExist))
                //    e.Visible = true;
                //else
                    e.Visible = false;
            }
        }

        protected void btnOpenNewRecall_Click(object sender, EventArgs e)
        {
            List<int> selectedRows = ASPxGridViewRecall.GetSelectedFieldValues("OdpoklicID").OfType<int>().ToList();
            List<string> selectedSuppliers = ASPxGridViewRecall.GetSelectedFieldValues("DobaviteljNaziv").OfType<string>().ToList();

            string firstSelectedSupplier = selectedSuppliers.First();
            bool isAllSuppliersEqual = selectedSuppliers.All(x => x.Equals(firstSelectedSupplier));
            //preverimo če je izbran enak dobavitelj na odpoklicih
            if (!isAllSuppliersEqual)
            {
                ShowClientWarningPopUp("Za odpiranje novega odpoklica izberi identične dobavitelje!");
                ASPxGridViewRecall.Selection.UnselectAll();
                return;
            }

            //pridobimo vse pozicije odpoklica ki še niso prevzete iz izbranih odpoklicev
            List<RecallPositionModel> recallPos = CheckModelValidation(GetDatabaseConnectionInstance().GetRecallPosFromPartialOverTakeRecalls(selectedRows));

            //trenutni id pozicije nastavimo v polje prvotniOdpoklicPozicijeID
            foreach (var item in recallPos)
            {
                item.PrvotniOdpoklicPozicijaID = item.OdpoklicPozicijaID;
                item.OdpoklicPozicijaID = 0;
            }

            RecallFullModel recall = new RecallFullModel();
            recall.OdpoklicPozicija = new List<RecallPositionModel>();
            object obj = ASPxGridViewRecall.GetRowValues(ASPxGridViewRecall.FocusedRowIndex, "DobaviteljPosta", "DobaviteljKraj", "DobaviteljNaslov");

            object[] items = (object[])obj;
            recall.DobaviteljNaziv = firstSelectedSupplier;
            recall.DobaviteljNaslov = items[2].ToString();
            recall.DobaviteljPosta = items[0].ToString();
            recall.DobaviteljKraj = items[1].ToString();
            recall.OdpoklicPozicija = recallPos;

            ClearAllSessions(Enum.GetValues(typeof(Enums.RecallSession)).Cast<Enums.RecallSession>().ToList());
            GetRecallDataProvider().SetRecallFullModel(recall);
            RedirectWithCustomURI("RecallForm.aspx", (int)Enums.UserAction.Add, -1);
        }

        protected void ASPxPopupControlCarriersInquirySummary_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.RecallSession.InquirySummaryRecallID);
        }

        protected void RecallCallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] split = e.Parameter.Split(';');

            if (split[0] == "InquirySummary")
            {
                bool open = SetSessionsAndOpenPopUp("2", Enums.RecallSession.InquirySummaryRecallID, split[1]);
                //ASPxPopupControlCarriersInquirySummary.ShowOnPageLoad = open;
            }
        }

        protected void btnClearStatus_Click(object sender, EventArgs e)
        {
            List<int> valueIDs = ASPxGridViewRecall.GetSelectedFieldValues("OdpoklicID").OfType<int>().ToList();

            if (valueIDs.Count == 1)
            {
                CheckModelValidation(GetDatabaseConnectionInstance().ResetRecallStatusByID(valueIDs[0]));
            }
            ASPxGridViewRecall.Selection.UnselectAll();
            ASPxGridViewRecall.DataBind();
        }

        protected void btnSendOrder_Click(object sender, EventArgs e)
        {
            CheckModelValidation(GetDatabaseConnectionInstance().CreateAndSendOrdersMultiple());
        }
    }
}