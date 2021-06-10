using System;
using CheckToolsCORE.OmaData.DbModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CheckToolsCORE.OmaData.Context
{ 
    public partial class DbContextMitico : DbContext
    {
        //public DbContextMitico()
        //{
        //    this.ChangeTracker.LazyLoadingEnabled = false;
        //    this.ChangeTracker.AutoDetectChangesEnabled = false;
        //}

        public DbContextMitico(DbContextOptions<DbContextMitico> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
            this.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public virtual DbSet<ApexWsSessions> ApexWsSessions { get; set; }
        public virtual DbSet<ApxUser> ApxUser { get; set; }
        public virtual DbSet<AtzCheckList> AtzCheckList { get; set; }
        public virtual DbSet<AtzControlli> AtzControlli { get; set; }
        public virtual DbSet<AtzRespo> AtzRespo { get; set; }
        public virtual DbSet<VwAtzControlli> VwAtzControlli { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // if (!optionsBuilder.IsConfigured)
          //  {
////#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
               //optionsBuilder.UseOracle("User Id=simcrp;Password=simcrp;Data Source=180.80.3.87:1521/OMADB;");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "SIMCRP");

            modelBuilder.Entity<ApexWsSessions>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("APEX_WS_SESSIONS");

                entity.Property(e => e.ApexSessionId)
                    .HasColumnName("APEX_SESSION_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.SessionCreated)
                    .HasColumnName("SESSION_CREATED")
                    .HasColumnType("DATE");

                entity.Property(e => e.SessionLang)
                    .HasColumnName("SESSION_LANG")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.SessionTerritory)
                    .HasColumnName("SESSION_TERRITORY")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SessionTimeZone)
                    .HasColumnName("SESSION_TIME_ZONE")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.WorkspaceDisplayName)
                    .HasColumnName("WORKSPACE_DISPLAY_NAME")
                    .IsUnicode(false);

                entity.Property(e => e.WorkspaceId)
                    .HasColumnName("WORKSPACE_ID")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.WorkspaceName)
                    .IsRequired()
                    .HasColumnName("WORKSPACE_NAME")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ApxUser>(entity =>
            {
                entity.ToTable("APX_USER");

                entity.HasIndex(e => e.Username)
                    .HasName("APX_USER_UK")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER");
                  

                entity.Property(e => e.Cenlav)
                    .HasColumnName("CENLAV")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Cfiscale)
                    .HasColumnName("CFISCALE")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.CngpwToken)
                    .HasColumnName("CNGPW_TOKEN")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Codcol)
                    .HasColumnName("CODCOL")
                    .HasColumnType("CHAR(4)");

                entity.Property(e => e.CodtcfCodclf)
                    .HasColumnName("CODTCF_CODCLF")
                    .HasMaxLength(255)
                    .IsUnicode(false);
                    

                entity.Property(e => e.Cognome)
                    .IsRequired()
                    .HasColumnName("COGNOME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Compartimento)
                    .HasColumnName("COMPARTIMENTO")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.DotnetToken)
                    .HasColumnName("DOTNET_TOKEN")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("EMAIL")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Flgabil)
                    .HasColumnName("FLGABIL")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Flgcngpw)
                    .HasColumnName("FLGCNGPW")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Flgeu)
                    .HasColumnName("FLGEU")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Flgfcpri)
                    .HasColumnName("FLGFCPRI")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Flggen)
                    .HasColumnName("FLGGEN")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Flgsoccorso)
                    .HasColumnName("FLGSOCCORSO")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Focalc)
                    .HasColumnName("FOCALC")
                    .HasColumnType("CHAR(8)");

                entity.Property(e => e.Focalp)
                    .HasColumnName("FOCALP")
                    .HasColumnType("CHAR(8)");

                entity.Property(e => e.IdFaEnti)
                    .HasColumnName("ID_FA_ENTI")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.IdStampanteDefault)
                    .HasColumnName("ID_STAMPANTE_DEFAULT")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.IdUserCopy)
                    .HasColumnName("ID_USER_COPY")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsEsterno)
                    .HasColumnName("IS_ESTERNO")
                    .HasColumnType("NUMBER");
                   

                entity.Property(e => e.LdapUser)
                    .HasColumnName("LDAP_USER")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MenuStyle)
                    .IsRequired()
                    .HasColumnName("MENU_STYLE")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("NOME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Operatore)
                    .HasColumnName("OPERATORE")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("PASSWORD")
                    .HasMaxLength(100)
                    .IsUnicode(false);
                   

                entity.Property(e => e.Poscol)
                    .HasColumnName("POSCOL")
                    .HasColumnType("CHAR(4)");

                entity.Property(e => e.RitFocalComm).HasColumnName("RIT_FOCAL_COMM");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnName("SALT")
                    .HasMaxLength(20)
                    .IsUnicode(false);
                   

                entity.Property(e => e.Statino)
                    .HasColumnName("STATINO")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.TckitFlgabil)
                    .HasColumnName("TCKIT_FLGABIL")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Telefono)
                    .HasColumnName("TELEFONO")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.TipoReparto)
                    .IsRequired()
                    .HasColumnName("TIPO_REPARTO")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.UserRole).HasColumnName("USER_ROLE");

                entity.Property(e => e.UserSchema)
                    .IsRequired()
                    .HasColumnName("USER_SCHEMA")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserSchemaTmp)
                    .HasColumnName("USER_SCHEMA_TMP")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("USERNAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UtMitico)
                    .HasColumnName("UT_MITICO")
                    .HasColumnType("CHAR(8)");

                entity.Property(e => e.UtProject)
                    .HasColumnName("UT_PROJECT")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.UtenteSpago)
                    .HasColumnName("UTENTE_SPAGO")
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AtzCheckList>(entity =>
            {
                entity.ToTable("ATZ_CHECK_LIST");

                entity.HasIndex(e => new { e.Codart, e.Codice })
                    .HasName("ATZ_CONTROLLI_LIST_UK")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)");
                    

                entity.Property(e => e.Active)
                    .HasColumnName("ACTIVE")
                    .HasColumnType("NUMBER(38)")
                    .HasDefaultValueSql("1 ");

                entity.Property(e => e.Codart)
                    .IsRequired()
                    .HasColumnName("CODART")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Codice)
                    .IsRequired()
                    .HasColumnName("CODICE")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Data)
                    .HasColumnName("DATA")
                    .HasColumnType("BLOB");

                entity.Property(e => e.DataCaricamento)
                    .HasColumnName("DATA_CARICAMENTO")
                    .HasColumnType("DATE");

                entity.Property(e => e.DataDocumento)
                    .HasColumnName("DATA_DOCUMENTO")
                    .HasColumnType("DATE");

                entity.Property(e => e.FileName)
                    .HasColumnName("FILE_NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastEdit)
                    .HasColumnName("LAST_EDIT")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUser)
                    .HasColumnName("LAST_USER")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MimeType)
                    .HasColumnName("MIME_TYPE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AtzControlli>(entity =>
            {
                entity.ToTable("ATZ_CONTROLLI");

                entity.HasIndex(e => e.Codart)
                    .HasName("ATZ_CONTROLLI_IX2");

                entity.HasIndex(e => new { e.Codart, e.DataControllo })
                    .HasName("ATZ_CONTROLLI_IX1");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Codart)
                    .IsRequired()
                    .HasColumnName("CODART")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.DataControllo)
                    .HasColumnName("DATA_CONTROLLO")
                    .HasColumnType("DATE");

                entity.Property(e => e.Dati)
                    .IsRequired()
                    .HasColumnName("DATI")
                    .HasColumnType("BLOB");

                entity.Property(e => e.FileName)
                    .HasColumnName("FILE_NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdCheckList)
                    .HasColumnName("ID_CHECK_LIST")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.LastEdit)
                    .HasColumnName("LAST_EDIT")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUser)
                    .HasColumnName("LAST_USER")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MimeType)
                    .HasColumnName("MIME_TYPE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AtzRespo>(entity =>
            {
                entity.ToTable("ATZ_RESPO");

                entity.HasIndex(e => new { e.Codgr1, e.Codgr2, e.UserId })
                    .HasName("UK_ATZ_RESPO")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Codgr1)
                    .IsRequired()
                    .HasColumnName("CODGR1")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Codgr2)
                    .IsRequired()
                    .HasColumnName("CODGR2")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("NUMBER(38)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AtzRespo)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ATZ_RESPO_APX_USER");
            });

            modelBuilder.Entity<VwAtzControlli>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VW_ATZ_CONTROLLI");

                entity.Property(e => e.Codart)
                    .IsRequired()
                    .HasColumnName("CODART")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Codgr1)
                    .IsRequired()
                    .HasColumnName("CODGR1")
                    .HasColumnType("CHAR(3)");

                entity.Property(e => e.Codgr2)
                    .IsRequired()
                    .HasColumnName("CODGR2")
                    .HasColumnType("CHAR(3)");
                
                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("FILE_NAME")
                    .HasColumnType("CHAR(100)");

                entity.Property(e => e.DataControllo)
                    .HasColumnName("DATA_CONTROLLO")
                    .HasColumnType("DATE");

                entity.Property(e => e.Desart)
                    .IsRequired()
                    .HasColumnName("DESART")
                    .HasColumnType("CHAR(60)");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.IdCheckList)
                    .HasColumnName("ID_CHECK_LIST")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.LastEdit)
                    .HasColumnName("LAST_EDIT")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUser)
                    .HasColumnName("LAST_USER")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Periodicita)
                    .HasColumnName("PERIODICITA")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Pn)
                    .IsRequired()
                    .HasColumnName("PN")
                    .HasColumnType("CHAR(32)");

                entity.Property(e => e.Scadenza)
                    .HasColumnName("SCADENZA")
                    .HasColumnType("DATE");
            });

        
          

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
