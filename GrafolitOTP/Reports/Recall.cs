using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.Xpo;
using DatabaseWebService.ModelsOTP.Recall;
using System.Configuration;

/// <summary>
/// Summary description for Recall
/// </summary>
public class Recall : DevExpress.XtraReports.UI.XtraReport
{
    #region Variables
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private DevExpress.Xpo.Session session1;
    private ReportHeaderBand ReportHeader;
    private XRLabel lblMaxPayload;
    private XRLabel lblNumberOfPallets;
    private XRLabel lblOrderToPickUp;
    private XRLabel lblLorryDriverName;
    private XRLabel lblTrailerNumber;
    private XRLabel lblTransportCompany;
    private XRLabel lblAddress;
    private XRLabel xrLabel11;
    private XRLabel xrLabel10;
    private XRLabel xrLabel9;
    private XRLabel xrLabel8;
    private XRLabel xrLabel7;
    private XRLabel xrLabel5;
    private XRLabel xrLabel4;
    private XRLabel lblCustomerName;
    private XRLabel xrLabel2;
    private DetailReportBand DetailReport;
    private DetailBand Detail1;
    private XRTable RecallPositionTable;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private XRLine xrLine1;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private GroupFooterBand GroupFooter1;
    private XRLabel lblOpomba;
    private XRLabel xrLabel1;
    private PageFooterBand PageFooter;
    private XRLabel xrLabel3;
    #endregion
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell3;
    private XRLabel xrLabel6;
    private XRLabel lblRecallNumber;
    private XRPictureBox xrPictureBox1;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell11;
    private XRLabel lblEmployeeName;
    private XRLabel lblDatumIzpisa;
    private XRLabel lblEmployeeEmail;
    private XRLabel lblEmployeePhone;
    private XRLabel xrLabel12;
    private XRLabel xrLabel16;
    private XRLabel lblPostSupplier;
    private XRLabel xrLabel14;
    private XRLabel lblAddressSupplier;
    private XRLabel lblSupplierName;
    private XRLabel lblLoadingDate;
    private XRLabel xrLabel15;
    private XRLabel lblSum;

