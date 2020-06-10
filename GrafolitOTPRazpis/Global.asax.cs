using GrafolitOTPRazpis.Common;
using OptimizacijaTransprotov.Common;
using System;
using System.Web;

namespace GrafolitOTPRazpis {
    public class Global_asax : System.Web.HttpApplication {
        void Application_Start(object sender, EventArgs e) {
            DevExpress.Web.ASPxWebControl.CallbackError += new EventHandler(Application_Error);
        }

        void Application_End(object sender, EventArgs e) {
            // Code that runs on application shutdown
        }
    
        void Application_Error(object sender, EventArgs e) {

            string error = "";
            if (Context != null && Server.GetLastError() != null)
                CommonMethods.getError(Context.Error, ref error);

            if (HttpContext.Current.Error != null)
                CommonMethods.getError(HttpContext.Current.Error, ref error);

            //if is there error on client side we need aditional information about error

            error += "\r\n \r\n" + sender.GetType().FullName + "\r\n" + HttpContext.Current.Request.UrlReferrer.AbsoluteUri + "\r\n";

            string body = "Pozdravljeni! \r\n Uporabnik z ip-jem" + Request.UserHostAddress.ToString() + " je dne " + DateTime.Now.ToLongDateString() + " ob " + DateTime.Now.ToLongTimeString() +
                    " naletel na napako! \r\n Podrobnosti napake so navedene spodaj: \r\n \r\n" + error + "\r\n";

            bool isSent = CommonMethods.SendEmailToDeveloper("GrafolitOTPRazpis - NAPAKA", "Napaka aplikacije", body);

            CommonMethods.LogThis(body);

            if (Context != null)
                Context.ClearError();


            Server.ClearError();
        }
    
        void Session_Start(object sender, EventArgs e) {
            // Code that runs when a new session is started
        }
    
        void Session_End(object sender, EventArgs e) {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }
    }
}