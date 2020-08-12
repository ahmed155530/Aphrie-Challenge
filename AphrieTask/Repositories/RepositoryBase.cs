using AphrieTask.Interfaces;
using AphrieTask.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AphrieTask.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        public RepositoryContext repositoryContext { get; set; }
        public RepositoryBase(RepositoryContext _repositoryContext)
        {
            repositoryContext = _repositoryContext;
        }
        public void Add(TEntity entity)
        {
            repositoryContext.Set<TEntity>().Add(entity);
        }
        public void Update(Guid id, TEntity entity)
        {
            repositoryContext.Set<TEntity>().Update(entity);
        }
        public void Delete(Guid id, TEntity entity)
        {
            repositoryContext.Set<TEntity>().Remove(entity);
        }
        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate)
        {
            return repositoryContext.Set<TEntity>().Where(predicate);
        }
        public IQueryable<TEntity> GetAll()
        {
            return repositoryContext.Set<TEntity>();
        }
        public TEntity FindOneByCondition(Expression<Func<TEntity, bool>> predicate)
        {
            return repositoryContext.Set<TEntity>().Where(predicate).FirstOrDefault();
        }
    }
}
