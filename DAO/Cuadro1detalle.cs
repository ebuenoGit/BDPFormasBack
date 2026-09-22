using System;

namespace Backend.Formas.Entities.DAO
{
    public partial class Cuadro1detalle
    {
        public Guid IdCabecera { get; set; }
        public Guid FormaId { get; set; }
        public string Dias { get; set; }
        public decimal? MedidaMm { get; set; }
        public decimal? AforoBls { get; set; }
        public decimal? TempF { get; set; }
        public decimal? FactorTemp { get; set; }
        public decimal? Bls60f { get; set; }
        public decimal? Bsw { get; set; }
        public decimal? FactorBsw { get; set; }
        public decimal? Ctsh { get; set; }
        public decimal? TempAmb { get; set; }
        public decimal? Blsnetos { get; set; }
        public decimal? TransfBls { get; set; }
        public decimal? RecibidoBls { get; set; }
        public decimal? EntregaBls { get; set; }
        public decimal? Api60f { get; set; }
        public decimal? Ge { get; set; }
        public decimal? NetosGe { get; set; }
        public decimal? SalBtb { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCrea { get; set; }
        public string PdenId { set; get; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
        public decimal? TrasRecibido { set; get; }
        public decimal? TrasEnvio { set; get; }
        public decimal? MovIntraRecibido { set; get; }
        public decimal? MovIntraEnvio { set; get; }
    }
}
