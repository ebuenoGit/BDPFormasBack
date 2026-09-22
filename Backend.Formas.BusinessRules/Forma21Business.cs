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
    public class Forma21Business : IForma21Business
    {
        private readonly ITelemetryException _telemetryException;
        private readonly IAdministracionFormas _formaOficial;
        private readonly IFormaValidate _formasValidate;
        private readonly IForma21Repository _forma21Repository;

        private readonly Administracion.AdmGrpc.AdmGrpcClient _administracionService;
        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient _formasService;
        private readonly Commons.CommonGrpc.CommonGrpcClient _commonService;

        private readonly CabeceraFormas<Pozos> JsonValidacion = new CabeceraFormas<Pozos>();
        private readonly List<ErrorFormasStructura> Errors = new List<ErrorFormasStructura>();
        private readonly RequestFormas<RequestForma21> Response = new RequestFormas<RequestForma21>();

        private HeaderForma21 Header = new HeaderForma21();
        private readonly Dictionary<string, string> pdenIds = new Dictionary<string, string>();
        private readonly CultureInfo cultureInfo = new CultureInfo("es-co");
        private readonly ISendMailService _SendMailService;
        private readonly IBaseRepository<Concreteform> RepositoryConcreform;
        private readonly IForma30SEERepository repository;

        public Forma21Business(ITelemetryException telemetryException, IAdministracionFormas administracionFormas, IFormaValidate formaValidate, IForma21Repository forma21Repository,
            Administracion.AdmGrpc.AdmGrpcClient admGrpcClient, Ppdm.PpdmGrpc.PpdmGrpcClient ppdmGrpcClient, Commons.CommonGrpc.CommonGrpcClient commonGrpcClient, IBaseRepository<Concreteform> concretForm,
            ISendMailService sendMailService, IForma30SEERepository _repository)
        {
            _telemetryException = telemetryException;
            _formaOficial = administracionFormas;
            _formasValidate = formaValidate;
            _forma21Repository = forma21Repository;
            _administracionService = admGrpcClient;
            _formasService = ppdmGrpcClient;
            _commonService = commonGrpcClient;
            RepositoryConcreform = concretForm;
            _SendMailService = sendMailService;
            JsonValidacion.RECARGAR = "0";
            repository = _repository;
        }

        public async Task<ResponseBase<dynamic>> Create(List<ResponseForma21> data, string GetUserId, string userName)
        {
            var response = new ResponseBase<dynamic>();
            try
            {
                for (var i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    HeaderForma21 headerForma = item.Info;
                    List<BodyTableForma21> dataForma = item.Data;
                    var fechaForma = DateTime.Parse($"{headerForma.Mes}/{headerForma.Anio}", cultureInfo);
                    await ValidaFechasPermisos(fechaForma: fechaForma, operador: headerForma.Compania, campo: headerForma.Campo, contrato: headerForma.Contrato, getUserId: GetUserId, true);
                    //var formafecha = ValidaFechasPermisos(fechaForma, DateTime.Now, headerForma.Compania, headerForma.Campo, headerForma.Contrato, GetUserId, "Forma 21CR");

                    var info2 = dataForma[0];
                    Form21 form21 = new Form21()
                    {
                        Form21id = Guid.NewGuid(),
                        Formation = headerForma.Formacion,
                        Block = headerForma.Campo,
                        Oilfield = headerForma.Formacion,
                        Structure = headerForma.Estructura,
                        Member = headerForma.Operador,
                        Operadorid = headerForma.CompaniaId,
                        Operador = headerForma.Operador,
                        Contractid = headerForma.ContratoId,
                        Contract = headerForma.Contrato,
                        Campoid = headerForma.CampoId,
                        Campo = headerForma.Campo,
                        row_created_by = GetUserId,
                        row_created_date = DateTime.Now,
                        row_changed_by = GetUserId,
                        row_changed_date = DateTime.Now
                    };

                    await _forma21Repository.SaveForma21(form21);

                    foreach (BodyTableForma21 forma21 in dataForma)
                    {
                        Form21InyectionDetail form21InyectionDetail = new Form21InyectionDetail()
                        {
                            Form21detailid = Guid.NewGuid(),
                            Formid = form21.Form21id,
                            Oilwell = forma21.InyeccionPozo,
                            Zone = "",
                            Gidays = decimal.Parse(forma21.InyeccionDiasMes),
                            Giaccumulateddays = decimal.Parse(forma21.InyeccionDiasAcumulados),
                            Pressure = decimal.Parse(forma21.InyeccionPresionMediaInyeccion),
                            Gidailygas = 0,
                            Gimonthlygas = decimal.Parse(forma21.InyeccionGasInyMesMPC),
                            Giaccumulatedgas = decimal.Parse(forma21.InyeccionGasInyAcumuladoMPC),
                            Oilwellfinalstate = forma21.InyeccionEstadoPozoFinalMes,
                            Poolname = "",
                            Danecode = "",
                            Pden_id = forma21.Pden_id,
                            row_created_by = GetUserId,
                            row_created_date = DateTime.Now,
                            row_changed_by = GetUserId,
                            row_changed_date = DateTime.Now
                        };

                        await _forma21Repository.SaveForm21InyectionDetail(form21InyectionDetail);

                        Form21ProductionDetail form21ProductionDetail = new Form21ProductionDetail()
                        {
                            Form21proddetailid = Guid.NewGuid(),
                            Formid = form21.Form21id,
                            Oilwell = forma21.ProduccionPozo,
                            Zone = "",
                            Danecode = 0,
                            Productionmethod = forma21.ProduccionMetodoProduccion,
                            Monthdays = decimal.Parse(forma21.ProduccionDiasMes),
                            Accumulatedays = decimal.Parse(forma21.ProduccionDiasAcumulados),
                            Dailyoilproduction = decimal.Parse(forma21.ProduccionPetroleoAcumuladoMPC),
                            Monthlyoilproduction = decimal.Parse(forma21.ProduccionPetroleoMensualBls),
                            Accumulateoilproduction = decimal.Parse(forma21.ProduccionPetroleoAcumuladoMPC),
                            Dailygasproduction = 0,
                            Montlhygasproduction = decimal.Parse(forma21.ProduccionGasMensualMPC),
                            Accumulategasproduction = decimal.Parse(forma21.ProduccionGasAcumuladoMPC),
                            Dailywaterproduction = 0,
                            Monthlywaterproduction = decimal.Parse(forma21.ProduccionAguaMensualMPC),
                            Accumulatewaterproduction = decimal.Parse(forma21.ProduccionAguaAcumuladaMPC),
                            Oilwellfinalstate = forma21.ProduccionEstadoPozoFinalMes,
                            Pressure = decimal.Parse(forma21.ProduccionPresionFondo),
                            row_created_by = GetUserId,
                            row_created_date = DateTime.Now,
                            row_changed_by = GetUserId,
                            row_changed_date = DateTime.Now,
                            Pden_id = forma21.Pden_id
                        };

                        await _forma21Repository.SaveForm21ProductionDetail(form21ProductionDetail);
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
                        Formid = form21.Form21id,
                        Generationjobid = 1,
                        Iqistatus = 0,
                        Usersigning = "",
                        Minrepsigning = "",
                        Formname = "Forma 21CR"
                    };

                    await RepositoryConcreform.AddAsync(concreteform);

                    string fecha = $"{headerForma.Anio}/{headerForma.Mes}";
                    DateTime dateForma = DateTime.Parse(fecha, cultureInfo);
                    var datauser = await _administracionService.UsuarioAprobadorAsync(new Administracion.RequestAprobaciones { NombreForma = "Cargue Forma 21CR" });
                    var dataService = datauser.Data.Deserialize<UsuariosRecursosAprobaciones>();
                    if (dataService != null)
                    {
                        var codeUser = dataService.CodigoUsuario;
                        var _aprobacion = new Aprobacioncarga
                        {
                            IdForma = form21.Form21id,
                            FechaForma = dateForma,
                            Usuario = GetUserId,
                            FechaCarga = DateTime.Now,
                            Estado = forStateid,
                            FechaActualizacion = DateTime.Now,
                            UsuarioAprobador = dataService.CodigoUsuario,
                            UsuarioNombre = userName,
                            UsuarioNombreAprobador = dataService.NombreUsuario,
                            FormaName = "Forma 21CR",
                            UrlForma = $"Forma21/{headerForma.Anio}/{headerForma.Mes}/{headerForma.FileName}",
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
                                Path = $"Forma21/{headerForma.Anio}/{headerForma.Mes}/{headerForma.FileName}"
                            },
                            Item = new Commons.FileSystemItemInfo
                            {
                                Path = $"tmp/f21/{headerForma.FileName}"
                            }
                        });


                        string asunto = "Notificación de carga Forma 21";
                        MailForma9 mailForma = new MailForma9();

                        string view = mailForma.GetView(asunto, dataService.NombreUsuario, headerForma.Compania, headerForma.Contrato, headerForma.Campo, fecha,
                                         "Se ha detectado la carga de una forma  relacionada a la Forma 21 en estado de <b>En Proceso de Aprobación</b> ");
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

        public async Task<ResponseBase<RequestFormas<RequestForma21>>> LoadFile(List<RequestForma21> data, Stream fileStream, string fileName, string getUserId)
        {
            var response = new ResponseBase<RequestFormas<RequestForma21>>();
            List<RequestForma21> dataResponse = new List<RequestForma21>();
            try
            {
                if (data?.Count > 0)
                {
                    for (int i = 0; i <= data?.Count - 1; i++)
                    {
                        RequestForma21 item = data[i];
                        var info = item.info;
                        info.FileName = fileName;
                        var dataForma = item.data;

                        //validation
                        await ValidacionCabecera(i, info, getUserId);
                        var detalleList = await ValidacionBody(i, dataForma);
                        await ValidacionJson(getUserId);

                        if (Errors.Count == 0)
                        {
                            if (detalleList.Count > 0)
                            {
                                detalleList.ForEach(l =>
                                {
                                    l.Pden_id = pdenIds.FirstOrDefault(m => m.Key == l.ProduccionPozo).Value;
                                });
                            }

                            var upload = await _commonService.UploadFileAsync(new Commons.FileSystemUploadFileOptions
                            {
                                FileName = fileName,
                                FileData = ConvertTypes.ConvertToBase64(fileStream),
                                Container = "formas",
                                DestinationDirectory = new Commons.FileSystemItemInfo
                                {
                                    Path = $"tmp/f21"
                                }
                            });

                            RequestForma21 information = new RequestForma21
                            {
                                info = Header,
                                data = detalleList
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

        private async Task ValidacionCabecera(int key, HeaderForma21 data, string getUserId)
        {
            Header = data;
            JsonValidacion.FORMA_CODIGO = "21";

            //Validation Headers

            if (string.IsNullOrEmpty(data.Compania))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = $"El formato no tiene una Compañia",
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
                    JsonValidacion.FORMACION_ID = STRAT_UNIT_ID;
                    JsonValidacion.FORMACION_SET_ID = STRAT_NAME_SET_ID;
                    JsonValidacion.FORMACION = data.Formacion;
                    JsonValidacion.YACIMIENTO_ID = STRAT_UNIT_ID;
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
                    Header.Contrato = campo;
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

            if (string.IsNullOrEmpty(data.Yacimiento))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = $"No tiene un Yacimiento",
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

            JsonValidacion.ANIO = data.Anio;
            JsonValidacion.MES = data.Mes;
            var fechaForma = DateTime.Parse($"{data.Mes}/{data.Anio}", cultureInfo);
            //var formafechaval = ValidaFechasPermisos(fechaForma, DateTime.Now, Header.Compania, Header.Campo, Header.Contrato, getUserId, "Forma 21CR");
            Response.permisoDeCargue = await ValidaFechasPermisos(fechaForma, operador: Header.Compania, campo: Header.Campo, contrato: Header.Contrato, getUserId, false);

        }

        private async Task<List<BodyTableForma21>> ValidacionBody(int key, List<BodyTableForma21> listData)
        {
            JsonValidacion.REGISTRO = new List<Pozos>();
            List<BodyTableForma21> bodyJson =new  List<BodyTableForma21>();

            foreach (BodyTableForma21 item in listData)
            {
                
                if (!string.IsNullOrEmpty(item.ProduccionPozo))
                {
                    Pozos pozo = new Pozos()
                    {
                        POZO = item.ProduccionPozo,
                        METODO_PRODUCCION = item.ProduccionMetodoProduccion
                    };

                    JsonValidacion.REGISTRO.Add(pozo);
                }

                if (!string.IsNullOrEmpty(item.ProduccionMetodoProduccion))
                {
                    var list = await _formasValidate.ValidarMethodoProducion(item.ProduccionMetodoProduccion);
                    if (list?.Count == 0)
                    {
                        ErrorFormasStructura error = new ErrorFormasStructura()
                        {
                            sheet = key,
                            message = $"El Metodo de produccion { item.ProduccionMetodoProduccion } no existe en la Base de Datos",
                            column = "",
                            row = 0,
                            value = item.ProduccionMetodoProduccion
                        };
                        Errors.Add(error);
                    }
                    if (list?.Count > 0)
                    {
                        var PRODUCTION_METHOD = ConvertTypes.ConverDataDynamic(list, "PRODUCTION_METHOD");
                        item.ProduccionMetodoProduccion = PRODUCTION_METHOD;

                    }

                }

                if (!string.IsNullOrEmpty(item.InyeccionEstadoPozoFinalMes))
                {
                    var list = await _formasValidate.ValidarEstadoPozo(item.InyeccionEstadoPozoFinalMes);
                    if (list?.Count == 0)
                    {
                        ErrorFormasStructura error = new ErrorFormasStructura()
                        {
                            sheet = key,
                            message = $"El Estado Pozo por inyeccion { item.InyeccionEstadoPozoFinalMes } no existe en la Base de Datos",
                            column = "",
                            row = 0,
                            value = item.InyeccionEstadoPozoFinalMes
                        };
                        Errors.Add(error);
                    }
                    if (list?.Count > 0)
                    {
                        var STATUS = ConvertTypes.ConverDataDynamic(list, "STATUS");
                        item.InyeccionEstadoPozoFinalMes = STATUS;
                    }

                }

                if (!string.IsNullOrEmpty(item.ProduccionEstadoPozoFinalMes))
                {
                    var list = await _formasValidate.ValidarEstadoPozo(item.ProduccionEstadoPozoFinalMes);
                    if (list?.Count == 0)
                    {
                        ErrorFormasStructura error = new ErrorFormasStructura()
                        {
                            sheet = key,
                            message = $"El Estado Pozo por produccion { item.ProduccionEstadoPozoFinalMes } no existe en la Base de Datos",
                            column = "",
                            row = 0,
                            value = item.ProduccionEstadoPozoFinalMes
                        };
                        Errors.Add(error);
                    }
                    if (list?.Count > 0)
                    {
                        var STATUS = ConvertTypes.ConverDataDynamic(list, "STATUS");
                        item.ProduccionEstadoPozoFinalMes = STATUS;
                    }
                    
                }

                bodyJson.Add(item);
            }

            return bodyJson;
        }

        private async Task ValidacionJson(string getUserId)
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
                                    pdenIds.Add(item.POZO, item.PDEN_ID);
                                }
                            }
                        }

                        if (dataService.root != null)
                        {
                            Element element = dataService.root.element;

                            if (element.MENSAJE == "Esta Forma ya fue cargada")
                            {
                                CultureInfo cultureInfo = new CultureInfo("es-co");
                                var dateTime = DateTime.Parse($"{JsonValidacion.ANIO}/{JsonValidacion.MES}", cultureInfo);
                                var mes = DateTime.Now;
                                DateTime mesActual = new DateTime(mes.Year, mes.Month - 1, 1);
                                var permisos = await _formaOficial.GetPrecarga("Forma Cuadro 4", dateTime, JsonValidacion.OPERADOR, JsonValidacion.CAMPO, JsonValidacion.CONTRATO);
                                if (permisos.Count() > 0)
                                {
                                    Response.permisoDeCargue = false;
                                    foreach (Aprobacionprecarga precarga in permisos)
                                    {
                                        if (precarga.Activo == 1 && precarga.Usuario == getUserId)
                                        {
                                            if (precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                                            {
                                                Response.permisoDeCargue = true;

                                            }
                                        }
                                      
                                    }


                                }
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
                                pdenIds.Add(registro.POZO, registro.PDEN_ID);
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
            DateTime mesActual = DateTime.Now;
            DateTime mesOperativo;
            
            if (date.Month == 1)
                mesOperativo = new DateTime(date.Year - 1, 12, 1);
            else
                mesOperativo = new DateTime(date.Year, date.Month - 1, 1);

            bool state = false;

            if (fechaForma < mesOperativo)
            {
                var permisos = await _formaOficial.GetPrecarga("Cargue Forma 21CR", fechaForma, operador, campo, contrato);
                if (permisos.Count > 0)
                {
                    foreach (Aprobacionprecarga precarga in permisos)
                    {
                        if (precarga.Activo == 1 && precarga.Usuario == getUserId)
                        {

                            if (precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                            {
                                Aprobacioncarga data = await _formaOficial.GetaFormAprobacion("Forma 21CR", precarga.FechaForma.Value, precarga.Operadora, precarga.Campo,
                                precarga.Contrato);
                                if (data != null)
                                {
                                    if (data.Formstate.Name != "Aprobada")
                                    {
                                        if (delete)
                                        {
                                            await _forma21Repository.Delete(data.IdForma.Value);
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
                Aprobacioncarga data = await _formaOficial.GetaFormAprobacion("Forma 21CR", fechaForma, operador, campo, contrato);
                var permisos = await _formaOficial.GetPrecarga("Cargue Forma 21CR", fechaForma, operador, campo, contrato);
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
                                        await _forma21Repository.Delete(data.IdForma.Value);
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
                                await _forma21Repository.Delete(data.IdForma.Value);
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

        //private async Task<bool> ValidaFechasPermisos(DateTime fechaForma, DateTime mesOperativo, string operador, string campo, string contrato, string getUserId, string forma)
        //{
        //    DateTime mesActual = DateTime.Now;
        //    bool state = false;

        //    try
        //    {
        //        if (fechaForma < mesOperativo)
        //        {
        //            var permisos = await repository.ConsultarPermisoCargua(forma, fechaForma, operador, contrato, getUserId);

        //            if (permisos.Rows.Count > 0)
        //            {
        //                state = false;
        //            }
        //            else
        //            {
        //                state = true;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        state = false;
        //    }


        //    return state;
        //}
    }
}
