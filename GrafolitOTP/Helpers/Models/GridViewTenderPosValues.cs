using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptimizacijaTransprotov.Helpers.Models
{
    public class GridViewTenderPosValues
    {
        public int KeyValue { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public string FieldName { get; set; }
    }
}