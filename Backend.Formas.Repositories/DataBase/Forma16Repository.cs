using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.Interface.Repository;
using System;
using System.Threading.Tasks;

namespace Backend.Formas.Repositories.DataBase
{
    public class Forma16Repository : IForma16Repository
    {
        private readonly IBaseRepository<Form16> _repositoryForm16;
        private readonly IBaseRepository<Form16Detail> _repositoryForm16Detail;
        private readonly IBaseRepository<Aprobacioncarga> _repositoryAprobacion;
        private readonly IBaseRepository<Concreteform> _repositoryConcreteForm;

        public Forma16Repository(IBaseRepository<Form16> repositoryForm16, IBaseRepository<Form16Detail> repositoryForm16Detail, IBaseRepository<Aprobacioncarga> repositoryAprobacion,
            IBaseRepository<Concreteform> repositoryConcreteForm)
        {
            _repositoryForm16 = repositoryForm16;
            _repositoryForm16Detail = repositoryForm16Detail;
            _repositoryAprobacion = repositoryAprobacion;
            _repositoryConcreteForm = repositoryConcreteForm;
        }

        public async Task<bool> Delete(Guid id)
        {
            await _repositoryAprobacion.DeleteAsync(predicate: x => x.IdForma == id);
            await _repositoryConcreteForm.DeleteAsync(predicate: x => x.Formid == id);
            await _repositoryForm16.DeleteAsync(predicate: x => x.Form16id == id);
            await _repositoryForm16Detail.DeleteAsync(predicate: x => x.Formid == id);
            return true;
        }

        public async Task<Form16> GetForm16(Guid id)
        {
            return await _repositoryForm16.GetAsync(predicate: x => x.Form16id == id);
        }

        public async Task<Form16Detail> GetForm16Detail(Guid id)
        {
            return await _repositoryForm16Detail.GetAsync(predicate: x => x.Formid == id);
        }

        public async Task<bool> SaveForm16(Form16 form16)
        {
            await _repositoryForm16.AddAsync(form16);
            return true;
        }

        public async Task<bool> SaveForm16Detail(Form16Detail form16)
        {
            await _repositoryForm16Detail.AddAsync(form16);
            return true;
        }
    }
}
