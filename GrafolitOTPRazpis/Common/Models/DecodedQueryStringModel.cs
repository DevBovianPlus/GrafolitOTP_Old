using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrafolitOTPRazpis.Common.Models
{
    public class DecodedQueryStringModel
    {
        public int PrijavaPrevoznikaID { get; set; }
        public decimal CenaPrevoza { get; set; }
        public int RelacijaID { get; set; }
        public string NazivRelacije { get; set; }
        public string OpombaZaPovprasevnjePrevoznikom { get; set; }
        public DateTime DatumNaklada { get; set; }
        public string NazivPrevoznika { get; set; }
        public string StevilkaOdpoklica { get; set; }
    }
}