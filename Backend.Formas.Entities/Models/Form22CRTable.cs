using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Formas.Entities.Models
{
    public class Form22CRTable
    {
        [Key]
        public Guid form_id { get; set; }
        public string compania_id { get; set; }
        public string compania { get; set; }
        public string formacion { get; set; }
        public string contrato_id { get; set; }
        public string contrato { get; set; }
        public string bloque { get; set; }
        public string campo_id { get; set; }
        public string campo { get; set; }
        public string yacimiento { get; set; }
        public string yacimiento_id { get; set; }
        public string estructura { get; set; }
        public string mes { get; set; }
        public string anio { get; set; }
        public string pden_id { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public string USUARIO_CREACION { get; set; }
    }



    public class Form22CRTableDetalle
    {
        public Guid id_cabecera { get; set; }
        public Guid form_id { get; set; }
        public string pdenId { set; get; }
        public string mes { get; set; }
        public decimal petroleoProducidoMensual { get; set; }
        public decimal petroleoProducidoAcumulado { get; set; }
        public decimal aguaInyectadoMensual { get; set; }
        public decimal aguaInyectadoAcumulado { get; set; }
        public decimal aguaProducidoMensual { get; set; }
        public decimal aguaProducidoAcumulado { get; set; }
        public decimal gasInyectadoMensual { get; set; }
        public decimal gasInyectadoAcumulado { get; set; }
        public decimal gasProducidoMensual { get; set; }
        public decimal gasProducidoAcumulado { get; set; }
        public decimal presionFondo { get; set; }
        public DateTime fecha_creacion { get; set; }
        public string usuario_creacion { get; set; }
    }


    public partial class Forma22cr
    {
        public Guid FormId { get; set; }
        public string CompaniaId { get; set; }
        public string Compania { get; set; }
        public string Formacion { get; set; }
        public string ContratoId { get; set; }
        public string Contrato { get; set; }
        public string Bloque { get; set; }
        public string CampoId { get; set; }
        public string Campo { get; set; }
        public string Yacimiento { get; set; }
        public string Estructura { get; set; }
        public string Mes { get; set; }
        public string Anio { get; set; }
        public string PdenId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string YacimientoId { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }


    public partial class Forma22crdetalle
    {
        public Guid IdCabecera { get; set; }
        public Guid FormId { get; set; }
        public string Mes { get; set; }
        public decimal? PetroleoProducidoMensual { get; set; }
        public decimal? PetroleoProducidoAcumulado { get; set; }
        public decimal? AguaInyectadoMensual { get; set; }
        public decimal? AguaInyectadoAcumulado { get; set; }
        public decimal? AguaProducidoMensual { get; set; }
        public decimal? AguaProducidoAcumulado { get; set; }
        public decimal? GasInyectadoMensual { get; set; }
        public decimal? GasInyectadoAcumulado { get; set; }
        public decimal? GasProducidoMensual { get; set; }
        public decimal? GasProducidoAcumulado { get; set; }
        public decimal? PresionFondo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string PdenId { get; set; }
        public string Pozo { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }

}
