using Microsoft.EntityFrameworkCore;
using Quicker.Interfaces.Model;
using Quicker.Interfaces.Service;
using System;
using System.Collections.Generic;
using DA = System.ComponentModel.DataAnnotations;
using Fluent = FluentValidation;
using FluentValidation.Results;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Quicker.Abstracts.Service
{
#warning Agregar funciones de filtro al crear
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
        public OpenServiceAsync(IServiceProvider service) :
            base(service) { }

        /// <summary>
        ///     Metodo para validar una entidad, antes de crearla, si no es valida, arroja un 
        ///     <see cref="Fluent.ValidationException"/>.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Por defecto, aunque use <see cref="Fluent.ValidationException"/> para la 
        ///         excepcion, usa DataAnnotations para funcionar.
        ///     </para>
        ///     <para>
        ///         En caso de hacerle un override, recuerda incluir el loggeo si la no es valida.
        ///     </para>
        /// </remarks>
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

            LogIfNotNull(LogLevel.Warning,
                $"La entidad no es valida"
            );

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
        ///     Filtro que se aplica para ver que elementos no crear, en base a si tienen determinadas
        ///     propiedades, si no lo pasa, arroja un <see cref="InvalidOperationException"/>
        /// </summary>
        /// <remarks>
        ///     Si hacer un override a esta funcion, recuerda agregar loggin para cuando la entidad no 
        ///     pase el filtro
        /// </remarks>
        /// <exception cref="InvalidOperationException" />
        /// 
        protected virtual void FilteringEntitiesBeforeCreating(TEntity entity)
        {
        }

        /// <summary>
        ///     Metodo para presetear los valores de la entidad que se creara.
        /// </summary>
        /// 
        protected virtual void PresetPropertiesBeforeCreating(TEntity entity) { }

        /// <summary>
        ///     <para>
        ///         Crea un registro en la base de datos basado en la entidad ingresada por parametro, retornando 
        ///         la entidad una vez sea creada
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Si la entidad es nula, arroja un <see cref="ArgumentNullException"/>.
        ///     </para>
        ///     <para>
        ///         Si la entidad no es valida, arroja un <see cref="Fluent.ValidationException"/>
        ///     </para>
        ///     <para>
        ///         Si la entidad ya existe en la base de datos (segun la PK), retorna un <see cref="InvalidOperationException"/>
        ///     </para>
        ///     <para>
        ///         En caso de que haya algun problema al momento de actualizar en la base de datos,
        ///         arroja un <see cref="DbUpdateException"/> o un <see cref="DbUpdateConcurrencyException"/>
        ///     </para>
        /// </remarks>
        /// <returns>
        ///     Un <see cref="Task"/> que retorna un <typeparamref name="TEntity"/>
        /// </returns>
        /// <exception cref="Fluent.ValidationException" />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="InvalidOperationException" />
        /// <exception cref="DbUpdateException" />
        /// <exception cref="DbUpdateConcurrencyException" />
        /// <param name="entity">Entidad con la que se creara el registro</param>
        /// 
        public virtual async Task<TEntity> Create(TEntity entity)
        {
            LogIfNotNull(LogLevel.Information,
                $"Creando entidad de tipo {{{typeof(TEntity).Name}}}"
            );

            if (entity is null)
            {
                LogIfNotNull(LogLevel.Warning,
                    $"La entidad enviada es nula"
                );

                throw new ArgumentNullException(nameof(entity));
            }

            LogIfNotNull(LogLevel.Information,
                $"Validando la entidad..."
            );

            ValidateObjectBeforeCreating(entity);

            LogIfNotNull(LogLevel.Information,
                $"Comprobando si es posible crear..."
            );

            FilteringEntitiesBeforeCreating(entity);
            
            LogIfNotNull(LogLevel.Information,
                "Preseteando valores..."
            );

            PresetPropertiesBeforeCreating(entity);

            entity.CreatedAt = DateTime.Now;
            entity.LastUpdated = DateTime.Now;

            var entry = Context.Set<TEntity>().Add(entity);
            await Context.SaveChangesAsync();

            var created = await Query()
                .Where(e => e.Id.Equals(entry.Entity.Id))
                .SingleOrDefaultAsync();

            LogIfNotNull(LogLevel.Information,
                $"Se ha creado con exito"
            );

            return created;
        }

        /// <summary>
        ///     Filtro que se aplica para ver que elementos no borrar, en base a si tienen determinadas
        ///     propiedades, si no lo pasa, arroja un <see cref="InvalidOperationException"/>
        /// </summary>
        /// <remarks>
        ///     Si hacer un override a esta funcion, recuerda agregar loggin para cuando la entidad no 
        ///     pase el filtro
        /// </remarks>
        /// <exception cref="InvalidOperationException" />
        /// 
        protected virtual void FilteringEntitiesBeforeDeleting(TEntity entity)
        {
        }

        /// <summary>
        ///     Borra el registro relacionado a la PK que al modelo pasado por parametro, si pasa el filtro, 
        ///     si no lo pasa, arroja un <see cref="InvalidOperationException"/>
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="InvalidOperationException" />
        /// 
        public virtual Task Delete(TEntity entity)
        {
            if (entity == null) { 
                LogIfNotNull(LogLevel.Warning,
                    "No es posible buscar, la entidad es nula"
                );

                throw new ArgumentNullException(nameof(entity));
            }

            return Delete(entity.Id);
        }

        /// <summary>
        ///     Borra el registro relacionado a la PK que se paso por parametro, si pasa el filtro, 
        ///     si no lo pasa, arroja un <see cref="InvalidOperationException"/>
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="InvalidOperationException" />
        /// 
        public virtual async Task Delete(TKey key)
        {
            LogIfNotNull(LogLevel.Information,
                $"Borrando entidad de tipo {{{typeof(TEntity).Name}}} y {{Id}} {key}"
            );

            if (key == null)
            {
                LogIfNotNull(LogLevel.Warning,
                    "No es posible buscar, la key es nula"
                );

                throw new ArgumentNullException(nameof(key));
            }

            var entity = await Context.Set<TEntity>().FindAsync(key);

            if (entity == null)
            {
                LogIfNotNull(LogLevel.Warning,
                    "No ha sido encontrada la entidad"
                );

                throw new InvalidOperationException(nameof(entity));
            }

            LogIfNotNull(LogLevel.Information,
                $"Comprobando si es posible borrar..."
            );

            FilteringEntitiesBeforeDeleting(entity);

            Context.Entry(entity).State = EntityState.Deleted;

            await Context.SaveChangesAsync();

            LogIfNotNull(LogLevel.Information,
                $"Se ha borrado con exito"
            );
        }

        /// <summary>
        ///     Retorna el tipo de todas las variables primitivas (O string) que tiene la clase.
        ///     Este metodo es util sobre todo en WebAPI's en donde debes indicar al front que datos 
        ///     incluye una entidad.
        /// </summary>
        /// <returns>
        ///     Un <see cref="Dictionary{string, string}"/> con las key el nombre de la propiedad y value
        ///     el tipo.
        /// </returns>
        /// 
        public Dictionary<string, string> GetPropertyInformationForCreate()
        {
            Dictionary<string, string> propertyTypes = new Dictionary<string, string>();

            Type entityType = typeof(TEntity);
            var propertyInfos = entityType.GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                var propertyType = propertyInfo.PropertyType;

                if (!(propertyType.IsClass || propertyType.IsInterface) || propertyType == typeof(String))
                    propertyTypes.Add(propertyInfo.Name, propertyInfo.PropertyType.Name);
            }

            return propertyTypes;
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
        public OpenServiceAsync(IServiceProvider service) :
            base(service)
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

            LogIfNotNull(LogLevel.Warning,
                $"La entidad no es valida"
            );

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
        ///     Filtro que se aplica para ver que elementos no borrar, en base a si tienen determinadas
        ///     propiedades, si no lo pasa, arroja un <see cref="InvalidOperationException"/>
        /// </summary>
        /// <remarks>
        ///     Si hacer un override a esta funcion, recuerda agregar loggin para cuando la entidad no 
        ///     pase el filtro
        /// </remarks>
        /// <exception cref="InvalidOperationException" />
        /// 
        protected virtual void FilteringEntitiesBeforeCreating(TEntity entity)
        {
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
            LogIfNotNull(LogLevel.Information,
                $"Creando entidad de tipo {{{typeof(TEntity).Name}}}"
            );

            if (entity is null)
            {
                LogIfNotNull(LogLevel.Warning,
                    $"La entidad enviada es nula"
                );

                throw new ArgumentNullException(nameof(entity));
            }

            LogIfNotNull(LogLevel.Information,
                $"Validando la entidad..."
            );

            ValidateObjectBeforeCreating(entity);

            entity.CreatedAt = DateTime.Now;
            entity.LastUpdated = DateTime.Now;

            LogIfNotNull(LogLevel.Information,
                "Volviendo DTO a entidad de Dominio..."
            );

            var domain = ToDomain(entity);

            LogIfNotNull(LogLevel.Information,
                $"Comprobando si es posible crear..."
            );

            FilteringEntitiesBeforeCreating(domain);

            LogIfNotNull(LogLevel.Information,
                "Preseteando valores..."
            );

            PresetPropertiesBeforeCreating(domain);

            var tracked = Context.Set<TEntity>().Add(domain);
            await Context.SaveChangesAsync();

            var created = await Query()
                .Where(e => e.Id.Equals(tracked.Entity.Id))
                .SingleOrDefaultAsync();

            LogIfNotNull(LogLevel.Information,
                $"Se ha creado con exito, pasando a DTO..."
            );

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
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="InvalidOperationException" />
        /// 
        public virtual Task Delete(TEntityDTO entity)
        {
            if (entity == null)
            {
                LogIfNotNull(LogLevel.Warning,
                    "No es posible buscar, la entidad es nula"
                );

                throw new ArgumentNullException(nameof(entity));
            }

            return Delete(entity.Id);
        }

        /// <summary>
        ///     Borra el registro relacionado a la PK que se paso por parametro, si pasa el filtro, 
        ///     si no lo pasa, arroja un <see cref="InvalidOperationException"/>
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="InvalidOperationException" />
        /// 
        public virtual async Task Delete(TKey key)
        {
            LogIfNotNull(LogLevel.Information,
                $"Borrando entidad de tipo {{{typeof(TEntity).Name}}} y {{Id}} {key}"
            );

            if (key == null)
            {
                LogIfNotNull(LogLevel.Warning,
                    "No es posible buscar, la key es nula"
                );

                throw new ArgumentNullException(nameof(key));
            }
            var entity = await Context.Set<TEntity>().FindAsync(key);

            if (entity == null)
            {
                LogIfNotNull(LogLevel.Warning,
                    "No ha sido encontrada la entidad"
                );

                throw new InvalidOperationException(nameof(entity));
            }

            DeleteFilter(entity);

            Context.Entry(entity).State = EntityState.Deleted;

            await Context.SaveChangesAsync();

            LogIfNotNull(LogLevel.Information,
                $"Se ha borrado con exito"
            );
        }

        /// <summary>
        ///     Retorna el tipo de todas las variables primitivas (O string) que tiene la clase.
        ///     Este metodo es util sobre todo en WebAPI's en donde debes indicar al front que datos 
        ///     incluye una entidad.
        /// </summary>
        /// <returns>
        ///     Un <see cref="Dictionary{string, string}"/> con las key el nombre de la propiedad y value
        ///     el tipo.
        /// </returns>
        /// 
        public Dictionary<string, string> GetPropertyInformationForCreate()
        {
            Dictionary<string, string> propertyTypes = new Dictionary<string, string>();

            Type entityType = typeof(TEntityDTO);
            var propertyInfos = entityType.GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                var propertyType = propertyInfo.PropertyType;

                if (!(propertyType.IsClass || propertyType.IsInterface) || propertyType == typeof(String))
                    propertyTypes.Add(propertyInfo.Name, propertyInfo.PropertyType.Name);
            }

            return propertyTypes;
        }
    }
}
