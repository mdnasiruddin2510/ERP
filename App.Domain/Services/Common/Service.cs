using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Interface.Repository.Common;
using App.Domain.Interface.Service.Common;

namespace App.Domain.Services.Common
{
    public class Service<TEntity> : IService<TEntity>
         where TEntity : class
    {
        #region Constructor

        private readonly IRepository<TEntity> _repository;
       // private readonly IReadOnlyRepository<TEntity> _readOnlyRepository;
        //private readonly ValidationResult _validationResult;

        //public Service(
        //    IRepository<TEntity> repository,IReadOnlyRepository<TEntity> readOnlyRepository)
        //{
        //    _repository = repository;
        //    _readOnlyRepository = readOnlyRepository;
        //    //_validationResult = new ValidationResult();
        //}
        public Service(
           IRepository<TEntity> repository)
        {
            _repository = repository;
           // _readOnlyRepository = readOnlyRepository;
            //_validationResult = new ValidationResult();
        }
        #endregion

        #region Properties

        protected IRepository<TEntity> Repository
        {
            get { return _repository; }
        }

        //protected IReadOnlyRepository<TEntity> ReadOnlyRepository
        //{
        //    get { return _readOnlyRepository; }
        //}

        //protected ValidationResult ValidationResult
        //{
        //    get { return _validationResult; }
        //}

        #endregion

        #region Read Methods

        public virtual TEntity Get(int id, bool @readonly = false)
        {
            return _repository.Get(id);
        }

        public virtual IEnumerable<TEntity> All(bool @readonly = false)
        {
            return _repository.All(@readonly);
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool @readonly = false)
        {
            return _repository.Find(predicate, @readonly);
        }

        public IEnumerable<TEntity> SqlQueary(string sql, params object[] parameters)
        {
            return _repository.SqlQueary(sql, parameters);
        }

        public void Update(TEntity obj)
        {
            _repository.Update(obj);
        }

        public void Delete(TEntity obj)
        {
           _repository.Delete(obj);
        }

        public void Save()
        {
           _repository.Save();
        }

        #endregion

        #region CRUD Methods

        //public virtual ValidationResult Add(TEntity entity)
        //{
        //    if (!ValidationResult.IsValid)
        //        return ValidationResult;

        //    var selfValidationEntity = entity as ISelfValidation;
        //    if (selfValidationEntity != null && !selfValidationEntity.IsValid)
        //        return selfValidationEntity.ValidationResult;


        //    _repository.Add(entity);
        //    return _validationResult;
        //}

        //public virtual ValidationResult Update(TEntity entity)
        //{
        //    if (!ValidationResult.IsValid)
        //        return ValidationResult;

        //    var selfValidationEntity = entity as ISelfValidation;
        //    if (selfValidationEntity != null && !selfValidationEntity.IsValid)
        //        return selfValidationEntity.ValidationResult;

        //    _repository.Update(entity);
        //    return _validationResult;
        //}

        //public virtual ValidationResult Delete(TEntity entity)
        //{
        //    if (!ValidationResult.IsValid)
        //        return ValidationResult;

        //    _repository.Delete(entity);
        //    return _validationResult;
        //}

        #endregion


        public void Add(TEntity obj)
        {
            _repository.Add(obj);
        }


        public void Setvalues(TEntity entity, TEntity existingEntity)
        {
            _repository.Setvalues(entity, existingEntity);
        }
    }
}
