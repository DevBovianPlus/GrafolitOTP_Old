using DatabaseWebService.ModelsOTP.Order;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptimizacijaTransprotov.Helpers.DataProviders
{
    public class OrderDataProvider : ServerMasterPage
    {
        public void SetOrderPositions(List<OrderPositionModelNew> positions)
        {
            AddValueToSession(Enums.OrderSession.OrdersPositionsList, positions);
        }

        public List<OrderPositionModelNew> GetOrderPositions()
        {
            if (SessionHasValue(Enums.OrderSession.OrdersPositionsList))
                return (List<OrderPositionModelNew>)GetValueFromSession(Enums.OrderSession.OrdersPositionsList);

            return null;
        }

        public void SetSelectedOrderPositions(List<OrderPositionModelNew> positions)
        {
            AddValueToSession(Enums.OrderSession.SelectedOrderPositions, positions);
        }

        public List<OrderPositionModelNew> GetSelectedOrderPositions()
        {
            if (SessionHasValue(Enums.OrderSession.SelectedOrderPositions))
                return (List<OrderPositionModelNew>)GetValueFromSession(Enums.OrderSession.SelectedOrderPositions);

            return null;
        }
    }
}