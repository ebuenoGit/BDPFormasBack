using Backend.Formas.BusinessRules.Aprobaciones;
using Backend.Formas.BusinessRules.Middle;
using Backend.Formas.BusinessRules.ViewSendMail;
using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTO;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.DTOI;
using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Entities.Models;
using Backend.Formas.Entities.ModelsAdm;
using Backend.Formas.Entities.Responses;
using Backend.Formas.Entities.Services;
using Backend.Formas.Utilities;
using Backend.Formas.Utilities.Telemetry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Linq;

namespace Backend.Formas.BusinessRules
{
    public class AdministrarFormasBusiness : IAdministracionFormasBusiness
    {
        private readonly IAdministracionFormas Repository;
        private readonly IForma9Repository RepositoryForma9;
        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient FormasService;
        private readonly ITelemetryException TelemetriExcepcion;
        private readonly Backend.Formas.Utilities.SendMail.ISendMailService SendMailService;
        private readonly Administracion.AdmGrpc.AdmGrpcClient AdministracionService;
        private readonly IForma30SEERepository _repositoryForma30;
        private readonly IForma30Repository _repositoryForm30;
        private readonly IC4 C4;
        private readonly IC1 C1;
        private readonly IC7 C7;
        private readonly IForma16 F16;
        private readonly IForma23Aprobacion F23;
        private readonly IFormaAprobacion<Aprobacion<Detalle21Aprobacion>> F21;
        private readonly IFormaAprobacion<Aprobacion<Detalle22Aprobacion>> F22;
        private readonly IFormaAprobacion<Aprobacion<Detalle15Aprobacion>> F15;
        private readonly IFormaAprobacion<Aprobacion<Detalle17Aprobacion>> F17;

        public AdministrarFormasBusiness(
            IAdministracionFormas _repository,
            IForma9Repository _forma9Repository9,
            Ppdm.PpdmGrpc.PpdmGrpcClient _formasService,
            ITelemetryException _telemetry,
            Backend.Formas.Utilities.SendMail.ISendMailService sendMailService,
            Administracion.AdmGrpc.AdmGrpcClient administracion,
            IForma30SEERepository repositoryForma30,
            IForma30Repository repositoryForm30,
            IC4 c4,
            IC1 c1,
            IC7 c7,
            IForma16 f16,
            IForma23Aprobacion f23,
            IFormaAprobacion<Aprobacion<Detalle21Aprobacion>> f21,
            IFormaAprobacion<Aprobacion<Detalle22Aprobacion>> f22,
            IFormaAprobacion<Aprobacion<Detalle15Aprobacion>> f15,
            IFormaAprobacion<Aprobacion<Detalle17Aprobacion>> f17
        )
        {
            Repository = _repository;
            RepositoryForma9 = _forma9Repository9;
            FormasService = _formasService;
            TelemetriExcepcion = _telemetry;
            SendMailService = sendMailService;
            AdministracionService = administracion;
            _repositoryForma30 = repositoryForma30;
            _repositoryForm30 = repositoryForm30;
            C4 = c4;
            C1 = c1;
            C7 = c7;
            F16 = f16;
            F23 = f23;
            F21 = f21;
            F22 = f22;
            F15 = f15;
            F17 = f17;
        }

        public async Task<ResponseBase<IList<AprobacionDTO>>> GetAprobacionFormas(string UserId)
        {
            var response = new ResponseBase<IList<AprobacionDTO>>();
            try
            {
                var list = await Repository.GetAprobacion(UserId);
                List<AprobacionDTO> dataJson = new List<AprobacionDTO>();

                foreach (Aprobacioncarga item in list)
                {
                    AprobacionDTO json = new AprobacionDTO()
                    {
                        Id = item.Id,
                        IdForma = item.IdForma,
                        Usuario = item.UsuarioNombre,
                        UsuarioAprobador = item.UsuarioNombreAprobador,
                        UsuarioCodigo = item.Usuario,
                        FechaActualizacion = item.FechaActualizacion,
                        FechaCarga = item.FechaCarga,
                        FechaForma = item.FechaForma,
                        EstadoNombre = item.Formstate.Name ?? "",
                        Estado = item.Estado,
                        ComparativoAgua = item.ComparativoAgua,
                        ComparativoCrudo = item.ComparativoCrudo,
                        ComparativoGas = item.ComparativoGas,
                        FormaName = $"{item.FormaName} - {item.Contrato}",
                        Url = item.UrlForma
                    };
                    dataJson.Add(json);
                }
                response.Data = dataJson;
                if (response.Data.Count > 0)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = Messages.Created;
                }
                else
                {
                    response.Count = (int)HttpStatusCode.BadRequest;
                    response.Message = Messages.ErrorCreation;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = !string.IsNullOrEmpty(ex.InnerException.Message)
                    ? ex.InnerException.Message
                    : Messages.ServerError;
                TelemetriExcepcion.RegisterException(ex);
            }

            return response;
        }

