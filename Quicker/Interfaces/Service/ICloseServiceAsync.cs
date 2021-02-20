using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Quicker.Interfaces.Service
{
    /// <summary>
    ///     Interface para especificar un <em>servicio cerrado</em>, con entidades que no tienen un DTO relacionado.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Un <em>servicio cerrado</em> provee funciones de solo lectura de elementos en la base de datos, dando la R de el CRUD.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TKey">Tipo de la PK en la base de datos</typeparam>
    /// <typeparam name="TEntity">
    ///     <para>
    ///         Tipo de la entidad que debe devolver el servicio. 
    ///     </para>
    ///     <para>
    ///         Debe ser una clase e implementar <see cref="IAbstractModel{TKey}"/>.
    ///     </para>
    /// </typeparam>
    /// 
    public interface ICloseServiceAsync<TKey, TEntity> 
        where TEntity : class, IAbstractModel<TKey>
    {
        /// <summary>
        ///     Lee todos los elementos de la base de datos.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que devuelve un <see cref="IEnumerable{T}"/> con tipo <typeparamref name="TEntity"/>.
        /// </returns>
        /// 
        Task<IEnumerable<TEntity>> Read();

        /// <summary>
        ///     Lee un elemento en la base de datos.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que devuelve la <typeparamref name="TEntity"/> relacionada con esa clave primaria.
        /// </returns>
        /// <param name="key">Valor de la Primary key a encontrar en la base de datos.</param>
        /// 
        Task<TEntity> Read(TKey key);

        /// <summary>
        ///     Lee elementos de la base de datos, de forma paginada.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que devuelve un <see cref="IEnumerable{T}"/> con tipo <typeparamref name="TEntity"/>, con un largo de maximo <paramref name="number"/>.
        /// </returns>
        /// <param name="number">Cantidad de elementos a tomar de la base de datos.</param>
        /// <param name="page">Numero de pagina de los elementos</param>
        /// 
        Task<IEnumerable<TEntity>> Paginate(int number, int page);
    }

    /// <summary>
    ///     Interface para especificar un <em>servicio cerrado</em>, con entidades que si tienen un DTO relacionado.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Un <em>servicio cerrado</em> provee funciones de solo lectura de elementos en la base de datos, dando la R de el CRUD.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TKey">Tipo de la PK en la base de datos</typeparam>
    /// <typeparam name="TEntity">
    ///     <para>
    ///         Tipo de la entidad que debe devolver la base de datos. 
    ///     </para>
    ///     <para>
    ///         Debe ser una clase, implementar <see cref="IAbstractModel{TKey}"/> y <see cref="IDTOOf{TDomain}"/>, 
    ///         con TDomain de valor <typeparamref name="TEntityDTO"/>
    ///     </para>
    /// </typeparam>
    /// <typeparam name="TEntityDTO">
    ///     <para>
    ///         Tipo de la entidad que debe devolver el servicio. 
    ///     </para>
    ///     <para>
    ///         Debe ser una clase, implementar <see cref="IAbstractModel{TKey}"/> y <see cref="IDomainOf{TDTO}"/>, 
    ///         con TDTO de valor <typeparamref name="TEntity"/>
    ///     </para>
    /// </typeparam>
    /// 
    public interface ICloseServiceAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        /// <summary>
        ///     Lee todos los elementos de la base de datos.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que devuelve un <see cref="IEnumerable{T}"/> con tipo <typeparamref name="TEntityDTO"/>.
        /// </returns>
        /// 
        Task<IEnumerable<TEntityDTO>> Read();

        /// <summary>
        ///     Lee un elemento en la base de datos.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que devuelve la <typeparamref name="TEntityDTO"/> relacionada con esa clave primaria.
        /// </returns>
        /// <param name="key">Valor de la Primary key a encontrar en la base de datos.</param>
        /// 
        Task<TEntityDTO> Read(TKey key);

        /// <summary>
        ///     Lee elementos de la base de datos, de forma paginada.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que devuelve un <see cref="IEnumerable{T}"/> con tipo <typeparamref name="TEntityDTO"/>, con un largo de maximo <paramref name="number"/>.
        /// </returns>
        /// <param name="number">Cantidad de elementos a tomar de la base de datos.</param>
        /// <param name="page">Numero de pagina de los elementos</param>
        /// 
        Task<IEnumerable<TEntityDTO>> Paginate(int number, int page);
    }
}
