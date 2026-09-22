using System.Collections.Generic;

namespace Backend.Formas.Entities.Models
{
    public class ParametrosConsultaFormas
    {
        public string FORMA_CODIGO { get; set; }  
        public List<DetalleForma> DETALLE { get; set; }
        
    }
    public class DetalleForma {
        public int I_FORMA { get; set; }
        public int I_ANNO { get; set; }
        public int I_MES { get; set; }
        public string I_ID_OPERADOR { get; set; } = null;
        public string I_ID_CAMPO { get; set; } = null;          // MCG 1-04-2022  Mejora para identificar el Nombre del campo asociado a la Forma Oficial
        public string I_ID_USUARIO { get; set; } = null;             // MCG 1-19-2022  Mejora para identificar el logueado a la Forma
        public string I_ID_CONTRATO { get; set; } = null;      // MCG 1-19-2022  Mejora para identificar el Contrato de la forma
       
    }
}
