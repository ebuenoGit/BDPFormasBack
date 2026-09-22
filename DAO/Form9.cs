using System;

namespace Backend.Formas.Entities.DAO
{
    public class Form9
    {
        public Guid Form9id { get; set; }
        public string Block { get; set; }
        public string Oilfield { get; set; }
        public string Structure { get; set; }
        public string Member { get; set; }
        public string operadorId { get; set; }
        public string campoId { get; set; }
        public string bloqueId { get; set; }
        public string formacionId { get; set; }
        public string formacionSetId { get; set; }
        public string yacimientoId { get; set; }
        public string campo { set; get; }
        public string bloque { set; get; }
        public string formacion { set; get; }
        public string yacimiento { set; get; }
        public string contratoId { set; get; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }

    }
}