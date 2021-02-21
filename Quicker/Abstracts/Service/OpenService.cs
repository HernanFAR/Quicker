using Microsoft.EntityFrameworkCore;
using Quicker.Interfaces.Model;
using Quicker.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicker.Abstracts.Service
{
    /// <summary>
    ///     Implementacion principal de <see cref="IOpenServiceAsync{TKey, TEntity}"/>, que brinda 
    ///     funciones de ayuda para la creacion y borrado de datos.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Un <em>servicio abierto</em> provee funciones de lectura, escritura y borrado de 
    ///         elementos en la base de datos, dando la CRD de el CRUD.
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
    public class OpenServiceAsync<TKey, TEntity> : CloseServiceAsync<TKey, TEntity>, IOpenServiceAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        public OpenServiceAsync(DbContext context) :
            base(context) { }

        /// <summary>
        ///     Metodo para validar una entidad, antes de crearla, si no es valida, arroja un 
        ///     <see cref="ValidationException"/>.
        /// </summary>
        /// <exception cref="ValidationException" />
        /// <exception cref="ArgumentNullException" />
        /// 
        protected virtual void ValidateObjectBeforeCreating(TEntity entity)
        {
            var context = new ValidationContext(entity);

            Validator.ValidateObject(entity, context);
        }

        /// <summary>
        ///     Metodo para presetear los valores de la entidad que se creara.
        /// </summary>
        /// 
        protected virtual void PresetPropertiesBeforeCreating(TEntity entity)
        { }

        /// <summary>
        ///     <para>
        ///         Crea un registro en la base de datos basado en la entidad ingresada por parametro, 
        ///         si no es una entidad valida, arroja un <see cref="ValidationException"/>.
        ///     </para>
        ///     <para>
        ///         En caso de que haya algun problema al momento de actualizar en la base de datos,
        ///         arroj a un <see cref="DbUpdateConcurrencyException"/> o un <see cref="DbUpdateException"/>
        ///     </para>
        /// </summary>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un elemento de tipo <typeparamref name="TEntity"/>
        /// </returns>
        /// <exception cref="ValidationException" />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="DbUpdateException" />
        /// <exception cref="DbUpdateConcurrencyException" />
        /// <param name="entity">Entidad con la que se creara el registro</param>
        /// 
        public virtual async Task<TEntity> Create(TEntity entity)
        {
            ValidateObjectBeforeCreating(entity);

            PresetPropertiesBeforeCreating(entity);

            entity.CreatedAt = DateTime.Now;
            entity.LastUpdated = DateTime.Now;

            var entry = Context.Set<TEntity>().Add(entity);
            await Context.SaveChangesAsync();

            var created = await Query()
                .Where(e => e.Id.Equals(entry.Entity.Id))
                .SingleOrDefaultAsync();

            return created;
        }
        /// <summary>
        ///     Filtro que se aplica para ver que elementos no borrar, en base a si tienen determinadas
        ///     propiedades, si no lo pasa, arroja un <see cref="InvalidOperationException"/>
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        /// 
        protected virtual void DeleteFilter(TEntity entity) {}

        /// <summary>
        ///     Borra el registro relacionado a la PK que se paso por parametro, si pasa el filtro, 
        ///     si no lo pasa, arroja un <see cref="InvalidOperationException"/>
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        /// 
        public virtual async Task Delete(TKey key)
        {
            var entity = await Context.Set<TEntity>().FindAsync(key);

            DeleteFilter(entity);

            Context.Entry(entity).State = EntityState.Deleted;

            await Context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// <para>Main implementation of <seealso cref="IOpenServiceAsync{TKey, TEntity, TEntityDTO}"/>.</para>
    /// </summary>
    public class OpenServiceAsync<TKey, TEntity, TEntityDTO> : CloseServiceAsync<TKey, TEntity, TEntityDTO>, IOpenServiceAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        public OpenServiceAsync(DbContext context, AutoMapper.IMapper mapper) : 
            base(context, mapper) { }

        /// <summary>
        /// <para>Main implementation of <seealso cref="IOpenServiceAsync{TKey, TEntity}.Create(TEntity)"/>.</para>
        /// </summary>
        public async Task<TEntityDTO> Create(TEntityDTO entity)
        {
            entity = ValidateObject(entity);

            entity.CreatedAt = DateTime.Now;
            entity.LastUpdated = DateTime.Now;

            var tracked = Context.Set<TEntityDTO>().Add(entity);
            await Context.SaveChangesAsync();

            var created = await Query()
                .Where(e => e.Id.Equals(tracked.Entity.Id))
                .SingleOrDefaultAsync();

            var dto = ToDTO(created);

            return dto;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="IOpenServiceAsync{TKey, TEntity}.Delete(TKey)"/>.</para>
        /// </summary>
        public async Task Delete(TKey key)
        {
            var entity = await Context.Set<TEntity>().FindAsync(key);

            Context.Entry(entity).State = EntityState.Deleted;

            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// <para>Method for validate a entity before create it.</para>
        /// <exception cref="ValidationException">If the provied entity is not valid and the validation form is the default (DataAnnotations).</exception>
        /// </summary>
        protected virtual TEntityDTO ValidateObject(TEntityDTO entity)
        {
            Validator.ValidateObject(entity, new ValidationContext(entity));

            return entity;
        }
    }
}
