using Microsoft.EntityFrameworkCore;
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
    public class ReadComponent<TKey, TEntity> : IReadComponent<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
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

        protected async Task ExecutingPreConditionAsync(Func<Task<bool>> condition, string actionName)
        {
            if (condition != null)
            {
                Logger?.Log(
                    LogLevel.Information,
                    $"Acción: {{{actionName}}}, validando con pre-acción."
                );

                if (!await condition?.Invoke())
                    throw new InvalidOperationException(QuickerExceptionConstants.Precondition);
            }
        }

        protected virtual IQueryable<TEntity> Query()
            => Context.Set<TEntity>()
                .OrderBy(e => e.Id)
                .AsNoTracking();

        protected virtual IQueryable<TEntity> ReadFilter(IQueryable<TEntity> entities) {
            if (entities is null)
            {
                throw new ArgumentNullException(QuickerExceptionConstants.Entities);
            }

            return entities;
        }

        protected virtual async Task<IEnumerable<TEntity>> FindManyWithAsync(
            Func<Task<bool>> conditionFunc = null, 
            Func<IQueryable<TEntity>> queryFunction = null,
            params Expression<Func<TEntity, bool>>[] conditions
        )
        {
            await ExecutingPreConditionAsync(conditionFunc, nameof(FindManyWithAsync));

            if (conditions is null)
            {
                throw new ArgumentNullException(QuickerExceptionConstants.Conditions);
            }

            Logger?.Log(
                LogLevel.Information,
                $"Acción: {{{nameof(FindManyWithAsync)}}}, leyendo los registros de tipo {{{typeof(TEntity).Name}}}, " +
                $"para buscar un elemento con condiciones especificas {(queryFunction == null ? "Funcion custom" : "Funcion Query")}."
            );

            var query = queryFunction?.Invoke() ?? Query();

            foreach (var condition in conditions)
            {
                query = query.Where(condition);
            }

            var entities = await query.ToListAsync();

            Logger?.Log(
                LogLevel.Information,
                $"Acción: {{{nameof(FindManyWithAsync)}}}, encontrados {{{entities.Count}}} elementos."
            );

            return entities;
        } 

        protected virtual async Task<TEntity> FindOneWithAsync(
            Func<Task<bool>> conditionFunc = null, 
            Func<IQueryable<TEntity>> queryFunction = null,
            params Expression<Func<TEntity, bool>>[] conditions
        )
        {
            await ExecutingPreConditionAsync(conditionFunc, nameof(FindOneWithAsync));

            if (conditions is null)
            {
                throw new ArgumentNullException(QuickerExceptionConstants.Conditions);
            }

            Logger?.Log(
                LogLevel.Information,
                $"Acción: {{{nameof(FindOneWithAsync)}}}, leyendo los registros de tipo {{{typeof(TEntity).Name}}}, " +
                $"para buscar un elemento con condiciones especificas {(queryFunction == null ? "Funcion custom" : "Funcion Query")}."
            );

            var query = queryFunction?.Invoke() ?? Query();

            foreach (var condition in conditions)
            {
                query = query.Where(condition);
            }

            var entity = await query.SingleOrDefaultAsync();

            if (entity != null)
            {
                Logger?.Log(
                    LogLevel.Information,
                    $"Acción: {{{nameof(FindOneWithAsync)}}}, se ha encontrado un elemento."
                );
            }
            else
            {
                Logger?.Log(
                    LogLevel.Information,
                    $"Acción: {{{nameof(FindOneWithAsync)}}}, no se ha encontrado un elemento."
                );
            }

            return entity;
        }

        #endregion

        #region Public methods

        public virtual async Task<bool> CheckExistenceAsync(
            TKey key, 
            Func<Task<bool>> conditionFunc = null
        )
        {
            await ExecutingPreConditionAsync(conditionFunc, nameof(CheckExistenceAsync));

            Logger?.Log(
                LogLevel.Information,
                $"Acción: {{{nameof(CheckExistenceAsync)}}}, leyendo los registros de tipo {{{typeof(TEntity).Name}}}, " +
                $"para buscar un elemento de {{ID}} {key}."
            );

            if (key == null)
            {
                Logger?.Log(
                    LogLevel.Warning,
                    $"Acción: {{{nameof(CheckExistenceAsync)}}}, no ha sido posible buscar, ya que la key es nula."
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
                    $"Acción: {{{nameof(CheckExistenceAsync)}}}, se ha encontrado el elemento."
                );
            }
            else
            {
                Logger?.Log(
                    LogLevel.Warning,
                    $"Acción: {{{nameof(CheckExistenceAsync)}}}, no se ha encontrado el elemento."
                );
            }

            return exists;
        }

        public virtual async Task<bool> CheckExistenceAsync(
            Func<Task<bool>> conditionFunc = null, 
            params Expression<Func<TEntity, bool>>[] conditions
        )
        {
            await ExecutingPreConditionAsync(conditionFunc, nameof(CheckExistenceAsync));

            if (conditions is null)
            {
                throw new ArgumentNullException(QuickerExceptionConstants.Conditions);
            }

            Logger?.Log(
                LogLevel.Information,
                $"Acción: {{{nameof(CheckExistenceAsync)}}}, leyendo los registros de tipo {{{typeof(TEntity).Name}}}, " +
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
                    $"Acción: {{{nameof(CheckExistenceAsync)}}}, se ha encontrado el elemento."
                );
            }
            else
            {
                Logger?.Log(
                    LogLevel.Warning,
                    $"Acción: {{{nameof(CheckExistenceAsync)}}}, no se ha encontrado el elemento."
                );
            }

            return exists;
        }

        public virtual async Task<IEnumerable<TEntity>> ReadAsync(Func<Task<bool>> conditionFunc = null)
        {
            await ExecutingPreConditionAsync(conditionFunc, nameof(ReadAsync));

            Logger?.Log(
                LogLevel.Information,
                $"Acción: {{{nameof(ReadAsync)}}}, leyendo los registros de tipo {{{typeof(TEntity).Name}}}"
            );

            var query = Query();

            var entities = await ReadFilter(query)
                .ToListAsync();

            Logger?.Log(
                LogLevel.Information,
                $"Acción: {{{nameof(ReadAsync)}}}, se han leido {{{entities.Count}}} registros."
            );

            return entities;
        }

        public virtual async Task<TEntity> ReadAsync(TKey key, Func<Task<bool>> conditionFunc = null)
        {
            await ExecutingPreConditionAsync(conditionFunc, nameof(ReadAsync));

            Logger?.Log(
                LogLevel.Information,
                $"Acción: {{{nameof(ReadAsync)}}}, leyendo los registros de tipo {{{typeof(TEntity).Name}}}, " +
                $"para buscar a la entidad de {{Id}}: {key}."
            );

            if (key == null)
            {
                Logger?.Log(
                    LogLevel.Warning,
                    $"Acción: {{{nameof(ReadAsync)}}}, no ha sido posible buscar, ya que la key es nula."
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
                    $"Acción: {{{nameof(ReadAsync)}}}, se ha encontrado el elemento de {{Id}} {key}."
                );

                Context.Entry(entity).State = EntityState.Detached;
            }
            else
            {
                Logger?.Log(
                    LogLevel.Warning,
                    $"Acción: {{{nameof(ReadAsync)}}}, no se ha encontrado el elemento de {{Id}} {key}."
                );
            }

            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> PaginateAsync(
            int page, int number = 10, 
            Func<Task<bool>> conditionFunc = null
        )
        {
            await ExecutingPreConditionAsync(conditionFunc, nameof(PaginateAsync));

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
                $"Acción: {{{nameof(PaginateAsync)}}}, leyendo los registros de tipo {{{typeof(TEntity).Name}}}, " +
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
                    $"Acción: {{{nameof(PaginateAsync)}}}, se tomaron solo {number - entities.Count}, " +
                    $"ya que no habian {number}."
                );
            }
            else if (entities.Count == 0)
            {
                Logger?.Log(LogLevel.Warning,
                    $"Acción: {{{nameof(PaginateAsync)}}}, no se ha tomado ningun elemento."
                );
            }

            return entities;
        }

        #endregion
    }
}
