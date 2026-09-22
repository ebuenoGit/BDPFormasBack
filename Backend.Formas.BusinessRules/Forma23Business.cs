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
    public class Forma23Business : IForma23Business
    {
        private readonly ITelemetryException _telemetryException;
        private readonly IAdministracionFormas _formaOficial;
        private readonly IFormaValidate _formasValidate;
        private readonly IForma23Repository _forma23Repository;

        private readonly Administracion.AdmGrpc.AdmGrpcClient _administracionService;
        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient _formasService;
        private readonly Commons.CommonGrpc.CommonGrpcClient _commonService;

        private readonly CabeceraFormas<PozoYacimiento> JsonValidacion = new CabeceraFormas<PozoYacimiento>();
        private readonly List<ErrorFormasStructura> Errors = new List<ErrorFormasStructura>();
        private readonly RequestFormas<RequestForma23> Response = new RequestFormas<RequestForma23>();

        private HeaderForma23 Header = new HeaderForma23();
        private readonly Dictionary<string, string> pdenIds = new Dictionary<string, string>();
        private readonly CultureInfo cultureInfo = new CultureInfo("es-co");
        private readonly ISendMailService _SendMailService;
        private readonly IBaseRepository<Concreteform> RepositoryConcreform;

        public Forma23Business(ITelemetryException telemetryException, IAdministracionFormas administracionFormas, IFormaValidate formaValidate, ISendMailService sendMailService,
                               Administracion.AdmGrpc.AdmGrpcClient admGrpcClient, Ppdm.PpdmGrpc.PpdmGrpcClient ppdmGrpcClient, Commons.CommonGrpc.CommonGrpcClient commonGrpcClient,
                               IBaseRepository<Concreteform> concretForm, IForma23Repository forma23Repository)
        {
            _telemetryException = telemetryException;
            _formaOficial = administracionFormas;
            _formasValidate = formaValidate;
            _administracionService = admGrpcClient;
            _formasService = ppdmGrpcClient;
            _commonService = commonGrpcClient;
            _SendMailService = sendMailService;
            RepositoryConcreform = concretForm;
            _forma23Repository = forma23Repository;
            JsonValidacion.RECARGAR = "0";
        }

        public async Task<ResponseBase<dynamic>> Create(List<ResponseForma23> data, string GetUserId, string userName)
        {
            var response = new ResponseBase<dynamic>();
            try
            {
                for (var i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    HeaderForma23 headerForma = item.Info;
                    List<BodyForma23> dataForma = item.Data;
                    var fechaForma = DateTime.Parse($"{headerForma.Mes}/{headerForma.Anio}", cultureInfo);
                    await ValidaFechasPermisos(fechaForma: fechaForma, operador: headerForma.Compania, campo: headerForma.Campo, contrato: headerForma.Contrato, getUserId: GetUserId, true);

                    var info2 = dataForma[0];
                    Form23 form23 = new Form23()
                    {
                        Form23id = Guid.NewGuid(),
                        Formation = headerForma.Formacion,
                        Block = headerForma.Campo,
                        Oilfield = headerForma.Formacion,
                        Structure = headerForma.Estructura,
                        Member = headerForma.Operador,
                        Operadorid = headerForma.CompaniaId,
                        Operador = headerForma.Compania,
                        Contractid = headerForma.ContratoId,
                        Contract = headerForma.Contrato,
                        Campoid = headerForma.CampoId,
                        Campo = headerForma.Campo,
                        row_created_by = GetUserId,
                        row_created_date = DateTime.Now,
                        row_changed_by = GetUserId,
                        row_changed_date = DateTime.Now
                    };

                    await _forma23Repository.SaveForm23(form23);
                    foreach (BodyForma23 forma23 in dataForma)
                    {
                        Form23Detail form23Detail = new Form23Detail()
                        {
                            Form23detailid = Guid.NewGuid(),
                            Formid = form23.Form23id,
                            Widays = decimal.Parse(forma23.ProduccionDiasMes),
                            Wiaccumulateddays = decimal.Parse(forma23.InyeccionDiasAcumulados),
                            Pressure = decimal.Parse(forma23.InyeccionPresionMediaInyeccion),
                            Widailywater = decimal.Parse(forma23.ProduccionSA),
                            Wimonthlywater = decimal.Parse(forma23.InyeccionVolumenesGLPAcumuladoBLS),
                            Wiaccumulatedwater = decimal.Parse(forma23.InyeccionVolumenesGLPMensualBLS),
                            Poolname = forma23.ProduccionPozoNo,
                            Injectionoilwellfinalstate = null,
                            productionmethodproduct =forma23.ProduccionMetodoProduccion,
                            Monthdays = decimal.Parse(forma23.InyeccionDiasMes),
                            Accumulatedays = decimal.Parse(forma23.InyeccionDiasAcumulados),
                            Dailyoilproduction = decimal.Parse(forma23.ProduccionDiasAcumulados),
                            Monthlyoilproduction = decimal.Parse(forma23.ProduccionPetroleoMesBLS),
                            Accumulateoilproduction = decimal.Parse(forma23.ProduccionPetroleoAcumuladoBLS),
                            Dailywaterproduction = decimal.Parse(forma23.ProduccionDiasMes),

                            Monthlywaterproduction = decimal.Parse(forma23.InyeccionInyectadosGasMensualMPC),
                            Accumulatewaterproduction = decimal.Parse(forma23.InyeccionInyectadosGasAcumuladoMPC),
                            Dailygasproduction = decimal.Parse(forma23.InyeccionEspesorEfectivoZonaAbiertaPies),
                            Monthlygasproduction = decimal.Parse(forma23.ProduccionGasMesMPC),
                            Accumulategasproduction = decimal.Parse(forma23.ProduccionGasAcumuladoMPC),
                            Productionoilwellfinalstate = forma23.ProduccionEstadoPozoFinalMes,
                            PdenId = (forma23.Pden_id != "") ? forma23.Pden_id : "",
                            PressureProd = forma23.ProduccionPresionFondo,
                            row_created_by = GetUserId,
                            row_created_date = DateTime.Now,
                            row_changed_by = GetUserId,
                            row_changed_date = DateTime.Now
                        };

                        await _forma23Repository.SaveForm23Detail(form23Detail);
                    }

                    var forStateid = await _formaOficial.GetState("En Proceso de Aprobación");
                    Concreteform concreteform = new Concreteform
                    {
                        Concreteformid = Guid.NewGuid(),
                        Maincampid = 0,
                        Company = headerForma.Compania,
                        Contract = headerForma.Contrato,
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
                        Formid = form23.Form23id,
                        Generationjobid = 1,
                        Iqistatus = 0,
                        Usersigning = "",
                        Minrepsigning = "",
                        Formname = "Forma 23CR"
                    };

                    await RepositoryConcreform.AddAsync(concreteform);

                    string fecha = $"{headerForma.Anio}/{headerForma.Mes}";
                    DateTime dateForma = DateTime.Parse(fecha, cultureInfo);
                    var datauser = await _administracionService.UsuarioAprobadorAsync(new Administracion.RequestAprobaciones { NombreForma = "Cargue Forma 23CR" });
                    var dataService = datauser.Data.Deserialize<UsuariosRecursosAprobaciones>();
                    if (dataService != null)
                    {
                        var codeUser = dataService.CodigoUsuario;
                        var _aprobacion = new Aprobacioncarga
                        {
                            IdForma = form23.Form23id,
                            FechaForma = dateForma,
                            Usuario = GetUserId,
                            FechaCarga = DateTime.Now,
                            Estado = forStateid,
                            FechaActualizacion = DateTime.Now,
                            UsuarioAprobador = dataService.CodigoUsuario,
                            UsuarioNombre = userName,
                            UsuarioNombreAprobador = dataService.NombreUsuario,
                            FormaName = "Forma 23CR",
                            UrlForma = $"Forma23/{headerForma.Anio}/{headerForma.Mes}/{headerForma.FileName}",
                            Campo = headerForma.Campo,
                            Contrato = headerForma.Contrato,
                            Operadora = headerForma.Compania,
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
                                Path = $"Forma23/{headerForma.Anio}/{headerForma.Mes}/{headerForma.FileName}"
                            },
                            Item = new Commons.FileSystemItemInfo
                            {
                                Path = $"tmp/f23/{headerForma.FileName}"
                            }
                        });


                        string asunto = "Notificación de carga Forma 23";
                        MailForma9 mailForma = new MailForma9();

                        string view = mailForma.GetView(asunto, dataService.NombreUsuario, headerForma.Compania, headerForma.Contrato, headerForma.Campo, fecha,
                                         "Se ha detectado la carga de una forma  relacionada a la Forma 23 en estado de <b>En Proceso de Aprobación</b> ");
                        EmailInfo Email = new EmailInfo()
                        {
                            To = new List<string>() { dataService.Correo },
                            Subject = asunto,
                            Body = view,
                            Styles = mailForma.GetHeaderStyle()
                        };

                        await _SendMailService.SendEmailAsync(Email);
                    }
                }
                response.Code = (int)HttpStatusCode.OK;
                response.Message = Messages.Created;
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.BadRequest;
                _telemetryException.RegisterException(ex);
            }
            return response;
        }

        public async Task<ResponseBase<RequestFormas<RequestForma23>>> LoadFile(List<RequestForma23> data, Stream fileStream, string fileName, string getUserId)
        {
            var response = new ResponseBase<RequestFormas<RequestForma23>>();
            List<RequestForma23> dataResponse = new List<RequestForma23>();
            try
            {
                if (data?.Count > 0)
                {
                    for (int i = 0; i <= data?.Count - 1; i++)
                    {
                        RequestForma23 item = data[i];
                        var info = item.Info;
                        info.FileName = fileName;
                        var dataForma = item.Data;

                        //validation
                        await ValidacionCabecera(i, info, getUserId);
                        ValidacionBody(i, dataForma);
                        await ValidacionJson(getUserId);

                        if (Errors.Count == 0)
                        {
                            if (dataForma.Count > 0)
                            {
                                dataForma.ForEach(l =>
                                {
                                    l.Pden_id = pdenIds.FirstOrDefault(m => m.Key == l.ProduccionPozoNo).Value;
                                });
                            }

                            var upload = await _commonService.UploadFileAsync(new Commons.FileSystemUploadFileOptions
                            {
                                FileName = fileName,
                                FileData = ConvertTypes.ConvertToBase64(fileStream),
                                Container = "formas",
                                DestinationDirectory = new Commons.FileSystemItemInfo
                                {
                                    Path = $"tmp/f23"
                                }
                            });

                            RequestForma23 information = new RequestForma23
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
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        message = "Por favor intentelo de nuevo ocurrio un error inesperado"
                    };
                    Errors.Add(error);
                    Response.errors = Errors;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.BadRequest;
                _telemetryException.RegisterException(ex);
            }
            return response;
        }

        private async Task ValidacionCabecera(int key, HeaderForma23 data, string getUserId)
        {
            Header = data;
            JsonValidacion.FORMA_CODIGO = "23";

            //Validation Headers

            if (string.IsNullOrEmpty(data.Compania))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El formato no tiene una Compañia",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            if (!string.IsNullOrEmpty(data.Compania))
            {
                var list = await _formasValidate.ValidaOperador(data.Compania);
                if (list?.Count == 0)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = "El Operador o Compañía no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Compania
                    };
                    Errors.Add(error);
                }
            }

            if (string.IsNullOrEmpty(data.Operador))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El formato no tiene un Operador",
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
                        message = "El Operador no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Campo
                    };
                    Errors.Add(error);
                }

            }

            if (string.IsNullOrEmpty(data.Bloque))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El formato no tiene un Operador",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }


            if (!string.IsNullOrEmpty(data.Bloque))
            {
                var list = await _formasValidate.ValidaCampo(data.Bloque);
                if (list?.Count == 0)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = "El Bloque no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Campo
                    };
                    Errors.Add(error);
                }

            }

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
                        message = "El Campo no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Campo
                    };
                    Errors.Add(error);
                }
            }

            if (string.IsNullOrEmpty(data.Yacimiento))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El formato no tiene un Yacimiento",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            if (!string.IsNullOrEmpty(data.Yacimiento))
            {
                var list = await _formasValidate.ValidaYacimiento(data.Yacimiento);
                if (list?.Count == 0)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = "El Yacimiento no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Campo
                    };
                    Errors.Add(error);
                }
                else
                {
                    //var  
                  var startunitId =   ConvertTypes.ConverDataDynamic(list, "STRAT_UNIT_ID");
                  JsonValidacion.YACIMIENTO = data.Yacimiento;
                  JsonValidacion.YACIMIENTO_ID = startunitId;
                }
            }

            if (string.IsNullOrEmpty(data.Estructura))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El formato no tiene un Estructura",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            if (string.IsNullOrEmpty(data.Estructura))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El formato no tiene un Estructura",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            if (string.IsNullOrEmpty(data.Formacion))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El formato no tiene una Formación",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            //if (!string.IsNullOrEmpty(data.Formacion))
            //{
            //    var list = await _formasValidate.ValidaFormacion(data.Formacion);

            //    if (list?.Count > 0)
            //    {
            //        var STRAT_UNIT_ID = ConvertTypes.ConverDataDynamic(list, "STRAT_UNIT_ID");
            //        var STRAT_NAME_SET_ID = ConvertTypes.ConverDataDynamic(list, "STRAT_NAME_SET_ID");
            //        JsonValidacion.FORMACION_ID = STRAT_UNIT_ID;
            //        JsonValidacion.FORMACION_SET_ID = STRAT_NAME_SET_ID;
            //        JsonValidacion.FORMACION = data.Formacion;
            //        JsonValidacion.YACIMIENTO_ID = STRAT_UNIT_ID;
            //        Header.FormacionId = STRAT_UNIT_ID;
            //        Header.FormacionSetId = STRAT_NAME_SET_ID;
            //    }

            //    if (list?.Count == 0 || list == null)
            //    {
            //        ErrorFormasStructura error = new ErrorFormasStructura()
            //        {
            //            sheet = key,
            //            message = "La Formación no existe en la Base de Datos",
            //            column = "",
            //            row = 0,
            //            value = data.Formacion
            //        };
            //        Errors.Add(error);
            //    }

            //}

            if (string.IsNullOrEmpty(data.Mes))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El formato no tiene un Mes",
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
                    message = "El formato no tiene un Año",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            if (!string.IsNullOrEmpty(data.Compania) && !string.IsNullOrEmpty(data.Campo))
            {
                var list = await _formasValidate.ValidaCompaniaCampo(data.Compania, data.Campo);
                if (list?.Count > 0)
                {
                    string operador_id = ConvertTypes.ConverDataDynamic(list, "OPERADOR_ID");
                    string contrato_id = ConvertTypes.ConverDataDynamic(list, "CONTRATO_ID");
                    string campo_id = ConvertTypes.ConverDataDynamic(list, "CAMPO_ID");
                    string campo = ConvertTypes.ConverDataDynamic(list, "CAMPO");
                    string contrato = ConvertTypes.ConverDataDynamic(list, "CONTRATO");

                    JsonValidacion.CONTRATO_ID = contrato_id;
                    JsonValidacion.CAMPO_ID = campo_id;
                    JsonValidacion.OPERADOR_ID = operador_id;
                    JsonValidacion.CAMPO = Header.Campo;
                    JsonValidacion.OPERADOR = Header.Compania;
                    JsonValidacion.CONTRATO = contrato;
                    Header.CompaniaId = operador_id;
                    Header.CampoId = campo_id;
                    Header.ContratoId = contrato_id;
                    Header.Contrato = contrato;
                }
                else
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = $"No existe información relacionada para el Operador {Header.Compania} con el campo {Header.Campo} y el contrato {Header.Contrato}",
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

            Response.permisoDeCargue = await ValidaFechasPermisos(fechaForma, operador: Header.Compania, campo: Header.Campo, contrato: Header.Contrato, getUserId, false);
        }

        private void ValidacionBody(int key, List<BodyForma23> listData)
        {
            JsonValidacion.REGISTRO = new List<PozoYacimiento>();
            foreach (BodyForma23 item in listData)
            {
                if (!string.IsNullOrEmpty(item.ProduccionPozoNo))
                {
                    PozoYacimiento pozo = new PozoYacimiento()
                    {
                        POZO_YACIMIENTO = item.ProduccionPozoNo
                    };

                    JsonValidacion.REGISTRO.Add(pozo);
                }
            }
        }

        private async Task ValidacionJson( string user)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };

            try
            {
                List<CabeceraFormas<PozoYacimiento>> listJson = new List<CabeceraFormas<PozoYacimiento>>
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
                                    pdenIds.Add(item.POZO_YACIMIENTO, item.PDEN_ID);
                                }
                            }
                        }

                        if (dataService.root != null)
                        {
                            Element element = dataService.root.element;

                            Response.permisoDeCargue = await ValidaPermisoCarga(element.MENSAJE, "Cargue Forma 23CR", user);

                            if (element.MENSAJE != "Esta Forma ya fue cargada")
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
                                pdenIds.Add(registro.POZO_YACIMIENTO, registro.PDEN_ID);
                            }

                        }

                        if (dataService.root != null)
                        {
                            Element element = dataService.root.element;

                            Response.permisoDeCargue = await ValidaPermisoCarga(element.MENSAJE, "Cargue Forma 23CR", user);
                            
                            if(element.MENSAJE != "Esta Forma ya fue cargada")
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
            DateTime mesActual = DateTime.Now;
            DateTime mesOperativo;

            if (date.Month == 1)
                mesOperativo = new DateTime(date.Year - 1, 12, 1);
            else
                mesOperativo = new DateTime(date.Year, date.Month - 1, 1);

            bool state = false;

            if (fechaForma < mesOperativo)
            {
                var permisos = await _formaOficial.GetPrecarga("Cargue Forma 23CR", fechaForma, operador, campo, contrato);
                if (permisos.Count > 0)
                {
                    foreach (Aprobacionprecarga precarga in permisos)
                    {
                        if (precarga.Activo == 1 && precarga.Usuario == getUserId)
                        {

                            if (precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                            {
                                Aprobacioncarga data = await _formaOficial.GetaFormAprobacion("Forma 23CR", precarga.FechaForma.Value, precarga.Operadora, precarga.Campo,
                                precarga.Contrato);
                                if (data != null)
                                {
                                    if (data.Formstate.Name != "Aprobada")
                                    {
                                        if (delete)
                                        {
                                            await _forma23Repository.DeleteForm23(data.IdForma.Value);
                                        }
                                        state = true;
                                        JsonValidacion.RECARGAR = "1";
                                    }
                                    else
                                    {
                                        state = true;
                                    }
                                    if (delete)
                                    {
                                        await _forma23Repository.DeleteForm23(data.IdForma.Value);
                                    }
                                    state = true;
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
                Aprobacioncarga data = await _formaOficial.GetaFormAprobacion("Forma 23CR", fechaForma, operador, campo, contrato);
                var permisos = await _formaOficial.GetPrecarga("Cargue Forma 23CR", fechaForma, operador, campo, contrato);
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
                                        await _forma23Repository.DeleteForm23(data.IdForma.Value);
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
                                await _forma23Repository.DeleteForm23(data.IdForma.Value);
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

        private async Task<bool> ValidaPermisoCarga(string message, string FormaName, string user)
        {
            List<Aprobacionprecarga> listPermisos = await _formaOficial.GetPrecarga(FormaName, DateTime.Parse($"{Header.Mes}/{Header.Anio}", cultureInfo), Header.Compania, Header.Campo, Header.Contrato);
            var permisoUser = listPermisos.FirstOrDefault(l => l.Usuario == user);
            if (message.Contains("Esta Forma ya fue cargada") && permisoUser == null)
                return false;
            else
                return true;
        }
    }
}
