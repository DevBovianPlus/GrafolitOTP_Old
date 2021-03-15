using DatabaseWebService.ModelsOTP.Recall;
using DevExpress.Web;
using DevExpress.XtraPrintingLinks;
using OptimizacijaTransprotov.Helpers;
using OptimizacijaTransprotov.Helpers.DataProviders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.UI;

namespace OptimizacijaTransprotov.Common
{
    public static class CommonMethods
    {
        public static int ParseInt(object param)
        {
            int num = 0;

            if (param != null)
            {
                int.TryParse(param.ToString(), out num);

                if (num < 0)
                    num = 0;
            }

            return num;
        }

        public static Nullable<int> ParseNullableInt(object param)
        {
            int num = 0;

            if (param != null)
            {
                int.TryParse(param.ToString(), out num);

                if (num < 0)
                    return null;

                return num;
            }
            else
                return null;
        }

        public static decimal ParseDecimal(object param)
        {
            decimal num = 0;
            if (param != null)
            {
                decimal.TryParse(param.ToString(), out num);

                if (num < 0)
                    num = 0;
            }

            return num;
        }

        public static double ParseDouble(object param)
        {
            double num = 0;
            if (param != null)
            {
                double.TryParse(param.ToString(), out num);

                if (num < 0)
                    num = 0;
            }

            return num;
        }

        public static bool ParseBool(string param)
        {
            bool value = false;
            bool.TryParse(param, out value);

            return value;
        }

        public static bool ParseBool(object param)
        {
            bool variable = false;
            if (param != null)
            {
                bool.TryParse(param.ToString(), out variable);
            }

            return variable;
        }

        public static string PreveriZaSumnike(string _crka)
        {
            char crkaC = ' ';
            string novS = "";

            _crka = _crka.ToUpper();

            foreach (char item in _crka)
            {
                switch (item)
                {
                    case 'Č':
                        crkaC = 'C';
                        break;
                    case 'Š':
                        crkaC = 'S';
                        break;
                    case 'Ž':
                        crkaC = 'Z';
                        break;
                    case 'Đ':
                        crkaC = 'D';
                        break;
                    default:
                        crkaC = item;
                        break;
                }

                novS += crkaC.ToString();
            }

            return novS;
        }

        public static string Trim(string sTrim)
        {
            return String.IsNullOrEmpty(sTrim) ? "" : sTrim.Trim();
        }

        public static void LogThis(string message)
        {
            bool isLoggingEnabled = CommonMethods.ParseBool(ConfigurationManager.AppSettings["EnableLogging"]);

            if (isLoggingEnabled)
            {
                var directory = AppDomain.CurrentDomain.BaseDirectory;
                File.AppendAllText(directory + "log.txt", DateTime.Now + " " + message + Environment.NewLine);
            }
        }

        public static bool SendEmailToDeveloper(string displayName, string subject, string body)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;//Port 465 (SSL required)
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("bovianplus@gmail.com", "Geslo123.");
                client.Timeout = 6000;

                MailMessage message;

                message = new MailMessage();
                message.To.Add(new MailAddress("martin@bovianplus.si"));
                message.To.Add(new MailAddress("boris.dolinsek@bovianplus.si"));

                message.Sender = new MailAddress("bovianplus@gmail.com");
                message.From = new MailAddress("bovianplus@gmail.com", displayName);
                message.Subject = subject;
                message.IsBodyHtml = false;
                message.Body = body;
                message.BodyEncoding = Encoding.UTF8;

                client.Send(message);

            }
            catch (SmtpFailedRecipientsException ex)
            {
                LogThis(ex.Message + "\r\n " + ex.Source + "\r\n " + ex.StackTrace);
                return false;
            }
            catch (SmtpException ex)
            {
                LogThis(ex.Message + "\r\n " + ex.Source + "\r\n " + ex.StackTrace);
                return false;
            }
            catch (Exception ex)
            {
                LogThis(ex.Message + "\r\n " + ex.Source + "\r\n " + ex.StackTrace);
                return false;
            }

