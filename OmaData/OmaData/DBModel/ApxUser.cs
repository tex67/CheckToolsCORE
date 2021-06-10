using System;
using System.Collections.Generic;

namespace CheckToolsCORE.OmaData.DbModel

{
    public partial class ApxUser
    {
        public ApxUser()
        {
            AtzRespo = new HashSet<AtzRespo>();
        }

        public decimal Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool UserRole { get; set; }
        public decimal IsEsterno { get; set; }
        public string Cenlav { get; set; }
        public string Operatore { get; set; }
        public string Focalc { get; set; }
        public string UtMitico { get; set; }
        public string MenuStyle { get; set; }
        public string Focalp { get; set; }
        public decimal? UtProject { get; set; }
        public string TipoReparto { get; set; }
        public decimal? IdFaEnti { get; set; }
        public bool RitFocalComm { get; set; }
        public string UtenteSpago { get; set; }
        public string Poscol { get; set; }
        public string Codcol { get; set; }
        public string Statino { get; set; }
        public decimal? IdStampanteDefault { get; set; }
        public decimal? Flggen { get; set; }
        public decimal? Flgabil { get; set; }
        public decimal? Flgcngpw { get; set; }
        public string CngpwToken { get; set; }
        public decimal? Telefono { get; set; }
        public decimal? Flgeu { get; set; }
        public string LdapUser { get; set; }
        public decimal? IdUserCopy { get; set; }
        public string Cfiscale { get; set; }
        public decimal? Flgfcpri { get; set; }
        public decimal TckitFlgabil { get; set; }
        public string CodtcfCodclf { get; set; }
        public string UserSchema { get; set; }
        public string UserSchemaTmp { get; set; }
        public decimal Flgsoccorso { get; set; }
        public decimal Compartimento { get; set; }
        public string DotnetToken { get; set; }

        public virtual ICollection<AtzRespo> AtzRespo { get; set; }
    }
}
