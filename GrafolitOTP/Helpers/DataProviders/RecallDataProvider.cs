using DatabaseWebService.ModelsOTP;
using DatabaseWebService.ModelsOTP.Client;
using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.ModelsOTP.Recall;
using DatabaseWebService.ModelsOTP.Tender;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Helpers.Models;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptimizacijaTransprotov.Helpers.DataProviders
{
    public class RecallDataProvider : ServerMasterPage
    {
        //Recall full model
        public void SetRecallFullModel(RecallFullModel model)
        {
            AddValueToSession(Enums.RecallSession.RecallFulModel, model);
        }

        public RecallFullModel GetRecallFullModel()
        {
            if (SessionHasValue(Enums.RecallSession.RecallFulModel))
                return (RecallFullModel)GetValueFromSession(Enums.RecallSession.RecallFulModel);

            return null;
        }

        //Recall Buyer full model
        public void SetRecallBuyerFullModel(RecallBuyerFullModel model)
        {
            AddValueToSession(Enums.RecallSession.RecallBuyerFulModel, model);
        }

        public RecallBuyerFullModel GetRecallBuyerFullModel()
        {
            if (SessionHasValue(Enums.RecallSession.RecallBuyerFulModel))
                return (RecallBuyerFullModel)GetValueFromSession(Enums.RecallSession.RecallBuyerFulModel);

            return null;
        }

        //Recall Buyer list disconnected invoices
        public void SetDisconnectedInvoicesList(List<DisconnectedInvoicesModel> modelList)
        {
            AddValueToSession(Enums.RecallSession.DisconnectedInvoicesList, modelList);
        }

        public List<DisconnectedInvoicesModel> GetDisconnectedInvoicesList()
        {
            if (SessionHasValue(Enums.RecallSession.DisconnectedInvoicesList))
                return (List<DisconnectedInvoicesModel>)GetValueFromSession(Enums.RecallSession.DisconnectedInvoicesList);

            return null;
        }

        //Recall positions
        public void SetRecallPositions(List<RecallPositionModel> positions)
        {
            AddValueToSession(Enums.RecallSession.RecallPositions, positions);
        }

        public List<RecallPositionModel> GetRecallPositions()
        {
            if (SessionHasValue(Enums.RecallSession.RecallPositions))
                return (List<RecallPositionModel>)GetValueFromSession(Enums.RecallSession.RecallPositions);

            return null;
        }

        //Selected recall positions from order
        public void SetSelectedRecallPositions(List<RecallPositionModel> positions)
        {
            AddValueToSession(Enums.RecallSession.SelectedPositionsRecall, positions);
        }

        public List<RecallPositionModel> GetSelectedRecallPositions()
        {
            if (SessionHasValue(Enums.RecallSession.SelectedPositionsRecall))
                return (List<RecallPositionModel>)GetValueFromSession(Enums.RecallSession.SelectedPositionsRecall);

            return null;
        }

        //Tender list
        public void SetTenderListFromSelectedRoute(List<TenderPositionModel> list)
        {
            AddValueToSession(Enums.RecallSession.TenderListFromRoute, list);
        }

        public List<TenderPositionModel> GetTenderListFromSelectedRoute()
        {
            if (SessionHasValue(Enums.RecallSession.TenderListFromRoute))
                return (List<TenderPositionModel>)GetValueFromSession(Enums.RecallSession.TenderListFromRoute);
            return null;
        }

        public bool HasTenderListValues()
        {
            if (SessionHasValue(Enums.RecallSession.TenderListFromRoute))
                return true;
            return false;
        }

        //Recall Status
        public void SetRecallStatuses(List<RecallStatus> list)
        {
            AddValueToSession(Enums.RecallSession.RecallStatuses, list);
        }

        public List<RecallStatus> GetRecallStatuses()
        {
            if (SessionHasValue(Enums.RecallSession.RecallStatuses))
                return (List<RecallStatus>)GetValueFromSession(Enums.RecallSession.RecallStatuses);
            return null;
        }

        public void SetRecallStatus(DatabaseWebService.Common.Enums.Enums.StatusOfRecall status)
        {
            AddValueToSession(Enums.RecallSession.RecallStatus, status);
        }

        public DatabaseWebService.Common.Enums.Enums.StatusOfRecall GetRecallStatus()
        {
            if (SessionHasValue(Enums.RecallSession.RecallStatus))
                return (DatabaseWebService.Common.Enums.Enums.StatusOfRecall)GetValueFromSession(Enums.RecallSession.RecallStatus);

            return DatabaseWebService.Common.Enums.Enums.StatusOfRecall.NEZNAN;
        }

        //Order position number10
        public void SetOrder10Positions(List<OrderPositionModelNew> list)
        {
            AddValueToSession(Enums.RecallSession.Order10Position, list);
        }

        public List<OrderPositionModelNew> GetOrder10Positions()
        {
            if (SessionHasValue(Enums.RecallSession.Order10Position))
                return (List<OrderPositionModelNew>)GetValueFromSession(Enums.RecallSession.Order10Position);
            return null;
        }

        //Recall type
        public void SetRecallTypes(List<RecallType> list)
        {
            AddValueToSession(Enums.RecallSession.RecallTypes, list);
        }

        public List<RecallType> GetRecallTypes()
        {
            if (SessionHasValue(Enums.RecallSession.RecallTypes))
                return (List<RecallType>)GetValueFromSession(Enums.RecallSession.RecallTypes);
            return null;
        }

        //Recall Print
        public void SetRecallFullModelForPrint(RecallFullModel model)
        {
            AddValueToSession(Enums.CommonSession.PrintModel, model);
        }

        public RecallFullModel GetRecallFullModelForPrint()
        {
            if (SessionHasValue(Enums.CommonSession.PrintModel))
                return (RecallFullModel)GetValueFromSession(Enums.CommonSession.PrintModel);

            return null;
        }

        public void SetSuppliersList(List<SupplierModel> model)
        {
            AddValueToSession(Enums.RecallSession.Suppliers, model);
        }

        public List<SupplierModel> GetSuppliersList()
        {
            if (SessionHasValue(Enums.RecallSession.Suppliers))
                return (List<SupplierModel>)GetValueFromSession(Enums.RecallSession.Suppliers);

            return null;
        }

        public void SetTransportTypes(List<ClientTransportType> model)
        {
            AddValueToSession(Enums.RecallSession.TransportTypes, model);
        }

        public List<ClientTransportType> GetTransportTypes()
        {
            if (SessionHasValue(Enums.RecallSession.TransportTypes))
                return (List<ClientTransportType>)GetValueFromSession(Enums.RecallSession.TransportTypes);

            return null;
        }

        public void SetZbirnikTon(List<ZbirnikTonModel> model)
        {
            AddValueToSession(Enums.RecallSession.ZbirnikTon, model);
        }

        public List<ZbirnikTonModel> GetZbirnikTon()
        {
            if (SessionHasValue(Enums.RecallSession.ZbirnikTon))
                return (List<ZbirnikTonModel>)GetValueFromSession(Enums.RecallSession.ZbirnikTon);

            return null;
        }

        public void SetRecallPosIDOptimalStockOverflow(List<object> model)
        {
            AddValueToSession(Enums.RecallSession.RecallPosOptimalStockOverflow, model);
        }

        public List<object> GetRecallPosIDOptimalStockOverflow()
        {
            if (SessionHasValue(Enums.RecallSession.RecallPosOptimalStockOverflow))
                return (List<object>)GetValueFromSession(Enums.RecallSession.RecallPosOptimalStockOverflow);

            return new List<object>();
        }

        //Create Order
        public void SetServices(List<ServiceListModel> list)
        {
            AddValueToSession(Enums.OrderFromRecallSession.ServiceList, list);
        }

        public List<ServiceListModel> GetServices()
        {
            if (SessionHasValue(Enums.OrderFromRecallSession.ServiceList))
                return (List<ServiceListModel>)GetValueFromSession(Enums.OrderFromRecallSession.ServiceList);
            return null;
        }
    }
}