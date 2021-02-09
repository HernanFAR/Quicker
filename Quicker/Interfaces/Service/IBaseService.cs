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
    /// <para>Interface to provide functions to get All or One Objects in DB, in classes who doesn't have a DTO related entity</para>
    /// <para>The utility of this function is centralice all object-retriving code that get objects of DB in two functions.</para>
    /// </summary>
    public interface IBaseService<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        /// <summary>
        /// <para>Method to provide the function to get ALL Objects in DB.</para>
        /// <para>The utility of this is centralice <em>all</em> object-retriving code that get ALL objects in DB, in one function (Not counting specific cases), acting like a <em>generic</em>.</para>
        /// </summary>
        IQueryable<TEntity> QueryAll();

        /// <summary>
        /// <para>Method to provide the function to get ONE Objects in DB.</para>
        /// <para>The utility of this is centralice <em>all</em> object-retriving code that get ONE object in DB, in one function (Not counting specific cases), acting like a <em>generic</em>.</para>
        /// <para><paramref name="key"/>: Primary key value to find the element in DB </para>
        /// </summary>
        Task<TEntity> QuerySingle(TKey key);
    }

    /// <summary>
    /// <para>Interface to provide functions to get All or One Objects in DB, in classes who have a DTO related entity</para>
    /// <para>The utility of this function is centralice all object-retriving code that get objects of DB in two functions.</para>
    /// </summary>
    public interface IBaseService<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        /// <summary>
        /// <para>Method to provide the function to get ALL Objects in DB.</para>
        /// <para>The utility of this is centralice <em>all</em> object-retriving code that get ALL objects in DB, in one function (Not counting specific cases), acting like a <em>generic</em>.</para>
        /// </summary>
        IQueryable<TEntity> QueryAll();

        /// <summary>
        /// <para>Method to provide the function to get ONE Objects in DB.</para>
        /// <para>The utility of this is centralice <em>all</em> object-retriving code that get ONE object in DB, in one function (Not counting specific cases), acting like a <em>generic</em>.</para>
        /// <para><paramref name="key"/>: Primary key value to find the element in DB </para>
        /// </summary>
        Task<TEntity> QuerySingle(TKey key);

        /// <summary>
        /// <para>Method to transform a class Domain instance to a DTO one.</para>
        /// <para>Counterpart of: <seealso cref="IBaseService{TKey, TEntity, TEntityDTO}.ToDomain(TEntityDTO)"/></para>
        /// <para><paramref name="entity"/>: Instance to clone, and convert to related DTO </para>
        /// </summary>
        TEntityDTO ToDTO(TEntity entity);

        /// <summary>
        /// <para>Method to transform a class DTO instance to a Domain one.</para>
        /// <para>Counterpart of: <seealso cref="IBaseService{TKey, TEntity, TEntityDTO}.ToDTO(TEntity)"/></para>
        /// <para><paramref name="entity"/>: Instance to clone, and convert to related Domain </para>
        /// </summary>
        TEntity ToDomain(TEntityDTO entity);
    }
}
