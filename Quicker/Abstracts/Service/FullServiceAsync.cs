using Microsoft.EntityFrameworkCore;
using Quicker.Interfaces.Model;
using Quicker.Interfaces.Service;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Quicker.Configuration;

namespace Quicker.Abstracts.Service
{
    /// <summary>
    ///     Implementacion principal de <see cref="IFullServiceAsync{TKey, TEntity}"/>, que brinda 
    ///     funciones de ayuda para la creacion, actualizacion y borrado de datos.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Un <em>servicio completo</em> provee funciones de lectura, escritura, actualizacion 
    ///         y borrado de elementos en la base de datos, dando el CRUD.
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
    public abstract class FullServiceAsync<TKey, TEntity> : OpenServiceAsync<TKey, TEntity>, IFullServiceAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        public FullServiceAsync(QuickerConfiguration configuration, IServiceProvider service) :
            base(configuration, service)
        { }

        /// <summary>
        /// <para>Main implementation of <seealso cref="IFullService{TKey, TEntity}.Update(TKey, TEntity)"/>.</para>
        /// <exception cref="InvalidOperationException">If the provied key and the entity key aren't equal.</exception>
        /// <exception cref="ValidationException">If the provied entity is not valid and the validation form is the default (DataAnnotations).</exception>
        /// </summary>
        public async Task<TEntity> Update(TKey key, TEntity entity)
        {
            if (!entity.Id.Equals(key))
                throw new InvalidOperationException();

            TEntity original = await Context.Set<TEntity>().FindAsync(key);

            if (original == null)
                return null;

            /*entity = ValidateObject(entity);*/

            SetNonUpdatableFields(entity, original);
            entity.LastUpdated = DateTime.Now;

            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return entity;
        }

        /// <summary>
        /// <para>A method to set values that should not be updated through this method, but through other methods.</para>
        /// </summary>
        protected virtual void SetNonUpdatableFields(TEntity updated, TEntity original)
        {
            updated.CreatedAt = original.CreatedAt;
        }
    }
    /// <summary>
    /// <para>Main implementation of <seealso cref="IFullServiceAsync{TKey, TEntity}"/>.</para>
    /// </summary>23
    public abstract class FullServiceAsync<TKey, TEntity, TEntityDTO> : OpenServiceAsync<TKey, TEntity, TEntityDTO>, IFullServiceAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        public FullServiceAsync(QuickerConfiguration configuration, IServiceProvider service) :
            base(configuration, service)
        { }

        /// <summary>
        /// <para>Main implementation of <seealso cref="IFullService{TKey, TEntity}.Update(TKey, TEntity)"/>.</para>
        /// <exception cref="InvalidOperationException">If the provied key and the entity key aren't equal.</exception>
        /// <exception cref="ValidationException">If the provied entity is not valid and the validation form is the default (DataAnnotations).</exception>
        /// </summary>
        public async Task<TEntityDTO> Update(TKey key, TEntityDTO entity)
        {
            if (!entity.Id.Equals(key))
                throw new InvalidOperationException();

            TEntity original = await Context.Set<TEntity>().FindAsync(key);

            if (original == null)
                return null;

            ValidateObjectBeforeCreating(entity);

            var domain = ToDomain(entity);

            SetNonUpdatableFields(domain, original);
            entity.LastUpdated = DateTime.Now;

            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return entity;
        }

        /// <summary>
        /// <para>A method to set values that should not be updated through this method, but through other methods.</para>
        /// </summary>
        protected virtual void SetNonUpdatableFields(TEntity updated, TEntity original)
        {
            updated.CreatedAt = original.CreatedAt;
        }
    }
}
