using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quicker.Interfaces.Component
{
    public interface IReadComponent<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        Task<IEnumerable<TEntity>> ReadAsync(Func<Task<bool>> action = null);

        Task<IEnumerable<TEntity>> PaginateAsync(int page, int number = 10, Func<Task<bool>> action = null);

        Task<TEntity> ReadAsync(TKey key, Func<Task<bool>> action = null);

        Task<bool> CheckExistenceAsync(TKey key, Func<Task<bool>> action = null);

        Task<bool> CheckExistenceAsync(Func<Task<bool>> action = null, params Expression<Func<TEntity, bool>>[] conditions);
    }

    public interface IReadComponent<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        Task<IEnumerable<TEntityDTO>> ReadAsync(Func<Task<bool>> action = null);

        Task<IEnumerable<TEntityDTO>> PaginateAsync(int page, int number = 10, Func<Task<bool>> action = null);

        Task<TEntityDTO> ReadAsync(TKey key, Func<Task<bool>> action = null);

        Task<bool> CheckExistenceAsync(TKey key, Func<Task<bool>> action = null);

        Task<bool> CheckExistenceAsync(Func<Task<bool>> action = null, params Expression<Func<TEntity, bool>>[] conditions);
    }
}
