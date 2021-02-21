using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quicker.Interfaces.Model;
using Quicker.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quicker.Abstracts.Service
{
    /// <summary>
    ///     Implementacion principal de <see cref="ICloseServiceAsync{TKey, TEntity}"/>, que brinda 
    ///     funciones de ayuda para la lectura de datos.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Un <em>servicio cerrado</em> provee funciones de solo lectura de elementos en la base 
    ///         de datos, dando la R de el CRUD.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TKey">Tipo de la PK en la base de datos</typeparam>
    /// <typeparam name="TEntity">
    ///     <para>
    ///         Tipo de la entidad que debe devolver el servicio. 
    ///     </para>
    ///     <para>
    ///         Debe ser una clase e implementar <see cref="IAbstractModel{TKey}"/>.
    ///     </para>
    /// </typeparam>
    /// 
    public class CloseServiceAsync<TKey, TEntity> : ICloseServiceAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        protected readonly DbContext Context;

        public CloseServiceAsync(DbContext context)
        {
            Context = context;
        }

        /// <summary>
        ///     Funcion que retorna todos los elementos de tipo <typeparamref name="TEntity"/> de la base de datos.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Tiene la utilidad de centralizar la funcion de obtencion de todos los elementos de la base de datos.
        ///         Ademas, aca puedes dejar tu forma de cargar las entidades relacionadas. ¡Se recomienda usar <em>Eager loading</em>!
        ///     </para>
        /// </remarks>
        /// <returns>
        ///     Un <see cref="IQueryable{T}"/> de tipo <typeparamref name="TEntity"/> de solo lectura.
        /// </returns>
        /// 
        protected virtual IQueryable<TEntity> Query() 
            =>  Context.Set<TEntity>()
                .OrderBy(e => e.Id)
                .AsNoTracking();

        /// <summary>
        ///     Funcion para filtrar un <see cref="IQueryable{T}"/> de tipo <typeparamref name="TEntity"/>, por defecto, 
        ///     retorna su misma entrada
        /// </summary>
        /// <remarks>
        ///     Se usa en las funciones:
        ///     <list type="number">
        ///         <item><see cref="Read"/></item>
        ///         <item><see cref="Read(TKey)"/></item>
        ///         <item><see cref="Paginate(int, int)"/></item>
        ///     </list>
        /// </remarks>
        /// <param name="entities"></param>
        /// <returns>
        ///     Un <see cref="IQueryable{Task}"/> con los <typeparamref name="TEntity"/> que pasaron el filtro.
        /// </returns>
        /// 
        protected virtual IQueryable<TEntity> ReadFilter(IQueryable<TEntity> entities) => entities;

        /// <summary>
        ///     Retorna todos los elementos que pasan unas condiciones especificas
        /// </summary>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un <see cref="IEnumerable{T}"/> de tipo <typeparamref name="TEntity"/>
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        /// 

        protected async Task<IEnumerable<TEntity>> FindManyWith(params Expression<Func<TEntity, bool>>[] conditions)
        {
            if (conditions is null)
            {
                throw new ArgumentNullException(nameof(conditions));
            }

            var query = Query();

            foreach (var condition in conditions)
            {
                query = query.Where(condition);
            }

            var entities = await query.ToListAsync();

            return entities;
        }

        /// <summary>
        ///     Retorna un solo elemento que pasa unas condiciones especificas
        /// </summary>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un <see cref="IEnumerable{T}"/> de tipo <typeparamref name="TEntity"/>
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="InvalidOperationException" />
        /// 
        protected async Task<TEntity> FindOneWith(params Expression<Func<TEntity, bool>>[] conditions)
        {
            if (conditions is null)
            {
                throw new ArgumentNullException(nameof(conditions));
            }

            var query = Query();

            foreach (var condition in conditions)
            {
                query = query.Where(condition);
            }

            var entities = await query.SingleOrDefaultAsync();

            return entities;
        }

        /// <summary>
        ///     Retorna todos los elementos que pasan el filtro de <see cref="ReadFilter(IQueryable{TEntity})"/>
        /// </summary>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un <see cref="IEnumerable{T}"/> de tipo <typeparamref name="TEntity"/>
        /// </returns>
        /// 
        public virtual async Task<IEnumerable<TEntity>> Read()
        {
            var query = Query();

            var entities = await ReadFilter(query)
                .ToListAsync();

            return entities;
        }

        /// <summary>
        ///     Retorna todos los elementos que pasan el filtro de <see cref="ReadFilter(IQueryable{TEntity})"/>
        /// </summary>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un elemento tipo <typeparamref name="TEntity"/>
        /// </returns>
        /// 
        public virtual async Task<TEntity> Read(TKey key)
        {
            var query = Query();

            var entity = await ReadFilter(query)
                .Where(e => e.Id.Equals(key))
                .SingleOrDefaultAsync();

            if (entity != null)
                Context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        /// <summary>
        ///     Retorna un <see cref="IEnumerable{T}"/> de tipo <typeparamref name="TEntity"/> de largo <paramref name="number"/>, 
        ///     saltandose los <paramref name="number"/> * <paramref name="page"/> primeros, que pasan el filtro de 
        ///     <see cref="ReadFilter(IQueryable{TEntity})"/>
        /// </summary>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un <see cref="IEnumerable{T}"/> de tipo <typeparamref name="TEntity"/> con largo <paramref name="number"/>
        /// </returns>
        /// <param name="number">Cantidad de elementos a obtener</param>
        /// <param name="page">Pagina de los elementos</param>
        /// <exception cref="ArgumentException" />
        /// 
        public virtual async Task<IEnumerable<TEntity>> Paginate(int number, int page)
        {
            if (number < 1)
            {
                throw new ArgumentException(nameof(number));
            }

            if (page < 0)
            {
                throw new ArgumentException(nameof(page));
            }

            var query = Query();

            var entities = await ReadFilter(query)
                .Skip(page * number)
                .Take(number)
                .ToListAsync();

            return entities;
        } 
    }

    /// <summary>
    ///     Implementacion principal de <see cref="ICloseServiceAsync{TKey, TEntity, TEntityDTO}"/>, que brinda 
    ///     funciones de ayuda para la lectura de datos.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Un <em>servicio cerrado</em> provee funciones de solo lectura de elementos en la base de datos, dando la R de el CRUD.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TKey">Tipo de la PK en la base de datos</typeparam>
    /// <typeparam name="TEntity">
    ///     <para>
    ///         Tipo de la entidad que debe devolver la base de datos. 
    ///     </para>
    ///     <para>
    ///         Debe ser una clase, implementar <see cref="IAbstractModel{TKey}"/> y <see cref="IDTOOf{TDomain}"/>, 
    ///         con TDomain de valor <typeparamref name="TEntityDTO"/>
    ///     </para>
    /// </typeparam>
    /// <typeparam name="TEntityDTO">
    ///     <para>
    ///         Tipo de la entidad que debe devolver el servicio. 
    ///     </para>
    ///     <para>
    ///         Debe ser una clase, implementar <see cref="IAbstractModel{TKey}"/> y <see cref="IDomainOf{TDTO}"/>, 
    ///         con TDTO de valor <typeparamref name="TEntity"/>
    ///     </para>
    /// </typeparam>
    /// 
    public class CloseServiceAsync<TKey, TEntity, TEntityDTO> : ICloseServiceAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;

        public CloseServiceAsync(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
        /// <summary>
        ///     Constructor para inicializar la clase sin implementar el <see cref="IMapper"/>, es util en caso de que
        ///     vayas a implementar un mappeador diferente a AutoMapper
        /// </summary>
        /// <param name="context">Contexto de la base de datos</param>
        public CloseServiceAsync(DbContext context)
        {
            Context = context;
            Mapper = null;
        }

        /// <summary>
        ///     Funcion que retorna todos los elementos de tipo <typeparamref name="TEntity"/> de la base de datos.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Tiene la utilidad de centralizar la funcion de obtencion de todos los elementos de la base de datos.
        ///         Ademas, aca puedes dejar tu forma de cargar las entidades relacionadas. ¡Se recomienda usar <em>Eager loading</em>!
        ///     </para>
        /// </remarks>
        /// <returns>
        ///     Un <see cref="IQueryable{T}"/> de tipo <typeparamref name="TEntity"/> de solo lectura.
        /// </returns>
        /// 
        protected virtual IQueryable<TEntity> Query()
            => Context.Set<TEntity>()
                .OrderBy(e => e.Id)
                .AsNoTracking();

        /// <summary>
        ///     Funcion para filtrar un <see cref="IQueryable{T}"/> de tipo <typeparamref name="TEntity"/>, por defecto, 
        ///     retorna su misma entrada
        /// </summary>
        /// <remarks>
        ///     Se usa en las funciones:
        ///     <list type="number">
        ///         <item><see cref="Read"/></item>
        ///         <item><see cref="Read(TKey)"/></item>
        ///         <item><see cref="Paginate(int, int)"/></item>
        ///     </list>
        /// </remarks>
        /// <param name="entities"></param>
        /// <returns>
        ///     Un <see cref="IQueryable{Task}"/> con los <typeparamref name="TEntity"/> que pasaron el filtro.
        /// </returns>
        /// 
        protected virtual IQueryable<TEntity> ReadFilter(IQueryable<TEntity> entities) => entities;

        /// <summary>
        ///     Retorna todos los elementos que pasan unas condiciones especificas
        /// </summary>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un <see cref="IEnumerable{T}"/> de tipo <typeparamref name="TEntity"/>
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        /// 

        protected async Task<IEnumerable<TEntityDTO>> FindManyWith(params Expression<Func<TEntity, bool>>[] conditions)
        {
            if (conditions is null)
            {
                throw new ArgumentNullException(nameof(conditions));
            }

            var query = Query();

            foreach (var condition in conditions)
            {
                query = query.Where(condition);
            }

            var entities = await query.ToListAsync();

            return entities.Select(e => ToDTO(e));
        }

        /// <summary>
        ///     Retorna un solo elemento que pasa unas condiciones especificas
        /// </summary>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un <see cref="IEnumerable{T}"/> de tipo <typeparamref name="TEntity"/>
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="InvalidOperationException" />
        /// 
        protected async Task<TEntityDTO> FindOneWith(params Expression<Func<TEntity, bool>>[] conditions)
        {
            if (conditions is null)
            {
                throw new ArgumentNullException(nameof(conditions));
            }

            var query = Query();

            foreach (var condition in conditions)
            {
                query = query.Where(condition);
            }

            var entities = await query.SingleOrDefaultAsync();

            return ToDTO(entities);
        }

        /// <summary>
        ///     Retorna todos los elementos que pasan el filtro de <see cref="ReadFilter(IQueryable{TEntity})"/>
        /// </summary>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un <see cref="IEnumerable{T}"/> de tipo <typeparamref name="TEntityDTO"/>
        /// </returns>
        /// 

        public virtual async Task<IEnumerable<TEntityDTO>> Read()
        {
            var query = Query();

            var entities = await ReadFilter(query).ToListAsync();

            return entities.Select(e => ToDTO(e));
        }

        /// <summary>
        ///     Retorna todos los elementos que pasan el filtro de <see cref="ReadFilter(IQueryable{TEntity})"/>
        /// </summary>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un elemento tipo <typeparamref name="TEntityDTO"/>
        /// </returns>
        /// 

        public virtual async Task<TEntityDTO> Read(TKey key)
        {
            var query = Query();

            var entity = await ReadFilter(query)
                .Where(e => e.Id.Equals(key))
                .SingleOrDefaultAsync();

            return ToDTO(entity);
        }

        /// <summary>
        ///     Retorna un <see cref="IEnumerable{T}"/> de tipo <typeparamref name="TEntityDTO"/> de largo <paramref name="number"/>, 
        ///     saltandose los <paramref name="number"/> * <paramref name="page"/> primeros, que pasan el filtro de 
        ///     <see cref="ReadFilter(IQueryable{TEntity})"/>
        /// </summary>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un <see cref="IEnumerable{T}"/> de tipo <typeparamref name="TEntity"/> con largo <paramref name="number"/>
        /// </returns>
        /// <param name="number">Cantidad de elementos a obtener</param>
        /// <param name="page">Pagina de los elementos</param>
        /// 

        public virtual async Task<IEnumerable<TEntityDTO>> Paginate(int number, int page)
        {
            if (number < 1)
            {
                throw new ArgumentException(nameof(number));
            }

            if (page < 0)
            {
                throw new ArgumentException(nameof(page));
            }

            var query = Query();

            var entities = await ReadFilter(query)
                .Skip((page - 1) * number)
                .Take(number)
                .ToListAsync();

            return entities.Select(e => ToDTO(e));
        }

        /// <summary>
        ///     Mappea una entidad de tipo <typeparamref name="TEntity"/> a <typeparamref name="TEntityDTO"/>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Por defecto, emplea AutoMapper para esto, en caso de que no desees usarlo, debes usar el
        ///         constructor <see cref="CloseServiceAsync(DbContext)"/> y hacerle un override a este metodo 
        ///         y a <see cref="ToDomain(TEntityDTO)"/>
        ///     </para>
        /// </remarks>
        /// <param name="entity">Entidad de tipo <typeparamref name="TEntity"/> a convertir.</param>
        /// <returns>Una entidad de tipo <typeparamref name="TEntityDTO"/></returns>
        /// 
        protected virtual TEntityDTO ToDTO(TEntity entity)
            => Mapper.Map<TEntity, TEntityDTO>(entity);

        /// <summary>
        ///     Mappea una entidad de tipo <typeparamref name="TEntityDTO"/> a <typeparamref name="TEntity"/>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Por defecto, emplea AutoMapper para esto, en caso de que no desees usarlo, debes usar el
        ///         constructor <see cref="CloseServiceAsync(DbContext)"/> y hacerle un override a este metodo 
        ///         y a <see cref="ToDTO(TEntity)"/>
        ///     </para>
        /// </remarks>
        /// <param name="entity">Entidad de tipo <typeparamref name="TEntityDTO"/> a convertir.</param>
        /// <returns>Una entidad de tipo <typeparamref name="TEntity"/></returns>
        /// 
        protected virtual TEntity ToDomain(TEntityDTO entity)
            => Mapper.Map<TEntityDTO, TEntity>(entity);
    }
}
