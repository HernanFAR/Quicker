using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quicker.Interfaces.Service
{
    public interface ICloseServiceAsync<TKey, TEntity> : IBaseService<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        Task<IEnumerable<TEntity>> Read();

        Task<TEntity> Read(TKey key);

        Task<IEnumerable<TEntity>> Paginate(int number, int page);
    }

    public interface ICloseServiceAsync<TKey, TEntity, TEntityDTO> : IBaseService<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDomainOf<TEntity>
    {
        Task<IEnumerable<TEntityDTO>> Read();

        Task<TEntityDTO> Read(TKey key);

        Task<IEnumerable<TEntityDTO>> Paginate(int number, int page);
    }
}
