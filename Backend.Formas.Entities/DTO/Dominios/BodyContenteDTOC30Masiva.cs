using Backend.Formas.Entities.DAO;
using System.Collections.Generic;
using Backend.Formas.Entities.DTO.Validaciones;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class BodyContenteDTOC30Masiva
    {
        public string Workbook { set; get; }
        public HeaderFormaC30MasivaDTO Header { set; get; }
        public List<DetalleJsonC30Masiva> RespBDP { set; get; }
    }

    public class JsonC30Masivo
    {
        public string FORMA_CODIGO { set; get; }
        public string ANIO { set; get; }
        public string MES { set; get; }
        public List<DetalleJsonC30Masiva> REGISTRO { set; get; }
    }


    public class DetalleJsonC30Masiva
    {
        //ANIO MES     OPERADOR CONTRATO
        //CAMPO PDEN_ID
        //GASFORMACION CONTENIDOPROPANO    CONTENIDOBUTANO
        //CONTENIDOGASOLINANATURAL    GASFORMACIONPROCESADO CONSUMOCAMPO    GENERACIONELECTRICA OTROSCONSUMOS
        //GASQUEMACAMPO VENTEADOALAIRE GASBOMBEONEUMATICO  GASINYECTADO TOTALGASPROCESADO   PRODUCCIONPROPANO PRODUCCIONBUTANO    PRODUCCIONGASOLINA
        //GASTRANSFORMADO CONSUMOENPLANTA ENTREGAGASODUCTO    ENTREGAGENERACION OTRASENTREGAS
        //GASQUEMAPLANTA GASBOMBEOPLANTA GASINYECTADOPLANTA OBSERVACIONES   ELIMINAR

        public string ANIO { set; get; }
        public string MES { set; get; }
        public string OPERADOR { set; get; }
        public string CONTRATO { set; get; }
        public string CAMPO { set; get; }
        public string PDEN_ID { set; get; }
        public string GASFORMACION { set; get; }
        public string CONTENIDOPROPANO { set; get; }
        public string CONTENIDOBUTANO { set; get; }


        public string CONTENIDOGASOLINANATURAL { set; get; }
        public string GASFORMACIONPROCESADO { set; get; }
        public string CONSUMOCAMPO { set; get; }
        public string GASODUCTOURBANO { set; get; }
        
        public string GENERACIONELECTRICA { set; get; }
        public string OTROSCONSUMOS { set; get; }

        public string GASQUEMACAMPO { set; get; }
        public string VENTEADOALAIRE { set; get; } //INDRA - 15/NOV/2023 - DMRICO - RF475973 Ajuste en el cargue de Forma30 de la asociada Incluir VENTEO
        public string GASBOMBEONEUMATICO { set; get; }
        public string GASINYECTADO { set; get; }
        public string TOTALGASPROCESADO { set; get; }

        public string PRODUCCIONPROPANO { set; get; }
        public string PRODUCCIONBUTANO { set; get; }
        public string PRODUCCIONGASOLINA { set; get; }

        public string GASTRANSFORMADO { set; get; }
        public string CONSUMOENPLANTA { set; get; }
        public string ENTREGAGASODUCTO { set; get; }
        public string ENTREGAGENERACION { set; get; }
        public string OTRASENTREGAS { set; get; }

        public string GASQUEMAPLANTA { set; get; }
        public string GASBOMBEOPLANTA { set; get; }
        public string GASINYECTADOPLANTA { set; get; }
        public string OBSERVACIONES { set; get; }

        public bool APROBADO { set; get; }
        public string USERID { set; get; }
        
        public bool ELIMINAR { set; get; }
        public IList<REG_MENSAJE> REG_MENSAJES { get; set; }

    }


    //ANIO MES     OPERADOR CONTRATO
    //CAMPO PDEN_ID
    //GASFORMACION CONTENIDOPROPANO    CONTENIDOBUTANO
    //CONTENIDOGASOLINANATURAL    GASFORMACIONPROCESADO CONSUMOCAMPO    GENERACIONELECTRICA OTROSCONSUMOS
    //GASQUEMACAMPO VENTEADOALAIRE GASBOMBEONEUMATICO  GASINYECTADO TOTALGASPROCESADO   PRODUCCIONPROPANO PRODUCCIONBUTANO    PRODUCCIONGASOLINA
    //GASTRANSFORMADO CONSUMOENPLANTA ENTREGAGASODUCTO    ENTREGAGENERACION OTRASENTREGAS
    //GASQUEMAPLANTA GASBOMBEOPLANTA GASINYECTADOPLANTA OBSERVACIONES   ELIMINAR
    public class JsonC30MSQL
    {
        public string Anio { set; get; }
        public string Mes { set; get; }
        public string Operdor { set; get; }

        public string Contrato { set; get; }
        public string Campo { set; get; }
        public string Pden_Id { set; get; }
        public string GasFormacion { set; get; }
        public string ContenidoPropano { set; get; }
        public string ContenidoButano{ set; get; }

        public string ContenidoGasolinaNatural { set; get; }
        public string GasFormacionProcesado{ set; get; }
        public string ConsumoCampo { set; get; }
        public string GasoductoUrbano { set; get; }

        public string GeneracionElectrica { set; get; }
        public string OtrosConsumos { set; get; }

        public string GasQuemaCampo { set; get; }
        public string VenteadoAlAire { set; get; }//INDRA - 15/NOV/2023 - DMRICO - RF475973 Ajuste en el cargue de Forma30 de la asociada Incluir VENTEO
        public string GasBombeoNeumatico { set; get; }
        public string GasInyectado { set; get; }
        public string TotalGasProcesado { set; get; }

        public string ProduccionPropano { set; get; }
        public string ProduccionButano { set; get; }
        public string ProduccionGasolina { set; get; }

        public string GasTransformado { set; get; }
        public string ConsumoenPlanta { set; get; }
        public string EntregaGasoducto { set; get; }
        public string EntregaGeneracion { set; get; }
        public string OtrasEntregas { set; get; }

        public string GasQuemaPlanta { set; get; }
        public string GasBombeoPlanta { set; get; }
        public string GasInyectadoPlanta { set; get; }
        public string Observaciones { set; get; }

        public string Aprobado { set; get; }
        public string CargadoaBDP { set; get; }
        public string row_created_by { set; get; }
        public string row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public string row_changed_date { set; get; }
        public string Json_MensajesErr { set; get; }
        public string Eliminar { set; get; }

    }

    public class JsonC30MBDP
    {
        public string ANIO { set; get; }
        public string MES { set; get; }
        public string OPERADOR { set; get; }
        public string CONTRATO { set; get; }
        public string CAMPO { set; get; }
        public string PDEN_ID { set; get; }
        public string GASFORMACION { set; get; }
        public string CONTENIDOPROPANO { set; get; }
        public string CONTENIDOBUTANO { set; get; }
        public string CONTENIDOGASOLINANATURAL { set; get; }
        public string GASFORMACIONPROCESADO { set; get; }
        public string CONSUMOCAMPO { set; get; }
        public string GASODUCTOURBANO { set; get; }
        public string GENERACIONELECTRICA { set; get; }
        public string OTROSCONSUMOS { set; get; }
        public string GASQUEMACAMPO { set; get; }
        public string VENTEADOALAIRE { set; get; } //INDRA - 15/NOV/2023 - DMRICO - RF475973 Ajuste en el cargue de Forma30 de la asociada Incluir VENTEO
        public string GASBOMBEONEUMATICO { set; get; }
        public string GASINYECTADO { set; get; }
        public string TOTALGASPROCESADO { set; get; }
        public string PRODUCCIONPROPANO { set; get; }
        public string PRODUCCIONBUTANO { set; get; }
        public string PRODUCCIONGASOLINA { set; get; }
        public string GASTRANSFORMADO { set; get; }
        public string CONSUMOENPLANTA { set; get; }
        public string ENTREGAGASODUCTO { set; get; }
        public string ENTREGAGENERACION { set; get; }
        public string OTRASENTREGAS { set; get; }
        public string GASQUEMAPLANTA { set; get; }
        public string GASBOMBEOPLANTA { set; get; }
        public string GASINYECTADOPLANTA { set; get; }
        public string OBSERVACIONES { set; get; }
        public string APROBADO { set; get; }
        public string CARGADOABDP { set; get; }
        public string ROW_CREATED_BY { set; get; }
        public string ROW_CREATED_DATE { set; get; }
        public string ROW_CHANGED_BY { set; get; }
        public string ROW_CHANGED_DATE { set; get; }
        public string JSON_MENSAJESERR { set; get; }
        public string USERID { set; get; }
        public string ELIMINAR { set; get; }
    }
}