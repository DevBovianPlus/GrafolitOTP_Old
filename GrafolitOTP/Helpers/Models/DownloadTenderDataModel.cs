using DatabaseWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptimizacijaTransprotov.Helpers.Models
{
    public class DownloadTenderDataModel
    {
        public byte[] ByteData {get;set;}
        public string FileExtension { get; set; }
        public bool IsInline { get; set; }
        public string FileName { get; set; }

        public hlpTenderCreateExcellData _hlpTenderCreateExcellData { get; set; }
    }
}