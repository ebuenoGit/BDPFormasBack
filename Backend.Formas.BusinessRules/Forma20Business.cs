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
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules
{
    public class Forma20Business : IForma20Business
    {
        private readonly IForma20Repository _repository;

        private readonly IFormaValidate FormasValidate;
        private readonly IAdministracionFormas FormaOficial;

        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;
        private readonly RequestFormas<Entities.DTO.Dominios.Forma20Request> RequestForma = new RequestFormas<Entities.DTO.Dominios.Forma20Request>();
        private readonly CabeceraFormas<PozoF20> JsonValidacion = new CabeceraFormas<PozoF20>();
        private readonly List<ErrorFormasStructura> Errors = new List<ErrorFormasStructura>();

        private readonly Administracion.AdmGrpc.AdmGrpcClient AdministracionService;
        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient FormasService;
        private readonly Commons.CommonGrpc.CommonGrpcClient CommonService;
        private readonly CultureInfo cultureInfo = new CultureInfo("es-co");
        private readonly Backend.Formas.Utilities.SendMail.ISendMailService SendMailService;
        private Forma20Header Header = new Forma20Header();
        private List<BodyForma20> Body = new List<BodyForma20>();

        private readonly IBaseRepository<Backend.Formas.Entities.DAO.Form20> RepositoryForma;
        private readonly IBaseRepository<Backend.Formas.Entities.DAO.Form20detail> RepositoryFormaDetail;
        private readonly IBaseRepository<Backend.Formas.Entities.DAO.Form20totalvolumedetail> RepositoryFormaTotal;
        private readonly IBaseRepository<Backend.Formas.Entities.DAO.Form20productiondetail> RepositoryFormaProduction;
        private readonly IBaseRepository<Concreteform> RepositoryConcreform;

        public Forma20Business(IForma20Repository repository,
                                            Utilities.Telemetry.ITelemetryException telemetryException,
                                            IFormaValidate validate,
                                            IAdministracionFormas _formaOficial,
                                            Administracion.AdmGrpc.AdmGrpcClient _AdministracionService,
                                            Backend.Formas.Utilities.SendMail.ISendMailService _SendMailService,
                                            IBaseRepository<Backend.Formas.Entities.DAO.Form20> repositoryForma,
                                            IBaseRepository<Backend.Formas.Entities.DAO.Form20detail> repositoryFormaDetail,
                                            IBaseRepository<Backend.Formas.Entities.DAO.Form20totalvolumedetail> repositoryFormaTotal,
                                            IBaseRepository<Backend.Formas.Entities.DAO.Form20productiondetail> repositoryFormaProduction,
                                            IBaseRepository<Concreteform> concreForm,
             Ppdm.PpdmGrpc.PpdmGrpcClient formasService,
             Commons.CommonGrpc.CommonGrpcClient commonService,
             Backend.Formas.Utilities.SendMail.ISendMailService sendMailService
        )
        {
            _repository = repository;
            TelemetryException = telemetryException;
            FormasValidate = validate;
            AdministracionService = _AdministracionService;
            SendMailService = _SendMailService;
            FormaOficial = _formaOficial;
            RepositoryForma = repositoryForma;
            RepositoryFormaDetail = repositoryFormaDetail;
            RepositoryFormaProduction = repositoryFormaProduction;
            RepositoryFormaTotal = repositoryFormaTotal;
            RepositoryConcreform = concreForm;
            FormasService = formasService;
            CommonService = commonService;
            SendMailService = sendMailService;
            RequestForma.permisoDeCargue = true;
            JsonValidacion.RECARGAR = "0";
        }


        public async Task<ResponseBase<RequestFormas<Entities.DTO.Dominios.Forma20Request>>> Create(List<Entities.DTO.Dominios.Forma20Request> data, string getUserId, string getNames)
        {
            var response = new ResponseBase<RequestFormas<Entities.DTO.Dominios.Forma20Request>>();

            foreach (Entities.DTO.Dominios.Forma20Request item in data)
            {
                var info = item.Info;
                var detalle = item.data;
                string fecha = $"{info.Anio}/{info.Mes}";
                DateTime dateForma = DateTime.Parse(fecha, cultureInfo);

                await ValidaFechasPermisos(dateForma, operador: info.Compania, campo: info.Campo, contrato: info.Contrato, getUserId, true);

                Form20 f20 = new Form20()
                {
                    Form20id = Guid.NewGuid(),
                    Formation = info.Formacion,
                    Block = info.Bloque,
                    Oilfield = info.Formacion,
                    Structure = info.Estructura,
                    Member = "",
                    operador = info.Compania,
                    operadorId = info.CompaniaId,
                    contrato = info.Contrato,
                    contratoId = info.ContratoId,
                    campo = info.Campo,
                    campoId = info.CampoId,
                    formacion_id = info.FormacionId,
                    formacion_set_id = info.FormacionSetId,
                    row_created_by = getUserId,
                    row_created_date = DateTime.Now,
                    row_changed_by = getUserId,
                    row_changed_date = DateTime.Now
                };

                try
                {
                    await RepositoryForma.AddAsync(f20);
                }
                catch (Exception ex)
                {
                    TelemetryException.RegisterException(ex);
                }

                try
                {
                    List<Form20detail> listdetail = new List<Form20detail>();
                    List<Form20productiondetail> lisproduction = new List<Form20productiondetail>();

                    foreach (var row in detalle)
                    {
                        //inyecion
                        Form20detail f20detail = new Form20detail()
                        {
                            Form20detailid = Guid.NewGuid(),
                            Formid = f20.Form20id,
                            Oilwell = row.produccionPozo,
                            Danecode = "",
                            PdenId = row.pdenId,
                            Zone = "",
                            Widays = decimal.Parse(row.inyeccionMes),
                            Wiaccumulateddays = decimal.Parse(row.inyeccionAcumulados),
                            Pressure = decimal.Parse(row.inyeccionPresionMediaInyeccion),
                            Widailywater = 0,
                            Wimonthlywater = decimal.Parse(row.inyeccionVolumenAguaInyMesBls),
                            Wiaccumulatedwater = decimal.Parse(row.inyeccionVolumenAguaInyAcumuladoBls),
                            Poolname = row.inyeccionPozo,
                            Oilwellfinalstate = row.estadoFinalMes,
                            row_created_by = getUserId,
                            row_created_date = DateTime.Now,
                            row_changed_by = getUserId,
                            row_changed_date = DateTime.Now
                        };

                        listdetail.Add(f20detail);
                        //produción
                        Form20productiondetail total = new Form20productiondetail()
                        {
                            Form20productiondetailid = Guid.NewGuid(),
                            Oilwell = row.produccionPozo,
                            Danecode = "",
                            Productionmethod = "",
                            Monthdays = decimal.Parse(row.produccionDiasMes),
                            Accumulatedays = decimal.Parse(row.produccionDiasAcumulados),
                            Dailyoilproduction = 0,
                            Monthlyoilproduction = decimal.Parse(row.produccionPetroleoMensualBls),
                            Accumulateoilproduction = decimal.Parse(row.produccionPetroleoAcumuladoBbl),
                            Correctionfactor = 0,
                            Dailywater = 0,
                            Monthlywater = decimal.Parse(row.produccionAguaMensualBls),
                            Accumulatedwater = decimal.Parse(row.produccionAguaAcumuladaBls),
                            Pressure = decimal.Parse(row.presionFondo),
                            Oilwellfinalstate = row.estadoFinalMes,
                            Formid = f20.Form20id,
                            row_created_by = getUserId,
                            row_created_date = DateTime.Now,
                            row_changed_by = getUserId,
                            row_changed_date = DateTime.Now
                        };
                        lisproduction.Add(total);

                    }

                    await RepositoryFormaDetail.AddAsync(listdetail);
                    await RepositoryFormaProduction.AddAsync(lisproduction);

                }
                catch (Exception ex)
                {
                    TelemetryException.RegisterException(ex);
                }
                try
                {
                    var forStateid = await FormaOficial.GetState("En Proceso de Aprobación");

                    Concreteform concreteform = new Concreteform
                    {
                        Concreteformid = Guid.NewGuid(),
                        Maincampid = 0,
                        Company = info.Compania,
                        Contract = info.Contrato,
                        Battery = "",
                        Tank = "",
                        Month = decimal.Parse(info.Mes),
                        Year = decimal.Parse(info.Anio),
                        Explotationmodality = info.ModalidadExplotacion,
                        Annotations = "",
                        Version = 1,
                        Currentstate = forStateid,
                        Generationflag = 1,
                        Campid = 0,
                        Pdenid = info.CompaniaId,
                        Formid = f20.Form20id,
                        Generationjobid = 1,
                        Iqistatus = 0,
                        Usersigning = "",
                        Minrepsigning = "",
                        Formname = "Forma 20CR"
                    };

                    await RepositoryConcreform.AddAsync(concreteform);


                    var datauser = await AdministracionService.UsuarioAprobadorAsync(new Administracion.RequestAprobaciones { NombreForma = "Cargue Forma 20CR" });
                    var dataService = datauser.Data.Deserialize<UsuariosRecursosAprobaciones>();
                    if (dataService != null)
                    {

                        var codeUser = dataService.CodigoUsuario;
                        // var dateTime = DateTime.Parse(date, cultureInfo);
                        var _aprobacion = new Aprobacioncarga
                        {
                            IdForma = f20.Form20id,
                            FechaForma = dateForma,
                            Usuario = getUserId,
                            FechaCarga = DateTime.Now,
                            Estado = forStateid,
                            FechaActualizacion = DateTime.Now,
                            UsuarioAprobador = dataService.CodigoUsuario,
                            UsuarioNombre = getNames,
                            UsuarioNombreAprobador = dataService.NombreUsuario,
                            FormaName = "Forma 20CR",
                            UrlForma = $"Forma20/{info.Anio}/{info.Mes}/{info.FileName}",
                            Campo = info.Campo,
                            Contrato = info.Contrato,
                            Operadora = info.Compania,
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
                                Path = $"Forma20/{info.Anio}/{info.Mes}/{info.FileName}"
                            },
                            Item = new Commons.FileSystemItemInfo
                            {
                                Path = $"tmp/f20/{info.FileName}"
                            }
                        });


                        try
                        {
                            string asunto = "Notificación de carga Forma 20CR";
                            MailForma9 mailForma = new MailForma9();

                            string view = mailForma.GetView(asunto, dataService.NombreUsuario, info.Compania, info.Contrato, info.Campo, fecha,
                                "Se ha detectado la carga de una forma  relacionada a la Forma 20CR en estado de <b>En Proceso de Aprobación</b> ");
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
                catch (Exception e)
                {
                    TelemetryException.RegisterException(e);
                }
            }
            return response;
        }

        public async Task<ResponseBase<RequestFormas<Entities.DTO.Dominios.Forma20Request>>> LoadFile(List<Entities.DTO.Dominios.Forma20Request> data, Stream fileStream, string fileName, string getUserId)
        {
            var response = new ResponseBase<RequestFormas<Entities.DTO.Dominios.Forma20Request>>();
            List<Entities.DTO.Dominios.Forma20Request> dataResponse = new List<Entities.DTO.Dominios.Forma20Request>();

            for (int i = 0; i < data.Count; i++)
            {
                Entities.DTO.Dominios.Forma20Request item = data[i];
                Body = item.data;
                await HeaderForma(item.Info, i, getUserId);
                BodyForma(item.data);
                await ValidacionJson(getUserId);


                if (Errors.Count > 0)
                {
                    Entities.DTO.Dominios.Forma20Request informacion = new Entities.DTO.Dominios.Forma20Request
                    {
                        Info = Header
                    };
                    dataResponse.Add(informacion);
                    RequestForma.errors = Errors;
                }

                if (Errors.Count == 0)
                {

                    var upload = await CommonService.UploadFileAsync(new Commons.FileSystemUploadFileOptions
                    {
                        FileName = fileName,
                        FileData = ConvertTypes.ConvertToBase64(fileStream),
                        Container = "formas",
                        DestinationDirectory = new Commons.FileSystemItemInfo
                        {
                            Path = $"tmp/f20"
                        }
                    });
                    Header.FileName = fileName;

                    Entities.DTO.Dominios.Forma20Request informacion = new Entities.DTO.Dominios.Forma20Request
                    {
                        Info = Header,
                        data = item.data
                    };
                    dataResponse.Add(informacion);
                }
            }
            RequestForma.data = dataResponse;
            response.Data = RequestForma;
            return response;
        }

        private async Task HeaderForma(Forma20Header header, int key, string getUserId)
        {

            JsonValidacion.FORMA_CODIGO = "20";

            Header = header;

            if (string.IsNullOrEmpty(header.Campo))
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
            if (!string.IsNullOrEmpty(header.Campo))
            {
                var list = await FormasValidate.ValidaCampo(header.Campo);
                if (list == null)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = "El Campo no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = header.Campo
                    };
                    Errors.Add(error);
                }

            }

            if (string.IsNullOrEmpty(header.Compania))
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

            if (!string.IsNullOrEmpty(header.Compania))
            {
                var list = await FormasValidate.ValidaOperador(header.Compania);
                if (list == null)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = "El Operador o Compañía no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = header.Compania
                    };
                    Errors.Add(error);
                }

            }



            if (string.IsNullOrEmpty(header.Formacion))
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

            if (!string.IsNullOrEmpty(header.Campo) && !string.IsNullOrEmpty(header.Compania))
            {
                var list = await FormasValidate.ValidaCompaniaCampo(campo: header.Campo, compania: header.Compania);
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
                    JsonValidacion.CAMPO = header.Campo;
                    JsonValidacion.OPERADOR = header.Compania;
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
                        message = $"No existe información relacionada para el Operador {header.Compania} con el campo {header.Campo} y el contrato {header.Contrato}",
                        column = "",
                        row = 0,
                        value = ""
                    };
                    Errors.Add(error);

                }
            }

            if (!string.IsNullOrEmpty(header.Formacion))
            {
                var list = await FormasValidate.ValidaFormacion(header.Formacion);

                if (list?.Count > 0)
                {
                    var STRAT_UNIT_ID = ConvertTypes.ConverDataDynamic(list, "STRAT_UNIT_ID");
                    var STRAT_NAME_SET_ID = ConvertTypes.ConverDataDynamic(list, "STRAT_NAME_SET_ID");
                    JsonValidacion.FORMACION_ID = STRAT_UNIT_ID;
                    JsonValidacion.FORMACION_SET_ID = STRAT_NAME_SET_ID;
                    JsonValidacion.FORMACION = header.Formacion;
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
                        value = header.Formacion
                    };
                    Errors.Add(error);
                }

            }

            JsonValidacion.ANIO = header.Anio;
            JsonValidacion.MES = header.Mes;
            DateTime fechaForma = DateTime.Parse($"{header.Anio}/{header.Mes}");

            int day = fechaForma.AddMonths(1).AddDays(-1).Day;
            DateTime volumDate = new DateTime(fechaForma.Year, fechaForma.Month, day);

            var res = await FormasValidate.ValidaCompaniaCampoEstado(header.Compania, header.Campo, volumDate.ToString("yyyyMMdd"));

            if (res?.Count == 0 || res == null)
            {
                Errors.Add(new ErrorFormasStructura() { sheet = key, column = "", row = 0, message = "Es posible que el campo contrato no este activo  o  la fecha de cargue no esta dentro del rango de la fecha efectiva o fecha expedicion ", value = header.Contrato });
            }

            var state = await ValidaFechasPermisos(fechaForma, operador: header.Compania, campo: header.Campo, contrato: header.Contrato, getUserId, false);
            if (state == false)
            {
                RequestForma.permisoDeCargue = false;
            }

        }
        private void BodyForma(List<BodyForma20> data)
        {
            JsonValidacion.REGISTRO = new List<PozoF20>();
            foreach (BodyForma20 item in data)
            {
                if (!string.IsNullOrEmpty(item.produccionPozo))
                {
                    PozoF20 pozo = new PozoF20()
                    {
                        POZO = item.produccionPozo
                    };
                    JsonValidacion.REGISTRO.Add(pozo);
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
                var permisos = await FormaOficial.GetPrecarga("Cargue Forma 20CR", fechaForma, operador, campo, contrato);
                if (permisos.Count > 0)
                {
                    foreach (Aprobacionprecarga precarga in permisos)
                    {
                        if (precarga.Activo == 1 && precarga.Usuario == getUserId)
                        {

                            if (precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                            {
                                Aprobacioncarga data = await FormaOficial.GetaFormAprobacion("Forma 20CR", precarga.FechaForma.Value, precarga.Operadora, precarga.Campo,
                                precarga.Contrato);
                                if (data != null)
                                {
                                    if (data.Formstate.Name != "Aprobada")
                                    {
                                        if (delete)
                                        {
                                            await RepositoryFormaProduction.DeleteAsync(predicate: x => x.Formid == data.IdForma);
                                            await RepositoryFormaDetail.DeleteAsync(predicate: x => x.Formid == data.IdForma);
                                            await RepositoryForma.DeleteAsync(predicate: x => x.Form20id == data.IdForma);
                                            await RepositoryConcreform.DeleteAsync(predicate: x => x.Formid == data.IdForma);
                                            await FormaOficial.Delete(data.IdForma.Value);
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
                Aprobacioncarga data = await FormaOficial.GetaFormAprobacion("Forma 20CR", fechaForma, operador, campo, contrato);
                
                var permisos = await FormaOficial.GetPrecarga("Cargue Forma 20CR", fechaForma, operador, campo, contrato);
                if (permisos != null && permisos?.Count > 0)
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
                                        await RepositoryFormaProduction.DeleteAsync(predicate: x => x.Formid == data.IdForma);
                                        await RepositoryFormaDetail.DeleteAsync(predicate: x => x.Formid == data.IdForma);
                                        await RepositoryForma.DeleteAsync(predicate: x => x.Form20id == data.IdForma);
                                        await RepositoryConcreform.DeleteAsync(predicate: x => x.Formid == data.IdForma);
                                        await FormaOficial.Delete(data.IdForma.Value);
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
                                await RepositoryFormaProduction.DeleteAsync(predicate: x => x.Formid == data.IdForma);
                                await RepositoryFormaDetail.DeleteAsync(predicate: x => x.Formid == data.IdForma);
                                await RepositoryForma.DeleteAsync(predicate: x => x.Form20id == data.IdForma);
                                await RepositoryConcreform.DeleteAsync(predicate: x => x.Formid == data.IdForma);
                                await FormaOficial.Delete(data.IdForma.Value);
                            }
                            state = true;
                        }
                        else
                        {
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


        private async Task ValidacionJson(string getUserId)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };

            try
            {
                List<CabeceraFormas<PozoF20>> listJson = new List<CabeceraFormas<PozoF20>>
                {
                    JsonValidacion
                };
                var json = listJson.Serialize(settings);

                var res = await FormasService.ValidarFormasMinAsync(new Ppdm.RequestBase { SJson = json });

                if (res.Code == 200)
                {
                    //validar codigo
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
                                        message = $"No se encontro el pozo {item.POZO}"
                                    };
                                    Errors.Add(error);
                                }

                                if (!string.IsNullOrEmpty(item.PDEN_ID) && item.PDEN_ID != "FALSE")
                                {
                                    Body.FirstOrDefault(x => x.inyeccionPozo == item.POZO).pdenId = item.PDEN_ID;
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
                                    RequestForma.permisoDeCargue = false;
                                    foreach (Aprobacionprecarga precarga in permisos)
                                    {
                                        if (precarga.Activo == 1 && precarga.Usuario == getUserId)
                                        {
                                            if (precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                                            {
                                               RequestForma.permisoDeCargue = true;
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
                                RequestForma.permisoDeCargue = false;
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
                                    message = $"No se encontro el pozo {registro.POZO}"
                                };
                                Errors.Add(error);
                            }
                            if (!string.IsNullOrEmpty(registro.PDEN_ID) && registro.PDEN_ID != "FALSE")
                            {
                                Body.FirstOrDefault(x => x.inyeccionPozo == registro.POZO).pdenId = registro.PDEN_ID;
                            }
                        }

                        if (dataService.root != null)
                        {
                            Element element = dataService.root.element;

                            if (element.MENSAJE == "Esta Forma ya fue cargada")
                            {
                                RequestForma.permisoDeCargue = false;
                            }
                            else
                            {

                                ErrorFormasStructura error = new ErrorFormasStructura()
                                {
                                    message = element.MENSAJE
                                };
                                Errors.Add(error);
                                RequestForma.permisoDeCargue = false;
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
                    RequestForma.permisoDeCargue = false;
                }
            }
            catch (Exception e)
            {
                TelemetryException.RegisterException(e);
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    message = "No se pudo procesar la validación de la forma que se esta cargando, verifique e intente nuevamente"
                };
                Errors.Add(error);
            }

        }

    }


}
