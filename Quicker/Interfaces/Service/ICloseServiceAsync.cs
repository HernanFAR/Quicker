using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Quicker.Interfaces.Service
{
    /// <summary>.
    /// <para>Interface to specify a close service, with entities that doesn't have a DTO related entity.</para>
    /// <para>A close service consist in functions to ONLY READ the elements in database </para>
    /// </summary> 
    public interface ICloseServiceAsync<TKey, TEntity> : IBaseService<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        /// <summary>.
        /// <para>Reads ALL elements in database.</para>
        /// </summary> 
        Task<IEnumerable<TEntity>> Read();

        /// <summary>.
        /// <para>Reads ONE elements in database.</para>
        /// <para><paramref name="key"/>: Primary key value to find the element in DB </para>
        /// </summary> 
        Task<TEntity> Read(TKey key);

        /// <summary>.
        /// <para>Reads paginated elements in database.</para>
        /// <list type="bullet">
        /// <item><paramref name="number"/>: <description>Ammount of elements to take</description></item>
        /// <item><paramref name="page"/>: <description>Page of paginated elements</description></item>
        /// </list>
        /// </summary> 
        Task<IEnumerable<TEntity>> Paginate(int number, int page);

        /// <summary>.
        /// <para>Find MANY elements with ONE condition.</para>
        /// <para><paramref name="expression"/>: Condition to filter the full list</para>
        /// <para><em>Usefull if you need to extend the  interface.</em></para>
        /// </summary> 
        Task<IEnumerable<TEntity>> FindManyByCondition(Expression<Func<TEntity, bool>> expression);

        /// <summary>.
        /// <para>Find MANY elements with MANY conditions.</para>
        /// <para><paramref name="expressions"/>: Conditions to filter the full list</para>
        /// <para><em>Usefull if you need to extend the interface.</em></para>
        /// </summary> 
        Task<IEnumerable<TEntity>> FindManyByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions);

        /// <summary>.
        /// <para>Find ONE element with ONE condition.</para>
        /// <para>If it finds more than one element with the specified condition, throws exception. If you only need the <em>first</em> element of a given condition, use <seealso cref="ICloseServiceAsync{TKey, TEntity}.FindFirstByCondition(Expression{Func{TEntity, bool}})"/></para>
        /// <para><paramref name="expression"/>: Condition to filter the full list</para>
        /// <para><em>Usefull if you need to extend the interface.</em></para>
        /// </summary> 
        Task<TEntity> FindOneByCondition(Expression<Func<TEntity, bool>> expression);

        /// <summary>.
        /// <para>Find ONE element with MANY conditions.</para>
        /// <para>If it finds more than one element with the specified condition, throws exception. If you only need the <em>first</em> element of a given condition, use <seealso cref="ICloseServiceAsync{TKey, TEntity}.FindFirstByConditions(IEnumerable{Expression{Func{TEntity, bool}}})"/></para>
        /// <para><paramref name="expressions"/>: Conditions to filter the full list</para>
        /// <para><em>Usefull if you need to extend the interface.</em></para>
        /// </summary>
        Task<TEntity> FindOneByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions);

        /// <summary>.
        /// <para>Find the FIRST element with ONE condition.</para>
        /// <para>If it finds more than one element with the specified condition, gives only the first. If you conditions always should bring one element, use <seealso cref="ICloseServiceAsync{TKey, TEntity}.FindOneByCondition(Expression{Func{TEntity, bool}})"/></para>
        /// <para><paramref name="expression"/>: Condition to filter the full list</para>
        /// <para><em>Usefull if you need to extend the interface.</em></para>
        /// </summary> 
        Task<TEntity> FindFirstByCondition(Expression<Func<TEntity, bool>> expression);

        /// <summary>.
        /// <para>Find the FIRST element with MANY conditions.</para>
        /// <para>If it finds more than one element with the specified condition, gives only the first. If you conditions always should bring one element, use <seealso cref="ICloseServiceAsync{TKey, TEntity}.FindOneByConditions(IEnumerable{Expression{Func{TEntity, bool}}})"/></para>
        /// <para><paramref name="expressions"/>: Conditions to filter the full list</para>
        /// <para><em>Usefull if you need to extend the interface.</em></para>
        /// </summary> 

        Task<TEntity> FindFirstByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions);
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

        /// <summary>.
        /// <para>Find MANY elements with ONE condition.</para>
        /// <para><paramref name="expression"/>: Condition to filter the full list</para>
        /// <para><em>Usefull if you need to extend the interface.</em></para>
        /// </summary> 
        Task<IEnumerable<TEntityDTO>> FindManyByCondition(Expression<Func<TEntity, bool>> expression);

        /// <summary>.
        /// <para>Find MANY elements with MANY conditions.</para>
        /// <para><paramref name="expressions"/>: Conditions to filter the full list</para>
        /// <para><em>Usefull if you need to extend the interface.</em></para>
        /// </summary> 
        Task<IEnumerable<TEntityDTO>> FindManyByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions);

        /// <summary>.
        /// <para>Find ONE element with ONE condition.</para>
        /// <para>If it finds more than one element with the specified condition, throws exception. If you only need the <em>first</em> element of a given condition, use <seealso cref="ICloseServiceAsync{TKey, TEntity, TEntityDTO}.FindFirstByCondition(Expression{Func{TEntity, bool}})"/></para>
        /// <para><paramref name="expression"/>: Condition to filter the full list</para>
        /// <para><em>Usefull if you need to extend the interface.</em></para>
        /// </summary> 
        Task<TEntityDTO> FindOneByCondition(Expression<Func<TEntity, bool>> expression);

        /// <summary>.
        /// <para>Find ONE element with ONE condition.</para>
        /// <para>If it finds more than one element with the specified condition, throws exception. If you only need the <em>first</em> element of a given condition, use <seealso cref="ICloseServiceAsync{TKey, TEntity, TEntityDTO}.FindFirstByCondition(Expression{Func{TEntity, bool}})"/></para>
        /// <para><paramref name="expressions"/>: Conditions to filter the full list</para>
        /// <para><em>Usefull if you need to extend the interface.</em></para>
        /// </summary> 
        Task<TEntityDTO> FindOneByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions);

        /// <summary>.
        /// <para>Find the FIRST element with ONE condition.</para>
        /// <para>If it finds more than one element with the specified condition, gives only the first. If you conditions always should bring one element, use <seealso cref="ICloseServiceAsync{TKey, TEntity, TEntityDTO}.FindOneByCondition(Expression{Func{TEntity, bool}})"/></para>
        /// <para><paramref name="expression"/>: Condition to filter the full list</para>
        /// <para><em>Usefull if you need to extend the interface.</em></para>
        /// </summary> 
        Task<TEntityDTO> FindFirstByCondition(Expression<Func<TEntity, bool>> expression);

        /// <summary>.
        /// <para>Find the FIRST element with MANY conditions.</para>
        /// <para>If it finds more than one element with the specified condition, gives only the first. If you conditions always should bring one element, use <seealso cref="ICloseServiceAsync{TKey, TEntity, TEntityDTO}.FindOneByConditions(IEnumerable{Expression{Func{TEntity, bool}}})"/></para>
        /// <para><paramref name="expressions"/>: Conditions to filter the full list</para>
        /// <para><em>Usefull if you need to extend the interface.</em></para>
        /// </summary> 
        Task<TEntityDTO> FindFirstByConditions(IEnumerable<Expression<Func<TEntity, bool>>> expressions);
    }
}
