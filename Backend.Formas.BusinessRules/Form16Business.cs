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
    public class Form16Business : IForm16Business
    {

        private readonly ITelemetryException _telemetryException;
        private readonly IAdministracionFormas _formaOficial;
        private readonly IFormaValidate _formasValidate;
        private readonly IForma16Repository _forma16Repository;

        private readonly CabeceraFormas<Pozos> JsonValidacion = new CabeceraFormas<Pozos>();
        private readonly List<ErrorFormasStructura> Errors = new List<ErrorFormasStructura>();
        private readonly RequestFormas<RequestForma16> Response = new RequestFormas<RequestForma16>();

        private readonly Administracion.AdmGrpc.AdmGrpcClient _administracionService;
        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient _formasService;
        private readonly Commons.CommonGrpc.CommonGrpcClient _commonService;

        private HeaderForma16 Header = new HeaderForma16();
        private readonly Dictionary<string, string> pdenIds = new Dictionary<string, string>();
        private readonly Dictionary<string, string> methodProd = new Dictionary<string, string>();
        private readonly CultureInfo cultureInfo = new CultureInfo("es-co");
        private readonly ISendMailService _sendMailService;
        private readonly IBaseRepository<Concreteform> _repositoryConcreform;

        public Form16Business(ITelemetryException telemetryException, IAdministracionFormas administracionFormas, IFormaValidate formaValidate, IForma16Repository forma16Repository,
            Administracion.AdmGrpc.AdmGrpcClient admGrpcClient, Ppdm.PpdmGrpc.PpdmGrpcClient ppdmGrpcClient, Commons.CommonGrpc.CommonGrpcClient commonGrpcClient,
            ISendMailService sendMailService, IBaseRepository<Concreteform> concreteFormRepository)
        {
            _telemetryException = telemetryException;
            _formaOficial = administracionFormas;
            _formasValidate = formaValidate;
            _forma16Repository = forma16Repository;
            _administracionService = admGrpcClient;
            _formasService = ppdmGrpcClient;
            _commonService = commonGrpcClient;
            _sendMailService = sendMailService;
            _repositoryConcreform = concreteFormRepository;
            JsonValidacion.RECARGAR = "0";
        }

        public async Task<ResponseBase<dynamic>> Create(List<ResponseForma16> data, string GetUserId, string userName)
        {
            var response = new ResponseBase<dynamic>();
            try
            {
                for (var i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    HeaderForma16 headerForma = item.Info;
                    List<BodyForma16> dataForma = item.Data;
                    var fechaForma = DateTime.Parse($"{headerForma.Mes}/{headerForma.Anio}", cultureInfo);
                    await ValidaFechasPermisos(fechaForma: fechaForma, operador: headerForma.Compania, campo: headerForma.Campo, contrato: headerForma.Contrato, getUserId: GetUserId, true);
                    var info2 = dataForma[0];
                    Form16 form16 = new Form16()
                    {
                        Form16id = Guid.NewGuid(),
                        Formation = headerForma.Formacion,
                        Block = headerForma.Campo,
                        Oilfield = headerForma.Formacion,
                        Structure = headerForma.Estructura,
                        Member = headerForma.Compania,
                        Operador = headerForma.Compania,
                        Contractid = headerForma.ContratoId,
                        Contract = headerForma.Contrato,
                        Campoid = headerForma.CampoId,
                        Campo = headerForma.Campo,
                        Yacimiento = headerForma.Yacimiento,
                        row_created_by = GetUserId,
                        row_created_date = DateTime.Now,
                        row_changed_by = GetUserId,
                        row_changed_date = DateTime.Now
                    };
                    await _forma16Repository.SaveForm16(form16);

                    foreach (BodyForma16 forma16 in dataForma)
                    {
                        Form16Detail form16Detail = new Form16Detail()
                        {
                            Form16detailid = Guid.NewGuid(),
                            Formid = form16.Form16id,
                            Oilwell = forma16.Pozo,
                            Productionmethod = forma16.MetodoProduccion,
                            Zone = "",
                            Testingdate = DateTime.Parse(forma16.FechaEnsayo),
                            Oilwellstate = forma16.EstadoPozo,
                            Thppressure = decimal.Parse(forma16.PresionTuberiaRevestimiento),
                            Chppressure = decimal.Parse(forma16.PresionTuberiaProduccion),
                            Hours = decimal.Parse(forma16.DuracionEnsayo),
                            Dailyoilproduction = 0,
                            ReductionSize = decimal.Parse(forma16.TamanioReduccion),
                            WellPumpingLength = decimal.Parse(forma16.PozoBombeoLongitudEmbolada),
                            WellPumpDumpsMinute = decimal.Parse(forma16.PozoBombeoEmboladasMinuto),
                            ProductionTestingOilBLS = decimal.Parse(forma16.ProduccionEnsayoPetroleoBLS),
                            ProductionTestGravityOil = decimal.Parse(forma16.ProduccionEnsayoGravedadPetroleo),
                            Api = 0,
                            Water = decimal.Parse(forma16.produccionEnsayoSedimientoAguaBLS),
                            Gas = decimal.Parse(forma16.ProduccionEnsayoGasMPC),
                            Rga = decimal.Parse(forma16.Rga),
                            Pden_id = forma16.Pden_id,
                            row_created_by = GetUserId,
                            row_created_date = DateTime.Now,
                            row_changed_by = GetUserId,
                            row_changed_date = DateTime.Now
                        };

                        await _forma16Repository.SaveForm16Detail(form16Detail);
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
                        Campid = decimal.Parse(headerForma.CampoId),
                        Pdenid = headerForma.CompaniaId,
                        Formid = form16.Form16id,
                        Generationjobid = 1,
                        Iqistatus = 0,
                        Usersigning = "",
                        Minrepsigning = "",
                        Formname = "Forma 16CR"
                    };

                    await _repositoryConcreform.AddAsync(concreteform);

                    string fecha = $"{headerForma.Anio}/{headerForma.Mes}";
                    DateTime dateForma = DateTime.Parse(fecha, cultureInfo);
                    var datauser = await _administracionService.UsuarioAprobadorAsync(new Administracion.RequestAprobaciones { NombreForma = "Forma 16CR" });
                    var dataService = datauser.Data.Deserialize<UsuariosRecursosAprobaciones>();
                    if (dataService != null)
                    {
                        var codeUser = dataService.CodigoUsuario;
                        var _aprobacion = new Aprobacioncarga
                        {
                            IdForma = form16.Form16id,
                            FechaForma = dateForma,
                            Usuario = GetUserId,
                            FechaCarga = DateTime.Now,
                            Estado = forStateid,
                            FechaActualizacion = DateTime.Now,
                            UsuarioAprobador = dataService.CodigoUsuario,
                            UsuarioNombre = userName,
                            UsuarioNombreAprobador = dataService.NombreUsuario,
                            FormaName = "Forma 16CR",
                            UrlForma = $"Forma16/{headerForma.Anio}/{headerForma.Mes}/{headerForma.FileName}",
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
                                Path = $"Forma16/{headerForma.Anio}/{headerForma.Mes}/{headerForma.FileName}"
                            },
                            Item = new Commons.FileSystemItemInfo
                            {
                                Path = $"tmp/f16/{headerForma.FileName}"
                            }
                        });

                        string asunto = "Notificación de carga Forma 16CR";
                        MailForma9 mailForma = new MailForma9();

                        string view = mailForma.GetView(asunto, dataService.NombreUsuario, headerForma.Compania, headerForma.Contrato, headerForma.Campo, fecha,
                                         "Se ha detectado la carga de una forma  relacionada a la Forma 16CR en estado de <b>En Proceso de Aprobación</b> ");
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
                response.Code = (int)HttpStatusCode.BadRequest;
                _telemetryException.RegisterException(ex);
            }

            return response;
        }

        public async Task<ResponseBase<RequestFormas<RequestForma16>>> LoadFile(List<RequestForma16> data, Stream fileStream, string fileName, string getUserId)
        {
            var response = new ResponseBase<RequestFormas<RequestForma16>>();
            List<RequestForma16> dataResponse = new List<RequestForma16>();
            try
            {
                if (data?.Count > 0)
                {
                    for (int i = 0; i <= data?.Count - 1; i++)
                    {
                        RequestForma16 item = data[i];
                        var info = item.Info;
                        info.FileName = fileName;
                        var dataForma = item.Data;

                        //validation
                        await ValidacionCabecera(i, info, getUserId);
                        dataForma = await ValidacionBody(i, dataForma);
                        await ValidacionJson(getUserId);

                        if (Errors.Count == 0)
                        {
                            if (dataForma.Count > 0)
                            {
                                dataForma.ForEach(l =>
                                {
                                    l.Pden_id = pdenIds.FirstOrDefault(m => m.Key == l.Pozo).Value;
                                    l.MetodoProduccion = methodProd.FirstOrDefault(m => m.Key == l.Pozo).Value;
                                });
                            }

                            var upload = await _commonService.UploadFileAsync(new Commons.FileSystemUploadFileOptions
                            {
                                FileName = fileName,
                                FileData = ConvertTypes.ConvertToBase64(fileStream),
                                Container = "formas",
                                DestinationDirectory = new Commons.FileSystemItemInfo
                                {
                                    Path = $"tmp/f16"
                                }
                            });

                            RequestForma16 information = new RequestForma16
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

        private async Task ValidacionCabecera(int key, HeaderForma16 data, string getUserId)
        {
            Header = data;
            JsonValidacion.FORMA_CODIGO = "16";

            //Validation Headers

            if (string.IsNullOrEmpty(data.Compania))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = $"El formato no tiene una Consecion",
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
                        message = $"El Operador o Compañía { data.Compania } no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Compania
                    };
                    Errors.Add(error);
                }

            }

            if (string.IsNullOrEmpty(data.Concesion))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = $"El formato no tiene una Consecion",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }


            if (string.IsNullOrEmpty(data.Concesionario))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = $"El formato no tiene una Consecionario",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            if (string.IsNullOrEmpty(data.Contrato))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = $"El formato no tiene una Contrato",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            if (!string.IsNullOrEmpty(data.Contrato))
            {
                var list = await _formasValidate.ValidaContrato(data.Contrato);

                if (list?.Count == 0)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = $"El Contrato { data.Contrato } no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Contrato
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
                        message = $"El Campo { data.Campo } no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Campo
                    };
                    Errors.Add(error);
                }
            }

            if (string.IsNullOrEmpty(data.Estructura))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El formato no tiene una Estructura",
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

            if (!string.IsNullOrEmpty(data.Formacion))
            {
                var list = await _formasValidate.ValidaFormacion(data.Formacion);

                if (list?.Count > 0)
                {
                    var STRAT_UNIT_ID = ConvertTypes.ConverDataDynamic(list, "STRAT_UNIT_ID");
                    var STRAT_NAME_SET_ID = ConvertTypes.ConverDataDynamic(list, "STRAT_NAME_SET_ID");
                    //JsonValidacion.FORMACION_ID = STRAT_UNIT_ID;
                    //JsonValidacion.FORMACION_SET_ID = STRAT_NAME_SET_ID;
                    //JsonValidacion.FORMACION = data.Formacion;
                    //JsonValidacion.YACIMIENTO_ID = STRAT_UNIT_ID;
                    Header.FormacionId = STRAT_UNIT_ID;
                    Header.FormacionSetId = STRAT_NAME_SET_ID;
                }

                if (list?.Count == 0 || list == null)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = $"La Formación { data.Formacion } no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Formacion
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

            if (!string.IsNullOrEmpty(data.Bloque))
            {
                var list = await _formasValidate.ValidaCampo(data.Bloque);
                if (list?.Count == 0)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = $"El Bloque { data.Bloque } no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Bloque
                    };
                    Errors.Add(error);
                }

            }


            if (string.IsNullOrEmpty(data.Yacimiento))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "No tiene un Yacimiento",
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
                        message = $"El Yacimiento { data.Yacimiento } no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Yacimiento
                    };
                    Errors.Add(error);
                }

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
                }
                else
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = $"No existe información relacionada para el Operador {Header.Compania} con el campo {Header.Campo} y el contato {Header.Contrato}",
                        column = "",
                        row = 0,
                        value = ""
                    };
                    Errors.Add(error);
                }
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

            JsonValidacion.ANIO = data.Anio;
            JsonValidacion.MES = data.Mes;
            var fechaForma = DateTime.Parse($"{data.Mes}/{data.Anio}", cultureInfo);

            Response.permisoDeCargue = await ValidaFechasPermisos(fechaForma, operador: Header.Compania, campo: Header.Campo, contrato: Header.Contrato, getUserId, false);
        }

        private async Task<List<BodyForma16>> ValidacionBody(int key, List<BodyForma16> listData)
        {
            List<BodyForma16> bodyJson = new List<BodyForma16>();
            JsonValidacion.REGISTRO = new List<Pozos>();
            foreach (BodyForma16 item in listData)
            {
                if (!string.IsNullOrEmpty(item.Pozo))
                {
                    Pozos pozo = new Pozos()
                    {
                        POZO = item.Pozo,
                        METODO_PRODUCCION = item.MetodoProduccion
                    };

                    JsonValidacion.REGISTRO.Add(pozo);


                    if (!string.IsNullOrEmpty(item.EstadoPozo))
                    {
                        var list = await _formasValidate.ValidarEstadoPozo(item.EstadoPozo);
                        if (list?.Count == 0)
                        {
                            ErrorFormasStructura error = new ErrorFormasStructura()
                            {
                                sheet = key,
                                message = $"El Estado Pozo { item.EstadoPozo } no existe en la Base de Datos",
                                column = "",
                                row = 0,
                                value = item.EstadoPozo
                            };
                            Errors.Add(error);
                        }
                        if (list?.Count > 0)
                        {
                            var STATUS = ConvertTypes.ConverDataDynamic(list, "STATUS");
                            item.EstadoPozo = STATUS;
                        }

                    }
                    
                    bodyJson.Add(item);

                }
            }
            return bodyJson;
        }

        private async Task ValidacionJson(string user)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };

            try
            {
                List<CabeceraFormas<Pozos>> listJson = new List<CabeceraFormas<Pozos>>
                {
                    JsonValidacion
                };
                var json = listJson.Serialize(settings);
                var res = await _formasService.ValidarFormasMinAsync(new Ppdm.RequestBase { SJson = json });
                if (res.Code == 200)
                {
                    try
                    {
                        var dataService = res.Data.Deserialize<FormasBase<List<RegistrosF16>>>(settings);
                        var registros = dataService.FORMAS?.FORMA?.REGISTRO;
                        if (registros?.Count > 0)
                        {
                            foreach (var item in registros)
                            {
                                if (item.UWI == "FALSE")
                                {
                                    ErrorFormasStructura error = new ErrorFormasStructura()
                                    {
                                        message = $"No se encontaron datos para el campo { JsonValidacion.CAMPO }"
                                    };
                                    Errors.Add(error);
                                }

                                if (!string.IsNullOrEmpty(item.UWI))
                                {
                                    pdenIds.Add(item.POZO, item.UWI);
                                    methodProd.Add(item.POZO, item.PRODUCTION_METHOD);
                                }
                            }
                        }

                        if (dataService.root != null)
                        {
                            Element element = dataService.root.element;

                            Response.permisoDeCargue = await ValidaPermisoCarga(element.MENSAJE, "Cargue Forma 16CR", user);

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
                        var dataService = res.Data.Deserialize<FormasBase<RegistrosF16>>(settings);
                        RegistrosF16 registro = dataService.FORMAS?.FORMA?.REGISTRO;


                        if (registro != null)
                        {
                            if (registro.UWI == "FALSE")
                            {
                                ErrorFormasStructura error = new ErrorFormasStructura()
                                {
                                    message = $"No se encontaron datos para el campo { JsonValidacion.CAMPO }"
                                };
                                Errors.Add(error);

                            }

                            if (!string.IsNullOrEmpty(registro.UWI))
                            {
                                pdenIds.Add(registro.POZO, registro.UWI);
                                methodProd.Add(registro.POZO, registro.PRODUCTION_METHOD);
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
                var permisos = await _formaOficial.GetPrecarga("Cargue Forma 16CR", fechaForma, operador, campo, contrato);
                if (permisos.Count > 0)
                {
                    foreach (Aprobacionprecarga precarga in permisos)
                    {
                        if (precarga.Activo == 1 && precarga.Usuario == getUserId)
                        {

                            if (precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                            {
                                Aprobacioncarga data = await _formaOficial.GetaFormAprobacion("Forma 16CR", precarga.FechaForma.Value, precarga.Operadora, precarga.Campo,
                                precarga.Contrato);
                                if (data != null)
                                {
                                    if (data.Formstate.Name != "Aprobada")
                                    {
                                        if (delete)
                                        {
                                            await _forma16Repository.Delete(data.IdForma.Value);
                                        }
                                        state = true;
                                        JsonValidacion.RECARGAR = "1";
                                    }
                                    else
                                    {
                                        state = true;
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
                Aprobacioncarga data = await _formaOficial.GetaFormAprobacion("Forma 16CR", fechaForma, operador, campo, contrato);
                var permisos = await _formaOficial.GetPrecarga("Cargue Forma 16CR", fechaForma, operador, campo, contrato);
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
                                        await _forma16Repository.Delete(data.IdForma.Value);
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
                                await _forma16Repository.Delete(data.IdForma.Value);
                            }
                            state = true;
                            JsonValidacion.RECARGAR = "1";
                        }
                        else
                        {
                            JsonValidacion.RECARGAR = "1";
                            state = false;
                        }

                    }
                    else
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