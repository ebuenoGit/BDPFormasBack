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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules
{
    public class Forma4Business : IFormas4Business
    {

        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;
        private readonly IAdministracionFormas FormaOficial;
        private readonly IFormaValidate FormasValidate;
        private readonly IForma4Repository Forma4Repository;

        private readonly Administracion.AdmGrpc.AdmGrpcClient AdministracionService;
        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient FormasService;
        private readonly Commons.CommonGrpc.CommonGrpcClient CommonService;

        private readonly CabeceraFormas<DetalleC4> JsonValidacion = new CabeceraFormas<DetalleC4>();
        private readonly List<ErrorFormasStructura> Errors = new List<ErrorFormasStructura>();
        private readonly RequestFormas<ResponseForma4> Response = new RequestFormas<ResponseForma4>();

        private Forma4Header Header = new Forma4Header();
        private readonly CultureInfo cultureInfo = new CultureInfo("es-co");
        private readonly Backend.Formas.Utilities.SendMail.ISendMailService SendMailService;
        private string GetUserId = "";

        public Forma4Business(Utilities.Telemetry.ITelemetryException telemetryException,
            IAdministracionFormas _formaOficial,
                              IFormaValidate _formasValidate, IForma4Repository _repository,
                              Administracion.AdmGrpc.AdmGrpcClient administracionService, Ppdm.PpdmGrpc.PpdmGrpcClient formasService,
                              Commons.CommonGrpc.CommonGrpcClient commonService,
                              Backend.Formas.Utilities.SendMail.ISendMailService sendMailService
                              )
        {
            FormaOficial = _formaOficial;
            FormasValidate = _formasValidate;
            TelemetryException = telemetryException;
            Forma4Repository = _repository;
            AdministracionService = administracionService;
            FormasService = formasService;
            CommonService = commonService;
            SendMailService = sendMailService;
            Response.permisoDeCargue = true;
            JsonValidacion.RECARGAR = "0";
        }

        public async Task<ResponseBase<RequestFormas<ResponseForma4>>> LoadFile(List<RequestForma4> data, Stream _file, string fileName, string getUserId)
        {
            var response = new ResponseBase<RequestFormas<ResponseForma4>>();
            try
            {
                GetUserId = getUserId;

                if (data?.Count > 0)
                {
                    for (int i = 0; i <= data?.Count - 1; i++)
                    {
                        RequestForma4 item = data[i];
                        var info = item.info;
                        var dataForma = item.data;
                        // validacion
                        await ValidacionCabecera(i, info, getUserId);
                        await ValidacionBody(i, dataForma);

                        if (Errors.Count == 0)
                        {
                            ResponseForma4 res = new ResponseForma4
                            {
                                Info = info,
                                data = dataForma,
                                total = item.total
                            };
                            Response.data.Add(res);

                            await ValidacionJson(i);

                            if (Errors.Count == 0)
                            {

                                var upload = await CommonService.UploadFileAsync(new Commons.FileSystemUploadFileOptions
                                {
                                    FileName = fileName,
                                    FileData = ConvertTypes.ConvertToBase64(_file),
                                    Container = "formas",
                                    DestinationDirectory = new Commons.FileSystemItemInfo
                                    {
                                        Path = $"tmp/c4"
                                    }
                                });
                                Response.data[i].Info.FileName = fileName;

                            }
                            if (Errors.Count > 0)
                            {
                                res = new ResponseForma4();
                                Response.errors = Errors;
                            }

                        }
                        else
                        {
                            ResponseForma4 res = new ResponseForma4();
                            Response.errors = Errors;

                        }
                    }
                }
                else
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        message = "Por favor intentelo de nuevo ocurrio un error inesperado"
                    };
                    Errors.Add(error);
                    ResponseForma4 res = new ResponseForma4();
                    Response.errors = Errors;
                }
            }
            catch (Exception e)
            {
                response.Code = (int)HttpStatusCode.BadRequest;
                TelemetryException.RegisterException(e);
            }
            response.Data = Response;

            return response;
        }

        public async Task<ResponseBase<dynamic>> Create(List<ResponseForma4> data, string GetUserId, string userName)
        {
            try
            {
                var response = new ResponseBase<dynamic>();

                for (var i = 0; i < data.Count; i++)
                {

                    var item = data[i];
                    List<Entities.DTO.Dominios.DetalleForma4> dataForma = item.data;
                    Forma4Header headerForma = item.Info;
                    TotalForma4 totalForma = item.total;

                    try
                    {
                        var fechaForma = DateTime.Parse($"{headerForma.Mes}/{headerForma.Anio}", cultureInfo);
                        await ValidaFechasPermisos(fechaForma: fechaForma, operador: headerForma.Compania, campo: headerForma.Campo, contrato: headerForma.Contrato, getUserId: GetUserId, true);

                    }
                    catch (Exception e)
                    {
                        TelemetryException.RegisterException(e);
                    }

                    var info2 = dataForma[0];
                    Formc4 c4 = new Formc4()
                    {
                        Formc4id = Guid.NewGuid(),
                        Deliverysite = "",
                        Initialstock = decimal.Parse(info2.existenciaInicialBasica ?? "0"),
                        Deliveries = decimal.Parse(info2.entregas),
                        Vesselsdeadvolume = decimal.Parse(info2.perdidasTotal),
                        Linesdeadvolume = decimal.Parse(info2.llenadoLineasVasijas),
                        Finalexistence = decimal.Parse(info2.existenciaFinal),
                        Totalbalance = 0,
                        Contrato = headerForma.Contrato,
                        ContratoId = headerForma.ContratoId,
                        Campo = headerForma.Campo,
                        CampoId = headerForma.CampoId,
                        Apigrades = decimal.Parse(info2.caracteristicasCrudoGravedadAPI60),
                        Bsw = decimal.Parse(info2.caracteristicasCrudoBSW),
                        Sulfurcontent = decimal.Parse(info2.caracteristicasCrudoContenidoAzufre),
                        row_created_by = GetUserId,
                        row_created_date = DateTime.Now,
                        row_changed_by = GetUserId,
                        row_changed_date = DateTime.Now

                    };
                    await Forma4Repository.SaveFormac4(c4);


                    List<Formc4totalvolumedetail> listTotal = new List<Formc4totalvolumedetail>
                {
                    new Formc4totalvolumedetail()
                    {
                        Formc4totalvolumedetailid = Guid.NewGuid(),
                        Formid = c4.Formc4id,
                        Formation = headerForma.Formacion??"",
                        Activity = "",
                        Totalvolume = decimal.Parse(totalForma.produccionAsociadaTotal),
                        row_created_by = GetUserId,
                        row_created_date = DateTime.Now,
                        row_changed_by = GetUserId,
                        row_changed_date = DateTime.Now
                    },

                    new Formc4totalvolumedetail()
                    {
                        Formc4totalvolumedetailid = Guid.NewGuid(),
                        Formid = c4.Formc4id,
                        Formation = headerForma.Formacion,
                        Activity = "",
                        Totalvolume = decimal.Parse(totalForma.consumoOperacionesTotal),
                        row_created_by = GetUserId,
                        row_created_date = DateTime.Now,
                        row_changed_by = GetUserId,
                        row_changed_date = DateTime.Now
                    },


                    new Formc4totalvolumedetail()
                    {
                        Formc4totalvolumedetailid = Guid.NewGuid(),
                        Formid = c4.Formc4id,
                        Formation = headerForma.Formacion??"",
                        Activity = "",
                        Totalvolume = decimal.Parse(totalForma.perdidasTotal),
                        row_created_by = GetUserId,
                        row_created_date = DateTime.Now,
                        row_changed_by = GetUserId,
                        row_changed_date = DateTime.Now
                    },


                    new Formc4totalvolumedetail()
                    {
                        Formc4totalvolumedetailid = Guid.NewGuid(),
                        Formid = c4.Formc4id,
                        Formation = headerForma.Formacion??"",
                        Activity = "",
                        Totalvolume = decimal.Parse(totalForma.produccionGravableTotal),
                        row_created_by = GetUserId,
                        row_created_date = DateTime.Now,
                        row_changed_by = GetUserId,
                        row_changed_date = DateTime.Now
                    }
                };

                    await Forma4Repository.SaveFormac4Total(listTotal);


                    //detalle de la forma

                    List<Formc4netvolumedetail> listData = new List<Formc4netvolumedetail>();
                    foreach (Entities.DTO.Dominios.DetalleForma4 detalle in dataForma)
                    {
                        Formc4netvolumedetail objc4 = new Formc4netvolumedetail()
                        {
                            Formc4netvolumedetailid = Guid.NewGuid(),
                            PdenId = detalle.pden_id,
                            Formid = c4.Formc4id,
                            Formation = headerForma.Formacion ?? "",
                            Municipality = detalle.municipio,
                            Danecode = detalle.codigoDANE,
                            Activity = "007",
                            Basica = detalle.produccionAsociadaBasica,
                            Incremental = detalle.produccionAsociadaIncremental,
                            Volume = decimal.Parse(detalle.produccionAsociadaTotal),
                            Productiontype = "",
                            row_created_by = GetUserId,
                            row_created_date = DateTime.Now,
                            row_changed_by = GetUserId,
                            row_changed_date = DateTime.Now
                        };

                        listData.Add(objc4);

                        objc4 = new Formc4netvolumedetail()
                        {
                            Formc4netvolumedetailid = Guid.NewGuid(),
                            Formid = c4.Formc4id,
                            Formation = headerForma.Formacion ?? "",
                            Municipality = detalle.municipio,
                            Danecode = detalle.codigoDANE,
                            Activity = "018",
                            Basica = detalle.consumoOperacionesBasica,
                            Incremental = detalle.consumoOperacionesIncremental,
                            Volume = decimal.Parse(detalle.consumoOperacionesTotal),
                            Productiontype = "",
                            PdenId = detalle.pden_id,
                            row_created_by = GetUserId,
                            row_created_date = DateTime.Now,
                            row_changed_by = GetUserId,
                            row_changed_date = DateTime.Now
                        };

                        listData.Add(objc4);

                        objc4 = new Formc4netvolumedetail()
                        {
                            Formc4netvolumedetailid = Guid.NewGuid(),
                            Formid = c4.Formc4id,
                            PdenId = detalle.pden_id,
                            Formation = headerForma.Formacion ?? "",
                            Municipality = detalle.municipio,
                            Danecode = detalle.codigoDANE,
                            Activity = "032",
                            Basica = detalle.perdidasBasica,
                            Incremental = detalle.perdidasIncremental,
                            Volume = decimal.Parse(detalle.perdidasTotal),
                            Productiontype = "",
                            row_created_by = GetUserId,
                            row_created_date = DateTime.Now,
                            row_changed_by = GetUserId,
                            row_changed_date = DateTime.Now
                        };
                        listData.Add(objc4);

                        objc4 = new Formc4netvolumedetail()
                        {
                            Formc4netvolumedetailid = Guid.NewGuid(),
                            Formid = c4.Formc4id,
                            PdenId = detalle.pden_id,
                            Formation = headerForma.Formacion ?? "",
                            Municipality = detalle.municipio,
                            Danecode = detalle.codigoDANE,
                            Activity = "030",
                            Basica = detalle.produccionGravableBasica,
                            Incremental = detalle.produccionGravableIncremental,
                            Volume = decimal.Parse(detalle.produccionGravableTotal),
                            Productiontype = "",
                            row_created_by = GetUserId,
                            row_created_date = DateTime.Now,
                            row_changed_by = GetUserId,
                            row_changed_date = DateTime.Now
                        };
                        listData.Add(objc4);
                    }
                    await Forma4Repository.SaveFormaDetail(listData);


                    var forStateid = await FormaOficial.GetState("En Proceso de Aprobación");

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
                        Formid = c4.Formc4id,
                        Generationjobid = 1,
                        Iqistatus = 0,
                        Usersigning = "",
                        Minrepsigning = "",
                        Formname = "Forma Cuadro 4"
                    };

                    await Forma4Repository.SaveConcret(concreteform);

                    string fecha = $"{headerForma.Anio}/{headerForma.Mes}";
                    DateTime dateForma = DateTime.Parse(fecha, cultureInfo);
                    var datauser = await AdministracionService.UsuarioAprobadorAsync(new Administracion.RequestAprobaciones { NombreForma = "Cargue Forma Cuadro 4" });
                    var dataService = datauser.Data.Deserialize<UsuariosRecursosAprobaciones>();
                    if (dataService != null)
                    {

                        var codeUser = dataService.CodigoUsuario;
                        // var dateTime = DateTime.Parse(date, cultureInfo);
                        var _aprobacion = new Aprobacioncarga
                        {
                            IdForma = c4.Formc4id,
                            FechaForma = dateForma,
                            Usuario = GetUserId,
                            FechaCarga = DateTime.Now,
                            Estado = forStateid,
                            FechaActualizacion = DateTime.Now,
                            UsuarioAprobador = dataService.CodigoUsuario,
                            UsuarioNombre = userName,
                            UsuarioNombreAprobador = dataService.NombreUsuario,
                            FormaName = "Forma Cuadro 4",
                            UrlForma = $"FormaCuadro4/{headerForma.Anio}/{headerForma.Mes}/{headerForma.FileName}",
                            Campo = headerForma.Campo,
                            Contrato = headerForma.Contrato,
                            Operadora = headerForma.Compania,
                            ComparativoGas = 0,
                            ComparativoAgua = 0,
                            ComparativoCrudo = 0
                        };
                        await FormaOficial.CreteAprobacion(_aprobacion);



                        var moveFile = await CommonService.MoveItemAsync(new Commons.FileSystemMoveItemOptions
                        {
                            Container = "formas",
                            DestinationDirectory = new Commons.FileSystemItemInfo
                            {
                                Path = $"FormaCuadro4/{headerForma.Anio}/{headerForma.Mes}/{headerForma.FileName}"
                            },
                            Item = new Commons.FileSystemItemInfo
                            {
                                Path = $"tmp/c4/{headerForma.FileName}"
                            }
                        });


                        try
                        {
                            string asunto = "Notificación de carga Forma Cuadro 4";
                            MailForma9 mailForma = new MailForma9();

                            string view = mailForma.GetView(asunto, dataService.NombreUsuario, headerForma.Compania, headerForma.Contrato, headerForma.Campo, fecha,
                                "Sea detectado la carga de una forma  relacionada a la Forma Cuadro 4 en estado de <b>En Proceso de Aprobación</b> ");
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
                            TelemetryException.RegisterException(e);
                        }
                    }
                }
                response.Code = (int)HttpStatusCode.OK;
                response.Message = Messages.Created;
                response.Data = data;

                return response;
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);

                return new ResponseBase<dynamic>(HttpStatusCode.InternalServerError, exc.Message);
            }
        }

        private async Task ValidacionCabecera(int key, Forma4Header data, string getUserId)
        {
            Header = data;
            JsonValidacion.FORMA_CODIGO = "4";

            if (string.IsNullOrEmpty(data.Contrato))
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

            if (!string.IsNullOrEmpty(data.Contrato))
            {
                var list = await FormasValidate.ValidaContrato(data.Contrato);

                if (list == null)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = "El Contrato no existe en la Base de Datos",
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
                var list = await FormasValidate.ValidaCampo(data.Campo);
                if (list == null)
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

            if (string.IsNullOrEmpty(data.Compania))
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

            if (!string.IsNullOrEmpty(data.Compania))
            {
                var list = await FormasValidate.ValidaOperador(data.Compania);
                if (list == null)
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

            if (string.IsNullOrEmpty(data.Bateria))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El formato no tiene una Bateria",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);
            }

            if (!string.IsNullOrEmpty(data.Bateria))
            {
                var list = await FormasValidate.ValidaCampo(data.Bateria);
                if (list == null)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = "La Bateria no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Bateria
                    };
                    Errors.Add(error);
                }

            }

            if (string.IsNullOrEmpty(data.Lugar))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El formato no tiene un Lugar",
                    column = "",
                    row = 0,
                    value = ""
                };
                Errors.Add(error);

            }



            if (!string.IsNullOrEmpty(data.Contrato) && !string.IsNullOrEmpty(data.Campo) && !string.IsNullOrEmpty(data.Compania))
            {
                var list = await FormasValidate.ValidaCampoContratoOperador(campo: data.Campo, contrato: data.Contrato, operador: data.Compania);
                if (list?.Count > 0)
                {
                    string operador_id = ConvertTypes.ConverDataDynamic(list, "OPERADOR_ID");
                    string contrato_id = ConvertTypes.ConverDataDynamic(list, "CONTRATO_ID");
                    string campo_id = ConvertTypes.ConverDataDynamic(list, "CAMPO_ID");
                    JsonValidacion.CONTRATO_ID = contrato_id;
                    JsonValidacion.CAMPO_ID = campo_id;
                    JsonValidacion.OPERADOR_ID = operador_id;
                    JsonValidacion.CAMPO = data.Campo;
                    JsonValidacion.OPERADOR = data.Compania;
                    JsonValidacion.CONTRATO = data.Contrato;
                    Header.CompaniaId = operador_id;
                    Header.CampoId = campo_id;
                    Header.ContratoId = contrato_id;
                    Header.Campo = data.Campo;
                    Header.Contrato = data.Contrato;
                    Header.Compania = data.Compania;
                }
                else
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = $"No existe información relacionada para el Operador {data.Compania} con el campo {data.Campo} y el contato {data.Contrato}",
                        column = "",
                        row = 0,
                        value = ""
                    };
                    Errors.Add(error);

                }
            }

            /* if (string.IsNullOrEmpty(data.Formacion))
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
            } */

            /* if (!string.IsNullOrEmpty(data.Formacion))
            {
                var list = await FormasValidate.ValidaFormacion(data.Formacion);

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
                        message = "La Formación no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.Formacion
                    };
                    Errors.Add(error);
                }

            } */

            JsonValidacion.ANIO = data.Anio;
            JsonValidacion.MES = data.Mes;
            var fechaForma = DateTime.Parse($"{data.Mes}/{data.Anio}", cultureInfo);

            int day = fechaForma.AddMonths(1).AddDays(-1).Day;
            DateTime volumDate = new DateTime(fechaForma.Year, fechaForma.Month, day);

            var res = await FormasValidate.ValidaCompaniaCampoEstado(data.Compania, data.Campo, volumDate.ToString("yyyyMMdd"));

            if (res?.Count == 0 || res == null)
            {
                Errors.Add(new ErrorFormasStructura() { sheet = key, column = "", row = 0, message = "Es posible que el campo contrato no este activo  o  la fecha de cargue no esta dentro del rango de la fecha efectiva o fecha expedicion ", value = data.Contrato });
            }


            var fechaActiva = await ValidaFechasPermisos(fechaForma: fechaForma, operador: data.Compania, campo: data.Campo, contrato: data.Contrato, getUserId: getUserId, false);
            if (fechaActiva == false)
            {
                Response.permisoDeCargue = false;
            }
        }

        private async Task ValidacionBody(int key, List<Entities.DTO.Dominios.DetalleForma4> listData)
        {
            List<DetalleC4> detalleC4 = new List<DetalleC4>();
            List<Entities.DTO.Dominios.DetalleForma4> dataList = new List<Entities.DTO.Dominios.DetalleForma4>();

            string bsw = "";
            string api = "";
            string azufre = "";
            string gravedad = "";
            string sal = "";

            foreach (Entities.DTO.Dominios.DetalleForma4 item in listData)
            {
                if (!string.IsNullOrEmpty(bsw))
                {
                    if (item.caracteristicasCrudoBSW != bsw)
                    {
                        ErrorFormasStructura error = new ErrorFormasStructura()
                        {
                            sheet = key,
                            message = "El valor de BSW son diferentes para los registros de la forma ",
                            column = "caracteristicasCrudoBSW",
                            row = 0,
                            value = ""
                        };

                        Errors.Add(error);
                    }
                }
                if (string.IsNullOrEmpty(bsw))
                {
                    bsw = item.caracteristicasCrudoBSW;
                }


                if (!string.IsNullOrEmpty(api))
                {
                    if (item.caracteristicasCrudoGravedadAPI60 != api)
                    {
                        ErrorFormasStructura error = new ErrorFormasStructura()
                        {
                            sheet = key,
                            message = "El valor de API son diferentes para los registros de la forma ",
                            column = "caracteristicasCrudoGravedadAPI60",
                            row = 0,
                            value = ""
                        };
                        Errors.Add(error);
                    }
                }
                if (string.IsNullOrEmpty(api))
                {
                    api = item.caracteristicasCrudoGravedadAPI60;
                }

                if (!string.IsNullOrEmpty(azufre))
                {
                    if (item.caracteristicasCrudoContenidoAzufre != azufre)
                    {
                        ErrorFormasStructura error = new ErrorFormasStructura()
                        {
                            sheet = key,
                            message = "El valor de AZUFRE son diferentes para los registros de la forma ",
                            column = "caracteristicasCrudoContenidoAzufre",
                            row = 0,
                            value = ""
                        };
                        Errors.Add(error);
                    }
                }
                if (string.IsNullOrEmpty(azufre))
                {
                    azufre = item.caracteristicasCrudoContenidoAzufre;
                }

                if (!string.IsNullOrEmpty(gravedad))
                {
                    if (item.caracteristicasCrudoGravedadEspecifica != gravedad)
                    {
                        ErrorFormasStructura error = new ErrorFormasStructura()
                        {
                            sheet = key,
                            message = "El valor de Gravedad especifica son diferentes para los registros de la forma ",
                            column = "caracteristicasCrudoGravedadEspecifica",
                            row = 0,
                            value = ""
                        };
                        Errors.Add(error);
                    }
                }
                if (string.IsNullOrEmpty(gravedad))
                {
                    gravedad = item.caracteristicasCrudoGravedadEspecifica;
                }

                if (!string.IsNullOrEmpty(sal))
                {
                    if (item.caracteristicasCrudoContenidoSAL != sal)
                    {
                        ErrorFormasStructura error = new ErrorFormasStructura()
                        {
                            sheet = key,
                            message = "El valor de Contenido de sal son diferentes para los registros de la forma ",
                            column = "Contenido de sal",
                            row = 0,
                            value = ""
                        };
                        Errors.Add(error);
                    }
                }
                if (string.IsNullOrEmpty(sal))
                {
                    sal = item.caracteristicasCrudoContenidoSAL;
                }

                if (!string.IsNullOrEmpty(item.municipio) && !string.IsNullOrEmpty(item.codigoDANE))
                {
                    var list = await FormasValidate.ValidaCodigoDane(item.codigoDANE);
                    if (list?.Count > 0)
                    {
                        DetalleC4 c4 = new DetalleC4()
                        {
                            MUNICIPIO = item.codigoDANE
                        };
                        detalleC4.Add(c4);
                    }
                    else
                    {
                        ErrorFormasStructura error = new ErrorFormasStructura()
                        {
                            sheet = key,
                            message = $"El Municipio {item.municipio} con codígo dane {item.codigoDANE} no existe en la Base de Datos",
                            column = "",
                            row = 0,
                            value = ""
                        };
                        Errors.Add(error);
                    }
                }
            }

            JsonValidacion.REGISTRO = detalleC4;
        }

        private async Task ValidacionJson(int key)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };

            List<CabeceraFormas<DetalleC4>> listJson = new List<CabeceraFormas<DetalleC4>>
            {
                JsonValidacion
            };

            var json = listJson.Serialize(settings);
            var res = await FormasService.ValidarFormasMinAsync(new Ppdm.RequestBase { SJson = json });
            if (res.Code == 200)
            {

                try
                {
                    var dataService = res.Data.Deserialize<FormasBase<RegistrosC4>>(settings);
                    var registro = dataService.FORMAS?.FORMA?.REGISTRO;
                    var produccion = dataService.FORMAS?.FORMA?.VAL_PRODUCCION;

                    if (registro != null)
                    {
                        if (string.IsNullOrEmpty(registro.PDEN_ID))
                        {
                            ErrorFormasStructura error = new ErrorFormasStructura()
                            {
                                message = $"No se encontrarón datos relacionado para la información validada con codígo de municipio {registro.MUNICIPIO} "
                            };
                            Errors.Add(error);
                        }

                        if (!string.IsNullOrEmpty(registro.PDEN_ID))
                        {
                            var item = Response.data[key].data;
                            item.FirstOrDefault(x => x.codigoDANE == registro.MUNICIPIO).pden_id = registro.PDEN_ID;
                            Response.data[key].data = item;
                        }
                    }
                    if (produccion != null)
                    {
                        for (var i = 0; i <= Response.data[key].data.Count - 1; i++)
                        {
                            var total = Response.data[key].data[i].produccionAsociadaTotal;
                            if (total != produccion)
                            {
                                ErrorFormasStructura error = new ErrorFormasStructura()
                                {
                                    message = $"El valor de la producción presenta diferencias respecto a la forma 9"
                                };
                                Errors.Add(error);
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
                            var permisos = await FormaOficial.GetPrecarga("Forma Cuadro 4", dateTime, JsonValidacion.OPERADOR, JsonValidacion.CAMPO, JsonValidacion.CONTRATO);
                            if (permisos.Count() > 0)
                            {
                                foreach (Aprobacionprecarga precarga in permisos)
                                {
                                    if (precarga.Activo == 1 && precarga.Usuario == GetUserId)
                                    {
                                        if (precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                                        {
                                            var existePdenId = Response.data[0].data.FirstOrDefault(x => x.pden_id != null);
                                            if (!string.IsNullOrEmpty(existePdenId?.pden_id))
                                            {
                                                Response.permisoDeCargue = true;
                                            }

                                            if (string.IsNullOrEmpty(existePdenId.pden_id))
                                            {
                                                ErrorFormasStructura error = new ErrorFormasStructura()
                                                {
                                                    message = $"No se registraron claves intente de nuevo "
                                                };
                                                Errors.Add(error);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Response.permisoDeCargue = false;
                                    }
                                }
                            }
                            else
                            {
                                Response.permisoDeCargue = false;
                            }

                        }
                        else
                        {
                            ErrorFormasStructura error = new ErrorFormasStructura()
                            {
                                message = element.MENSAJE
                            };

                            Errors.Add(error);
                        }
                    }
                }
                catch
                {
                    var dataService = res.Data.Deserialize<FormasBase<List<RegistrosC4>>>(settings);
                    var registros = dataService.FORMAS?.FORMA?.REGISTRO;
                    var produccion = dataService.FORMAS?.FORMA?.VAL_PRODUCCION;

                    if (registros.Count > 0)
                    {
                        foreach (var registro in registros)
                        {
                            if (string.IsNullOrEmpty(registro.PDEN_ID))
                            {
                                ErrorFormasStructura error = new ErrorFormasStructura()
                                {
                                    message = $"No se encontrarón datos relacionado para la información validada con codígo de municipio {registro.MUNICIPIO} "
                                };
                                Errors.Add(error);
                            }

                            if (!string.IsNullOrEmpty(registro.PDEN_ID))
                            {
                                var item = Response.data[key].data;
                                item.FirstOrDefault(x => x.codigoDANE == registro.MUNICIPIO && x.pden_id == null).pden_id = registro.PDEN_ID;
                                Response.data[key].data = item;
                            }
                        }
                    }

                    if (produccion != null)
                    {
                        for (var i = 0; i <= Response.data[key].data.Count - 1; i++)
                        {
                            var total = Response.data[key].data[i].produccionAsociadaTotal;
                            if (total != produccion)
                            {
                                ErrorFormasStructura error = new ErrorFormasStructura()
                                {
                                    message = $"El valor de la producción presenta diferencias respecto a la forma 9"
                                };
                                Errors.Add(error);
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
                            var permisos = await FormaOficial.GetPrecarga("Forma Cuadro 4", dateTime, JsonValidacion.OPERADOR, JsonValidacion.CAMPO, JsonValidacion.CONTRATO);
                            if (permisos.Count() > 0)
                            {
                                foreach (Aprobacionprecarga precarga in permisos)
                                {
                                    if (precarga.Activo == 1 && precarga.Usuario == GetUserId)
                                    {
                                        if (precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                                        {
                                            var existePdenId = Response.data[0].data.FirstOrDefault(x => x.pden_id != null);
                                            if (!string.IsNullOrEmpty(existePdenId?.pden_id))
                                            {
                                                Response.permisoDeCargue = true;
                                            }

                                            if (string.IsNullOrEmpty(existePdenId.pden_id))
                                            {
                                                ErrorFormasStructura error = new ErrorFormasStructura()
                                                {
                                                    message = $"No se registraron claves intente de nuevo "
                                                };
                                                Errors.Add(error);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Response.permisoDeCargue = false;
                            }

                        }
                        else
                        {
                            ErrorFormasStructura error = new ErrorFormasStructura()
                            {
                                message = element.MENSAJE
                            };

                            Errors.Add(error);
                        }


                    }
                }
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
                var permisos = await FormaOficial.GetPrecarga("Cargue Forma Cuadro 4", fechaForma, operador, campo, contrato);
                if (permisos.Count() > 0)
                {
                    foreach (Aprobacionprecarga precarga in permisos)
                    {
                        if (precarga.Activo == 1 && precarga.Usuario == getUserId)
                        {

                            if (precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                            {
                                Aprobacioncarga data = await FormaOficial.GetaFormAprobacion("Forma Cuadro 4", precarga.FechaForma.Value, precarga.Operadora, precarga.Campo,
                                precarga.Contrato);
                                if (data != null)
                                {
                                    if (data.Formstate.Name != "Aprobada")
                                    {
                                        if (delete)
                                        {
                                            await Forma4Repository.Delete(data.IdForma.Value);
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
                Aprobacioncarga data = await FormaOficial.GetaFormAprobacion("Forma Cuadro 4", fechaForma, operador, campo,
                     contrato);
                if (data != null)
                {
                    var permisos = await FormaOficial.GetPrecarga("Cargue Forma Cuadro 4", fechaForma, operador, campo, contrato);
                    foreach (Aprobacionprecarga precarga in permisos)
                    {
                        if (precarga.Activo == 1 && precarga.Usuario == getUserId && precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                        {
                            if (data.Formstate.Name != "Aprobado" && data.Usuario == getUserId)
                            {
                                if (delete)
                                {
                                    await Forma4Repository.Delete(data.IdForma.Value);
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
                    }

                    if (delete)
                    {
                        await Forma4Repository.Delete(data.IdForma.Value);
                    }
                    state = true;

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