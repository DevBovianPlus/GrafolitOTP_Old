using DatabaseWebService.ModelsOTP.Recall;
using DatabaseWebService.ModelsOTP.Route;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptimizacijaTransprotov.Helpers.DataProviders
{
    public class CarriersInquiryProvider : ServerMasterPage
    {
        public void SetCarrierInquiryModel(List<CarrierInquiryModel> model)
        {
            AddValueToSession(Enums.RecallSession.InquirySummaries, model);
        }

        public List<CarrierInquiryModel> GetCarrierInquiryModel()
        {
            if (SessionHasValue(Enums.RecallSession.InquirySummaries))
                return (List<CarrierInquiryModel>)GetValueFromSession(Enums.RecallSession.InquirySummaries);

            return null;
        }
    }
}