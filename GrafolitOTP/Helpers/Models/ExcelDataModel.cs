using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptimizacijaTransprotov.Helpers.Models
{
    public class ExcelDataModel
    {
        public int TenderID { get; set; }
        public string TenderName { get; set; }

        public int CarrierID { get; set; }
        public string CarrierName { get; set; }

        public List<ExcelRouteModel> ExcelRoutes { get; set; }
    }

    public class ExcelRouteModel 
    {
        public int RouteID { get; set; }
        public string RouteName { get; set; }
        public List<ExcelTonsModel> TonsList { get; set; }
  
    }

    public class ExcelTonsModel
    {
        public int ZbirnikTonID { get; set; }
        public string TonsKoda { get; set; }
        public decimal Price { get; set; }
    }
}