using System;
using System.Collections.Generic;

namespace CheckToolsCORE.OmaData.DbModel
{

    public partial class AtzCheckList
    {
        public decimal Id { get; set; }
        public string Codart { get; set; }
        public string Codice { get; set; }
        public DateTime DataDocumento { get; set; }
        public DateTime DataCaricamento { get; set; }
        public decimal Active { get; set; }
        public string LastUser { get; set; }
        public DateTime? LastEdit { get; set; }
        public byte[] Data { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
    }
}
