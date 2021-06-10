using System;
using System.Collections.Generic;

namespace CheckToolsCORE.OmaData.DbModel
{
    public partial class VwAtzControlli
    {
        public decimal Id { get; set; }
        public string Codart { get; set; }
        public string Pn { get; set; }
        public string Codgr1 { get; set; }
        public string Codgr2 { get; set; }
        public string Desart { get; set; }
        public DateTime DataControllo { get; set; }
        public decimal IdCheckList { get; set; }
        public string LastUser { get; set; }
        public DateTime? LastEdit { get; set; }
        public decimal Periodicita { get; set; }
        public DateTime? Scadenza { get; set; }
        public string FileName { get; set; }
    }
}
