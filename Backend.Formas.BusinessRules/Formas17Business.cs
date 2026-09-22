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
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules
{
    public class Formas17Business : IFormas17Business
    {
        public readonly IForma17Repository forma17;
        private readonly IFormaValidate validate;
        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient FormasService;
        private readonly RequestFormas<Forma17CR> Response = new RequestFormas<Forma17CR>();
        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;
        private readonly IForma30SEERepository repository;
        private readonly Administracion.AdmGrpc.AdmGrpcClient AdministracionService;
        private readonly Commons.CommonGrpc.CommonGrpcClient CommonService;
        private readonly IAdministracionFormas _repositoryAdministracion;
        private readonly Backend.Formas.Utilities.SendMail.ISendMailService SendMailService;

        public Formas17Business(IForma17Repository _forma17
                                , IFormaValidate _validate
                                , Ppdm.PpdmGrpc.PpdmGrpcClient _FormasService
                                , Utilities.Telemetry.ITelemetryException _TelemetryException
                                , IForma30SEERepository _repository
                                , Administracion.AdmGrpc.AdmGrpcClient _AdministracionService
                                , Commons.CommonGrpc.CommonGrpcClient _CommonService
                                , IAdministracionFormas repositoryAdministracion
                                , Backend.Formas.Utilities.SendMail.ISendMailService _SendMailService)
        {
            forma17 = _forma17;
            validate = _validate;
            FormasService = _FormasService;
            TelemetryException = _TelemetryException;
            repository = _repository;
            AdministracionService = _AdministracionService;
            CommonService = _CommonService;
            _repositoryAdministracion = repositoryAdministracion;
            SendMailService = _SendMailService;
        }

        public async Task<ResponseBase<RequestFormas<Forma17CR>>> ValidateForm17CR(DateTime date, string forma, string fileJson, string usuario)
        {
            var json = fileJson.Deserialize<List<Forma17CR>>();

            var response = new ResponseBase<RequestFormas<Forma17CR>>();
            List<ErrorFormasStructura> err = new List<ErrorFormasStructura>();
            List<CabValForma17CR> val = new List<CabValForma17CR>();
            List<DetValForma17CR> valDet = new List<DetValForma17CR>();


            try
            {

                var hojas = json.Count;
                var formafecha = ValidaFechasPermisos(date, DateTime.Now, json[0].info.operador, "", json[0].info.contrato, usuario, forma);

                if (hojas > 0)
                {
                    for (int i = 0; i < hojas; i++)
                    {

                        var valOperador = new ResponseBase<List<dynamic>>
                        {
                            Data = await validate.ValidaOperador(json[i].info.operador)
                        };

                        if (valOperador.Data?.Count == 0 || valOperador.Data == null)
                        {
                            err.Add(new ErrorFormasStructura() { sheet = i, column = "operador", row = 0, message = "El nombre del operador no existe.", value = json[i].info.operador });
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

                        var validaRegistrosArchivo = await validate.ValidaCampoContratoOperador(json[i].info.operador, json[i].info.campo, json[i].info.contrato);

                        if (validaRegistrosArchivo?.Count == 0 || validaRegistrosArchivo == null)
                        {

                            err.Add(new ErrorFormasStructura() { sheet = i, column = "", row = 0, message = "Al validar el nombre del operador y del campo, estos no coinciden. Compañia: " + json[i].info.operador + "Contrato: " + json[i].info.contrato + ", campo: " + json[i].info.campo, value = "" });

                        }
                        else
                        {
                            json[i].info.campo_id = ConvertTypes.ConverDataDynamic(validaRegistrosArchivo, "CAMPO_ID");
                            json[i].info.operador_id = ConvertTypes.ConverDataDynamic(validaRegistrosArchivo, "OPERADOR_ID");
                            json[i].info.contrato_id = ConvertTypes.ConverDataDynamic(validaRegistrosArchivo, "CONTRATO_ID");
                        }

                        var data = json[i].data.Count;

                        if (data > 0)
                        {
                            for (int d = 0; d < data; d++)
                            {
                                if (string.IsNullOrEmpty(json[i].data[d].diasEnElMes.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "diasEnElMes", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].diasEnElMes.ToString() }); }
                                else if (validarDecimal(json[i].data[d].diasEnElMes.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "diasEnElMes", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].diasEnElMes.ToString() }); }
                                else if (json[i].data[d].diasEnElMes < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "diasEnElMes", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].diasEnElMes.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].diasAcumulados.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "diasAcumulados", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].diasAcumulados.ToString() }); }
                                else if (validarDecimal(json[i].data[d].diasAcumulados.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "diasAcumulados", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].diasAcumulados.ToString() }); }
                                else if (json[i].data[d].diasAcumulados < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "diasAcumulados", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].diasAcumulados.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].produccionGasMCPDiaria.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionGasMCPDiaria", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].produccionGasMCPDiaria.ToString() }); }
                                else if (validarDecimal(json[i].data[d].produccionGasMCPDiaria.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionGasMCPDiaria", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].produccionGasMCPDiaria.ToString() }); }
                                else if (json[i].data[d].produccionGasMCPDiaria < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionGasMCPDiaria", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].produccionGasMCPDiaria.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].produccionGasMCPMensual.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionGasMCPMensual", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].produccionGasMCPMensual.ToString() }); }
                                else if (validarDecimal(json[i].data[d].produccionGasMCPMensual.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionGasMCPMensual", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].produccionGasMCPMensual.ToString() }); }
                                else if (json[i].data[d].produccionGasMCPMensual < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionGasMCPMensual", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].produccionGasMCPMensual.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].produccionGasMCPAcumulada.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionGasMCPAcumulada", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].produccionGasMCPAcumulada.ToString() }); }
                                else if (validarDecimal(json[i].data[d].produccionGasMCPAcumulada.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionGasMCPAcumulada", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].produccionGasMCPAcumulada.ToString() }); }
                                else if (json[i].data[d].produccionGasMCPAcumulada < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionGasMCPAcumulada", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].produccionGasMCPAcumulada.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].produccionAguaMensual.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionAguaMensual", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].produccionAguaMensual.ToString() }); }
                                else if (validarDecimal(json[i].data[d].produccionAguaMensual.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionAguaMensual", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].produccionAguaMensual.ToString() }); }
                                else if (json[i].data[d].produccionAguaMensual < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionAguaMensual", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].produccionAguaMensual.ToString() }); }

                                if (string.IsNullOrEmpty(json[i].data[d].produccionAguaAcumulada.ToString())) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionAguaAcumulada", row = d, message = "El campo no debe estar vacio.", value = json[i].data[d].produccionAguaAcumulada.ToString() }); }
                                else if (validarDecimal(json[i].data[d].produccionAguaAcumulada.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionAguaAcumulada", row = d, message = "El campo debe ser decimal.", value = json[i].data[d].produccionAguaAcumulada.ToString() }); }
                                else if (json[i].data[d].produccionAguaAcumulada < 0) { err.Add(new ErrorFormasStructura() { sheet = i, column = "produccionAguaAcumulada", row = d, message = "El campo no debe ser negativo.", value = json[i].data[d].produccionAguaAcumulada.ToString() }); }

                            }
                        }
                        else
                        {
                            err.Add(new ErrorFormasStructura() { sheet = 1, column = "", row = 0, message = "Los registros de la Hoja se encuentran vacios.", value = "" });
                        }

                        var precarga = await repository.ConsultarpreCargua(forma, date);
                        var permiso = 0;

                        if (precarga.Rows.Count > 0)
                        {
                            permiso = 1;
                        }

                        if (err.Count == 0)
                        {

                            for (int d = 0; d < data; d++)
                            {
                                valDet.Add(new DetValForma17CR()
                                {
                                    POZO = json[i].data[d].pozo,
                                    FORMACION = json[i].info.formacion
                                });
                            }

                            val.Add(new CabValForma17CR()
                            {
                                FORMA_CODIGO = "17",
                                OPERADOR = json[i].info.operador,
                                OPERADOR_ID = json[i].info.operador_id,
                                CONTRATO_ID = json[i].info.contrato_id,
                                CONTRATO = json[i].info.contrato,
                                CAMPO_ID = json[i].info.campo_id,
                                CAMPO = json[i].info.campo,
                                ESTRUCTURA_ID = "",
                                BLOQUE_ID = "",
                                FORMACION_ID = "",
                                FORMACION_SET_ID = "",
                                FORMACION = json[i].info.formacion,
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

                            //var valConForm17 = new CabForma17Val();

                            var resValBDP = await FormasService.ValidarFormasMinAsync(new Ppdm.RequestBase { SJson = jsonVal });

                            try
                            {

                                if (data == 1)
                                {
                                    var valConForm17 = resValBDP.Data.Deserialize<CabForma17Val>();

                                    for (int d = 0; d < data; d++)
                                    {

                                        if (string.IsNullOrEmpty(valConForm17?.FORMAS?.FORMA?.REGISTRO.PDEN_ID) || valConForm17.FORMAS.FORMA.REGISTRO.PDEN_ID == "FALSE")
                                        {
                                            err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = "No se identifico el id del pozo: " + json[i].data[d].pozo, value = "" });
                                        }
                                        else
                                        {
                                            json[i].data[d].pden_id = valConForm17.FORMAS.FORMA.REGISTRO.PDEN_ID;
                                        }
                                    }
                                }
                                else
                                {
                                    var valConForm17 = resValBDP.Data.Deserialize<CabForma17Valvarios>();

                                    for (int d = 0; d < data; d++)
                                    {

                                        if (string.IsNullOrEmpty(valConForm17?.FORMAS?.FORMA?.REGISTRO[d].PDEN_ID) || valConForm17.FORMAS.FORMA.REGISTRO[d].PDEN_ID == "FALSE")
                                        {
                                            err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = "No se identifico el id del pozo: " + json[i].data[d].pozo, value = "" });
                                        }
                                        else
                                        {
                                            for (int m = 0; m < data; m++)
                                            {
                                                if (json[i].data[m].pozo == valConForm17.FORMAS.FORMA.REGISTRO[d].POZO)
                                                {
                                                    json[i].data[d].pden_id = valConForm17.FORMAS.FORMA.REGISTRO[d].PDEN_ID;
                                                }
                                            }

                                        }
                                    }
                                }


                            }
                            catch (Exception)
                            {
                                var errval17 = resValBDP.Data.Deserialize<rootVal17>();
                                err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = errval17.root.element.MENSAJE, value = "" });
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

        public async Task<ResponseBase<List<dynamic>>> InsertForma17(Stream fileStream, DateTime date, string forma, string fileJSON, string GetUserId, string nombre, string email, string FileName)
        {
            List<dynamic> result = new List<dynamic>();
            List<ErrorFormasStructura> err = new List<ErrorFormasStructura>();
            AprobacionCarga pr = new AprobacionCarga();

            try
            {
                var json = fileJSON.Deserialize<List<Forma17CR>>();

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
                            InfoForma17CR cab = new InfoForma17CR
                            {
                                concesion = json[i].info.concesion,
                                operador_id = json[i].info.operador_id,
                                operador = json[i].info.operador,
                                contrato_id = json[i].info.contrato_id,
                                contrato = json[i].info.contrato,
                                campo_id = json[i].info.campo_id,
                                campo = json[i].info.campo,
                                estructura = json[i].info.estructura,
                                formacion = json[i].info.formacion,
                                bloque = json[i].info.bloque,
                                yacimiento = json[i].info.yacimiento,
                                mes = json[i].info.mes,
                                anio = json[i].info.anio
                            };

                            var gCabecera = await forma17.guardarCabecera(concreteform.Rows[0]["formid"].ToString(), cab, GetUserId);

                            if (gCabecera.Rows.Count > 0)
                            {
                                var detCount = json[i].data.Count;

                                for (int b = 0; b < detCount; b++)
                                {
                                    dataForma17CR det = new dataForma17CR
                                    {
                                        pozo = json[i].data[b].pozo,
                                        diasEnElMes = json[i].data[b].diasEnElMes,
                                        diasAcumulados = json[i].data[b].diasAcumulados,
                                        produccionGasMCPDiaria = json[i].data[b].produccionGasMCPDiaria,
                                        produccionGasMCPMensual = json[i].data[b].produccionGasMCPMensual,
                                        produccionGasMCPAcumulada = json[i].data[b].produccionGasMCPAcumulada,
                                        produccionAguaMensual = json[i].data[b].produccionAguaMensual,
                                        produccionAguaAcumulada = json[i].data[b].produccionAguaAcumulada,
                                        estadoPozosFinalMes = json[i].data[b].estadoPozosFinalMes,
                                        pden_id = json[i].data[b].pden_id
                                    };

                                    var detGuardar = await forma17.guardarDetalle(concreteform.Rows[0]["formid"].ToString(), gCabecera.Rows[0]["id_detalle"].ToString(), det, GetUserId);
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
                    pr.FormaName = "Cargue Forma 17CR";
                    pr.urlForma = rutaArch + Convert.ToChar(92) + fi.Name;
                    pr.operador = json[0].info.operador;
                    pr.contrato = json[0].info.contrato;
                    pr.campo = json[0].info.campo;

                    Entities.DAO.Aprobacioncarga currentApprove = await _repositoryAdministracion.GetaFormAprobacion(pr.FormaName, pr.FechaForma, pr.operador, pr.campo, pr.contrato);
                    if (currentApprove?.Id != null && currentApprove.Formstate.Name != "Aprobada")
                        await _repositoryAdministracion.Delete(currentApprove.IdForma.Value);

                    _ = repository.guardarAprobacion(pr);

                    var mensaje = "Se realizó el cargue de la forma relacionada a la  " + forma + " en estado de En Proceso de Aprobación ";
                    var notifica = notificarAsync(dataService.NombreUsuario, dataService.Correo, json[0].info.operador, json[0].info.contrato, json[0].info.campo, date, mensaje, forma);
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

        public DataTable guardarConcreteForm(List<Forma17CR> json, DateTime date, string forma, string usuario, int bandera, string nombre)
        {
            try
            {
                var cab = new BDConcreteForm
                {
                    concreteformid = new Guid(),
                    maincampid = 0,
                    company = json[0].info.operador,
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
                    campid = Convert.ToDecimal(json[0].info.operador_id),
                    pdenid = json[0].info.contrato_id,
                    formid = new Guid(),
                    generationjobid = 0,
                    iqistatus = 0,
                    usersigning = usuario,
                    minrepsigning = "",
                    formname = "Forma 17CR"
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
