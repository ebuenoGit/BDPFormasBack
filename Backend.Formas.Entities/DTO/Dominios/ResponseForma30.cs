using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class ResponseForma30
    {
        public Forma30Header Header { set; get; }
        public List<Forma30Body> Body { set; get; }
        public Forma30BodyTotal BodyTotal { set; get; }
        public Forma30GasProcesado GasProcesado { set; get; }
        public ExcelError Errors { set; get; }
    }
}