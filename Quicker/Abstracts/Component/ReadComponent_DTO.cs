﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quicker.Configuration;
using Quicker.Interfaces.Component;
using Quicker.Interfaces.Model;
using Quicker.Service.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Quicker.Abstracts.Component
{
    public abstract class ReadComponent<TKey, TEntity, TEntityDTO> : IReadComponent<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        protected DbContext Context { get; }

        protected ILogger Logger { get; }

        public ReadComponent(IServiceProvider service)
        {
            Context = service.GetRequiredService<DbContext>();
            var configuration = service.GetRequiredService<IOptions<QuickerConfiguration>>().Value;

            if (configuration.UseLogger)
            {
                var factoryLogger = service.GetRequiredService<ILoggerFactory>();
                Logger = factoryLogger.CreateLogger(GetType().FullName);
            }
        }

        #region Protected methods

        protected virtual IQueryable<TEntity> Query()
            => Context.Set<TEntity>()
                .OrderBy(e => e.Id)
                .AsNoTracking();

        protected virtual IQueryable<TEntity> ReadFilter(IQueryable<TEntity> entities) => entities;

        protected abstract TEntityDTO ToDTO(TEntity model);

        protected async Task<IEnumerable<TEntityDTO>> FindManyWith(
            Func<Task<bool>> action = null, 
            Func<IQueryable<TEntity>> queryFunction = null,
            params Expression<Func<TEntity, bool>>[] conditions
        )
        {
            if (action != null)
            {
                Logger?.Log(
                    LogLevel.Information,
                    $"Acción: {{{nameof(CheckExistence)}}}, validando con pre-acción."
                );

                if (!await action?.Invoke())
                    return default;
            }

            if (conditions is null)
            {
                throw new ArgumentNullException(QuickerExceptionConstants.Conditions);
            }

            var query = queryFunction?.Invoke() ?? Query();

            foreach (var condition in conditions)
            {
                query = query.Where(condition);
            }

            var entities = await query.ToListAsync();

            return entities.Select(ToDTO);
        } 

        protected async Task<TEntityDTO> FindOneWith(
            Func<Task<bool>> action = null, 
            Func<IQueryable<TEntity>> queryFunction = null,
            params Expression<Func<TEntity, bool>>[] conditions
        )
        {
            if (action != null)
            {
                Logger?.Log(
                    LogLevel.Information,
                    $"Acción: {{{nameof(CheckExistence)}}}, validando con pre-acción."
                );

                if (!await action?.Invoke())
                    return default;
            }

            if (conditions is null)
            {
                throw new ArgumentNullException(QuickerExceptionConstants.Conditions);
            }

            var query = queryFunction?.Invoke() ?? Query();

            foreach (var condition in conditions)
            {
                query = query.Where(condition);
            }

            var entities = await query.SingleOrDefaultAsync();

            return ToDTO(entities);
        }

        #endregion

        #region Public methods

        public virtual async Task<bool> CheckExistence(
            TKey key, 
            Func<Task<bool>> action = null
        )
        {
            if (action != null)
            {
                Logger?.Log(
                    LogLevel.Information,
                    $"Acción: {{{nameof(CheckExistence)}}}, validando con pre-acción."
                );

                if (!await action?.Invoke())
                    return default;
            }

            Logger?.Log(
                LogLevel.Information,
                $"Acción: {{{nameof(CheckExistence)}}}, leyendo los registros de tipo {{{typeof(TEntity).Name}}}, " +
                $"para buscar un elemento de {{ID}} {key}."
            );

            if (key == null)
            {
                Logger?.Log(
                    LogLevel.Warning,
                    $"Acción: {{{nameof(CheckExistence)}}}, no ha sido posible buscar, ya que la key es nula."
                );

                throw new ArgumentNullException(QuickerExceptionConstants.Key);
            }

            var query = Query();

            var exists = await ReadFilter(query)
                .Where(e => e.Id.Equals(key))
                .AnyAsync(e => e.Id.Equals(key));

            if (exists)
            {
                Logger?.Log(
                    LogLevel.Information,
                    $"Acción: {{{nameof(CheckExistence)}}}, se ha encontrado el elemento."
                );
            }
            else
            {
                Logger?.Log(
                    LogLevel.Warning,
                    $"Acción: {{{nameof(CheckExistence)}}}, no se ha encontrado el elemento."
                );
            }

            return exists;
        }

        public virtual async Task<bool> CheckExistence(
            Func<Task<bool>> action = null, 
            params Expression<Func<TEntity, bool>>[] conditions
        )
        {
            if (action != null)
            {
                Logger?.Log(
                    LogLevel.Information,
                    $"Acción: {{{nameof(CheckExistence)}}}, validando con pre-acción."
                );

                if (!await action?.Invoke())
                    return default;
            }

            if (conditions is null)
            {
                throw new ArgumentNullException(nameof(conditions));
            }

            Logger?.Log(
                LogLevel.Information,
                $"Acción: {{{nameof(CheckExistence)}}}, leyendo los registros de tipo {{{typeof(TEntity).Name}}}, " +
                $"para buscar un elemento con filtros especificos."
            );

            var query = Query();

            foreach (var condition in conditions)
            {
                query = query.Where(condition);
            }

            var exists = await query.AnyAsync();

            if (exists)
            {
                Logger?.Log(
                    LogLevel.Information,
                    $"Acción: {{{nameof(CheckExistence)}}}, se ha encontrado el elemento."
                );
            }
            else
            {
                Logger?.Log(
                    LogLevel.Warning,
                    $"Acción: {{{nameof(CheckExistence)}}}, no se ha encontrado el elemento."
                );
            }

            return exists;
        }

        public virtual async Task<IEnumerable<TEntityDTO>> Read(Func<Task<bool>> action = null)
        {
            if (action != null)
            {
                Logger?.Log(
                    LogLevel.Information,
                    $"Acción: {{{nameof(Read)}}}, validando con preacción."
                );

                if (!await action?.Invoke())
                    return default;
            }

            Logger?.Log(
                LogLevel.Information,
                $"Acción: {{{nameof(Read)}}}, leyendo los registros de tipo {{{typeof(TEntity).Name}}}"
            );

            var query = Query();

            var entities = await ReadFilter(query)
                .ToListAsync();

            return entities.Select(ToDTO);
        }

        public virtual async Task<TEntityDTO> Read(TKey key, Func<Task<bool>> action = null)
        {
            if (action != null)
            {
                Logger?.Log(
                    LogLevel.Information,
                    $"Acción: {{{nameof(Read)}}}, validando con pre-acción."
                );

                if (!await action?.Invoke())
                    return default;
            }

            Logger?.Log(
                LogLevel.Information,
                $"Acción: {{{nameof(Read)}}}, leyendo los registros de tipo {{{typeof(TEntity).Name}}}, " +
                $"para buscar a la entidad de {{Id}}: {key}."
            );

            if (key == null)
            {
                Logger?.Log(
                    LogLevel.Warning,
                    $"Acción: {{{nameof(Read)}}}, no ha sido posible buscar, ya que la key es nula."
                );

                throw new ArgumentNullException(QuickerExceptionConstants.Key);
            }

            var query = Query();

            var entity = await ReadFilter(query)
                .Where(e => e.Id.Equals(key))
                .SingleOrDefaultAsync();

            if (entity != null)
            {
                Logger?.Log(
                    LogLevel.Information,
                    $"Acción: {{{nameof(Read)}}}, se ha encontrado el elemento de {{Id}} {key}."
                );

                Context.Entry(entity).State = EntityState.Detached;
            }
            else
            {
                Logger?.Log(
                    LogLevel.Warning,
                    $"Acción: {{{nameof(Read)}}}, no se ha encontrado el elemento de {{Id}} {key}."
                );
            }

            return ToDTO(entity);
        }

        public virtual async Task<IEnumerable<TEntityDTO>> Paginate(
            int page, int number = 10, 
            Func<Task<bool>> action = null
        )
        {
            if (action != null)
            {
                Logger?.Log(
                    LogLevel.Information,
                    $"Acción: {{{nameof(Paginate)}}}, validando con pre-acción."
                );

                if (!await action?.Invoke())
                    return default;
            }

            if (number < 1)
            {
                throw new ArgumentException(QuickerExceptionConstants.Number);
            }

            if (page < 0)
            {
                throw new ArgumentException(QuickerExceptionConstants.Page);
            }

            Logger?.Log(
                LogLevel.Information,
                $"Acción: {{{nameof(Paginate)}}}, leyendo los registros de tipo {{{typeof(TEntity).Name}}}, " +
                $"para saltar {page * number} y tomar {number}."
            );

            var query = Query();

            var entities = await ReadFilter(query)
                .Skip(page * number)
                .Take(number)
                .ToListAsync();

            if (entities.Count != number)
            {
                Logger?.Log(LogLevel.Information,
                    $"Acción: {{{nameof(Paginate)}}}, se tomaron solo {number - entities.Count}, " +
                    $"ya que no habian {number}."
                );
            }
            else if (entities.Count == 0)
            {
                Logger?.Log(LogLevel.Warning,
                    $"Acción: {{{nameof(Paginate)}}}, no se ha tomado ningun elemento."
                );
            }

            return entities.Select(ToDTO);
        }

        #endregion
    }
}
