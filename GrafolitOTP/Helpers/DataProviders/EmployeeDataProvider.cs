using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.Models;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptimizacijaTransprotov.Helpers.DataProviders
{
    public class EmployeeDataProvider : ServerMasterPage
    {
        public void SetEmployeeFullModel(EmployeeFullModel model)
        {
           AddValueToSession(Enums.EmployeeSession.EmployeeModel, model);
        }

        public EmployeeFullModel GetEmployeeFullModel()
        {
            if (SessionHasValue(Enums.EmployeeSession.EmployeeModel))
                return (EmployeeFullModel)GetValueFromSession(Enums.EmployeeSession.EmployeeModel);

            return null;
        }
    }
}