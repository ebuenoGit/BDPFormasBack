using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.Interface.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Formas.Repositories.DataBase
{
    public class AdministracionFormas : IAdministracionFormas
    {
        private readonly IBaseRepository<Aprobacioncarga> RepositoryAprobacion;
        private readonly IBaseRepository<Aprobacionprecarga> RepositoryPrecarga;
        private readonly IBaseRepository<Formstate> RepositoryState;
        private readonly IBaseRepository<Acumulados> RepositoryAcumulados;

        public AdministracionFormas(
            IBaseRepository<Formstate> repositoryState,
            IBaseRepository<Aprobacioncarga> repositoryAprobacion,
            IBaseRepository<Aprobacionprecarga> repositoryPrecarga,
            IBaseRepository<Acumulados> repositoryAcumulado
        )
        {
            RepositoryState = repositoryState;
            RepositoryAprobacion = repositoryAprobacion;
            RepositoryPrecarga = repositoryPrecarga;
            RepositoryAcumulados = repositoryAcumulado;
        }

        public async Task<IList<Aprobacioncarga>> GetAprobacion(string userId)
        {
            var list = await RepositoryAprobacion.GetAllAsync(
                //  predicate: x=>x.UsuarioAprobador == userId,
                include: i => i.Include(inc => inc.Formstate)
             );
            return list;
        }

        public async Task<List<Acumulados>> GetAcumulador(Guid id)
        {
            var list = await RepositoryAcumulados.GetAllAsync(predicate: x => x.Id_Forma == id);
            return list.ToList();

        }

        public async Task<Aprobacioncarga> CreteAprobacion(Aprobacioncarga _data)
        {
            await RepositoryAprobacion.AddAsync(_data);
            return _data;
        }

        public async Task<Guid> GetState(string name)
        {
            var _formStateId = await RepositoryState.GetAsync(x => x.Name == name);
            return _formStateId.Formstateid;
        }

        public async Task<List<Formstate>> GetListState()
        {
            var list = await RepositoryState.GetAllAsync();
            return list.ToList();
        }

        public async Task<Aprobacioncarga> GetAprobacion(decimal id)
        {
            var _find = await RepositoryAprobacion.GetAsync(x => x.Id == id);
            return _find;
        }

        public async Task<Aprobacioncarga> FormaAprobada(decimal id)
        {
            Formstate state = await RepositoryState.GetAsync(x => x.Name == "Aprobada");
            Aprobacioncarga item = await RepositoryAprobacion.GetAsync(x => x.Id == id);
            item.Estado = state.Formstateid;
            await RepositoryAprobacion.UpdateAsync(item);

            return item;
        }

        public async Task<List<Aprobacionprecarga>> GetPrecarga(string forma, DateTime fechaforma, string operador,
            string campo, string contrato)
        {
                var datos = await RepositoryPrecarga.GetAllAsync(
                predicate: x => x.FechaForma == fechaforma && x.Activo == 1 && x.Operadora == operador
                                && x.Campo == campo && x.Contrato == contrato && x.FormaName == forma);
               
            return datos.ToList();

        }

        public async Task<List<Aprobacionprecarga>> GetPrecarga(string forma, DateTime fechaforma, string operador, string campo, string contrato, string user)
        {
            var datos = await RepositoryPrecarga.GetAllAsync(
            predicate: x => x.FechaForma == fechaforma && x.Activo == 1 && x.Operadora == operador
                            && x.Campo == campo && x.Contrato == contrato && x.FormaName == forma && x.Usuario == user);

            return datos.ToList();

        }

        public async Task<List<Aprobacionprecarga>> GetPrecargaF22(string forma, DateTime fechaforma, string operador, string campo, string contrato)
        {
            var datos = await RepositoryPrecarga.GetAllAsync(
            predicate: x => x.FechaForma.Value.Year == fechaforma.Year && x.Activo == 1 && x.Operadora == operador
                            && x.Campo == campo && x.Contrato == contrato && x.FormaName == forma);

            return datos.ToList();

        }

        public async Task<List<Aprobacionprecarga>> GetPrecarga(string forma, DateTime fechaforma, string operador,
            string campo)
        {
            var datos = await RepositoryPrecarga.GetAllAsync(
                predicate: x => x.FechaForma == fechaforma && x.Activo == 1 && x.Operadora == operador
                                && x.Campo == campo && x.FormaName == forma);
            return datos.ToList();
        }

        public async Task<Aprobacioncarga> GetaFormAprobacion(string forma, DateTime fechaforma, string operador,
            string campo,
            string contrato)
        {
            Aprobacioncarga datos = await RepositoryAprobacion.GetAsync(
                    predicate: x => x.FormaName == forma && x.FechaForma == fechaforma && x.Operadora == operador
                     && x.Campo == campo && x.Contrato == contrato,
                    include: i => i.Include(inc => inc.Formstate)
                );

            return datos;
        }

        public async Task<Aprobacioncarga> GetaFormAprobacion(string forma, DateTime fechaforma, string operador, string campo, string contrato, string user)
        {
            Aprobacioncarga datos = await RepositoryAprobacion.GetAsync(
                    predicate: x => x.FormaName == forma && x.FechaForma == fechaforma && x.Operadora == operador
                     && x.Campo == campo && x.Contrato == contrato && x.Usuario == user,
                    include: i => i.Include(inc => inc.Formstate));

            return datos;
        }

        public async Task<Aprobacioncarga> GetFormAprobacionF22(string forma, DateTime fechaforma, string operador, string campo, string contrato)
        {
            Aprobacioncarga datos = await RepositoryAprobacion.GetAsync(
                    predicate: x => x.FormaName == forma && x.FechaForma.Year == fechaforma.Year && x.Operadora == operador
                     && x.Campo == campo && x.Contrato == contrato,
                    include: i => i.Include(inc => inc.Formstate)
                );

            return datos;
        }

        public async Task<Aprobacioncarga> GetaFormAprobacion(string forma, DateTime fechaforma, string operador, string campo)
        {
            Aprobacioncarga datos = await RepositoryAprobacion.GetAsync(
                    predicate: x => x.FormaName == forma && x.FechaForma == fechaforma && x.Operadora == operador
                     && x.Campo == campo,
                    include: i => i.Include(inc => inc.Formstate)
                );

            return datos;
        }

        public async Task<bool> Delete(Guid id)
        {
            await RepositoryAprobacion.DeleteAsync(predicate: x => x.IdForma == id);
            return true;
        }

        public async Task<bool> CreatePrecarga(List<Aprobacionprecarga> data)
        {
            await RepositoryPrecarga.AddAsync(data);
            return true;
        }
        public async Task<bool> UpdatePrecarga(Aprobacionprecarga data)
        {
            await RepositoryPrecarga.UpdateAsync(data);
            return true;
        }

        public async Task<List<Aprobacionprecarga>> PrecargaList()
        {
            var data = await RepositoryPrecarga.GetAllAsync();
            return data.ToList();
        }


    }
}