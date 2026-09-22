using Backend.Formas.BusinessRules.Middle;
using Backend.Formas.BusinessRules.ViewSendMail;
using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTO;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.DTO.Validaciones;
using Backend.Formas.Entities.DTOI;
using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Entities.ModelsAdm;
using Backend.Formas.Entities.Responses;
using Backend.Formas.Entities.Services;
using Backend.Formas.Utilities;
using Backend.Formas.Utilities.SendMail;
using Backend.Formas.Utilities.Telemetry;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules
{
    public class FormaC7Business : IFormaC7Business
    {

        private readonly ITelemetryException _telemetryException;
        private readonly IAdministracionFormas _formaOficial;
        private readonly IFormaValidate _formasValidate;
        private readonly IFormaC7Repository _formaC7Repository;

        private readonly Administracion.AdmGrpc.AdmGrpcClient _administracionService;
        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient _formasService;
        private readonly Commons.CommonGrpc.CommonGrpcClient _commonService;

        private readonly CabeceraFormas<DetalleF7> JsonValidacion = new CabeceraFormas<DetalleF7>();
        private readonly List<ErrorFormasStructura> Errors = new List<ErrorFormasStructura>();
        private readonly RequestFormas<RequestFormaC7> Response = new RequestFormas<RequestFormaC7>();

        private HeaderFormaC7 Header = new HeaderFormaC7();
        private readonly CultureInfo cultureInfo = new CultureInfo("es-co");
        private readonly Dictionary<string, string> pdenIds = new Dictionary<string, string>();
        private readonly ISendMailService _sendMailService;
        private readonly IBaseRepository<Concreteform> _repositoryConcreform;

        public FormaC7Business(ITelemetryException telemetryException, IAdministracionFormas administracionFormas, IFormaValidate formaValidate, IFormaC7Repository formaC7Repository,
                    Administracion.AdmGrpc.AdmGrpcClient admGrpcClient, Ppdm.PpdmGrpc.PpdmGrpcClient ppdmGrpcClient, Commons.CommonGrpc.CommonGrpcClient commonGrpcClient,
                    ISendMailService sendMailService, IBaseRepository<Concreteform> repositoryConcretForm)
        {
            _telemetryException = telemetryException;
            _formaOficial = administracionFormas;
            _formasValidate = formaValidate;
            _formaC7Repository = formaC7Repository;
            _administracionService = admGrpcClient;
            _formasService = ppdmGrpcClient;
            _commonService = commonGrpcClient;
            _sendMailService = sendMailService;
            _repositoryConcreform = repositoryConcretForm;
            JsonValidacion.RECARGAR = "0";
        }

        public async Task<ResponseBase<dynamic>> Create(List<ResponseFormaC7> data, string GetUserId, string userName)
        {
            var response = new ResponseBase<dynamic>();
            try
            {
                for (var i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    HeaderFormaC7 headerForma = item.Info;
                    List<BodyFormaC7> dataForma = item.Data;
                    TotalesFormaC7 totalesForma = item.Total;
                    var fechaForma = DateTime.Parse($"{headerForma.Mes}/{headerForma.Anio}", cultureInfo);
                    await ValidaFechasPermisos(fechaForma: fechaForma, operador: headerForma.Operador, campo: headerForma.Campo, contrato: headerForma.ContratoAsociacion, getUserId: GetUserId, true);

                    var info2 = dataForma[0];
                    FormC7 formC7 = new FormC7()
                    {
                        Formc7id = Guid.NewGuid(),
                        Formation = "",
                        Block = headerForma.Bloque,
                        Oilfield = headerForma.Campo,
                        Structure = headerForma.Estructura,
                        Member = headerForma.Operador,
                        Operadorid = headerForma.CompaniaId,
                        Operador = headerForma.Operador,
                        Contractid = headerForma.ContratoId,
                        Contract = headerForma.ContratoAsociacion,
                        Campoid = headerForma.CampoId,
                        Campo = headerForma.Campo,
                        row_created_by = GetUserId,
                        row_created_date = DateTime.Now,
                        row_changed_by = GetUserId,
                        row_changed_date = DateTime.Now

                    };
                    await _formaC7Repository.SaveFormC7(formC7);
                    foreach (BodyFormaC7 formaC7 in dataForma)
                    {
                        FormC7Detail formC7Detail = new FormC7Detail()
                        {
                            Formc7Detailid = Guid.NewGuid(),
                            Formid = formC7.Formc7id,
                            Fieldid = 0,
                            Oilproduction = decimal.Parse(formaC7.ProduccionBbls),
                            Gasproduction = 0,
                            Endwells = decimal.Parse(formaC7.TotalPozosTerminadosOficialmente),
                            Activewells = decimal.Parse(formaC7.PozosProductoresActivosTotal),
                            Inactivewells = decimal.Parse(formaC7.PozosProductoresInactivosMiscelaneos),
                            Abandonedwells = decimal.Parse(formaC7.PozosTaponadosAbandonados),
                            Unfinishedwells = decimal.Parse(formaC7.PozosSuspendidosSecosSinTerminar),
                            Injectorwells = decimal.Parse(formaC7.pozosTaponadosInyectores),
                            Oilproductionwell = 0,
                            Gasproductionwells = 0,
                            PdenId = (formaC7.Pden_id != "") ? formaC7.Pden_id : "",
                            row_created_by = GetUserId,
                            row_created_date = DateTime.Now,
                            row_changed_by = GetUserId,
                            row_changed_date = DateTime.Now,

                            pozosProductoresActivosLevantamientoArtificial =  formaC7.PozosProductoresActivosLevantamientoArtificial,
                            pozosProductoresActivosFlujoNatural = formaC7.PozosProductoresActivosFlujoNatural,
                            pozosProductoresInactivosCerradoAltaRelacionAguaPetroleo  = formaC7.PozosProductoresInactivosCerradoAltaRelacionAguaPetroleo,
                            pozosProductoresInactivosCerradoTemporalmente = formaC7.PozosProductoresInactivosCerradoTemporalmente,
                            pozosTaponadosSecos = formaC7.PozosTaponadosSecos,
                            pozosSuspendidosTemporalmente =  formaC7.PozosSuspendidosTemporalmente,
                            PozosSinTerminar = formaC7.PozosSinTerminar,

                            pozosTaponadosInyectores =  Convert.ToDecimal(formaC7.pozosTaponadosInyectores??"0"),
                            pozosTaponadosInyectoresGas = Convert.ToDecimal(formaC7.pozosTaponadosInyectoresGas??"0"),
                            pozosTaponadosInyectoresAire = Convert.ToDecimal(formaC7.pozosTaponadosInyectoresAire??"0")
                        };

                        await _formaC7Repository.SaveFormC7Detail(formC7Detail);
                    }

                    var forStateid = await _formaOficial.GetState("En Proceso de Aprobación");
                    Concreteform concreteform = new Concreteform
                    {
                        Concreteformid = Guid.NewGuid(),
                        Maincampid = 0,
                        Company = headerForma.Operador,
                        Contract = headerForma.ContratoAsociacion,
                        Battery = "",
                        Tank = "",
                        Month = decimal.Parse(headerForma.Mes),
                        Year = decimal.Parse(headerForma.Anio),
                        Explotationmodality = headerForma.ModalidadExplotacion,
                        Annotations = "",
                        Version = 1,
                        Currentstate = forStateid,
                        Generationflag = 1,
                        Campid = 0,
                        Pdenid = headerForma.CompaniaId,
                        Formid = formC7.Formc7id,
                        Generationjobid = 1,
                        Iqistatus = 0,
                        Usersigning = "",
                        Minrepsigning = "",
                        Formname = "Forma Cuadro 7"
                    };

                    await _repositoryConcreform.AddAsync(concreteform);

                    string fecha = $"{headerForma.Anio}/{headerForma.Mes}";
                    DateTime dateForma = DateTime.Parse(fecha, cultureInfo);
                    var datauser = await _administracionService.UsuarioAprobadorAsync(new Administracion.RequestAprobaciones { NombreForma = "Cargue Forma Cuadro 7" });
                    var dataService = datauser.Data.Deserialize<UsuariosRecursosAprobaciones>();
                    if (dataService != null)
                    {
                        var codeUser = dataService.CodigoUsuario;
                        var _aprobacion = new Aprobacioncarga
                        {
                            IdForma = formC7.Formc7id,
                            FechaForma = dateForma,
                            Usuario = GetUserId,
                            FechaCarga = DateTime.Now,
                            Estado = forStateid,
                            FechaActualizacion = DateTime.Now,
                            UsuarioAprobador = dataService.CodigoUsuario,
                            UsuarioNombre = userName,
                            UsuarioNombreAprobador = dataService.NombreUsuario,
                            FormaName = "Forma Cuadro 7",
                            UrlForma = $"FormaC7/{headerForma.Anio}/{headerForma.Mes}/{headerForma.FileName}",
                            Campo = headerForma.Campo,
                            Contrato = headerForma.ContratoAsociacion,
                            Operadora = headerForma.Operador,
                            ComparativoGas = 0,
                            ComparativoAgua = 0,
                            ComparativoCrudo = 0
                        };

                        await _formaOficial.CreteAprobacion(_aprobacion);
                        var moveFile = await _commonService.MoveItemAsync(new Commons.FileSystemMoveItemOptions
                        {
                            Container = "formas",
                            DestinationDirectory = new Commons.FileSystemItemInfo
                            {
                                Path = $"FormaC7/{headerForma.Anio}/{headerForma.Mes}/{headerForma.FileName}"
                            },
                            Item = new Commons.FileSystemItemInfo
                            {
                                Path = $"tmp/fc7/{headerForma.FileName}"
                            }
                        });


                        string asunto = "Notificación de carga Forma Cuadro 7";
                        MailForma9 mailForma = new MailForma9();

                        string view = mailForma.GetView(asunto, dataService.NombreUsuario, headerForma.Operador, headerForma.ContratoAsociacion, headerForma.Campo, fecha,
                                          "Se ha detectado la carga de una forma  relacionada a la Forma Cuadro 7 en estado de <b>En Proceso de Aprobación</b> ");
                        EmailInfo Email = new EmailInfo()
                        {
                            To = new List<string>() { dataService.Correo },
                            Subject = asunto,
                            Body = view,
                            Styles = mailForma.GetHeaderStyle()
                        };

                        await _sendMailService.SendEmailAsync(Email);
                    }
                }
                response.Code = (int)HttpStatusCode.OK;
                response.Message = Messages.Created;
                response.Data = data;
            }
            catch (Exception ex)
            {
                _telemetryException.RegisterException(ex);
                response.Code = (int)HttpStatusCode.BadRequest;
            }
            return response;
        }

        public async Task<ResponseBase<RequestFormas<RequestFormaC7>>> LoadFile(List<RequestFormaC7> data, Stream fileStream, string fileName, string getUserId)
        {
            var response = new ResponseBase<RequestFormas<RequestFormaC7>>();
            List<RequestFormaC7> dataResponse = new List<RequestFormaC7>();
            fileName = fileName.Replace(" ", "_");
            try
            {
                if (data?.Count > 0)
                {
                    for (int i = 0; i <= data?.Count - 1; i++)
                    {
                        RequestFormaC7 item = data[i];
                        var info = item.Info;
                        info.FileName = fileName;
                        var dataForma = item.Data;

                        await ValidacionCabecera(i, info, getUserId);
                        await ValidacionBody(i, dataForma);
                        await ValidacionJson();

                        if (Errors.Count == 0)
                        {
                            if (dataForma.Count > 0)
                            {
                                dataForma.ForEach(l =>
                                {
                                    l.Pden_id = pdenIds.FirstOrDefault(m => m.Key == l.EstructuraCampo).Value;
                                });
                            }

                            var upload = await _commonService.UploadFileAsync(new Commons.FileSystemUploadFileOptions
                            {
                                FileName = fileName,
                                FileData = ConvertTypes.ConvertToBase64(fileStream),
                                Container = "formas",
                                DestinationDirectory = new Commons.FileSystemItemInfo
                                {
                                    Path = $"tmp/fc7"
                                }
                            });

                            RequestFormaC7 information = new RequestFormaC7
                            {
                                Info = Header,
                                Data = dataForma
                            };
                            dataResponse.Add(information);
                            Response.errors = Errors;
                        }
                        else
                        {
                            Response.errors = Errors;
                        }
                    }
                    Response.data = dataResponse;
                    response.Data = Response;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.BadRequest;
                _telemetryException.RegisterException(ex);
            }
            return response;
        }

        private async Task ValidacionCabecera(int key, HeaderFormaC7 data, string getUserId)
        {
            Header = data;
            JsonValidacion.FORMA_CODIGO = "7";

            //Validation Headers
            if (string.IsNullOrEmpty(data.Campo))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El formato no tiene un Campo",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            if (!string.IsNullOrEmpty(data.Campo))
            {
                var list = await _formasValidate.ValidaCampo(data.Campo);
                if (list?.Count == 0)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = $"El Campo { data.Campo } no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Campo
                    };
                    Errors.Add(error);
                }

            }

            if (string.IsNullOrEmpty(data.Operador))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = $"No tiene un Operador",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            if (!string.IsNullOrEmpty(data.Operador))
            {
                var list = await _formasValidate.ValidaOperador(data.Operador);
                if (list?.Count == 0)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = $"El Operador { data.Operador } no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Operador
                    };
                    Errors.Add(error);
                }
            }

            if (string.IsNullOrEmpty(data.Bloque))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El bloque no tiene un Operador",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            //if (!string.IsNullOrEmpty(data.Bloque))
            //{
            //    var list = await _formasValidate.ValidaCampo(data.Bloque);
            //    if (list?.Count == 0)
            //    {
            //        ErrorFormasStructura error = new ErrorFormasStructura()
            //        {
            //            sheet = key,
            //            message = $"El Bloque { data.Bloque } no existe en la Base de Datos",
            //            column = "",
            //            row = 0,
            //            value = data.Bloque
            //        };
            //        Errors.Add(error);
            //    }

            //}

            if (string.IsNullOrEmpty(data.Estructura))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = $"No tiene un Estructura",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            if (string.IsNullOrEmpty(data.Mes))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "No tiene un Mes",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            if (string.IsNullOrEmpty(data.Anio))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "No tiene un Año",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            if (string.IsNullOrEmpty(data.ContratoAsociacion))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El formato no tiene un Contrato",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            if (!string.IsNullOrEmpty(data.Operador) && !string.IsNullOrEmpty(data.Campo))
            {
                var list = await _formasValidate.ValidaCompaniaCampo(data.Operador, data.Campo);
                if (list?.Count > 0)
                {
                    string operador_id = ConvertTypes.ConverDataDynamic(list, "OPERADOR_ID");
                    string contrato_id = ConvertTypes.ConverDataDynamic(list, "CONTRATO_ID");
                    string campo_id = ConvertTypes.ConverDataDynamic(list, "CAMPO_ID");
                    string campo = ConvertTypes.ConverDataDynamic(list, "CAMPO");

                    JsonValidacion.CONTRATO_ID = contrato_id;
                    JsonValidacion.CAMPO_ID = campo_id;
                    JsonValidacion.OPERADOR_ID = operador_id;
                    JsonValidacion.CAMPO = Header.Campo;
                    JsonValidacion.OPERADOR = Header.Operador;
                    JsonValidacion.CONTRATO = Header.ContratoAsociacion;
                    Header.CompaniaId = operador_id;
                    Header.CampoId = campo_id;
                    Header.ContratoId = contrato_id;
                    Header.ContratoAsociacion = campo;
                }
                else
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = $"No existe información relacionada para el Operador {Header.Operador} con el campo {Header.Campo} y el contrato {Header.ContratoAsociacion}",
                        column = "",
                        row = 0,
                        value = ""
                    };
                    Errors.Add(error);
                }
            }

            JsonValidacion.ANIO = data.Anio;
            JsonValidacion.MES = data.Mes;
            var fechaForma = DateTime.Parse($"{data.Mes}/{data.Anio}", cultureInfo);
            Response.permisoDeCargue = await ValidaFechasPermisos(fechaForma, operador: Header.Operador, campo: Header.Campo, contrato: Header.ContratoAsociacion, getUserId, false);
        }

        private async Task ValidacionBody(int key, List<BodyFormaC7> listData)
        {
            JsonValidacion.REGISTRO = new List<DetalleF7>();
            foreach (BodyFormaC7 item in listData)
            {
                if (!string.IsNullOrEmpty(item.EstructuraCampo))
                {
                    DetalleF7 campo = new DetalleF7()
                    {
                        CAMPO = item.EstructuraCampo
                    };
                    await Task.Run(() => JsonValidacion.REGISTRO.Add(campo));
                }
            }
        }

        private async Task ValidacionJson()
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };

            try
            {
                List<CabeceraFormas<DetalleF7>> listJson = new List<CabeceraFormas<DetalleF7>>
                {
                    JsonValidacion
                };

                var json = listJson.Serialize(settings);
                var res = await _formasService.ValidarFormasMinAsync(new Ppdm.RequestBase { SJson = json });
                if (res.Code == 200)
                {
                    try
                    {
                        var dataService = res.Data.Deserialize<ResponseForm9>(settings);
                        List<Registro> registros = dataService.FORMAS?.FORMA?.REGISTRO;
                        if (registros?.Count > 0)
                        {
                            foreach (var item in registros)
                            {
                                if (item.PDEN_ID == "FALSE")
                                {
                                    ErrorFormasStructura error = new ErrorFormasStructura()
                                    {
                                        message = $"No se encontaron datos para el campo { JsonValidacion.CAMPO }"
                                    };
                                    Errors.Add(error);
                                }
                                if (!string.IsNullOrEmpty(item.PDEN_ID))
                                {
                                    pdenIds.Add(item.CAMPO, item.PDEN_ID);
                                }
                            }
                        }

                        if (dataService.root != null)
                        {
                            Element element = dataService.root.element;

                            if (element.MENSAJE == "Esta Forma ya fue cargada")
                            {
                                Response.permisoDeCargue = false;
                            }
                            else
                            {

                                ErrorFormasStructura error = new ErrorFormasStructura()
                                {
                                    message = element.MENSAJE
                                };
                                Errors.Add(error);
                                Response.permisoDeCargue = false;
                            }
                        }
                    }
                    catch
                    {
                        var dataService = res.Data.Deserialize<ResponseForm9Json>(settings);
                        Registro registro = dataService.FORMAS?.FORMA?.REGISTRO;


                        if (registro != null)
                        {
                            if (registro.PDEN_ID == "FALSE")
                            {
                                ErrorFormasStructura error = new ErrorFormasStructura()
                                {
                                    message = $"No se encontaron datos para el campo { JsonValidacion.CAMPO }"
                                };
                                Errors.Add(error);
                            }

                            if (!string.IsNullOrEmpty(registro.PDEN_ID))
                            {
                                pdenIds.Add(registro.CAMPO, registro.PDEN_ID);
                            }

                        }
                        if (dataService.root != null)
                        {
                            Element element = dataService.root.element;
                            ErrorFormasStructura error = new ErrorFormasStructura()
                            {
                                message = element.MENSAJE
                            };
                            Errors.Add(error);
                        }
                    }
                }

                if (res.Code != 200)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        message = "No se pudo procesar la validación de la forma que se esta cargando, verifique e intente nuevamente"
                    };
                    Errors.Add(error);
                    Response.permisoDeCargue = false;


                }

            }
            catch (Exception ex)
            {
                _telemetryException.RegisterException(ex);
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    message = "No se pudo procesar la validación de la forma que se esta cargando, verifique e intente nuevamente"
                };
                Errors.Add(error);
                Response.permisoDeCargue = false;
            }
        }

        private async Task<bool> ValidaFechasPermisos(DateTime fechaForma, string operador, string campo, string contrato, string getUserId, bool delete)
        {
            DateTime date = DateTime.Now;
            DateTime mesOperativo;
            if (date.Month == 1)
                mesOperativo = new DateTime(date.Year - 1, 12, 1);
            else
                mesOperativo = new DateTime(date.Year, date.Month - 1, 1);
             
            DateTime mesActual = DateTime.Now;
            bool state = false;

            if (fechaForma < mesOperativo)
            {
                var permisos = await _formaOficial.GetPrecarga("Cargue Forma Cuadro 7", fechaForma, operador, campo, contrato);
                if (permisos.Count > 0)
                {
                    foreach (Aprobacionprecarga precarga in permisos)
                    {
                        if (precarga.Activo == 1 && precarga.Usuario == getUserId)
                        {

                            if (precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                            {
                                Aprobacioncarga data = await _formaOficial.GetaFormAprobacion("Forma Cuadro 7", precarga.FechaForma.Value, precarga.Operadora, precarga.Campo,
                                precarga.Contrato);
                                if (data != null)
                                {
                                    if (data.Formstate.Name != "Aprobada")
                                    {
                                        if (delete)
                                        {
                                            await _formaC7Repository.Delete(data.IdForma.Value);
                                        }
                                        state = true;
                                        JsonValidacion.RECARGAR = "1";
                                    }
                                    else
                                    {
                                        state = true;
                                        JsonValidacion.RECARGAR = "1";
                                    }
                                }
                                if (data == null)
                                {
                                    state = true;
                                }

                            }
                        }
                    }
                }
            }

            if (mesOperativo == fechaForma)
            {
                Aprobacioncarga data = await _formaOficial.GetaFormAprobacion("Forma Cuadro 7", fechaForma, operador, campo, contrato);
                var permisos = await _formaOficial.GetPrecarga("Cargue Forma Cuadro 7", fechaForma, operador, campo, contrato);
                if (permisos?.Count > 0)
                {
                    foreach (Aprobacionprecarga precarga in permisos)
                    {
                        if (precarga.Activo == 1 && precarga.Usuario == getUserId && precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                        {
                            if (data != null)
                            {
                                if (data.Formstate.Name != "Aprobada" && data.Usuario == getUserId)
                                {
                                    if (delete)
                                    {
                                        await _formaC7Repository.Delete(data.IdForma.Value);
                                    }
                                    state = true;
                                    JsonValidacion.RECARGAR = "1";
                                }
                                else
                                {
                                    JsonValidacion.RECARGAR = "1";
                                    state = true;
                                }

                            }
                            else
                            {
                                JsonValidacion.RECARGAR = "1";
                                state = true;
                            }
                        }
                    }
                }
                else
                {
                    if (data != null)
                    {
                        if (data.Formstate.Name != "Aprobada" && data.Usuario == getUserId)
                        {
                            if (delete)
                            {
                                await _formaC7Repository.Delete(data.IdForma.Value);
                            }
                            state = true;
                            JsonValidacion.RECARGAR = "1";
                        }
                        else
                        {
                            JsonValidacion.RECARGAR = "0";
                            state = true;
                        }

                    }
                }
                if (data == null)
                {
                    state = true;

                }
            }
            return state;
        }
    }
}
