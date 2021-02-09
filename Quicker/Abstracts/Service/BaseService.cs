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
    /// <summary>
    /// <para>Main implementation of <seealso cref="IBaseService{TKey, TEntity}"/>.</para>
    /// </summary>
    public abstract class BaseService<TKey, TEntity> : IBaseService<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        /// <summary>
        /// <para>Database context of the service.</para>
        /// </summary>
        protected DbContext Context { get; set; }

        public BaseService(DbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="IBaseService{TKey, TEntity}.QueryAll"/>.</para>
        /// <para>Return a <em>"AsNoTracking"</em> list, to avoid problems with EF Core</para>
        /// </summary>
        public IQueryable<TEntity> QueryAll()
            => Context
                .Set<TEntity>()
                .AsNoTracking();

        /// <summary>
        /// <para>Main implementation of <seealso cref="IBaseService{TKey, TEntity}.QuerySingle(TKey)"/>.</para>
        /// <para>Return a entity tracked by the context to fulfill with related entities.</para>
        /// </summary>
        public async Task<TEntity> QuerySingle(TKey key)
            => await Context
                .Set<TEntity>()
                .FindAsync(key);

    }

    /// <summary>
    /// <para>Main implementation of <seealso cref="IBaseService{TKey, TEntity}"/>.</para>
    /// </summary>
    public abstract class BaseService<TKey, TEntity,TEntityDTO> : IBaseService<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        /// <summary>
        /// <para>Database context of the service.</para>
        /// </summary>
        protected DbContext Context { get; }

        /// <summary>
        /// <para>Mapper property to map the DTO to Domain, a viceversa.</para>
        /// </summary>
        protected IMapper Mapper { get;  }

        public BaseService(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }


        /// <summary>
        /// <para>Main implementation of <seealso cref="IBaseService{TKey, TEntity, TEntityDTO}.QueryAll"/>.</para>
        /// <para>Return a <em>"AsNoTracking"</em> list, to avoid problems with EF Core</para>
        /// </summary>
        public IQueryable<TEntity> QueryAll()
            => Context
                .Set<TEntity>()
                .AsNoTracking();

        /// <summary>
        /// <para>Main implementation of <seealso cref="IBaseService{TKey, TEntity, TEntityDTO}.QuerySingle(TKey)"/>.</para>
        /// <para>Return a entity tracked by the context to fulfill with related entities.</para>
        /// </summary>
        public async Task<TEntity> QuerySingle(TKey key)
            => await Context
                .Set<TEntity>()
                .FindAsync(key);


        /// <summary>
        /// <para>Main implementation of <seealso cref="IBaseService{TKey, TEntity, TEntityDTO}.ToDTO(TEntity)"/>.</para>
        /// <para>This function uses AutoMapper to work.</para>
        /// </summary>
        public TEntityDTO ToDTO(TEntity entity)
            => Mapper.Map<TEntity, TEntityDTO>(entity);

        /// <summary>
        /// <para>Main implementation of <seealso cref="IBaseService{TKey, TEntity, TEntityDTO}.ToDomain(TEntityDTO)"/>.</para>
        /// <para>This function uses AutoMapper to work.</para>
        /// </summary>
        public TEntity ToDomain(TEntityDTO entity)
            => Mapper.Map<TEntityDTO, TEntity>(entity);
    }
}
