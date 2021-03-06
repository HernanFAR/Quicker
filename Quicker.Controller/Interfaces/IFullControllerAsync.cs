﻿using Microsoft.AspNetCore.Mvc;
using Quicker.Interfaces.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quicker.Interfaces.WebApiController
{
    /// <summary>
    ///     Interface para especificar un <em>servicio completo</em>, con entidades que no tienen un DTO relacionado.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Un <em>servicio completo</em> provee funciones de lectura, escritura, actualizacion y borrado de elementos en la base de datos, dando el CRUD.
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
    public interface IFullControllerAsync<TKey, TEntity> : IOpenControllerAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        /// <summary>
        ///     Obtiene los nombres de las propiedades y los tipos de la entidad a editar.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que devuelve un <see cref="ActionResult"/> con un 
        ///     <see cref="Dictionary{string, string}"/> con la informacion de la entidad.
        /// </returns>
        /// 
        ActionResult<Dictionary<string, string>> Edit();

        /// <summary>
        ///     Actualiza un elemento en la base de datos.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que devuelve el <typeparamref name="TEntity"/> recientemente actualizado.
        /// </returns>
        /// <param name="key">PK de la entidad a actualizar en la base de datos.</param>
        /// <param name="entity">Entidad a actualizar en la base de datos.</param>
        /// 
        Task<ActionResult<TEntity>> Update(TKey key, TEntity entity);
    }

    /// <summary>
    ///     Interface para especificar un <em>servicio abierto</em>, con entidades que si tienen un DTO relacionado.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Un <em>servicio completo</em> provee funciones de lectura, escritura, actualizacion y borrado de elementos en la base de datos, dando el CRUD.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TKey">Tipo de la PK en la base de datos</typeparam>
    /// <typeparam name="TEntity">
    ///     <para>
    ///         Tipo de la entidad que debe devolver la base de datos. 
    ///     </para>
    ///     <para>
    ///         Debe ser una clase, implementar <see cref="IAbstractModel{TKey}"/> y <see cref="IDomainOf{TDTO}"/>, 
    ///         con TDTO de valor <typeparamref name="TEntityDTO"/>
    ///     </para>
    /// </typeparam>
    /// <typeparam name="TEntityDTO">
    ///     <para>
    ///         Tipo de la entidad que debe devolver el servicio. 
    ///     </para>
    ///     <para>
    ///         Debe ser una clase, implementar <see cref="IAbstractModel{TKey}"/> y <see cref="IDTOOf{TDomain}"/>, 
    ///         con TDomain de valor <typeparamref name="TEntity"/>
    ///     </para>
    /// </typeparam>
    /// 
    public interface IFullControllerAsync<TKey, TEntity, TEntityDTO> : IOpenControllerAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        /// <summary>
        ///     Obtiene los nombres de las propiedades y los tipos de la entidad a editar.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que devuelve un <see cref="ActionResult"/> con un 
        ///     <see cref="Dictionary{string, string}"/> con la informacion de la entidad.
        /// </returns>
        /// 
        ActionResult<Dictionary<string, string>> Edit();

        /// <summary>
        ///     Actualiza un elemento en la base de datos.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que devuelve el <typeparamref name="TEntityDTO"/> recientemente actualizado.
        /// </returns>
        /// <param name="key">PK de la entidad a actualizar en la base de datos.</param>
        /// <param name="entity">Entidad a actualizar en la base de datos.</param>
        /// 
        Task<ActionResult<TEntityDTO>> Update(TKey key, TEntityDTO entity);
    }
}
