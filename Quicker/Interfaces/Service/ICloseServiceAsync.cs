using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Quicker.Interfaces.Service
{
    public interface ICloseServiceAsync<TKey, TEntity> : IBaseService<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        Task<IEnumerable<TEntity>> Read();

        Task<TEntity> Read(TKey key);

        Task<IEnumerable<TEntity>> Paginate(int number, int page);

        Task<IEnumerable<TEntity>> FindManyByCondition(Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntity>> FindManyByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions);

        Task<TEntity> FindOneByCondition(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> FindOneByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions);

        Task<TEntity> FindFirstByCondition(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> FindFirstByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions);
    }

    public interface ICloseServiceAsync<TKey, TEntity, TEntityDTO> : IBaseService<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        Task<IEnumerable<TEntityDTO>> Read();

        Task<TEntityDTO> Read(TKey key);

        Task<IEnumerable<TEntityDTO>> Paginate(int number, int page);

        Task<IEnumerable<TEntityDTO>> FindManyByCondition(Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntityDTO>> FindManyByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions);

        Task<TEntityDTO> FindOneByCondition(Expression<Func<TEntity, bool>> expression);

        Task<TEntityDTO> FindOneByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions);

        Task<TEntityDTO> FindFirstByCondition(Expression<Func<TEntity, bool>> expression);

        Task<TEntityDTO> FindFirstByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions);
    }
}
