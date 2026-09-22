using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Form = Backend.Formas.Entities.DAO.Form;
using Form9 = Backend.Formas.Entities.DAO.Form9;

namespace Backend.Formas.Repositories.DataBase
{
    public class Forma9Repository : IForma9Repository
    {
        private readonly IBaseRepository<Concreteform> RepositoryConcreteForm;
        private readonly IBaseRepository<Form> RepositoryForm;

        private readonly IBaseRepository<Form9> RepositoryForm9;

        private readonly IBaseRepository<Form9detail> RepositoryForm9Detail;

        private readonly IBaseRepository<Form9totalvolumedetail> RepositoryForm9totalvolumedetail;
        private readonly IBaseRepository<Acumulados> RepositoryAcumulado;

        public Forma9Repository(IBaseRepository<Form> repositoryForm,
            IBaseRepository<Form9> repositoryForm9,
            IBaseRepository<Form9detail> repositoryForm9Detail,
            IBaseRepository<Form9totalvolumedetail> repositoryForm9totalvolumedetail,
            IBaseRepository<Concreteform> respositoryConcretForm,
            IBaseRepository<Acumulados> respositoryAcumulado
        )
        {
            RepositoryForm = repositoryForm;
            RepositoryForm9 = repositoryForm9;
            RepositoryForm9Detail = repositoryForm9Detail;
            RepositoryForm9totalvolumedetail = repositoryForm9totalvolumedetail;
            RepositoryConcreteForm = respositoryConcretForm;
            RepositoryAcumulado = respositoryAcumulado;
        }

        public async Task<bool> CreateForm(Form _form)
        {
            await RepositoryForm.AddAsync(_form);

            return true;
        }

        public async Task<Form9> CreateForm9(Form9 _form9)
        {
            await RepositoryForm9.AddAsync(_form9);
            return _form9;
        }

        public async Task<Concreteform> CreateConcreteform(Concreteform _concrete)
        {
            await RepositoryConcreteForm.AddAsync(_concrete);
            return _concrete;
        }

        public async Task<bool> CreateForm9Detail(List<Form9detail> _form9Detail)
        {
            await RepositoryForm9Detail.AddAsync(_form9Detail);
            return true;
        }

        public async Task<bool> CreateForm9TotalVolumenDetail(Form9totalvolumedetail _form9Total)
        {
            await RepositoryForm9totalvolumedetail.AddAsync(_form9Total);
            return true;
        }

        public async Task<Form9> GetForm(Guid? id)
        {
            return await RepositoryForm9.GetAsync(predicate: f => f.Form9id == id);
        }

        public async Task<List<Form9detail>> GetFormDetail(Guid? id)
        {
            var data = await RepositoryForm9Detail.GetAllAsync(predicate: f => f.Formid == id);
            return data.ToList();
        }

        public async Task<List<Form9totalvolumedetail>> GetFormTotal(Guid? id)
        {
            var data = await RepositoryForm9totalvolumedetail.GetAllAsync(predicate: x => x.Formid == id);
            return data.ToList();
        }

        public async Task<Concreteform> GetFormConcreted(Guid? id)
        {
            var data = await RepositoryConcreteForm.GetAsync(predicate: x => x.Formid == id.Value);
            return data;
        }

        public async Task<bool> CreateAcumulados(List<Acumulados> data)
        {
            try
            {

                await RepositoryAcumulado.AddAsync(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return true;
        }

        public async Task Delete(Guid id)
        {
            await RepositoryConcreteForm.DeleteAsync(predicate: x => x.Formid == id);
            await RepositoryForm9Detail.DeleteAsync(predicate: x => x.Formid == id);
            await RepositoryForm9totalvolumedetail.DeleteAsync(predicate: x => x.Formid == id);
            await RepositoryForm9.DeleteAsync(predicate: x => x.Form9id == id);
            //await RepositoryAcumulado.DeleteAsync(predicate: x => x.Id_Forma == id);
        }

    }
}