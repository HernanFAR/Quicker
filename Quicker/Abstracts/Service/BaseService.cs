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
    ///     Main implementation of <seealso cref="IBaseService{TKey, TEntity}"/>, this class 
    ///     serves the main functions to get single or many elements in database.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Typically, this class is not inherented, unless you want to create a full
    ///         implementation of CloseService and OpenService for some reason
    ///     </para>
    ///     <para>
    ///         The utility of this class is centralice all object-retriving code that get objects of DB in two functions.    
    ///     </para>
    /// </remarks>
    /// 

    public abstract class BaseService<TKey, TEntity> : IBaseService<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        protected DbContext Context { get; set; }

        /// <summary>
        ///     Gives a way to set the <paramref name="context"/> property from subclasses.
        /// </summary>
        /// <param name="context">
        ///     The <see cref="DbContext"/> to be consumed by the service
        /// </param>
        /// 
        public BaseService(DbContext context)
        {
            Context = context;
        }

        /// <summary>
        ///     Main implementation of <seealso cref="IBaseService{TKey, TEntity}.QueryAll"/>.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The utility of this is centralice <em>all</em> object-retriving code that get ALL objects in DB, in one 
        ///         function (Not counting specific cases), acting like a <em>generic</em>.
        ///     </para>
        /// </remarks>
        /// <returns>
        ///     A readonly (Not tracked by the <see cref="DbContext"/>) <see cref="IQueryable{TEntity}"/>.
        /// </returns>
        /// 
        public IQueryable<TEntity> QueryAll()
            => Context
                .Set<TEntity>()
                .AsNoTracking();

        /// <summary>
        ///     Main implementation of <seealso cref="IBaseService{TKey, TEntity}.QuerySingle(TKey)"/>.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The utility of this is centralice <em>all</em> object-retriving code that get ONE object 
        ///         in DB, in one function (Not counting specific cases), acting like a <em>generic</em>.
        ///     </para>
        /// </remarks>
        /// <returns>
        ///     Return a <see cref="TEntity"/> founded by <paramref name="key"/> tracked by the context or null.
        /// </returns>
        /// <param name="key">
        ///     The primary key to found the entity
        /// </param>
        /// 
        public async Task<TEntity> QuerySingle(TKey key)
            => await Context
                .Set<TEntity>()
                .FindAsync(key);

    }

    /// <summary>
    ///     Main implementation of <seealso cref="IBaseService{TKey, TEntity TEntityDTO}"/>, this class 
    ///     serves the main functions to get single or many elements in database.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Typically, this class is not inherented, unless you want to create a full
    ///         implementation of CloseService and OpenService for some reason
    ///     </para>
    ///     <para>
    ///         The utility of this class is centralice all object-retriving code that get objects of DB in two functions.    
    ///     </para>
    /// </remarks>
    /// 
    public abstract class BaseService<TKey, TEntity,TEntityDTO> : IBaseService<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        protected DbContext Context { get; }

        protected IMapper Mapper { get;  }

        /// <summary>
        ///     Gives a way to set the <paramref name="context"/> and <paramref name="mapper"/> properties from subclasses.
        /// </summary>
        /// <param name="context">
        ///     The <see cref="DbContext"/> to be consumed by the service
        /// </param>
        /// <param name="mapper">
        ///     The <see cref="IMapper"/> to be consumed by the service
        /// </param>
        /// 
        public BaseService(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        /// <summary>
        ///     Gives a way to set the <paramref name="context"/> property from subclasses.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This constructor put a null in the Mapper property, so, you only must use 
        ///         it if you will provide you own Mapper
        ///     </para>
        /// </remarks>
        /// <param name="context">
        ///     The <see cref="DbContext"/> to be consumed by the service
        /// </param>
        /// 
        public BaseService(DbContext context)
        {
            Context = context;
            Mapper = null;
        }

        /// <summary>
        ///     Main implementation of <seealso cref="IBaseService{TKey, TEntity, TEntityDTO}.QueryAll"/>.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The utility of this is centralice <em>all</em> object-retriving code that get ALL objects in DB, in one 
        ///         function (Not counting specific cases), acting like a <em>generic</em>.
        ///     </para>
        /// </remarks>
        /// <returns>
        ///     A readonly (Not tracked by the <see cref="DbContext"/>) <see cref="IQueryable{TEntity}"/>.
        /// </returns>
        /// 
        public IQueryable<TEntity> QueryAll()
            => Context
                .Set<TEntity>()
                .AsNoTracking();

        /// <summary>
        ///     Main implementation of <seealso cref="IBaseService{TKey, TEntity}.QuerySingle(TKey)"/>.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The utility of this is centralice <em>all</em> object-retriving code that get ONE object 
        ///         in DB, in one function (Not counting specific cases), acting like a <em>generic</em>.
        ///     </para>
        /// </remarks>
        /// <returns>
        ///     Return a <see cref="TEntity"/> founded by <paramref name="key"/> tracked by the context or null.
        /// </returns>
        /// <param name="key">
        ///     The primary key to found the entity
        /// </param>
        /// 
        public async Task<TEntity> QuerySingle(TKey key)
            => await Context
                .Set<TEntity>()
                .FindAsync(key);


        /// <summary>
        ///     Main implementation of <seealso cref="IBaseService{TKey, TEntity, TEntityDTO}.ToDTO(TEntity)"/>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This function uses AutoMapper to work, but you can override it to use your own. To do this, 
        ///         use the <see cref="BaseService{TKey, TEntity, TEntityDTO}.BaseService(DbContext)"/> constructor
        ///     </para>
        /// </remarks>
        /// <param name="entity">The Domain entity to be mapped to DTO</param>
        /// 
        public virtual TEntityDTO ToDTO(TEntity entity)
            => Mapper.Map<TEntity, TEntityDTO>(entity);


        /// <summary>
        ///     Main implementation of <seealso cref="IBaseService{TKey, TEntity, TEntityDTO}.ToDomain(TEntityDTO)"/>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This function uses AutoMapper to work, but you can override it to use your own. To do this, 
        ///         use the <see cref="BaseService{TKey, TEntity, TEntityDTO}.BaseService(DbContext)"/> constructor
        ///     </para>
        /// </remarks>
        /// <param name="entity">The DTO entity to be mapped to Domain</param>
        /// 
        public virtual TEntity ToDomain(TEntityDTO entity)
            => Mapper.Map<TEntityDTO, TEntity>(entity);
    }
}
