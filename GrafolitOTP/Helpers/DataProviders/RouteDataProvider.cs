using DatabaseWebService.ModelsOTP.Route;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptimizacijaTransprotov.Helpers.DataProviders
{
    public class RouteDataProvider : ServerMasterPage
    {
        public void SetRouteModel(RouteModel model)
        {
            AddValueToSession(Enums.RouteSession.RouteModel, model);
        }

        public RouteModel GetRouteModel()
        {
            if (SessionHasValue(Enums.RouteSession.RouteModel))
                return (RouteModel)GetValueFromSession(Enums.RouteSession.RouteModel);

            return null;
        }

        public void SetRoutesByCarrierID(List<RouteModel> model)
        {
            AddValueToSession(Enums.RouteSession.RouteList, model);
        }

        public List<RouteModel> GetRoutesByCarrierID()
        {
            if (SessionHasValue(Enums.RouteSession.RouteList))
                return (List<RouteModel>)GetValueFromSession(Enums.RouteSession.RouteList);

            return null;
        }

        public void SetRoutesByCarrierIDAndRouteID(List<RouteModel> model)
        {
            AddValueToSession(Enums.RouteSession.RouteDetailList, model);
        }

        public List<RouteModel> GetRoutesByCarrierIDAndRouteID()
        {
            if (SessionHasValue(Enums.RouteSession.RouteList))
                return (List<RouteModel>)GetValueFromSession(Enums.RouteSession.RouteDetailList);

            return null;
        }
    }
}