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
    public class Formas15CRBusiness : IFormas15CRBusiness
    {
        private readonly IFormaValidate validate;
        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient FormasService;
        private readonly RequestFormas<Forma15CR> Response = new RequestFormas<Forma15CR>();
        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;
        private readonly IForma30SEERepository repository;
        private readonly Administracion.AdmGrpc.AdmGrpcClient AdministracionService;
        private readonly Commons.CommonGrpc.CommonGrpcClient CommonService;
        private readonly IAdministracionFormas _repositoryAdministracion;
        private readonly Backend.Formas.Utilities.SendMail.ISendMailService SendMailService;
        private readonly IForma15CRRepository forma15CR;

        public Formas15CRBusiness(IFormaValidate _validate
                                , Ppdm.PpdmGrpc.PpdmGrpcClient _FormasService
                                , Utilities.Telemetry.ITelemetryException _TelemetryException
                                , IForma30SEERepository _repository
                                , Administracion.AdmGrpc.AdmGrpcClient _AdministracionService
                                , Commons.CommonGrpc.CommonGrpcClient _CommonService
                                , IAdministracionFormas repositoryAdministracion
                                , Backend.Formas.Utilities.SendMail.ISendMailService _SendMailService
                                , IForma15CRRepository _forma15CR)
        {
            validate = _validate;
            FormasService = _FormasService;
            TelemetryException = _TelemetryException;
            repository = _repository;
            AdministracionService = _AdministracionService;
            CommonService = _CommonService;
            _repositoryAdministracion = repositoryAdministracion;
            SendMailService = _SendMailService;
            forma15CR = _forma15CR;
        }

        public async Task<ResponseBase<RequestFormas<Forma15CR>>> ValidateForm15CR(DateTime date, string forma, string fileJson, string usuario)
        {
            var json = fileJson.Deserialize<List<Forma15CR>>();

            var response = new ResponseBase<RequestFormas<Forma15CR>>();
            List<ErrorFormasStructura> err = new List<ErrorFormasStructura>();
            List<ValCabForma15CR> val = new List<ValCabForma15CR>();
            List<DetValForma15CR> valDet = new List<DetValForma15CR>();


            try
            {

                var hojas = json.Count;


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
                            err.Add(new ErrorFormasStructura() { sheet = i, column = "Compania", row = 0, message = "El nombre de la compañia no existe.", value = json[i].info.compania });
                        }

                        var valCampo = new ResponseBase<List<dynamic>>
                        {
                            Data = await validate.ValidaCampo(json[i].info.campo)
                        };

                        if (valCampo.Data?.Count == 0 || valCampo.Data == null)
                        {
                            err.Add(new ErrorFormasStructura() { sheet = i, column = "Campo", row = 0, message = "El nombre del Campo no existe.", value = json[i].info.campo });
                        }

                        var valContrato = new ResponseBase<List<dynamic>>
                        {
                            Data = await validate.ValidaContrato(json[i].info.contrato)
                        };

                        if (valCampo.Data?.Count == 0 || valCampo.Data == null)
                        {
                            err.Add(new ErrorFormasStructura() { sheet = i, column = "Contrato", row = 0, message = "El nombre del contrato no existe.", value = json[i].info.contrato });
                        }

                        var validaRegistrosArchivo = await validate.ValidaCampoContratoOperador(json[i].info.compania, json[i].info.campo, json[i].info.contrato);

                        if (validaRegistrosArchivo?.Count == 0 || validaRegistrosArchivo == null)
                        {

                            err.Add(new ErrorFormasStructura() { sheet = i, column = "", row = 0, message = "Al validar el nombre de la compañia, contrato y del campo, estos no coinciden. Compañia: " + json[i].info.compania + " Contrato: " + json[i].info.contrato + ", campo: " + json[i].info.campo, value = "" });

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
                                if (string.IsNullOrEmpty(json[i].data[d].valInyecFormacionProductora.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecFormacionProductora", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].valInyecFormacionProductora.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].valInyecPresionInyeccion.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecPresionInyeccion", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].valInyecPresionInyeccion.ToString() }); }
                                else if (validarDecimal(json[i].data[d].valInyecPresionInyeccion.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecPresionInyeccion", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].valInyecPresionInyeccion.ToString() }); }
                                else if (json[i].data[d].valInyecPresionInyeccion < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecPresionInyeccion", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].valInyecPresionInyeccion.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].valInyecCiclo.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecCiclo", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].valInyecCiclo.ToString() }); }
                                else if (validarDecimal(json[i].data[d].valInyecCiclo.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecCiclo", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].valInyecCiclo.ToString() }); }
                                else if (json[i].data[d].valInyecCiclo < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecCiclo", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].valInyecCiclo.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].valInyecDiasMes.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecDiasMes", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].valInyecDiasMes.ToString() }); }
                                else if (validarDecimal(json[i].data[d].valInyecDiasMes.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecDiasMes", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].valInyecDiasMes.ToString() }); }
                                else if (json[i].data[d].valInyecDiasMes < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecDiasMes", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].valInyecDiasMes.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].valInyecDiasAcumulados.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecDiasAcumulados", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].valInyecDiasAcumulados.ToString() }); }
                                else if (validarDecimal(json[i].data[d].valInyecDiasAcumulados.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecDiasAcumulados", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].valInyecDiasAcumulados.ToString() }); }
                                else if (json[i].data[d].valInyecDiasAcumulados < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecDiasAcumulados", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].valInyecDiasAcumulados.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].valInyecLibrasMes.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecLibrasMes", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].valInyecLibrasMes.ToString() }); }
                                else if (validarDecimal(json[i].data[d].valInyecLibrasMes.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecLibrasMes", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].valInyecLibrasMes.ToString() }); }
                                else if (json[i].data[d].valInyecLibrasMes < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecLibrasMes", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].valInyecLibrasMes.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].valInyecLibrasAcumulados.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecLibrasAcumulados", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].valInyecLibrasAcumulados.ToString() }); }
                                else if (validarDecimal(json[i].data[d].valInyecLibrasAcumulados.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecLibrasAcumulados", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].valInyecLibrasAcumulados.ToString() }); }
                                else if (json[i].data[d].valInyecLibrasAcumulados < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecLibrasAcumulados", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].valInyecLibrasAcumulados.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].valInyecBTUMes.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecBTUMes", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].valInyecBTUMes.ToString() }); }
                                else if (validarDecimal(json[i].data[d].valInyecBTUMes.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecBTUMes", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].valInyecBTUMes.ToString() }); }
                                else if (json[i].data[d].valInyecBTUMes < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecBTUMes", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].valInyecBTUMes.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].valInyecBTUAcumulados.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecBTUAcumulados", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].valInyecBTUAcumulados.ToString() }); }
                                else if (validarDecimal(json[i].data[d].valInyecBTUAcumulados.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecBTUAcumulados", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].valInyecBTUAcumulados.ToString() }); }
                                else if (json[i].data[d].valInyecBTUAcumulados < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "valInyecBTUAcumulados", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].valInyecBTUAcumulados.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].produccionPetroleoBlsNetosMensual.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionPetroleoBlsNetosMensual", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].produccionPetroleoBlsNetosMensual.ToString() }); }
                                else if (validarDecimal(json[i].data[d].produccionPetroleoBlsNetosMensual.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionPetroleoBlsNetosMensual", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].produccionPetroleoBlsNetosMensual.ToString() }); }
                                else if (json[i].data[d].produccionPetroleoBlsNetosMensual < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionPetroleoBlsNetosMensual", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].produccionPetroleoBlsNetosMensual.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].produccionPetroleoBlsNetosAcumulado.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionPetroleoBlsNetosAcumulado", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].produccionPetroleoBlsNetosAcumulado.ToString() }); }
                                else if (validarDecimal(json[i].data[d].produccionPetroleoBlsNetosAcumulado.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionPetroleoBlsNetosAcumulado", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].produccionPetroleoBlsNetosAcumulado.ToString() }); }
                                else if (json[i].data[d].produccionPetroleoBlsNetosAcumulado < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionPetroleoBlsNetosAcumulado", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].produccionPetroleoBlsNetosAcumulado.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].produccionAguaBlsMensual.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionAguaBlsMensual", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].produccionAguaBlsMensual.ToString() }); }
                                else if (validarDecimal(json[i].data[d].produccionAguaBlsMensual.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionAguaBlsMensual", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].produccionAguaBlsMensual.ToString() }); }
                                else if (json[i].data[d].produccionAguaBlsMensual < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionAguaBlsMensual", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].produccionAguaBlsMensual.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].produccionAguaBlsAcumulado.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionAguaBlsAcumulado", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].produccionAguaBlsAcumulado.ToString() }); }
                                else if (validarDecimal(json[i].data[d].produccionAguaBlsAcumulado.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionAguaBlsAcumulado", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].produccionAguaBlsAcumulado.ToString() }); }
                                else if (json[i].data[d].produccionAguaBlsAcumulado < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionAguaBlsAcumulado", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].produccionAguaBlsAcumulado.ToString() }); }

                            }
                        }
                        else
                        {
                            err.Add(new ErrorFormasStructura() { sheet = 1, column = "", row = 0, message = "Los registros de la Hoja se encuentran vacios.", value = "" });
                        }

                    }
                }
                else
                {
                    err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = "El archivo se encuentra vacio.", value = "" });
                }

                var formafecha = ValidaFechasPermisos(date, DateTime.Now, json[0].info.compania, "", json[0].info.contrato, usuario, forma);
                var precarga = await repository.ConsultarpreCargua(forma, date);
                var permiso = 0;

                if (precarga.Rows.Count > 0)
                {
                    permiso = 1;
                }

                if (err.Count == 0)
                {
                    for (int i = 0; i < hojas; i++)
                    {
                        var candet = json[i].data.Count;

                        for (int b = 0; b < candet; b++)
                        {
                            valDet.Add(new DetValForma15CR()
                            {
                                POZO = json[i].data[b].valInyecPozo,
                                FORMACION = json[i].data[b].valInyecFormacionProductora,
                                METODO_PRODUCCION = json[i].data[b].valInyecMetodoProduccion
                            });

                            val.Add(new ValCabForma15CR()
                            {
                                FORMA_CODIGO = "15",
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
                                var res = resValBDP.Data.Deserialize<valFormasForm15CRCab>();

                                if (string.IsNullOrEmpty(res?.FORMAS?.FORMA?.REGISTRO?.PDEN_ID) || res.FORMAS.FORMA.REGISTRO.PDEN_ID == "FALSE")
                                {
                                    err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = "No se encontro el Pozo : " + json[i].data[b].valInyecPozo, value = "" });

                                }
                                else
                                {
                                    json[i].info.PDEN_ID = res.FORMAS.FORMA.REGISTRO.PDEN_ID;
                                    json[i].data[b].PDEN_ID = res.FORMAS.FORMA.REGISTRO.PDEN_ID;
                                }
                            }
                            catch (Exception)
                            {
                                var errval15 = resValBDP.Data.Deserialize<rootVal15>();
                                err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = errval15.root.element[0].MENSAJE, value = "" });
                            }
                        }
                    }
                }

                Response.data = json;
                Response.errors = err;
                Response.permisoDeCargue = formafecha.Result;

                response.Code = (int)HttpStatusCode.OK;
                response.Data = Response;

            }
            catch (Exception ex)
            {
                err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = "En este momento no se encuentra disponible el validador de la BDP, por favor intente mas tarde. error: (" + ex.Message + ")", value = "" });

                TelemetryException.RegisterException(ex);
                response.Code = (int)HttpStatusCode.OK;
                response.Message = "En este momento no se encuentra disponible el validador de la BDP, por favor intente mas tarde. error: (" +  ex.Message + ")";
            }
            return response;
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
                    var permisos = await repository.ConsultarPermisoCargua(forma, fechaForma, operador, contrato, getUserId);

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

        public async Task<ResponseBase<List<dynamic>>> InsertForma15(Stream fileStream, DateTime date, string forma, string fileJSON, string GetUserId, string nombre, string email, string FileName)
        {
            List<dynamic> result = new List<dynamic>();
            List<ErrorFormasStructura> err = new List<ErrorFormasStructura>();
            AprobacionCarga pr = new AprobacionCarga();

            try
            {
                var json = fileJSON.Deserialize<List<Forma15CR>>();

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
                            infoForma15CR cab = new infoForma15CR
                            {
                                compania_id = json[i].info.compania_id,
                                compania = json[i].info.compania,
                                concesion = json[i].info.concesion,
                                contrato_id = json[i].info.contrato_id,
                                contrato = json[i].info.contrato,
                                campo_id = json[i].info.campo_id,
                                campo = json[i].info.campo,
                                mes = json[i].info.mes,
                                anio = json[i].info.anio,
                                row_created_by = GetUserId,
                                row_created_date = DateTime.Now,
                                row_changed_by = GetUserId,
                                row_changed_date = DateTime.Now
                            };

                            var gCabecera = await forma15CR.guardarCabecera(concreteform.Rows[0]["formid"].ToString(), cab, GetUserId);

                            if (gCabecera.Rows.Count > 0)
                            {
                                var detCount = json[i].data.Count;

                                for (int b = 0; b < detCount; b++)
                                {
                                    dataForma15CR det = new dataForma15CR
                                    {
                                        valInyecPozo = json[i].data[b].valInyecPozo,
                                        valInyecFormacionProductora = json[i].data[b].valInyecFormacionProductora,
                                        valInyecMetodoProduccion = json[i].data[b].valInyecMetodoProduccion,
                                        valInyecPresionInyeccion = json[i].data[b].valInyecPresionInyeccion,
                                        valInyecCiclo = json[i].data[b].valInyecCiclo,
                                        valInyecDiasMes = json[i].data[b].valInyecDiasMes,
                                        valInyecDiasAcumulados = json[i].data[b].valInyecDiasAcumulados,
                                        valInyecLibrasMes = json[i].data[b].valInyecLibrasMes,
                                        valInyecLibrasAcumulados = json[i].data[b].valInyecLibrasAcumulados,
                                        valInyecBTUMes = json[i].data[b].valInyecBTUMes,
                                        valInyecBTUAcumulados = json[i].data[b].valInyecBTUAcumulados,
                                        valInyecCalidadVapor = json[i].data[b].valInyecCalidadVapor,
                                        produccionPetroleoBlsNetosMensual = json[i].data[b].produccionPetroleoBlsNetosMensual,
                                        produccionPetroleoBlsNetosAcumulado = json[i].data[b].produccionPetroleoBlsNetosAcumulado,
                                        produccionAguaBlsMensual = json[i].data[b].produccionAguaBlsMensual,
                                        produccionAguaBlsAcumulado = json[i].data[b].produccionAguaBlsAcumulado,
                                        row_created_by = GetUserId,
                                        row_created_date = DateTime.Now,
                                        row_changed_by = GetUserId,
                                        row_changed_date = DateTime.Now,
                                        PDEN_ID = json[i].data[b].PDEN_ID
                                    };

                                    var detGuardar = await forma15CR.guardarDetalle(concreteform.Rows[0]["formid"].ToString(), gCabecera.Rows[0]["id_detalle"].ToString(), det, GetUserId);
                                }


                            }
                        }
                    }
                    string cadena = FileName;
                    FileInfo fi = new FileInfo(cadena);

                    string rutaArch = forma + Convert.ToChar(92) + DateTime.Now.Year.ToString() + Convert.ToChar(92) + DateTime.Now.Month.ToString();// + fi.Name;

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
                    pr.FormaName = forma;
                    pr.urlForma = rutaArch + Convert.ToChar(92) + fi.Name;
                    pr.operador = json[0].info.compania;
                    pr.contrato = json[0].info.contrato;
                    pr.campo = json[0].info.campo;

                    var guardarAprobacion = repository.guardarAprobacion(pr);

                    var mensaje = "Se realizó el cargue de la forma relacionada a la  " + forma + " en estado de En Proceso de Aprobación ";

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

        public DataTable guardarConcreteForm(List<Forma15CR> json, DateTime date, string forma, string usuario, int bandera, string nombre)
        {
            try
            {
                var cab = new BDConcreteForm
                {
                    concreteformid = new Guid(),
                    maincampid = 0,
                    company = json[0].info.compania,
                    contract = json[0].info.contrato,
                    battery = "",
                    tank = "",
                    month = date.Month,
                    year = date.Year,
                    explotationmodality = "Cargue Archivo " + forma,
                    annotations = "",
                    version = 1,
                    currentstate = new Guid(),
                    generationflag = 0,
                    campid = Convert.ToDecimal(json[0].info.compania_id),
                    pdenid = json[0].info.PDEN_ID,
                    formid = new Guid(),
                    generationjobid = 0,
                    iqistatus = 0,
                    usersigning = usuario,
                    minrepsigning = "",
                    formname = forma
                };

                DataTable guardarCabecera = repository.GuardarCabecera(cab);

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
