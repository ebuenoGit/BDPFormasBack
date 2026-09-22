using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.Interface.Repository;
using System;
using System.Threading.Tasks;

namespace Backend.Formas.Repositories.DataBase
{
    public class FormaC7Repository : IFormaC7Repository
    {
        private readonly IBaseRepository<FormC7> _repositoryFormC7;
        private readonly IBaseRepository<FormC7Detail> _repositoryFormC7Detail;
        private readonly IBaseRepository<FormC7Total> _repositoryFormC7Total;
        private readonly IBaseRepository<Aprobacioncarga> _repositoryAprobacion;
        private readonly IBaseRepository<Concreteform> _repositoryConcreteForm;
        public FormaC7Repository(IBaseRepository<FormC7> repositoryFormC7, IBaseRepository<FormC7Detail> repositoryFormC7Detail, IBaseRepository<FormC7Total> repositoryFormC7Total,
                    IBaseRepository<Aprobacioncarga> repositoryAprobacion, IBaseRepository<Concreteform> repositoryConcreteForm)
        {
            _repositoryFormC7 = repositoryFormC7;
            _repositoryFormC7Detail = repositoryFormC7Detail;
            _repositoryFormC7Total = repositoryFormC7Total;
            _repositoryAprobacion = repositoryAprobacion;
            _repositoryConcreteForm = repositoryConcreteForm;
        }

        public async Task<bool> Delete(Guid id)
        {
            await _repositoryAprobacion.DeleteAsync(predicate: x => x.IdForma == id);
            await _repositoryConcreteForm.DeleteAsync(predicate: x => x.Formid == id);
            await _repositoryFormC7.DeleteAsync(predicate: x => x.Formc7id == id);
            await _repositoryFormC7Detail.DeleteAsync(predicate: x => x.Formid == id);
            return true;

        }

        public async Task<FormC7> GetFormC7(Guid id)
        {
            return await _repositoryFormC7.GetAsync(predicate: x => x.Formc7id == id);
        }

        public async Task<FormC7Detail> GetFormC7Detail(Guid id)
        {
            return await _repositoryFormC7Detail.GetAsync(predicate: x => x.Formid == id);
        }

        public async Task<bool> SaveFormC7(FormC7 formC7)
        {
            await _repositoryFormC7.AddAsync(formC7);
            return true;
        }

        public async Task<bool> SaveFormC7Detail(FormC7Detail formC7dDetail)
        {
            await _repositoryFormC7Detail.AddAsync(formC7dDetail);
            return true;
        }
    }
}
