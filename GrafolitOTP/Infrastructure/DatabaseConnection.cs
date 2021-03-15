using DatabaseWebService.Models;
using DatabaseWebService.Models.Client;
using DatabaseWebService.ModelsOTP;
using DatabaseWebService.ModelsOTP.Client;
using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.ModelsOTP.Recall;
using DatabaseWebService.ModelsOTP.Route;
using DatabaseWebService.ModelsOTP.Tender;
using Newtonsoft.Json;
using OptimizacijaTransprotov.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace OptimizacijaTransprotov.Infrastructure
{
    public class DatabaseConnection
    {
        public UserModel SignIn(string username, string password)
        {
            WebResponseContentModel<UserModel> user = GetResponseFromWebRequest<WebResponseContentModel<UserModel>>(WebServiceHelper.SignIn(username, password), "get");
            return user.Content;
        }

        public WebResponseContentModel<byte[]> GetWebServiceLogFile()
        {
            WebResponseContentModel<byte[]> user = new WebResponseContentModel<byte[]>();
            try
            {
                user = GetResponseFromWebRequest<WebResponseContentModel<byte[]>>(WebServiceHelper.GetWebServiceLogFile(), "get");
            }
            catch (Exception ex)
            {
                user.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }
            return user;
        }

        public WebResponseContentModel<byte[]> GetUtilityServiceLogFile()
        {
            WebResponseContentModel<byte[]> user = new WebResponseContentModel<byte[]>();
            try
            {
                user = GetResponseFromWebRequest<WebResponseContentModel<byte[]>>(WebServiceHelper.GetUtilityServiceLogFile(), "get");
            }
            catch (Exception ex)
            {
                user.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }
            return user;
        }


      


        #region Order
        public WebResponseContentModel<List<OrderModel>> GetOrders()
        {
            WebResponseContentModel<List<OrderModel>> dt = new WebResponseContentModel<List<OrderModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<OrderModel>>>(WebServiceHelper.GetAllOrders(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<OrderPositionModel>> GetOrdersPositions()
        {
            WebResponseContentModel<List<OrderPositionModel>> dt = new WebResponseContentModel<List<OrderPositionModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<OrderPositionModel>>>(WebServiceHelper.GetAllOrdersPoistions(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<SupplierModel>> GetAllSuppliers()
        {
            WebResponseContentModel<List<SupplierModel>> dt = new WebResponseContentModel<List<SupplierModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<SupplierModel>>>(WebServiceHelper.GetAllSuppliers(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<OrderPositionModelNew>> GetOrderPositionsBySupplier(string supplier, int clientID = 0)
        {
            WebResponseContentModel<List<OrderPositionModelNew>> dt = new WebResponseContentModel<List<OrderPositionModelNew>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<OrderPositionModelNew>>>(WebServiceHelper.GetAllOrdersPositionsBySupplier(supplier, clientID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<OrderPositionModelNew>> GetListOfOrderNumber10()
        {
            WebResponseContentModel<List<OrderPositionModelNew>> dt = new WebResponseContentModel<List<OrderPositionModelNew>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<OrderPositionModelNew>>>(WebServiceHelper.GetListOfOrderNumber10(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }
        //public WebResponseContentModel<List<ProductCategory>> GetCategoryList()
        //{
        //    WebResponseContentModel<List<ProductCategory>> client = new WebResponseContentModel<List<ProductCategory>>();
        //    try
        //    {
        //        client = GetResponseFromWebRequest<WebResponseContentModel<List<ProductCategory>>>(WebServiceHelper.GetCategoryList(), "get");
        //    }
        //    catch (Exception ex)
        //    {
        //        client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
        //    }

        //    return client;
        //}

        #endregion

        #region Recall

        public WebResponseContentModel<List<RecallModel>> GetAllRecalls()
        {
            WebResponseContentModel<List<RecallModel>> dt = new WebResponseContentModel<List<RecallModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<RecallModel>>>(WebServiceHelper.GetAllRecalls(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<RecallFullModel> GetRecallByID(int recallID)
        {
            WebResponseContentModel<RecallFullModel> dt = new WebResponseContentModel<RecallFullModel>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<RecallFullModel>>(WebServiceHelper.GetRecallByID(recallID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }


        public WebResponseContentModel<RecallBuyerFullModel> GetRecallBuyerByID(int recallID)
        {
            WebResponseContentModel<RecallBuyerFullModel> dt = new WebResponseContentModel<RecallBuyerFullModel>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<RecallBuyerFullModel>>(WebServiceHelper.GetRecallBuyerByID(recallID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<RecallFullModel> SaveRecall(RecallFullModel newData)
        {
            WebResponseContentModel<RecallFullModel> model = new WebResponseContentModel<RecallFullModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<RecallFullModel>>(WebServiceHelper.SaveRecall(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<RecallBuyerFullModel> SaveBuyerRecall(RecallBuyerFullModel newData)
        {
            WebResponseContentModel<RecallBuyerFullModel> model = new WebResponseContentModel<RecallBuyerFullModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<RecallBuyerFullModel>>(WebServiceHelper.SaveBuyerRecall(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteRecall(int recallID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteRecall(recallID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteBuyerRecall(int recallID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteBuyerRecall(recallID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<RecallPositionModel> GetRecallPositionByID(int recallPosID)
        {
            WebResponseContentModel<RecallPositionModel> dt = new WebResponseContentModel<RecallPositionModel>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<RecallPositionModel>>(WebServiceHelper.GetRecallPositionByID(recallPosID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<RecallPositionModel> SaveRecallPosition(RecallPositionModel newData)
        {
            WebResponseContentModel<RecallPositionModel> model = new WebResponseContentModel<RecallPositionModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<RecallPositionModel>>(WebServiceHelper.SaveRecallPosition(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<RecallPositionModel>> SaveRecallPosition(List<RecallPositionModel> newData)
        {
            WebResponseContentModel<List<RecallPositionModel>> model = new WebResponseContentModel<List<RecallPositionModel>>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<List<RecallPositionModel>>>(WebServiceHelper.SaveRecallPositions(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteRecallPosition(int recallPosID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteRecallPosition(recallPosID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<RecallType> GetRecallTypeByID(int typeID)
        {
            WebResponseContentModel<RecallType> dt = new WebResponseContentModel<RecallType>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<RecallType>>(WebServiceHelper.GetRecallTypeByID(typeID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<RecallType> GetRecallTypeByCode(string typeCode)
        {
            WebResponseContentModel<RecallType> dt = new WebResponseContentModel<RecallType>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<RecallType>>(WebServiceHelper.GetRecallTypeByCode(typeCode), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<RecallType>> GetRecallTypes()
        {
            WebResponseContentModel<List<RecallType>> dt = new WebResponseContentModel<List<RecallType>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<RecallType>>>(WebServiceHelper.GetRecallTypes(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<RecallStatus> GetRecallStatusByID(int statusID)
        {
            WebResponseContentModel<RecallStatus> dt = new WebResponseContentModel<RecallStatus>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<RecallStatus>>(WebServiceHelper.GetRecallStatusByID(statusID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<RecallStatus> GetRecallStatusByCode(string statusCode)
        {
            WebResponseContentModel<RecallStatus> dt = new WebResponseContentModel<RecallStatus>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<RecallStatus>>(WebServiceHelper.GetRecallStatusByCode(statusCode), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<RecallStatus>> GetRecallStatuses()
        {
            WebResponseContentModel<List<RecallStatus>> dt = new WebResponseContentModel<List<RecallStatus>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<RecallStatus>>>(WebServiceHelper.GetRecallStatuses(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<MaterialModel>> GetLatestQuantityForProduct(List<MaterialModel> idents)
        {
            WebResponseContentModel<List<MaterialModel>> model = new WebResponseContentModel<List<MaterialModel>>();

            try
            {
                model.Content = idents;
                model = PostWebRequestData<WebResponseContentModel<List<MaterialModel>>>(WebServiceHelper.GetLatestKolicinaOTPForProduct(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<RecallPositionModel>> GetRecallPosFromPartialOverTakeRecalls(List<int> recallIDs)
        {
            WebResponseContentModel<List<int>> model = new WebResponseContentModel<List<int>>();
            WebResponseContentModel<List<RecallPositionModel>> returnModel = new WebResponseContentModel<List<RecallPositionModel>>();

            try
            {
                model.Content = recallIDs;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(WebServiceHelper.GetRecallPosFromPartialOverTakeRecalls());
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";

                using (var sw = new StreamWriter(request.GetRequestStream()))
                {
                    string clientData = JsonConvert.SerializeObject(model);
                    sw.Write(clientData);
                    sw.Flush();
                    sw.Close();
                }


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string streamString = reader.ReadToEnd();

                returnModel = JsonConvert.DeserializeObject<WebResponseContentModel<List<RecallPositionModel>>>(streamString);

            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return returnModel;
        }

        public WebResponseContentModel<bool> ResetSequentialNumInRecallPos()
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.ResetSequentialNumInRecallPos(), "get");

            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> TakeOverConfirmedRecalls()
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.TakeOverConfirmedRecalls(), "get");

            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<RecallModel>> GetAllTakeOverRecalls()
        {
            WebResponseContentModel<List<RecallModel>> dt = new WebResponseContentModel<List<RecallModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<RecallModel>>>(WebServiceHelper.GetAllTakeOverRecalls(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<RecallModel>> GetAllNoneTakeOverRecalls()
        {
            WebResponseContentModel<List<RecallModel>> dt = new WebResponseContentModel<List<RecallModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<RecallModel>>>(WebServiceHelper.GetAllNoneTakeOverRecalls(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<RecallBuyerModel>> GetAllBuyersRecalls()
        {
            WebResponseContentModel<List<RecallBuyerModel>> dt = new WebResponseContentModel<List<RecallBuyerModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<RecallBuyerModel>>>(WebServiceHelper.GetAllBuyersRecalls(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }


        public string IsSubmittingPriceForCarrierStillValid(int prijavaPrevoznikaID)
        {
            WebResponseContentModel<string> dt = new WebResponseContentModel<string>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<string>>(WebServiceHelper.IsSubmittingPriceForCarrierStillValid(prijavaPrevoznikaID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            if (dt.IsRequestSuccesful)
                return dt.Content;
            else
                return dt.Content = "Server side error: " + dt.ValidationError + "APP side erro: " + dt.ValidationErrorAppSide;

        }

        public bool SubmitPriceForCarrierTransport(int prijavaPrevoznikaID, decimal newPrice)
        {
            WebResponseContentModel<bool> dt = new WebResponseContentModel<bool>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.SubmitPriceForCarrierTransport(prijavaPrevoznikaID, newPrice), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            if (dt.IsRequestSuccesful)
                return dt.Content;
            else
                return false;
        }

        public WebResponseContentModel<bool> CheckRecallsForCarriersSubmittingPrices()
        {
            WebResponseContentModel<bool> dt = new WebResponseContentModel<bool>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.CheckRecallsForCarriersSubmittingPrices(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<bool> CheckForRecallsWithNoSubmitedPrices()
        {
            WebResponseContentModel<bool> dt = new WebResponseContentModel<bool>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.CheckForRecallsWithNoSubmitedPrices(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<CarrierInquiryModel>> GetCarriersInquiry(int recallID)
        {
            WebResponseContentModel<List<CarrierInquiryModel>> dt = new WebResponseContentModel<List<CarrierInquiryModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<CarrierInquiryModel>>>(WebServiceHelper.GetCarriersInquiry(recallID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<RecallFullModel> SaveNewAddedCarrierForInquiry(RecallFullModel newData)
        {
            WebResponseContentModel<RecallFullModel> model = new WebResponseContentModel<RecallFullModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<RecallFullModel>>(WebServiceHelper.SaveNewAddedCarrierForInquiry(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<CarrierInquiryModel>> ReSendEmailToCarriers(List<CarrierInquiryModel> newData)
        {
            WebResponseContentModel<List<CarrierInquiryModel>> model = new WebResponseContentModel<List<CarrierInquiryModel>>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<List<CarrierInquiryModel>>>(WebServiceHelper.ReSendEmailToCarriers(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> ManualSelectCarrierForTransport(int prijavaPrevoznikaID)
        {
            WebResponseContentModel<bool> dt = new WebResponseContentModel<bool>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.ManualSelectCarrierForTransport(prijavaPrevoznikaID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<bool> DeleteCarrierInquiry(int prijavaPrevoznikaID)
        {
            WebResponseContentModel<bool> dt = new WebResponseContentModel<bool>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteCarrierInquiry(prijavaPrevoznikaID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<CreateOrderModel> CreateOrderTransport(CreateOrderModel newData)
        {
            WebResponseContentModel<CreateOrderModel> model = new WebResponseContentModel<CreateOrderModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<CreateOrderModel>>(WebServiceHelper.CreateOrderTransport(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> LaunchPantheonCreatePDF()
        {
            WebResponseContentModel<bool> dt = new WebResponseContentModel<bool>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.LaunchPantheonCreatePDF(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

       

        public string GetOrderPDFFile(int iRecallID)
        {
            WebResponseContentModel<string> dt = new WebResponseContentModel<string>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<string>>(WebServiceHelper.GetOrderPDFFile(iRecallID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            if (dt.IsRequestSuccesful)
                return dt.Content;
            else
                return dt.Content = "Server side error: " + dt.ValidationError + "APP side erro: " + dt.ValidationErrorAppSide;

        }

        public WebResponseContentModel<bool> CheckForOrderTakeOver2()
        {
            WebResponseContentModel<bool> dt = new WebResponseContentModel<bool>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.CheckForOrderTakeOver2(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<bool> CreateAndSendOrdersMultiple()
        {
            WebResponseContentModel<bool> dt = new WebResponseContentModel<bool>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.CreateAndSendOrdersMultiple(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

       
        public WebResponseContentModel<bool> ChangeConfigValue(string sConfigName, string sConfigValue)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.ChangeConfigValue(sConfigName, sConfigValue), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<string> GetConfigValue(string sConfigName)
        {
            WebResponseContentModel<string> model = new WebResponseContentModel<string>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<string>>(WebServiceHelper.GetConfigValue(sConfigName), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> ResetRecallStatusByID(int orderID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.ResetRecallStatusByID(orderID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<DisconnectedInvoicesModel>> GetDisconnectedInvoices()
        {
            WebResponseContentModel<List<DisconnectedInvoicesModel>> dt = new WebResponseContentModel<List<DisconnectedInvoicesModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<DisconnectedInvoicesModel>>>(WebServiceHelper.GetDisconnectedInvoices(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }


        #endregion

        #region Client

        public WebResponseContentModel<List<ClientSimpleModel>> GetAllClients()
        {
            WebResponseContentModel<List<ClientSimpleModel>> dt = new WebResponseContentModel<List<ClientSimpleModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<ClientSimpleModel>>>(WebServiceHelper.GetClientsFromDb(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<ClientSimpleModel>> GetAllClients(string typeCode)
        {
            WebResponseContentModel<List<ClientSimpleModel>> dt = new WebResponseContentModel<List<ClientSimpleModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<ClientSimpleModel>>>(WebServiceHelper.GetClientsFromDb(typeCode), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<ClientSimpleModel>> GetAllClients(int employeeID)
        {
            WebResponseContentModel<List<ClientSimpleModel>> dt = new WebResponseContentModel<List<ClientSimpleModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<ClientSimpleModel>>>(WebServiceHelper.GetClientsFromDb(employeeID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<ClientSimpleModel>> GetAllClients(int employeeID, string typeCode)
        {
            WebResponseContentModel<List<ClientSimpleModel>> dt = new WebResponseContentModel<List<ClientSimpleModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<ClientSimpleModel>>>(WebServiceHelper.GetClientsFromDb(employeeID, typeCode), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<ClientFullModel> GetClient(int clientID)
        {
            WebResponseContentModel<ClientFullModel> client = new WebResponseContentModel<ClientFullModel>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<ClientFullModel>>(WebServiceHelper.GetClientByID(clientID), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<ClientFullModel> GetClient(int clientID, int employeeID)
        {
            WebResponseContentModel<ClientFullModel> client = new WebResponseContentModel<ClientFullModel>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<ClientFullModel>>(WebServiceHelper.GetClientByID(clientID, employeeID), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<ClientFullModel> SaveClientChanges(ClientFullModel newData)
        {
            WebResponseContentModel<ClientFullModel> model = new WebResponseContentModel<ClientFullModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<ClientFullModel>>(WebServiceHelper.SaveClientDataChanges(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteClient(int clientID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteClient(clientID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<ContactPersonModel> SaveContactPersonChanges(ContactPersonModel newData)
        {
            WebResponseContentModel<ContactPersonModel> model = new WebResponseContentModel<ContactPersonModel>();
            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<ContactPersonModel>>(WebServiceHelper.SaveContactPersonChanges(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteContactPerson(int contactPersonID, int clientID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteContactPerson(contactPersonID, clientID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<ClientEmployeeModel> SaveClientEmployeeChanges(ClientEmployeeModel newData)
        {
            WebResponseContentModel<ClientEmployeeModel> model = new WebResponseContentModel<ClientEmployeeModel>();
            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<ClientEmployeeModel>>(WebServiceHelper.SaveClientEmployeeChanges(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteClientEmployee(int clientID, int employeeID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteClientEmployee(clientID, employeeID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> ClientEmployeeExist(int clientID, int employeeID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.ClientEmployeeExist(clientID, employeeID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<ClientType> GetClientTypeByCode(string typeCode)
        {
            WebResponseContentModel<ClientType> dt = new WebResponseContentModel<ClientType>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<ClientType>>(WebServiceHelper.GetClientTypeByCode(typeCode), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<ClientType> GetClientTypeById(int id)
        {
            WebResponseContentModel<ClientType> dt = new WebResponseContentModel<ClientType>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<ClientType>>(WebServiceHelper.GetClientTypeByID(id), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<ClientType>> GetClientTypes()
        {
            WebResponseContentModel<List<ClientType>> dt = new WebResponseContentModel<List<ClientType>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<ClientType>>>(WebServiceHelper.GetClientTypes(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<ClientTransportType>> GetAllTransportTypes()
        {
            WebResponseContentModel<List<ClientTransportType>> dt = new WebResponseContentModel<List<ClientTransportType>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<ClientTransportType>>>(WebServiceHelper.GetClientTransportTypes(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<ZbirnikTonModel>> GetAllZbirnikTon()
        {
            WebResponseContentModel<List<ZbirnikTonModel>> dt = new WebResponseContentModel<List<ZbirnikTonModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<ZbirnikTonModel>>>(WebServiceHelper.GetAllZbirnikTon(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<ClientTransportType> GetTransportTypeByID(int transportTypeID)
        {
            WebResponseContentModel<ClientTransportType> transportType = new WebResponseContentModel<ClientTransportType>();
            try
            {
                transportType = GetResponseFromWebRequest<WebResponseContentModel<ClientTransportType>>(WebServiceHelper.GetClientTransportTypeByID(transportTypeID), "get");
            }
            catch (Exception ex)
            {
                transportType.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return transportType;
        }

        public WebResponseContentModel<ClientTransportType> SaveTransportType(ClientTransportType newData)
        {
            WebResponseContentModel<ClientTransportType> model = new WebResponseContentModel<ClientTransportType>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<ClientTransportType>>(WebServiceHelper.SaveClientTransportType(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteTransportType(int transportTypeID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteClientTransportType(transportTypeID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<ClientFullModel> GetClientByName(string clientName)
        {
            WebResponseContentModel<ClientFullModel> client = new WebResponseContentModel<ClientFullModel>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<ClientFullModel>>(WebServiceHelper.GetClientByName(clientName), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<List<LanguageModelOTP>> GetLanguages()
        {
            WebResponseContentModel<List<LanguageModelOTP>> dt = new WebResponseContentModel<List<LanguageModelOTP>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<LanguageModelOTP>>>(WebServiceHelper.GetLanguages(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }
        #endregion

        #region Route

        public WebResponseContentModel<List<RouteModel>> GetAllRoutes()
        {
            WebResponseContentModel<List<RouteModel>> dt = new WebResponseContentModel<List<RouteModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<RouteModel>>>(WebServiceHelper.GetAllRoutes(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<hlpViewRoutePricesModel> GetAllRoutesTransportPricesByViewType(hlpViewRoutePricesModel vRPModel)
        {            
            WebResponseContentModel<hlpViewRoutePricesModel> model = new WebResponseContentModel<hlpViewRoutePricesModel>();
            try
            {                
                model.Content = vRPModel;
                model = PostWebRequestData<WebResponseContentModel<hlpViewRoutePricesModel>>(WebServiceHelper.GetAllRoutesTransportPricesByViewType(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }



        public WebResponseContentModel<RouteModel> GetRouteByID(int routeID)
        {
            WebResponseContentModel<RouteModel> dt = new WebResponseContentModel<RouteModel>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<RouteModel>>(WebServiceHelper.GetRouteByID(routeID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<RouteModel> SaveRoute(RouteModel newData)
        {
            WebResponseContentModel<RouteModel> model = new WebResponseContentModel<RouteModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<RouteModel>>(WebServiceHelper.SaveRoute(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteRoute(int routeID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteRoute(routeID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<RouteModel>> GetRoutesByCarrierID(int carrierID)
        {
            WebResponseContentModel<List<RouteModel>> model = new WebResponseContentModel<List<RouteModel>>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<List<RouteModel>>>(WebServiceHelper.GetRoutesByCarrierID(carrierID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<RouteModel>> GetRoutesByCarrierIDAndRouteID(int carrierID, int routeID)
        {
            WebResponseContentModel<List<RouteModel>> model = new WebResponseContentModel<List<RouteModel>>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<List<RouteModel>>>(WebServiceHelper.GetRoutesByCarrierIDAndRouteID(carrierID, routeID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        #endregion

        #region Tender

        public WebResponseContentModel<List<TenderFullModel>> GetTenderList(string dtFrom, string dtTo, string sFilterString)
        {
            WebResponseContentModel<List<TenderFullModel>> dt = new WebResponseContentModel<List<TenderFullModel>>();
            try
            {                
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<TenderFullModel>>>(WebServiceHelper.GetTenderList(dtFrom, dtTo, sFilterString), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<TenderPositionModel>> GetTenderListPositionByTenderID(int tenderID)
        {
            WebResponseContentModel<List<TenderPositionModel>> dt = new WebResponseContentModel<List<TenderPositionModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<TenderPositionModel>>>(WebServiceHelper.GetTenderListPositionByTenderID(tenderID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }



        public WebResponseContentModel<List<TenderPositionChangeModel>> GetTenderListPositionChanges()
        {
            WebResponseContentModel<List<TenderPositionChangeModel>> dt = new WebResponseContentModel<List<TenderPositionChangeModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<TenderPositionChangeModel>>>(WebServiceHelper.GetTenderListPositionChanges(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }





        public WebResponseContentModel<TenderFullModel> GetTenderByID(int tenderID)
        {
            WebResponseContentModel<TenderFullModel> dt = new WebResponseContentModel<TenderFullModel>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<TenderFullModel>>(WebServiceHelper.GetTenderByID(tenderID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<TenderModel> GetTenderSimpleModelByID(int tenderID)
        {
            WebResponseContentModel<TenderModel> dt = new WebResponseContentModel<TenderModel>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<TenderModel>>(WebServiceHelper.GetTenderSimpleModelByID(tenderID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<TenderFullModel> SaveTender(TenderFullModel newData)
        {
            WebResponseContentModel<TenderFullModel> model = new WebResponseContentModel<TenderFullModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<TenderFullModel>>(WebServiceHelper.SaveTender(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteTender(int tednerID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteTender(tednerID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<TenderPositionModel>> GetTenderListByRouteIDandZbirnikTon(int routeID, int ZbirnikTonID)
        {
            WebResponseContentModel<List<TenderPositionModel>> dt = new WebResponseContentModel<List<TenderPositionModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<TenderPositionModel>>>(WebServiceHelper.GetTenderListByRouteIDandZbirnikTon(routeID, ZbirnikTonID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<TenderPositionModel>> GetTenderListByRouteIDAndTenderDate(int routeID, string TenderDate)
        {
            WebResponseContentModel<List<TenderPositionModel>> dt = new WebResponseContentModel<List<TenderPositionModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<TenderPositionModel>>>(WebServiceHelper.GetTenderListByRouteIDAndTenderDate(routeID, TenderDate), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<TenderFullModel>> SaveTenders(List<TenderFullModel> newData)
        {
            WebResponseContentModel<List<TenderFullModel>> model = new WebResponseContentModel<List<TenderFullModel>>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<List<TenderFullModel>>>(WebServiceHelper.SaveTenders(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<TenderPositionModel> SaveTenderPosition(TenderPositionModel newData)
        {
            WebResponseContentModel<TenderPositionModel> model = new WebResponseContentModel<TenderPositionModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<TenderPositionModel>>(WebServiceHelper.SaveTenderPosition(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<TenderFullModel> SaveTenderAndTenderPosition(TenderFullModel newData)
        {
            WebResponseContentModel<TenderFullModel> model = new WebResponseContentModel<TenderFullModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<TenderFullModel>>(WebServiceHelper.SaveTenderAndTenderPosition(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<TenderPositionChangeModel>> SaveTenderPositionChanges(List<TenderPositionChangeModel> newData)
        {
            WebResponseContentModel<List<TenderPositionChangeModel>> model = new WebResponseContentModel<List<TenderPositionChangeModel>>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<List<TenderPositionChangeModel>>>(WebServiceHelper.SaveTenderPositionChanges(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteTenderPosition(int tednerPosID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteTenderPosition(tednerPosID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<int>> DeleteTenderPos(List<int> positionsToDelete)
        {
            WebResponseContentModel<List<int>> model = new WebResponseContentModel<List<int>>();

            try
            {
                model.Content = positionsToDelete;
                model = PostWebRequestData<WebResponseContentModel<List<int>>>(WebServiceHelper.DeleteTenderPos(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<TransportCountModel>> GetTransportCounByTransporterAndRoute(List<TransportCountModel> positionsToDelete)
        {
            WebResponseContentModel<List<TransportCountModel>> model = new WebResponseContentModel<List<TransportCountModel>>();

            try
            {
                model.Content = positionsToDelete;
                model = PostWebRequestData2<WebResponseContentModel<List<TransportCountModel>>>(WebServiceHelper.GetTransportCounByTransporterAndRoute(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public async Task<WebResponseContentModel<TransportCountModel>> GetTransportCounByTransporterIDAndRouteID(TransportCountModel positionsToDelete)
        {
            WebResponseContentModel<TransportCountModel> model = new WebResponseContentModel<TransportCountModel>();

            try
            {
                model.Content = positionsToDelete;
                model = await PostWebRequestDataAsync<WebResponseContentModel<TransportCountModel>>(WebServiceHelper.GetTransportCounByTransporterIDAndRouteID(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public async Task<WebResponseContentModel<hlpTenderTransporterSelection>> PrepareDataForTenderTransportAsync(hlpTenderTransporterSelection vTTModel)
        {
            WebResponseContentModel<hlpTenderTransporterSelection> model = new WebResponseContentModel<hlpTenderTransporterSelection>();

            try
            {
                model.Content = vTTModel;
                var outputStr = await MakeRequestAsync(WebServiceHelper.PrepareDataForTenderTransport());
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }


        public WebResponseContentModel<byte[]> GetTenderDownloadFile(int iIDTender)
        {
            WebResponseContentModel<byte[]> user = new WebResponseContentModel<byte[]>();
            try
            {
                user = GetResponseFromWebRequest<WebResponseContentModel<byte[]>>(WebServiceHelper.GetTenderDownloadFile(iIDTender), "get");
            }
            catch (Exception ex)
            {
                user.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }
            return user;
        }

        public WebResponseContentModel<decimal> GetLowestAndMostRecentPriceByRouteID(int routeID)
        {
            WebResponseContentModel<decimal> dt = new WebResponseContentModel<decimal>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<decimal>>(WebServiceHelper.GetLowestAndMostRecentPriceByRouteID(routeID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<decimal> GetLowestAndMostRecentPriceByRouteIDandZbirnikTonsID(int routeID, int ZbirnikTonID)
        {
            WebResponseContentModel<decimal> dt = new WebResponseContentModel<decimal>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<decimal>>(WebServiceHelper.GetLowestAndMostRecentPriceByRouteIDandZbirnikTonsID(routeID, ZbirnikTonID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<TenderPositionModel>> GetTenderListByRouteIDAndRecallID(int routeID, int recallID)
        {
            WebResponseContentModel<List<TenderPositionModel>> dt = new WebResponseContentModel<List<TenderPositionModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<TenderPositionModel>>>(WebServiceHelper.GetTenderListByRouteIDAndRecallID(routeID, recallID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<TonsModel>> GetAllTons()
        {
            WebResponseContentModel<List<TonsModel>> dt = new WebResponseContentModel<List<TonsModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<TonsModel>>>(WebServiceHelper.GetAllTons(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }


        #endregion

        #region Employee

        public WebResponseContentModel<List<EmployeeFullModel>> GetAllEmployees()
        {
            WebResponseContentModel<List<EmployeeFullModel>> dt = new WebResponseContentModel<List<EmployeeFullModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<EmployeeFullModel>>>(WebServiceHelper.GetAllEmployees(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<EmployeeFullModel>> GetAllEmployeesByRoleID(int roleID)
        {
            WebResponseContentModel<List<EmployeeFullModel>> dt = new WebResponseContentModel<List<EmployeeFullModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<EmployeeFullModel>>>(WebServiceHelper.GetAllEmployeesByRoleID(roleID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<EmployeeFullModel> GetAllEmployees(int employeeID)
        {
            WebResponseContentModel<EmployeeFullModel> dt = new WebResponseContentModel<EmployeeFullModel>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<EmployeeFullModel>>(WebServiceHelper.GetEmployeeByID(employeeID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<hlpTenderTransporterSelection> PrepareDataForTenderTransport(hlpTenderTransporterSelection vTTModel)
        {
            WebResponseContentModel<hlpTenderTransporterSelection> model = new WebResponseContentModel<hlpTenderTransporterSelection>();
            try
            {
                model.Content = vTTModel;
                model = PostWebRequestData<WebResponseContentModel<hlpTenderTransporterSelection>>(WebServiceHelper.PrepareDataForTenderTransport(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<hlpTenderCreateExcellData> SendTenderToTransportersEmails(hlpTenderCreateExcellData vTTModel)
        {
            WebResponseContentModel<hlpTenderCreateExcellData> model = new WebResponseContentModel<hlpTenderCreateExcellData>();
            try
            {
                model.Content = vTTModel;
                model = PostWebRequestData<WebResponseContentModel<hlpTenderCreateExcellData>>(WebServiceHelper.SendTenderToTransportersEmails(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }
        #endregion

        #region Dashboard

        public WebResponseContentModel<DashboardDataModel> GetDashboardData()
        {
            WebResponseContentModel<DashboardDataModel> dt = new WebResponseContentModel<DashboardDataModel>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<DashboardDataModel>>(WebServiceHelper.GetDashboardData(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        #endregion

        #region Settings

        public WebResponseContentModel<List<OTPEmailModel>> GetAllEmails()
        {
            WebResponseContentModel<List<OTPEmailModel>> dt = new WebResponseContentModel<List<OTPEmailModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<OTPEmailModel>>>(WebServiceHelper.GetAllEmailsOTP(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }
      

        public WebResponseContentModel<bool> CreateMailCopyOTP(int mailID)
        {
            WebResponseContentModel<bool> dt = new WebResponseContentModel<bool>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.CreateMailCopyOTP(mailID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

       

        public WebResponseContentModel<OTPEmailModel> GetMailByID(int mailID)
        {
            WebResponseContentModel<OTPEmailModel> dt = new WebResponseContentModel<OTPEmailModel>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<OTPEmailModel>>(WebServiceHelper.GetMailByIDOTP(mailID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }
        #endregion

        

        public T GetResponseFromWebRequest<T>(string uri, string requestMethod)
        {
            object obj = default(T);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = requestMethod.ToUpper();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string streamString = reader.ReadToEnd();

            obj = JsonConvert.DeserializeObject<T>(streamString);

            return (T)obj;
        }

        public T PostWebRequestData<T>(string uri, string requestMethod, T objectToSerialize)
        {
            object obj = default(T);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = requestMethod.ToUpper();
            request.ContentType = "application/json; charset=utf-8";
            request.Timeout = 99999999;             //Increase timeout for testing

            //using (var sw = new StreamWriter(request.GetRequestStream()))
            //{
            //    string clientData = JsonConvert.SerializeObject(objectToSerialize);
            //    sw.Write(clientData);                
            //    sw.Flush();
            //    sw.Close();
            //}
            //string path = @"D:\Example.txt";

            using (var sw = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.UTF8))
            {
                string clientData = JsonConvert.SerializeObject(objectToSerialize);
                sw.Write(clientData);
                sw.Flush();
                sw.Close();
            }


            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            HttpWebResponse httpResponse;
            try
            {
                httpResponse = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                //
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                httpResponse = (HttpWebResponse)ex.Response;
            }


            Stream stream = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string streamString = reader.ReadToEnd();

            obj = JsonConvert.DeserializeObject<T>(streamString);

            return (T)obj;
        }

        private string ConcatenateExceptionMessage(Exception ex)
        {
            return ex.Message + " \r\n" + ex.Source + (ex.InnerException != null ? ex.InnerException.Message + " \r\n" + ex.Source : "");
        }

        void DoWithResponse(HttpWebRequest request, Action<HttpWebResponse> responseAction)
        {
            Action wrapperAction = () =>
            {
                request.BeginGetResponse(new AsyncCallback((iar) =>
                {
                    var response = (HttpWebResponse)((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);
                    responseAction(response);
                }), request);
            };
            wrapperAction.BeginInvoke(new AsyncCallback((iar) =>
            {
                var action = (Action)iar.AsyncState;
                action.EndInvoke(iar);
            }), wrapperAction);
        }

        private async Task<String> MakeRequestAsync(String url)
        {
            String responseText = await Task.Run(() =>
            {
                try
                {
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    WebResponse response = request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    return new StreamReader(responseStream).ReadToEnd();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
                return null;
            });

            return responseText;
        }


        public async Task<T> PostWebRequestDataAsync<T>(string uri, string requestMethod, T objectToSerialize)
        {
            object obj = default(T);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = requestMethod.ToUpper();
            request.ContentType = "application/json; charset=utf-8";

            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                string clientData = JsonConvert.SerializeObject(objectToSerialize);
                sw.Write(clientData);
                sw.Flush();
                sw.Close();
            }

            var response = (HttpWebResponse)await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);
            //var response = Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);
            //response.Wait();

            // HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string streamString = reader.ReadToEnd();

            obj = JsonConvert.DeserializeObject<T>(streamString);

            return (T)obj;

        }

        public T PostWebRequestData2<T>(string uri, string requestMethod, T objectToSerialize)
        {
            object obj = default(T);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = requestMethod.ToUpper();
            request.ContentType = "application/json; charset=utf-8";

            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                string clientData = JsonConvert.SerializeObject(objectToSerialize);
                sw.Write(clientData);
                sw.Flush();
                sw.Close();
            }

            var response = (HttpWebResponse)WebRequestExtensions.GetResponseWithTimeout(request, 36000);

            // HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string streamString = reader.ReadToEnd();

            obj = JsonConvert.DeserializeObject<T>(streamString);

            return (T)obj;

        }
    }

    public static class WebRequestExtensions
    {
        public static Stream GetRequestStreamWithTimeout(
            this WebRequest request,
            int? millisecondsTimeout = null)
        {
            return AsyncToSyncWithTimeout(
                request.BeginGetRequestStream,
                request.EndGetRequestStream,
                millisecondsTimeout ?? request.Timeout);
        }

        public static WebResponse GetResponseWithTimeout(
            this HttpWebRequest request,
            int? millisecondsTimeout = null)
        {
            return AsyncToSyncWithTimeout(
                request.BeginGetResponse,
                request.EndGetResponse,
                millisecondsTimeout ?? request.Timeout);
        }

        private static T AsyncToSyncWithTimeout<T>(
            Func<AsyncCallback, object, IAsyncResult> begin,
            Func<IAsyncResult, T> end,
            int millisecondsTimeout)
        {
            var iar = begin(null, null);
            if (!iar.AsyncWaitHandle.WaitOne(millisecondsTimeout))
            {
                var ex = new TimeoutException();
                throw new WebException(ex.Message, ex, WebExceptionStatus.Timeout, null);
            }
            return end(iar);
        }
    }
}