    private decimal dSum = 0;
    private decimal dSumPalete = 0;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public Recall(RecallFullModel model)
    {
        InitializeComponent();

        lblCustomerName.Text = ConfigurationManager.AppSettings["Company"].ToString();
        lblAddress.Text = ConfigurationManager.AppSettings["CompanyAddress"].ToString();

        lblTransportCompany.Text = model.Dobavitelj != null ? model.Dobavitelj.NazivPrvi : model.Prevozniki;//Prevoznik
        lblTrailerNumber.Text = model.Registracija;
        lblLorryDriverName.Text = model.SoferNaziv;
        lblNumberOfPallets.Text = model.PaleteSkupaj.ToString();
        lblMaxPayload.Text = "24t";
        lblOpomba.Text = model.Opis;
        lblRecallNumber.Text = model.OdpoklicStevilka.ToString();
        lblDatumIzpisa.Text = "Date : " + DateTime.Now.ToString("dd-MM-yyyy");
        lblEmployeeName.Text = model.User != null ? "Referent : " + model.User.Ime + " " + model.User.Priimek : "";
        lblEmployeePhone.Text = model.User != null ? model.User.TelefonGSM : "";
        lblEmployeeEmail.Text = model.User != null ? model.User.Email : "";
        lblSupplierName.Text = model.DobaviteljNaziv != null ? model.DobaviteljNaziv : "";
        lblAddressSupplier.Text = model.DobaviteljNaslov != null ? model.DobaviteljNaslov : "";
        lblPostSupplier.Text = model.DobaviteljPosta != null ? model.DobaviteljPosta : "";

        lblLoadingDate.Text = model.DatumNaklada > new DateTime(2000, 1, 1) ? model.DatumNaklada.Value.ToShortDateString() : "";


        foreach (var item in model.OdpoklicPozicija)
        {
            dSum = dSum + item.Kolicina;
            dSumPalete += item.Palete; 
            XRTableRow row = new XRTableRow();
            XRTableCell cell = new XRTableCell();
            cell.WidthF = 50f;
            //cell.BackColor = Color.AliceBlue;
            cell.Text = item.ZaporednaStevilka.ToString();
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            row.Cells.Add(cell);

            cell = new XRTableCell();
            cell.WidthF = 230f;
            //cell.BackColor = Color.Beige;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            cell.Text = item.OC.ToString();
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            row.Cells.Add(cell);

            cell = new XRTableCell();
            cell.WidthF = 230f;
            //cell.BackColor = Color.Chartreuse;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            cell.Text = item.NarociloID.ToString();
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            row.Cells.Add(cell);

            cell = new XRTableCell();
            cell.WidthF = 550f;
            //cell.BackColor = Color.DarkSalmon;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            cell.Text = item.Material;
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            row.Cells.Add(cell);

            cell = new XRTableCell();
            cell.WidthF = 130f;
            //cell.BackColor = Color.LightBlue;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            cell.Text = item.Kolicina.ToString("N2");
            if (item.Palete > 0)
                cell.Text += " (" + item.Palete.ToString("N2") + " pall)";
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            row.Cells.Add(cell);

            cell = new XRTableCell();
            cell.WidthF = 910f;
            //cell.BackColor = Color.Peru;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;

            if (item.KupecViden == 1)
            {
                cell.Text = item.KupecNaziv != null ? item.KupecNaziv.Trim() : "";
                cell.Text += item.KupecNaslov != null ? ", " + item.KupecNaslov.Trim() : "";
                cell.Text += item.KupecPosta != null ? ", " + item.KupecPosta.Trim() : "";
                cell.Text += item.KupecKraj != null ? " " + item.KupecKraj.Trim() : "";
            }
            else
            {
                cell.Text = ConfigurationManager.AppSettings["Company"].ToString() + ", " + ConfigurationManager.AppSettings["CompanyAddress"].ToString();                
            }
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            row.Cells.Add(cell);

            RecallPositionTable.Rows.Add(row);
        }

        // add sum for količina

        #region Sum količina
        XRTableRow rowSum = new XRTableRow();
        XRTableCell cellSum = new XRTableCell();
        cellSum.WidthF = 50f;
        //cellSum.BackColor = Color.AliceBlue;
        cellSum.Text = "";
        cellSum.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        cellSum.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
        rowSum.Cells.Add(cellSum);

        cellSum = new XRTableCell();
        cellSum.WidthF = 230f;
        //cellSum.BackColor = Color.Beige;
        cellSum.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        cellSum.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
        cellSum.Text = "";
        cellSum.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        rowSum.Cells.Add(cellSum);

        cellSum = new XRTableCell();
        cellSum.WidthF = 230f;
        //cellSum.BackColor = Color.Chartreuse;
        cellSum.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        cellSum.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
        cellSum.Text = "";
        cellSum.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        rowSum.Cells.Add(cellSum);

        cellSum = new XRTableCell();
        cellSum.WidthF = 550f;
        //cellSum.BackColor = Color.DarkSalmon;
        cellSum.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        cellSum.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
        cellSum.Text = "Truck load: " + dSum.ToString("N2");
        if (dSumPalete > 0)
            cellSum.Text += " (" + dSumPalete.ToString("N2") + " pall)";
        cellSum.Font = new Font(cellSum.Font, FontStyle.Bold);
        cellSum.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        rowSum.Cells.Add(cellSum);

        cellSum = new XRTableCell();
        cellSum.WidthF = 130f;
        //cellSum.BackColor = Color.LightBlue;
        cellSum.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        cellSum.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
        cellSum.Text = "";
        cellSum.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        rowSum.Cells.Add(cellSum);

        cellSum = new XRTableCell();
        cellSum.WidthF = 910f;
        //cellSum.BackColor = Color.Peru;
        cellSum.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        cellSum.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
        #endregion
        
        RecallPositionTable.Rows.Add(rowSum);
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Recall));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.session1 = new DevExpress.Xpo.Session(this.components);
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.lblLoadingDate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPostSupplier = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAddressSupplier = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSupplierName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblEmployeeEmail = new DevExpress.XtraReports.UI.XRLabel();
            this.lblEmployeePhone = new DevExpress.XtraReports.UI.XRLabel();
            this.lblEmployeeName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDatumIzpisa = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRecallNumber = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblMaxPayload = new DevExpress.XtraReports.UI.XRLabel();
            this.lblNumberOfPallets = new DevExpress.XtraReports.UI.XRLabel();
            this.lblOrderToPickUp = new DevExpress.XtraReports.UI.XRLabel();
            this.lblLorryDriverName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTrailerNumber = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTransportCompany = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAddress = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCustomerName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.RecallPositionTable = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.lblSum = new DevExpress.XtraReports.UI.XRLabel();
            this.lblOpomba = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            ((System.ComponentModel.ISupportInitialize)(this.session1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RecallPositionTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 254F;
            this.TopMargin.HeightF = 82.02084F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 254F;
            this.BottomMargin.HeightF = 8F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // session1
            // 
            this.session1.IsObjectModifiedOnNonPersistentPropertyChange = null;
            this.session1.TrackPropertiesModifications = false;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblLoadingDate,
            this.xrLabel15,
            this.xrLabel16,
            this.lblPostSupplier,
            this.xrLabel14,
            this.lblAddressSupplier,
            this.lblSupplierName,
            this.xrLabel12,
            this.lblEmployeeEmail,
            this.lblEmployeePhone,
            this.lblEmployeeName,
            this.lblDatumIzpisa,
            this.xrPictureBox1,
            this.xrLabel6,
            this.lblRecallNumber,
            this.xrLabel3,
            this.lblMaxPayload,
            this.lblNumberOfPallets,
            this.lblOrderToPickUp,
            this.lblLorryDriverName,
            this.lblTrailerNumber,
            this.lblTransportCompany,
            this.lblAddress,
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel5,
            this.xrLabel4,
            this.lblCustomerName,
            this.xrLabel2});
            this.ReportHeader.Dpi = 254F;
            this.ReportHeader.HeightF = 810.3165F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // lblLoadingDate
            // 
            this.lblLoadingDate.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblLoadingDate.Dpi = 254F;
            this.lblLoadingDate.Font = new System.Drawing.Font("Candara", 11F);
            this.lblLoadingDate.LocationFloat = new DevExpress.Utils.PointFloat(625.839F, 752.5052F);
            this.lblLoadingDate.Name = "lblLoadingDate";
            this.lblLoadingDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblLoadingDate.SizeF = new System.Drawing.SizeF(1085.744F, 39.89911F);
            this.lblLoadingDate.StylePriority.UseBorders = false;
            this.lblLoadingDate.StylePriority.UseFont = false;
            // 
            // xrLabel15
            // 
            this.xrLabel15.Dpi = 254F;
            this.xrLabel15.Font = new System.Drawing.Font("Candara", 11F);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(0.0001009305F, 752.5052F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(625.839F, 39.89917F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.Text = "Loading date: ";
            // 
            // xrLabel16
            // 
            this.xrLabel16.Dpi = 254F;
            this.xrLabel16.Font = new System.Drawing.Font("Candara", 11F);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(298.5627F, 210.7639F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(625.839F, 39.89911F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "Post: ";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblPostSupplier
            // 
            this.lblPostSupplier.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPostSupplier.Dpi = 254F;
            this.lblPostSupplier.Font = new System.Drawing.Font("Candara", 11F);
            this.lblPostSupplier.LocationFloat = new DevExpress.Utils.PointFloat(924.4016F, 210.7639F);
            this.lblPostSupplier.Name = "lblPostSupplier";
            this.lblPostSupplier.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPostSupplier.SizeF = new System.Drawing.SizeF(1005.599F, 39.89909F);
            this.lblPostSupplier.StylePriority.UseBorders = false;
            this.lblPostSupplier.StylePriority.UseFont = false;
            // 
            // xrLabel14
            // 
            this.xrLabel14.Dpi = 254F;
            this.xrLabel14.Font = new System.Drawing.Font("Candara", 11F);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(298.5624F, 165.8648F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(625.839F, 39.89911F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "Address :";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblAddressSupplier
            // 
            this.lblAddressSupplier.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblAddressSupplier.Dpi = 254F;
            this.lblAddressSupplier.Font = new System.Drawing.Font("Candara", 11F);
            this.lblAddressSupplier.LocationFloat = new DevExpress.Utils.PointFloat(924.4014F, 165.8648F);
            this.lblAddressSupplier.Name = "lblAddressSupplier";
            this.lblAddressSupplier.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAddressSupplier.SizeF = new System.Drawing.SizeF(1005.599F, 39.89909F);
            this.lblAddressSupplier.StylePriority.UseBorders = false;
            this.lblAddressSupplier.StylePriority.UseFont = false;
            // 
            // lblSupplierName
            // 
            this.lblSupplierName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblSupplierName.Dpi = 254F;
            this.lblSupplierName.Font = new System.Drawing.Font("Candara", 12F, System.Drawing.FontStyle.Bold);
            this.lblSupplierName.LocationFloat = new DevExpress.Utils.PointFloat(924.4014F, 109.2325F);
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblSupplierName.SizeF = new System.Drawing.SizeF(1005.599F, 50.48244F);
            this.lblSupplierName.StylePriority.UseBorders = false;
            this.lblSupplierName.StylePriority.UseFont = false;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Dpi = 254F;
            this.xrLabel12.Font = new System.Drawing.Font("Candara", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(676.9167F, 109.2325F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(247.4847F, 50.48244F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "Name  :  ";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblEmployeeEmail
            // 
            this.lblEmployeeEmail.Dpi = 254F;
            this.lblEmployeeEmail.Font = new System.Drawing.Font("Candara", 11F);
            this.lblEmployeeEmail.LocationFloat = new DevExpress.Utils.PointFloat(1970.584F, 415.9232F);
            this.lblEmployeeEmail.Name = "lblEmployeeEmail";
            this.lblEmployeeEmail.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblEmployeeEmail.SizeF = new System.Drawing.SizeF(751.416F, 39.89908F);
            this.lblEmployeeEmail.StylePriority.UseFont = false;
            this.lblEmployeeEmail.StylePriority.UseTextAlignment = false;
            this.lblEmployeeEmail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // lblEmployeePhone
            // 
            this.lblEmployeePhone.Dpi = 254F;
            this.lblEmployeePhone.Font = new System.Drawing.Font("Candara", 11F);
            this.lblEmployeePhone.LocationFloat = new DevExpress.Utils.PointFloat(1970.584F, 376.0241F);
            this.lblEmployeePhone.Name = "lblEmployeePhone";
            this.lblEmployeePhone.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblEmployeePhone.SizeF = new System.Drawing.SizeF(751.416F, 39.89908F);
            this.lblEmployeePhone.StylePriority.UseFont = false;
            this.lblEmployeePhone.StylePriority.UseTextAlignment = false;
            this.lblEmployeePhone.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // lblEmployeeName
            // 
            this.lblEmployeeName.Dpi = 254F;
            this.lblEmployeeName.Font = new System.Drawing.Font("Candara", 11F);
            this.lblEmployeeName.LocationFloat = new DevExpress.Utils.PointFloat(1970.584F, 336.1251F);
            this.lblEmployeeName.Name = "lblEmployeeName";
            this.lblEmployeeName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblEmployeeName.SizeF = new System.Drawing.SizeF(751.416F, 39.89914F);
            this.lblEmployeeName.StylePriority.UseFont = false;
            this.lblEmployeeName.StylePriority.UseTextAlignment = false;
            this.lblEmployeeName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // lblDatumIzpisa
            // 
            this.lblDatumIzpisa.Dpi = 254F;
            this.lblDatumIzpisa.Font = new System.Drawing.Font("Candara", 12F);
            this.lblDatumIzpisa.LocationFloat = new DevExpress.Utils.PointFloat(1970.584F, 279.4793F);
            this.lblDatumIzpisa.Multiline = true;
            this.lblDatumIzpisa.Name = "lblDatumIzpisa";
            this.lblDatumIzpisa.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblDatumIzpisa.SizeF = new System.Drawing.SizeF(751.416F, 39.89911F);
            this.lblDatumIzpisa.StylePriority.UseFont = false;
            this.lblDatumIzpisa.StylePriority.UseTextAlignment = false;
            this.lblDatumIzpisa.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Dpi = 254F;
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(1970.584F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(751.4163F, 189.2533F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Dpi = 254F;
            this.xrLabel6.Font = new System.Drawing.Font("Candara", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(13.22941F, 12.7967F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(358.6098F, 95.46167F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "Transport num.: ";
            // 
            // lblRecallNumber
            // 
            this.lblRecallNumber.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblRecallNumber.Dpi = 254F;
            this.lblRecallNumber.Font = new System.Drawing.Font("Candara", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblRecallNumber.LocationFloat = new DevExpress.Utils.PointFloat(371.8392F, 12.7967F);
            this.lblRecallNumber.Name = "lblRecallNumber";
            this.lblRecallNumber.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblRecallNumber.SizeF = new System.Drawing.SizeF(321.0983F, 73.26907F);
            this.lblRecallNumber.StylePriority.UseBorders = false;
            this.lblRecallNumber.StylePriority.UseFont = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Candara", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(883.7083F, 0F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(1015.729F, 96.64912F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "ANNOUCEMENT FOR LOADING ON Supplier: ";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblMaxPayload
            // 
            this.lblMaxPayload.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblMaxPayload.Dpi = 254F;
            this.lblMaxPayload.Font = new System.Drawing.Font("Candara", 11F);
            this.lblMaxPayload.LocationFloat = new DevExpress.Utils.PointFloat(2082.271F, 596.2982F);
            this.lblMaxPayload.Name = "lblMaxPayload";
            this.lblMaxPayload.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblMaxPayload.SizeF = new System.Drawing.SizeF(596.2654F, 39.89923F);
            this.lblMaxPayload.StylePriority.UseBorders = false;
            this.lblMaxPayload.StylePriority.UseFont = false;
            this.lblMaxPayload.Visible = false;
            // 
            // lblNumberOfPallets
            // 
            this.lblNumberOfPallets.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblNumberOfPallets.Dpi = 254F;
            this.lblNumberOfPallets.Font = new System.Drawing.Font("Candara", 11F);
            this.lblNumberOfPallets.LocationFloat = new DevExpress.Utils.PointFloat(625.8391F, 694.1732F);
            this.lblNumberOfPallets.Name = "lblNumberOfPallets";
            this.lblNumberOfPallets.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblNumberOfPallets.SizeF = new System.Drawing.SizeF(1085.744F, 39.89917F);
            this.lblNumberOfPallets.StylePriority.UseBorders = false;
            this.lblNumberOfPallets.StylePriority.UseFont = false;
            // 
            // lblOrderToPickUp
            // 
            this.lblOrderToPickUp.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblOrderToPickUp.Dpi = 254F;
            this.lblOrderToPickUp.Font = new System.Drawing.Font("Candara", 11F);
            this.lblOrderToPickUp.LocationFloat = new DevExpress.Utils.PointFloat(625.8391F, 620.09F);
            this.lblOrderToPickUp.Name = "lblOrderToPickUp";
            this.lblOrderToPickUp.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblOrderToPickUp.SizeF = new System.Drawing.SizeF(1085.744F, 39.89917F);
            this.lblOrderToPickUp.StylePriority.UseBorders = false;
            this.lblOrderToPickUp.StylePriority.UseFont = false;
            // 
            // lblLorryDriverName
            // 
            this.lblLorryDriverName.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblLorryDriverName.Dpi = 254F;
            this.lblLorryDriverName.Font = new System.Drawing.Font("Candara", 11F);
            this.lblLorryDriverName.LocationFloat = new DevExpress.Utils.PointFloat(625.8391F, 548.6525F);
            this.lblLorryDriverName.Name = "lblLorryDriverName";
            this.lblLorryDriverName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblLorryDriverName.SizeF = new System.Drawing.SizeF(1085.744F, 39.89917F);
            this.lblLorryDriverName.StylePriority.UseBorders = false;
            this.lblLorryDriverName.StylePriority.UseFont = false;
            // 
            // lblTrailerNumber
            // 
            this.lblTrailerNumber.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblTrailerNumber.Dpi = 254F;
            this.lblTrailerNumber.Font = new System.Drawing.Font("Candara", 11F);
            this.lblTrailerNumber.LocationFloat = new DevExpress.Utils.PointFloat(625.8391F, 474.5689F);
            this.lblTrailerNumber.Name = "lblTrailerNumber";
            this.lblTrailerNumber.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTrailerNumber.SizeF = new System.Drawing.SizeF(1085.744F, 39.89911F);
            this.lblTrailerNumber.StylePriority.UseBorders = false;
            this.lblTrailerNumber.StylePriority.UseFont = false;
            // 
            // lblTransportCompany
            // 
            this.lblTransportCompany.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblTransportCompany.Dpi = 254F;
            this.lblTransportCompany.Font = new System.Drawing.Font("Candara", 11F);
            this.lblTransportCompany.LocationFloat = new DevExpress.Utils.PointFloat(625.8391F, 342.2773F);
            this.lblTransportCompany.Name = "lblTransportCompany";
            this.lblTransportCompany.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTransportCompany.SizeF = new System.Drawing.SizeF(1085.744F, 95.46167F);
            this.lblTransportCompany.StylePriority.UseBorders = false;
            this.lblTransportCompany.StylePriority.UseFont = false;
            this.lblTransportCompany.StylePriority.UseTextAlignment = false;
            this.lblTransportCompany.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblAddress
            // 
            this.lblAddress.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblAddress.Dpi = 254F;
            this.lblAddress.Font = new System.Drawing.Font("Candara", 11F);
            this.lblAddress.LocationFloat = new DevExpress.Utils.PointFloat(2238.318F, 713.606F);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAddress.SizeF = new System.Drawing.SizeF(458.6814F, 39.89923F);
            this.lblAddress.StylePriority.UseBorders = false;
            this.lblAddress.StylePriority.UseFont = false;
            this.lblAddress.Visible = false;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Dpi = 254F;
            this.xrLabel11.Font = new System.Drawing.Font("Candara", 11F);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(2082.271F, 556.3989F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(355.964F, 39.89923F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.Text = "maximum payload :";
            this.xrLabel11.Visible = false;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Dpi = 254F;
            this.xrLabel10.Font = new System.Drawing.Font("Candara", 11F);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(0.0002422333F, 694.1732F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(625.8389F, 39.89917F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.Text = "No of pallets/reels to collect :";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Dpi = 254F;
            this.xrLabel9.Font = new System.Drawing.Font("Candara", 11F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(0.0002422333F, 620.09F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(625.8389F, 39.89917F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = "Order to pick up : ";
            // 
            // xrLabel8
            // 
            this.xrLabel8.Dpi = 254F;
            this.xrLabel8.Font = new System.Drawing.Font("Candara", 11F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(0.0002422333F, 548.6525F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(625.8389F, 39.89917F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "Lorry driver\'s name : ";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Dpi = 254F;
            this.xrLabel7.Font = new System.Drawing.Font("Candara", 11F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(0.0002422333F, 474.5689F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(625.839F, 39.89911F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "Trailer/moto tractor no :";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Candara", 11F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(0.0002422333F, 342.2774F);
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(625.839F, 95.46167F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "Transport company put in charge for\r\n transport/collecting goods ex mill :";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Candara", 11F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(2071.161F, 753.5052F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(625.8389F, 39.8992F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "Ultimate destination/address :";
            this.xrLabel4.Visible = false;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblCustomerName.Dpi = 254F;
            this.lblCustomerName.Font = new System.Drawing.Font("Candara", 11F);
            this.lblCustomerName.LocationFloat = new DevExpress.Utils.PointFloat(625.8391F, 288.4793F);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblCustomerName.SizeF = new System.Drawing.SizeF(1085.744F, 39.89909F);
            this.lblCustomerName.StylePriority.UseBorders = false;
            this.lblCustomerName.StylePriority.UseFont = false;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Candara", 11F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0.0002422333F, 288.4793F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(625.839F, 39.89911F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Customer (Name, Country)  :  ";
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1});
            this.DetailReport.Dpi = 254F;
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            // 
            // Detail1
            // 
            this.Detail1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.RecallPositionTable,
            this.xrLine1,
            this.xrTable1});
            this.Detail1.Dpi = 254F;
            this.Detail1.HeightF = 254F;
            this.Detail1.Name = "Detail1";
            // 
            // RecallPositionTable
            // 
            this.RecallPositionTable.Dpi = 254F;
            this.RecallPositionTable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 82.23254F);
            this.RecallPositionTable.Name = "RecallPositionTable";
            this.RecallPositionTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.RecallPositionTable.SizeF = new System.Drawing.SizeF(2722F, 63.5F);
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5,
            this.xrTableCell9,
            this.xrTableCell12,
            this.xrTableCell6,
            this.xrTableCell7,
            this.xrTableCell8});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Font = new System.Drawing.Font("Candara", 10F);
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell5.StylePriority.UseBorders = false;
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell5.Weight = 0.058873040762561737D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell9.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.Font = new System.Drawing.Font("Candara", 10F);
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell9.Weight = 0.27081602354666034D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Font = new System.Drawing.Font("Candara", 10F);
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell12.Weight = 0.40721218873174514D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.Font = new System.Drawing.Font("Candara", 10F);
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell6.Weight = 0.79369990469564844D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Font = new System.Drawing.Font("Candara", 10F);
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell7.Weight = 0.40904561842775528D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Font = new System.Drawing.Font("Candara", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseFont = false;
            this.xrTableCell8.Weight = 1.2654017800561785D;
            // 
            // xrLine1
            // 
            this.xrLine1.BackColor = System.Drawing.Color.DarkGray;
            this.xrLine1.BorderWidth = 1F;
            this.xrLine1.Dpi = 254F;
            this.xrLine1.ForeColor = System.Drawing.Color.DarkGray;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 63.5F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(2722F, 5F);
            this.xrLine1.StylePriority.UseBackColor = false;
            this.xrLine1.StylePriority.UseBorderWidth = false;
            this.xrLine1.StylePriority.UseForeColor = false;
            // 
            // xrTable1
            // 
            this.xrTable1.Dpi = 254F;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(2722F, 63.49999F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell10,
            this.xrTableCell11,
            this.xrTableCell4,
            this.xrTableCell1,
            this.xrTableCell2});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Font = new System.Drawing.Font("Candara", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "#";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell3.Weight = 0.023569426652483037D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Font = new System.Drawing.Font("Candara", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "OC                 ";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell10.Weight = 0.066014038583306123D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.Font = new System.Drawing.Font("Candara", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrTableCell11.Multiline = true;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseFont = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "         Order No.";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell11.Weight = 0.16884549076682215D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Font = new System.Drawing.Font("Candara", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "Material";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.36182031582924828D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Font = new System.Drawing.Font("Candara", 11F);
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Quantity";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.13071223774429444D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Font = new System.Drawing.Font("Candara", 11F);
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Client";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.53215836985493348D;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblSum,
            this.lblOpomba,
            this.xrLabel1});
            this.GroupFooter1.Dpi = 254F;
            this.GroupFooter1.HeightF = 254F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // lblSum
            // 
            this.lblSum.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblSum.Dpi = 254F;
            this.lblSum.Font = new System.Drawing.Font("Candara", 11F);
            this.lblSum.LocationFloat = new DevExpress.Utils.PointFloat(1264.646F, 0F);
            this.lblSum.Name = "lblSum";
            this.lblSum.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblSum.SizeF = new System.Drawing.SizeF(233.1879F, 39.89909F);
            this.lblSum.StylePriority.UseBorders = false;
            this.lblSum.StylePriority.UseFont = false;
            // 
            // lblOpomba
            // 
            this.lblOpomba.Dpi = 254F;
            this.lblOpomba.Font = new System.Drawing.Font("Candara", 11F);
            this.lblOpomba.LocationFloat = new DevExpress.Utils.PointFloat(124.3542F, 80.58F);
            this.lblOpomba.Multiline = true;
            this.lblOpomba.Name = "lblOpomba";
            this.lblOpomba.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblOpomba.SizeF = new System.Drawing.SizeF(2597.646F, 58.42F);
            this.lblOpomba.StylePriority.UseFont = false;
            this.lblOpomba.StylePriority.UseTextAlignment = false;
            this.lblOpomba.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Candara", 11F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 80.58F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(124.3542F, 58.42F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Note : ";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // PageFooter
            // 
            this.PageFooter.Dpi = 254F;
            this.PageFooter.HeightF = 254F;
            this.PageFooter.Name = "PageFooter";
            // 
            // Recall
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.DetailReport,
            this.GroupFooter1,
            this.PageFooter});
            this.Dpi = 254F;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(132, 116, 82, 8);
            this.PageHeight = 2100;
            this.PageWidth = 2970;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.SnapGridSize = 25F;
            this.Version = "16.1";
            ((System.ComponentModel.ISupportInitialize)(this.session1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RecallPositionTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
