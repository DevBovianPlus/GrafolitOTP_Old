using GrafolitOTPRazpis.Common;
using GrafolitOTPRazpis.Common.Models;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrafolitOTPRazpis
{
    public partial class SubmitToTender : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                if (e.Parameter == "CheckAuthentication")
                {
                    if (Request.QueryString[Enums.QueryStringName.id.ToString()] != null)
                    {//TODO: Preveri če že odpoklic spremenil status.
                        string encodedString = Request.QueryString[Enums.QueryStringName.id.ToString()].ToString();
                        string decodedString = CommonMethods.Base64Decode(encodedString);

                        if (decodedString.Contains(";"))
                        {
                            string[] split = decodedString.Split(';');

                            if (split.Length != 8)
                            {
                                CallbackPanel.JSProperties["cpQueryStringValidationError"] = true;
                                CommonMethods.LogThis("Napačna zahteva! Query string vrednost ne vsebuje potrebnih parametrov (';').");
                                return;
                            }

                            DecodedQueryStringModel model = new DecodedQueryStringModel();
                            model.PrijavaPrevoznikaID = CommonMethods.ParseInt(split[0]);
                            model.CenaPrevoza = CommonMethods.ParseDecimal(split[1]);
                            model.RelacijaID = CommonMethods.ParseInt(split[2]);
                            model.NazivRelacije = split[3];
                            model.DatumNaklada = DateTime.Parse(split[4], CultureInfo.GetCultureInfo("sl-SI"));
                            model.OpombaZaPovprasevnjePrevoznikom = split[5];
                            model.NazivPrevoznika = split[6];
                            model.StevilkaOdpoklica = split[7];

                            string result = GetDatabaseConnectionInstance().IsSubmittingPriceForCarrierStillValid(model.PrijavaPrevoznikaID);
                            if (result != "OK")
                            {
                                CallbackPanel.JSProperties["cpTimeForSubmittingPriceExpired"] = true;
                                CommonMethods.LogThis("Rok za vpis cene je potekel.");
                                return;
                            }

                            Session[Enums.CommonSession.DecodedQueryString.ToString()] = model; // ? zakaj bi bilo potrebno to shranjevati v sejo ?

                            txtPrice.Text = model.CenaPrevoza.ToString("N2") + "€";
                            txtRoute.Text = model.NazivRelacije;
                            txtStOdpoklica.Text = model.StevilkaOdpoklica;
                            DateEditDatumNaklada.Date = model.DatumNaklada;
                            lblPozdrav.Text = "Spoštovani " + model.NazivPrevoznika;
                            memOpombaPrevoznikov.Text = model.OpombaZaPovprasevnjePrevoznikom;
                        }
                        else
                        {
                            CallbackPanel.JSProperties["cpQueryStringValidationError"] = true;
                            CommonMethods.LogThis("Napačna zahteva! Query string vrednost ne vsebuje potrebnih parametrov (';').");
                        }
                    }
                    else
                    {
                        CallbackPanel.JSProperties["cpNoQueryStringError"] = true;
                        CommonMethods.LogThis("Napačna zahteva! Predvideni query string ne obstaja!");
                    }
                }
                else if (e.Parameter == "SubmitPrice")
                {
                    DecodedQueryStringModel model = (DecodedQueryStringModel)Session[Enums.CommonSession.DecodedQueryString.ToString()];
                    if (model != null)
                    {
                        decimal newPrice = CommonMethods.ParseDecimal(txtSubmitPrice.Text);
                        bool result = GetDatabaseConnectionInstance().SubmitPriceForCarrierTransport(model.PrijavaPrevoznikaID, newPrice);
                        RemoveSession(Enums.CommonSession.DecodedQueryString);

                        if (!result)
                        {
                            CommonMethods.LogThis("Napaka pri shranjevanju nove cene za prevoznika (prijava prevoznika id: " + model.PrijavaPrevoznikaID.ToString() + ")");
                            CallbackPanel.JSProperties["cpErrorOnSubmittingPrice"] = true;
                        }
                        else
                        {
                            CallbackPanel.JSProperties["cpRedirectToThankYouPage"] = true;
                        }
                    }
                    else
                    {
                        CommonMethods.LogThis("Napaka! Seja je prazna");
                        CallbackPanel.JSProperties["cpErrorOnSubmittingPrice"] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                CommonMethods.LogThis(error);

                string body = "Pozdravljeni! \r\n Uporabnik z ip-jem" + Request.UserHostAddress.ToString() + " je dne " + DateTime.Now.ToLongDateString() + " ob " + DateTime.Now.ToLongTimeString() +
                    " naletel na napako! \r\n Podrobnosti napake so navedene spodaj: \r\n \r\n" + error + "\r\n";

                CommonMethods.SendEmailToDeveloper("GrafolitOTPRazpis - NAPAKA", "Napaka aplikacije", body);
                CallbackPanel.JSProperties["cpError"] = true;
            }
        }
    }
}