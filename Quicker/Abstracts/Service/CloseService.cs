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
    public class CloseService<TKey, TEntity> : BaseService<TKey, TEntity>, ICloseServiceAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        public CloseService(DbContext context) : 
            base(context) { }

        public async Task<IEnumerable<TEntity>> FindManyByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions)
        {
            var query = QueryAll();

            foreach (var expression in expressions)
                query = query.Where(expression);

            var entities = await query.ToListAsync();

            return entities;
        }

        public async Task<IEnumerable<TEntity>> FindManyByCondition(Expression<Func<TEntity, bool>> expression)
        {
            var entities = await QueryAll()
                .Where(expression)
                .ToListAsync();

            return entities;
        }

        public async Task<TEntity> FindOneByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions)
        {
            var query = QueryAll();

            foreach (var expression in expressions)
                query = query.Where(expression);

            var entity = await query.SingleOrDefaultAsync();

            return entity;
        }

        public async Task<TEntity> FindOneByCondition(Expression<Func<TEntity, bool>> expression)
        {
            var entity = await QueryAll()
                .Where(expression)
                .SingleOrDefaultAsync();

            return entity;
        }


        public async Task<TEntity> FindFirstByCondition(Expression<Func<TEntity, bool>> expression)
        {
            var entity = await QueryAll()
                .Where(expression)
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task<TEntity> FindFirstByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions)
        {
            var query = QueryAll();

            foreach (var expression in expressions)
                query = query.Where(expression);

            var entity = await query.FirstOrDefaultAsync();

            return entity;
        }
        public async Task<IEnumerable<TEntity>> Paginate(int number, int page)
        {
            var entity = await QueryAll()
                .Skip((page - 1) * number)
                .Take(number)
                .ToListAsync();

            return entity;
        }

        public async Task<IEnumerable<TEntity>> Read()
        {
            var entities = await QueryAll()
                .ToListAsync();

            return entities;
        }

        public async Task<TEntity> Read(TKey key)
        {
            var entity = await QuerySingle(key);

            Context.Entry(entity).State = EntityState.Detached;

            return entity;
        }
    }

    public class CloseService<TKey, TEntity, TEntityDTO> : BaseService<TKey, TEntity, TEntityDTO>, ICloseServiceAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        public CloseService(DbContext context, IMapper mapper) :
            base(context, mapper)
        { }

        public async Task<IEnumerable<TEntityDTO>> FindManyByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions)
        {
            var query = QueryAll();

            foreach (var expression in expressions)
                query = query.Where(expression);

            var entities = (await query.ToListAsync())
                .Select(e => ToDTO(e));

            return entities;
        }

        public async Task<IEnumerable<TEntityDTO>> FindManyByCondition(Expression<Func<TEntity, bool>> expression)
        {
            var entities = (await QueryAll()
                .Where(expression)
                .ToListAsync())
                .Select(e => ToDTO(e));

            return entities;
        }

        public async Task<TEntityDTO> FindOneByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions)
        {
            var query = QueryAll();

            foreach (var expression in expressions)
                query = query.Where(expression);

            var entity = await query.SingleOrDefaultAsync();
            var dto = ToDTO(entity);

            return dto;
        }

        public async Task<TEntityDTO> FindOneByCondition(Expression<Func<TEntity, bool>> expression)
        {
            var entity = await QueryAll()
                .Where(expression)
                .SingleOrDefaultAsync();

            var dto = ToDTO(entity);

            return dto;
        }


        public async Task<TEntityDTO> FindFirstByCondition(Expression<Func<TEntity, bool>> expression)
        {
            var entity = await QueryAll()
                .Where(expression)
                .FirstOrDefaultAsync();

            var dto = ToDTO(entity);

            return dto;
        }

        public async Task<TEntityDTO> FindFirstByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions)
        {
            var query = QueryAll();

            foreach (var expression in expressions)
                query = query.Where(expression);

            var entity = await query.FirstOrDefaultAsync();

            var dto = ToDTO(entity);

            return dto;
        }
        public async Task<IEnumerable<TEntityDTO>> Paginate(int number, int page)
        {
            var entity = (await QueryAll()
                .Skip((page - 1) * number)
                .Take(number)
                .ToListAsync())
                .Select(e => ToDTO(e));

            return entity;
        }

        public async Task<IEnumerable<TEntityDTO>> Read()
        {
            var entities = (await QueryAll()
                .ToListAsync())
                .Select(e => ToDTO(e));

            return entities;
        }

        public async Task<TEntityDTO> Read(TKey key)
        {
            var entity = await QuerySingle(key);
            var dto = ToDTO(entity);

            Context.Entry(entity).State = EntityState.Detached;

            return dto;
        }
    }
}
