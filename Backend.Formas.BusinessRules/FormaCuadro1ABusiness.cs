using Backend.Formas.BusinessRules.ViewSendMail;
using Backend.Formas.Entities.DTO;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Entities.Models;
using Backend.Formas.Entities.ModelsAdm;
using Backend.Formas.Entities.Responses;
using Backend.Formas.Entities.Services;
using Backend.Formas.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules
{
    public class FormaCuadro1ABusiness : IFormaCuadro1ABusiness
    {
        private readonly IFormaCuadro1ARepository formaCuadro1A;
        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;
        private readonly RequestFormas<FormaCuadro1A> Response = new RequestFormas<FormaCuadro1A>();
        private readonly IFormaValidate validate;
        private readonly IForma30SEERepository _repository;
        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient FormasService;
        private readonly Administracion.AdmGrpc.AdmGrpcClient AdministracionService;
        private readonly Commons.CommonGrpc.CommonGrpcClient CommonService;
        private readonly IAdministracionFormas _repositoryAdministracion;
        private readonly Backend.Formas.Utilities.SendMail.ISendMailService SendMailService;

        public FormaCuadro1ABusiness(IFormaCuadro1ARepository _formaCuadro1A
                                    , IFormaValidate _validate
                                    , Ppdm.PpdmGrpc.PpdmGrpcClient _FormasService
                                    , Administracion.AdmGrpc.AdmGrpcClient _AdministracionService
                                    , Commons.CommonGrpc.CommonGrpcClient commonService
                                    , IAdministracionFormas repositoryAdministracion
                                    , IForma30SEERepository repository
                                    , Utilities.Telemetry.ITelemetryException _TelemetryException
                                    , Backend.Formas.Utilities.SendMail.ISendMailService _SendMailService)
        {
            formaCuadro1A = _formaCuadro1A;
            validate = _validate;
            FormasService = _FormasService;
            AdministracionService = _AdministracionService;
            CommonService = commonService;
            _repositoryAdministracion = repositoryAdministracion;
            _repository = repository;
            TelemetryException = _TelemetryException;
            SendMailService = _SendMailService;
        }

        /// <summary>
        /// Valida Estructura Archivo.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="forma"></param>
        /// <param name="fileJson"></param>
        /// <returns></returns>
        public async Task<ResponseBase<RequestFormas<FormaCuadro1A>>> ValidateFormaCuadro1A(DateTime date, string forma, string fileJson, string usuario)
        {
            var json = fileJson.Deserialize<List<FormaCuadro1A>>();

            var response = new ResponseBase<RequestFormas<FormaCuadro1A>>();
            List<ErrorFormasStructura> err = new List<ErrorFormasStructura>();
            List<FormaCuadro1Validador> val = new List<FormaCuadro1Validador>();
            List<FormaCuadro1ValidadorDetalle> valDet = new List<FormaCuadro1ValidadorDetalle>();

            try
            {

                var hojas = json.Count;

                var formafecha = ValidaFechasPermisos(date, DateTime.Now, json[0].info.compania, "", json[0].info.contrato, usuario, forma);

                if (hojas > 0)
                {


                    for (int i = 0; i < hojas; i++)
                    {

                        var valOperador = new ResponseBase<List<dynamic>>
                        {
                            Data = await validate.ValidaOperador(json[i].info.compania)
                        };

                        if (valOperador.Data?.Count == 0 || valOperador.Data == null)
                        {
                            err.Add(new ErrorFormasStructura() { sheet = i, column = "operador", row = 0, message = "El nombre de la compañia no existe.", value = json[i].info.compania });
                        }

                        var valCampo = new ResponseBase<List<dynamic>>
                        {
                            Data = await validate.ValidaCampo(json[i].info.campo)
                        };

                        if (valCampo.Data?.Count == 0 || valCampo.Data == null)
                        {
                            err.Add(new ErrorFormasStructura() { sheet = i, column = "Campo", row = 0, message = "El nombre del Campo no existe.", value = json[i].info.campo });
                        }

                        var valContrato = await validate.ValidaCompaniaCampo(json[i].info.compania, json[i].info.campo);
                        if (valOperador.Data?.Count == 0 || valOperador.Data == null)
                        {
                            err.Add(new ErrorFormasStructura() { sheet = i, column = null, row = 0, message = "No se puede extraer el contrato para la compañia: " + json[i].info.compania + " y el campo: " + json[i].info.campo, value = "" });
                        }
                        else
                        {
                            json[i].info.contrato_id = ConvertTypes.ConverDataDynamic(valContrato, "CONTRATO_ID");
                            json[i].info.contrato = ConvertTypes.ConverDataDynamic(valContrato, "CONTRATO");
                        }

                        var validaRegistrosArchivo = await validate.ValidaCampoContratoOperador(json[i].info.compania, json[i].info.campo, json[i].info.contrato);

                        if (validaRegistrosArchivo?.Count == 0 || validaRegistrosArchivo == null)
                        {

                            err.Add(new ErrorFormasStructura() { sheet = i, column = "", row = 0, message = "Al validar el nombre de la compañia y del campo, estos no coinciden. Compañia: " + json[i].info.compania + ", campo: " + json[i].info.campo, value = "" });

                        }
                        else
                        {
                            json[i].info.campo_id = ConvertTypes.ConverDataDynamic(validaRegistrosArchivo, "CAMPO_ID");
                            json[i].info.compania_id = ConvertTypes.ConverDataDynamic(validaRegistrosArchivo, "OPERADOR_ID");
                            json[i].info.contrato_id = ConvertTypes.ConverDataDynamic(validaRegistrosArchivo, "CONTRATO_ID");
                        }

                        var data = json[i].data.Count;

                        if (data > 0)
                        {
                            for (int d = 0; d < data; d++)
                            {
                                if (string.IsNullOrEmpty(json[i].data[d].medidaMM.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "medidaMM", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].medidaMM.ToString() }); }
                                else if (validarDecimal(json[i].data[d].medidaMM.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "medidaMM", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].medidaMM.ToString() }); }
                                else if (json[i].data[d].medidaMM < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "medidaMM", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].medidaMM.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].aforoBLS.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "aforoBLS", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].aforoBLS.ToString() }); }
                                else if (validarDecimal(json[i].data[d].aforoBLS.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "aforoBLS", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].aforoBLS.ToString() }); }
                                else if (json[i].data[d].aforoBLS < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "aforoBLS", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].aforoBLS.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].tempF.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "tempF", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].tempF.ToString() }); }
                                else if (validarDecimal(json[i].data[d].tempF.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "tempF", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].tempF.ToString() }); }
                                else if (json[i].data[d].tempF < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "tempF", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].tempF.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].factorTemp.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "factorTemp", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].factorTemp.ToString() }); }
                                else if (validarDecimal(json[i].data[d].factorTemp.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "factorTemp", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].factorTemp.ToString() }); }
                                else if (json[i].data[d].factorTemp < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "factorTemp", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].factorTemp.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].BLS60F.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BLS60F", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].BLS60F.ToString() }); }
                                else if (validarDecimal(json[i].data[d].BLS60F.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BLS60F", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].BLS60F.ToString() }); }
                                else if (json[i].data[d].BLS60F < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BLS60F", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].BLS60F.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].BSW.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BSW", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].BSW.ToString() }); }
                                else if (validarDecimal(json[i].data[d].BSW.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BSW", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].BSW.ToString() }); }
                                else if (json[i].data[d].BSW < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BSW", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].BSW.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].factorBSW.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "factorBSW", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].factorBSW.ToString() }); }
                                else if (validarDecimal(json[i].data[d].factorBSW.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "factorBSW", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].factorBSW.ToString() }); }
                                else if (json[i].data[d].factorBSW < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "factorBSW", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].factorBSW.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].CTSH.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "CTSH", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].CTSH.ToString() }); }
                                else if (validarDecimal(json[i].data[d].CTSH.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "CTSH", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].CTSH.ToString() }); }
                                else if (json[i].data[d].CTSH < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "CTSH", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].CTSH.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].tempAmb.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "tempAmb", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].tempAmb.ToString() }); }
                                else if (validarDecimal(json[i].data[d].tempAmb.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "tempAmb", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].tempAmb.ToString() }); }
                                else if (json[i].data[d].tempAmb < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "tempAmb", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].tempAmb.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].BLSNetos.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BLSNetos", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].BLSNetos.ToString() }); }
                                else if (validarDecimal(json[i].data[d].BLSNetos.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BLSNetos", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].BLSNetos.ToString() }); }
                                else if (json[i].data[d].BLSNetos < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BLSNetos", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].BLSNetos.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].transfBLS.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "transfBLS", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].transfBLS.ToString() }); }
                                else if (validarDecimal(json[i].data[d].transfBLS.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "transfBLS", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].transfBLS.ToString() }); }
                                else if (json[i].data[d].transfBLS < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "transfBLS", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].transfBLS.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].recibidoBLS.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "recibidoBLS", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].recibidoBLS.ToString() }); }
                                else if (validarDecimal(json[i].data[d].recibidoBLS.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "recibidoBLS", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].recibidoBLS.ToString() }); }
                                else if (json[i].data[d].recibidoBLS < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "recibidoBLS", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].recibidoBLS.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].entregaBLS.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "entregaBLS", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].entregaBLS.ToString() }); }
                                else if (validarDecimal(json[i].data[d].entregaBLS.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "entregaBLS", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].entregaBLS.ToString() }); }
                                else if (json[i].data[d].entregaBLS < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "entregaBLS", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].entregaBLS.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].API60F.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "API60F", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].API60F.ToString() }); }
                                else if (validarDecimal(json[i].data[d].API60F.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "API60F", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].API60F.ToString() }); }
                                else if (json[i].data[d].API60F < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "API60F", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].API60F.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].GE.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "GE", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].GE.ToString() }); }
                                else if (validarDecimal(json[i].data[d].GE.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "GE", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].GE.ToString() }); }
                                else if (json[i].data[d].GE < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "GE", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].GE.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].netosGE.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "netosGE", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].netosGE.ToString() }); }
                                else if (validarDecimal(json[i].data[d].netosGE.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "netosGE", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].netosGE.ToString() }); }
                                else if (json[i].data[d].netosGE < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "netosGE", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].netosGE.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].salBTB.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "salBTB", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].salBTB.ToString() }); }
                                else if (validarDecimal(json[i].data[d].salBTB.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "salBTB", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].salBTB.ToString() }); }
                                else if (json[i].data[d].salBTB < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "salBTB", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].salBTB.ToString() }); }

                               
                            }
                        }
                        else
                        {
                            err.Add(new ErrorFormasStructura() { sheet = 1, column = "", row = 0, message = "Los registros de la Hoja se encuentran vacios.", value = "" });
                        }

                        if (string.IsNullOrEmpty(json[i].total.BLS60F.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BLS60F", row = 0, message = "El campo no debe estar vacio.", value = json[i].total.BLS60F.ToString() }); }
                        else if (validarDecimal(json[i].total.BLS60F.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BLS60F", row = 0, message = "El campo debe ser decimal.", value = json[i].total.BLS60F.ToString() }); }
                        else if (json[i].total.BLS60F < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BLS60F", row = 0, message = "El campo no debe ser negativo.", value = json[i].total.BLS60F.ToString() }); }

                        if (string.IsNullOrEmpty(json[i].total.BLSNetos.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BLSNetos", row = 0, message = "El campo no debe estar vacio.", value = json[i].total.BLSNetos.ToString() }); }
                        else if (validarDecimal(json[i].total.BLSNetos.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BLSNetos", row = 0, message = "El campo debe ser decimal.", value = json[i].total.BLSNetos.ToString() }); }
                        else if (json[i].total.BLSNetos < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "BLSNetos", row = 0, message = "El campo no debe ser negativo.", value = json[i].total.BLSNetos.ToString() }); }

                        if (string.IsNullOrEmpty(json[i].total.recibidoBLS.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "recibidoBLS", row = 0, message = "El campo no debe estar vacio.", value = json[i].total.recibidoBLS.ToString() }); }
                        else if (validarDecimal(json[i].total.recibidoBLS.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "recibidoBLS", row = 0, message = "El campo debe ser decimal.", value = json[i].total.recibidoBLS.ToString() }); }
                        else if (json[i].total.recibidoBLS < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "recibidoBLS", row = 0, message = "El campo no debe ser negativo.", value = json[i].total.recibidoBLS.ToString() }); }

                        if (string.IsNullOrEmpty(json[i].total.entregaBLS.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "entregaBLS", row = 0, message = "El campo no debe estar vacio.", value = json[i].total.entregaBLS.ToString() }); }
                        else if (validarDecimal(json[i].total.entregaBLS.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "entregaBLS", row = 0, message = "El campo debe ser decimal.", value = json[i].total.entregaBLS.ToString() }); }
                        else if (json[i].total.entregaBLS < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "entregaBLS", row = 0, message = "El campo no debe ser negativo.", value = json[i].total.entregaBLS.ToString() }); }

                        var precarga = await _repository.ConsultarpreCargua(forma, date);
                        var permiso = 0;

                        if (precarga.Rows.Count > 0)
                        {
                            permiso = 1;
                        }

                        if (err.Count == 0)
                        {
                                valDet.Add(new FormaCuadro1ValidadorDetalle()
                                {
                                    BATERIA = json[i].info.bateria,
                                    TANQUE = json[i].info.tanque
                                });

                            val.Add(new FormaCuadro1Validador()
                            {
                                FORMA_CODIGO = "1",
                                OPERADOR = json[i].info.compania,
                                OPERADOR_ID = json[i].info.compania_id,
                                CONTRATO_ID = json[i].info.contrato_id,
                                CONTRATO = json[i].info.contrato,
                                CAMPO_ID = json[i].info.campo_id,
                                CAMPO = json[i].info.campo,
                                ESTRUCTURA_ID = "",
                                BLOQUE_ID = "",
                                FORMACION_ID = "",
                                FORMACION_SET_ID = "",
                                FORMACION = "",
                                MIEMBRO_ID = "",
                                YACIMIENTO_ID = "",
                                ANIO = json[i].info.anio,
                                MES = json[i].info.mes,
                                RECARGA = permiso.ToString(),
                                MODALIDADEXPLOTACION_ID = "",
                                REGISTRO = valDet
                            });

                            var settings = new Newtonsoft.Json.JsonSerializerSettings
                            {
                                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
                            };

                            var jsonVal = val.Serialize(settings);

                            var resValBDP = await FormasService.ValidarFormasMinAsync(new Ppdm.RequestBase { SJson = jsonVal });

                            try
                            {
                                Cuadro1EstrucValCab valreg = new Cuadro1EstrucValCab();

                                valreg = resValBDP.Data.Deserialize<Cuadro1EstrucValCab>();
                                for (int d = 0; d < data; d++)
                                {
                                    if (string.IsNullOrEmpty(valreg?.FORMAS?.FORMA?.REGISTRO?.PDEN_ID_TANQUE) || valreg?.FORMAS?.FORMA?.REGISTRO?.PDEN_ID_TANQUE == "FALSE" || valreg?.FORMAS?.FORMA?.REGISTRO?.PDEN_ID_BATERIA == "FALSE")
                                    {
                                        err.Add(new ErrorFormasStructura() { sheet = i, column = "", row = d, message = "No se identifico el id del tanque: " + json[i].info.tanque + " o el id de la bateria: " + json[i].info.bateria, value = null });
                                    }
                                    else
                                    {

                                        json[i].data[d].PDEN_ID_BATERIA = valreg.FORMAS.FORMA.REGISTRO.PDEN_ID_BATERIA;
                                        json[i].data[d].PDEN_ID_TANQUE = valreg.FORMAS.FORMA.REGISTRO.PDEN_ID_TANQUE;
                                        json[i].info.PDEN_ID_BATERIA = valreg.FORMAS.FORMA.REGISTRO.PDEN_ID_BATERIA;
                                        json[i].info.PDEN_ID_TANQUE = valreg.FORMAS.FORMA.REGISTRO.PDEN_ID_TANQUE;

                                    }
                                }
                            }
                            catch (Exception)
                            {
                                var cuadr1err = resValBDP.Data.Deserialize<rootValCuadro1>();
                                err.Add(new ErrorFormasStructura() { sheet = i, column = "", row = 0, message = cuadr1err.root.element.MENSAJE, value = null });
                            }
                        }
                    }
                }
                else
                {
                    err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = "El archivo se encuentra vacio.", value = "" });
                }

               

                Response.data = json;
                Response.errors = err;
                Response.permisoDeCargue = formafecha.Result;

                response.Code = (int)HttpStatusCode.OK;
                response.Data = Response;

            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                response.Code = (int)HttpStatusCode.BadRequest;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseBase<List<dynamic>>> InsertFormaCuadro1A(Stream fileStream, DateTime date, string forma, string fileJSON, string GetUserId, string nombre, string email, string FileName)
        {
            List<dynamic> result = new List<dynamic>();
            List<ErrorFormasStructura> err = new List<ErrorFormasStructura>();
            AprobacionCarga pr = new AprobacionCarga();

            try
            {
                var json = fileJSON.Deserialize<List<FormaCuadro1A>>();

                try
                {
                    var DataUser = await AdministracionService.UsuarioAprobadorAsync(new Administracion.RequestAprobaciones { NombreForma = forma });
                    var dataService = DataUser.Data.Deserialize<UsuariosRecursosAprobaciones>();

                    var hojas = json.Count;
                    var concreteform = guardarConcreteForm(json, date, forma, GetUserId, 0, nombre);

                    if (concreteform.Rows.Count > 0)
                    {
                        for (int i = 0; i < hojas; i++)
                        {
                            cabeCuadro1 cab = new cabeCuadro1
                            {
                                form_id = Guid.Parse(concreteform.Rows[0]["formid"].ToString()),
                                mes = json[i].info.mes,
                                anio = json[i].info.anio,
                                compania_id = Convert.ToInt32(json[i].info.compania_id),
                                compania = json[i].info.compania,
                                contrato_id = Convert.ToInt32(json[i].info.contrato_id),
                                contrato = json[i].info.contrato,
                                campo_id = Convert.ToInt32(json[i].info.campo_id),
                                campo = json[i].info.campo,
                                lugar = json[i].info.lugar,
                                tanque = json[i].info.tanque,
                                bateria = json[i].info.bateria,
                                usuario = GetUserId,
                                PDEN_ID_TANQUE = json[i].info.PDEN_ID_TANQUE,
                                PDEN_ID_BATERIA = json[i].info.PDEN_ID_BATERIA
                            };

                            var gCabecera = await formaCuadro1A.guardarCabecera(cab);

                            if (gCabecera.Rows.Count > 0)
                            {
                                var detCount = json[i].data.Count;

                                for (int b = 0; b < detCount; b++)
                                {
                                    detaCuadro1 det = new detaCuadro1
                                    {
                                        id_cabecera = Guid.Parse(gCabecera.Rows[0]["id_FormaCuadro1"].ToString()),
                                        form_id = Guid.Parse(concreteform.Rows[0]["formid"].ToString()),
                                        dias = json[i].data[b].dias,
                                        medidaMM = Convert.ToDecimal(json[i].data[b].medidaMM),
                                        aforoBLS = Convert.ToDecimal(json[i].data[b].aforoBLS),
                                        tempF = Convert.ToDecimal(json[i].data[b].tempF),
                                        factorTemp = Convert.ToDecimal(json[i].data[b].factorTemp),
                                        BLS60F = Convert.ToDecimal(json[i].data[b].BLS60F),
                                        BSW = Convert.ToDecimal(json[i].data[b].BSW),
                                        factorBSW = Convert.ToDecimal(json[i].data[b].factorBSW),
                                        CTSH = Convert.ToDecimal(json[i].data[b].CTSH),
                                        tempAmb = Convert.ToDecimal(json[i].data[b].tempAmb),
                                        BLSNetos = Convert.ToDecimal(json[i].data[b].BLSNetos),
                                        transfBLS = Convert.ToDecimal(json[i].data[b].transfBLS),
                                        recibidoBLS = Convert.ToDecimal(json[i].data[b].recibidoBLS),
                                        entregaBLS = Convert.ToDecimal(json[i].data[b].entregaBLS),
                                        API60F = Convert.ToDecimal(json[i].data[b].API60F),
                                        GE = Convert.ToDecimal(json[i].data[b].GE),
                                        netosGE = Convert.ToDecimal(json[i].data[b].netosGE),
                                        salBTB = Convert.ToDecimal(json[i].data[b].salBTB),
                                        usuario = GetUserId,
                                        PDEN_ID_TANQUE = json[i].data[b].PDEN_ID_TANQUE,
                                        PDEN_ID_BATERIA = json[i].data[b].PDEN_ID_BATERIA,

                                        ECP_BALANCE_RECEIVED = json[i].data[b].TrasRecibido,
                                        ECP_BALANCE_SENT =  json[i].data[b].TrasEnvio,
                                        ECP_INTRADIARY_MOV_RECEIVED=  json[i].data[b].MovIntraRecibido,
                                        ECP_INTRADIARY_MOV_SENT =  json[i].data[b].MovIntraEnvio,

                                    };

                                    var detGuardar = await formaCuadro1A.guardarDetalle(det);
                                }


                            }

                            totalFormaCuadro1A tot = new totalFormaCuadro1A
                            {
                                BLS60F = json[i].total.BLS60F,
                                BLSNetos = json[i].total.BLSNetos,
                                recibidoBLS = json[i].total.recibidoBLS,
                                entregaBLS = json[i].total.entregaBLS
                            };

                            var guardarTotal = await formaCuadro1A.guardarTotal(gCabecera.Rows[0]["id_FormaCuadro1"].ToString(), concreteform.Rows[0]["formid"].ToString(), tot, GetUserId);

                        }
                    }
                    string cadena = FileName;
                    FileInfo fi = new FileInfo(cadena);

                    string rutaArch = forma + Convert.ToChar(92) + DateTime.Now.Year.ToString() + Convert.ToChar(92) + DateTime.Now.Month.ToString();

                    var resultC = cargarArchivoForma(fi.Name, fileStream, forma, rutaArch);
                    var forStateid = await _repositoryAdministracion.GetState("En Proceso de Aprobación");

                    pr.ID_Forma = Guid.Parse(concreteform.Rows[0]["formid"].ToString());
                    pr.FechaForma = date;
                    pr.Usuario = GetUserId;
                    pr.UsuarioNombre = nombre;
                    pr.FechaCarga = DateTime.Now;
                    pr.Estado = forStateid;
                    pr.ComparativoAgua = null;
                    pr.ComparativoGas = null;
                    pr.ComparativoCrudo = null;
                    pr.FechaActualizacion = DateTime.Now;
                    pr.UsuarioAprobador = dataService.CodigoUsuario;
                    pr.UsuarioNombreAprobador = dataService.NombreUsuario;
                    pr.FormTypeID = new Guid();
                    pr.FormEntidad = "";
                    pr.FormaName = "Cargue Forma Cuadro 1A";
                    pr.urlForma = rutaArch + Convert.ToChar(92) + fi.Name;
                    pr.operador = json[0].info.compania;
                    pr.contrato = json[0].info.contrato;
                    pr.campo = json[0].info.campo;

                    var guardarAprobacion = _repository.guardarAprobacion(pr);

                    var mensaje = "Se realizó el cargue de la forma relacionada a la " + forma + " en estado de En Proceso de Aprobación ";

                    var notifica = notificarAsync(dataService.NombreUsuario, dataService.Correo, json[0].info.compania, json[0].info.contrato, json[0].info.campo, date, mensaje, forma);

                }
                catch (Exception)
                {
                    err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = "Debe asignarse el usuario aprobador.", value = "" });
                }





                return new ResponseBase<List<dynamic>>(HttpStatusCode.OK, "Se insertaron los registros para su aprobación.", data: result);
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<dynamic>>(HttpStatusCode.BadRequest, ex.Message, data: result);
            }
        }

        public bool validarDecimal(string valor)
        {

            if (decimal.TryParse(valor, out decimal number))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<bool> ValidaFechasPermisos(DateTime fechaForma, DateTime mesOperativo, string operador, string campo, string contrato, string getUserId, string forma)
        {
            DateTime mesActual = DateTime.Now;
            bool state = false;

            try
            {
                if (fechaForma < mesOperativo)
                {
                    var permisos = await _repository.ConsultarPermisoCargua(forma, fechaForma, operador, contrato, getUserId);

                    if (permisos.Rows.Count > 0)
                    {
                        state = false;
                    }
                    else
                    {
                        state = true;
                    }
                }
            }
            catch (Exception)
            {
                state = false;
            }


            return state;
        }

        public DataTable guardarConcreteForm(List<FormaCuadro1A> json, DateTime date, string forma, string usuario, int bandera, string nombre)
        {
            try
            {
                var cab = new BDConcreteForm
                {
                    concreteformid = new Guid(),
                    maincampid = 0,
                    company = json[0].info.compania,
                    contract = json[0].info.contrato,
                    battery = json[0].info.bateria,
                    tank = json[0].info.tanque,
                    month = date.Month,
                    year = date.Year,
                    explotationmodality = "Cargue Archivo Forma " + forma,
                    annotations = "",
                    version = 1,
                    currentstate = new Guid(),
                    generationflag = 0,
                    campid = Convert.ToDecimal(json[0].info.compania_id),
                    pdenid = json[0].info.contrato_id,
                    formid = new Guid(),
                    generationjobid = 0,
                    iqistatus = 0,
                    usersigning = usuario,
                    minrepsigning = "",
                    formname = "Forma Cuadro 1A"
                };

                DataTable guardarCabecera = _repository.GuardarCabecera(cab);

                return guardarCabecera;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> cargarArchivoForma(string fileName, Stream file, string forma, string rutaArchivoUrl)
        {
            string nombreArchivo;
            try
            {

                var upload = await CommonService.UploadFileAsync(new Commons.FileSystemUploadFileOptions
                {
                    FileName = fileName,
                    FileData = ConvertTypes.ConvertToBase64(file),
                    Container = "formas",
                    DestinationDirectory = new Commons.FileSystemItemInfo
                    {
                        Path = $"" + rutaArchivoUrl
                    }
                });

                if (upload.Code == 200)
                {
                    nombreArchivo = fileName;
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> notificarAsync(string usuario, string correo, string operador, string contrato, string campo, DateTime fecha, string mensaje, string forma)
        {
            try
            {
                string asunto = "Notificación de carga " + forma;
                MailForma9 mailForma = new MailForma9();

                string view = mailForma.GetView(asunto, usuario, operador, contrato, campo, fecha.Year.ToString() + "/" + fecha.Month.ToString(), mensaje);
                EmailInfo Email = new EmailInfo()
                {
                    To = new List<string>() { correo },
                    Subject = asunto,
                    Body = view,
                    Styles = mailForma.GetHeaderStyle()
                };

                var response = await SendMailService.SendEmailAsync(Email);

                return response.Data;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
