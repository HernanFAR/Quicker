using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Quicker.Interfaces.Service
{
    /// <summary>
    ///     Interface to specify a close service, with entities that doesn't have a DTO related entity.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A close service consist in functions to ONLY READ the elements in database, so it brings the R in CRUD.
    ///     </para>
    /// </remarks>
    /// 
    public interface ICloseServiceAsync<TKey, TEntity> : IBaseService<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        /// <summary>
        ///     Reads ALL elements in database.
        /// </summary> 
        /// <returns>
        ///     A <see cref="IEnumerable{T}"/> with of type <typeparamref cref="TEntity"/>
        /// </returns>
        Task<IEnumerable<TEntity>> Read();

        /// <summary>
        ///     Read ONE element in database of type <typeparamref cref="TEntity"/>.
        /// </summary> 
        /// <returns>
        ///     The entity related to the primary key
        /// </returns>
        /// <param name="key">Primary key value to find the element in DB</param>
        Task<TEntity> Read(TKey key);

        /// <summary>
        ///     Reads paginated elements in database.
        /// </summary> 
        /// <returns>
        ///     A <see cref="IEnumerable{T}"/> with of type <typeparamref name="TEntity"/> with large of <paramref name="number"/>
        /// </returns>
        /// <param name="number">Ammount of elements to take</param>
        /// <param name="page">Page of paginated elements</param>
        Task<IEnumerable<TEntity>> Paginate(int number, int page);
    }

    /// <summary>.
    /// <para>Interface to specify a close service, with entities that have a DTO related entity.</para>
    /// <para>A close service consist in functions to ONLY READ the elements in database </para>
    /// </summary> 
    public interface ICloseServiceAsync<TKey, TEntity, TEntityDTO> : IBaseService<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        /// <summary>.
        /// <para>Reads ALL elements in database.</para>
        /// </summary> 
        Task<IEnumerable<TEntityDTO>> Read();

        /// <summary>.
        /// <para>Reads ONE elements in database.</para>
        /// <para><paramref name="key"/>: Primary key value to find the element in DB </para>
        /// </summary> 
        Task<TEntityDTO> Read(TKey key);

        /// <summary>.
        /// <para>Reads paginated elements in database.</para>
        /// <list type="bullet">
        /// <item><paramref name="number"/>: <description>Ammount of elements to take</description></item>
        /// <item><paramref name="page"/>: <description>Page of paginated elements</description></item>
        /// </list>
        /// </summary> 
        Task<IEnumerable<TEntityDTO>> Paginate(int number, int page);
    }
}
