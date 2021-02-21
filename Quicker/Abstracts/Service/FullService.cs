using Microsoft.EntityFrameworkCore;
using Quicker.Interfaces.Model;
using Quicker.Interfaces.Service;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace Quicker.Abstracts.Service
{
    
    /// <summary>
    /// <para>Main implementation of <seealso cref="IFullServiceAsync{TKey, TEntity}"/>.</para>
    /// </summary>
    public class FullServiceAsync<TKey, TEntity> : OpenServiceAsync<TKey, TEntity>, IFullServiceAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        public FullServiceAsync(DbContext context) : 
            base(context) { }

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
    /// </summary>
    public class FullServiceAsync<TKey, TEntity, TEntityDTO> : OpenServiceAsync<TKey, TEntity, TEntityDTO>, IFullServiceAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        public FullServiceAsync(DbContext context, IMapper mapper) : 
            base(context, mapper) { }

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

            entity = ValidateObject(entity);

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
