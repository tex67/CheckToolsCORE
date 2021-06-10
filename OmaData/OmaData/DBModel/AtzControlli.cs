using System;
using System.Collections.Generic;

namespace CheckToolsCORE.OmaData.DbModel
{

    public partial class AtzControlli
    {
        public string Codart { get; set; }
        public DateTime DataControllo { get; set; }
        public decimal IdCheckList { get; set; }
        public string LastUser { get; set; }
        public DateTime? LastEdit { get; set; }
        public decimal Id { get; set; }
        public byte[] Dati { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
    }
}
