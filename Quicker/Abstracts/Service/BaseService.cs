using Microsoft.EntityFrameworkCore;
using Quicker.Interfaces.Model;
using Quicker.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicker.Abstracts.Service
{
    public abstract class BaseService<TKey, TEntity> : IBaseService<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        protected DbContext Context { get; set; }

        public BaseService(DbContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> QueryAll()
        {
            var entities = Context
                .Set<TEntity>()
                .AsNoTracking();

            return entities;
        }

        public async Task<TEntity> QuerySingle(TKey key)
        {
            var entity = await Context
                .Set<TEntity>()
                .FindAsync(key);

            return entity;
        }
    }
}
