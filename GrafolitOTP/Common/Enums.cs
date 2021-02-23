using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptimizacijaTransprotov.Common
{
    public class Enums
    {
        public enum UserAction : int
        {
            Add = 1,
            Edit = 2,
            Delete = 3
        }

        public enum UserActionStr 
        {
            Add ,
            Edit ,
            Update,
            Delete
        }

        public enum UserRole
        {
            SuperAdmin,
            Admin,
            Leader,
            Warehouse,
            User,
            Logistics
        }

        public enum CommonSession
        {
            ShowWarning,
            ShowWarningMessage,
            UserActionPopUp,
            UserActionNestedPopUp,
            activeTab,
            PrintModel,
            PreviousPageName,
            PreviousPageSessions,
            DecodedQueryString,
            StayOnFormAndOpenPopup
        }

        public enum QueryStringName
        {
            action,
            recordId,
            printReport,
            printId,
            showPreviewReport,
            filter,
            id,
            lang,
            Refresh,
            showValue
        }

        public enum OrderSession
        {
            OrdersPositionsList,
            SelectedOrderPositions,
            SupplierID,
            CientID
        }
        public enum RecallSession
        {
            Suppliers,
            RecallPositions,
            SelectedPositionsRecall,
            RecallFulModel,
            RecallBuyerFulModel,
            TenderListFromRoute,
            RecallStatuses,
            RecallStatus,
            RecallTypes,
            ArgumentsOfApproval,
            ArgumentsOfApprovalToDB,
            TransportTypes,
            ZbirnikTon,
            RecallPosOptimalStockOverflow,
            InquirySummaryRecallID,
            InquirySummaries,
            Order10Position,
            DisconnectedInvoicesList,
            SelectedBuyerRecallID

        }

        public enum TenderSession
        {
            TenderFullModel,
            TenderID,
            SelectedTenderID,
            DownloadTenderData,
            SelectedTenderPositionRows
        }

        public enum ClientSession
        {
            ClientFullModel,
            ClientTransportTypeID,
            ClientTransportTypeModel,
            ContactPersonModel,
            ContactPersonID,
            ClientID
        }

        public enum RouteSession
        {
            RouteModel,
            RouteID,
            RouteList,
            RouteDetailList,
            CarrierIDPopup,
            RouteIDPopup,
            RouteNamePopup,
            CarrierNamePopup,
            CarrierListModel,
            RouteTransportPriceModel

        }

        public enum PreviousPage
        {
            Orders,
            Recalls,
            Tender,
            SendTender
        }

        public enum Cookies
        {
            UserLastRequest,
            SessionExpires,
            OTPTenderLanguage
        }

        public enum CookieCommonValue
        {
            STOP
        }

        public enum CustomDisplayText
        {
            DA,
            NE
        }

        public enum UnitsFromOrder
        {
            KOS,
            KG
        }

        public enum TicketSession
        {
            TicketModel
        }

        public enum OrderFromRecallSession
        {
            ServiceList
        }

        public enum SystemEmailMessageSession
        {
            SystemMessageID,
            SystemMessageModel
        }
    }
}
