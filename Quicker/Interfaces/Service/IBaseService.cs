using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicker.Interfaces.Service
{
    public interface IBaseService<TKey, TEntity>
    {
        Task<IQueryable<TEntity>> QueryAll();

        Task<TEntity> QuerySingle(TKey key);
    }

    public interface IBaseService<TKey, TEntity, TEntityDTO>
    {
        Task<IQueryable<TEntity>> QueryAll();

        Task<TEntity> QuerySingle(TKey key);

        TEntityDTO ToDTO(IMapper mapper, TEntity entity);

        TEntity ToDomain(IMapper mapper, TEntityDTO entity);
    }
}
