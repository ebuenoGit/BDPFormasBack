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
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules
{
    public class Forma30SEEBusiness : IForma30SEEBusiness
    {
        private readonly IForma30SEERepository _repository;

        private readonly IFormaValidate _validate;

        private readonly Administracion.AdmGrpc.AdmGrpcClient AdministracionService;

        private readonly Backend.Formas.Utilities.SendMail.ISendMailService SendMailService;

        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient FormasService;

        private readonly Commons.CommonGrpc.CommonGrpcClient CommonService;

        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;

        private readonly IAdministracionFormas _repositoryAdministracion;

        private readonly RequestFormas<fileJSON> Response = new RequestFormas<fileJSON>();

        public Forma30SEEBusiness(IForma30SEERepository repository,
                                            Utilities.Telemetry.ITelemetryException telemetryException,
                                            IFormaValidate validate,
                                            Administracion.AdmGrpc.AdmGrpcClient _AdministracionService,
                                            Ppdm.PpdmGrpc.PpdmGrpcClient formasService,
                                            Backend.Formas.Utilities.SendMail.ISendMailService _SendMailService,
                                            Commons.CommonGrpc.CommonGrpcClient commonService,
                                            IAdministracionFormas repositoryAdministracion,
                                            IForma9Repository repositoryForma9)
        {
            _repository = repository;
            TelemetryException = telemetryException;
            _validate = validate;
            AdministracionService = _AdministracionService;
            SendMailService = _SendMailService;
            FormasService = formasService;
            CommonService = commonService;
            _repositoryAdministracion = repositoryAdministracion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="fileJSON"></param>
        /// <param name="forma"></param>
        /// <param name="Usuario"></param>
        /// <returns></returns>
        public async Task<ResponseBase<RequestFormas<fileJSON>>> InsertForma30(DateTime date, string fileJSON, string forma, string Usuario)
        {
            var json = fileJSON.Deserialize<Forma30>();
            var response = new ResponseBase<RequestFormas<fileJSON>>();
            List<ErrorFormasStructura> err = new List<ErrorFormasStructura>();
            List<Forma30ConsultaBDP> form30Bdp = new List<Forma30ConsultaBDP>();
            List<dynamic> list = new List<dynamic>();
            List<Forma30Data> salida = new List<Forma30Data>();
            var strUsuario = "";
            var strOperador = "";
            var strContrato = "";
            var strCampo = "";
            int banderaForma9 = 0;
            CultureInfo cultureInfo = new CultureInfo("es-co");
            string dateForma = "";


            try
            {

                var valCorreo = new ResponseBase<List<dynamic>>();

                DateTime fechaForma9 = date;

                
                 
                var formafecha = ValidaFechasPermisos(date, DateTime.Now, json.Archivo[0].info.operador, "", json.Archivo[0].info.contrato, strUsuario, forma);

                var valMesCarga = new ResponseBase<List<dynamic>>();

                var hojas = json.Archivo.Count;

                for (int h = 0; h < hojas; h++)
                {

                    var infoArchivo = json.Archivo[h].info;
                    dateForma = $"{infoArchivo.mes}/{infoArchivo.anio}";

                    if (string.IsNullOrEmpty(json.Archivo[h].info.operador))
                    {
                        err.Add(new ErrorFormasStructura() { sheet = h, column = "operador", row = 0, message = "El nombre del operador", value = json.Archivo[h].info.operador });
                    }
                    else
                    {
                        var valOperador = new ResponseBase<List<dynamic>>();
                        strOperador = json.Archivo[0].info.operador;
                        valOperador.Data = await _validate.ValidaOperador(json.Archivo[h].info.operador);

                        if (valOperador.Data?.Count == 0 || valOperador.Data == null)
                        {
                            err.Add(new ErrorFormasStructura() { sheet = h, column = "operador", row = 0, message = "El nombre del operador no existe.", value = json.Archivo[h].info.operador });
                        }
                    }

                    if (string.IsNullOrEmpty(json.Archivo[h].info.contrato))
                    {
                        err.Add(new ErrorFormasStructura() { sheet = h, column = "CONTRATO", row = 0, message = "campo vacio", value = json.Archivo[h].info.contrato });
                    }
                    else
                    {
                        var valContrato = new ResponseBase<List<dynamic>>();
                        strContrato = json.Archivo[h].info.contrato;
                        valContrato.Data = await _validate.ValidaContrato(json.Archivo[h].info.contrato);

                        if (valContrato.Data?.Count == 0 || valContrato.Data == null)
                        {
                            err.Add(new ErrorFormasStructura() { sheet = h, column = "CONTRATO", row = 0, message = "El nombre del contrato no existe.", value = json.Archivo[0].info.contrato });
                        }
                    }

                    
                   



                    int datos = json.Archivo[h].data.Count;

                    if (datos > 0)
                    {
                        for (int conDat = 0; conDat < datos; conDat++)
                        {
                            if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].campo))
                            {
                                err.Add(new ErrorFormasStructura() { sheet = h, column = "campo", row = conDat, message = "campo vacio", value = json.Archivo[h].data[conDat].campo });
                            }
                            else
                            {
                                var valCampo = new ResponseBase<List<dynamic>>();
                                strCampo = json.Archivo[h].data[conDat].campo;
                                valCampo.Data = await _validate.ValidaCampo(json.Archivo[h].data[conDat].campo);

                                var validaRegistrosArchivo = await _validate.ValidaCampoContratoOperador(json.Archivo[h].info.operador, json.Archivo[h].data[conDat].campo, json.Archivo[h].info.contrato);

                                if (validaRegistrosArchivo?.Count == 0 || validaRegistrosArchivo == null)
                                {
                                    err.Add(new ErrorFormasStructura() { sheet = h, column = "", row = conDat, message = "Validar nombre del contrato -" + json.Archivo[h].info.contrato + ", o nombre del campo " + json.Archivo[h].data[conDat].campo + ", u operador, ya que no se identifica la forma a cargar", value = "" });
                                }
                                else
                                {
                                    json.Archivo[h].data[conDat].campo_id = ConvertTypes.ConverDataDynamic(validaRegistrosArchivo, "CAMPO_ID");
                                    json.Archivo[h].info.operador_id = ConvertTypes.ConverDataDynamic(validaRegistrosArchivo, "OPERADOR_ID");
                                    json.Archivo[h].info.contrato_id = ConvertTypes.ConverDataDynamic(validaRegistrosArchivo, "CONTRATO_ID");
                                }

                                if (valCampo.Data?.Count == 0 || valCampo.Data == null)
                                {
                                    err.Add(new ErrorFormasStructura() { sheet = h, column = "campo", row = conDat, message = "El nombre del campo no existe", value = json.Archivo[h].data[conDat].campo });
                                }

                                if (!string.IsNullOrEmpty(json.Archivo[h].data[conDat].campo) && !string.IsNullOrEmpty(json.Archivo[h].info.operador))
                                {
                                    var info = json.Archivo[h].info;

                                    DateTime dateTime = DateTime.Parse(dateForma, cultureInfo);
                                    int day = dateTime.AddMonths(1).AddDays(-1).Day;
                                    DateTime volumDate = new DateTime(dateTime.Year, dateTime.Month, day);

                                    var res = await _validate.ValidaCompaniaCampoEstado(info.operador, json.Archivo[h].data[conDat].campo, volumDate.ToString("yyyyMMdd"));

                                    if (res?.Count == 0|| res == null)
                                    {
                                        err.Add(new ErrorFormasStructura() { sheet = h, column = "", row = 0, message = "Es posible que el campo contrato no este activo  o  la fecha de cargue no esta dentro del rango de la fecha efectiva o fecha expedicion ", value = json.Archivo[0].info.contrato });
                                    }
                                }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].gasFormacionKPC.ToString())) 
                                { 
                                    err.Add(new ErrorFormasStructura() { sheet = h, column = "gasFormacionKPC", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].gasFormacionKPC.ToString() }); 
                                }
                                else if (validarDecimal(json.Archivo[h].data[conDat].gasFormacionKPC.ToString()) == false) 
                                { err.Add(new ErrorFormasStructura() { sheet = h, column = "gasFormacionKPC", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].gasFormacionKPC.ToString() }); 
                                }
                                else if (json.Archivo[h].data[conDat].gasFormacionKPC < 0) 
                                { 
                                    err.Add(new ErrorFormasStructura() { sheet = h, column = "gasFormacionKPC", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].gasFormacionKPC.ToString() }); 
                                }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].contenidoPropano.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "contenidoPropano", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].contenidoPropano.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].contenidoPropano.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "contenidoPropano", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].contenidoPropano.ToString() }); }
                                else if (json.Archivo[h].data[conDat].contenidoPropano < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "contenidoPropano", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].contenidoPropano.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].contenidoButano.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "contenidoButano", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].contenidoButano.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].contenidoButano.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "contenidoButano", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].contenidoButano.ToString() }); }
                                else if (json.Archivo[h].data[conDat].contenidoButano < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "contenidoButano", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].contenidoButano.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].contenidoGasolinaNatural.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "contenidoGasolinaNatural", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].contenidoGasolinaNatural.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].contenidoGasolinaNatural.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "contenidoGasolinaNatural", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].contenidoGasolinaNatural.ToString() }); }
                                else if (json.Archivo[h].data[conDat].contenidoGasolinaNatural < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "contenidoGasolinaNatural", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].contenidoGasolinaNatural.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].gasFormacionProcesado.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "gasFormacionProcesado", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].gasFormacionProcesado.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].gasFormacionProcesado.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "gasFormacionProcesado", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].gasFormacionProcesado.ToString() }); }
                                else if (json.Archivo[h].data[conDat].gasFormacionProcesado < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "gasFormacionProcesado", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].gasFormacionProcesado.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].consumoEnCampo.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "consumoEnCampo", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].consumoEnCampo.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].consumoEnCampo.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "consumoEnCampo", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].consumoEnCampo.ToString() }); }
                                else if (json.Archivo[h].data[conDat].consumoEnCampo < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "consumoEnCampo", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].consumoEnCampo.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].generacionElectrica.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "generacionElectrica", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].generacionElectrica.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].generacionElectrica.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "generacionElectrica", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].generacionElectrica.ToString() }); }
                                else if (json.Archivo[h].data[conDat].generacionElectrica < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "generacionElectrica", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].generacionElectrica.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].otros.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "otros", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].otros.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].otros.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "otros", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].otros.ToString() }); }
                                else if (json.Archivo[h].data[conDat].otros < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "otros", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].otros.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].quemadoAire.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "quemadoAire", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].quemadoAire.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].quemadoAire.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "quemadoAire", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].quemadoAire.ToString() }); }
                                else if (json.Archivo[h].data[conDat].quemadoAire < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "quemadoAire", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].quemadoAire.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].usadoBombeoNeumatico.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "usadoBombeoNeumatico", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].usadoBombeoNeumatico.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].usadoBombeoNeumatico.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "usadoBombeoNeumatico", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].usadoBombeoNeumatico.ToString() }); }
                                else if (json.Archivo[h].data[conDat].usadoBombeoNeumatico < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "usadoBombeoNeumatico", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].usadoBombeoNeumatico.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].inyectadoYacimiento.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "inyectadoYacimiento", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].inyectadoYacimiento.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].inyectadoYacimiento.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "inyectadoYacimiento", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].inyectadoYacimiento.ToString() }); }
                                else if (json.Archivo[h].data[conDat].inyectadoYacimiento < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "inyectadoYacimiento", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].inyectadoYacimiento.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].productosObtenidosGasTotalGasProcesadoPlanta.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasTotalGasProcesadoPlanta", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].productosObtenidosGasTotalGasProcesadoPlanta.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].productosObtenidosGasTotalGasProcesadoPlanta.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasTotalGasProcesadoPlanta", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].productosObtenidosGasTotalGasProcesadoPlanta.ToString() }); }
                                else if (json.Archivo[h].data[conDat].productosObtenidosGasTotalGasProcesadoPlanta < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasTotalGasProcesadoPlanta", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].productosObtenidosGasTotalGasProcesadoPlanta.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].productosObtenidosGasPropano.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasPropano", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].productosObtenidosGasPropano.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].productosObtenidosGasPropano.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasPropano", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].productosObtenidosGasPropano.ToString() }); }
                                else if (json.Archivo[h].data[conDat].productosObtenidosGasPropano < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasPropano", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].productosObtenidosGasPropano.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].productosObtenidosGasButano.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasButano", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].productosObtenidosGasButano.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].productosObtenidosGasButano.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasButano", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].productosObtenidosGasButano.ToString() }); }
                                else if (json.Archivo[h].data[conDat].productosObtenidosGasButano < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasButano", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].productosObtenidosGasButano.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].productosObtenidosGasGasolina.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasGasolina", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].productosObtenidosGasGasolina.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].productosObtenidosGasGasolina.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasGasolina", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].productosObtenidosGasGasolina.ToString() }); }
                                else if (json.Archivo[h].data[conDat].productosObtenidosGasGasolina < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasGasolina", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].productosObtenidosGasGasolina.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos.ToString() }); }
                                else if (json.Archivo[h].data[conDat].productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].productosGasProcesadoConsumoEnCampo.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoConsumoEnCampo", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].productosGasProcesadoConsumoEnCampo.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].productosGasProcesadoConsumoEnCampo.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoConsumoEnCampo", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].productosGasProcesadoConsumoEnCampo.ToString() }); }
                                else if (json.Archivo[h].data[conDat].productosGasProcesadoConsumoEnCampo < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoConsumoEnCampo", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].productosGasProcesadoConsumoEnCampo.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].productosGasProcesadoGasoductosUrbanos.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoGasoductosUrbanos", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].productosGasProcesadoGasoductosUrbanos.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].productosGasProcesadoGasoductosUrbanos.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoGasoductosUrbanos", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].productosGasProcesadoGasoductosUrbanos.ToString() }); }
                                else if (json.Archivo[h].data[conDat].productosGasProcesadoGasoductosUrbanos < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoGasoductosUrbanos", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].productosGasProcesadoGasoductosUrbanos.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].productosGasProcesadoGeneracionElectrica.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoGeneracionElectrica", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].productosGasProcesadoGeneracionElectrica.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].productosGasProcesadoGeneracionElectrica.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoGeneracionElectrica", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].productosGasProcesadoGeneracionElectrica.ToString() }); }
                                else if (json.Archivo[h].data[conDat].productosGasProcesadoGeneracionElectrica < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoGeneracionElectrica", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].productosGasProcesadoGeneracionElectrica.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].productosGasProcesadoOtros.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoOtros", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].productosGasProcesadoOtros.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].productosGasProcesadoOtros.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoOtros", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].productosGasProcesadoOtros.ToString() }); }
                                else if (json.Archivo[h].data[conDat].productosGasProcesadoOtros < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoOtros", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].productosGasProcesadoOtros.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].productosGasProcesadoQuemadoAire.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoQuemadoAire", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].productosGasProcesadoQuemadoAire.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].productosGasProcesadoQuemadoAire.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoQuemadoAire", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].productosGasProcesadoQuemadoAire.ToString() }); }
                                else if (json.Archivo[h].data[conDat].productosGasProcesadoQuemadoAire < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoQuemadoAire", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].productosGasProcesadoQuemadoAire.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].productosGasProcesadoUsadoBombeoNeumatico.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoUsadoBombeoNeumatico", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].productosGasProcesadoUsadoBombeoNeumatico.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].productosGasProcesadoUsadoBombeoNeumatico.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoUsadoBombeoNeumatico", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].productosGasProcesadoUsadoBombeoNeumatico.ToString() }); }
                                else if (json.Archivo[h].data[conDat].productosGasProcesadoUsadoBombeoNeumatico < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoUsadoBombeoNeumatico", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].productosGasProcesadoUsadoBombeoNeumatico.ToString() }); }

                                if (string.IsNullOrEmpty(json.Archivo[h].data[conDat].productosGasProcesadoInyectadoYacimiento.ToString())) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoInyectadoYacimiento", row = conDat, message = "El campo no debe estar vacio.", value = json.Archivo[h].data[conDat].productosGasProcesadoInyectadoYacimiento.ToString() }); }
                                else if (validarDecimal(json.Archivo[h].data[conDat].productosGasProcesadoInyectadoYacimiento.ToString()) == false) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoInyectadoYacimiento", row = conDat, message = "El campo debe ser decimal.", value = json.Archivo[h].data[conDat].productosGasProcesadoInyectadoYacimiento.ToString() }); }
                                else if (json.Archivo[h].data[conDat].productosGasProcesadoInyectadoYacimiento < 0) { err.Add(new ErrorFormasStructura() { sheet = h, column = "productosGasProcesadoInyectadoYacimiento", row = conDat, message = "El campo no debe ser negativo.", value = json.Archivo[h].data[conDat].productosGasProcesadoInyectadoYacimiento.ToString() }); }

                             }


                            if (err.Count == 0)
                            {
                                var precarga = await _repository.ConsultarpreCargua(forma, date);
                                var permiso = 0;

                                if (precarga.Rows.Count > 0)
                                {
                                    permiso = 1;
                                }

                                var valForma = GenerarValidacionForma30Async(json, forma, date, h, permiso);

                                formaValidador30 fv30 = new formaValidador30();

                                var settings = new Newtonsoft.Json.JsonSerializerSettings
                                {
                                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
                                };

                                List<formaValidador30> listJson = new List<formaValidador30> { fv30 };
                                var jsonVal = valForma.Serialize(settings);

                                List<regVal30> reg = new List<regVal30>();
                                var listv = new List<RegisValidadorForma30>();

                                RegisValidadorForma30 f9vsf30 = new RegisValidadorForma30();

                                var res = await FormasService.ValidarFormasMinAsync(new Ppdm.RequestBase { SJson = jsonVal });

                                f9vsf30 = res.Data.Deserialize<RegisValidadorForma30>();

                                if (res.Code != 500)
                                {
                                    try
                                    {
                                        reg.Add(res.Data.Deserialize<regVal30>());

                                        var contador = reg.Count;

                                        for (int b = 0; b < contador; b++)
                                        {
                                            listv.Add(new RegisValidadorForma30()
                                            {
                                                CAMPO = reg[b].FORMAS.FORMA.REGISTRO.CAMPO,
                                                PDEN_ID = reg[b].FORMAS.FORMA.REGISTRO.PDEN_ID,
                                                VALIDACION_F9F30 = reg[b].FORMAS.FORMA.REGISTRO.VALIDACION_F9F30,
                                                GAS_F9 = reg[b].FORMAS.FORMA.REGISTRO.GAS_F9,
                                                GAS_F30 = reg[b].FORMAS.FORMA.REGISTRO.GAS_F30,
                                                MENSAJE = reg[b].FORMAS.FORMA.REGISTRO.MENSAJE

                                            });

                                            string gasFormacionF9 = reg[b].FORMAS.FORMA.REGISTRO.GAS_F9.Replace(",",".");
                                            int searchCharacter = json.Archivo[h].data[conDat].gasFormacionKPC.ToString().IndexOf(".");
                                            string gasFormacionKPC = json.Archivo[h].data[conDat].gasFormacionKPC.ToString().Substring(0, searchCharacter + 3);

                                            /* Esto aplica cuando tenemos forma 30 con diferentes formaciones ya que suma a nivel de campo 01/07/2021 */
                                            if (Convert.ToDecimal(gasFormacionKPC) > Convert.ToDecimal(gasFormacionF9))
                                            {
                                                err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = "Se encontraron diferencias para el campo: " + json.Archivo[h].data[conDat].campo + " en los valores de la producción de gas de la forma 9:  " + reg[b].FORMAS.FORMA.REGISTRO.GAS_F9 + " contra la producción de gas total " + forma + ": " + json.Archivo[h].data[conDat].gasFormacionKPC, value = "" });
                                            }

                                        }

                                        if (!string.IsNullOrEmpty(listv[0].PDEN_ID))
                                        {
                                            int position = listv[0].PDEN_ID.IndexOf("-");
                                            int totalreg = listv[0].PDEN_ID.Length;
                                            var pdenid = listv[0].PDEN_ID.Substring(0, position).Trim();
                                            var volumedate = listv[0].PDEN_ID.Substring(position + 1, 8).Trim();

                                            json.Archivo[h].info.pden_id = listv[0].PDEN_ID;
                                            json.Archivo[h].data[conDat].pden_id = pdenid;
                                            json.Archivo[h].data[conDat].volume_date = volumedate;
                                        }

                                    }
                                    catch (Exception)
                                    {
                                        err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = "La Forma 9 no ha sido Cargada para el periodo a consultar.", value = "" });
                                    }
                                }
                                else
                                {
                                    banderaForma9 = 1;

                                    listv.Add(new RegisValidadorForma30()
                                    {
                                        CAMPO = "",
                                        PDEN_ID = "No se identifico el pden_id",
                                        VALIDACION_F9F30 = "",
                                        GAS_F9 = ""
                                    });
                                }

                                json.Archivo[h].FORMAS = listv;
                            }
                        }
                    }
                    else
                    {
                        err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = "las filas del archivo estan vacias", value = "" });
                    }

                    if (string.IsNullOrEmpty(date.ToString()))
                    {
                        err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = "Fecha Vacia", value = date.ToString() });
                    }
                }

                if (banderaForma9 == 1)
                {
                    err.Add(new ErrorFormasStructura() { sheet = null, column = "", row = 0, message = "El mes de la forma 9 no se encuentra registrado.", value = "" });
                }

                Response.data = json.Archivo;
                Response.errors = err;
                Response.permisoDeCargue = formafecha.Result;

                response.Code = (int)HttpStatusCode.OK;
                response.Data = Response;

                return response;
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                response.Code = (int)HttpStatusCode.BadRequest;
                response.Message = ex.Message;
            }

            return response;
        }

        public DataTable guardarConcreteForm(Forma30 json, DateTime date, string forma, string usuario, int bandera)
        {
            try
            {
                var cab = new BDConcreteForm
                {
                    concreteformid = new Guid(),
                    maincampid = 0,
                    company = json.Archivo[bandera].info.operador,
                    contract = json.Archivo[bandera].info.contrato,
                    battery = "",
                    tank = "",
                    month = date.Month,
                    year = date.Year,
                    explotationmodality = "Cargue Archivo " + forma,
                    annotations = "",
                    version = 1,
                    currentstate = new Guid(),
                    generationflag = 0,
                    campid = 0,
                    pdenid = json.Archivo[bandera].info.pden_id,
                    formid = new Guid(),
                    generationjobid = 0,
                    iqistatus = 0,
                    usersigning = usuario,
                    minrepsigning = "",
                    formname = forma
                };

                DataTable guardarCabecera = _repository.GuardarCabecera(cab);

                return guardarCabecera;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GuardarForma30BD(Forma30 json, string formId, DateTime date, string forma, int bandera, string user)
        {
            try
            {
                DataTable resultado = new DataTable();
                var dat = new BDForma30();
                int contador = json.Archivo[bandera].data.Count;

                for (int i = 0; i < contador; i++)
                {
                    dat.form30id = Guid.Parse(formId);

                    dat.operador_id = json.Archivo[bandera].info.operador_id;
                    dat.operador = json.Archivo[bandera].info.operador;
                    dat.contrato_id = json.Archivo[bandera].info.contrato_id;
                    dat.contrato = json.Archivo[bandera].info.contrato;
                    dat.date = date;
                    dat.uid = "";
                    dat.lastModified = 0;
                    dat.lastModifiedDate = date;
                    dat.name = forma;
                    dat.size = 0;
                    dat.type = "";
                    dat.percent = 0;
                    dat.originFileObj_uid = "";
                    dat.usuario = user;

                    resultado = _repository.GuardarForma30BD(dat);
                }

                return resultado;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GuardarDetail(Forma30 json, string formId, int bandera, string user)
        {
            try
            {
                DataTable resultado = new DataTable();

                int data;

                data = json.Archivo[bandera].data.Count;

                for (int j = 0; j < data; j++)
                {
                    var dat = new BDForma30Detail
                    {
                        form30detail = new Guid(),
                        formid = Guid.Parse(formId),
                        campo_id = json.Archivo[bandera].data[j].campo_id,
                        campo = json.Archivo[bandera].data[j].campo,
                        gasFormacionKPC = json.Archivo[bandera].data[j].gasFormacionKPC,
                        contenidoPropano = json.Archivo[bandera].data[j].contenidoPropano,
                        contenidoButano = json.Archivo[bandera].data[j].contenidoButano,
                        contenidoGasolinaNatural = json.Archivo[bandera].data[j].contenidoGasolinaNatural,
                        gasFormacionProcesado = json.Archivo[bandera].data[j].gasFormacionProcesado,
                        consumoEnCampo = json.Archivo[bandera].data[j].consumoEnCampo,
                        generacionElectrica = json.Archivo[bandera].data[j].generacionElectrica,
                        otros = json.Archivo[bandera].data[j].otros,
                        quemadoAire = json.Archivo[bandera].data[j].quemadoAire,
                        usadoBombeoNeumatico = json.Archivo[bandera].data[j].usadoBombeoNeumatico,
                        inyectadoYacimiento = json.Archivo[bandera].data[j].inyectadoYacimiento,
                        productosObtenidosGasTotalGasProcesadoPlanta = json.Archivo[bandera].data[j].productosObtenidosGasTotalGasProcesadoPlanta,
                        productosObtenidosGasPropano = json.Archivo[bandera].data[j].productosObtenidosGasPropano,
                        productosObtenidosGasButano = json.Archivo[bandera].data[j].productosObtenidosGasButano,
                        productosObtenidosGasGasolina = json.Archivo[bandera].data[j].productosObtenidosGasGasolina,
                        productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos = json.Archivo[bandera].data[j].productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos,
                        productosGasProcesadoConsumoEnCampo = json.Archivo[bandera].data[j].productosGasProcesadoConsumoEnCampo,
                        productosGasProcesadoGasoductosUrbanos = json.Archivo[bandera].data[j].productosGasProcesadoGasoductosUrbanos,
                        productosGasProcesadoGeneracionElectrica = json.Archivo[bandera].data[j].productosGasProcesadoGeneracionElectrica,
                        productosGasProcesadoOtros = json.Archivo[bandera].data[j].productosGasProcesadoOtros,
                        productosGasProcesadoQuemadoAire = json.Archivo[bandera].data[j].productosGasProcesadoQuemadoAire,
                        productosGasProcesadoUsadoBombeoNeumatico = json.Archivo[bandera].data[j].productosGasProcesadoUsadoBombeoNeumatico,
                        productosGasProcesadoInyectadoYacimiento = json.Archivo[bandera].data[j].productosGasProcesadoInyectadoYacimiento,
                        productosGasProcesadoObservaciones = json.Archivo[bandera].data[j].productosGasProcesadoObservaciones,
                        REAL_TP = json.Archivo[bandera].data[j].REAL_TP,
                        REAL = json.Archivo[bandera].data[j].REAL,
                        REAL_BAS = json.Archivo[bandera].data[j].REAL_BAS,
                        REAL_INC = json.Archivo[bandera].data[j].REAL_INC,
                        GAS_PROCESADO_TP = json.Archivo[bandera].data[j].GAS_PROCESADO_TP,
                        GAS_QUEMADO_TP = json.Archivo[bandera].data[j].GAS_QUEMADO_TP,
                        GLP_TP = json.Archivo[bandera].data[j].GLP_TP,
                        GLP = json.Archivo[bandera].data[j].GLP,
                        PROPANO_TP = json.Archivo[bandera].data[j].PROPANO_TP,
                        BUTANO_TP = json.Archivo[bandera].data[j].BUTANO_TP,
                        GASOLINA_TP = json.Archivo[bandera].data[j].GASOLINA_TP,
                        APIASOL_TP = json.Archivo[bandera].data[j].APIASOL_TP,
                        APIASOL = json.Archivo[bandera].data[j].APIASOL,
                        CONDENSADO_TP = json.Archivo[bandera].data[j].CONDENSADO_TP,
                        CONDENSADO = json.Archivo[bandera].data[j].CONDENSADO,
                        CONSUMOS_TP = json.Archivo[bandera].data[j].CONSUMOS_TP,
                        GASODUCTOS_URBANOS_TP = json.Archivo[bandera].data[j].GASODUCTOS_URBANOS_TP,
                        GASODUCTOS_URBANOS = json.Archivo[bandera].data[j].GASODUCTOS_URBANOS,
                        GENERACION_ELECTRICA_TP = json.Archivo[bandera].data[j].GENERACION_ELECTRICA_TP,
                        OTRAS_VENTAS_TP = json.Archivo[bandera].data[j].OTRAS_VENTAS_TP,
                        GAS_TRANFERENCIA_TP = json.Archivo[bandera].data[j].GAS_TRANFERENCIA_TP,
                        GAS_TRANFERENCIA = json.Archivo[bandera].data[j].GAS_TRANFERENCIA,
                        GAS_NEUMATICO_TP = json.Archivo[bandera].data[j].GAS_NEUMATICO_TP,
                        GAS_INYECTADO_TP = json.Archivo[bandera].data[j].GAS_INYECTADO_TP,
                        TOTAL_PROC_PLANTA_TP = json.Archivo[bandera].data[j].TOTAL_PROC_PLANTA_TP,
                        GP_TRATOTAL_PROC_PLANTA_TP = json.Archivo[bandera].data[j].GP_TRATOTAL_PROC_PLANTA_TP,
                        GP_TRATOTAL_PROC_PLANTA = json.Archivo[bandera].data[j].GP_TRATOTAL_PROC_PLANTA,
                        GP_CONSUMOS_TP = json.Archivo[bandera].data[j].GP_CONSUMOS_TP,
                        GP_GASODUCTOS_URBANOS_TP = json.Archivo[bandera].data[j].GP_GASODUCTOS_URBANOS_TP,
                        GP_GENERACION_ELECTRICA_TP = json.Archivo[bandera].data[j].GP_GENERACION_ELECTRICA_TP,
                        GP_OTRAS_VENTAS_TP = json.Archivo[bandera].data[j].GP_OTRAS_VENTAS_TP,
                        GP_NEUMATICO_TP = json.Archivo[bandera].data[j].GP_NEUMATICO_TP,
                        GP_QUEMADO_TP = json.Archivo[bandera].data[j].GP_QUEMADO_TP,
                        GP_INYECTADO_TP = json.Archivo[bandera].data[j].GP_INYECTADO_TP,
                        REPRESENTA_OPER = json.Archivo[bandera].data[j].REPRESENTA_OPER,
                        REPRESENTA_ANH = json.Archivo[bandera].data[j].REPRESENTA_ANH,
                        IDFORMA = json.Archivo[bandera].data[j].IDFORMA,
                        pden_id = json.Archivo[bandera].data[j].pden_id,
                        volume_date = json.Archivo[bandera].data[j].volume_date,
                        usuario = user

                    };

                    resultado = _repository.GuardarForma30BD(dat);
                }




                return resultado;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ResponseBase<List<dynamic>>> ValidarJsonBDP(DateTime date, Stream fileStream, string FileName, string forma, string fileJSON, string GetUserId, string nombre, string email)
        {
            var json = fileJSON.Deserialize<Forma30>();
            List<dynamic> result = new List<dynamic>();
            DataTable cabecera = new DataTable();
            List<Errores> err = new List<Errores>();
            AprobacionCarga pr = new AprobacionCarga();

            try
            {
                var DataUser = await AdministracionService.UsuarioAprobadorAsync(new Administracion.RequestAprobaciones { NombreForma = forma });
                var dataService = DataUser.Data.Deserialize<UsuariosRecursosAprobaciones>();

                var strUsuarioNot = dataService.NombreUsuario;
                var usuarioIdAprNot = dataService.CodigoUsuario;
                var strCorreoNot = dataService.Correo;

                var strUsuario = nombre;
                var usuarioIdApr = GetUserId;
                var strCorreo = email;
                var hojas = json.Archivo.Count;
                string cadena = FileName;
                FileInfo fi = new FileInfo(cadena);

                if (hojas > 0)
                {
                    cabecera = guardarConcreteForm(json, date, forma, GetUserId, 0);

                    for (int h = 0; h < hojas; h++)
                    {
                        DataTable registros = new DataTable();
                        DataTable detail = new DataTable();

                        if (cabecera.Rows.Count > 0)
                        {
                            registros = GuardarForma30BD(json, cabecera.Rows[0]["formid"].ToString(), date, fi.Name, h, GetUserId);
                            detail = GuardarDetail(json, cabecera.Rows[0]["formid"].ToString(), h, GetUserId);
                        }

                    }

                    string rutaArch = forma + Convert.ToChar(92) + DateTime.Now.Year.ToString() + Convert.ToChar(92) + DateTime.Now.Month.ToString();

                    var resultC = cargarArchivoForma30Async(fi.Name, fileStream, forma, rutaArch);
                    var forStateid = await _repositoryAdministracion.GetState("En Proceso de Aprobación");

                    pr.ID_Forma = Guid.Parse(cabecera.Rows[0]["formid"].ToString());
                    pr.FechaForma = date;
                    pr.Usuario = GetUserId;
                    pr.UsuarioNombre = strUsuario;
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
                    pr.operador = json.Archivo[0].info.operador;
                    pr.contrato = json.Archivo[0].info.contrato;
                    pr.campo = "";

                    var guardarAprobacion = _repository.guardarAprobacion(pr);
                }

                if (err.Count > 0)
                {
                    result.Add(err);
                }
                else
                {
                    result.Add("Se insertaron los registros para su aprobación.");

                }

                var mensaje = "Se realizó el cargue de la forma relacionada a la " + forma + " en estado de En Proceso de Aprobación ";

                var correo = notificarAsync(dataService.NombreUsuario, dataService.Correo, json.Archivo[0].info.operador, json.Archivo[0].info.contrato, json.Archivo[0].data[0].campo, date, mensaje, forma);

                return new ResponseBase<List<dynamic>>(HttpStatusCode.OK, "Se insertaron los registros para su aprobación.", data: result);
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<dynamic>>(HttpStatusCode.BadRequest, ex.Message, data: result);
            }
        }

        public List<formaValidador30> GenerarValidacionForma30Async(Forma30 json, string forma, DateTime date, int bandera, int permiso)
        {
            var val = new List<formaValidador30>();

            formaValidador30 fv30 = new formaValidador30();
            List<dynamic> result = new List<dynamic>();

            try
            {
                int col = json.Archivo[bandera].data.Count;

                var list = new List<REGISTROVVALFORM30>();

                for (int b = 0; b < col; b++)
                {
                    list.Add(new REGISTROVVALFORM30() { CAMPO = json.Archivo[bandera].data[b].campo, GAS_TOTAL_FORMACION = json.Archivo[bandera].data[b].gasFormacionKPC.ToString() });
                }

                val.Add(new formaValidador30()
                {
                    FORMA_CODIGO = "30",
                    OPERADOR = json.Archivo[bandera].info.operador,
                    OPERADOR_ID = json.Archivo[bandera].info.operador_id.ToString(),
                    CONTRATO_ID = json.Archivo[bandera].info.contrato_id.ToString(),
                    CONTRATO = json.Archivo[bandera].info.contrato,
                    CAMPO_ID = "",
                    CAMPO = "",
                    ESTRUCTURA_ID = null,
                    BLOQUE_ID = "",
                    FORMACION_ID = "",
                    FORMACION_SET_ID = "",
                    FORMACION = "",
                    MIEMBRO_ID = null,
                    YACIMIENTO_ID = "",
                    ANIO = date.Year.ToString(),
                    MES = date.Month.ToString(),
                    RECARGA = permiso.ToString(),
                    REGISTRO = list
                });

                return val;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> cargarArchivoForma30Async(string fileName, Stream file, string forma, string rutaArchivoUrl)
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

        private async Task<bool> ValidaFechasPermisos(DateTime fechaForma, DateTime mesOperativo, string operador, string campo, string contrato, string getUserId, string forma)
        {
            DateTime mesActual = DateTime.Now;
            bool state = false;

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

            return state;
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
    }
}
