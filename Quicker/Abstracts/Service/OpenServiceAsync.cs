﻿using Microsoft.EntityFrameworkCore;
using Quicker.Interfaces.Model;
using Quicker.Interfaces.Service;
using System;
using System.Collections.Generic;
using DA = System.ComponentModel.DataAnnotations;
using Fluent = FluentValidation;
using FluentValidation.Results;
using System.Linq;
using System.Threading.Tasks;
using Quicker.Configuration;

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
    public abstract class OpenServiceAsync<TKey, TEntity> : CloseServiceAsync<TKey, TEntity>, IOpenServiceAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        public OpenServiceAsync(QuickerConfiguration configuration, IServiceProvider service) :
            base(configuration, service) { }

        /// <summary>
        ///     Metodo para validar una entidad, antes de crearla, si no es valida, arroja un 
        ///     <see cref="Fluent.ValidationException"/>.
        /// </summary>
        /// <exception cref="Fluent.ValidationException" />
        /// <exception cref="ArgumentNullException" />
        /// 
        protected virtual void ValidateObjectBeforeCreating(TEntity entity)
        {
            var context = new DA.ValidationContext(entity);
            List<DA.ValidationResult> errors = new List<DA.ValidationResult>();

            var isValid = DA.Validator.TryValidateObject(entity, context, errors, true);

            if (isValid)
                return;

            List<ValidationFailure> validationFailures = new List<ValidationFailure>();

            errors.ForEach(e => 
                validationFailures.Add(
                    new ValidationFailure(
                        string.Join(", ", e.MemberNames),
                        e.ErrorMessage
                    )
                )
            );

            throw new Fluent.ValidationException(validationFailures);
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
        ///         si no es una entidad valida, arroja un <see cref="Fluent.ValidationException"/>.
        ///     </para>
        ///     <para>
        ///         En caso de que haya algun problema al momento de actualizar en la base de datos,
        ///         arroja un <see cref="DbUpdateConcurrencyException"/> o un <see cref="DbUpdateException"/>,
        ///         si hay un conflicto con la ID, arroja un <see cref="InvalidOperationException"/>
        ///     </para>
        /// </summary>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un <typeparamref name="TEntity"/>
        /// </returns>
        /// <exception cref="Fluent.ValidationException" />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="DbUpdateException" />
        /// <exception cref="InvalidOperationException" />
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
        protected virtual void DeleteFilter(TEntity entity) { }

        /// <summary>
        ///     Borra el registro relacionado a la PK que al modelo pasado por parametro, si pasa el filtro, 
        ///     si no lo pasa, arroja un <see cref="InvalidOperationException"/>
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        /// 
        public virtual Task Delete(TEntity entity)
            => Delete(entity.Id);

        /// <summary>
        ///     Borra el registro relacionado a la PK que se paso por parametro, si pasa el filtro, 
        ///     si no lo pasa, arroja un <see cref="InvalidOperationException"/>
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        /// 
        public virtual async Task Delete(TKey key)
        {
            var entity = await Context.Set<TEntity>().FindAsync(key);

            if (entity == null) throw new InvalidOperationException(nameof(entity));

            DeleteFilter(entity);

            Context.Entry(entity).State = EntityState.Deleted;

            await Context.SaveChangesAsync();
        }
    }

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
    public abstract class OpenServiceAsync<TKey, TEntity, TEntityDTO> : CloseServiceAsync<TKey, TEntity, TEntityDTO>, IOpenServiceAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        public OpenServiceAsync(QuickerConfiguration configuration, IServiceProvider service) :
            base(configuration, service)
        { }

        /// <summary>
        ///     Metodo para validar una entidad, antes de crearla, si no es valida, arroja un 
        ///     <see cref="Fluent.ValidationException"/>.
        /// </summary>
        /// <exception cref="Fluent.ValidationException" />
        /// <exception cref="ArgumentNullException" />
        /// 
        protected virtual void ValidateObjectBeforeCreating(TEntityDTO entity)
        {
            var context = new DA.ValidationContext(entity);
            List<DA.ValidationResult> errors = new List<DA.ValidationResult>();

            var isValid = DA.Validator.TryValidateObject(entity, context, errors, true);

            if (isValid)
                return;

            List<ValidationFailure> validationFailures = new List<ValidationFailure>();

            errors.ForEach(e =>
                validationFailures.Add(
                    new ValidationFailure(
                        string.Join(", ", e.MemberNames),
                        e.ErrorMessage
                    )
                )
            );

            throw new Fluent.ValidationException(validationFailures);
        }

        /// <summary>
        ///     Metodo para presetear los valores de la entidad que se creara.
        /// </summary>
        /// 
        protected virtual void PresetPropertiesBeforeCreating(TEntityDTO entity)
        { }

        /// <summary>
        ///     <para>
        ///         Crea un registro en la base de datos basado en la entidad ingresada por parametro, 
        ///         si no es una entidad valida, arroja un <see cref="Fluent.ValidationException"/>.
        ///     </para>
        ///     <para>
        ///         En caso de que haya algun problema al momento de actualizar en la base de datos,
        ///         arroja un <see cref="DbUpdateConcurrencyException"/> o un <see cref="DbUpdateException"/>,
        ///         si hay un conflicto con la ID, arroja un <see cref="InvalidOperationException"/>
        ///     </para>
        /// </summary>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un <typeparamref name="TEntityDTO"/>
        /// </returns>
        /// <exception cref="Fluent.ValidationException" />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="DbUpdateException" />
        /// <exception cref="InvalidOperationException" />
        /// <exception cref="DbUpdateConcurrencyException" />
        /// <param name="entity">Entidad con la que se creara el registro</param>
        /// 
        public virtual async Task<TEntityDTO> Create(TEntityDTO entity)
        {
            ValidateObjectBeforeCreating(entity);

            PresetPropertiesBeforeCreating(entity);

            entity.CreatedAt = DateTime.Now;
            entity.LastUpdated = DateTime.Now;

            var toCreate = ToDomain(entity);

            var tracked = Context.Set<TEntity>().Add(toCreate);
            await Context.SaveChangesAsync();

            var created = await Query()
                .Where(e => e.Id.Equals(tracked.Entity.Id))
                .SingleOrDefaultAsync();

            var dto = ToDTO(created);

            return dto;
        }
        /// <summary>
        ///     Filtro que se aplica para ver que elementos no borrar, en base a si tienen determinadas
        ///     propiedades, si no lo pasa, arroja un <see cref="InvalidOperationException"/>
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        /// 
        protected virtual void DeleteFilter(TEntity entity) { }

        /// <summary>
        ///     Borra el registro relacionado a la entidad que se paso por parametro, si pasa el 
        ///     filtro, si no lo pasa, arroja un <see cref="InvalidOperationException"/>
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        /// 
        public virtual Task Delete(TEntityDTO entity)
            => Delete(entity.Id);

        /// <summary>
        ///     Borra el registro relacionado a la PK que se paso por parametro, si pasa el filtro, 
        ///     si no lo pasa, arroja un <see cref="InvalidOperationException"/>
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        /// 
        public virtual async Task Delete(TKey key)
        {
            var entity = await Context.Set<TEntity>().FindAsync(key);

            if (entity == null) throw new InvalidOperationException(nameof(entity));

            DeleteFilter(entity);

            Context.Entry(entity).State = EntityState.Deleted;

            await Context.SaveChangesAsync();
        }
    }
}
