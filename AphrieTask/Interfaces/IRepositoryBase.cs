using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AphrieTask.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate);
        TEntity FindOneByCondition(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void Update(Guid id, TEntity entity);
        void Delete(Guid id, TEntity entity);
    }
}
