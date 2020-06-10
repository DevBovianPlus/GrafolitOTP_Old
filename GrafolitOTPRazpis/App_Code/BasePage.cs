using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace GrafolitOTPRazpis.Common
{
    public class BasePage : ServerMasterPage
    {
        protected override void InitializeCulture()
        {
            string language = "en-us";

            //Detect User's Language.
            if (Request.UserLanguages != null)
            {
                //Set the Language.
                language = Request.UserLanguages[0];
            }

            if (Request.QueryString[Enums.QueryStringName.lang.ToString()] != null)
            {
                language = Request.QueryString[Enums.QueryStringName.lang.ToString()].ToString();
                InfrastructureHelper.SetCookieValue(Enums.Cookies.OTPTenderLanguage.ToString(), language);
            }
            else if (!String.IsNullOrEmpty(InfrastructureHelper.GetCookieValue(Enums.Cookies.OTPTenderLanguage.ToString())))
            {
                language = InfrastructureHelper.GetCookieValue(Enums.Cookies.OTPTenderLanguage.ToString());
            }

            //Set the Culture.
            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
        }
    }
}