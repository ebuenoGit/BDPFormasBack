using System;

namespace Backend.Formas.Entities.DAO
{
    public partial class Form20
    {
        public Guid Form20id { get; set; }
        public string Formation { get; set; }
        public string Block { get; set; }
        public string Oilfield { get; set; }
        public string Structure { get; set; }
        public string Member { get; set; }
        public string operador { set; get; }
        public string operadorId { set; get; }
        public string campo { set; get; }
        public string campoId { set; get; }
        public string contrato { set; get; }
        public string contratoId { set; get; }
        public string formacion_id { set; get; }
        public string formacion_set_id { set; get; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }

    }
}
