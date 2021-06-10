using System;
using System.Collections.Generic;

namespace CheckToolsCORE.OmaData.DbModel
{
    public partial class AtzRespo
    {
        public decimal Id { get; set; }
        public string Codgr1 { get; set; }
        public string Codgr2 { get; set; }
        public decimal UserId { get; set; }

        public virtual ApxUser User { get; set; }
    }
}
