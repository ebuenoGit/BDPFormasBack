using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Formas.Repositories.Context
{
    public class FormasContext : DbContext
    {
        public FormasContext()
        {
        }
        public FormasContext(DbContextOptions<FormasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aprobacioncarga> Aprobacioncarga { get; set; }
        public virtual DbSet<Aprobacionprecarga> Aprobacionprecarga { get; set; }
        public virtual DbSet<Concreteform> Concreteform { get; set; }
        public virtual DbSet<Form> Form { get; set; }
        public virtual DbSet<Form30> Form30 { get; set; }
        public virtual DbSet<Form30detail> Form30detail { get; set; }
        public virtual DbSet<Form9> Form9 { get; set; }
        public virtual DbSet<Form9detail> Form9detail { get; set; }
        public virtual DbSet<Form9totalvolumedetail> Form9totalvolumedetail { get; set; }
        public virtual DbSet<Formc4> Formc4 { get; set; }
        public virtual DbSet<Formc4netvolumedetail> Formc4netvolumedetail { get; set; }
        public virtual DbSet<Formc4totalvolumedetail> Formc4totalvolumedetail { get; set; }
        public virtual DbSet<Formstate> Formstate { get; set; }
        public virtual DbSet<Forma20Archivo> Forma20Archivo { get; set; }
        public virtual DbSet<Acumulados> Acumulados { set; get; }
        public virtual DbSet<Form20> Form20 { get; set; }
        public virtual DbSet<Form20detail> Form20detail { get; set; }
        public virtual DbSet<Form20productiondetail> Form20productiondetail { get; set; }
        public virtual DbSet<Form20totalvolumedetail> Form20totalvolumedetail { get; set; }
        public virtual DbSet<Form21> Form21 { get; set; }
        public virtual DbSet<Form21InyectionDetail> Form21InyectionDetail { get; set; }
        public virtual DbSet<Form21ProductionDetail> Form21ProductionDetail { get; set; }
        public virtual DbSet<Form21TotalVolumenDetail> Form21TotalVolumenDetail { get; set; }
        public virtual DbSet<Form23> Form23 { get; set; }
        public virtual DbSet<Form23Detail> Form23Detail { get; set; }
        public virtual DbSet<Form16> Form16 { get; set; }
        public virtual DbSet<Form16Detail> Form16Detail { get; set; }
        public virtual DbSet<FormC7> FormC7 { get; set; }
        public virtual DbSet<FormC7Detail> FormaC7Detail { get; set; }
        public virtual DbSet<FormC7Total> FormC7Total { get; set; }
        public virtual DbSet<Forma15CRTable> Form15CR { get; set; }
        public virtual DbSet<Forma15CRTableDetalle> Form15CRDetalle { get; set; }
        public virtual DbSet<Forma17CRTable> Form17CR { get; set; }
        public virtual DbSet<Forma17CRTableDetalle> Form17CRDetalle { get; set; }
        // public virtual DbSet<Form22CRTable> Form22CR { get; set; }
        public virtual DbSet<Forma22cr> Forma22crs { get; set; }
        //public virtual DbSet<Form22CRTableDetalle> Form22CRDetalle { get; set; }
        public virtual DbSet<Forma22crdetalle> Forma22crdetalles { get; set; }
        public virtual DbSet<Cuadro1cabecera> Cuadro1cabecera { get; set; }
        public virtual DbSet<Cuadro1detalle> Cuadro1detalle { get; set; }
        public virtual DbSet<Cuadro1total> Cuadro1total { get; set; }


        public virtual DbSet<Formtype> Formtype { get; set; }
        // public virtual DbSet<Homologacion> Homologacion { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Cuadro1cabecera>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CUADRO1CABECERA", "FOXT");

                entity.Property(e => e.Anio)
                    .HasColumnName("anio")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Bateria)
                    .HasColumnName("bateria")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Campo)
                    .HasColumnName("campo")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CampoId).HasColumnName("campo_id");

                entity.Property(e => e.Compania)
                    .HasColumnName("compania")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CompaniaId).HasColumnName("compania_id");

                entity.Property(e => e.Contrato)
                    .HasColumnName("contrato")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContratoId).HasColumnName("contrato_id");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("fecha_creacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FormaId).HasColumnName("Forma_id");

                entity.Property(e => e.IdFormaCuadro1).HasColumnName("id_FormaCuadro1");

                entity.Property(e => e.Lugar)
                    .HasColumnName("lugar")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mes)
                    .HasColumnName("mes")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Tanque)
                    .HasColumnName("tanque")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCrea)
                    .HasColumnName("usuario_crea")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.id_tanque)
                    .HasColumnName("id_tanque")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.id_bateria)
                    .HasColumnName("id_bateria")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

            });

            modelBuilder.Entity<Cuadro1detalle>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CUADRO1DETALLE", "FOXT");

                entity.Property(e => e.AforoBls)
                    .HasColumnName("aforoBLS")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Api60f)
                    .HasColumnName("API60F")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Bls60f)
                    .HasColumnName("BLS60F")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Blsnetos)
                    .HasColumnName("BLSNetos")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Bsw)
                    .HasColumnName("BSW")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Ctsh)
                    .HasColumnName("CTSH")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Dias)
                    .HasColumnName("dias")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EntregaBls)
                    .HasColumnName("entregaBLS")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.FactorBsw)
                    .HasColumnName("factorBSW")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.FactorTemp)
                    .HasColumnName("factorTemp")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("fecha_creacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FormaId).HasColumnName("Forma_id");

                entity.Property(e => e.Ge)
                    .HasColumnName("GE")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.IdCabecera).HasColumnName("id_cabecera");

                entity.Property(e => e.MedidaMm)
                    .HasColumnName("medidaMM")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.NetosGe)
                    .HasColumnName("netosGE")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.RecibidoBls)
                    .HasColumnName("recibidoBLS")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.SalBtb)
                    .HasColumnName("salBTB")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.TempAmb)
                    .HasColumnName("tempAmb")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.TempF)
                    .HasColumnName("tempF")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.TransfBls)
                    .HasColumnName("transfBLS")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.UsuarioCrea)
                    .HasColumnName("usuario_crea")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PdenId)
                    .HasColumnName("pdenId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.TrasEnvio)
               .HasColumnName("TrasEnvio")
               .HasColumnType("decimal(20, 2)")
               .IsUnicode(false);

                entity.Property(e => e.TrasRecibido)
               .HasColumnName("TrasRecibido")
               .HasColumnType("decimal(20, 2)")
               .IsUnicode(false);

                entity.Property(e => e.MovIntraRecibido)
               .HasColumnName("MovIntraRecibido")
               .HasColumnType("decimal(20, 2)")
               .IsUnicode(false);

                entity.Property(e => e.MovIntraEnvio)
               .HasColumnName("MovIntraEnvio")
               .HasColumnType("decimal(20, 2)")
               .IsUnicode(false);
            });

            modelBuilder.Entity<Cuadro1total>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CUADRO1TOTAL", "FOXT");

                entity.Property(e => e.Bls60f)
                    .HasColumnName("BLS60F")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.Blsnetos)
                    .HasColumnName("BLSNetos")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.EntregaBls)
                    .HasColumnName("entregaBLS")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("fecha_creacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FormaId).HasColumnName("Forma_id");

                entity.Property(e => e.IdCabecera).HasColumnName("id_cabecera");

                entity.Property(e => e.RecibidoBls)
                    .HasColumnName("recibidoBLS")
                    .HasColumnType("decimal(20, 2)");

                entity.Property(e => e.UsuarioCrea)
                    .HasColumnName("usuario_crea")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });

            modelBuilder.Entity<Acumulados>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.ToTable("Acumulados", "FOXT");
                entity.Property(e => e.Id_Forma).HasColumnType("(newid())");
                entity.Property(e => e.AguaAcumulado).HasColumnType("varchar(50)");
                entity.Property(e => e.GasAcumulado).HasColumnType("varchar(50)");
                entity.Property(e => e.CrudoAcumulado).HasColumnType("varchar(50)");
            });
            modelBuilder.Entity<Aprobacioncarga>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.ToTable("APROBACIONCARGA", "FOXT");

                entity.Property(e => e.ComparativoAgua).HasColumnType("numeric(38, 0)");

                entity.Property(e => e.ComparativoCrudo).HasColumnType("numeric(38, 0)");

                entity.Property(e => e.ComparativoGas).HasColumnType("numeric(38, 0)");

                entity.Property(e => e.Estado).HasDefaultValueSql("(newid())");

                entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");

                entity.Property(e => e.FechaCarga).HasColumnType("datetime");

                entity.Property(e => e.FechaForma).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(38, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdForma)
                    .HasColumnName("ID_Forma")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Usuario).HasDefaultValueSql("(newid())");
                entity.Property(e => e.UsuarioNombre).HasDefaultValueSql("varchar(150)");
                entity.Property(e => e.UsuarioAprobador).HasColumnType("varchar(50)");
                entity.Property(e => e.UsuarioNombreAprobador).HasColumnType("varchar(150)");
                entity.Property(e => e.FormaName).HasColumnType("varchar(50)");
                entity.Property(e => e.UsuarioNombre).HasColumnType("varchar(50)");
                entity.Property(e => e.UsuarioNombreAprobador).HasColumnType("varchar(50)");
                entity.Property(e => e.Operadora).HasColumnType("varchar(50)")
                .HasColumnName("operador");
                entity.Property(e => e.Contrato).HasColumnType("varchar(50)")
                .HasColumnName("contrato");

                entity.Property(e => e.Campo).HasColumnType("varchar(50)")
                .HasColumnName("campo");
                entity.Property(e => e.UrlForma).HasColumnName("urlForma").HasColumnType("varchar(150)");
            });
            modelBuilder.Entity<Aprobacionprecarga>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.ToTable("APROBACIONPRECARGA", "FOXT");

                entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");

                entity.Property(e => e.FechaApertura).HasColumnType("datetime");

                entity.Property(e => e.FechaCierre).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaForma).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(38, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdForma)
                    .HasColumnName("ID_Forma")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Usuario).HasDefaultValueSql("varchar(10)");

                entity.Property(e => e.UsuarioAprobador).HasDefaultValueSql("varchar(50)");
                entity.Property(e => e.Usuario).HasDefaultValueSql("varchar(50)");

                entity.Property(e => e.UsuarioAprobador).HasColumnType("varchar(50)");

                entity.Property(e => e.FormaName).HasColumnType("varchar(50)");
                entity.Property(e => e.UsuarioNombre).HasColumnType("varchar(50)");
                entity.Property(e => e.UsuarioNombreAprobador).HasColumnType("varchar(50)");
                entity.Property(e => e.Operadora).HasColumnType("varchar(50)").HasColumnName("operador");
                entity.Property(e => e.Activo).HasColumnType("numeric(38,0)")
                .HasColumnName("Activo");
                entity.Property(e => e.Contrato).HasColumnType("varchar(50)")
                .HasColumnName("contrato");

                entity.Property(e => e.Campo).HasColumnType("varchar(50)")
                .HasColumnName("campo");
                entity.Property(e => e.UrlForma).HasColumnName("urlForma").HasColumnType("varchar(150)");
                entity.Property(e => e.Motivo).HasColumnName("Motivo");

            });
            modelBuilder.Entity<Concreteform>(entity =>
            {
                entity.ToTable("CONCRETEFORM", "FOXT");

                entity.Property(e => e.Concreteformid)
                    .HasColumnName("concreteformid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Annotations)
                    .HasColumnName("annotations")
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Battery)
                    .HasColumnName("battery")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Campid)
                    .HasColumnName("campid")
                    .HasColumnType("numeric(38, 0)");

                entity.Property(e => e.Company)
                    .HasColumnName("company")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Contract)
                    .HasColumnName("contract")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Currentstate).HasColumnName("currentstate");

                entity.Property(e => e.Explotationmodality)
                    .HasColumnName("explotationmodality")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Formname)
                    .HasColumnName("formname")
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.Generationflag)
                    .HasColumnName("generationflag")
                    .HasColumnType("numeric(1, 0)");

                entity.Property(e => e.Generationjobid)
                    .HasColumnName("generationjobid")
                    .HasColumnType("numeric(38, 0)");

                entity.Property(e => e.Iqistatus)
                    .HasColumnName("iqistatus")
                    .HasColumnType("numeric(22, 0)");

                entity.Property(e => e.Maincampid)
                    .HasColumnName("maincampid")
                    .HasColumnType("numeric(38, 0)");

                entity.Property(e => e.Minrepsigning)
                    .HasColumnName("minrepsigning")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Month)
                    .HasColumnName("month")
                    .HasColumnType("numeric(2, 0)");

                entity.Property(e => e.Pdenid)
                    .HasColumnName("pdenid")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Tank)
                    .HasColumnName("tank")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Usersigning)
                    .HasColumnName("usersigning")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Version)
                    .HasColumnName("version")
                    .HasColumnType("numeric(8, 0)");

                entity.Property(e => e.Year)
                    .HasColumnName("year")
                    .HasColumnType("numeric(4, 0)");
            });
            modelBuilder.Entity<Form>(entity =>
            {
                entity.ToTable("FORM", "FOXT");

                entity.Property(e => e.Formid)
                    .HasColumnName("formid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Formtypeid).HasColumnName("formtypeid");

                entity.Property(e => e.Ismaincamp)
                    .IsRequired()
                    .HasColumnName("ismaincamp")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.HasOne(d => d.Formtype)
                    .WithMany(p => p.Form)
                    .HasForeignKey(d => d.Formtypeid)
                    .HasConstraintName("FK_FORM_FORMTYPE");
            });
            modelBuilder.Entity<Form30>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("FORM30", "FOXT");

                entity.Property(e => e.Form30id)
                    .HasColumnName("form30id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Gpbutane)
                    .HasColumnName("gpbutane")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gpgasoline)
                    .HasColumnName("gpgasoline")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gppgcburned)
                    .HasColumnName("gppgcburned")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gppgcconsumed)
                    .HasColumnName("gppgcconsumed")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gppgcelectricgeneration)
                    .HasColumnName("gppgcelectricgeneration")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gppgcinyected)
                    .HasColumnName("gppgcinyected")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gppgcothersales)
                    .HasColumnName("gppgcothersales")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gppgcurbangaspipeline)
                    .HasColumnName("gppgcurbangaspipeline")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gppropane)
                    .HasColumnName("gppropane")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gptotalprocessedgas)
                    .HasColumnName("gptotalprocessedgas")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gptransformedgas)
                    .HasColumnName("gptransformedgas")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gpugcmechanicpumping)
                    .HasColumnName("gpugcmechanicpumping")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalbasicgasproduction)
                    .HasColumnName("totalbasicgasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalformationprocessedgas)
                    .HasColumnName("totalformationprocessedgas")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalgasproduction)
                    .HasColumnName("totalgasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalincrementalgasproduction)
                    .HasColumnName("totalincrementalgasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalugcburned)
                    .HasColumnName("totalugcburned")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalugcconsumed)
                    .HasColumnName("totalugcconsumed")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalugcelectricgeneration)
                    .HasColumnName("totalugcelectricgeneration")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalugcinyected)
                    .HasColumnName("totalugcinyected")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalugcmechanicpumping)
                    .HasColumnName("totalugcmechanicpumping")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalugcothersales)
                    .HasColumnName("totalugcothersales")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalugcurbangaspipeline)
                    .HasColumnName("totalugcurbangaspipeline")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalwpcbutane)
                    .HasColumnName("totalwpcbutane")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalwpcgasoline)
                    .HasColumnName("totalwpcgasoline")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalwpcpropane)
                    .HasColumnName("totalwpcpropane")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });
            modelBuilder.Entity<Form30detail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("FORM30DETAIL", "FOXT");

                entity.Property(e => e.Basicgasproduction)
                    .HasColumnName("basicgasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Danecode)
                    .HasColumnName("danecode")
                    .HasColumnType("numeric(6, 0)");

                entity.Property(e => e.Form30detail1)
                    .HasColumnName("form30detail")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Formation)
                    .HasColumnName("formation")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Formationprocessedgas)
                    .HasColumnName("formationprocessedgas")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Incrementalgasproduction)
                    .HasColumnName("incrementalgasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Oilfieldid)
                    .HasColumnName("oilfieldid")
                    .HasColumnType("numeric(38, 0)");

                entity.Property(e => e.Totalgasproduction)
                    .HasColumnName("totalgasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Ugcburned)
                    .HasColumnName("ugcburned")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Ugcelectricgeneration)
                    .HasColumnName("ugcelectricgeneration")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Ugcinyected)
                    .HasColumnName("ugcinyected")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Ugcmechanicpumping)
                    .HasColumnName("ugcmechanicpumping")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Ugcoconsumed)
                    .HasColumnName("ugcoconsumed")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Ugcothersales)
                    .HasColumnName("ugcothersales")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Ugcurbangaspipeline)
                    .HasColumnName("ugcurbangaspipeline")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Wpcbutane)
                    .HasColumnName("wpcbutane")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Wpcgasoline)
                    .HasColumnName("wpcgasoline")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Wpcpropane)
                    .HasColumnName("wpcpropane")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });
            modelBuilder.Entity<Form9>(entity =>
            {
                // entity.HasNoKey();
                entity.ToTable("FORM9", "FOXT");

                entity.Property(e => e.Form9id)
                    .HasColumnName("form9id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Block)
                    .HasColumnName("block")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Member)
                    .HasColumnName("member")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Oilfield)
                    .HasColumnName("oilfield")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Structure)
                    .HasColumnName("structure")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.operadorId)
                  .HasColumnName("operadorId")
                  .HasColumnType("varchar(60)");

                entity.Property(e => e.campoId)
                  .HasColumnName("campoId")
                  .HasColumnType("varchar(60)");

                entity.Property(e => e.bloqueId)
                  .HasColumnName("bloqueId")
                  .HasColumnType("varchar(60)");

                entity.Property(e => e.formacionId)
                 .HasColumnName("formacionId")
                 .HasColumnType("varchar(60)");

                entity.Property(e => e.formacionSetId)
                 .HasColumnName("formacionSetId")
                 .HasColumnType("varchar(60)");

                entity.Property(e => e.yacimientoId)
                 .HasColumnName("yacimientoId")
                 .HasColumnType("varchar(60)");

                entity.Property(e => e.campo)
                 .HasColumnName("campo")
                 .HasColumnType("varchar(60)");

                entity.Property(e => e.formacion)
                 .HasColumnName("formacion")
                 .HasColumnType("varchar(60)");

                entity.Property(e => e.bloque)
                 .HasColumnName("bloque")
                 .HasColumnType("varchar(60)");

                entity.Property(e => e.yacimiento)
                 .HasColumnName("yacimiento")
                 .HasColumnType("varchar(60)");

                entity.Property(e => e.contratoId)
                    .HasColumnName("contratoId")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

            });
            modelBuilder.Entity<Form9detail>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("FORM9DETAIL", "FOXT");

                entity.HasIndex(e => e.Form9detailid)
                    .HasName("FORM9DETAIL_UK")
                    .IsUnique();

                entity.Property(e => e.Accumulatedays)
                    .HasColumnName("accumulatedays")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Accumulategasproduction)
                    .HasColumnName("accumulategasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Accumulateoilproduction)
                    .HasColumnName("accumulateoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Accumulatewaterproduction)
                    .HasColumnName("accumulatewaterproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Apigrades)
                    .HasColumnName("apigrades")
                    .HasColumnType("numeric(15, 5)");

                entity.Property(e => e.Bsw)
                    .HasColumnName("bsw")
                    .HasColumnType("numeric(8, 5)");

                entity.Property(e => e.Correctionfactor)
                    .HasColumnName("correctionfactor")
                    .HasColumnType("numeric(20, 5)");

                entity.Property(e => e.Dailygasproduction)
                    .HasColumnName("dailygasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Dailyoilproduction)
                    .HasColumnName("dailyoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Dailywaterproduction)
                    .HasColumnName("dailywaterproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Danecode)
                    .HasColumnName("danecode")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Form9detailid)
                    .HasColumnName("form9detailid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Formation)
                    .HasColumnName("formation")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Monthdays)
                    .HasColumnName("monthdays")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Monthlyoilproduction)
                    .HasColumnName("monthlyoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Monthlywaterproduction)
                    .HasColumnName("monthlywaterproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Montlhygasproduction)
                    .HasColumnName("montlhygasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Oilwell)
                    .HasColumnName("oilwell")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Oilwellfinalstate)
                    .HasColumnName("oilwellfinalstate")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Poolname)
                    .HasColumnName("poolname")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.PdenId)
                   .HasColumnName("pdenid")
                   .HasColumnType("varchar(60)");

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);



                entity.Property(e => e.Productionmethod)
                    .HasColumnName("productionmethod")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Rgp)
                    .HasColumnName("rgp")
                    .HasColumnType("numeric(20, 5)");

                entity.HasOne(d => d.Form)
                    .WithMany()
                    .HasForeignKey(d => d.Formid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FORM9DETAIL_FORM9_FK1");
            });
            modelBuilder.Entity<Form9totalvolumedetail>(entity =>
            {
                entity.HasKey(e => e.Form9tvdetailid);
                //entity.HasNoKey();

                entity.ToTable("FORM9TOTALVOLUMEDETAIL", "FOXT");

                entity.Property(e => e.Accumulategasproduction)
                    .HasColumnName("accumulategasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Accumulateoilproduction)
                    .HasColumnName("accumulateoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Accumulatewaterproduction)
                    .HasColumnName("accumulatewaterproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Apigrades)
                    .HasColumnName("apigrades")
                    .HasColumnType("numeric(8, 5)");

                entity.Property(e => e.Bsw)
                    .HasColumnName("bsw")
                    .HasColumnType("numeric(8, 5)");

                entity.Property(e => e.Dailygasproduction)
                    .HasColumnName("dailygasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Dailyoilproduction)
                    .HasColumnName("dailyoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Dailywaterproduction)
                    .HasColumnName("dailywaterproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Form9tvdetailid)
                    .HasColumnName("form9tvdetailid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Formation)
                    .HasColumnName("formation")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Formid).HasColumnName("formid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Monthlyoilproduction)
                    .HasColumnName("monthlyoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Monthlywaterproduction)
                    .HasColumnName("monthlywaterproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Montlhygasproduction)
                    .HasColumnName("montlhygasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Rgp)
                    .HasColumnName("rgp")
                    .HasColumnType("numeric(20, 5)");

                entity.Property(e => e.Volumetype)
                    .HasColumnName("volumetype")
                    .HasColumnType("numeric(1, 0)");

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });

            modelBuilder.Entity<Formc4>(entity =>
            {
                entity.ToTable("FORMC4", "FOXT");

                entity.Property(e => e.Formc4id)
                   .HasColumnName("formc4id")
                   .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Apigrades)
                    .HasColumnName("apigrades")
                    .HasColumnType("numeric(8, 5)");

                entity.Property(e => e.Bloque)
                    .HasColumnName("bloque")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.BloqueId)
                    .HasColumnName("bloqueId")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Bsw)
                    .HasColumnName("bsw")
                    .HasColumnType("numeric(8, 5)");

                entity.Property(e => e.Campo)
                    .HasColumnName("campo")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.CampoId)
                    .HasColumnName("campoId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Contrato)
                    .HasColumnName("contrato")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.ContratoId)
                    .HasColumnName("contratoId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Deliveries)
                    .HasColumnName("deliveries")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Deliverysite)
                    .HasColumnName("deliverysite")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estructura)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EstructuraId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Finalexistence)
                    .HasColumnName("finalexistence")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Formacion)
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.FormacionId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FormacionSetId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Initialstock)
                    .HasColumnName("initialstock")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Linesdeadvolume)
                    .HasColumnName("linesdeadvolume")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Sulfurcontent)
                    .HasColumnName("sulfurcontent")
                    .HasColumnType("numeric(8, 5)");

                entity.Property(e => e.Totalbalance)
                    .HasColumnName("totalbalance")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Vesselsdeadvolume)
                    .HasColumnName("vesselsdeadvolume")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Yacimiento)
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.YacimientoId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

            });

            modelBuilder.Entity<Formc4netvolumedetail>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("FORMC4NETVOLUMEDETAIL", "FOXT");

                entity.Property(e => e.Activity)
                    .HasColumnName("activity")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Basica)
                    .HasColumnName("basica")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Incremental)
                    .HasColumnName("incremental")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Danecode)
                    .HasColumnName("danecode")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Formation)
                    .HasColumnName("formation")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Formc4netvolumedetailid)
                    .HasColumnName("formc4netvolumedetailid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Municipality)
                    .HasColumnName("municipality")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Productiontype)
                    .HasColumnName("productiontype")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Volume)
                    .HasColumnName("volume")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });

            modelBuilder.Entity<Formc4totalvolumedetail>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("FORMC4TOTALVOLUMEDETAIL", "FOXT");

                entity.Property(e => e.Activity)
                    .HasColumnName("activity")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Formation)
                    .HasColumnName("formation")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Formc4totalvolumedetailid)
                    .HasColumnName("formc4totalvolumedetailid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Totalvolume)
                    .HasColumnName("totalvolume")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });

            modelBuilder.Entity<Formstate>(entity =>
            {
                entity.ToTable("FORMSTATE", "FOXT");

                entity.Property(e => e.Formstateid)
                    .HasColumnName("formstateid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<Formtype>(entity =>
            {
                entity.ToTable("FORMTYPE", "FOXT");

                entity.Property(e => e.Formtypeid)
                    .HasColumnName("formtypeid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<Forma20Archivo>(entity =>
                {
                    entity.Property(e => e.id).ValueGeneratedOnAdd();
                    entity.Property(e => e.forma).HasColumnType("Varchar(50)");
                    entity.Property(e => e.name).HasColumnType("Varchar(200)");
                    entity.Property(e => e.size).HasColumnType("int");
                    entity.Property(e => e.type).HasColumnType("Varchar(200)");
                    entity.Property(e => e.uid).HasColumnType("Varchar(50)");
                    entity.Property(e => e.User_Created).HasColumnType("Varchar(50)");
                    entity.Property(e => e.Date_Created).HasColumnType("datetime");



                });
            modelBuilder.Entity<Form20>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("FORM20", "FOXT");

                entity.Property(e => e.Block)
                 .HasColumnName("block")
                 .HasMaxLength(50)
                 .IsUnicode(false);

                entity.Property(e => e.campo)
                    .HasColumnName("campo")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.campoId)
                    .HasColumnName("campoId")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.contrato)
                    .HasColumnName("contrato")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.contratoId)
                    .HasColumnName("contratoId")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Form20id)
                    .HasColumnName("form20id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Formation)
                    .HasColumnName("formation")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Member)
                    .HasColumnName("member")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Oilfield)
                    .HasColumnName("oilfield")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.operador)
                    .HasColumnName("operador")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.operadorId)
                    .HasColumnName("operadorId")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Structure)
                    .HasColumnName("structure")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.formacion_id)
                    .HasColumnName("formacion_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.formacion_set_id)
                    .HasColumnName("formacion_set_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });

            modelBuilder.Entity<Form20detail>(entity =>
            {
                // entity.HasNoKey();

                entity.ToTable("FORM20DETAIL", "FOXT");

                entity.Property(e => e.Danecode)
                    .HasColumnName("danecode")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Form20detailid)
                    .HasColumnName("form20detailid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Oilwell)
                    .HasColumnName("oilwell")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Oilwellfinalstate)
                    .HasColumnName("oilwellfinalstate")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Poolname)
                    .HasColumnName("poolname")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Pressure)
                    .HasColumnName("pressure")
                    .HasColumnType("numeric(12, 2)");

                entity.Property(e => e.Wiaccumulateddays)
                    .HasColumnName("wiaccumulateddays")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Wiaccumulatedwater)
                    .HasColumnName("wiaccumulatedwater")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Widailywater)
                    .HasColumnName("widailywater")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Widays)
                    .HasColumnName("widays")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Wimonthlywater)
                    .HasColumnName("wimonthlywater")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Zone)
                    .HasColumnName("zone")
                    .HasMaxLength(20)
                    .IsUnicode(false);


                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });
            modelBuilder.Entity<Form20productiondetail>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("FORM20PRODUCTIONDETAIL", "FOXT");

                entity.Property(e => e.Accumulatedays)
                    .HasColumnName("accumulatedays")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Accumulatedwater)
                    .HasColumnName("accumulatedwater")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Accumulateoilproduction)
                    .HasColumnName("accumulateoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Correctionfactor)
                    .HasColumnName("correctionfactor")
                    .HasColumnType("numeric(8, 5)");

                entity.Property(e => e.Dailyoilproduction)
                    .HasColumnName("dailyoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Dailywater)
                    .HasColumnName("dailywater")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Danecode)
                    .HasColumnName("danecode")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Form20productiondetailid)
                    .HasColumnName("form20productiondetailid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Monthdays)
                    .HasColumnName("monthdays")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Monthlyoilproduction)
                    .HasColumnName("monthlyoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Monthlywater)
                    .HasColumnName("monthlywater")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Oilwell)
                    .HasColumnName("oilwell")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Oilwellfinalstate)
                    .HasColumnName("oilwellfinalstate")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Pressure)
                    .HasColumnName("pressure")
                    .HasColumnType("numeric(12, 2)");

                entity.Property(e => e.Productionmethod)
                    .HasColumnName("productionmethod")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });
            modelBuilder.Entity<Form20totalvolumedetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("FORM20TOTALVOLUMEDETAIL", "FOXT");

                entity.Property(e => e.Accumulatewaterinjection)
                    .HasColumnName("accumulatewaterinjection")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Dailywaterinjection)
                    .HasColumnName("dailywaterinjection")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Form20tvdetailid)
                    .HasColumnName("form20tvdetailid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Formation)
                    .HasColumnName("formation")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Monthlywaterinjection)
                    .HasColumnName("monthlywaterinjection")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Volumetype)
                    .HasColumnName("volumetype")
                    .HasColumnType("numeric(1, 0)");

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });
            modelBuilder.Entity<Form21>(entity =>
            {
                entity.ToTable("FORM21", "FOXT");

                entity.Property(e => e.Form21id)
                    .HasColumnName("form21id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Block)
                    .HasColumnName("block")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Formation)
                    .HasColumnName("formation")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Member)
                    .HasColumnName("member")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Oilfield)
                    .HasColumnName("oilfield")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Structure)
                    .HasColumnName("structure")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Operadorid)
                    .HasColumnName("operadorid")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Operador)
                    .HasColumnName("operador")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Contractid)
                    .HasColumnName("contractid")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Contract)
                    .HasColumnName("contract")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Campoid)
                    .HasColumnName("campoid")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Campo)
                    .HasColumnName("campo")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });
            modelBuilder.Entity<Form21InyectionDetail>(entity =>
            {
                entity.HasKey(e => e.Form21detailid)
                    .HasName("PK__FORM21IN__6CABE0B075908AFA");

                entity.ToTable("FORM21INYECTIONDETAIL", "FOXT");

                entity.Property(e => e.Form21detailid)
                    .HasColumnName("form21detailid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Danecode)
                    .HasColumnName("danecode")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Giaccumulateddays)
                    .HasColumnName("giaccumulateddays")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Giaccumulatedgas)
                    .HasColumnName("giaccumulatedgas")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gidailygas)
                    .HasColumnName("gidailygas")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gidays)
                    .HasColumnName("gidays")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gimonthlygas)
                    .HasColumnName("gimonthlygas")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Oilwell)
                    .HasColumnName("oilwell")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Oilwellfinalstate)
                    .HasColumnName("oilwellfinalstate")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Poolname)
                    .HasColumnName("poolname")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Pressure)
                    .HasColumnName("pressure")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Zone)
                    .HasColumnName("zone")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Pden_id)
                    .HasColumnName("pden_id")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });
            modelBuilder.Entity<Form21ProductionDetail>(entity =>
            {
                entity.HasKey(e => e.Form21proddetailid)
                    .HasName("PK__FORM21PR__5381A6ACB90E37DC");

                entity.ToTable("FORM21PRODUCTIONDETAIL", "FOXT");

                entity.Property(e => e.Form21proddetailid)
                    .HasColumnName("form21proddetailid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Accumulatedays)
                    .HasColumnName("accumulatedays")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Accumulategasproduction)
                    .HasColumnName("accumulategasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Accumulateoilproduction)
                    .HasColumnName("accumulateoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Accumulatewaterproduction)
                    .HasColumnName("accumulatewaterproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Dailygasproduction)
                    .HasColumnName("dailygasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Dailyoilproduction)
                    .HasColumnName("dailyoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Dailywaterproduction)
                    .HasColumnName("dailywaterproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Danecode)
                    .HasColumnName("danecode")
                     .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Monthdays)
                    .HasColumnName("monthdays")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Monthlyoilproduction)
                    .HasColumnName("monthlyoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Monthlywaterproduction)
                    .HasColumnName("monthlywaterproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Montlhygasproduction)
                    .HasColumnName("montlhygasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Oilwell)
                    .HasColumnName("oilwell")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Oilwellfinalstate)
                    .HasColumnName("oilwellfinalstate")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Pressure)
                    .HasColumnName("pressure")
                    .HasColumnType("numeric(12, 2)");

                entity.Property(e => e.Productionmethod)
                    .HasColumnName("productionmethod")
                     .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Zone)
                    .HasColumnName("zone")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Pden_id)
                    .HasColumnName("pden_id")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });
            modelBuilder.Entity<Form21TotalVolumenDetail>(entity =>
            {
                entity.HasKey(e => e.Idform21tvdetailid)
                    .HasName("PK__FORM21TO__39D668FBAADAF4D7");

                entity.ToTable("FORM21TOTALVOLUMEDETAIL", "FOXT");

                entity.Property(e => e.Idform21tvdetailid).ValueGeneratedNever();

                entity.Property(e => e.Accumulategasinjection)
                    .HasColumnName("accumulategasinjection")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Dailygasinjection)
                    .HasColumnName("dailygasinjection")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Formation)
                    .HasColumnName("formation")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Monthlygasinjection)
                    .HasColumnName("monthlygasinjection")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Volumetype)
                    .HasColumnName("volumetype")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });
            modelBuilder.Entity<Form16>(entity =>
            {
                entity.ToTable("FORM16", "FOXT");

                entity.Property(e => e.Form16id)
                    .HasColumnName("form16id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Block)
                    .HasColumnName("block")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Formation)
                    .HasColumnName("formation")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Member)
                    .HasColumnName("member")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Oilfield)
                    .HasColumnName("oilfield")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Structure)
                    .HasColumnName("structure")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Operador)
                    .HasColumnName("operador")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Contractid)
                    .HasColumnName("contractid")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Contract)
                    .HasColumnName("contract")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Campoid)
                    .HasColumnName("campoid")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Campo)
                    .HasColumnName("campo")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Yacimiento)
                    .HasColumnName("yacimiento")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });
            modelBuilder.Entity<Form16Detail>(entity =>
            {
                entity.ToTable("FORM16DETAIL", "FOXT");

                entity.Property(e => e.Form16detailid)
                    .HasColumnName("form16detailid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Oilwell)
                    .HasColumnName("oilwell")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Productionmethod)
                    .HasColumnName("productionmethod")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Zone)
                    .HasColumnName("zone")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Testingdate)
                    .HasColumnName("testingdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Oilwellstate)
                    .HasColumnName("oilwellstate")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Thppressure)
                    .HasColumnName("thppressure")
                    .HasColumnType("numeric(10, 4)");

                entity.Property(e => e.Chppressure)
                    .HasColumnName("chppressure")
                    .HasColumnType("numeric(10, 4)");

                entity.Property(e => e.Hours)
                    .HasColumnName("hours")
                    .HasColumnType("numeric(2, 0)");

                entity.Property(e => e.Dailyoilproduction)
                    .HasColumnName("dailyoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.ReductionSize)
                    .HasColumnName("reductionSize")
                    .HasColumnType("numeric(10, 4)");

                entity.Property(e => e.WellPumpingLength)
                    .HasColumnName("wellpumpinglength")
                    .HasColumnType("numeric(10, 4)");

                entity.Property(e => e.WellPumpDumpsMinute)
                    .HasColumnName("wellpumpdumpsminute")
                    .HasColumnType("numeric(10, 4)");

                entity.Property(e => e.ProductionTestingOilBLS)
                    .HasColumnName("productiontestingoilBLS")
                    .HasColumnType("numeric(10, 4)");

                entity.Property(e => e.ProductionTestGravityOil)
                    .HasColumnName("productiontestgravityoil")
                    .HasColumnType("numeric(10, 4)");

                entity.Property(e => e.Api)
                    .HasColumnName("api")
                    .HasColumnType("numeric(8, 5)");

                entity.Property(e => e.Gas)
                    .HasColumnName("gas")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Rga)
                    .HasColumnName("rga")
                    .HasColumnType("numeric(15, 6)");

                entity.Property(e => e.Pden_id)
                    .HasColumnName("pden_id")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });
            modelBuilder.Entity<Form23>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("FORM23", "FOXT");

                entity.Property(e => e.Block)
                    .HasColumnName("block")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Form23id)
                    .HasColumnName("form23id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Formation)
                    .HasColumnName("formation")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Member)
                    .HasColumnName("member")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Oilfield)
                    .HasColumnName("oilfield")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Structure)
                    .HasColumnName("structure")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Operadorid)
                    .HasColumnName("operadorid")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Operador)
                    .HasColumnName("operador")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Contractid)
                    .HasColumnName("contractid")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Contract)
                    .HasColumnName("contract")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Campoid)
                    .HasColumnName("campoid")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Campo)
                    .HasColumnName("campo")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

            });

            modelBuilder.Entity<Form23Detail>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("FORM23DETAIL", "FOXT");

                entity.Property(e => e.Form23detailid)
                    .HasColumnName("form23detailid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Widays)
                    .HasColumnName("widays")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Wiaccumulateddays)
                    .HasColumnName("wiaccumulateddays")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Pressure)
                    .HasColumnName("pressure")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Widailywater)
                    .HasColumnName("widailywater")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Wiaccumulatedwater)
                    .HasColumnName("wiaccumulatedwater")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Wimonthlywater)
                    .HasColumnName("wimonthlywater")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Poolname)
                    .HasColumnName("poolname")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Injectionoilwellfinalstate)
                    .HasColumnName("injectionoilwellfinalstate")
                    .HasColumnType("numeric(12, 0)");

                entity.Property(e => e.Monthdays)
                    .HasColumnName("monthdays")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Accumulatedays)
                    .HasColumnName("accumulatedays")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Dailyoilproduction)
                    .HasColumnName("dailyoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Monthlyoilproduction)
                    .HasColumnName("monthlyoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Accumulateoilproduction)
                    .HasColumnName("accumulateoilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Dailywaterproduction)
                    .HasColumnName("dailywaterproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Monthlywaterproduction)
                    .HasColumnName("monthlywaterproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Accumulatewaterproduction)
                    .HasColumnName("accumulatewaterproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Dailygasproduction)
                    .HasColumnName("dailygasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Monthlygasproduction)
                    .HasColumnName("monthlygasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Accumulategasproduction)
                    .HasColumnName("accumulategasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Productionoilwellfinalstate)
                    .HasColumnName("productionoilwellfinalstate")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.PdenId)
                    .HasColumnName("pden_id")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.PressureProd)
                   .HasColumnName("pressure_prod")
                   .HasMaxLength(260)
                   .IsUnicode(false);

                entity.Property(e => e.productionmethodproduct)
                .HasColumnName("productionmethodproduct")
                .HasMaxLength(260)
                .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });
            modelBuilder.Entity<FormC7>(entity =>
            {
                entity.ToTable("FORMC7", "FOXT");

                entity.Property(e => e.Formc7id)
                    .HasColumnName("Formc7id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Formation)
                    .HasColumnName("formation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Block)
                    .HasColumnName("block")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Oilfield)
                    .HasColumnName("oilfield")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Structure)
                    .HasColumnName("structure")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Member)
                    .HasColumnName("member")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Operadorid)
                    .HasColumnName("operadorid")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Operador)
                    .HasColumnName("operador")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Contractid)
                    .HasColumnName("contractid")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Contract)
                    .HasColumnName("contract")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Campoid)
                    .HasColumnName("campoid")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.Campo)
                    .HasColumnName("campo")
                    .HasMaxLength(260)
                    .IsUnicode(false);


            });
            modelBuilder.Entity<FormC7Detail>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("FORMC7DETAIL", "FOXT");

                entity.Property(e => e.Formc7Detailid)
                    .HasColumnName("formc7id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Fieldid)
                    .HasColumnName("fieldid")
                    .HasColumnType("numeric(38, 0)");

                entity.Property(e => e.Oilproduction)
                    .HasColumnName("oilproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gasproduction)
                    .HasColumnName("gasproduction")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Endwells)
                    .HasColumnName("endwells")
                    .HasColumnType("numeric(5, 0)");

                entity.Property(e => e.Activewells)
                    .HasColumnName("activewells")
                    .HasColumnType("numeric(5, 0)");

                entity.Property(e => e.Inactivewells)
                    .HasColumnName("inactivewells")
                    .HasColumnType("numeric(5, 0)");

                entity.Property(e => e.Abandonedwells)
                    .HasColumnName("abandonedwells")
                    .HasColumnType("numeric(5, 0)");

                entity.Property(e => e.Injectorwells)
                    .HasColumnName("injectorwells")
                    .HasColumnType("numeric(5, 0)");

                entity.Property(e => e.Unfinishedwells)
                    .HasColumnName("unfinishedwells")
                    .HasColumnType("numeric(5, 0)");

                entity.Property(e => e.Oilproductionwell)
                    .HasColumnName("oilproductionwell")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Gasproductionwells)
                    .HasColumnName("gasproductionwells")
                    .HasColumnType("numeric(5, 0)");

                entity.Property(e => e.PdenId)
                    .HasColumnName("pden_id")
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.pozosProductoresActivosFlujoNatural)
                   .HasColumnName("pozosProductoresActivosFlujoNatural")
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.Property(e => e.pozosProductoresActivosLevantamientoArtificial)
                   .HasColumnName("pozosProductoresActivosLevantamientoArtificial")
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.Property(e => e.pozosProductoresInactivosCerradoAltaRelacionAguaPetroleo)
                 .HasColumnName("pozosProductoresInactivosCerradoAltaRelacionAguaPetroleo")
                 .HasMaxLength(100)
                 .IsUnicode(false);

                entity.Property(e => e.pozosProductoresInactivosCerradoTemporalmente)
                 .HasColumnName("pozosProductoresInactivosCerradoTemporalmente")
                 .HasMaxLength(100)
                 .IsUnicode(false);


                entity.Property(e => e.pozosTaponadosSecos)
                 .HasColumnName("pozosTaponadosSecos")
                 .HasMaxLength(100)
                 .IsUnicode(false);

                entity.Property(e => e.pozosSuspendidosTemporalmente)
                     .HasColumnName("pozosSuspendidosTemporalmente")
                     .HasMaxLength(100)
                     .IsUnicode(false);

                entity.Property(e => e.PozosSinTerminar)
                    .HasColumnName("pozosSinTerminar")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.pozosTaponadosInyectores)
                   .HasColumnName("pozosTaponadosInyectores")
                   .HasColumnType("DECIMAL(20, 4)");

                entity.Property(e => e.pozosTaponadosInyectoresAire)
                   .HasColumnName("pozosTaponadosInyectoresAire")
                   .HasColumnType("DECIMAL(20, 4)");

                entity.Property(e => e.pozosTaponadosInyectoresGas)
                   .HasColumnName("pozosTaponadosInyectoresGas")
                   .HasColumnType("DECIMAL(20, 4)");


            });

            modelBuilder.Entity<FormC7Total>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("FORMC7TOTAL", "FOXT");

                entity.Property(e => e.Formac7totalid)
                    .HasColumnName("formac7totalid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Formid).HasColumnName("formid");

                entity.Property(e => e.Productionbbls)
                    .HasColumnName("productionbbls")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Productionbbls)
                    .HasColumnName("productionbbls")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.ProductioninactiveoilWaterWell)
                    .HasColumnName("productioninactiveoilWaterWell")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Productioninactivewellclosedtemp)
                    .HasColumnName("productioninactivewellclosedtemp")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Productioninactivewellsmiscellaneous)
                    .HasColumnName("productioninactivewellsmiscellaneous")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Productiontotalwellsassets)
                    .HasColumnName("productiontotalwellsassets")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Injectorswellsplugged)
                    .HasColumnName("injectorswellsplugged")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Wellpluggeddry)
                    .HasColumnName("wellpluggeddry")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Wellspluggedabandoned)
                    .HasColumnName("wellspluggedabandoned")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Productionwellsassetflownatural)
                    .HasColumnName("productionwellsassetflownatural")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Totalofficiallycompletedwells)
                    .HasColumnName("totalofficiallycompletedwells")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Wellstemporarilysuspended)
                    .HasColumnName("wellstemporarilysuspended")
                    .HasColumnType("numeric(20, 4)");

                entity.Property(e => e.Wellssuspendeddryunfinished)
                    .HasColumnName("wellssuspendeddryunfinished")
                    .HasColumnType("numeric(20, 4)");

            });
            modelBuilder.Entity<Forma15CRTable>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("Forma15CR", "FOXT");

                entity.Property(e => e.form_id)
                .HasColumnName("form_id")
                .ValueGeneratedNever();

                entity.Property(e => e.id_detalle)
                .HasColumnName("id_detalle")
                .ValueGeneratedNever();

                entity.Property(e => e.compania_id)
                .HasColumnName("compania_id")
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

                entity.Property(e => e.compania)
                .HasColumnName("compania")
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

                entity.Property(e => e.concesion)
                .HasColumnName("concesion")
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

                entity.Property(e => e.contrato_id)
                .HasColumnName("contrato_id")
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

                entity.Property(e => e.contrato)
                .HasColumnName("contrato")
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

                entity.Property(e => e.campo_id)
                .HasColumnName("campo_id")
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

                entity.Property(e => e.campo)
                .HasColumnName("campo")
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

                entity.Property(e => e.mes)
                .HasColumnName("mes")
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

                entity.Property(e => e.anio)
                .HasColumnName("anio")
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);
                
                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);


            });
            modelBuilder.Entity<Forma15CRTableDetalle>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("Forma15CRDetalle", "FOXT");

                entity.Property(e => e.id_detalle)
                .HasColumnName("id_detalle")
                .ValueGeneratedNever();

                entity.Property(e => e.form_id)
                .HasColumnName("form_id")
                .ValueGeneratedNever();

                entity.Property(e => e.valInyecPozo)
                .HasColumnName("valInyecPozo")
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

                entity.Property(e => e.valInyecFormacionProductora)
                .HasColumnName("valInyecFormacionProductora")
                .HasColumnType("varchar(260)")
                .IsUnicode(false);


                entity.Property(e => e.valInyecMetodoProduccion)
                .HasColumnName("valInyecMetodoProduccion")
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

                entity.Property(e => e.valInyecPresionInyeccion)
                .HasColumnName("valInyecPresionInyeccion")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);

                entity.Property(e => e.valInyecCiclo)
                .HasColumnName("valInyecCiclo")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);

                entity.Property(e => e.valInyecDiasMes)
                .HasColumnName("valInyecDiasMes")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);

                entity.Property(e => e.valInyecDiasAcumulados)
                .HasColumnName("valInyecDiasAcumulados")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);

                entity.Property(e => e.valInyecLibrasMes)
                .HasColumnName("valInyecLibrasMes")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);

                entity.Property(e => e.valInyecLibrasAcumulados)
                .HasColumnName("valInyecLibrasAcumulados")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);

                entity.Property(e => e.valInyecBTUMes)
                .HasColumnName("valInyecBTUMes")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);

                entity.Property(e => e.valInyecBTUAcumulados)
                .HasColumnName("valInyecBTUAcumulados")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);


                entity.Property(e => e.valInyecCalidadVapor)
                .HasColumnName("valInyecCalidadVapor")
                .HasMaxLength(50)
                .IsUnicode(false);

                entity.Property(e => e.produccionPetroleoBlsNetosMensual)
                .HasColumnName("produccionPetroleoBlsNetosMensual")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);


                entity.Property(e => e.produccionPetroleoBlsNetosAcumulado)
                .HasColumnName("produccionPetroleoBlsNetosAcumulado")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);

                entity.Property(e => e.produccionAguaBlsMensual)
                .HasColumnName("produccionAguaBlsMensual")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);

                entity.Property(e => e.produccionAguaBlsAcumulado)
                .HasColumnName("produccionAguaBlsAcumulado")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);


                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.pden_id)
                .HasColumnName("pden_id")
                .HasColumnType("varchar(50)")
                .IsUnicode(false);
            });
            modelBuilder.Entity<Forma17CRTable>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("Forma17CR", "FOXT");

                entity.Property(e => e.form_id)
                .HasColumnName("form_id")
                .ValueGeneratedNever();

                entity.Property(e => e.id_detalle)
                .HasColumnName("id_detalle")
                .ValueGeneratedNever();

                entity.Property(e => e.concesion)
                .HasColumnName("concesion")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.operador_id)
                .HasColumnName("operador_id")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.operador)
                .HasColumnName("operador")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.contrato_id)
                .HasColumnName("contrato_id")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.contrato)
                .HasColumnName("contrato")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.campo_id)
                .HasColumnName("campo_id")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.campo)
                .HasColumnName("campo")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.estructura)
                .HasColumnName("estructura")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.formacion)
                .HasColumnName("formacion")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.bloque)
                .HasColumnName("bloque")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.yacimiento)
                .HasColumnName("yacimiento")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.mes)
                .HasColumnName("mes")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.anio)
                .HasColumnName("anio")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.fecha_creacion)
                .HasColumnName("fecha_creacion")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.usuario_crea)
                .HasColumnName("usuario_crea")
                .HasColumnType("varchar(110)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

            });
            modelBuilder.Entity<Forma17CRTableDetalle>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Forma17CRDetalle", "FOXT");

                entity.Property(e => e.id_detalle)
                .HasColumnName("id_detalle")
                .ValueGeneratedNever();

                entity.Property(e => e.form_id)
                .HasColumnName("form_id");

                entity.Property(e => e.pozo)
                .HasColumnName("pozo")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.diasEnElMes)
                .HasColumnName("diasEnElMes")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);


                entity.Property(e => e.diasAcumulados)
                .HasColumnName("diasAcumulados")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);

                entity.Property(e => e.produccionGasMCPDiaria)
                .HasColumnName("produccionGasMCPDiaria")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);

                entity.Property(e => e.produccionGasMCPMensual)
                .HasColumnName("produccionGasMCPMensual")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);

                entity.Property(e => e.produccionGasMCPAcumulada)
                .HasColumnName("produccionGasMCPAcumulada")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);

                entity.Property(e => e.produccionAguaAcumulada)
                .HasColumnName("produccionAguaAcumulada")
                .HasColumnType("decimal(20,2)")
                .IsUnicode(false);

                entity.Property(e => e.estadoPozosFinalMes)
                .HasColumnName("estadoPozosFinalMes")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.fecha_creacion)
                .HasColumnName("fecha_creacion")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.usuario_creacion)
                .HasColumnName("usuario_creacion")
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

                entity.Property(e => e.pden_id)
                    .HasColumnName("pdenId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });

            modelBuilder.Entity<Forma22cr>(entity =>
            {
                entity.HasKey(e=> new { e.FormId});

                entity.ToTable("FORMA22CR", "FOXT");

                entity.Property(e => e.Anio)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("anio");

                entity.Property(e => e.Bloque)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bloque");

                entity.Property(e => e.Campo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("campo");

                entity.Property(e => e.CampoId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("campo_id");

                entity.Property(e => e.Compania)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("compania");

                entity.Property(e => e.CompaniaId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("compania_id");

                entity.Property(e => e.Contrato)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("contrato");

                entity.Property(e => e.ContratoId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("contrato_id");

                entity.Property(e => e.Estructura)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("estructura");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_CREACION");

                entity.Property(e => e.FormId).HasColumnName("form_id");

                entity.Property(e => e.Formacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("formacion");

                entity.Property(e => e.Mes)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mes");

                entity.Property(e => e.PdenId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pden_id");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO_CREACION");

                entity.Property(e => e.Yacimiento)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("yacimiento");

                entity.Property(e => e.YacimientoId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("yacimiento_id");

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });

            modelBuilder.Entity<Forma22crdetalle>(entity =>
            {
                entity.HasKey(e => new { e.FormId,e.IdCabecera });

                entity.ToTable("FORMA22CRDETALLE", "FOXT");

                entity.Property(e => e.AguaInyectadoAcumulado)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("aguaInyectadoAcumulado");

                entity.Property(e => e.AguaInyectadoMensual)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("aguaInyectadoMensual");

                entity.Property(e => e.AguaProducidoAcumulado)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("aguaProducidoAcumulado");

                entity.Property(e => e.AguaProducidoMensual)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("aguaProducidoMensual");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creacion");

                entity.Property(e => e.FormId).HasColumnName("form_id");

                entity.Property(e => e.GasInyectadoAcumulado)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("gasInyectadoAcumulado");

                entity.Property(e => e.GasInyectadoMensual)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("gasInyectadoMensual");

                entity.Property(e => e.GasProducidoAcumulado)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("gasProducidoAcumulado");

                entity.Property(e => e.GasProducidoMensual)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("gasProducidoMensual");

                entity.Property(e => e.IdCabecera).HasColumnName("id_cabecera");

                entity.Property(e => e.Mes)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("mes");

                entity.Property(e => e.PdenId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pdenId");

                entity.Property(e => e.PetroleoProducidoAcumulado)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("petroleoProducidoAcumulado");

                entity.Property(e => e.PetroleoProducidoMensual)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("petroleoProducidoMensual");

                entity.Property(e => e.Pozo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PresionFondo)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("presionFondo");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usuario_creacion");

                entity.Property(e => e.row_created_by)
                .HasColumnName("row_created_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_created_date)
                .HasColumnName("row_created_date")
                .HasColumnType("datetime")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_by)
                .HasColumnName("row_changed_by")
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

                entity.Property(e => e.row_changed_date)
                .HasColumnName("row_changed_date")
                .HasColumnType("datetime")
                .IsUnicode(false);
            });
        }
    }
}