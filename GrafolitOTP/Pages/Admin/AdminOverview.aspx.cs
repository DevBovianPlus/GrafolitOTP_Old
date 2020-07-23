﻿using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Admin
{
    public partial class AdminOverview : ServerMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();
            if (!PrincipalHelper.IsUserSuperAdmin()) RedirectHome();

            this.Master.PageHeadlineTitle = this.Title;
        }

        protected void CallbackPanelUserInput_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "ResetZaporednaStevilka")
            {
                bool isSuccess = CheckModelValidation(GetDatabaseConnectionInstance().ResetSequentialNumInRecallPos());
                if (isSuccess)
                    CallbackPanelUserInput.JSProperties["cpResetZaporednaStevilka"] = "Uspešno ste resetirali ZaporednaStevilka polje v tabeli OdpoklicPozicija";
                else
                    CallbackPanelUserInput.JSProperties["cpError"] = "Prišlo je do napake.";
            }
            else if (e.Parameter == "PrevzemiPotrjeneOdpoklice")
            {
                bool? isSuccess = CheckModelValidation(GetDatabaseConnectionInstance().TakeOverConfirmedRecalls());
                if (isSuccess.HasValue && isSuccess.Value)
                    CallbackPanelUserInput.JSProperties["cpPrevzemiPotrjeneOdpoklice"] = "Uspešno ste zaključili potrjene odpoklice!";
                else
                    CallbackPanelUserInput.JSProperties["cpError"] = "Operacija je obsežna in potrebuje več časa da se sprocesira. Medtem lahko nemoteno uporabljate aplikacijo.";
            }
            else if (e.Parameter == "PreveriPovprasevanjeZaOdpoklice")
            {
                bool? isSuccess = CheckModelValidation(GetDatabaseConnectionInstance().CheckRecallsForCarriersSubmittingPrices());
                if (isSuccess.HasValue && isSuccess.Value)
                    CallbackPanelUserInput.JSProperties["cpPreveriPovprasevanjeZaOdpoklice"] = "Uspešno ste potrdili odpoklice pri katerih so se prevozniki prijavljali svojo ceno!";
                else
                    CallbackPanelUserInput.JSProperties["cpError"] = "Operacija je obsežna in potrebuje več časa da se sprocesira. Medtem lahko nemoteno uporabljate aplikacijo.";
            }
            else if (e.Parameter == "OdpokliciBrezPonudb")
            {
                bool? isSuccess = CheckModelValidation(GetDatabaseConnectionInstance().CheckForRecallsWithNoSubmitedPrices());
                if (isSuccess.HasValue && isSuccess.Value)
                    CallbackPanelUserInput.JSProperties["cpPreveriPovprasevanjeZaOdpoklice"] = "Uspešno ste potrdili odpoklice pri katerih so se prevozniki prijavljali svojo ceno!";
                else
                    CallbackPanelUserInput.JSProperties["cpError"] = "Operacija je obsežna in potrebuje več časa da se sprocesira. Medtem lahko nemoteno uporabljate aplikacijo.";
            }
        }

        protected void btnbtnGenerateXMLPrevoznik_Click(object sender, EventArgs e)
        {
            //CheckModelValidation(GetDatabaseConnectionInstance().CreateOrderXMLByType(2554));
        }

        protected void btnLaunchPDFPantheon_Click(object sender, EventArgs e)
        {
            CheckModelValidation(GetDatabaseConnectionInstance().LaunchPantheonCreatePDF());
        }

        protected void btnCheckPDFExist_Click(object sender, EventArgs e)
        {            
            lblRezultat.Text = GetDatabaseConnectionInstance().GetOrderPDFFile( Convert.ToInt32( txtPath.Text));
        }

        protected void btnNoRecall_Click(object sender, EventArgs e)
        {
            CheckModelValidation(GetDatabaseConnectionInstance().CheckForOrderTakeOver2());
        }

        protected void btnAvtomatskaIzbira_Click(object sender, EventArgs e)
        {
            CheckModelValidation(GetDatabaseConnectionInstance().CheckRecallsForCarriersSubmittingPrices());
        }

        protected void btnAvtomatskaIzbira2_Click(object sender, EventArgs e)
        {
            CheckModelValidation(GetDatabaseConnectionInstance().CheckForRecallsWithNoSubmitedPrices());
        }

        protected void btnCreadeAndSendOrders_Click(object sender, EventArgs e)
        {
            CheckModelValidation(GetDatabaseConnectionInstance().CreateAndSendOrdersMultiple());
        }

        protected void btnCreatePDFAndSendPDOOrdersMultiple_Click(object sender, EventArgs e)
        {
            CheckModelValidation(GetDatabaseConnectionInstance().CreatePDFAndSendPDOOrdersMultiple());
        }

        protected void btnGetConfigVal_Click(object sender, EventArgs e)
        {
            lblRezultat2.Text = CheckModelValidation(GetDatabaseConnectionInstance().GetConfigValue(txtConfigNameRet.Text));
        }

        protected void btnChangeConfig_Click(object sender, EventArgs e)
        {
            CheckModelValidation(GetDatabaseConnectionInstance().ChangeConfigValue(txtConfigName.Text, txtConfigValue.Text));
        }

        protected void btnGetLogs_Click(object sender, EventArgs e)
        {
            byte[] bytes = CheckModelValidation(GetDatabaseConnectionInstance().GetWebServiceLogFile());
            byte[] UtilityServbytes = CheckModelValidation(GetDatabaseConnectionInstance().GetUtilityServiceLogFile());

            string applicationLogFile = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
            byte[] applicationBytes = System.IO.File.ReadAllBytes(applicationLogFile);

            List<FileToDownload> list = new List<FileToDownload> { new FileToDownload { Name = "WebServiceLog.txt", Content = bytes, Extension=".txt" },
                new FileToDownload { Name = "ApplicationLog", Content = applicationBytes, Extension=".txt" }, new FileToDownload { Name = "UtilityServiceLog.txt", Content = UtilityServbytes, Extension=".txt" } };

            byte[] zip = CommonMethods.GetZipMemmoryStream(list);

            Response.Clear();
            Response.ContentType = "application/zip";
            Response.AddHeader("content-disposition", "attachment;filename=Logs.zip");
            Response.Buffer = true;
            Response.BinaryWrite(zip);

            Response.Flush();
            Response.End();
        }
    }
}