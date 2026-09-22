using Backend.Formas.Entities.DAO;
using System.Collections.Generic;
using Backend.Formas.Entities.DTO.Validaciones;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class BodyContenteDTOC4Masiva
    {
        public string Workbook { set; get; }
        public HeaderFormaC4MasivaDTO Header { set; get; }
        public List<DetalleJsonC4Masiva> RespBDP { set; get; }
        public List<DetalleJsonC4Municipio> RespBDPMuni { set; get; }
    }
    public class BodyContenteDTOC4Municipio
    {
        public string Workbook { set; get; }
        public HeaderFormaC4MasivaDTO Header { set; get; }
        public List<DetalleJsonC4Municipio> RespBDP { set; get; }
    }
    public class JsonC4Masivo
    {
        public string FORMA_CODIGO { set; get; }
        public string ANIO { set; get; }
        public string MES { set; get; }
        public List<DetalleJsonC4Masiva> REGISTRO { set; get; }
    }

    public class JsonC4Municipio
    {
        public string FORMA_CODIGO { set; get; }
        public string ANIO { set; get; }
        public string MES { set; get; }
        public List<DetalleJsonC4Municipio> REGISTRO { set; get; }
    }

    public class DetalleJsonC4Masiva
    {
        // ANIO	MES	PDEN_ID	PDEN_NAME	ID_CAMPO	CAMPO	CONTRATO	MODALIDAD	MUNICIPIO	
        // PRODUCCIONBASICA	PRODUCCIONINCREMENTAL	PRODUCCIONTOTAL	CONSUMOSBASICA	
        // CONSUMOSINCREMENTAL	CONSUMOSTOTAL	PERDIDASBASICA	PERDIDASINCREMENTAL	
        // PERDIDASTOTAL	GRAVABLEBASICA	GRAVABLEINCREMENTAL	GRAVABLETOTAL	
        // ENTREGAS	EXISTENCIAFINAL	LLENADOLINEAS	LLENADOVASIJAS	OTRASPERDIDAS	
        // OTROSCONSUMOS	GRAVEDADAPI	CONTENIDOAZUFRE	BSW 	
        // GRAVEDADESPECIFICA	CONTENIDOSAL	ELIMINAR
        public string ANIO { set; get; }
        public string MES { set; get; }
        public string PDEN_ID { set; get; }
        public string PDEN_NAME { set; get; }
        public string ID_CAMPO { set; get; }
        public string CAMPO { set; get; }
        public string CONTRATO { set; get; }
        public string MODALIDAD { set; get; }
        public string EXISTENCIAINICIAL { set;  get; }
        public string MUNICIPIO { set; get; }
        public string PRODUCCIONBASICA { set; get; }
        public string PRODUCCIONINCREMENTAL { set; get; }
        public string PRODUCCIONTOTAL { set; get; }
        public string CONSUMOSBASICA { set; get; }
        public string CONSUMOSINCREMENTAL { set; get; }
        public string CONSUMOSTOTAL { set; get; }
        public string PERDIDASBASICA { set; get; }
        public string PERDIDASINCREMENTAL { set; get; }
        public string PERDIDASTOTAL { set; get; }
        public string GRAVABLEBASICA { set; get; }
        public string GRAVABLEINCREMENTAL { set; get; }
        public string GRAVABLETOTAL { set; get; }
        public string OTRASENTRADAS { set; get; }
        public string ENTREGAS { set; get; }
        public string EXISTENCIAFINAL { set; get; }
        public string LLENADOLINEAS { set; get; }
        public string LLENADOVASIJAS { set; get; }
        public string OTRASPERDIDAS { set; get; }
        public string OTROSCONSUMOS { set; get; }
        public string GRAVEDADAPI { set; get; }
        public string CONTENIDOAZUFRE { set; get; }
        public string BSW { set; get; }
        public string GRAVEDADESPECIFICA { set; get; }
        public string CONTENIDOSAL { set; get; }
        public bool APROBADO { set; get; }
        public string USERID { set; get; }
        public string OPERADOR { set; get; }
        public bool ELIMINAR { set; get; }
        public IList<REG_MENSAJE> REG_MENSAJES { get; set; }

    }

    public class DetalleJsonC4Municipio
    {
        // ANIO	MES	PDEN_ID	PDEN_NAME	ID_CAMPO	CAMPO	CONTRATO	MODALIDAD	MUNICIPIO	
        // PRODUCCIONBASICA	PRODUCCIONINCREMENTAL	PRODUCCIONTOTAL	CONSUMOSBASICA	
        // CONSUMOSINCREMENTAL	CONSUMOSTOTAL	PERDIDASBASICA	PERDIDASINCREMENTAL	
        // PERDIDASTOTAL	GRAVABLEBASICA	GRAVABLEINCREMENTAL	GRAVABLETOTAL	
	    // ELIMINAR
        public string ANIO { set; get; }
        public string MES { set; get; }
        public string PDEN_ID { set; get; }
        public string PDEN_NAME { set; get; }
        public string ID_CAMPO { set; get; }
        public string CAMPO { set; get; }
        public string CONTRATO { set; get; }
        public string MUNICIPIO { set; get; }
        public string PRODUCCIONBASICA { set; get; }
        public string PRODUCCIONINCREMENTAL { set; get; }
        public string PRODUCCIONTOTAL { set; get; }
        public string CONSUMOSBASICA { set; get; }
        public string CONSUMOSINCREMENTAL { set; get; }
        public string CONSUMOSTOTAL { set; get; }
        public string PERDIDASBASICA { set; get; }
        public string PERDIDASINCREMENTAL { set; get; }
        public string PERDIDASTOTAL { set; get; }
        public string GRAVABLEBASICA { set; get; }
        public string GRAVABLEINCREMENTAL { set; get; }
        public string GRAVABLETOTAL { set; get; }
        public bool APROBADO { set; get; }
        public string USERID { set; get; }
        public bool ELIMINAR { set; get; }
        public IList<REG_MENSAJE> REG_MENSAJES { get; set; }
        public string PDEN_XREF { get; set; }

    }
    public class JsonC4MSQL
    {
        public string Anio { set; get; }
        public string Mes { set; get; }
        public string Pden_Id { set; get; }
        public string Pden_Name { set; get; }
        public string Id_Campo { set; get; }
        public string Campo { set; get; }
        public string Contrato { set; get; }
        public string Modalidad { set; get; }
        public string ExistenciaInicial { set; get; }
        public string ProduccionBasica { set; get; }    
        public string ProduccionIncremental { set; get; }
        public string ProduccionTotal { set; get; }
        public string ConsumosBasica { set; get; }
        public string ConsumosIncremental { set; get; }
        public string ConsumosTotal { set; get; }
        public string PerdidasBasica { set; get; }
        public string PerdidasIncremental { set; get; }
        public string PerdidasTotal { set; get; }
        public string GravableBasica { set; get; }
        public string GravableIncremental { set; get; }
        public string GravableTotal { set; get; }
        public string OtrasEntradas { set; get; }
        public string Entregas { set; get; }
        public string ExistenciaFinal { set; get; }
        public string LlenadoLineas { set; get; }
        public string LlenadoVasijas { set; get; }
        public string OtrasPerdidas { set; get; }
        public string OtrosConsumos { set; get; }
        public string GravedadApi { set; get; }
        public string ContenidoAzufre { set; get; }
        public string BSW { set; get; }
        public string GravedadEspecifica { set; get; }
        public string ContenidoSal { set; get; }
        public string Aprobado { set; get; }
        public string CargadoaBDP { set; get; }
        public string row_created_by { set; get; }
        public string row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public string row_changed_date { set; get; }
        public string Json_MensajesErr { set; get; }
        public string Eliminar { set; get; }
        public string Operador { set; get; }
    }

    public class JsonC4MBDP
    {
        public string ANIO { set; get; }
        public string MES { set; get; }
        public string PDEN_ID { set; get; }
        public string PDEN_NAME { set; get; }
        public string ID_CAMPO { set; get; }
        public string CAMPO { set; get; }
        public string CONTRATO { set; get; }
        public string MODALIDAD { set; get; }
        public string PRODUCCIONBASICA{ set; get; }
        public string PRODUCCIONINCREMENTAL { set; get; }
        public string PRODUCCIONTOTAL { set; get; }
        public string CONSUMOSBASICA { set; get; }
        public string CONSUMOSINCREMENTAL { set; get; }
        public string CONSUMOSTOTAL { set; get; }
        public string PERDIDASBASICA { set; get; }
        public string PERDIDASINCREMENTAL { set; get; }
        public string PERDIDASTOTAL { set; get; }
        public string GRAVABLEBASICA { set; get; }
        public string GRAVABLEINCREMENTAL { set; get; }
        public string GRAVABLETOTAL { set; get; }
        public string OTRASENTRADAS { set; get; }
        public string ENTREGAS { set; get; }
        public string EXISTENCIAFINAL { set; get; }
        public string LLENADOLINEAS { set; get; }
        public string LLENADOVASIJAS { set; get; }
        public string OTRASPERDIDAS { set; get; }
        public string OTROSCONSUMOS { set; get; }
        public string GRAVEDADAPI { set; get; }
        public string CONTENIDOAZUFRE { set; get; }
        public string BSW { set; get; }
        public string GRAVEDADESPECIFICA { set; get; }
        public string CONTENIDOSAL { set; get; }
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

    public class JsonC4MSQLMuni
    {
        public string Anio { set; get; }
        public string Mes { set; get; }
        public string Pden_Id { set; get; }
        public string Pden_Name { set; get; }
        public string Id_Campo { set; get; }
        public string Campo { set; get; }
        public string Contrato { set; get; }
        public string Monicipio { set; get; }
        public string ProduccionBasica { set; get; }
        public string ProduccionIncremental { set; get; }
        public string ProduccionTotal { set; get; }
        public string ConsumosBasica { set; get; }
        public string ConsumosIncremental { set; get; }
        public string ConsumosTotal { set; get; }
        public string PerdidasBasica { set; get; }
        public string PerdidasIncremental { set; get; }
        public string PerdidasTotal { set; get; }
        public string GravableBasica { set; get; }
        public string GravableIncremental { set; get; }
        public string GravableTotal { set; get; }
        public string Aprobado { set; get; }
        public string CargadoaBDP { set; get; }
        public string row_created_by { set; get; }
        public string row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public string row_changed_date { set; get; }
        public string Json_MensajesErr { set; get; }
        public string Eliminar { set; get; }
    }

    public class JsonC4MBDPMuni
    {
        public string ANIO { set; get; }
        public string MES { set; get; }
        public string PDEN_ID { set; get; }
        public string PDEN_NAME { set; get; }
        public string ID_CAMPO { set; get; }
        public string CAMPO { set; get; }
        public string CONTRATO { set; get; }
        public string MUNICIPIO { set; get; }
        public string PRODUCCIONBASICA { set; get; }
        public string PRODUCCIONINCREMENTAL { set; get; }
        public string PRODUCCIONTOTAL { set; get; }
        public string CONSUMOSBASICA { set; get; }
        public string CONSUMOSINCREMENTAL { set; get; }
        public string CONSUMOSTOTAL { set; get; }
        public string PERDIDASBASICA { set; get; }
        public string PERDIDASINCREMENTAL { set; get; }
        public string PERDIDASTOTAL { set; get; }
        public string GRAVABLEBASICA { set; get; }
        public string GRAVABLEINCREMENTAL { set; get; }
        public string GRAVABLETOTAL { set; get; }
        public string APROBADO { set; get; }
        public string PDEN_XREF { set; get; }
        public string JSON_MENSAJESERR { set; get; }
        public string USERID { set; get; }
        public string ELIMINAR { set; get; }
    }
}