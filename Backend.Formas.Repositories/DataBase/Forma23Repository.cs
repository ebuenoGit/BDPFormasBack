using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.Interface.Repository;
using System;
using System.Threading.Tasks;

namespace Backend.Formas.Repositories.DataBase
{
    public class Forma23Repository : IForma23Repository
    {
        private readonly IBaseRepository<Form23> _repositoryForm23;
        private readonly IBaseRepository<Form23Detail> _repositoryForm23Detail;
        private readonly IBaseRepository<Aprobacioncarga> _repositoryAprobacion;
        private readonly IBaseRepository<Concreteform> _repositoryConcreteForm;

        public Forma23Repository(IBaseRepository<Form23> repositoryForm23, IBaseRepository<Form23Detail> repositoryForm23Detail, IBaseRepository<Aprobacioncarga> repositoryAprobacion,
            IBaseRepository<Concreteform> repositoryConcreteForm)
        {
            _repositoryForm23 = repositoryForm23;
            _repositoryForm23Detail = repositoryForm23Detail;
            _repositoryAprobacion = repositoryAprobacion;
            _repositoryConcreteForm = repositoryConcreteForm;
        }

        public async Task<bool> DeleteForm23(Guid id)
        {
            await _repositoryAprobacion.DeleteAsync(predicate: x => x.IdForma == id);
            await _repositoryConcreteForm.DeleteAsync(predicate: x => x.Formid == id);
            await _repositoryForm23.DeleteAsync(predicate: l => l.Form23id == id);
            await _repositoryForm23Detail.DeleteAsync(predicate: l => l.Formid == id);
            return true;
        }

        public async Task<Form23> GetForm23(Guid id)
        {
            return await _repositoryForm23.GetAsync(predicate: l => l.Form23id == id);
        }

        public async Task<Form23Detail> GetForm23Detail(Guid id)
        {
            return await _repositoryForm23Detail.GetAsync(predicate: l => l.Formid == id);
        }

        public async Task<bool> SaveForm23(Form23 form23)
        {
            await _repositoryForm23.AddAsync(form23);
            return true;
        }

        public async Task<bool> SaveForm23Detail(Form23Detail form23)
        {
            await _repositoryForm23Detail.AddAsync(form23);
            return true;
        }
    }
}
