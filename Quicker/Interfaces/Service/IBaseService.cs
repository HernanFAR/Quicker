using AutoMapper;
using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicker.Interfaces.Service
{
    /// <summary>
    ///     Interface to provide functions to get All or One Objects in DB, in classes who doesn't have a DTO related entity
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The utility of this interface is centralice all object-retriving code that get objects of DB in two functions.    
    ///     </para>
    /// </remarks>
    /// 
    public interface IBaseService<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        /// <summary>
        ///     Method to provide the function to get ALL Objects in DB.
        /// </summary>
        /// 
        IQueryable<TEntity> QueryAll();

        /// <summary>
        ///     Method to provide the function to get ONE Objects in DB.
        /// </summary>
        /// 
        Task<TEntity> QuerySingle(TKey key);
    }

    /// <summary>
    ///     Interface to provide functions to get All or One Objects in DB, in classes who have a DTO related entity.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The utility of this interface is centralice all object-retriving code that get objects of DB in two functions.    
    ///     </para>
    /// </remarks>
    /// 
    public interface IBaseService<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        /// <summary>
        ///     Method to provide the function to get ALL Objects in DB.
        /// </summary>
        /// 
        IQueryable<TEntity> QueryAll();

        /// <summary>
        ///     Method to provide the function to get ONE Objects in DB.
        /// </summary>
        /// 
        Task<TEntity> QuerySingle(TKey key);

        /// <summary>
        ///     Method to transform a class Domain instance to a DTO one.
        /// </summary>
        /// <param name="entity">Instance to clone, and convert to related DTO</param> 
        /// 
        TEntityDTO ToDTO(TEntity entity);

        /// <summary>
        ///     Method to transform a class DTO instance to a Domain one.
        /// </summary>
        /// <param name="entity">Instance to clone, and convert to related Domain</param> 
        /// 
        TEntity ToDomain(TEntityDTO entity);
    }
}
