using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Formas.Entities
{
    public class ReporteForma<T>
    {
        public RegistroClass<T> root { set; get; }
    }
    public class RegistroClass<T>
    {
        public List<T> Registro { set; get; } 
    }

    public class ReporteFormaJson<T>
    {
        public RegistroClassJson<T> root { set; get; }
    }
    public class RegistroClassJson<T>
    {
        public T Registro { set; get; }
    }

    public class DetalleReporte
    {
        public string FORMA_ID{set;get;}
                public string FORMA_CODIGO{set;get;} 
                public string MES{set;get;} 
                public string ANIO{set;get;} 
                public string OPERADOR_ID{set;get;} 
                public string OPERADOR{set;get;} 
                public string CONTRATO_ID{set;get;}
                public string CONTRATO{set;get;} 
                public string CAMPO_ID{set;get;} 
                public string CAMPO{set;get;}
                public string BATERIA_ID{set;get;}
                public string BATERIA{set;get;} 
                public string TANQUE_ID{set;get;}
                public string TANQUE{set;get;} 
                public string ESTRUCTURA_ID{set;get;}
                public string ESTRUCTURA{set;get;} 
                public string BLOQUE_ID{set;get;} 
                public string BLOQUE{set;get;} 
                public string FORMACION_ID{set;get;}
                public string FORMACION_SET_ID{set;get;} 
                public string FORMACION{set;get;}
                public string MIEMBRO_ID{set;get;} 
                public string MIEMBRO{set;get;} 
                public string YACIMIENTO_ID{set;get;}
                public string YACIMIENTO{set;get;} 
                public string MODALIDADEXPLOTACION_ID{set;get;} 
                public string MODALIDADEXPLOTACION{set;get;} 
                public string REPRESENTANTE_OPERADOR_NM{set;get;}
                public string REPRESENTANTE_OPERADOR_TP{set;get;}
                public string REPRESENTANTE_ANH_NAME{set;get;} 
                public string REPRESENTANTE_ANH_TP{set;get;} 
                public string GENERADO_DESDE{set;get;} 
                public string ROW_CHANGED_BY{set;get;} 
                public string ROW_CHANGED_DATE{set;get;} 
                public string ROW_CREATED_BY{set;get;} 
                public string ROW_CREATED_DATE{set;get;} 
                public string OBSERVACIONES{set;get;} 

    }
    public class DetalleReporteForma
    {
       public string pden_land_right{set;get;}
       public string volume_date{set;get;}
                public string inv_inicial{set;get;}
               public string inv_final{set;get;}
                public string entregas{set;get;}
                public string vol_muerto_vasijas{set;get;}
                public string vol_muerto_lineas{set;get;}
                public string perdidas_gravables{set;get;}
                public string perdidas{set;get;}
                public string consumos{set;get;}
                public string prod_gravable{set;get;}
                public string produccion{set;get;}
                public string api{set;get;}
                public string bsw{set;get;}
                public string sulphur{set;get;}

    }
}
