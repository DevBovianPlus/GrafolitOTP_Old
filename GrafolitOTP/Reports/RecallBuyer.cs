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
public class RecallBuyer : DevExpress.XtraReports.UI.XtraReport
{
    #region Variables
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private DevExpress.Xpo.Session session1;
    private ReportHeaderBand ReportHeader;
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
    private XRTableCell tcPrevzemnik;
    private GroupFooterBand GroupFooter1;
    private PageFooterBand PageFooter;
    private XRLabel xrLabel3;
    #endregion
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell3;
    private XRLabel xrLabel6;
    private XRLabel lblOrderNumber;
    private XRPictureBox xrPictureBox1;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell11;
    private XRLabel lblDatumIzpisa;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell19;
    private XRTableCell tcVrednost;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell16;

    private decimal dSumVrednost = 0;
    private decimal dSumVrednostTransporta = 0;
    private decimal dSumProcentTransporta = 0;
    private decimal dSumKolicina = 0;
    private decimal dAvgProcentPrevoza = 0;
    private int iCnt = 0;
    private XRLabel lblPrevoznik;
    private XRLabel xrLabel1;
    private XRLine xrLine2;
    private XRLabel lblOpomba;
    private XRRichText lblOpomba1;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    

    public RecallBuyer(RecallBuyerFullModel model, bool showValue)
    {
        InitializeComponent();

        tcVrednost.Visible = showValue;
        if (!showValue)
        {
            tcPrevzemnik.WidthF += tcVrednost.WidthF + 10;
        }

        lblOrderNumber.Text = model.StevilkaNarocilnica.ToString();
        lblPrevoznik.Text = model.PrevoznikNaziv;
        lblDatumIzpisa.Text = "Date : " + DateTime.Now.ToString("dd.MM.yyyy");
        lblOpomba1.Text = model.OpisOdpoklicKupec.ToString();

        iCnt = 0;
        foreach (var item in model.OdpoklicKupecPozicija)
        {
            iCnt++;
            dSumKolicina = dSumKolicina + item.Kolicina;
            dSumVrednost += item.Vrednost;
            dSumVrednostTransporta += item.VrednostPrevoza;
            dSumProcentTransporta += item.ProcentPrevoza;



            XRTableRow row = new XRTableRow();
            row.HeightF = 45f;
            XRTableCell cell = new XRTableCell();
            cell.WidthF = 20f;
            //cell.BackColor = Color.AliceBlue;
            cell.Text = item.ZaporednaStevilka.ToString();
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            row.Cells.Add(cell);

            cell = new XRTableCell();
            cell.WidthF = 130f;
            //cell.BackColor = Color.Beige;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            cell.Text = item.Kljuc.ToString();
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            row.Cells.Add(cell);

            cell = new XRTableCell();
            cell.WidthF = 90f;
            //cell.BackColor = Color.Chartreuse;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            cell.Text = item.Datum.ToString("dd.MM.yyyy");
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            row.Cells.Add(cell);

            cell = new XRTableCell();
            cell.WidthF = 90f;
            //cell.BackColor = Color.DarkSalmon;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            cell.Text = "EUR";
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            row.Cells.Add(cell);

            cell = new XRTableCell();
            cell.WidthF = 350f;
            //cell.BackColor = Color.Bisque;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            cell.Text = item.Kupec;
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            row.Cells.Add(cell);

            cell = new XRTableCell();
            cell.WidthF = 330f;
            //cell.BackColor = Color.DarkOrange;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            cell.Text = item.Prevzemnik;
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            row.Cells.Add(cell);

            //cell = new XRTableCell();
            //cell.WidthF = 130f;
            //cell.BackColor = Color.LightBlue;
            //cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            //cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            //cell.Text = item.Vrednost.ToString("N2");
            //cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //row.Cells.Add(cell);

            if (showValue)
            {
                cell = new XRTableCell();
                cell.WidthF = 130f;
                //cell.BackColor = Color.DodgerBlue;
                cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
                cell.Text = item.VrednostPrevoza.ToString("N2");
                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                row.Cells.Add(cell);
            }
            cell = new XRTableCell();
            cell.WidthF = 130f;
            //cell.BackColor = Color.Crimson;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            cell.Text = item.ProcentPrevoza.ToString("N2");
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            row.Cells.Add(cell);

            cell = new XRTableCell();
            cell.WidthF = 130f;
            //cell.BackColor = Color.FloralWhite;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            cell.Text = item.Kolicina.ToString("N2");
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            row.Cells.Add(cell);


            RecallPositionTable.Rows.Add(row);
        }

        dAvgProcentPrevoza = dSumProcentTransporta / iCnt;

        // add sum for količina
        #region Sum količina
        XRTableRow rowSum = new XRTableRow();

        XRTableCell cellSum = new XRTableCell();
        cellSum.WidthF = 1000f;
        //cellSum.BackColor = Color.AliceBlue;
        cellSum.Text = "SKUPAJ:";
        cellSum.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
        cellSum.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
        rowSum.Cells.Add(cellSum);

        //cellSum = new XRTableCell();
        //cellSum.WidthF = 130f;
        ////cellSum.BackColor = Color.Beige;
        //cellSum.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
        //cellSum.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
        //cellSum.Text = dSumVrednost.ToString("n2");
        //cellSum.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        //rowSum.Cells.Add(cellSum);

        if (showValue)
        {
            cellSum = new XRTableCell();
            cellSum.WidthF = 130f;
            //cellSum.BackColor = Color.Chartreuse;
            cellSum.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
            cellSum.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
            cellSum.Text = dSumVrednostTransporta.ToString("n2");
            cellSum.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            rowSum.Cells.Add(cellSum);
        }
        cellSum = new XRTableCell();
        cellSum.WidthF = 130f;
        //cellSum.BackColor = Color.DarkSalmon;
        cellSum.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
        cellSum.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
        cellSum.Text = dAvgProcentPrevoza.ToString("n2");
        cellSum.Font = new Font(cellSum.Font, FontStyle.Bold);
        cellSum.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        rowSum.Cells.Add(cellSum);

        cellSum = new XRTableCell();
        cellSum.WidthF = 130f;
        //cellSum.BackColor = Color.LightBlue;
        cellSum.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
        cellSum.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
        cellSum.Text = dSumKolicina.ToString("n2");
        cellSum.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        rowSum.Cells.Add(cellSum);


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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecallBuyer));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.session1 = new DevExpress.Xpo.Session(this.components);
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.lblPrevoznik = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDatumIzpisa = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblOrderNumber = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.RecallPositionTable = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
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
            this.tcPrevzemnik = new DevExpress.XtraReports.UI.XRTableCell();
            this.tcVrednost = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.lblOpomba = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.lblOpomba1 = new DevExpress.XtraReports.UI.XRRichText();
            ((System.ComponentModel.ISupportInitialize)(this.session1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RecallPositionTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOpomba1)).BeginInit();
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
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblPrevoznik,
            this.lblDatumIzpisa,
            this.xrPictureBox1,
            this.xrLabel6,
            this.lblOrderNumber,
            this.xrLabel3});
            this.ReportHeader.Dpi = 254F;
            this.ReportHeader.HeightF = 265.2748F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // lblPrevoznik
            // 
            this.lblPrevoznik.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPrevoznik.Dpi = 254F;
            this.lblPrevoznik.Font = new System.Drawing.Font("Candara", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblPrevoznik.LocationFloat = new DevExpress.Utils.PointFloat(883.7083F, 196.4798F);
            this.lblPrevoznik.Name = "lblPrevoznik";
            this.lblPrevoznik.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPrevoznik.SizeF = new System.Drawing.SizeF(1003.784F, 61.06587F);
            this.lblPrevoznik.StylePriority.UseBorders = false;
            this.lblPrevoznik.StylePriority.UseFont = false;
            // 
            // lblDatumIzpisa
            // 
            this.lblDatumIzpisa.Dpi = 254F;
            this.lblDatumIzpisa.Font = new System.Drawing.Font("Candara", 12F);
            this.lblDatumIzpisa.LocationFloat = new DevExpress.Utils.PointFloat(1970.584F, 196.4798F);
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
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(1970.584F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(751.4163F, 189.2533F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Dpi = 254F;
            this.xrLabel6.Font = new System.Drawing.Font("Candara", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(0.0002422333F, 196.4798F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(308.339F, 61.06586F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "Naročilnica:\t ";
            // 
            // lblOrderNumber
            // 
            this.lblOrderNumber.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblOrderNumber.Dpi = 254F;
            this.lblOrderNumber.Font = new System.Drawing.Font("Candara", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblOrderNumber.LocationFloat = new DevExpress.Utils.PointFloat(308.3392F, 196.4798F);
            this.lblOrderNumber.Name = "lblOrderNumber";
            this.lblOrderNumber.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblOrderNumber.SizeF = new System.Drawing.SizeF(392.5358F, 61.06586F);
            this.lblOrderNumber.StylePriority.UseBorders = false;
            this.lblOrderNumber.StylePriority.UseFont = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Candara", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(883.7083F, 0F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(1015.729F, 96.64912F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Povezane fakture";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
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
            this.Detail1.HeightF = 246.6339F;
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
            this.xrTableCell17,
            this.xrTableCell18,
            this.xrTableCell20,
            this.xrTableCell19,
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
            this.xrTableCell5.Weight = 0.12335046367845713D;
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
            this.xrTableCell9.Weight = 0.48422871374012744D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Font = new System.Drawing.Font("Candara", 10F);
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell12.Weight = 0.4064719220965044D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.Font = new System.Drawing.Font("Candara", 10F);
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell6.Weight = 0.40910377585003677D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Dpi = 254F;
            this.xrTableCell17.Font = new System.Drawing.Font("Candara", 10F);
            this.xrTableCell17.Multiline = true;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.StylePriority.UseFont = false;
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell17.Weight = 1.230905486185609D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Dpi = 254F;
            this.xrTableCell18.Font = new System.Drawing.Font("Candara", 10F);
            this.xrTableCell18.Multiline = true;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseFont = false;
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell18.Weight = 1.35725123533651D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Dpi = 254F;
            this.xrTableCell20.Font = new System.Drawing.Font("Candara", 10F);
            this.xrTableCell20.Multiline = true;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseFont = false;
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell20.Weight = 0.64514972004122473D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.Dpi = 254F;
            this.xrTableCell19.Font = new System.Drawing.Font("Candara", 10F);
            this.xrTableCell19.Multiline = true;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseFont = false;
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell19.Weight = 0.71293591984850935D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Font = new System.Drawing.Font("Candara", 10F);
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell7.Weight = 0.73570911026647012D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Font = new System.Drawing.Font("Candara", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseFont = false;
            this.xrTableCell8.Weight = 0.61009342755011087D;
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
            this.tcPrevzemnik,
            this.tcVrednost,
            this.xrTableCell15,
            this.xrTableCell16});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Font = new System.Drawing.Font("Candara", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseBorders = false;
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "#";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell3.Weight = 0.062669949517549409D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Font = new System.Drawing.Font("Candara", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseBorders = false;
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Ključ";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell10.Weight = 0.28732314209280169D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.Font = new System.Drawing.Font("Candara", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrTableCell11.Multiline = true;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseBorders = false;
            this.xrTableCell11.StylePriority.UseFont = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "Datum";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell11.Weight = 0.20500558757435972D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Font = new System.Drawing.Font("Candara", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseBorders = false;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "Valuta";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.20121830841641195D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Font = new System.Drawing.Font("Candara", 11F);
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Kupec";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.83430524361984293D;
            // 
            // tcPrevzemnik
            // 
            this.tcPrevzemnik.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tcPrevzemnik.Dpi = 254F;
            this.tcPrevzemnik.Font = new System.Drawing.Font("Candara", 11F);
            this.tcPrevzemnik.Name = "tcPrevzemnik";
            this.tcPrevzemnik.StylePriority.UseBorders = false;
            this.tcPrevzemnik.StylePriority.UseFont = false;
            this.tcPrevzemnik.StylePriority.UseTextAlignment = false;
            this.tcPrevzemnik.Text = "Prevzemnik";
            this.tcPrevzemnik.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tcPrevzemnik.Weight = 0.94920720699020888D;
            // 
            // tcVrednost
            // 
            this.tcVrednost.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tcVrednost.Dpi = 254F;
            this.tcVrednost.Font = new System.Drawing.Font("Candara", 11F);
            this.tcVrednost.Multiline = true;
            this.tcVrednost.Name = "tcVrednost";
            this.tcVrednost.StylePriority.UseBorders = false;
            this.tcVrednost.StylePriority.UseFont = false;
            this.tcVrednost.StylePriority.UseTextAlignment = false;
            this.tcVrednost.Text = "Vred. Trans";
            this.tcVrednost.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tcVrednost.Weight = 0.28829301549451841D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.Font = new System.Drawing.Font("Candara", 11F);
            this.xrTableCell15.Multiline = true;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseBorders = false;
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.Text = "Proc. Trans.";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell15.Weight = 0.32019153014232032D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell16.Dpi = 254F;
            this.xrTableCell16.Font = new System.Drawing.Font("Candara", 11F);
            this.xrTableCell16.Multiline = true;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StylePriority.UseBorders = false;
            this.xrTableCell16.StylePriority.UseFont = false;
            this.xrTableCell16.StylePriority.UseTextAlignment = false;
            this.xrTableCell16.Text = "KG";
            this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell16.Weight = 0.26353890986926987D;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblOpomba1,
            this.lblOpomba});
            this.GroupFooter1.Dpi = 254F;
            this.GroupFooter1.HeightF = 127.3319F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // lblOpomba
            // 
            this.lblOpomba.Dpi = 254F;
            this.lblOpomba.Font = new System.Drawing.Font("Candara", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblOpomba.LocationFloat = new DevExpress.Utils.PointFloat(0.0002422333F, 0F);
            this.lblOpomba.Multiline = true;
            this.lblOpomba.Name = "lblOpomba";
            this.lblOpomba.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblOpomba.SizeF = new System.Drawing.SizeF(279.2349F, 61.06586F);
            this.lblOpomba.StylePriority.UseFont = false;
            this.lblOpomba.Text = "Opomba:\t ";
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrLine2});
            this.PageFooter.Dpi = 254F;
            this.PageFooter.HeightF = 254F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Candara", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(1887.492F, 47.72892F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(308.339F, 45.19088F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Podpis šoferja";
            // 
            // xrLine2
            // 
            this.xrLine2.Dpi = 254F;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(2106.375F, 87.62813F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(603.25F, 5.291668F);
            // 
            // lblOpomba1
            // 
            this.lblOpomba1.Dpi = 254F;
            this.lblOpomba1.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.lblOpomba1.LocationFloat = new DevExpress.Utils.PointFloat(0.0002422333F, 61.06588F);
            this.lblOpomba1.Name = "lblOpomba1";
            this.lblOpomba1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lblOpomba1.SerializableRtfString = resources.GetString("lblOpomba1.SerializableRtfString");
            this.lblOpomba1.SizeF = new System.Drawing.SizeF(2722F, 58.41999F);
            // 
            // RecallBuyer
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
            this.Version = "19.2";
            ((System.ComponentModel.ISupportInitialize)(this.session1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RecallPositionTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOpomba1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
