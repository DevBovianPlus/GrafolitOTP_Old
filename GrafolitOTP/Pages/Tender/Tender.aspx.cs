using DatabaseWebService.ModelsOTP.Tender;
using DevExpress.Web;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Spreadsheet;
using OptimizacijaTransprotov.Helpers.Models;
using System.IO;
using System.Drawing;
using System.Reflection;
using DevExpress.Web.Data;
using System.Collections;

namespace OptimizacijaTransprotov.Pages.Tender
{
    public partial class Tender : ServerMasterPage
    {

        int selectedTenderID = 0;
        const string UploadDirectory = "~/UploadControl/UploadDocuments/";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ASPxGridViewTender.DataBind();
            }
        }

        protected void ASPxGridViewTender_DataBinding(object sender, EventArgs e)
        {
            List<TenderFullModel> tenderList = CheckModelValidation(GetDatabaseConnectionInstance().GetTenderList());
            Session["TenderList"] = tenderList;
            (sender as ASPxGridView).DataSource = tenderList;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridViewTender_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] split = e.Parameters.Split(';');
            if (split[0] == "DblClick" && !String.IsNullOrEmpty(split[1]))
                ASPxWebControl.RedirectOnCallback(GenerateURI("TenderForm.aspx", (int)Enums.UserAction.Edit, split[1]));
        }

        protected void ASPxPopupControlTender_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            if (e.Parameter == "ClosePopupButtonClick")//if the user click on close button on popup we have to clear sessions
            {
                RemoveSession(Enums.CommonSession.UserActionPopUp);
                RemoveSession(Enums.TenderSession.TenderFullModel);
                RemoveSession(Enums.TenderSession.TenderID);
            }
        }

        protected void CallbackPanelTender_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "RefreshGrid")
            {
                ASPxGridViewTender.DataBind();
            }
            else if (e.Parameter == "UploadExcel")
            {
                TenderFullModel tender = ((List<TenderFullModel>)Session["TenderList"]).Where(t => t.RazpisID == selectedTenderID).FirstOrDefault();
                if (tender != null)
                    tender = CheckModelValidation(GetDatabaseConnectionInstance().SaveTenderAndTenderPosition(tender));
                //RemoveSession("TenderList");
                RemoveSession("GridViewTenderValues");
            }
            else if (e.Parameter == "AddNewTenderPosition")
            {
                AddValueToSession(Enums.CommonSession.UserActionPopUp, ((int)Enums.UserAction.Add).ToString());
                AddValueToSession(Enums.TenderSession.TenderID, selectedTenderID);

                ASPxPopupControlTender.ShowOnPageLoad = true;
            }
            else if (e.Parameter.Contains("DeleteSelected;"))
            {
                string[] split = e.Parameter.Split(';');
                List<int> deletedID = split[1].Split(',').ToList().ConvertAll(int.Parse);
                CheckModelValidation(GetDatabaseConnectionInstance().DeleteTenderPos(deletedID));
                ASPxGridViewTender.DataBind();
            }
            else
            {
                object valueID = null;
                if (ASPxGridViewTender.VisibleRowCount > 0)
                    valueID = ASPxGridViewTender.GetRowValues(ASPxGridViewTender.FocusedRowIndex, "RazpisID");

                bool isValid = SetSessionsAndOpenPopUp(e.Parameter, Enums.TenderSession.TenderID, valueID);
                if (isValid)
                    ASPxPopupControlTender.ShowOnPageLoad = true;
            }
        }

        protected void ASPxGridViewTenderPosition_DataBinding(object sender, EventArgs e)
        {
            List<TenderFullModel> tenderList = (List<TenderFullModel>)Session["TenderList"];

            if (tenderList != null && tenderList.Count > 0)
            {
                List<TenderPositionModel> tenderPosList = tenderList.Where(t => t.RazpisID == selectedTenderID).FirstOrDefault().RazpisPozicija;
                (sender as ASPxGridView).DataSource = tenderPosList;
                (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
            }

        }

        protected void ASPxGridViewTenderPosition_BeforePerformDataSelect(object sender, EventArgs e)
        {
            selectedTenderID = CommonMethods.ParseInt((sender as ASPxGridView).GetMasterRowKeyValue());

        }

        #region Export from Excel
        private int GetTenderIDFromUploadExcel(Worksheet sheet)
        {
            int tenderID = CommonMethods.ParseInt(sheet.Cells[0, 0].Value);
            if (tenderID <= 0) throw new ExcelException("V excel datotkeki manjkajo podatki! Ni identifikacijske številke razpisa.");

            return tenderID;
        }

        private string GetTenderNameFromUploadExcel(Worksheet sheet)
        {
            string tenderName = sheet.Cells[0, 1].Value.ToString();
            if (String.IsNullOrEmpty(tenderName)) throw new ExcelException("V excel datotkeki manjkajo podatki! Ni naziva razpisa.");
            return tenderName;
        }

        private int GetCarrierIDFromUploadExcel(Worksheet sheet)
        {
            int carrierID = CommonMethods.ParseInt(sheet.Cells[1, 0].Value);
            if (carrierID <= 0) throw new ExcelException("V excel datotkeki manjkajo podatki! Ni identifikacijske številke prevoznika.");
            return carrierID;
        }

        private string GetCarrierNameFromUploadExcel(Worksheet sheet)
        {
            return sheet.Cells[1, 1].Value.ToString();
        }

        public ExcelDataModel ExportDataFromExcel(Worksheet sheet, ref string excelError)
        {
            ExcelDataModel model = new ExcelDataModel();

            try
            {
                model.TenderID = GetTenderIDFromUploadExcel(sheet);
                model.TenderName = GetTenderNameFromUploadExcel(sheet);

                model.CarrierID = GetCarrierIDFromUploadExcel(sheet);
                model.CarrierName = GetCarrierNameFromUploadExcel(sheet);

                model.ExcelRoutes = new List<ExcelRouteModel>();

                int rowIndex = 3;

                while (true)
                {
                    ExcelRouteModel routeModel = new ExcelRouteModel();
                    int routeID = CommonMethods.ParseInt(sheet.Cells[rowIndex, 0].Value);
                    if (routeID > 0)
                    {
                        routeModel.RouteID = routeID;
                        routeModel.RouteName = sheet.Cells[rowIndex, 1].Value.ToString();
                        routeModel.Price = CommonMethods.ParseDecimal(sheet.Cells[rowIndex, 2].Value);
                    }
                    else
                        break;

                    rowIndex++;
                    model.ExcelRoutes.Add(routeModel);
                }

                return model;
            }
            catch (ExcelException ex)
            {
                excelError += ex.Message;
                return null;
            }
        }
        #endregion

        protected void DocumentsUploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            e.CallbackData = e.UploadedFile.FileName;
            string path = Server.MapPath(UploadDirectory);
            string excelError = "";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string resultFileUrl = UploadDirectory + e.UploadedFile.FileName;
            string resultFilePath = Server.MapPath(resultFileUrl);

            Workbook wb = new Workbook();
            wb.LoadDocument(e.UploadedFile.FileContent, DocumentFormat.OpenXml);

            ExcelDataModel model = ExportDataFromExcel(wb.Worksheets[0], ref excelError);
            if (!String.IsNullOrEmpty(excelError))
            {
                e.ErrorText += excelError;
                return;
            }


            List<TenderFullModel> tenderList = (List<TenderFullModel>)Session["TenderList"];
            if (selectedTenderID == model.TenderID)
            {
                e.UploadedFile.SaveAs(resultFilePath);
                List<GridViewTenderPosValues> values = new List<GridViewTenderPosValues>();
                GridViewTenderPosValues val = null;
                TenderFullModel tender = tenderList.Where(t => t.RazpisID == model.TenderID).FirstOrDefault();
                foreach (var item in tender.RazpisPozicija.Where(rp => rp.StrankaID == model.CarrierID).ToList())
                {
                    decimal newPrice = model.ExcelRoutes.Where(er => er.RouteID == item.RelacijaID).FirstOrDefault().Price;
                    if (item.Cena != newPrice)
                    {
                        val = new GridViewTenderPosValues();
                        val.FieldName = "Cena";
                        val.KeyValue = item.RazpisPozicijaID;
                        val.NewValue = newPrice;
                        val.OldValue = item.Cena;

                        item.Cena = newPrice;

                        values.Add(val);
                    }
                }
                Session["GridViewTenderValues"] = values;
            }
            else
                e.ErrorText += "Naložene cene so napačne za izbrani razpis!";
        }

        protected void ASPxGridViewTenderPosition_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            List<GridViewTenderPosValues> values = (List<GridViewTenderPosValues>)Session["GridViewTenderValues"];
            if (values != null && values.Count > 0)
            {
                int tenderPosID = CommonMethods.ParseInt(e.KeyValue);
                GridViewTenderPosValues val = values.Where(v => v.KeyValue == tenderPosID).FirstOrDefault();
                if (val != null && (val.OldValue != val.NewValue))
                    e.Row.BackColor = Color.LightGreen;
            }
        }

        protected void ASPxGridViewTenderPosition_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            List<TenderFullModel> tenderList = (List<TenderFullModel>)Session["TenderList"];

            if (tenderList != null && tenderList.Count > 0)
            {
                TenderFullModel model = tenderList.Where(t => t.RazpisID == selectedTenderID).FirstOrDefault();
                List<TenderPositionModel> tenderPosList = model.RazpisPozicija;
                TenderPositionModel tenderPosModel = null;
                List<TenderPositionModel> tenderPosListToSave = new List<TenderPositionModel>();

                Type myType = typeof(TenderPositionModel);
                List<PropertyInfo> myPropInfo = myType.GetProperties().ToList();


                foreach (ASPxDataUpdateValues item in e.UpdateValues)
                {
                    tenderPosModel = new TenderPositionModel();

                    foreach (DictionaryEntry obj in item.Keys)//we set table ID
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                        {
                            tenderPosModel = tenderPosList.Where(r => r.RazpisPozicijaID == (int)obj.Value).FirstOrDefault();
                            break;
                        }
                    }

                    foreach (DictionaryEntry obj in item.NewValues)
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                        {
                            info.SetValue(tenderPosModel, obj.Value);
                            tenderPosListToSave.Add(tenderPosModel);
                        }
                    }
                }

                model.RazpisPozicija = tenderPosListToSave;
                CheckModelValidation(GetDatabaseConnectionInstance().SaveTenderAndTenderPosition(model));
                model.RazpisPozicija = tenderPosList;
            }
            e.Handled = true;
        }

    
        protected void ASPxGridViewTender_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        protected void btnDownloadTender_Click(object sender, EventArgs e)
        {
            //ASPxButton btn = sender as ASPxButton;
            //GridViewDetailRowTemplateContainer container = btn.NamingContainer.NamingContainer as GridViewDetailRowTemplateContainer;
            //int ind = ((GridViewDetailRowTemplateContainer)btn.NamingContainer.NamingContainer).VisibleIndex;
            //ASPxFormLayout f1 = ASPxGridViewTender.FindDetailRowTemplateControl(ind, "FormLayout") as ASPxFormLayout;

            List<TenderFullModel> tenderList = (List<TenderFullModel>)Session["TenderList"];
            if (selectedTenderID > 0)
            {
                TenderFullModel tenderF = tenderList.Where(t => t.RazpisID == selectedTenderID).FirstOrDefault();
                if (tenderF != null && tenderF.PotRazpisa != null && tenderF.PotRazpisa.Length>0)
                {
                    string sExtension = tenderF.PotRazpisa.Substring(tenderF.PotRazpisa.IndexOf(".")+1,3);

                    string[] split = tenderF.PotRazpisa.Split('\\');
                    string sFileName = split[split.Length - 1];
                    byte[] bytes = CheckModelValidation(GetDatabaseConnectionInstance().GetTenderDownloadFile(tenderF.RazpisID));
                    if (bytes != null)
                    {
                        CommonMethods.WriteDocumentToResponse(this, bytes, sExtension, false, sFileName);
                    }
                }
            }

        }
    }
}