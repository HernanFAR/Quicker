using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quicker.Interfaces.Model;
using Quicker.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicker.Abstracts.Service
{
    public abstract class BaseService<TKey, TEntity> : IBaseService<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        protected DbContext Context { get; set; }

        public BaseService(DbContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> QueryAll()
        {
            var entities = Context
                .Set<TEntity>()
                .AsNoTracking();

            return entities;
        }

        public async Task<TEntity> QuerySingle(TKey key)
        {
            var entity = await Context
                .Set<TEntity>()
                .FindAsync(key);

            return entity;
        }
    }
    
    public abstract class BaseService<TKey, TEntity,TEntityDTO> : IBaseService<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        protected DbContext Context { get; }
        
        protected IMapper Mapper { get;  }

        public BaseService(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public IQueryable<TEntity> QueryAll()
        {
            var entities = Context
                .Set<TEntity>()
                .AsNoTracking();

            return entities;
        }

        public async Task<TEntity> QuerySingle(TKey key)
        {
            var entity = await Context
                .Set<TEntity>()
                .FindAsync(key);

            return entity;
        }

        public TEntityDTO ToDTO(TEntity entity)
            => Mapper.Map<TEntity, TEntityDTO>(entity);

        public TEntity ToDomain(TEntityDTO entity)
            => Mapper.Map<TEntityDTO, TEntity>(entity);
    }
}
