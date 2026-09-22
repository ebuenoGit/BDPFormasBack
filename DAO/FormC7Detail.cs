using System;

namespace Backend.Formas.Entities.DAO
{
    public class FormC7Detail
    {
        public Guid Formc7Detailid { get; set; }
        public Guid? Formid { get; set; }
        public decimal? Fieldid { get; set; }
        public decimal? Oilproduction { get; set; }
        public decimal? Gasproduction { get; set; }
        public decimal? Endwells { get; set; }
        public decimal? Activewells { get; set; }
        public decimal? Inactivewells { get; set; }
        public decimal? Abandonedwells { get; set; }
        public decimal? Unfinishedwells { get; set; }
        public decimal? Injectorwells { get; set; }
        public decimal? Oilproductionwell { get; set; }
        public decimal? Gasproductionwells { get; set; }
        public string PdenId { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }

        public string pozosProductoresActivosLevantamientoArtificial { set; get; }
        public string pozosProductoresActivosFlujoNatural { set; get; }
        public string pozosProductoresInactivosCerradoAltaRelacionAguaPetroleo { set; get; }
        public string pozosProductoresInactivosCerradoTemporalmente { set; get; }
        public string pozosTaponadosSecos { set; get; }
        public string pozosSuspendidosTemporalmente { set; get; }
        public string PozosSinTerminar { set; get; }

        public decimal pozosTaponadosInyectores { set; get; }
        public decimal pozosTaponadosInyectoresGas { set; get; }
        public decimal pozosTaponadosInyectoresAire { set; get; }
    }
}
