using Microsoft.EntityFrameworkCore;
using Quicker.Interfaces.Model;
using Quicker.Interfaces.Service;
using System;
using System.Threading.Tasks;
using DA = System.ComponentModel.DataAnnotations;
using Fluent = FluentValidation;
using System.Collections.Generic;
using FluentValidation.Results;

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
        public FullServiceAsync(IServiceProvider service) :
            base(service)
        { }

        /// <summary>
        ///     Metodo para validar una entidad, antes de actualizarla, si no es valida, arroja un 
        ///     <see cref="Fluent.ValidationException"/>.
        /// </summary>
        /// <remarks>
        ///     Por defecto, aunque use <see cref="Fluent.ValidationException"/> para la excepcion, 
        ///     usa DataAnnotations para funcionar.
        /// </remarks>
        /// <exception cref="Fluent.ValidationException" />
        /// <exception cref="ArgumentNullException" />
        /// 
        protected virtual void ValidateObjectBeforeUpdating(TEntity entity)
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
        ///     Metodo para presetear los valores de la entidad que se actualice.
        /// </summary>
        /// 
        protected virtual void PresetPropertiesBeforeUpdating(TEntity updated, TEntity original)
        { }

        /// <summary>
        ///     <para>
        ///         Actualiza un registro en la base de datos basado en la entidad ingresada por parametro, 
        ///         retornando la entidad una vez se actualice.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Si se entrega una entidad nula, arroja un <see cref="ArgumentNullException"/>.
        ///     </para>
        ///     <para>
        ///         Si la Id del modelo es diferente a la pasada por parametro, arroja un 
        ///         <see cref="InvalidOperationException"/>, con mensaje <em>"key"</em>.
        ///     </para>
        ///     <para>
        ///         Si la entidad no existe en la base de datos, retorna un null.
        ///     </para>
        ///     <para>
        ///         Si la entidad no es valida, retorna un <see cref="Fluent.ValidationException"/>
        ///     </para>
        ///     <para>
        ///         En caso de que haya algun problema al momento de actualizar en la base de datos,
        ///         arroja un <see cref="DbUpdateConcurrencyException"/> o un <see cref="DbUpdateException"/>.
        ///     </para>
        /// </remarks>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un <typeparamref name="TEntity"/>
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="InvalidOperationException" />
        /// <exception cref="Fluent.ValidationException" />
        /// <exception cref="DbUpdateException" />
        /// <exception cref="DbUpdateConcurrencyException" />
        /// <param name="entity">Entidad con la que se creara el registro</param>
        /// 
        public async Task<TEntity> Update(TKey key, TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (!entity.Id.Equals(key))
                throw new InvalidOperationException(nameof(key));

            TEntity original = await Context.Set<TEntity>().FindAsync(key);

            if (original == null)
                return null;

            ValidateObjectBeforeUpdating(entity);
            PresetPropertiesBeforeUpdating(entity, original);

            entity.CreatedAt = original.CreatedAt;
            entity.LastUpdated = DateTime.Now;

            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return entity;
        }
    }
    /// <summary>
    ///     Implementacion principal de <see cref="IFullServiceAsync{TKey, TEntity, TEntityDTO}"/>, que brinda 
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
    public abstract class FullServiceAsync<TKey, TEntity, TEntityDTO> : OpenServiceAsync<TKey, TEntity, TEntityDTO>, IFullServiceAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        public FullServiceAsync(IServiceProvider service) :
            base(service)
        { }

        /// <summary>
        ///     Metodo para validar una entidad, antes de actualizarla, si no es valida, arroja un 
        ///     <see cref="Fluent.ValidationException"/>.
        /// </summary>
        /// <remarks>
        ///     Por defecto, aunque use <see cref="Fluent.ValidationException"/> para la excepcion, 
        ///     usa DataAnnotations para funcionar.
        /// </remarks>
        /// <exception cref="Fluent.ValidationException" />
        /// <exception cref="ArgumentNullException" />
        /// 
        protected virtual void ValidateObjectBeforeUpdating(TEntityDTO entity)
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
        ///     Metodo para presetear los valores de la entidad que se actualice.
        /// </summary>
        /// 
        protected virtual void PresetPropertiesBeforeUpdating(TEntity updated, TEntity original)
        { }

        /// <summary>
        ///     <para>
        ///         Actualiza un registro en la base de datos basado en la entidad ingresada por parametro, 
        ///         retornando la entidad una vez se actualice.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Si se entrega una entidad nula, arroja un <see cref="ArgumentNullException"/>.
        ///     </para>
        ///     <para>
        ///         Si la Id del modelo es diferente a la pasada por parametro, arroja un 
        ///         <see cref="InvalidOperationException"/>, con mensaje <em>"key"</em>.
        ///     </para>
        ///     <para>
        ///         Si la entidad no existe en la base de datos, retorna un null.
        ///     </para>
        ///     <para>
        ///         Si la entidad no es valida, retorna un <see cref="Fluent.ValidationException"/>
        ///     </para>
        ///     <para>
        ///         En caso de que haya algun problema al momento de actualizar en la base de datos,
        ///         arroja un <see cref="DbUpdateConcurrencyException"/> o un <see cref="DbUpdateException"/>.
        ///     </para>
        /// </remarks>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un <typeparamref name="TEntity"/>
        /// </returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="InvalidOperationException" />
        /// <exception cref="Fluent.ValidationException" />
        /// <exception cref="DbUpdateException" />
        /// <exception cref="DbUpdateConcurrencyException" />
        /// <param name="entity">Entidad con la que se creara el registro</param>
        /// 
        public async Task<TEntityDTO> Update(TKey key, TEntityDTO entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (!entity.Id.Equals(key))
                throw new InvalidOperationException(nameof(key));

            TEntity original = await Context.Set<TEntity>().FindAsync(key);

            if (original == null)
                return null;

            var domain = ToDomain(entity);

            ValidateObjectBeforeUpdating(entity);
            PresetPropertiesBeforeUpdating(domain, original);

            domain.CreatedAt = original.CreatedAt;
            domain.LastUpdated = DateTime.Now;

            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();

            return entity;
        }
    }
}
