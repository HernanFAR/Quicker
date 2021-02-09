using AutoMapper;
using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicker.Interfaces.Service
{
    public interface IBaseService<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        Task<IQueryable<TEntity>> QueryAll();

        Task<TEntity> QuerySingle(TKey key);
    }

    public interface IBaseService<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDomainOf<TEntity>
    {
        Task<IQueryable<TEntity>> QueryAll();

        Task<TEntity> QuerySingle(TKey key);

        TEntityDTO ToDTO(IMapper mapper, TEntity entity);

        TEntity ToDomain(IMapper mapper, TEntityDTO entity);
    }
}
