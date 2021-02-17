﻿using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quicker.Interfaces.Service
{
    public interface IFullServiceAsync<TKey, TEntity> : IOpenServiceAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        Task<TEntity> Update(TKey key, TEntity entity);
    }

    public interface IFullServiceAsync<TKey, TEntity, TEntityDTO> : IOpenServiceAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        Task<TEntityDTO> Update(TKey key, TEntityDTO entity);
    }
}
