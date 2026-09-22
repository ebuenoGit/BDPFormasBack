using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Formas.Entities.DAO
{
    public class PeticionJsonReporte
    {
        public string FORMA_CODIGO { set; get; }
        public List<jsonDetalle> DETALLE { set; get; }
    }
    public class jsonDetalle
    {
        public string I_FORMA { set; get; }
        public string I_ANNO { set; get; }
        public string I_MES { set; get; }
        public string I_ID_OPERADOR { set; get; }
        public string I_ID_CAMPO { set; get; }   // MCG 1-04-2022  Mejora para identificar el Nombre del campo asociado a la Forma Oficial
        public string I_ID_USUARIO { get; set; } = null;             // MCG 1-20-2022  Mejora para identificar el logueado a la Forma
        public string I_ID_CONTRATO { set; get; }              // MCG 1-19-2022  Mejora para identificar el Contrato de la forma 

    }
}

