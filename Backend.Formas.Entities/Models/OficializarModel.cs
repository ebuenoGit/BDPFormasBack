using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Formas.Entities.Models
{
    public class OficializarModel
    {
        public string Anno { set; get; }
        public string Mes { set; get; }
        public string Forma { set; get; }
        public string Campo { set; get; }

    }

    public class ConsultaContratoOperativos
    {
       
        public DateTime FechaOperativa { set; get; }
        public string Forma { set; get; }

    }

    public class RootOficializar
    {
        public string root { set; get; }

    }
}
