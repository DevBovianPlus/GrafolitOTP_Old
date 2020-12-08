using DatabaseWebService.Models;
using DatabaseWebService.ModelsOTP.Route;
using DatabaseWebService.ModelsOTP.Tender;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Drawings;
using DevExpress.Web;
using Ionic.Zip;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Helpers.Models;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Tender
{
    public partial class SendTender : ServerMasterPage
    {

        hlpViewRoutePricesModel helperRPModel = null;
        List<RouteModel> lFilteredRoutes = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ASPxGridViewCarrier.DataBind();
                ASPxGridViewRoutes.DataBind();
                ASPxGridViewTons.DataBind();
                DateEditDatumRazpisa.Date = DateTime.Now;

                //int idx = ASPxGridViewTons.FindVisibleIndexByKeyValue(10);
                ASPxGridViewTons.Selection.SelectRowByKey(10);
            }
        }

        protected void ASPxGridViewRoutes_DataBinding(object sender, EventArgs e)
        {
            if (helperRPModel == null) helperRPModel = new hlpViewRoutePricesModel();
            helperRPModel.DateFrom = DateTime.Now.AddYears(-1);
            helperRPModel.DateTo = DateTime.Now;
            helperRPModel.iWeightType = 0;
            helperRPModel.iViewType = 1;

            lFilteredRoutes = ((GetRouteDataProvider().GetFilteredRoutesForTender() == null) ? CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutesTransportPricesByViewType(helperRPModel)).lRouteList : GetRouteDataProvider().GetFilteredRoutesForTender());
            lFilteredRoutes = lFilteredRoutes.OrderByDescending(fr => fr.RecallCount).ToList();
            (sender as ASPxGridView).DataSource = lFilteredRoutes;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }


        protected void ASPxGridViewTons_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = CheckModelValidation(GetDatabaseConnectionInstance().GetAllTons());
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridViewCarrier_DataBinding(object sender, EventArgs e)
        {
            string prevoznik = DatabaseWebService.Common.Enums.Enums.TypeOfClient.PREVOZNIK.ToString();
            (sender as ASPxGridView).DataSource = CheckModelValidation(GetDatabaseConnectionInstance().GetAllClients(prevoznik));
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void CallbackPanelSendTenders_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "OpenPopUp")
            {
                // pridobimo najcenejšo ceno za prvo izbrano relacijo
                List<object> selectedRowsRoutes = ASPxGridViewRoutes.GetSelectedFieldValues("RelacijaID", "Naziv");

                foreach (var routeItem in selectedRowsRoutes)
                {
                    IList routeList = (IList)routeItem;
                    int iRouteID = CommonMethods.ParseInt(routeList[0]);
                    decimal dLowestPrice = CheckModelValidation(GetDatabaseConnectionInstance().GetLowestAndMostRecentPriceByRouteID(iRouteID));
                    if (dLowestPrice != 0)
                    {
                        txtCiljnaCena.Text = dLowestPrice.ToString();
                        break;
                    }
                }

                ASPxPopupCompleteTenderData.ShowOnPageLoad = true;
            }
            else
            {
                ASPxGridViewRoutes.Selection.UnselectAll();

                int iViewType = CommonMethods.ParseInt(RadioButtonList.Value.ToString());

                if (helperRPModel == null) helperRPModel = new hlpViewRoutePricesModel();

                helperRPModel.DateFrom = DateTime.Now.AddYears(-1);
                helperRPModel.DateTo = DateTime.Now;
                helperRPModel.iWeightType = 0;
                helperRPModel.iViewType = iViewType;

                hlpViewRoutePricesModel vRPModel = CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutesTransportPricesByViewType(helperRPModel));
                lFilteredRoutes = vRPModel.lRouteList;
                GetRouteDataProvider().SetFilteredRoutesForTender(lFilteredRoutes);
                ASPxGridViewRoutes.DataBind();
            }

        }

        protected void ASPxPopupCompleteTenderData_WindowCallback(object source, PopupWindowCallbackArgs e)
        {

        }

        //private void WriteDocumentToResponse( Page pCurrentPage,  byte[] documentData, string format, bool isInline, string fileName)
        //{
        //    try
        //    {
        //        string contentType = "application/pdf";

        //        if (format == "png")
        //            contentType = "image/png";
        //        else if (format == "jpg" || format == "jpeg")
        //            contentType = "image/jpeg";
        //        else if (format == "xls")
        //            contentType = "application/xls";
        //        else if (format == "zip")
        //            contentType = "application/zip";
        //        else
        //            contentType = "application/octet-stream";

        //        string disposition = (isInline) ? "inline" : "attachment";

        //        CommonMethods.LogThis("Before dowload");
        //        Response.Clear();
        //        Response.ContentType = contentType;

        //        Response.ClearHeaders();

        //        CommonMethods.LogThis("Before Add header");
        //        Response.AddHeader("Content-Disposition", String.Format("{0}; filename={1}", disposition, fileName));
        //        Response.AddHeader("Content-Length", documentData.Length.ToString());

        //        Response.Clear(); // dodal boris - 21.02.2019       
        //        Response.BufferOutput = false;
        //        Response.ClearContent();

        //        CommonMethods.LogThis("Before Binnarywrite");
        //        Response.BinaryWrite(documentData);

        //        Response.Flush();

        //        //Response.Close();
        //        //Response.End();

        //        Response.SuppressContent = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonMethods.LogThis(ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace);

        //    }
        //}

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                decimal dCiljnaCena = CommonMethods.ParseDecimal(txtCiljnaCena.Text);

                List<object> selectedRowsRoutes = ASPxGridViewRoutes.GetSelectedFieldValues("RelacijaID", "Naziv", "RecallCount");
                List<object> selectedRowsCarriers = ASPxGridViewCarrier.GetSelectedFieldValues("idStranka", "NazivPrvi");
                List<object> selectedRowsTons = ASPxGridViewTons.GetSelectedFieldValues("ZbirnikTonID", "Naziv");

                TenderFullModel tender = new TenderFullModel();
                tender.CenaSkupaj = 0;
                tender.DatumRazpisa = DateEditDatumRazpisa.Date;
                tender.Naziv = txtTenderName.Text;
                tender.RazpisID = 0;
                tender.CiljnaCena = dCiljnaCena;
                tender.IsCiljnaCena = CommonMethods.ParseBool(chkCiljnaCena.Checked);
                tender.IsNajcenejsiPrevoznik = CommonMethods.ParseBool(chkNajcenejsiPrevoznik.Checked);
                tender.tsIDOseba = PrincipalHelper.GetUserPrincipal().ID;
                List<TenderPositionModel> tenderPositionsToSave = new List<TenderPositionModel>();
                List<TransportCountModel> transportCountList = new List<TransportCountModel>();

                var objTender = CheckModelValidation(GetDatabaseConnectionInstance().SaveTender(tender));

                // podatki za preverjenje 
                // če ne gre za najcenejšega prevoznika ga izločimo iz seznama 
                hlpTenderTransporterSelection vTTModel = new hlpTenderTransporterSelection();
                vTTModel.SelectedRowsRoutes = selectedRowsRoutes;
                vTTModel.SelectedRowsCarriers = selectedRowsCarriers;
                vTTModel.SelectedRowsTons = selectedRowsTons;
                vTTModel.CheapestTransporterTender = CommonMethods.ParseBool(chkNajcenejsiPrevoznik.Checked);
                vTTModel.RazpisPozicija = tenderPositionsToSave;

                if (vTTModel.tTenderCreateExcellData == null) vTTModel.tTenderCreateExcellData = new hlpTenderCreateExcellData();

                vTTModel.tTenderCreateExcellData._TenderModel = objTender;
                


                Task.Run(() =>
                {
                    var output = GetDatabaseConnectionInstance().PrepareDataForTenderTransport(vTTModel);
                });
                
                tenderPositionsToSave = vTTModel.RazpisPozicija;
                //Server.ScriptTimeout = 1200;

                // transportCountList = CheckModelValidation(GetDatabaseConnectionInstance().GetTransportCounByTransporterAndRoute(transportCountList));

                //tender.RazpisPozicija = tenderPositionsToSave;
                //CallbackPanelSendTenders.JSProperties["cpSendTender"] = tenderPositionsToSave.Count.ToString();
                ////CommonMethods.LogThis("Before Save 1");
                //var objTender = CheckModelValidation(GetDatabaseConnectionInstance().SaveTender(tender));
                ////CommonMethods.LogThis("After Save 1");
                ////CommonMethods.LogThis("After objTender.RazpisID" + objTender.RazpisID);
                //tender.RazpisID = objTender.RazpisID;
                ////CommonMethods.LogThis("Before Save 2");
                //tender = CheckModelValidation(GetDatabaseConnectionInstance().SaveTenderAndTenderPosition(tender));
                ////CommonMethods.LogThis("Ater Save 2");


                //string path = Server.MapPath("~");

                //if (!Directory.Exists(path + "Razpisi"))
                //    path = Directory.CreateDirectory(path + "Razpisi").FullName;

                //Workbook workbook = null;
                //int rowIndex = 0;

                ////Če še ni mape razpisi za trenutni dan se ustvari in zapiše pot v spremenljivko razpisiPath
                //string razpisiPath = Server.MapPath("~/Razpisi/");
                //if (!Directory.Exists(razpisiPath + "Razpisi_" + DateTime.Now.ToString("dd-MM-yyyy")))
                //    razpisiPath = Directory.CreateDirectory(razpisiPath + "Razpisi_" + DateTime.Now.ToString("dd-MM-yyyy")).FullName;

                //razpisiPath = Server.MapPath("~/Razpisi/Razpisi_" + DateTime.Now.ToString("dd-MM-yyyy") + "/");

                ////uporabimi če se bo generiral samo en razpis potem ne rabimo prenašat zip datoteke k uporabniku ampak samo xlsx 
                //string currentFullFileName = "";
                //string currentFileName = "";
                //string zipFileName = "Razpisi_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".zip";
                //string sFileLocation = razpisiPath + zipFileName;
                //CommonMethods.LogThis("zipFileName: " + zipFileName);
                //using (ZipFile zip = new ZipFile(razpisiPath + zipFileName))
                //{

                //    foreach (TransporterSimpleModel tsm in vTTModel.tTenderCreateExcellData.TransporterList)
                //    {
                //        workbook = new Workbook();
                //        string wsName = tsm.Naziv.ToString().Length > 30 ? tsm.Naziv.ToString().Substring(0, 30) : tsm.Naziv.ToString();
                //        workbook.Worksheets[0].Name =  CommonMethods.RemoveForbidenChracters(wsName);
                //        workbook.Worksheets[0].MergeCells(workbook.Worksheets[0].Range["B1:C1"]);
                //        workbook.Worksheets[0].Cells[0, 0].Value = vTTModel.tTenderCreateExcellData._TenderModel.RazpisID.ToString();//prvi stolpec shranjujemo ID-je
                //        workbook.Worksheets[0].Cells[0, 1].Value = txtTenderName.Text;
                //        workbook.Worksheets[0].Cells[0, 1].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //        workbook.Worksheets[0].Cells[0, 1].Font.Bold = true;
                //        workbook.Worksheets[0].Cells[1, 0].Value = tsm.ClientID.ToString();
                //        workbook.Worksheets[0].Cells[1, 0].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                //        workbook.Worksheets[0].Cells[1, 1].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                //        workbook.Worksheets[0].Cells[1, 1].Font.Bold = true;
                //        workbook.Worksheets[0].Cells[1, 1].Value = tsm.Naziv.ToString();
                //        workbook.Worksheets[0].Cells[1, 2].Font.Bold = true;
                //        workbook.Worksheets[0].Cells[1, 2].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                //        workbook.Worksheets[0].Cells[1, 2].Value = "Ciljna cena";
                //        workbook.Worksheets[0].Cells[1, 3].Font.Bold = true;
                //        workbook.Worksheets[0].Cells[1, 3].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                //        workbook.Worksheets[0].Cells[1, 3].Value = "Cena";
                //        workbook.Worksheets[0].Cells[1, 4].Font.Bold = true;
                //        workbook.Worksheets[0].Cells[1, 4].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                //        workbook.Worksheets[0].Cells[1, 4].Value = "Št. VAŠIH prevozev v zadnjem letu";
                //        workbook.Worksheets[0].Cells[1, 5].Font.Bold = true;
                //        workbook.Worksheets[0].Cells[1, 5].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                //        workbook.Worksheets[0].Cells[1, 5].Value = "Št. vseh prevozev za relacijo v zadnjem letu";
                //        workbook.Worksheets[0].Cells[1, 1].AutoFitColumns();
                //        workbook.Worksheets[0].Cells[1, 1].AutoFitRows();

                //        rowIndex = 3;
                //        foreach (RouteSimpleModel rsm in tsm.RouteList)
                //        {
                //            workbook.Worksheets[0].Cells[rowIndex, 0].Value = rsm.RouteID.ToString();
                //            workbook.Worksheets[0].Cells[rowIndex, 1].Value = rsm.Naziv.ToString();
                //            workbook.Worksheets[0].Cells[rowIndex, 1].ColumnWidth = 200;
                //            workbook.Worksheets[0].Cells[rowIndex, 1].Font.Bold = true;
                //            workbook.Worksheets[0].Cells[rowIndex, 1].Font.Italic = true;

                //            //var value = transportCountList.Where(tcl => tcl.PrevoznikID == (int)carrierList[0] && tcl.RelacijaID == (int)routeList[0]).FirstOrDefault();
                //            // 08.06.2020 - ugotovil da tega sploh ne uporabljajo in smo enostavno zakomentirali
                //            //var value = CheckModelValidation(GetDatabaseConnectionInstance().GetTransportCounByTransporterIDAndRouteID(new TransportCountModel() { PrevoznikID = (int)carrierList[0], RelacijaID = (int)routeList[0] }).Result);

                //            //workbook.Worksheets[0].Cells[rowIndex, 2].Value = dCiljnaCena;
                //            workbook.Worksheets[0].Cells[rowIndex, 4].Value = rsm.SteviloPrevozVLetuNaRelacijoPrevoznik.ToString();
                //            workbook.Worksheets[0].Cells[rowIndex, 5].Value = rsm.SteviloPrevozVLetuNaRelacijoVsiPrevozniki.ToString();
                //            //workbook.Worksheets[0].Cells[rowIndex, 5].Value = tsm.RouteList != null ? CommonMethods.ParseInt(routeList[2]) : 0;
                //            rowIndex++;
                //            if (rsm.TonsList.Count > 0)
                //            {
                //                foreach (TonsModel sTons in rsm.TonsList)
                //                {
                //                    workbook.Worksheets[0].Cells[rowIndex, 0].Value = sTons.ZbirnikTonID.ToString();
                //                    workbook.Worksheets[0].Cells[rowIndex, 1].Value = sTons.Naziv.ToString();
                //                    workbook.Worksheets[0].Cells[rowIndex, 2].Value = dCiljnaCena;
                //                    workbook.Worksheets[0].Cells[rowIndex, 3].FillColor = Color.LightGreen;
                //                    workbook.Worksheets[0].Cells[rowIndex, 3].Protection.Locked = false;

                //                    // nastavimo še podatek v razpis pozicija 


                //                    rowIndex++;
                //                }
                //            }

                //            rowIndex++;


                //        }
                //        workbook.Worksheets[0].Columns.AutoFit(0, 5);
                //        workbook.Worksheets[0].Columns[2].Visible = chkCiljnaCena.Checked;
                //        workbook.Worksheets[0].Protect("123", WorksheetProtectionPermissions.Default);

                //        currentFileName = RemoveForbidenChracters(tsm.Naziv.ToString()).Replace(" ", "_").Replace(".", "") + "_" + DateTime.Now.Ticks.ToString() + "_Razpis.xls";
                //        currentFullFileName = razpisiPath + currentFileName;
                //        // dodamo ime dattotek v pozicijo in v session tdm
                //        tsm.ExcellFilePath = currentFullFileName;

                //        List<TenderPositionModel> lTenderPositionModel = tender.RazpisPozicija.Where(rp => rp.StrankaID == tsm.ClientID).ToList();
                //        if (lTenderPositionModel != null)
                //        {
                //            foreach (TenderPositionModel tpm in lTenderPositionModel)
                //            {
                //                tpm.PotDokumenta = tsm.ExcellFilePath;
                //            }
                //        }

                //        workbook.Worksheets[0].Columns[0].Visible = false;
                //        CommonMethods.LogThis("Ime in pot datoteke: " + currentFullFileName);
                //        workbook.SaveDocument(currentFullFileName, DocumentFormat.OpenXml);

                //        zip.AddFile(currentFullFileName, "");
                //    }

                //    zip.Save();


                //    DownloadTenderDataModel tdm = new DownloadTenderDataModel();

                //    if (selectedRowsCarriers.Count == 1)
                //    {
                //        byte[] byteFile = File.ReadAllBytes(currentFullFileName);
                //        //WriteDocumentToResponse(byteFile, "xls", false, currentFileName);

                //        tdm.ByteData = byteFile;
                //        tdm.FileExtension = "xls";
                //        tdm.FileName = currentFileName;
                //        tdm.IsInline = false;
                //    }
                //    else
                //    {
                //        byte[] byteFile = File.ReadAllBytes(razpisiPath + zipFileName);
                //        //WriteDocumentToResponse(byteFile, "zip", false, zipFileName);

                //        tdm.ByteData = byteFile;
                //        tdm.FileExtension = "zip";
                //        tdm.FileName = zipFileName;
                //        tdm.IsInline = false;
                //    }
                //    tender.RazpisKreiran = true;
                //    tender.PotRazpisa = sFileLocation;
                //    tdm._hlpTenderCreateExcellData = vTTModel.tTenderCreateExcellData;
                //    vTTModel.tTenderCreateExcellData._TenderModel = tender;
                //    tender = CheckModelValidation(GetDatabaseConnectionInstance().SaveTenderAndTenderPosition(tender));
                //    //var objTender2 = CheckModelValidation(GetDatabaseConnectionInstance().SaveTender(tender));

                //    AddValueToSession(Enums.TenderSession.DownloadTenderData, tdm);

                //    ASPxGridViewRoutes.Selection.UnselectAll();
                //    ASPxGridViewCarrier.Selection.UnselectAll();

                Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseLoadingPanel", String.Format("clientLoadingPanel.Hide();" +
                    "$('#warningButtonModal').modal('show');" +
                    "$('#warningButtonModalBody').empty();" +
                    "$('#warningButtonModalBody').append('Uspešno ste kreirali razpis z izbranimi pozicijami in prevozniki');" +
                    "$('#warningButtonModalTitle').empty();" +                    
                    "$('#warningButtonModalBody').append('\\r\\n POZOR: Razpis za prenos in za pošiljanje prevoznikom preverite na seznamu Razpisov');" +
                    "$('#warningButtonModalTitle').empty();" +
                    "$('#warningButtonModalTitle').append('Uspešno kreiran razpis')"), true);

                //    ASPxGridViewCarrier.DataBind();
                //    ASPxGridViewRoutes.DataBind();

                //}
            }
            catch (Exception ex)
            {
                CommonMethods.LogThis(ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace);

            }
        }



        protected void btnConfirmDownload_Click(object sender, EventArgs e)
        {
            if (SessionHasValue(Enums.TenderSession.DownloadTenderData))
            {
                DownloadTenderDataModel tdm = (DownloadTenderDataModel)GetValueFromSession(Enums.TenderSession.DownloadTenderData);

                CommonMethods.WriteDocumentToResponse(this, tdm.ByteData, tdm.FileExtension, tdm.IsInline, tdm.FileName);

                RemoveSession(Enums.TenderSession.DownloadTenderData);
            }
        }

        protected void btnSendTender_Click(object sender, EventArgs e)
        {
            if (SessionHasValue(Enums.TenderSession.DownloadTenderData))
            {
                DownloadTenderDataModel tdm = (DownloadTenderDataModel)GetValueFromSession(Enums.TenderSession.DownloadTenderData);

                hlpTenderCreateExcellData hlp = CheckModelValidation(GetDatabaseConnectionInstance().SendTenderToTransportersEmails(tdm._hlpTenderCreateExcellData));

                RemoveSession(Enums.TenderSession.DownloadTenderData);
            }
        }

        protected void ASPxGridViewTons_DataBound(object sender, EventArgs e)
        {

            //ASPxGridView grid = sender as ASPxGridView;
            //for (int i = 0; i < grid.VisibleRowCount; i++)
            //{
            //    grid.Selection.SelectRow(i);
            //}
        }
    }
}