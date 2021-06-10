using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace CheckToolsCORE.Models
{
    public class ControlliGridViewModel
    {
        public string CodArt { get; set; }
        public string Pn { get; set; }
        public string Descrizione { get; set; }
        public string RevLev { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd/MM/yyyy")] 
        public DateTime? UltimoControllo { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd/MM/yyyy")] 
        public DateTime? Scadenza { get; set; }
        public int Periodicita { get; set; }
        public string CurrentUser { get; set; }
       
    }
}