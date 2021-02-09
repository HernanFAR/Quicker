using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quicker.Interfaces.Service
{
    public interface IFullService<TKey, TEntity> : IOpenServiceAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        Task<TEntity> Update(TKey key, TEntity entity);
    }

    public interface IFullService<TKey, TEntity, TEntityDTO> : IOpenServiceAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDomainOf<TEntity>
    {
        Task<TEntityDTO> Update(TKey key, TEntityDTO entity);
    }
}
