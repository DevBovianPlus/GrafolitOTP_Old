using DatabaseWebService.ModelsOTP;
using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.ModelsOTP.Recall;
using DatabaseWebService.ModelsOTP.Tender;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.Data;
using DevExpress.Web.Rendering;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Helpers.Models;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OptimizacijaTransprotov.Pages.Recall
{
    public partial class CreateOrder_popup : ServerMasterPage
    {
        RecallFullModel model;
        int action = -1;
        string positionCode = "STORITEV";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            action = CommonMethods.ParseInt(GetStringValueFromSession(Enums.CommonSession.UserActionPopUp));

            if (model == null && SessionHasValue(Enums.RecallSession.RecallFulModel))
                model = GetRecallDataProvider().GetRecallFullModel();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }


        #region Initialize
        private void Initialize()
        {
            ASPxGridViewServices.DataBind();
            AddHtmlDefaultOpombe();
        }

        #endregion

        #region Helper methods

        private void AddHtmlDefaultOpombe()
        {
            HtmlEditorNotes.Html = "<br/>" +
                "<br/>" +
                "<b>Opomba: vozilo ne bo razloženo/naloženo v kolikor vozilo in voznik nimata ustrezne opreme, oziroma obutve in oblačil (zakonsko predpisane varovalne opreme), ter če voznik ne upošteva pravila in navodil na kraju naklada/razklada." +
                "<br/>Za neupoštevanje navodil odštejemo 30 % od plačila računa." +
                "<br/>" +
                "<br/>" +
                "Note: The vehicle will not be unloaded / loaded as far as vehicle and driver do not have the appropriate equipment and clothing and footwear (statutory protective equipment), and if the driver does not take account the rules and instructions at the place of loading / unloading. " +
                "<br/>For ignoring of instructions we will deduct 30 % from the payment of the invoice!</b>" +
                "<br/>" +
                "<br/>" +
                "<br/>" +
                "VOZNIK MORA BITI PRISOTEN PRI NAKLADU IN RAZKLADU BLAGA!DIREKTNI KONTAKT S STRANKO NI DOVOLJEN.V PRIMERU POŠKODBE BLAGA, MORA LE - TO BITI OZNAČENO NA TOV. LISTU. " +
                "<br/>PRIDRŽUJEMO SI PRAVICO ZARAČUNATI VAM STROŠKE OB NEIZVRŠITVI NALOGA.PREVOZNIK MORA IMETI ZAVAROVANO PREVOZNIŠKO ODGOVORNOST. OB IZSTAVITVI RAČUNA MORA BITI PRILOŽENA ORIGINAL KOPIJA TOVORNEGA LISTA, POTRJENA OD PREJEMNIKA BLAGA.BREZ CMR-a RAČUN NE PRIZNAMO." +
                "<br/>" +
                "<br/>" +
                "THE DRIVER MUST TO BE PRESENT AT LOADING AND UNLOADING OF GOODS!DIRECT CONTACT WITH CUSTOMER ISN`T ALLOWED. IN CASE OF INJURY OF GOODS, IT MUST BE MARKED ON WAYBILL(CMR).WE ARE ENTITLED TO BILL THE CARRIER THE COSTS IN CASE OF NOT FULLFILLING THE CONFIRMED ORDER. CARRIER MUST HAVE THE INSURANCE OF TRANSPORT RESPONSIBILITY. ALONG WITH THE ISSUED INVOICE, THE ORIGINAL COPY OF THE CMR, CONFIRMED BY THE RECEIVER OF GOODS, MUST BE ENCLOSED.WITHOUT THE CMR THE INVOICE HAS NO VALIDITY.";
        }

        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.OrderFromRecallSession.ServiceList);

            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}','{2}','{3}');", confirmCancelAction, "CreateOrder", 2, model.OdpoklicID), true);
            RemoveSession(Enums.RecallSession.RecallFulModel);
        }
        #endregion

        private string ConverHtmlToRtf(ASPxHtmlEditor editor)
        {
            MemoryStream stream = new MemoryStream();
            HtmlEditorNotes.Export(DevExpress.Web.ASPxHtmlEditor.HtmlEditorExportFormat.Rtf, stream);

            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string tekst = ConverHtmlToRtf(HtmlEditorNotes);
            List<ServiceListModel> storitve = GetServices();
            
            CreateOrderModel createorder = new CreateOrderModel
            {
                services = storitve,
                RecallID = model.OdpoklicID,
                Note = tekst,
                TypeCode = "0240"

            };
            var obj = GetDatabaseConnectionInstance().CreateOrderTransport(createorder);
            CheckModelValidation(obj);

            if (obj.IsRequestSuccesful) RemoveSessionsAndClosePopUP(true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RemoveSessionsAndClosePopUP();
        }

        protected void ASPxGridViewServices_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = GetServices();
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridViewServices_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "AddServicePosition")
            {
                List<ServiceListModel> services = GetServices();
                services.Add(new ServiceListModel() { ServiceID = services.Count + 1, Code = positionCode });
                GetRecallDataProvider().SetServices(services);

                ASPxGridViewServices.DataBind();
            }
        }

        protected void ASPxGridViewServices_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            List<ServiceListModel> services = GetServices();

            if (services != null)
            {
                ServiceListModel service = null;

                Type myType = typeof(ServiceListModel);
                List<PropertyInfo> myPropInfo = myType.GetProperties().ToList();

                //Spreminjanje zapisov v gridu
                foreach (ASPxDataUpdateValues item in e.UpdateValues)
                {
                    service = new ServiceListModel();

                    foreach (DictionaryEntry obj in item.Keys)//we set table ID
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                        {
                            service = services.Where(s => s.ServiceID == (int)obj.Value).FirstOrDefault();
                            break;
                        }
                    }

                    foreach (DictionaryEntry obj in item.NewValues)
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                        {
                            info.SetValue(service, obj.Value);
                        }
                    }
                }

                //Brisanje zapisov v gridu
                foreach (ASPxDataDeleteValues item in e.DeleteValues)
                {
                    service = new ServiceListModel();

                    foreach (DictionaryEntry obj in item.Keys)//we set table ID
                    {
                        PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                        if (info != null)
                        {
                            service = services.Where(s => s.ServiceID == (int)obj.Value).FirstOrDefault();
                            break;
                        }
                    }
                    services.Remove(service);
                }

                GetRecallDataProvider().SetServices(services);
            }

            e.Handled = true;
        }

        private List<ServiceListModel> GetServices()
        {
            List<ServiceListModel> services = GetRecallDataProvider().GetServices();
            if (model != null)
            {
                if (services == null)
                {
                    services = new List<ServiceListModel>();
                    services.Add(new ServiceListModel() { ServiceID = 1, Code = "STORITEV", Name = "PREVOZ " + model.Relacija.Naziv, Price = model.CenaPrevoza, UnitOfMeasure = "ST", Quantity = 1 });
                    GetRecallDataProvider().SetServices(services);
                }
            }
            return services;
        }
    }
}