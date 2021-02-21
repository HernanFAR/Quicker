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
    /// <para>Main implementation of <seealso cref="IOpenServiceAsync{TKey, TEntity}"/>.</para>
    /// </summary>
    public class OpenService<TKey, TEntity> : CloseServiceAsync<TKey, TEntity>, IOpenServiceAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        public OpenService(DbContext context) : 
            base(context) { }

        /// <summary>
        /// <para>Main implementation of <seealso cref="IOpenServiceAsync{TKey, TEntity}.Create(TEntity)"/>.</para>
        /// </summary>
        public async Task<TEntity> Create(TEntity entity)
        {
            entity = ValidateObject(entity);

            entity.CreatedAt = DateTime.Now;
            entity.LastUpdated = DateTime.Now;

            Context.Set<TEntity>().Add(entity);
            await Context.SaveChangesAsync();

            var created = Context.Entry(entity);
            created.State = EntityState.Detached;

            return created.Entity;
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
        /// </summary>
        protected virtual TEntity ValidateObject(TEntity entity)
        {
            Validator.ValidateObject(entity, new ValidationContext(entity));

            return entity;
        }
    }
    /// <summary>
    /// <para>Main implementation of <seealso cref="IOpenServiceAsync{TKey, TEntity, TEntityDTO}"/>.</para>
    /// </summary>
    public class OpenService<TKey, TEntity, TEntityDTO> : CloseServiceAsync<TKey, TEntity, TEntityDTO>, IOpenServiceAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        public OpenService(DbContext context, AutoMapper.IMapper mapper) : 
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
