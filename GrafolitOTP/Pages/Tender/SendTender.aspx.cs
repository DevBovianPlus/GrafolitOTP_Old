using DatabaseWebService.ModelsOTP.Tender;
using DevExpress.Spreadsheet;
using DevExpress.Web;
using Ionic.Zip;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Helpers.Models;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
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
                DateEditDatumRazpisa.Date = DateTime.Now;
            }
        }

        protected void ASPxGridViewRoutes_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = CheckModelValidation(GetDatabaseConnectionInstance().GetAllRoutes());
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
            List<object> selectedRowsRoutes = ASPxGridViewRoutes.GetSelectedFieldValues("RelacijaID", "Naziv");
            List<object> selectedRowsCarriers = ASPxGridViewCarrier.GetSelectedFieldValues("idStranka", "NazivPrvi");

            TenderFullModel tender = new TenderFullModel();
            tender.CenaSkupaj = 0;
            tender.DatumRazpisa = DateEditDatumRazpisa.Date;
            tender.Naziv = txtTenderName.Text;
            tender.RazpisID = 0;
            tender.tsIDOseba = PrincipalHelper.GetUserPrincipal().ID;
            List<TenderPositionModel> tenderPositionsToSave = new List<TenderPositionModel>();
            List<TransportCountModel> transportCountList = new List<TransportCountModel>();

            foreach (var routeItem in selectedRowsRoutes)
            {
                IList routeList = (IList)routeItem;
                foreach (var carrierItem in selectedRowsCarriers)
                {
                    IList carrierList = (IList)carrierItem;
                    TenderPositionModel model = new TenderPositionModel();
                    model.Cena = 0;
                    model.RazpisID = 0;
                    model.RazpisPozicijaID = 0;
                    model.RelacijaID = CommonMethods.ParseInt(routeList[0]);
                    model.StrankaID = CommonMethods.ParseInt(carrierList[0]);
                    model.IDOseba = PrincipalHelper.GetUserPrincipal().ID;
                    tenderPositionsToSave.Add(model);
                    transportCountList.Add(new TransportCountModel { RelacijaID = model.RelacijaID, PrevoznikID = model.StrankaID });
                }
            }

            // transportCountList = CheckModelValidation(GetDatabaseConnectionInstance().GetTransportCounByTransporterAndRoute(transportCountList));

            string path = Server.MapPath("~");
            Workbook workbook = null;
            int rowIndex = 0;
            string zipFileName = "Razpisi_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm") + ".zip";
            using (ZipFile zip = new ZipFile(path + zipFileName))
            {

                foreach (var item in selectedRowsCarriers)
                {
                    IList carrierList = (IList)item;
                    workbook = new Workbook();
                    workbook.Worksheets[0].Name = carrierList[1].ToString();
                    workbook.Worksheets[0].Cells[0, 0].Value = carrierList[0].ToString();//prvi stolpec shranjujemo ID-je
                    workbook.Worksheets[0].Cells[0, 0].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                    workbook.Worksheets[0].Cells[0, 1].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                    workbook.Worksheets[0].Cells[0, 1].Font.Bold = true;
                    workbook.Worksheets[0].Cells[0, 1].Value = carrierList[1].ToString();
                    workbook.Worksheets[0].Cells[0, 2].Font.Bold = true;
                    workbook.Worksheets[0].Cells[0, 2].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                    workbook.Worksheets[0].Cells[0, 2].Value = "Cena";
                    workbook.Worksheets[0].Cells[0, 1].AutoFitColumns();
                    workbook.Worksheets[0].Cells[0, 1].AutoFitRows();

                    rowIndex = 1;
                    foreach (var obj in selectedRowsRoutes)
                    {
                        IList routeList = (IList)obj;
                        workbook.Worksheets[0].Cells[rowIndex, 0].Value = routeList[0].ToString();
                        workbook.Worksheets[0].Cells[rowIndex, 1].Value = routeList[1].ToString();

                        rowIndex++;
                    }

                    workbook.Worksheets[0].Columns[0].Visible = false;
                    workbook.SaveDocument(path + carrierList[1].ToString().Replace(" ", "_").Replace(".", "") + "_Razpis.xls", DocumentFormat.OpenXml);

                    zip.AddFile(path + carrierList[1].ToString().Replace(" ", "_") + "_Razpis.xls");
                }

                zip.Save();

                byte[] byteFile = File.ReadAllBytes(path + zipFileName);
                WriteDocumentToResponse(byteFile, "zip", false, zipFileName);
            }

            tender.RazpisPozicija = tenderPositionsToSave;
            CallbackPanelSendTenders.JSProperties["cpSendTender"] = tenderPositionsToSave.Count.ToString();
            //CheckModelValidation(GetDatabaseConnectionInstance().SaveTenderAndTenderPosition(tender));
        }

        protected void ASPxPopupCompleteTenderData_WindowCallback(object source, PopupWindowCallbackArgs e)
        {

        }

        private void WriteDocumentToResponse(byte[] documentData, string format, bool isInline, string fileName)
        {
            try
            {
                string contentType = "application/pdf";

                if (format == "png")
                    contentType = "image/png";
                else if (format == "jpg" || format == "jpeg")
                    contentType = "image/jpeg";
                else if (format == "xls")
                    contentType = "application/xls";
                else if (format == "zip")
                    contentType = "application/zip";
                else
                    contentType = "application/octet-stream";

                string disposition = (isInline) ? "inline" : "attachment";

                CommonMethods.LogThis("Before dowload");
                Response.Clear();
                Response.ContentType = contentType;

                Response.ClearHeaders();

                CommonMethods.LogThis("Before Add header");
                Response.AddHeader("Content-Disposition", String.Format("{0}; filename={1}", disposition, fileName));
                Response.AddHeader("Content-Length", documentData.Length.ToString());

                Response.Clear(); // dodal boris - 21.02.2019       
                Response.BufferOutput = false;
                Response.ClearContent();

                CommonMethods.LogThis("Before Binnarywrite");
                Response.BinaryWrite(documentData);

                Response.Flush();

                //Response.Close();
                //Response.End();

                Response.SuppressContent = true;
            }
            catch (Exception ex)
            {
                CommonMethods.LogThis(ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace);

            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                List<object> selectedRowsRoutes = ASPxGridViewRoutes.GetSelectedFieldValues("RelacijaID", "Naziv", "RecallCount");
                List<object> selectedRowsCarriers = ASPxGridViewCarrier.GetSelectedFieldValues("idStranka", "NazivPrvi");

                TenderFullModel tender = new TenderFullModel();
                tender.CenaSkupaj = 0;
                tender.DatumRazpisa = DateEditDatumRazpisa.Date;
                tender.Naziv = txtTenderName.Text;
                tender.RazpisID = 0;
                tender.tsIDOseba = PrincipalHelper.GetUserPrincipal().ID;
                List<TenderPositionModel> tenderPositionsToSave = new List<TenderPositionModel>();
                List<TransportCountModel> transportCountList = new List<TransportCountModel>();

                foreach (var routeItem in selectedRowsRoutes)
                {
                    IList routeList = (IList)routeItem;
                    foreach (var carrierItem in selectedRowsCarriers)
                    {
                        IList carrierList = (IList)carrierItem;
                        TenderPositionModel model = new TenderPositionModel();
                        model.Cena = 0;
                        model.RazpisID = 0;
                        model.RazpisPozicijaID = 0;
                        model.RelacijaID = CommonMethods.ParseInt(routeList[0]);
                        model.StrankaID = CommonMethods.ParseInt(carrierList[0]);
                        model.IDOseba = PrincipalHelper.GetUserPrincipal().ID;
                        tenderPositionsToSave.Add(model);
                        transportCountList.Add(new TransportCountModel { RelacijaID = model.RelacijaID, PrevoznikID = model.StrankaID });
                    }
                }

                //Server.ScriptTimeout = 1200;

                // transportCountList = CheckModelValidation(GetDatabaseConnectionInstance().GetTransportCounByTransporterAndRoute(transportCountList));

                tender.RazpisPozicija = tenderPositionsToSave;
                CallbackPanelSendTenders.JSProperties["cpSendTender"] = tenderPositionsToSave.Count.ToString();
                //CommonMethods.LogThis("Before Save 1");
                var objTender = CheckModelValidation(GetDatabaseConnectionInstance().SaveTender(tender));
                //CommonMethods.LogThis("After Save 1");
                //CommonMethods.LogThis("After objTender.RazpisID" + objTender.RazpisID);
                tender.RazpisID = objTender.RazpisID;
                //CommonMethods.LogThis("Before Save 2");
                tender = CheckModelValidation(GetDatabaseConnectionInstance().SaveTenderAndTenderPosition(tender));
                //CommonMethods.LogThis("Ater Save 2");


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
                CommonMethods.LogThis("zipFileName: " + zipFileName);
                using (ZipFile zip = new ZipFile(razpisiPath + zipFileName))
                {

                    foreach (var item in selectedRowsCarriers)
                    {
                        IList carrierList = (IList)item;
                        workbook = new Workbook();
                        string wsName = carrierList[1].ToString().Length > 30 ? carrierList[1].ToString().Substring(0, 30) : carrierList[1].ToString();
                        workbook.Worksheets[0].Name = RemoveForbidenChracters(wsName);
                        workbook.Worksheets[0].MergeCells(workbook.Worksheets[0].Range["B1:C1"]);
                        workbook.Worksheets[0].Cells[0, 0].Value = objTender.RazpisID.ToString();//prvi stolpec shranjujemo ID-je
                        workbook.Worksheets[0].Cells[0, 1].Value = txtTenderName.Text;
                        workbook.Worksheets[0].Cells[0, 1].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                        workbook.Worksheets[0].Cells[0, 1].Font.Bold = true;
                        workbook.Worksheets[0].Cells[1, 0].Value = carrierList[0].ToString();
                        workbook.Worksheets[0].Cells[1, 0].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                        workbook.Worksheets[0].Cells[1, 1].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                        workbook.Worksheets[0].Cells[1, 1].Font.Bold = true;
                        workbook.Worksheets[0].Cells[1, 1].Value = carrierList[1].ToString();
                        workbook.Worksheets[0].Cells[1, 2].Font.Bold = true;
                        workbook.Worksheets[0].Cells[1, 2].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                        workbook.Worksheets[0].Cells[1, 2].Value = "Cena";
                        workbook.Worksheets[0].Cells[1, 3].Font.Bold = true;
                        workbook.Worksheets[0].Cells[1, 3].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                        workbook.Worksheets[0].Cells[1, 3].Value = "Št. prevozev v zadnjem letu";
                        workbook.Worksheets[0].Cells[1, 4].Font.Bold = true;
                        workbook.Worksheets[0].Cells[1, 4].Borders.BottomBorder.LineStyle = BorderLineStyle.Thick;
                        workbook.Worksheets[0].Cells[1, 4].Value = "Št. vseh prevozev za relacijo v zadnjem letu";
                        workbook.Worksheets[0].Cells[1, 1].AutoFitColumns();
                        workbook.Worksheets[0].Cells[1, 1].AutoFitRows();

                        rowIndex = 3;
                        foreach (var obj in selectedRowsRoutes)
                        {
           

                            IList routeList = (IList)obj;
                            workbook.Worksheets[0].Cells[rowIndex, 0].Value = routeList[0].ToString();
                            workbook.Worksheets[0].Cells[rowIndex, 1].Value = routeList[1].ToString();

                            //var value = transportCountList.Where(tcl => tcl.PrevoznikID == (int)carrierList[0] && tcl.RelacijaID == (int)routeList[0]).FirstOrDefault();
                            // 08.06.2020 - ugotovil da tega sploh ne uporabljajo in smo enostavno zakomentirali
                            //var value = CheckModelValidation(GetDatabaseConnectionInstance().GetTransportCounByTransporterIDAndRouteID(new TransportCountModel() { PrevoznikID = (int)carrierList[0], RelacijaID = (int)routeList[0] }).Result);
                            TransportCountModel value = null;
                            workbook.Worksheets[0].Cells[rowIndex, 3].Value = value != null ? value.StPotrjenihOdpoklicev : 0;
                            //workbook.Worksheets[0].Cells[rowIndex, 4].Value = value != null ? value.StVsehOdpoklicevZaRelacijo : 0;
                            workbook.Worksheets[0].Cells[rowIndex, 4].Value = routeList != null ? CommonMethods.ParseInt(routeList[2]) : 0;

                            rowIndex++;
                        }
                        currentFileName = RemoveForbidenChracters(carrierList[1].ToString()).Replace(" ", "_").Replace(".", "") + "_" + DateTime.Now.Ticks.ToString() + "_Razpis.xls";
                        currentFullFileName = razpisiPath + currentFileName;
                        workbook.Worksheets[0].Columns[0].Visible = false;
                        CommonMethods.LogThis("Ime in pot datoteke: " + currentFullFileName);
                        workbook.SaveDocument(currentFullFileName, DocumentFormat.OpenXml);

                        zip.AddFile(currentFullFileName, "");
                    }

                    zip.Save();


                    DownloadTenderDataModel tdm = new DownloadTenderDataModel();

                    if (selectedRowsCarriers.Count == 1)
                    {
                        byte[] byteFile = File.ReadAllBytes(currentFullFileName);
                        //WriteDocumentToResponse(byteFile, "xls", false, currentFileName);

                        tdm.ByteData = byteFile;
                        tdm.FileExtension = "xls";
                        tdm.FileName = currentFileName;
                        tdm.IsInline = false;
                    }
                    else
                    {
                        byte[] byteFile = File.ReadAllBytes(razpisiPath + zipFileName);
                        //WriteDocumentToResponse(byteFile, "zip", false, zipFileName);

                        tdm.ByteData = byteFile;
                        tdm.FileExtension = "zip";
                        tdm.FileName = zipFileName;
                        tdm.IsInline = false;
                    }

                    AddValueToSession(Enums.TenderSession.DownloadTenderData, tdm);

                    ASPxGridViewRoutes.Selection.UnselectAll();
                    ASPxGridViewCarrier.Selection.UnselectAll();

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseLoadingPanel", String.Format("clientLoadingPanel.Hide();" +
                        "$('#warningButtonModal').modal('show');" +
                        "$('#warningButtonModalBody').empty();" +
                        "$('#warningButtonModalBody').append('Uspešno ste kreirali razpis z izbranimi pozicijami in prevozniki');" +
                        "$('#warningButtonModalTitle').empty();" +
                        "$('#warningButtonModalTitle').append('Uspešno kreiran razpis')"), true);

                    ASPxGridViewCarrier.DataBind();
                    ASPxGridViewRoutes.DataBind();

                }
            }
            catch (Exception ex)
            {
                CommonMethods.LogThis(ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace);

            }
        }

        private string RemoveForbidenChracters(string possibleWorksheetName)
        {
            if (possibleWorksheetName.Contains("\\"))
                possibleWorksheetName = possibleWorksheetName.Replace("\\", "-");

            if (possibleWorksheetName.Contains("/"))
                possibleWorksheetName = possibleWorksheetName.Replace("/", "-");

            if (possibleWorksheetName.Contains("?"))
                possibleWorksheetName = possibleWorksheetName.Replace("?", "-");

            if (possibleWorksheetName.Contains(":"))
                possibleWorksheetName = possibleWorksheetName.Replace(":", "-");

            if (possibleWorksheetName.Contains("*"))
                possibleWorksheetName = possibleWorksheetName.Replace("*", "-");

            if (possibleWorksheetName.Contains("["))
                possibleWorksheetName = possibleWorksheetName.Replace("[", "");

            if (possibleWorksheetName.Contains("]"))
                possibleWorksheetName = possibleWorksheetName.Replace("]", "");

            if (possibleWorksheetName.Contains("\""))
                possibleWorksheetName = possibleWorksheetName.Replace("\"", "");

            return possibleWorksheetName;
        }

        protected void btnConfirmDownload_Click(object sender, EventArgs e)
        {
            if (SessionHasValue(Enums.TenderSession.DownloadTenderData))
            {
                DownloadTenderDataModel tdm = (DownloadTenderDataModel)GetValueFromSession(Enums.TenderSession.DownloadTenderData);

                WriteDocumentToResponse(tdm.ByteData, tdm.FileExtension, tdm.IsInline, tdm.FileName);

                RemoveSession(Enums.TenderSession.DownloadTenderData);
            }
        }
    }
}