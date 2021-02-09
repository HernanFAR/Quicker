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
    /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity}"/>.</para>
    /// </summary>
    public class CloseService<TKey, TEntity> : BaseService<TKey, TEntity>, ICloseServiceAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        public CloseService(DbContext context) : 
            base(context) { }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity}.FindManyByConditions(IEnumerable{Expression{Func{TEntity, bool}}})"/>.</para>
        /// </summary>
        public async Task<IEnumerable<TEntity>> FindManyByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions)
        {
            var query = QueryAll();

            foreach (var expression in expressions)
                query = query.Where(expression);

            var entities = await query.ToListAsync();

            return entities;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity}.FindManyByCondition(Expression{Func{TEntity, bool}})"/>.</para>
        /// </summary>
        public async Task<IEnumerable<TEntity>> FindManyByCondition(Expression<Func<TEntity, bool>> expression)
        {
            var entities = await QueryAll()
                .Where(expression)
                .ToListAsync();

            return entities;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity}.FindOneByConditions(IEnumerable{Expression{Func{TEntity, bool}}}))"/>.</para>
        /// </summary>
        public async Task<TEntity> FindOneByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions)
        {
            var query = QueryAll();

            foreach (var expression in expressions)
                query = query.Where(expression);

            var entity = await query.SingleOrDefaultAsync();

            return entity;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity}.FindOneByCondition(Expression{Func{TEntity, bool}})"/>.</para>
        /// </summary>
        public async Task<TEntity> FindOneByCondition(Expression<Func<TEntity, bool>> expression)
        {
            var entity = await QueryAll()
                .Where(expression)
                .SingleOrDefaultAsync();

            return entity;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity}.FindFirstByCondition(Expression{Func{TEntity, bool}})"/>.</para>
        /// </summary>
        public async Task<TEntity> FindFirstByCondition(Expression<Func<TEntity, bool>> expression)
        {
            var entity = await QueryAll()
                .Where(expression)
                .FirstOrDefaultAsync();

            return entity;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity}.FindFirstByConditions(IEnumerable{Expression{Func{TEntity, bool}}}))"/>.</para>
        /// </summary>
        public async Task<TEntity> FindFirstByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions)
        {
            var query = QueryAll();

            foreach (var expression in expressions)
                query = query.Where(expression);

            var entity = await query.FirstOrDefaultAsync();

            return entity;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity}.Paginate(int, int))"/>.</para>
        /// </summary>
        public async Task<IEnumerable<TEntity>> Paginate(int number, int page)
        {
            var entity = await QueryAll()
                .Skip((page - 1) * number)
                .Take(number)
                .ToListAsync();

            return entity;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity}.Read"/>.</para>
        /// </summary>
        public async Task<IEnumerable<TEntity>> Read()
        {
            var entities = await QueryAll()
                .ToListAsync();

            return entities;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity}.Read(TKey)"/>.</para>
        /// </summary>
        public async Task<TEntity> Read(TKey key)
        {
            var entity = await QuerySingle(key);

            Context.Entry(entity).State = EntityState.Detached;

            return entity;
        }
    }

    /// <summary>
    /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity}"/>.</para>
    /// </summary>
    public class CloseService<TKey, TEntity, TEntityDTO> : BaseService<TKey, TEntity, TEntityDTO>, ICloseServiceAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        public CloseService(DbContext context, IMapper mapper) :
            base(context, mapper)
        { }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity, TEntityDTO}.FindManyByConditions(IEnumerable{Expression{Func{TEntity, bool}}})"/>.</para>
        /// </summary>
        public async Task<IEnumerable<TEntityDTO>> FindManyByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions)
        {
            var query = QueryAll();

            foreach (var expression in expressions)
                query = query.Where(expression);

            var entities = (await query.ToListAsync())
                .Select(e => ToDTO(e));

            return entities;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity, TEntityDTO}.FindManyByCondition(Expression{Func{TEntity, bool}})"/>.</para>
        /// </summary>
        public async Task<IEnumerable<TEntityDTO>> FindManyByCondition(Expression<Func<TEntity, bool>> expression)
        {
            var entities = (await QueryAll()
                .Where(expression)
                .ToListAsync())
                .Select(e => ToDTO(e));

            return entities;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity, TEntityDTO}.FindOneByConditions(IEnumerable{Expression{Func{TEntity, bool}}}))"/>.</para>
        /// </summary>
        public async Task<TEntityDTO> FindOneByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions)
        {
            var query = QueryAll();

            foreach (var expression in expressions)
                query = query.Where(expression);

            var entity = await query.SingleOrDefaultAsync();
            var dto = ToDTO(entity);

            return dto;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity, TEntityDTO}.FindOneByCondition(Expression{Func{TEntity, bool}})"/>.</para>
        /// </summary>
        public async Task<TEntityDTO> FindOneByCondition(Expression<Func<TEntity, bool>> expression)
        {
            var entity = await QueryAll()
                .Where(expression)
                .SingleOrDefaultAsync();

            var dto = ToDTO(entity);

            return dto;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity, TEntityDTO}.FindFirstByCondition(Expression{Func{TEntity, bool}})"/>.</para>
        /// </summary>
        public async Task<TEntityDTO> FindFirstByCondition(Expression<Func<TEntity, bool>> expression)
        {
            var entity = await QueryAll()
                .Where(expression)
                .FirstOrDefaultAsync();

            var dto = ToDTO(entity);

            return dto;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity, TEntityDTO}.FindFirstByConditions(IEnumerable{Expression{Func{TEntity, bool}}}))"/>.</para>
        /// </summary>
        public async Task<TEntityDTO> FindFirstByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions)
        {
            var query = QueryAll();

            foreach (var expression in expressions)
                query = query.Where(expression);

            var entity = await query.FirstOrDefaultAsync();

            var dto = ToDTO(entity);

            return dto;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity, TEntityDTO}.Paginate(int, int))"/>.</para>
        /// </summary>
        public async Task<IEnumerable<TEntityDTO>> Paginate(int number, int page)
        {
            var entity = (await QueryAll()
                .Skip((page - 1) * number)
                .Take(number)
                .ToListAsync())
                .Select(e => ToDTO(e));

            return entity;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity, TEntityDTO}.Read"/>.</para>
        /// </summary>
        public async Task<IEnumerable<TEntityDTO>> Read()
        {
            var entities = (await QueryAll()
                .ToListAsync())
                .Select(e => ToDTO(e));

            return entities;
        }

        /// <summary>
        /// <para>Main implementation of <seealso cref="ICloseServiceAsync{TKey, TEntity, TEntityDTO}.Read(TKey)"/>.</para>
        /// </summary>
        public async Task<TEntityDTO> Read(TKey key)
        {
            var entity = await QuerySingle(key);
            var dto = ToDTO(entity);

            Context.Entry(entity).State = EntityState.Detached;

            return dto;
        }
    }
}
