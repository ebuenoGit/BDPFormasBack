using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.Interface.Repository;
using System;
using System.Threading.Tasks;

namespace Backend.Formas.Repositories.DataBase
{
    public class Formas21Repository : IForma21Repository
    {
        private readonly IBaseRepository<Form21> _repositoryForm21;
        private readonly IBaseRepository<Form21InyectionDetail> _repositoryForm21InyectionDetail;
        private readonly IBaseRepository<Form21ProductionDetail> _repositoryForm21ProductionDetail;
        private readonly IBaseRepository<Form21TotalVolumenDetail> _repositoryForm21TotalVolumenDetail;
        private readonly IBaseRepository<Aprobacioncarga> _repositoryAprobacion;
        private readonly IBaseRepository<Concreteform> _repositoryConcreteForm;

        public Formas21Repository(IBaseRepository<Form21> form21, IBaseRepository<Form21InyectionDetail> form21InyectionDetail,
                                  IBaseRepository<Form21ProductionDetail> form21ProductionDetail, IBaseRepository<Form21TotalVolumenDetail> form21TotalVolumenDetail,
                                  IBaseRepository<Aprobacioncarga> repositoryAprobacion, IBaseRepository<Concreteform> repositoryConcreteForm)
        {
            _repositoryForm21 = form21;
            _repositoryForm21InyectionDetail = form21InyectionDetail;
            _repositoryForm21ProductionDetail = form21ProductionDetail;
            _repositoryForm21TotalVolumenDetail = form21TotalVolumenDetail;
            _repositoryAprobacion = repositoryAprobacion;
            _repositoryConcreteForm = repositoryConcreteForm;
        }

        public async Task<bool> Delete(Guid id)
        {
            await _repositoryAprobacion.DeleteAsync(predicate: x => x.IdForma == id);
            await _repositoryConcreteForm.DeleteAsync(predicate: x => x.Formid == id);
            await _repositoryForm21.DeleteAsync(predicate: x => x.Form21id == id);
            await _repositoryForm21InyectionDetail.DeleteAsync(predicate: x => x.Formid == id);
            await _repositoryForm21ProductionDetail.DeleteAsync(predicate: x => x.Formid == id);
            return true;
        }

        public async Task<Form21> GetForm21(Guid id)
        {
            return await _repositoryForm21.GetAsync(predicate: x => x.Form21id == id);
        }

        public async Task<Form21InyectionDetail> GetForm21InyectionDetail(Guid id)
        {
            return await _repositoryForm21InyectionDetail.GetAsync(predicate: x => x.Formid == id);
        }

        public async Task<Form21ProductionDetail> GetForm21ProductionDetail(Guid id)
        {
            return await _repositoryForm21ProductionDetail.GetAsync(predicate: x => x.Formid == id);
        }

        public async Task<Form21TotalVolumenDetail> GetForm21TotalVolumenDetail(Guid id)
        {
            return await _repositoryForm21TotalVolumenDetail.GetAsync(predicate: x => x.Idform21tvdetailid == id);
        }

        public async Task<bool> SaveForm21InyectionDetail(Form21InyectionDetail form21InyectionDetail)
        {
            await _repositoryForm21InyectionDetail.AddAsync(form21InyectionDetail);
            return true;
        }

        public async Task<bool> SaveForm21ProductionDetail(Form21ProductionDetail form21ProductionDetail)
        {
            await _repositoryForm21ProductionDetail.AddAsync(form21ProductionDetail);
            return true;
        }

        public async Task<bool> SaveForm21TotalVolumenDetail(Form21TotalVolumenDetail form21TotalVolumenDetail)
        {
            await _repositoryForm21TotalVolumenDetail.AddAsync(form21TotalVolumenDetail);
            return true;
        }

        public async Task<Form21> SaveForma21(Form21 data)
        {
            await _repositoryForm21.AddAsync(data);
            return data;
        }
    }
}
