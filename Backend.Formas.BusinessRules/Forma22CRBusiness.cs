using Backend.Formas.BusinessRules.ViewSendMail;
using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTO;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.DTO.Validaciones;
using Backend.Formas.Entities.DTOI;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules
{
    public class Forma22CRBusiness : IForma22CRBusiness
    {

        private readonly IForma22CRRepository forma22CR;

        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient FormasService;
        private readonly RequestFormas<Forma22CrReques> Response = new RequestFormas<Forma22CrReques>();
        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;
        private readonly IForma30SEERepository repository;
        private readonly Administracion.AdmGrpc.AdmGrpcClient AdministracionService;
        private readonly Commons.CommonGrpc.CommonGrpcClient CommonService;
        private readonly IAdministracionFormas _repositoryAdministracion;
        private readonly Backend.Formas.Utilities.SendMail.ISendMailService SendMailService;

        private readonly List<ErrorFormasStructura> Errors = new List<ErrorFormasStructura>();
        private readonly IFormaValidate FormasValidate;
        private readonly CultureInfo cultureInfo = new CultureInfo("es-co");
        private readonly CabeceraFormas<F22Mes> JsonValidacion = new CabeceraFormas<F22Mes>();
        private readonly IBaseRepository<Forma22cr> RepositoryF22;
        private readonly IBaseRepository<Forma22crdetalle> RepositoryF22Detalid;
        private readonly IBaseRepository<Concreteform> RepositoryConcreteForm;




        public Forma22CRBusiness(IForma22CRRepository _forma22CRRepository
                                , IFormaValidate _validate
                                , Ppdm.PpdmGrpc.PpdmGrpcClient _FormasService
                                , Utilities.Telemetry.ITelemetryException _TelemetryException
                                , IForma30SEERepository _repository
                                , Administracion.AdmGrpc.AdmGrpcClient _AdministracionService
                                , Commons.CommonGrpc.CommonGrpcClient _CommonService
                                , IAdministracionFormas repositoryAdministracion
                                , Backend.Formas.Utilities.SendMail.ISendMailService _SendMailService,
            IBaseRepository<Forma22cr> repositoryF22,
             IBaseRepository<Concreteform> repositoryConcreteForm,
             IBaseRepository<Forma22crdetalle> repositoryF22Detalid)
        {
            forma22CR = _forma22CRRepository;
            FormasValidate = _validate;
            FormasService = _FormasService;
            TelemetryException = _TelemetryException;
            repository = _repository;
            AdministracionService = _AdministracionService;
            CommonService = _CommonService;
            _repositoryAdministracion = repositoryAdministracion;
            SendMailService = _SendMailService;
            RepositoryF22 = repositoryF22;
            RepositoryConcreteForm = repositoryConcreteForm;
            RepositoryF22Detalid = repositoryF22Detalid;
        }

        public async Task<ResponseBase<RequestFormas<Forma22CrReques>>> ValidateForma22CR(DateTime date, string forma, string fileJson, string usuario)
        {

            var response = new ResponseBase<RequestFormas<Forma22CrReques>>();
            JsonValidacion.RECARGAR = "0";
            Response.permisoDeCargue = true;
            try
            {
                var json = fileJson.Deserialize<List<Forma22CR>>();
                for (var i = 0; i <= json.Count - 1; i++)
                {
                    infoForm22CR info = json[i].info;
                    List<dataForm22CR> detail = json[i].data;
                    //validacion de cabecera 
                    var infoComplet = await ValidateInfoHeader(info, i, usuario, forma);
                    ValidateBody(detail);
                    var f22Pden = await ValidacionJson(usuario, forma);

                    if (f22Pden == null)
                    {
                        if (Response.permisoDeCargue)
                        {
                            ErrorFormasStructura error = new ErrorFormasStructura()
                            {
                                sheet = i,
                                message = "No se encontraron registros para el pozo formacion",
                                column = "",
                                row = 0,
                                value = ""
                            };
                            Errors.Add(error);
                        }


                        Forma22CrReques request = new Forma22CrReques()
                        {
                            info = infoComplet,
                            data = detail
                        };
                        Response.data.Add(request);


                    }

                    if (f22Pden != null)
                    {
                        detail.ForEach(action: a => a.pden_id = f22Pden.PDEN_ID);
                        Forma22CrReques request = new Forma22CrReques()
                        {
                            info = infoComplet,
                            data = detail
                        };
                        Response.data.Add(request);
                    }
                }

                if (Errors.Count > 0)
                {
                    Response.errors = Errors;
                }

                response.Data = Response;

            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                response.Code = 500;
                response.Message = "Error de servidor";

            }

            return response;
        }




        private async Task<infoForm22CR> ValidateInfoHeader(infoForm22CR data, int key, string getUserId, string forma)
        {
            JsonValidacion.FORMA_CODIGO = "22";

            if (string.IsNullOrEmpty(data.operador))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El Campo no existe en la Base de Datos",
                    column = "",
                    row = 0,
                    value = data.operador
                };
                Errors.Add(error);
            }

            if (!string.IsNullOrEmpty(data.operador))
            {
                var list = await FormasValidate.ValidaOperador(data.operador);
                if (list == null || list?.Count == 0)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = "La Compañia no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.operador
                    };
                    Errors.Add(error);
                }
                else
                {
                    string operador_id = ConvertTypes.ConverDataDynamic(list, "BUSINESS_ASSOCIATE");
                    JsonValidacion.OPERADOR_ID = operador_id;
                    JsonValidacion.OPERADOR = data.operador;
                    data.operador_id = operador_id;
                }
            }

            if (string.IsNullOrEmpty(data.campo))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El Campo no existe en la Base de Datos",
                    column = "",
                    row = 0,
                    value = data.campo
                };
                Errors.Add(error);
            }

            if (!string.IsNullOrEmpty(data.campo))
            {
                var list = await FormasValidate.ValidaCampo(data.campo);
                if (list == null || list?.Count == 0)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = "El Campo no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.campo
                    };
                    Errors.Add(error);
                }
                else
                {
                    string campo_id = ConvertTypes.ConverDataDynamic(list, "FIELD_ID");
                    JsonValidacion.CAMPO_ID = campo_id;
                    JsonValidacion.CAMPO = data.campo;
                    data.campo_id = campo_id;
                }
            }




            if (string.IsNullOrEmpty(data.yacimiento))
            {
                ErrorFormasStructura error = new ErrorFormasStructura()
                {
                    sheet = key,
                    message = "El Campo no existe en la Base de Datos",
                    column = "",
                    row = 0,
                    value = data.yacimiento
                };
                Errors.Add(error);
            }

            if (!string.IsNullOrEmpty(data.yacimiento))
            {
                var list = await FormasValidate.ValidaFormacion(data.yacimiento);

                if (list?.Count > 0)
                {
                    var STRAT_UNIT_ID = ConvertTypes.ConverDataDynamic(list, "STRAT_UNIT_ID");

                    JsonValidacion.YACIMIENTO_ID = STRAT_UNIT_ID;
                    JsonValidacion.YACIMIENTO = data.yacimiento;

                    data.yacimiento_id = STRAT_UNIT_ID;
                }

                if (list?.Count == 0 || list == null)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = "La Formación no existe en la Base de Datos",
                        column = "",
                        row = 0,
                        value = data.yacimiento
                    };
                    Errors.Add(error);
                }

            }


            if (!string.IsNullOrEmpty(data.anio))
            {
                DateTime dateTime = new DateTime(DateTime.Now.Year - 1, 12, 31);
                if (dateTime.Year == Convert.ToInt32(data.anio))
                {
                    JsonValidacion.ANIO = data.anio;
                    JsonValidacion.MES = dateTime.Month.ToString();
                    Response.permisoDeCargue = await ValidaFechasPermisos(dateTime, data.operador, data.campo, data.contrato, getUserId, false, forma);

                }
                else
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        sheet = key,
                        message = "El año de carga para esta forma debe ser " + (dateTime.Year - 1).ToString(),
                        column = "",
                        row = 0,
                        value = data.anio
                    };
                    Errors.Add(error);
                }
            }


            return data;

        }

        // cuerpo del documento obtener que los meses de la forma 
        private void ValidateBody(List<dataForm22CR> data)
        {
            JsonValidacion.REGISTRO = new List<F22Mes>();
            DateTime date = DateTime.Now;

            foreach (dataForm22CR item in data)
            {
                if (!string.IsNullOrEmpty(item.mes))
                {
                    string dateString = $"{item.mes}/{date.Year}";
                    DateTime dateTime = DateTime.Parse(dateString, cultureInfo);
                    string month = dateTime.Month.ToString();

                    F22Mes mes = new F22Mes()
                    {
                        MES = month
                    };
                    JsonValidacion.REGISTRO.Add(mes);
                }
            }
        }

        // consultar servicio grpc para validar informacion 
        private async Task<RegistrosF22> ValidacionJson(string getUserId, string formaname)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };

            RegistrosF22 registros = new RegistrosF22();

            try
            {
                List<CabeceraFormas<F22Mes>> listJson = new List<CabeceraFormas<F22Mes>>
                {
                    JsonValidacion
                };

                var json = listJson.Serialize(settings);

                var res = await FormasService.ValidarFormasMinAsync(new Ppdm.RequestBase { SJson = json });


                try
                {
                    var dataService = res.Data.Deserialize<FormasBase<RegistrosF22>>(settings);
                    registros = dataService.FORMAS?.FORMA?.REGISTRO;
                    var mensaje = dataService.root?.element?.MENSAJE ?? "";

                    if (mensaje == "Esta Forma ya fue cargada")
                    {
                        CultureInfo cultureInfo = new CultureInfo("es-co");
                        var dateTime = DateTime.Parse($"{JsonValidacion.ANIO}/{JsonValidacion.MES}", cultureInfo);
                        var mes = DateTime.Now;
                        DateTime mesActual = new DateTime(mes.Year, mes.Month - 1, 1);
                        var permisos = await _repositoryAdministracion.GetPrecarga(formaname, dateTime, JsonValidacion.OPERADOR, JsonValidacion.CAMPO, JsonValidacion.CONTRATO);
                        if (permisos.Count() > 0)
                        {
                            foreach (Aprobacionprecarga precarga in permisos)
                            {
                                if (precarga.Activo == 1 && precarga.Usuario == getUserId)
                                {
                                    if (precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                                    {
                                        Response.permisoDeCargue = true;
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

                if (res.Code != 200)
                {
                    ErrorFormasStructura error = new ErrorFormasStructura()
                    {
                        message = "No se pudo procesar la validación de la forma que se esta cargando, verifique e intente nuevamente"
                    };
                    Errors.Add(error);
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
            return registros;
        }

        public async Task<ResponseBase<List<dynamic>>> InsertForma22CR(Stream fileStream, DateTime date, string forma, string fileJSON, string GetUserId, string nombre, string email, string fileName)
        {
            List<dynamic> result = new List<dynamic>();
            List<ErrorFormasStructura> err = new List<ErrorFormasStructura>();
            AprobacionCarga pr = new AprobacionCarga();

            try
            {
                var json = fileJSON.Deserialize<List<Forma22CR>>();

                try
                {
                    var DataUser = await AdministracionService.UsuarioAprobadorAsync(new Administracion.RequestAprobaciones { NombreForma = forma });
                    var dataService = DataUser.Data.Deserialize<UsuariosRecursosAprobaciones>();

                    var hojas = json.Count;
                    // var concreteform = guardarConcreteForm(json, date, forma, GetUserId, 0, nombre);

                    Forma22cr f22 = new Forma22cr();
                    for (int i = 0; i < hojas; i++)
                    {

                        try
                        {
                            f22 = new Forma22cr()
                            {
                                FormId = Guid.NewGuid(),
                                CompaniaId = json[i].info.operador_id,
                                Compania = json[i].info.operador,
                                Formacion = json[i].info.formacion,
                                ContratoId = json[i].info.contrato_id ?? "",
                                Contrato = json[i].info.contrato,
                                Bloque = json[i].info.bloque,
                                CampoId = json[i].info.campo_id,
                                Campo = json[i].info.campo,
                                Yacimiento = json[i].info.yacimiento,
                                YacimientoId = json[i].info.yacimiento_id,
                                Estructura = json[i].info.estructura,
                                Mes = json[i].info.mes,
                                Anio = json[i].info.anio,
                                PdenId = json[i].data.FirstOrDefault().pden_id,
                                UsuarioCreacion = GetUserId,
                                row_created_by = GetUserId,
                                row_created_date = DateTime.Now,
                                row_changed_by = GetUserId,
                                row_changed_date = DateTime.Now
                            };

                            await RepositoryF22.AddAsync(f22);
                        }
                        catch (Exception e)
                        {

                            TelemetryException.RegisterException(e);
                        }

                        try
                        {
                            var detCount = json[i].data.Count;
                            List<Forma22crdetalle> listAdd = new List<Forma22crdetalle>();
                            for (int b = 0; b < detCount; b++)
                            {
                                Forma22crdetalle det = new Forma22crdetalle
                                {
                                    FormId = f22.FormId,
                                    IdCabecera = Guid.NewGuid(),
                                    Pozo = json[i].data[b].Pozo ?? "",
                                    Mes = json[i].data[b].mes,
                                    PetroleoProducidoMensual = json[i].data[b].petroleoProducidoMensual,
                                    PetroleoProducidoAcumulado = json[i].data[b].petroleoProducidoAcumulado,
                                    AguaInyectadoMensual = json[i].data[b].aguaInyectadoMensual,
                                    AguaInyectadoAcumulado = json[i].data[b].aguaInyectadoAcumulado,
                                    AguaProducidoMensual = json[i].data[b].aguaProducidoMensual,
                                    AguaProducidoAcumulado = json[i].data[b].aguaProducidoAcumulado,
                                    GasInyectadoMensual = json[i].data[b].gasInyectadoMensual,
                                    GasInyectadoAcumulado = json[i].data[b].gasInyectadoAcumulado,
                                    GasProducidoMensual = json[i].data[b].gasProducidoMensual,
                                    GasProducidoAcumulado = json[i].data[b].gasProducidoAcumulado,
                                    PresionFondo = json[i].data[b].presionFondo,
                                    PdenId = json[i].data[b].pden_id,
                                    UsuarioCreacion = GetUserId,
                                    FechaCreacion = DateTime.Now,
                                    row_created_by = GetUserId,
                                    row_created_date = DateTime.Now,
                                    row_changed_by = GetUserId,
                                    row_changed_date = DateTime.Now

                                };
                                listAdd.Add(det);

                                //var detGuardar = await forma22CR.guardarDetalle(f22.form_id.ToString(), f22.id_detalle.ToString(), det, GetUserId);
                            }
                            if (listAdd.Count > 0)
                            {
                                await RepositoryF22Detalid.AddAsync(listAdd);
                            }
                        }
                        catch (Exception ex)
                        {
                            TelemetryException.RegisterException(ex);
                        }



                    }

                    var forStateid = await _repositoryAdministracion.GetState("En Proceso de Aprobación");

                    var cab = new Concreteform
                    {
                        Concreteformid = Guid.NewGuid(),
                        Maincampid = 0,
                        Company = json[0].info.operador,
                        Contract = json[0].info.contrato,
                        Battery = "",
                        Tank = "",
                        Month = date.Month,
                        Year = date.Year,
                        Explotationmodality = null,
                        Annotations = "",
                        Version = 1,
                        Currentstate = forStateid,
                        Generationflag = 0,
                        Campid = Convert.ToDecimal(json[0].info.operador_id),
                        Pdenid = json[0].data.FirstOrDefault().pden_id,
                        Formid = f22.FormId,
                        Generationjobid = 0,
                        Iqistatus = 0,
                        Usersigning = GetUserId,
                        Minrepsigning = "",
                        Formname = forma
                    };
                    await RepositoryConcreteForm.AddAsync(cab);

                    string cadena = fileName;
                    FileInfo fi = new FileInfo(cadena);

                    string rutaArch = forma + Convert.ToChar(92) + DateTime.Now.Year.ToString() + Convert.ToChar(92) + DateTime.Now.Month.ToString();// + fi.Name;

                    var resultC = cargarArchivoForma(fi.Name, fileStream, forma, rutaArch);


                    pr.ID_Forma = f22.FormId;
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
                catch (Exception ex)
                {
                    err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = "Verifique que el usuario aprobador este creado." + " error: " + ex.Message, value = "" });
                    result.Add(err);
                }

                if (err.Count > 0)
                {
                    return new ResponseBase<List<dynamic>>(HttpStatusCode.BadRequest, "No es posible insertar los registros por error de comunicación entre las interfaces .", data: result);
                }
                else
                {
                    return new ResponseBase<List<dynamic>>(HttpStatusCode.OK, "Se insertaron los registros para su aprobación.", data: result);
                }

            }
            catch (Exception ex)
            {
                return new ResponseBase<List<dynamic>>(HttpStatusCode.BadRequest, ex.Message, data: result);
            }
        }

        public DataTable guardarConcreteForm(List<Forma22CR> json, DateTime date, string forma, string usuario, int bandera, string nombre)
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
                    pdenid = json[0].data.FirstOrDefault().pden_id,
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
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
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
        private async Task<bool> ValidaFechasPermisos(DateTime fechaForma, string operador, string campo, string contrato, string getUserId, bool delete, string forma)
        {

            DateTime mesActual = DateTime.Now;
            bool state = false;
            Aprobacioncarga data = await _repositoryAdministracion.GetFormAprobacionF22(forma, fechaForma, operador, campo, contrato);

            var permisos = await _repositoryAdministracion.GetPrecargaF22(forma, fechaForma, operador, campo, contrato);
            foreach (Aprobacionprecarga precarga in permisos)
            {
                if (precarga.Activo == 1 && precarga.Usuario == getUserId && precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                {
                    if (data?.Formstate?.Name != "Aprobado" && data?.Usuario == getUserId)
                    {

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
                //await Forma4Repository.Delete(data.IdForma.Value);
            }

            if(permisos.Count == 0)
                state = (data == null)? true: false;

            return state;
        }



    }
}