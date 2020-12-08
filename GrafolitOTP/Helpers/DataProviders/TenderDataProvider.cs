using DatabaseWebService.ModelsOTP.Tender;
using OptimizacijaTransprotov.Common;
using OptimizacijaTransprotov.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptimizacijaTransprotov.Helpers.DataProviders
{
    public class TenderDataProvider :ServerMasterPage
    {
        public void SetTenderFullModel(TenderFullModel model)
        {
            AddValueToSession(Enums.TenderSession.TenderFullModel, model);
        }

        public TenderFullModel GetTenderFullModel()
        {
            if (SessionHasValue(Enums.TenderSession.TenderFullModel))
                return (TenderFullModel)GetValueFromSession(Enums.TenderSession.TenderFullModel);

            return null;
        }

        public void SetSelectedTenderPositionRowsModel(List<TenderPositionModel> modelRows)
        {
            AddValueToSession(Enums.TenderSession.SelectedTenderPositionRows, modelRows);
        }

        public List<TenderPositionModel> GetSelectedTenderPositionRows()
        {
            if (SessionHasValue(Enums.TenderSession.SelectedTenderPositionRows))
                return (List<TenderPositionModel>)GetValueFromSession(Enums.TenderSession.SelectedTenderPositionRows);

            return null;
        }
    }
}