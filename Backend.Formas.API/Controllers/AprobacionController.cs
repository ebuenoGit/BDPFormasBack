using Backend.Formas.API.Controllers.Base;
using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Formas.API.Controllers
{
    /// <summary>
    /// Aprobacion
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class AprobacionController : Base.BaseController
    {
        /// <summary>
        /// The business
        /// </summary>
        private readonly IAdministracionFormasBusiness business;
        private readonly IOficializarFormas businessOficializar;

        private readonly Administracion.AdmGrpc.AdmGrpcClient IntegradorImplementClient;
        /// <summary>
        /// Initializes a new instance of the <see cref="AprobacionController"/> class.
        /// </summary>
        /// <param name="aprobacion">The aprobacion.</param>
        /// <param name="integradorImplementClient">The integrador implement client.</param>
        /// <param name="oficializar">The aprobacion.</param>
        public AprobacionController(IAdministracionFormasBusiness aprobacion,
            Administracion.AdmGrpc.AdmGrpcClient integradorImplementClient,
            IOficializarFormas oficializar
            )
        {
            business = aprobacion;
            IntegradorImplementClient = integradorImplementClient;
            businessOficializar = oficializar;
        }

        /// <summary>
        /// Aprobacion. Lista formas aprobación.
        /// </summary>
        /// <returns></returns>
        [HttpGet("listFormasAprobacion")]
        public async Task<ActionResult> GestListAprobacion()
        {
            var result = await business.GetAprobacionFormas(GetUserId);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Aprobacion. Lista Formas Solicitud Precarga.
        /// </summary>
        /// <returns></returns>
        [HttpGet("listFormasSolicitudPrecarga")]
        public async Task<ActionResult> GestListPrecarga()
        {
            var result = await business.GetPrecargasList();
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Precarga. Registrar solicitud de precarga
        /// </summary>
        /// <returns></returns>
        [HttpPost("crearPrecarga")]
        public async Task<ActionResult> SavePrecarga(List<Aprobacionprecarga> data)
        {

            var result = await business.RegistrarPrecargas(data, GetUserId, GetNames);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Precarga. Registrar solicitud de precarga
        /// </summary>
        /// <returns></returns>
        [HttpPost("updatePrecarga")]
        public async Task<ActionResult> updatePrecarga(Aprobacionprecarga data)
        {
            var result = await business.ActualizarPrecarga(data, GetUserId);
            return StatusCode(200, result);
        }

        /// <summary>
        /// Aprobacion. Lista Formas Solicitud Precarga.
        /// </summary>
        /// <returns></returns>
        [HttpPost("aprobarForma")]
        public async Task<ActionResult> GenerarAprobacion(List<Aprobacioncarga> data)
        {
            var result = await business.generarAprobacion(data, GetUserId);
            return StatusCode(result.Code, result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("OficializarFormas")]
        public async Task<ActionResult> OficializarFormas(OficializarModel data)
        {
            var result = await businessOficializar.GetOficializarFormas(data,GetUserId);
            return StatusCode(result.Code, result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaOperativa"></param>
        /// <param name="idForma"></param>
        /// <returns></returns>
        [HttpGet("ConsultaOficializarFormas")]
        public async Task<ActionResult> ConsultaOficializarFormas(System.DateTime fechaOperativa, string idForma)
        {
            //ConsultaContratoOperativos data =  new ConsultaContratoOperativos()
            //{
            //    FechaOperativa = fechaOperativa.ToDate(),

            //}

            var result = await businessOficializar.ConsultarOficializacion(fechaOperativa,idForma);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Tests this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Test")]
        [AllowAnonymous]
        public async Task<ActionResult> Test()
        {
            var response = await IntegradorImplementClient.UsuarioAprobadorAsync(new Administracion.RequestAprobaciones { NombreForma = "forma 9" });
            return StatusCode(response.Code, response);
        }
    }
}