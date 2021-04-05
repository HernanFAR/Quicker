﻿using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quicker.Interfaces.Component
{
    public interface IReadComponent<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        Task<IEnumerable<TEntity>> Read(Func<Task<bool>> action = null);

        Task<IEnumerable<TEntity>> Paginate(int page, int number = 10, Func<Task<bool>> action = null);

        Task<TEntity> Read(TKey key, Func<Task<bool>> action = null);

        Task<bool> CheckExistence(TKey key, Func<Task<bool>> action = null);

        Task<bool> CheckExistence(Func<Task<bool>> action = null, params Expression<Func<TEntity, bool>>[] conditions);
    }
}
