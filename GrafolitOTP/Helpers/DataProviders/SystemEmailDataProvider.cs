using DatabaseWebService.Models.Client;
using DatabaseWebService.ModelsOTP;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptimizacijaTransprotov.Helpers.DataProviders
{
    public class SystemEmailDataProvider : ServerMasterPage
    {
        public void SetSystemEmailMessageModel(OTPEmailModel model)
        {
            AddValueToSession(Enums.SystemEmailMessageSession.SystemMessageModel, model);
        }

        public OTPEmailModel GetSystemEmailMessageModel()
        {
            if (SessionHasValue(Enums.SystemEmailMessageSession.SystemMessageModel))
                return (OTPEmailModel)GetValueFromSession(Enums.SystemEmailMessageSession.SystemMessageModel);

            return null;
        }
    }
}