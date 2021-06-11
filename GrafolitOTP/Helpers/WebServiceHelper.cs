using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace OptimizacijaTransprotov.Helpers
{
    public static class WebServiceHelper
    {
        private static string BaseWebServiceURI
        {
            get
            {
                return WebConfigurationManager.AppSettings["BaseWebService"].ToString();
            }
        }

        private static string WebServiceSignInURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["ValuesController"].ToString();
            }
        }

        private static string WebServiceOrderURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["OrderController"].ToString();
            }
        }

        private static string WebServiceRecallURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["RecallController"].ToString();
            }
        }

        private static string WebServiceClientURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["ClientController"].ToString();
            }
        }

        private static string WebServiceRouteURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["RouteController"].ToString();
            }
        }

        private static string WebServiceTenderURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["TenderController"].ToString();
            }
        }

        private static string WebServiceEmployeeURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["EmployeeController"].ToString();
            }
        }

        private static string WebServiceDashboardURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["DashboardController"].ToString();
            }
        }

        private static string WebServiceSettingsURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["SettingsController"].ToString();
            }
        }

        public static string SignIn(string username, string pass)
        {
            return WebServiceSignInURL + "SignInOTP?username=" + username + "&password=" + pass;
        }

        public static string GetWebServiceLogFile()
        {
            return WebServiceSignInURL + "GetWebServiceLogFile";
        }

        public static string GetUtilityServiceLogFile()
        {
            return WebServiceSignInURL + "GetUtilityServiceLogFile";
        }


       

        #region Order
        public static string GetAllOrders()
        {
            return WebServiceOrderURL + "GetAllOrders";
        }
        public static string GetAllOrdersPoistions()
        {
            return WebServiceOrderURL + "GetAllOrdersPositions";
        }

        public static string GetAllSuppliers()
        {
            return WebServiceOrderURL + "GetAllSuppliers";
        }
        public static string GetAllOrdersPositionsBySupplier(string supplier, int clientID = 0)
        {
            return WebServiceOrderURL + "GetListOfOpenedOrderPositionsBySupplier?supplier=" + supplier + "&clientID=" + clientID;
        }

        public static string GetListOfOrderNumber10()
        {
            return WebServiceOrderURL + "GetListOfOrderNumber10";
        }

        public static string GetCategoryList()
        {
            return WebServiceOrderURL + "GetCategoryList";
        }

        #endregion

        #region Recall

        public static string GetAllRecalls()
        {
            return WebServiceRecallURL + "GetAllRecalls";
        }
        public static string GetRecallByID(int recallID)
        {
            return WebServiceRecallURL + "GetRecallByID?recallID=" + recallID;
        }

        public static string GetRecallBuyerByID(int recallID)
        {
            return WebServiceRecallURL + "GetRecallBuyerByID?recallID=" + recallID;
        }


        public static string SaveRecall()
        {
            return WebServiceRecallURL + "SaveRecall";
        }
        public static string SaveBuyerRecall()
        {
            return WebServiceRecallURL + "SaveBuyerRecall";
        }

        public static string DeleteRecall(int id)
        {
            return WebServiceRecallURL + "DeleteRecall?recallID=" + id;
        }

        public static string DeleteBuyerRecall(int id)
        {
            return WebServiceRecallURL + "DeleteBuyerRecall?recallBuyerID=" + id;
        }

        public static string GetRecallPositionByID(int recallPosID)
        {
            return WebServiceRecallURL + "GetRecallPositionByID?recallPosID=" + recallPosID;
        }

        public static string SaveRecallPosition()
        {
            return WebServiceRecallURL + "SaveRecallPosition";
        }

        public static string SaveRecallPositions()
        {
            return WebServiceRecallURL + "SaveRecallPositions";
        }

        public static string DeleteRecallPosition(int id)
        {
            return WebServiceRecallURL + "DeleteRecallPosition?recallPosID=" + id;
        }

        public static string GetRecallTypeByID(int typeID)
        {
            return WebServiceRecallURL + "GetRecallTypeByID?typeID=" + typeID;
        }
        public static string GetRecallTypeByCode(string typeCode)
        {
            return WebServiceRecallURL + "GetRecallTypeByCode?typeCode=" + typeCode;
        }
        public static string GetRecallTypes()
        {
            return WebServiceRecallURL + "GetRecallTypes";
        }

        public static string GetRecallStatusByID(int statusID)
        {
            return WebServiceRecallURL + "GetRecallStatusByID?statusID=" + statusID;
        }
        public static string GetRecallStatusByCode(string statusCode)
        {
            return WebServiceRecallURL + "GetRecallStatusByCode?statusCode=" + statusCode;
        }
        public static string GetRecallStatuses()
        {
            return WebServiceRecallURL + "GetRecallStatuses";
        }
        public static string GetLatestKolicinaOTPForProduct()
        {
            return WebServiceRecallURL + "GetLatestKolicinaOTPForProduct";
        }

        public static string GetRecallPosFromPartialOverTakeRecalls()
        {
            return WebServiceRecallURL + "GetRecallPosFromPartialOverTakeRecalls";
        }

        public static string ResetSequentialNumInRecallPos()
        {
            return WebServiceRecallURL + "ResetSequentialNumInRecallPos";
        }

        public static string TakeOverConfirmedRecalls()
        {
            return WebServiceRecallURL + "TakeOverConfirmedRecalls";
        }

        public static string GetAllTakeOverRecalls()
        {
            return WebServiceRecallURL + "GetAllTakeOverRecalls";
        }

        public static string GetAllNoneTakeOverRecalls()
        {
            return WebServiceRecallURL + "GetAllNoneTakeOverRecalls";
        }

        public static string GetAllBuyersRecalls()
        {
            return WebServiceRecallURL + "GetAllBuyersRecalls";
        }

        public static string IsSubmittingPriceForCarrierStillValid(int prijavaPrevoznikaID)
        {
            return WebServiceRecallURL + "IsSubmittingPriceForCarrierStillValid?prijavaPevoznikaID=" + prijavaPrevoznikaID;
        }

        public static string SubmitPriceForCarrierTransport(int prijavaPevoznikaID, decimal newPrice)
        {
            return WebServiceRecallURL + "SubmitPriceForCarrierTransport?prijavaPevoznikaID=" + prijavaPevoznikaID + "&newPrice=" + newPrice.ToString().Replace(',','.');
        }

        public static string CheckRecallsForCarriersSubmittingPrices()
        {
            return WebServiceRecallURL + "CheckRecallsForCarriersSubmittingPrices";
        }

        public static string CheckForRecallsWithNoSubmitedPrices()
        {
            return WebServiceRecallURL + "CheckForRecallsWithNoSubmitedPrices";
        }

        public static string GetCarriersInquiry(int odpoklicID)
        {
            return WebServiceRecallURL + "GetCarriersInquiry?recallID=" + odpoklicID;
        }

        public static string SaveNewAddedCarrierForInquiry()
        {
            return WebServiceRecallURL + "SavePrijavaPrevoznika";
        }

        public static string ReSendEmailToCarriers()
        {
            return WebServiceRecallURL + "ReSendEmailToCarriers";
        }

        public static string ManualSelectCarrierForTransport(int prijavaPrevoznikaID)
        {
            return WebServiceRecallURL + "ManualSelectCarrierForTransport?prijavaPrevoznikaID=" + prijavaPrevoznikaID;
        }

        public static string DeleteCarrierInquiry(int prijavaPrevoznikaID)
        {
            return WebServiceRecallURL + "DeleteCarrierInquiry?prijavaPrevoznikaID=" + prijavaPrevoznikaID;
        }

        public static string CreateOrderTransport()
        {
            return WebServiceRecallURL + "CreateOrderTransport"; 
        }

        public static string LaunchPantheonCreatePDF()
        {
            return WebServiceRecallURL + "LaunchPantheonCreatePDF";
        }

        public static string GetOrderPDFFile(int iRecallID)
        {
            return WebServiceRecallURL + "GetOrderPDFFile?iRecallID=" + iRecallID;
        }

        public static string CheckForOrderTakeOver2()
        {
            return WebServiceRecallURL + "CheckForOrderTakeOver2";
        }

        public static string CreateAndSendOrdersMultiple()
        {
            return WebServiceRecallURL + "CreateAndSendOrdersMultiple";
        }

        public static string CreatePDFAndSendPDOOrdersMultiple()
        {
            return WebServiceRecallURL + "CreatePDFAndSendPDOOrdersMultiple";
        }

        public static string ChangeConfigValue(string sConfigName, string sConfigValue)
        {
            return WebServiceRecallURL + "ChangeConfigValue?sConfigName=" + sConfigName + "&sConfigValue=" + sConfigValue;
        }

        public static string GetConfigValue(string sConfigName)
        {
            return WebServiceRecallURL + "GetConfigValue?sConfigName=" + sConfigName;
        }

        public static string ResetRecallStatusByID(int RecallID)
        {
            return WebServiceRecallURL + "ResetRecallStatusByID?RecallID=" + RecallID;
        }

        public static string GetDisconnectedInvoices()
        {
            return WebServiceRecallURL + "GetDisconnectedInvoices";
        }

        #endregion

        #region Client

        public static string GetClientsFromDb()
        {
            return WebServiceClientURL + "GetAllClients";
        }
        public static string GetClientsFromDb(int employeeID)
        {
            return WebServiceClientURL + "GetAllClients?employeeID=" + employeeID.ToString();
        }

        public static string GetClientsFromDb(string typeCode)
        {
            return WebServiceClientURL + "GetAllClients?employeeID=0&typeCode=" + typeCode;
        }

        public static string GetClientsFromDb(int employeeID, string typeCode)
        {
            return WebServiceClientURL + "GetAllClients?employeeID=" + employeeID.ToString() + "&typeCode=" + typeCode;
        }

        public static string GetClientByID(int id)
        {
            return WebServiceClientURL + "GetClientByID?clientID=" + id.ToString();
        }

        public static string GetClientByID(int id, int employeeID)
        {
            return WebServiceClientURL + "GetClientByID?clientID=" + id.ToString() + "&employeeID=" + employeeID.ToString();
        }

        public static string SaveClientDataChanges()
        {
            return WebServiceClientURL + "SaveClientData";
        }

        public static string DeleteClient(int id)
        {
            return WebServiceClientURL + "DeleteClient?clientID=" + id;
        }


        public static string SaveContactPersonChanges()
        {
            return WebServiceClientURL + "SaveContactPersonToClient";
        }

        public static string DeleteContactPerson(int contactPersonID, int clientID)
        {
            return WebServiceClientURL + "DeleteContactPerson?contactPersonID=" + contactPersonID + "&clientID=" + clientID;
        }

        public static string SaveClientEmployeeChanges()
        {
            return WebServiceClientURL + "SaveClientEmployee";
        }

        public static string DeleteClientEmployee(int clientID, int employeeID)
        {
            return WebServiceClientURL + "DeleteClientEmployee?clientID=" + clientID + "&employeeID=" + employeeID;
        }

        public static string ClientEmployeeExist(int clientID, int employeeID)
        {
            return WebServiceClientURL + "ClientEmployeeExist?clientID=" + clientID + "&employeeID=" + employeeID;
        }

        public static string GetClientTypeByID(int id)
        {
            return WebServiceClientURL + "GetClientTypeByCode?id=" + id;
        }

        public static string GetClientTypeByCode(string typeCode)
        {
            return WebServiceClientURL + "GetAllClients?typeCode=" + typeCode;
        }

        public static string GetClientTypes()
        {
            return WebServiceClientURL + "GetClientTypes";
        }

        public static string GetClientTransportTypes()
        {
            return WebServiceClientURL + "GetAllTransportTypes";
        }


        public static string GetAllZbirnikTon()
        {
            return WebServiceClientURL + "GetAllZbirnikTon";
        }
        public static string GetClientTransportTypeByID(int id)
        {
            return WebServiceClientURL + "GetTransportTypeByID?transportTypeID=" + id;
        }
        public static string SaveClientTransportType()
        {
            return WebServiceClientURL + "SaveTransportTypeData";
        }
        public static string DeleteClientTransportType(int transportTypeID)
        {
            return WebServiceClientURL + "DeleteTransportType?transportTypeID=" + transportTypeID;
        }

        public static string GetClientByName(string clientName)
        {
            return WebServiceClientURL + "GetClientByName?clientName=" + clientName;
        }

        public static string GetLanguages()
        {
            return WebServiceClientURL + "GetLanguages";
        }
        #endregion

        #region Route

        public static string GetAllRoutes()
        {
            return WebServiceRouteURL + "GetAllRoutes";
        }
        public static string GetAllRoutesTransportPricesByViewType()
        {
            return WebServiceRouteURL + "GetAllRoutesTransportPricesByViewType";
        }
        public static string GetRouteByID(int routeID)
        {
            return WebServiceRouteURL + "GetRouteByID?routeID=" + routeID;
        }

        public static string SaveRoute()
        {
            return WebServiceRouteURL + "SaveRoute";
        }

        public static string DeleteRoute(int id)
        {
            return WebServiceRouteURL + "DeleteRoute?routeID=" + id;
        }

        public static string GetRoutesByCarrierID(int carrierID)
        {
            return WebServiceRouteURL + "GetRoutesByCarrierID?carrierID=" + carrierID;
        }

        public static string GetRoutesByCarrierIDAndRouteID(int carrierID, int routeID)
        {
            return WebServiceRouteURL + "GetRoutesByCarrierIDAndRouteID?carrierID=" + carrierID + "&routeID=" + routeID;
        }

        #endregion


        #region Tender

        public static string GetTenderList(string dtFrom, string dtTo,string sFilterString)
        {
            return WebServiceTenderURL + "GetTenderList?dtFrom=" + dtFrom + "&dtTo=" + dtTo + "&sFilterString=" + sFilterString; 
        }

        public static string GetTenderListPositionByTenderID(int tenderID)
        {
            return WebServiceTenderURL + "GetTenderListPositionByTenderID?tenderID=" + tenderID;
        }

        public static string GetTenderListPositionChanges()
        {
            return WebServiceTenderURL + "GetTenderListPositionChanges";
        }
        public static string GetTenderByID(int tenderID)
        {
            return WebServiceTenderURL + "GetTenderByID?tenderID=" + tenderID;
        }

        public static string GetTenderSimpleModelByID(int tenderID)
        {
            return WebServiceTenderURL + "GetTenderSimpleModelByID?tenderID=" + tenderID;
        }

        public static string SaveTender()
        {
            return WebServiceTenderURL + "SaveTender";
        }

        public static string DeleteTender(int id)
        {
            return WebServiceTenderURL + "DeleteTender?tenderID=" + id;
        }

        public static string GetTenderListByRouteIDandZbirnikTon(int routeID, int ZbirnikTonID)
        {
            return WebServiceTenderURL + "GetTenderListByRouteIDandZbirnikTon?routeID=" + routeID+"&ZbirnikTonID=" + ZbirnikTonID;
        }

        public static string GetTenderListByRouteIDAndTenderDate(int routeID, string TenderDate)
        {
            return WebServiceTenderURL + "GetTenderListByRouteIDAndTenderDate?routeID=" + routeID+ "&TenderDate=" + TenderDate;
        }

        public static string SaveTenders()
        {
            return WebServiceTenderURL + "SaveTenders";
        }

        public static string SaveTenderPosition()
        {
            return WebServiceTenderURL + "SaveTenderPosition";
        }

        public static string SaveTenderAndTenderPosition()
        {
            return WebServiceTenderURL + "SaveTenderAndUploadPosition";
        }

        public static string SaveTenderPositionChanges()
        {
            return WebServiceTenderURL + "SaveTenderPositionChanges";
        }

        public static string DeleteTenderPosition(int tenderPositionID)
        {
            return WebServiceTenderURL + "DeleteTenderPosition?tenderPositionID=" + tenderPositionID.ToString();
        }

        public static string DeleteTenderPos()
        {
            return WebServiceTenderURL + "DeleteTenderPositions";
        }

        public static string GetTransportCounByTransporterAndRoute()
        {
            return WebServiceTenderURL + "GetTransportCounByTransporterAndRoute";
        }

        public static string GetLowestAndMostRecentPriceByRouteID(int routeID)
        {
            return WebServiceTenderURL + "GetLowestAndMostRecentPriceByRouteID?routeID=" + routeID.ToString();
        }

        public static string GetLowestAndMostRecentPriceByRouteIDandZbirnikTonsID(int routeID, int ZbirnikTonID)
        {
            return WebServiceTenderURL + "GetLowestAndMostRecentPriceByRouteIDandZbirnikTonsID?routeID=" + routeID.ToString() + "&ZbirnikTonID=" + ZbirnikTonID.ToString();
        }

        public static string GetTransportCounByTransporterIDAndRouteID()
        {
            return WebServiceTenderURL + "GetTransportCounByTransporterIDAndRouteID";
        }

        public static string GetTenderListByRouteIDAndRecallID(int routeID, int recallID)
        {
            return WebServiceTenderURL + "GetTenderListByRouteIDAndRecallID?routeID=" + routeID + "&recallID=" + recallID;
        }

        public static string GetTenderDownloadFile(int iIDTender)
        {
            return WebServiceTenderURL + "GetTenderDownloadFile?TenderID=" + iIDTender;
        }

        public static string GetAllTons()
        {
            return WebServiceTenderURL + "GetAllTons";
        }

        public static string PrepareDataForTenderTransport()
        {
            return WebServiceTenderURL + "PrepareDataForTenderTransport";
        }

        public static string SendTenderToTransportersEmails()
        {
            return WebServiceTenderURL + "SendTenderToTransportersEmails";
        }
        #endregion

        #region Employee

        public static string GetAllEmployees()
        {
            return WebServiceEmployeeURL + "GetAllEmployees";
        }

        public static string GetAllEmployeesByRoleID(int roleID)
        {
            return WebServiceEmployeeURL + "GetAllEmployeesByRoleID?roleID=" + roleID;
        }

        public static string GetEmployeeByID(int employeeId)
        {
            return WebServiceEmployeeURL + "GetEmployeeByID?employeeId=" + employeeId;
        }

        public static string GetPantheonUsers()
        {
            return WebServiceEmployeeURL + "GetPantheonUsers";
        }

        public static string SaveEmployeeOTP()
        {
            return WebServiceEmployeeURL + "SaveEmployeeOTP";
        }

        public static string DeleteEmployee(int employeeID)
        {
            return WebServiceEmployeeURL + "DeleteEmployee?employeeID=" + employeeID;
        }

        #endregion

        #region "Vloga"

        public static string GetRolesOTP()
        {
            return WebServiceEmployeeURL + "GetRolesOTP";
        }
        #endregion

        #region Dashboard

        public static string GetDashboardData()
        {
            return WebServiceDashboardURL + "GetDashboardData";
        }

        #endregion

        #region Settings
       
        public static string GetAllEmailsOTP()
        {
            return WebServiceSettingsURL + "GetAllEmailsOTP";
        }
        
        public static string CreateMailCopyOTP(int mailID)
        {
            return WebServiceSettingsURL + "CreateMailCopyOTP?mailID=" + mailID;
        }
        
        public static string GetMailByIDOTP(int mailID)
        {
            return WebServiceSettingsURL + "GetMailByIDOTP?mailID=" + mailID;
        }
        #endregion
    }
}