        public async Task<ResponseBase<dynamic>> generarAprobacion(List<Aprobacioncarga> data, string getUserid)
        {
            var response = new ResponseBase<dynamic>();

            try
            {
                List<dynamic> jsonFormas = new List<dynamic>();

                foreach (Aprobacioncarga json in data)
                {
                    int position = json.FormaName.IndexOf("-");

                    var nombreForma = json.FormaName.Substring(0, position).Trim();

                    var aprobacion = await Repository.GetAprobacion(json.Id);

                    

                    if ((aprobacion != null && nombreForma == "Cargue Forma 9") ||(aprobacion != null && nombreForma == "Forma 9"))
                    {
                        var forma9 = await RepositoryForma9.GetForm(aprobacion.IdForma);
                        var concret = await RepositoryForma9.GetFormConcreted(aprobacion.IdForma);
                        var total = await RepositoryForma9.GetFormTotal(aprobacion.IdForma);
                        var detail = await RepositoryForma9.GetFormDetail(aprobacion.IdForma);
                        CultureInfo cultureInfo = new CultureInfo("es-co");
                        List<DetailForm9> detailForma9json = new List<DetailForm9>();

                        foreach (var item in detail)
                        {

                            string dateForma = $"{concret.Month}/{concret.Year}";
                            DateTime dateTime = DateTime.Parse(dateForma, cultureInfo);
                            int day = dateTime.AddMonths(1).AddDays(-1).Day;
                            DateTime volumDate = new DateTime(dateTime.Year, dateTime.Month, day);
                            DateTime now = DateTime.Now;

                            DetailForm9 detailForma9 = new DetailForm9()
                            {
                                PDEN_ID = item.PdenId, //pden id numerico no nombre de pozo
                                PDEN_TYPE = "PDEN_PR_STR_FORM",
                                PDEN_SOURCE = "OFFICIAL",
                                VOLUME_METHOD = "1005",
                                ACTIVITY_TYPE = "007",
                                PERIOD_TYPE = "004",
                                VOLUME_DATE = volumDate.ToString("yyyyMMdd"),
                                AMENDMENT_SEQ_NO = "0",
                                ECP_PROD_METHOD = item.Productionmethod,
                                PERIOD_ON_PRODUCTION = item.Monthdays.ToString(),
                                ECP_CUM_PERIOD_ON_PRODUCTION = item.Accumulatedays.ToString(),
                                PERIOD_ON_PRODUCTION_UOM = "004",
                                OIL_VOLUME = item.Monthlyoilproduction.ToString(),
                                OIL_CUM_VOLUME = item.Accumulateoilproduction.ToString(),
                                ECP_CORRECTION_FACTOR = item.Correctionfactor.ToString(),
                                GAS_VOLUME = item.Montlhygasproduction.ToString(),
                                GAS_CUM_VOLUME = item.Accumulategasproduction.ToString(),
                                WATER_VOLUME = item.Monthlywaterproduction.ToString(),
                                WATER_CUM_VOLUME = item.Accumulatewaterproduction.ToString(),
                                ECP_BSW = item.Bsw.ToString(),
                                OIL_QUALITY = item.Apigrades.ToString(),
                                ECP_GOR = item.Rgp.ToString(),
                                EFFECTIVE_DATE = now.ToString("yyyyMMdd"),
                                EXPIRY_DATE = "20501231",
                                ACTIVE_IND = "Y",
                                ROW_CHANGED_BY = $"FRONTALBDP/${item.row_changed_by}",
                                ROW_CHANGED_DATE = DateTime.Parse(aprobacion.FechaCarga.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                                ROW_CREATED_BY = $"FRONTALBDP/${item.row_created_by}",
                                ROW_CREATED_DATE = DateTime.Parse(aprobacion.FechaActualizacion.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                            };

                            detailForma9json.Add(detailForma9);
                        }

                        JsonForma9 jsonForm = new JsonForma9()
                        {

                            FORMA_CODIGO = 9,
                            FORMA_NOMBRE = "Forma 9",
                            OPERADOR_ID = concret.Pdenid,
                            OPERADOR = concret.Company,
                            CONTRATO = concret.Contract,
                            CONTRATO_ID = forma9.contratoId,
                            CAMPO_ID = forma9.campoId.ToString(),
                            CAMPO = forma9.campo,
                            ESTRUCTURA_ID = forma9.Structure,
                            BLOQUE_ID = forma9.bloqueId,
                            FORMACION_ID = forma9.formacionId,
                            FORMACION = forma9.formacion,
                            FORMACION_SET_ID = forma9.formacionSetId,
                            MIEMBRO_ID = forma9.Member,
                            YACIMIENTO_ID = forma9.yacimientoId,
                            MES = concret.Month.ToString(),
                            ANIO = concret.Year.ToString(),
                            MODALIDADEXPLOTACION_ID = concret.Explotationmodality,
                            REPRESENTANTE_OPERADOR_NM = concret.Usersigning,
                            REPRESENTANTE_OPERADOR_TP = concret.Usersigning,
                            REPRESENTANTE_ANH_NAME = concret.Minrepsigning,
                            REPRESENTANTE_ANH_TP = concret.Minrepsigning,
                            GENERADO_DESDE = "FRONTALBDP",
                            ROW_CHANGED_BY = $"FRONTALBDP/${aprobacion.Usuario}",
                            ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                            ROW_CREATED_BY = $"FRONTALBDP/${aprobacion.Usuario}",
                            ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                            DETALLE = detailForma9json,
                            OBSERVACIONES = "",
                        };
                        jsonFormas.Add(jsonForm);
                    }
                    if ((aprobacion != null && nombreForma == "Cargue Forma 30SEE") || (aprobacion != null && nombreForma == "Forma 30SEE"))
                    {
                        List<BDConcreteForm> conForm = new List<BDConcreteForm>();
                        List<Form30Cab> form30Cab = new List<Form30Cab>();
                        List<DetalleAprobador30> detForm30 = new List<DetalleAprobador30>();
                        List<jsonForma30Aprobador> jsonForm30 = new List<jsonForma30Aprobador>();

                        var forma30Head = await _repositoryForma30.ConsultarCabeceraForma30(aprobacion.IdForma);
                        var forma30Reg = await _repositoryForma30.ConsultarCabeceraForma30Reg(aprobacion.IdForma);
                        var forma30Det = await _repositoryForma30.ConsultarCabeceraForma30Det(aprobacion.IdForma);


                        if (forma30Head.Rows.Count > 0)
                        {
                            foreach (DataRow row in forma30Head.Rows)
                            {
                                conForm.Add(new BDConcreteForm()
                                {
                                    concreteformid = Guid.Parse(row["concreteformid"].ToString()),
                                    maincampid = Convert.ToDecimal(row["maincampid"].ToString()),
                                    company = row["company"].ToString(),
                                    contract = row["contract"].ToString(),
                                    battery = row["battery"].ToString(),
                                    tank = row["tank"].ToString(),
                                    month = Convert.ToDecimal(row["month"].ToString()),
                                    year = Convert.ToDecimal(row["year"].ToString()),
                                    explotationmodality = row["explotationmodality"].ToString(),
                                    annotations = row["annotations"].ToString(),
                                    version = Convert.ToDecimal(row["version"].ToString()),
                                    currentstate = Guid.Parse(row["currentstate"].ToString()),
                                    generationflag = Convert.ToDecimal(row["generationflag"].ToString()),
                                    campid = Convert.ToDecimal(row["campid"].ToString()),
                                    pdenid = row["pdenid"].ToString(),
                                    formid = Guid.Parse(row["formid"].ToString()),
                                    generationjobid = Convert.ToDecimal(row["generationjobid"].ToString()),
                                    iqistatus = Convert.ToDecimal(row["iqistatus"].ToString()),
                                    usersigning = row["usersigning"].ToString(),
                                    minrepsigning = row["minrepsigning"].ToString(),
                                    formname = row["formname"].ToString()
                                });
                            }

                        }

                        if (forma30Reg.Rows.Count > 0)
                        {
                            foreach (DataRow row in forma30Reg.Rows)
                            {
                                form30Cab.Add(new Form30Cab()
                                {
                                    form30id = row["form30id"].ToString(),
                                    operador_id = row["operador_id"].ToString(),
                                    operador = row["operador"].ToString(),
                                    contrato_id = row["contrato_id"].ToString(),
                                    contrato = row["contrato"].ToString(),
                                    date = row["date"].ToString(),
                                    uid = row["uid"].ToString(),
                                    lastModified = row["lastModified"].ToString(),
                                    lastModifiedDate = row["lastModifiedDate"].ToString(),
                                    name = row["name"].ToString(),
                                    size = row["size"].ToString(),
                                    type = row["type"].ToString(),
                                    percent = row["percent"].ToString(),
                                    originFileObj_uid = row["originFileObj_uid"].ToString(),
                                });
                            }
                        }
                       
                        if (forma30Det.Rows.Count > 0)
                        {
                            foreach (DataRow row in forma30Det.Rows)
                            {
                                detForm30.Add(new DetalleAprobador30()
                                {
                                    PDEN_ID = row["pden_id"].ToString(),
                                    PDEN_TYPE = "PDEN_LAND_RIGHT",
                                    PDEN_SOURCE = "OFFICIAL",
                                    VOLUME_METHOD = "1005",
                                    PERIOD_TYPE = "004",
                                    VOLUME_DATE = row["volume_date"].ToString(),
                                    AMENDMENT_SEQ_NO = "0",
                                    ACTIVE_IND = "Y",
                                    ACTIVE_TYPE = "007",
                                    PRODUCT_TYPE="006",
                                    REAL_TP = "007",
                                    REAL_OIL = row["REAL"].ToString(),
                                    REAL_BAS = row["REAL_BAS"].ToString(),
                                    REAL_INC = row["REAL_INC"].ToString(),
                                    GAS_PROCESADO_TP = "005",
                                    GAS_PROCESADO = row["GAS_PROCESADO"].ToString(),
                                    GAS_QUEMADO_TP = "004",
                                    GAS_QUEMADO = row["GAS_QUEMADO"].ToString(),
                                    GAS_VOLUME = row["gasFormacionKPC"].ToString(),

                                    GP_BUTANO = row["BUTANO"].ToString(),
                                    GP_PROPANO = row["PROPANO"].ToString(),
                                    GP_GASOLINA= row["GASOLINA"].ToString(),
                                    GP_TRANSFORMADO_GASOLINA = row["productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos"].ToString(),


                                    GLP_TP = row["GLP_TP"].ToString(),
                                    GLP = row["GLP"].ToString(),
                                    PROPANO_TP = "007",
                                    PROPANO = row["contenidoPropano"].ToString(),
                                    BUTANO_TP = "007",
                                    BUTANO = row["contenidoButano"].ToString(),
                                    GASOLINA_TP = "007",
                                    GASOLINA = row["contenidoGasolinaNatural"].ToString(),
                                    APIASOL_TP = row["APIASOL_TP"].ToString(),
                                    APIASOL = row["APIASOL"].ToString(),
                                    CONDENSADO_TP = row["CONDENSADO_TP"].ToString(),
                                    CONDENSADO = row["CONDENSADO"].ToString(),
                                    CONSUMOS_TP = "018",
                                    CONSUMOS = row["CONSUMOS"].ToString(),
                                    GASODUCTOS_URBANOS_TP = "028",
                                    GASODUCTOS_URBANOS = row["GASODUCTOS_URBANOS"].ToString(),
                                    GENERACION_ELECTRICA_TP = "020",
                                    GENERACION_ELECTRICA = row["GENERACION_ELECTRICA"].ToString(),
                                    OTRAS_VENTAS_TP = "010",
                                    OTRAS_VENTAS = row["OTRAS_VENTAS"].ToString(),
                                    GAS_TRANFERENCIA_TP = "081",
                                    GAS_TRANFERENCIA = row["GAS_TRANFERENCIA"].ToString(),
                                    GAS_NEUMATICO_TP = "021",
                                    GAS_NEUMATICO = row["GAS_NEUMATICO"].ToString(),
                                    GAS_INYECTADO_TP = "006",
                                    GAS_INYECTADO = row["GAS_INYECTADO"].ToString(),
                                    TOTAL_PROC_PLANTA_TP = row["TOTAL_PROC_PLANTA_TP"].ToString(),
                                    TOTAL_PROC_PLANTA = row["TOTAL_PROC_PLANTA"].ToString(),
                                    GP_TRATOTAL_PROC_PLANTA_TP = row["GP_TRATOTAL_PROC_PLANTA_TP"].ToString(),
                                    GP_TRATOTAL_PROC_PLANTA = row["GP_TRATOTAL_PROC_PLANTA"].ToString(),
                                    GP_TRANSFORMADO_TP = row["GP_TRANSFORMADO_TP"].ToString(),
                                    GP_TRANSFORMADO = row["GP_TRANSFORMADO"].ToString(),
                                    GP_CONSUMOS_TP = "018",

                                    GP_CONSUMOS = row["GP_CONSUMOS"].ToString(),
                                    GP_GASODUCTOS_URBANOS_TP = "028",
                                    GP_GASODUCTOS_URBANOS = row["GP_GASODUCTOS_URBANOS"].ToString(),
                                    GP_GENERACION_ELECTRICA_TP = "027",
                                    GP_GENERACION_ELECTRICA = row["GP_GENERACION_ELECTRICA"].ToString(),
                                    GP_OTRAS_VENTAS_TP = row["GP_OTRAS_VENTAS_TP"].ToString(),
                                    GP_OTRAS_VENTAS = row["GP_OTRAS_VENTAS"].ToString(),
                                    GP_NEUMATICO_TP = row["GP_NEUMATICO_TP"].ToString(),
                                    GP_NEUMATICO = row["GP_NEUMATICO"].ToString(),
                                    GP_QUEMADO_TP = row["GP_QUEMADO_TP"].ToString(),
                                    GP_QUEMADO = row["GP_QUEMADO"].ToString(),
                                    GP_INYECTADO_TP = row["GP_INYECTADO_TP"].ToString(),
                                    GP_INYECTADO = row["GP_INYECTADO"].ToString(),
                                    OBSERVACIONES = row["OBSERVACIONES"].ToString(),
                                    REPRESENTA_OPER = row["REPRESENTA_OPER"].ToString(),
                                    REPRESENTA_ANH = row["REPRESENTA_ANH"].ToString(),
                                    IDFORMA = row["IDFORMA"].ToString(),
                                    ESTADOFORMA = row["ESTADOFORMA"].ToString(),
                                    ROW_CHANGED_BY = $"FRONTALBDP/${aprobacion.Usuario}",
                                    ROW_CHANGED_DATE = Convert.ToDateTime(row["ROW_CHANGED_DATE"].ToString()).ToString("yyyyMMdd HH:mm:ss"),
                                    ROW_CREATED_BY = $"FRONTALBDP/${aprobacion.Usuario}",
                                    ROW_CREATED_DATE = Convert.ToDateTime(row["ROW_CREATED_DATE"].ToString()).ToString("yyyyMMdd HH:mm:ss")
                                });
                            }
                        }
                       
                        var cabecera = form30Cab.Count;
                        DataRow dataCampo = forma30Det.Rows[0];
                        var campoId = dataCampo["campo_id"];
                        var campo = dataCampo["campo"];

                        for (int i = 0; i < cabecera; i++)
                        {
                            jsonForm30.Add(new jsonForma30Aprobador()
                            {
                                FORMA_CODIGO = "30",
                                FORMA_NOMBRE = nombreForma,
                                MES = conForm[i].month.ToString(),
                                ANIO = conForm[i].year.ToString(),
                                OPERADOR_ID = form30Cab[i].operador_id,
                                OPERADOR = form30Cab[i].operador,
                                CONTRATO_ID = form30Cab[i].contrato_id,
                                CONTRATO = form30Cab[i].contrato,
                                CAMPO_ID =campoId.ToString() ,
                                CAMPO = campo.ToString(),
                                BATERIA_ID = null,
                                BATERIA = null,
                                TANQUE_ID = null,
                                TANQUE = null,
                                ESTRUCTURA_ID = null,
                                ESTRUCTURA = null,
                                BLOQUE_ID = null,
                                BLOQUE = null,
                                FORMACION_ID = null,
                                FORMACION_SET_ID = null,
                                FORMACION = null,
                                MIEMBRO_ID = null,
                                MIEMBRO = null,
                                YACIMIENTO_ID = null,
                                YACIMIENTO = null,
                                MODALIDADEXPLOTACION_ID = null,
                                MODALIDADEXPLOTACION = null,
                                REPRESENTANTE_OPERADOR_NM = null,
                                REPRESENTANTE_OPERADOR_TP = null,
                                REPRESENTANTE_ANH_NAME = null,
                                REPRESENTANTE_ANH_TP = null,
                                GENERADO_DESDE = "FRONTAL",
                                ROW_CHANGED_BY = $"FRONTALBDP/{aprobacion.Usuario}",
                                ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                                ROW_CREATED_BY = $"FRONTALBDP/{aprobacion.Usuario}",
                                ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                                OBSERVACIONES = null,
                                DETALLE = detForm30
                            });
                        }

                        var lista = new List<dynamic>();

                        jsonFormas.Add(jsonForm30);
                    }
                    if ((aprobacion != null && nombreForma == "Cargue Forma 20CR")||(aprobacion != null && nombreForma == "Forma 20CR"))
                    {
                        //var forma = await formasAprobarRepository.ConsultarForma(json.Id);

                        List<dynamic> result = new List<dynamic>();
                        FormasCabecera FormaCabecera = new FormasCabecera();
                        //Forma20SHAprobar Forma20 = new Forma20SHAprobar();
                        List<DetalleForma20SH> formaDetalle = new List<DetalleForma20SH>();

                        try
                        {
                            var cabecera = await _repositoryForma30.ConsultarEncabezado(json.Id);
                            var detalle = await _repositoryForma30.ConsultarDetalle(json.Id);



                            if (detalle.Rows.Count > 0)
                            {
                                foreach (DataRow row in detalle.Rows)
                                {
                                    formaDetalle.Add(new DetalleForma20SH()
                                    {
                                        PDEN_ID = row["PDEN_ID"].ToString(),
                                        PDEN_TYPE = row["PDEN_TYPE"].ToString(),
                                        PDEN_SOURCE = row["PDEN_SOURCE"].ToString(),
                                        VOLUME_METHOD = "1005",
                                        ACTIVITY_TYPE = "006",
                                        PERIOD_TYPE = "004",
                                        VOLUME_DATE = Convert.ToDateTime(row["VOLUME_DATE"].ToString()).ToString("yyyyMMdd"),
                                        AMENDMENT_SEQ_NO = row["AMENDMENT_SEQ_NO"].ToString(),
                                        ACTIVE_IND = row["ACTIVE_IND"].ToString(),
                                        EFFECTIVE_DATE = Convert.ToDateTime(row["EFFECTIVE_DATE"].ToString()).ToString("yyyyMMdd"),
                                        EXPIRY_DATE = Convert.ToDateTime(row["EXPIRY_DATE"].ToString()).ToString("yyyyMMdd"),
                                        PERIOD_ON_INJECTION = Convert.ToInt32(row["PERIOD_ON_INJECTION"].ToString()).ToString(),
                                        PERIOD_ON_INJECTION_OUOM = "DIAS",
                                        INJECTION_PRESSURE = row["INJECTION_PRESSURE"].ToString(),
                                        PRIMARY_PRODUCT = "039",
                                        EC_INJECTION_VOLUME = row["EC_INJECTION_VOLUME"].ToString(),
                                        EC_INJECTION_VOLUME_OUOM = "BLS",
                                        EC_INJECTION_CUM_VOLUME = row["EC_INJECTION_CUM_VOLUME"].ToString(),
                                        ECP_CUM_PERIOD_ON_PRODUCTION = row["ECP_CUM_PERIOD_ON_PRODUCTION"].ToString(),
                                        ROW_CHANGED_BY = $"FRONTALBDP/{aprobacion.Usuario}",
                                        ROW_CHANGED_DATE = Convert.ToDateTime(row["ROW_CHANGED_DATE"].ToString()).ToString("yyyyMMdd HH:mm:ss"),
                                        ROW_CREATED_BY = $"FRONTALBDP/{ aprobacion.Usuario }",
                                        ROW_CREATED_DATE = Convert.ToDateTime(row["ROW_CREATED_DATE"].ToString()).ToString("yyyyMMdd HH:mm:ss")
                                    });
                                }
                            }

                            if (cabecera.Rows.Count > 0)
                            {
                                foreach (DataRow row in cabecera.Rows)
                                {

                                    FormaCabecera.FORMA_CODIGO = row["FORMA_CODIGO"].ToString();
                                    FormaCabecera.FORMA_NOMBRE = row["FORMA_NOMBRE"].ToString();
                                    FormaCabecera.MES = row["MES"].ToString();
                                    FormaCabecera.ANIO = row["ANIO"].ToString();
                                    FormaCabecera.OPERADOR_ID = row["OPERADOR_ID"].ToString();
                                    FormaCabecera.OPERADOR = row["OPERADOR"].ToString();
                                    FormaCabecera.CONTRATO_ID = row["CONTRATO_ID"].ToString();
                                    FormaCabecera.CONTRATO = row["CONTRATO"].ToString();
                                    FormaCabecera.CAMPO_ID = row["CAMPO_ID"].ToString();
                                    FormaCabecera.CAMPO = row["CAMPO"].ToString();
                                    FormaCabecera.BATERIA_ID = null;
                                    FormaCabecera.BATERIA = null;
                                    FormaCabecera.TANQUE_ID = null;
                                    FormaCabecera.TANQUE = null;
                                    FormaCabecera.ESTRUCTURA_ID = null;
                                    FormaCabecera.ESTRUCTURA = null;
                                    FormaCabecera.BLOQUE_ID = null;
                                    FormaCabecera.BLOQUE = null;
                                    FormaCabecera.FORMACION_ID = row["FORMACION_ID"].ToString();
                                    FormaCabecera.FORMACION_SET_ID = row["FORMACION_SET_ID"].ToString();
                                    FormaCabecera.FORMACION = row["FORMACION"].ToString();
                                    FormaCabecera.MIEMBRO_ID = null;
                                    FormaCabecera.MIEMBRO = null;
                                    FormaCabecera.YACIMIENTO_ID = null;
                                    FormaCabecera.YACIMIENTO = null;
                                    FormaCabecera.MODALIDADEXPLOTACION_ID = null;
                                    FormaCabecera.MODALIDADEXPLOTACION = row["MODALIDADEXPLOTACION"].ToString();
                                    FormaCabecera.REPRESENTANTE_OPERADOR_NM = row["REPRESENTANTE_OPERADOR_NM"].ToString();
                                    FormaCabecera.REPRESENTANTE_OPERADOR_TP = row["REPRESENTANTE_OPERADOR_TP"].ToString();
                                    FormaCabecera.REPRESENTANTE_ANH_NAME = row["REPRESENTANTE_ANH_NAME"].ToString();
                                    FormaCabecera.REPRESENTANTE_ANH_TP = row["REPRESENTANTE_ANH_TP"].ToString();
                                    FormaCabecera.GENERADO_DESDE = "FRONTALBDP";
                                    FormaCabecera.ROW_CHANGED_BY = $"FRONTALBDP/{ aprobacion.Usuario }";
                                    FormaCabecera.ROW_CHANGED_DATE = Convert.ToDateTime(row["ROW_CHANGED_DATE"].ToString()).ToString("yyyyMMdd HH:mm:ss");
                                    FormaCabecera.ROW_CREATED_BY = $"FRONTALBDP/{ aprobacion.Usuario }";
                                    FormaCabecera.ROW_CREATED_DATE = Convert.ToDateTime(row["ROW_CREATED_BY"].ToString()).ToString("yyyyMMdd HH:mm:ss");
                                    FormaCabecera.OBSERVACIONES = null;
                                    FormaCabecera.DETALLE = formaDetalle;
                                    break;
                                }
                            }


                            result.Add(FormaCabecera);

                        }
                        catch (Exception ex)
                        {
                            TelemetriExcepcion.RegisterException(ex);
                            return null;
                        }

                        jsonFormas.Add(FormaCabecera);
                    }
                    if ((aprobacion != null && nombreForma == "Cargue Forma 21CR")||(aprobacion != null && nombreForma == "Forma 21CR"))
                    {
                        var f = await F21.JsonFormat(json, getUserid);
                        jsonFormas.Add(f);
                    }
                    if ((aprobacion != null && nombreForma == "Cargue Forma Cuadro 4") || (aprobacion != null && nombreForma == "Forma Cuadro 4"))
                    {
                        var c4 = await C4.JsonFormat(json, getUserid);
                        jsonFormas.Add(c4);
                    }
                    if ((aprobacion != null && nombreForma == "Cargue Forma Cuadro 1A")||(aprobacion != null && nombreForma == "Forma Cuadro 1A"))
                    {
                        var c1 = await C1.JsonFormat(json, getUserid);
                        jsonFormas.Add(c1);
                    }
                    if ((aprobacion != null && nombreForma == "Cargue Forma Cuadro 7")||(aprobacion != null && nombreForma == "Forma Cuadro 7"))
                    {
                        var c7 = await C7.JsonFormat(json, getUserid);
                        jsonFormas.Add(c7);
                    }
                    if ((aprobacion != null && nombreForma == "Cargue Forma 15ACR") ||(aprobacion != null && nombreForma == "Forma 15ACR"))
                    {
                        var fm = await F15.JsonFormat(json, getUserid);
                        jsonFormas.Add(fm);
                    }
                    if ((aprobacion != null && nombreForma == "Cargue Forma 16CR") ||(aprobacion != null && nombreForma == "Forma 16CR"))
                    {
                        var f16 = await F16.JsonFormat(json, getUserid);
                        jsonFormas.Add(f16); 
                    }
                    if ((aprobacion != null && nombreForma == "Cargue Forma 17CR") || (aprobacion != null && nombreForma == "Forma 17CR"))
                    {
                        var fm = await F17.JsonFormat(json, getUserid);
                        jsonFormas.Add(fm);
                    }
                    if ((aprobacion != null && nombreForma == "Cargue Forma 23CR")||(aprobacion != null && nombreForma == "Forma 23CR"))
                    {
                        var f23 = await F23.JsonFormat(json, getUserid);
                        jsonFormas.Add(f23);
                    }

                    if ((aprobacion != null && nombreForma == "Cargue Forma 22CR")||(aprobacion != null && nombreForma == "Forma 22CR"))
                    {
                        var fm = await F22.JsonFormat(json, getUserid);
                        jsonFormas.Add(fm);
                    }

                    var settings = new Newtonsoft.Json.JsonSerializerSettings
                    {
                        NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
                    };

                    var jsonConvert = jsonFormas.Serialize(settings);
                    var jsonrequest = jsonConvert.Replace("[[", "[").Replace("]]", "]");
                    var res = await FormasService.IntegradorFormasMinAsync(new Ppdm.RequestBase { SJson = jsonrequest});

                    try
                    {
                        var deserializa = res.Data.Deserialize<RequestAprobacion<C4ModelRequest>>();

                        if (!string.IsNullOrEmpty(deserializa?.root?.error))
                        {
                            response.Count = (int)HttpStatusCode.BadRequest;
                            response.Message = deserializa.root.error;
                        }
                    }
                    catch
                    {
                        var respuestaExitosa = res.Data.Deserialize<RequesAprobacionExitosa>();

                        if (res.Code == 200 && !string.IsNullOrEmpty(respuestaExitosa.root) && respuestaExitosa.root == "EXITO")
                        {
                            /*foreach (Aprobacioncarga json in data)
                            {*/

                            var forma = await Repository.GetAprobacion(json.Id);

                            decimal id = json.Id;
                            DataTable notifica = _repositoryForma30.ConsultarNotificacion(id);
                            await Repository.FormaAprobada(id);
                            var userService = await AdministracionService.UsuarioAprobadorAsync(new Administracion.RequestAprobaciones { NombreForma = nombreForma });
                            var usuario = await AdministracionService.ObtenerUsuarioAsync(new Administracion.RequesUsuario { Codigo = forma.Usuario });
                            var user = usuario.Data.Deserialize<Usuario>();
                            var userEmail = userService.Data.Deserialize<UsuarioCorreo>();


                            string asunto = "Se ha aprobado la forma: " + nombreForma;
                            MailForma9 mailForma = new MailForma9();

                            string view = mailForma.GetView(asunto, json.UsuarioNombre, notifica.Rows[0]["operador"].ToString(), notifica.Rows[0]["contrato"].ToString(), notifica.Rows[0]["campo"].ToString(), json.FechaForma.Month.ToString() + " / " + json.FechaForma.Year.ToString(),
                                $"La solicitud para la Forma: {nombreForma} ha sido Aprobada");

                            EmailInfo Email = new EmailInfo()
                            {
                                To = new List<string>() { user.email },
                                Subject = asunto,
                                Body = view,
                                Styles = mailForma.GetHeaderStyle()
                            };

                            try
                            {
                                await SendMailService.SendEmailAsync(Email);
                            }
                            catch (Exception e)
                            {
                                TelemetriExcepcion.RegisterException(e);
                            }
                            //}
                            response.Count = (int)HttpStatusCode.Created;
                            response.Message = Messages.Created;

                        }
                    }
                }

            }
            catch (Exception e)
            {
                response.Count = (int)HttpStatusCode.BadRequest;
                response.Message = e.Message;
                TelemetriExcepcion.RegisterException(e);
            }

            return response;
        }


        public async Task<ResponseBase<List<Aprobacionprecarga>>> RegistrarPrecargas(List<Aprobacionprecarga> data, string GetUserId, string GetNames)
        {
            var response = new ResponseBase<List<Aprobacionprecarga>>();

            //await Repository.CreatePrecarga(data);
            try
            {

                var DataUser = await AdministracionService.UsuarioAprobadorAsync(new Administracion.RequestAprobaciones { NombreForma = data[0].FormaName });
                var dataService = DataUser.Data.Deserialize<UsuariosRecursosAprobaciones>();

                data[0].Usuario = GetUserId;
                data[0].UsuarioNombre = GetNames;
                data[0].UsuarioNombreAprobador = dataService.NombreUsuario;
                data[0].UsuarioAprobador = dataService.CodigoUsuario;

                await Repository.CreatePrecarga(data);

                try
                {

                    var info = data[0];
                    string asunto = "Notificación de solicitud de recarga para la forma: <br/>" + data[0].FormaName;
                    MailForma9 mailForma = new MailForma9();

                    string view = mailForma.GetView(asunto, info.UsuarioNombreAprobador, info.Operadora, info.Contrato, info.Campo, info.FechaForma.ToString(),
                        $"El usuario {GetNames} ha solicitado un permiso para cargar una forma fuera de la fecha opertativa ó la forma que intenta cargar se encuentra aprobada ");
                    EmailInfo Email = new EmailInfo()
                    {
                        To = new List<string>() { dataService.Correo },
                        Subject = asunto,
                        Body = view,
                        Styles = mailForma.GetHeaderStyle()
                    };

                    await SendMailService.SendEmailAsync(Email);
                }
                catch (Exception e)
                {
                    TelemetriExcepcion.RegisterException(e);
                }
                response.Data = data;
                response.Code = (int)HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                TelemetriExcepcion.RegisterException(e);
                response.Code = (int)HttpStatusCode.BadRequest;
            }
            return response;
        }

        public async Task<ResponseBase<List<Aprobacionprecarga>>> GetPrecargasList()
        {
            var response = new ResponseBase<List<Aprobacionprecarga>>();
            try
            {
                var list = await Repository.PrecargaList();
                response.Data = list;
                response.Code = (int)HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                TelemetriExcepcion.RegisterException(e);
                response.Code = (int)HttpStatusCode.BadRequest;
            }

            return response;
        }

        public async Task<ResponseBase<Aprobacionprecarga>> ActualizarPrecarga(Aprobacionprecarga data, string usuarioId)
        {
            var response = new ResponseBase<Aprobacionprecarga>();

            try
            {
                var res = await AdministracionService.ObtenerUsuarioAsync(new Administracion.RequesUsuario { Codigo = data.Usuario });

                try
                {
                    if (res.Data != null)
                    {
                        var user = res.Data.Deserialize<Usuario>();
                        if (data.Activo == 1)
                        {
                            try
                            {
                                string asunto = "Notificación recarga aprobada";
                                MailForma9 mailForma = new MailForma9();

                                string view = mailForma.GetView(asunto, data.UsuarioNombre, data.Operadora, data.Contrato, data.Campo, data.FechaForma.ToString(),
                                    $"La solicitud para la recarga de la forma {data.FormaName} fue aprobada para las fechas <br/>Fecha Apertura: {data.FechaApertura} <br/> Fecha Cierre: {data.FechaCierre}");

                                EmailInfo Email = new EmailInfo()
                                {
                                    To = new List<string>() { user.email },
                                    Subject = asunto,
                                    Body = view,
                                    Styles = mailForma.GetHeaderStyle()
                                };

                                await SendMailService.SendEmailAsync(Email);
                            }
                            catch (Exception e)
                            {
                                TelemetriExcepcion.RegisterException(e);
                            }
                        }
                        if (data.Activo == 0)
                        {
                            try
                            {
                                string asunto = "Notificación recarga rechazada";
                                MailForma9 mailForma = new MailForma9();

                                string view = mailForma.GetView(asunto, data.UsuarioNombre, data.Operadora, data.Contrato, data.Campo, data.FechaForma.ToString(), $"La solicitud para realizar la recarga de la forma {data.FormaName} fue rechazada");

                                EmailInfo Email = new EmailInfo()
                                {
                                    To = new List<string>() { user.email },
                                    Subject = asunto,
                                    Body = view,
                                    Styles = mailForma.GetHeaderStyle()
                                };

                                await SendMailService.SendEmailAsync(Email);
                            }
                            catch (Exception e)
                            {
                                TelemetriExcepcion.RegisterException(e);
                            }
                        }

                    }
                    await Repository.UpdatePrecarga(data);
                    response.Data = data;
                    response.Code = (int)HttpStatusCode.OK;
                }
                catch (Exception e)
                {
                    TelemetriExcepcion.RegisterException(e);
                    response.Data = data;
                    response.Code = (int)HttpStatusCode.InternalServerError;
                }


            }
            catch (Exception e)
            {
                TelemetriExcepcion.RegisterException(e);
                response.Code = (int)HttpStatusCode.BadRequest;
            }
            return response;
        }
    }
}