            return true;
        }

        public static void getError(Exception e, ref string errors)
        {
            if (e.GetType() != typeof(HttpException)) errors += " -------- " + e.ToString();
            if (e.InnerException != null) getError(e.InnerException, ref errors);
        }

        public static bool IsCallbackRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            var context = HttpContext.Current;
            var isCallbackRequest = false;// callback requests are ajax requests
            if (context != null && context.CurrentHandler != null && context.CurrentHandler is System.Web.UI.Page)
            {
                isCallbackRequest = ((System.Web.UI.Page)context.CurrentHandler).IsCallback;
            }
            return isCallbackRequest || (request["X-Requested-With"] == "XMLHttpRequest") || (request.Headers["X-Requested-With"] == "XMLHttpRequest");
        }

        public static string GetTimeStamp()
        {
            return PrincipalHelper.GetUserPrincipal().firstName + "_" + PrincipalHelper.GetUserPrincipal().lastName + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm");
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static byte[] GetZipMemmoryStream(List<FileToDownload> fileList)
        {
            using (var ms = new MemoryStream())
            {//Create an archive and store the stream in memory.
                using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var item in fileList)
                    {
                        //Create a zip entry for each attachment
                        var zipEntry = zipArchive.CreateEntry(item.Name + item.Extension);

                        //Get the stream of the attachment
                        using (var originalFileStream = new MemoryStream(item.Content))
                        {
                            using (var zipEntryStream = zipEntry.Open())
                            {
                                //Copy the attachment stream to the zip entry stream
                                originalFileStream.CopyTo(zipEntryStream);
                            }
                        }
                    }
                }

                return ms.ToArray();
            }
        }

        public static void ExportToPDFFitToPage(ASPxGridViewExporter GridViewExporter, Page pg)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PrintableComponentLinkBase pcl = new PrintableComponentLinkBase(new DevExpress.XtraPrinting.PrintingSystemBase());
                pcl.Component = GridViewExporter;
                pcl.Margins.Left = pcl.Margins.Right = 50;
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                pcl.PrintingSystemBase.Document.AutoFitToPagesWidth = 1;
                pcl.ExportToPdf(ms);
                WriteResponse(pg.Response, ms.ToArray(), System.Net.Mime.DispositionTypeNames.Inline.ToString());
            }
        }

        public static void WriteResponse(HttpResponse response, byte[] filearray, string type)
        {
            response.ClearContent();
            response.Buffer = true;
            response.Cache.SetCacheability(HttpCacheability.Private);
            response.ContentType = "application/pdf";
            ContentDisposition contentDisposition = new ContentDisposition();
            contentDisposition.FileName = "test.pdf";
            contentDisposition.DispositionType = type;
            response.AddHeader("Content-Disposition", contentDisposition.ToString());
            response.BinaryWrite(filearray);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            try
            {
                response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }

        }

        public static void WriteDocumentToResponse(Page pCurrentPage, byte[] documentData, string format, bool isInline, string fileName)
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
                pCurrentPage.Response.Clear();
                pCurrentPage.Response.ContentType = contentType;

                pCurrentPage.Response.ClearHeaders();

                CommonMethods.LogThis("Before Add header");
                pCurrentPage.Response.AddHeader("Content-Disposition", String.Format("{0}; filename={1}", disposition, fileName));
                pCurrentPage.Response.AddHeader("Content-Length", documentData.Length.ToString());

                pCurrentPage.Response.Clear(); // dodal boris - 21.02.2019       
                pCurrentPage.Response.BufferOutput = false;
                pCurrentPage.Response.ClearContent();

                CommonMethods.LogThis("Before Binnarywrite");
                pCurrentPage.Response.BinaryWrite(documentData);

                pCurrentPage.Response.Flush();

                //Response.Close();
                //Response.End();

                pCurrentPage.Response.SuppressContent = true;
            }
            catch (Exception ex)
            {
                CommonMethods.LogThis(ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace);

            }
        }

        public static string RemoveForbidenChracters(string possibleWorksheetName)
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

        public static RecallBuyerFullModel CalculatePercentageShippingCost(RecallBuyerFullModel model)
        {
            var sum = (from t in model.OdpoklicKupecPozicija where t.Akcija != (int)Enums.UserAction.Delete select t.Vrednost).Sum();

            if (sum != 0)
            {
                model.ProcentPrevozaSkupno = (model.CenaPrevozaSkupno * 100) / sum;
            }


            foreach (RecallBuyerPositionModel pos in model.OdpoklicKupecPozicija)
            {
                pos.VrednostPrevoza = (pos.Vrednost * model.ProcentPrevozaSkupno) / 100;
                pos.ProcentPrevoza = model.ProcentPrevozaSkupno;
            }

            return model;
        }

        public static void Redirect(this HttpResponse response, string url, string target, string windowFeatures)
        {

            if ((String.IsNullOrEmpty(target) || target.Equals("_self", StringComparison.OrdinalIgnoreCase)) && String.IsNullOrEmpty(windowFeatures))
            {
                response.Redirect(url);
            }
            else
            {
                Page page = (Page)HttpContext.Current.Handler;

                if (page == null)
                {
                    throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }
                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page, typeof(Page), "Redirect", script, true);
            }
        }
    }
}