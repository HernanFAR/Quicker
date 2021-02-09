using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quicker.Interfaces.Service
{
    public interface IOpenServiceAsync<TKey, TEntity> : ICloseServiceAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        Task<TEntity> Create(TEntity entity);

        Task Delete(TKey key);
    }

    public interface IOpenServiceAsync<TKey, TEntity, TEntityDTO> : ICloseServiceAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        Task<TEntityDTO> Create(TEntityDTO entity);

        Task Delete(TKey key);
    }
}
