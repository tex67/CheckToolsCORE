using System;
using System.Collections.Generic;

namespace CheckToolsCORE.OmaData.DbModel
{
    public partial class ApexWsSessions
    {
        public decimal WorkspaceId { get; set; }
        public string WorkspaceName { get; set; }
        public string WorkspaceDisplayName { get; set; }
        public decimal ApexSessionId { get; set; }
        public string UserName { get; set; }
        public DateTime? SessionCreated { get; set; }
        public string SessionTimeZone { get; set; }
        public string SessionLang { get; set; }
        public string SessionTerritory { get; set; }
    }
}
