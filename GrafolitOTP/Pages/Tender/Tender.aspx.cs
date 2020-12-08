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
using Ionic.Zip;
using DatabaseWebService.Models;
using Newtonsoft.Json;

namespace OptimizacijaTransprotov.Pages.Tender
{
    public partial class Tender : ServerMasterPage
    {
        hlpDateFilterModel helperDTModel = null;
        int selectedTenderID = 0;
        const string UploadDirectory = "~/UploadControl/UploadDocuments/";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            DateTime dateStart = DateTime.Now.AddMonths(-6).Date;
            DateTime dateEnd = DateTime.Now.Date;

            DateEditDatumOd.Date = dateStart;
            DateEditDatumDo.Date = dateEnd;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (helperDTModel == null) helperDTModel = new hlpDateFilterModel();
                helperDTModel.DateFrom = DateEditDatumOd.Date;
                helperDTModel.DateTo = DateEditDatumDo.Date.AddHours(23).AddMinutes(59);


                ASPxGridViewTender.DataBind();
            }
        }

        protected void ASPxGridViewTender_DataBinding(object sender, EventArgs e)
        {
            List<TenderFullModel> tenderList = null;

            tenderList = CheckModelValidation(GetDatabaseConnectionInstance().GetTenderList(helperDTModel.DateFrom.ToString(), helperDTModel.DateTo.ToString()));

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
            else if (e.Parameter.Contains("SelectPozicija;"))
            {
                string[] split = e.Parameter.Split(';');
                int TenderIdx = CommonMethods.ParseInt(split[1]);
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

        
        bool bDifferentSelectedTenderID = false;

        protected void ASPxGridViewTenderPosition_DataBinding(object sender, EventArgs e)
        {
            List<TenderPositionModel> tenderPosList = null;

            tenderPosList = (!bDifferentSelectedTenderID) ? GetTenderDataProvider().GetSelectedTenderPositionRows() : CheckModelValidation(GetDatabaseConnectionInstance().GetTenderListPositionByTenderID(selectedTenderID));

            (sender as ASPxGridView).DataSource = tenderPosList;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;

            GetTenderDataProvider().SetSelectedTenderPositionRowsModel(tenderPosList);
        }

        protected void ASPxGridViewTenderPosition_BeforePerformDataSelect(object sender, EventArgs e)
        {
            selectedTenderID = CommonMethods.ParseInt((sender as ASPxGridView).GetMasterRowKeyValue());

            int iLastSelectedTenderID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.TenderSession.SelectedTenderID));

            if (iLastSelectedTenderID != selectedTenderID)
            {
                AddValueToSession(Enums.TenderSession.SelectedTenderID, selectedTenderID);                
                bDifferentSelectedTenderID = true;
            }
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
                int routeID = 0;
                int cntExit = 0;
                ExcelRouteModel routeModel = null;
                while (true)
                {

                    int tmpID = CommonMethods.ParseInt(sheet.Cells[rowIndex, 0].Value);
                    string routeName = sheet.Cells[rowIndex, 1].Value.ToString();

                    if (routeName.Length > 9)
                    {
                        cntExit = 0;
                        routeModel = new ExcelRouteModel();
                        routeModel.RouteID = tmpID;
                        routeModel.RouteName = routeName;

                    }
                    else
                    {
                        if (routeName.Length != 0)
                        {
                            if (routeModel.TonsList == null) routeModel.TonsList = new List<ExcelTonsModel>();
                            ExcelTonsModel tonsModel = new ExcelTonsModel();
                            tonsModel.ZbirnikTonID = CommonMethods.ParseInt(sheet.Cells[rowIndex, 0].Value);
                            tonsModel.TonsKoda = sheet.Cells[rowIndex, 1].Value.ToString();
                            tonsModel.Price = CommonMethods.ParseDecimal(sheet.Cells[rowIndex, 3].Value);
                            routeModel.TonsList.Add(tonsModel);
                        }
                        else
                        {
                            if (cntExit == 0)
                            {
                                model.ExcelRoutes.Add(routeModel);
                            }
                        }
                    }

                    if (tmpID == 0)
                    {
                        cntExit++;
                        routeID = 0;

                        if (cntExit == 2)
                            break;
                    }

                    rowIndex++;
                    //model.ExcelRoutes.Add(routeModel);
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
                tender.RazpisPozicija = (tender.RazpisPozicija == null) ? (GetTenderDataProvider().GetSelectedTenderPositionRows() != null) ? GetTenderDataProvider().GetSelectedTenderPositionRows() : CheckModelValidation(GetDatabaseConnectionInstance().GetTenderListPositionByTenderID(selectedTenderID)) : tender.RazpisPozicija;
                foreach (var item in tender.RazpisPozicija.Where(rp => rp.StrankaID == model.CarrierID).ToList())
                {
                    ExcelRouteModel erm = model.ExcelRoutes.Where(er => er.RouteID == item.RelacijaID).FirstOrDefault();
                    if (erm != null)
                    {
                        ExcelTonsModel etm = erm.TonsList.Where(et => et.ZbirnikTonID == item.ZbirnikTonID).FirstOrDefault();
                        decimal newPrice = (etm != null) ? etm.Price : 0;
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
                if (tenderF != null && tenderF.PotRazpisa != null && tenderF.PotRazpisa.Length > 0)
                {
                    string sExtension = tenderF.PotRazpisa.Substring(tenderF.PotRazpisa.IndexOf(".") + 1, 3);

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

        protected void ASPxGridViewTender_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            if (e.GetValue("GeneriranTender") != null && CommonMethods.ParseBool(e.GetValue("GeneriranTender")))
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#dff0d8");

        }

        protected void ASPxGridViewTender_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "GeneriranTender")
            {
                if (Convert.ToBoolean(e.Value))
                    e.DisplayText = "DA";
                else
                    e.DisplayText = "NE";
            }
        }

        protected void btnPosljiTender_Click(object sender, EventArgs e)
        {
            int valueID = CommonMethods.ParseInt(ASPxGridViewTender.GetRowValues(ASPxGridViewTender.FocusedRowIndex, "RazpisID"));

            TenderFullModel model = CheckModelValidation(GetDatabaseConnectionInstance().GetTenderByID(valueID));

            if (model != null)
            {
                hlpTenderTransporterSelection vTTModel = IzdelajExcellDatoteke(model);
                hlpTenderCreateExcellData hlp = CheckModelValidation(GetDatabaseConnectionInstance().SendTenderToTransportersEmails(vTTModel.tTenderCreateExcellData));
            }

        }

        private hlpTenderTransporterSelection IzdelajExcellDatoteke(TenderFullModel model)
        {
            hlpTenderTransporterSelection vTTModel = null;


            if (model.PodatkiZaExcell_JSon != null && model.PodatkiZaExcell_JSon.Length > 0)
            {
                vTTModel = JsonConvert.DeserializeObject<hlpTenderTransporterSelection>(model.PodatkiZaExcell_JSon);

                string path = Server.MapPath("~");

                if (!Directory.Exists(path + "Razpisi"))
                    path = Directory.CreateDirectory(path + "Razpisi").FullName;

                Workbook workbook = null;
                int rowIndex = 0;

                //Če še ni mape razpisi za trenutni dan se ustvari in zapiše pot v spremenljivko razpisiPath
                string razpisiPath = Server.MapPath("~/Razpisi/");
                if (!Directory.Exists(razpisiPath + "Razpisi_" + DateTime.Now.ToString("dd-MM-yyyy")))
                    razpisiPath = Directory.CreateDirectory(razpisiPath + "Razpisi_" + DateTime.Now.ToString("dd-MM-yyyy")).FullName;

                razpisiPath = Server.MapPath("~/Razpisi/Razpisi_" + DateTime.Now.ToString("dd-MM-yyyy") + "/");

                //uporabimi če se bo generiral samo en razpis potem ne rabimo prenašat zip datoteke k uporabniku ampak samo xlsx 
                string currentFullFileName = "";
                string currentFileName = "";
                string zipFileName = "Razpisi_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".zip";
                string sFileLocation = razpisiPath + zipFileName;
                CommonMethods.LogThis("zipFileName: " + zipFileName);
                using (ZipFile zip = new ZipFile(razpisiPath + zipFileName))
                {

                    foreach (TransporterSimpleModel tsm in vTTModel.tTenderCreateExcellData.TransporterList)
                    {
                        workbook = new Workbook();
                        string wsName = tsm.Naziv.ToString().Length > 30 ? tsm.Naziv.ToString().Substring(0, 30) : tsm.Naziv.ToString();
                        workbook.Worksheets[0].Name = CommonMethods.RemoveForbidenChracters(wsName);
                        workbook.Worksheets[0].MergeCells(workbook.Worksheets[0].Range["B1:C1"]);
                        workbook.Worksheets[0].Cells[0, 0].Value = vTTModel.tTenderCreateExcellData._TenderModel.RazpisID.ToString();//prvi stolpec shranjujemo ID-je
                        workbook.Worksheets[0].Cells[0, 1].Value = model.Naziv;
                        workbook.Worksheets[0].Cells[0, 1].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                        workbook.Worksheets[0].Cells[0, 1].Font.Bold = true;
                        workbook.Worksheets[0].Cells[1, 0].Value = tsm.ClientID.ToString();
                        workbook.Worksheets[0].Cells[1, 0].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                        workbook.Worksheets[0].Cells[1, 1].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                        workbook.Worksheets[0].Cells[1, 1].Font.Bold = true;
                        workbook.Worksheets[0].Cells[1, 1].Value = tsm.Naziv.ToString();
                        workbook.Worksheets[0].Cells[1, 2].Font.Bold = true;
                        workbook.Worksheets[0].Cells[1, 2].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                        workbook.Worksheets[0].Cells[1, 2].Value = "Ciljna cena";
                        workbook.Worksheets[0].Cells[1, 3].Font.Bold = true;
                        workbook.Worksheets[0].Cells[1, 3].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                        workbook.Worksheets[0].Cells[1, 3].Value = "Cena";
                        workbook.Worksheets[0].Cells[1, 4].Font.Bold = true;
                        workbook.Worksheets[0].Cells[1, 4].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                        workbook.Worksheets[0].Cells[1, 4].Value = "Št. VAŠIH prevozev v zadnjem letu";
                        workbook.Worksheets[0].Cells[1, 5].Font.Bold = true;
                        workbook.Worksheets[0].Cells[1, 5].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                        workbook.Worksheets[0].Cells[1, 5].Value = "Št. vseh prevozev za relacijo v zadnjem letu";
                        workbook.Worksheets[0].Cells[1, 1].AutoFitColumns();
                        workbook.Worksheets[0].Cells[1, 1].AutoFitRows();

                        rowIndex = 3;
                        foreach (RouteSimpleModel rsm in tsm.RouteList)
                        {
                            workbook.Worksheets[0].Cells[rowIndex, 0].Value = rsm.RouteID.ToString();
                            workbook.Worksheets[0].Cells[rowIndex, 1].Value = rsm.Naziv.ToString();
                            workbook.Worksheets[0].Cells[rowIndex, 1].ColumnWidth = 200;
                            workbook.Worksheets[0].Cells[rowIndex, 1].Font.Bold = true;
                            workbook.Worksheets[0].Cells[rowIndex, 1].Font.Italic = true;

                            //var value = transportCountList.Where(tcl => tcl.PrevoznikID == (int)carrierList[0] && tcl.RelacijaID == (int)routeList[0]).FirstOrDefault();
                            // 08.06.2020 - ugotovil da tega sploh ne uporabljajo in smo enostavno zakomentirali
                            //var value = CheckModelValidation(GetDatabaseConnectionInstance().GetTransportCounByTransporterIDAndRouteID(new TransportCountModel() { PrevoznikID = (int)carrierList[0], RelacijaID = (int)routeList[0] }).Result);

                            //workbook.Worksheets[0].Cells[rowIndex, 2].Value = dCiljnaCena;
                            workbook.Worksheets[0].Cells[rowIndex, 4].Value = rsm.SteviloPrevozVLetuNaRelacijoPrevoznik.ToString();
                            workbook.Worksheets[0].Cells[rowIndex, 5].Value = rsm.SteviloPrevozVLetuNaRelacijoVsiPrevozniki.ToString();
                            //workbook.Worksheets[0].Cells[rowIndex, 5].Value = tsm.RouteList != null ? CommonMethods.ParseInt(routeList[2]) : 0;
                            rowIndex++;
                            if (rsm.TonsList.Count > 0)
                            {
                                foreach (TonsModel sTons in rsm.TonsList)
                                {
                                    workbook.Worksheets[0].Cells[rowIndex, 0].Value = sTons.ZbirnikTonID.ToString();
                                    workbook.Worksheets[0].Cells[rowIndex, 1].Value = sTons.Naziv.ToString();
                                    workbook.Worksheets[0].Cells[rowIndex, 2].Value = model.CiljnaCena;
                                    workbook.Worksheets[0].Cells[rowIndex, 3].FillColor = Color.LightGreen;
                                    workbook.Worksheets[0].Cells[rowIndex, 3].Protection.Locked = false;

                                    // nastavimo še podatek v razpis pozicija 


                                    rowIndex++;
                                }
                            }

                            rowIndex++;


                        }
                        workbook.Worksheets[0].Columns.AutoFit(0, 5);
                        workbook.Worksheets[0].Columns[2].Visible = model.IsCiljnaCena;
                        workbook.Worksheets[0].Protect("123", WorksheetProtectionPermissions.Default);

                        currentFileName = CommonMethods.RemoveForbidenChracters(tsm.Naziv.ToString()).Replace(" ", "_").Replace(".", "") + "_" + DateTime.Now.Ticks.ToString() + "_Razpis.xls";
                        currentFullFileName = razpisiPath + currentFileName;
                        // dodamo ime dattotek v pozicijo in v session tdm
                        tsm.ExcellFilePath = currentFullFileName;

                        model.PotRazpisa = tsm.ExcellFilePath;

                        //List<TenderPositionModel> lTenderPositionModel = tender.RazpisPozicija.Where(rp => rp.StrankaID == tsm.ClientID).ToList();
                        //if (lTenderPositionModel != null)
                        //{
                        //    foreach (TenderPositionModel tpm in lTenderPositionModel)
                        //    {
                        //        tpm.PotDokumenta = tsm.ExcellFilePath;
                        //    }
                        //}


                        workbook.Worksheets[0].Columns[0].Visible = false;
                        CommonMethods.LogThis("Ime in pot datoteke: " + currentFullFileName);
                        workbook.SaveDocument(currentFullFileName, DocumentFormat.OpenXml);

                        zip.AddFile(currentFullFileName, "");
                    }

                    zip.Save();

                    model.GeneriranTender = true;
                    model.RazpisKreiran = true;
                    model.PotRazpisa = sFileLocation;

                    var objTender2 = CheckModelValidation(GetDatabaseConnectionInstance().SaveTender(model));

                    vTTModel.ZipFilePath = sFileLocation;
                }

            }

            return vTTModel;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (helperDTModel == null) helperDTModel = new hlpDateFilterModel();
            helperDTModel.DateFrom = DateEditDatumOd.Date;
            helperDTModel.DateTo = DateEditDatumDo.Date.AddHours(23).AddMinutes(59); ;

            ASPxGridViewTender.DataBind();
        }

        protected void btnPrenosTender_Click(object sender, EventArgs e)
        {
            int valueID = CommonMethods.ParseInt(ASPxGridViewTender.GetRowValues(ASPxGridViewTender.FocusedRowIndex, "RazpisID"));
            TenderFullModel model = CheckModelValidation(GetDatabaseConnectionInstance().GetTenderByID(valueID));
            hlpTenderTransporterSelection vTTModel = null;

            if (model != null)
            {
                if (model.PotRazpisa == null && model.PodatkiZaExcell_JSon != null)
                {
                    vTTModel = IzdelajExcellDatoteke(model);
                }
                else
                {
                    vTTModel = new hlpTenderTransporterSelection();
                    vTTModel.ZipFilePath = model.PotRazpisa;
                }

                DownloadTenderDataModel tdm = new DownloadTenderDataModel();

                byte[] byteFile = File.ReadAllBytes(vTTModel.ZipFilePath);
                tdm.ByteData = byteFile;
                tdm.FileExtension = "zip";
                tdm.FileName = model.PotRazpisa.Substring(model.PotRazpisa.LastIndexOf("\\") + 1, model.PotRazpisa.Length - model.PotRazpisa.LastIndexOf("\\") - 1);
                tdm.IsInline = false;


                CommonMethods.WriteDocumentToResponse(this, tdm.ByteData, tdm.FileExtension, tdm.IsInline, tdm.FileName);

                RemoveSession(Enums.TenderSession.DownloadTenderData);


            }

        }
    }
}