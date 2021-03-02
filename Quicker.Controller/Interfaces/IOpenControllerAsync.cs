using Microsoft.AspNetCore.Mvc;
using Quicker.Interfaces.Model;
using Quicker.Interfaces.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quicker.Interfaces.WebApiController
{
    /// <summary>
    ///     Interface para especificar un <em>controlador abierto</em>, con entidades que no tienen un 
    ///     DTO relacionado.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Un <em>controlador abierto</em> consume un servicio abierto, que tiene funciones de solo 
    ///         lectura, escritura y borrado de elementos en la base de datos, dando la CRD de el CRUD.
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
    public interface IOpenControllerAsync<TKey, TEntity> : ICloseControllerAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
    {
        /// <summary>
        ///     Obtiene los nombres de las propiedades y los tipos de la entidad a crear.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que devuelve un <see cref="ActionResult"/> con un 
        ///     <see cref="Dictionary{string, string}"/> con la informacion de la entidad.
        /// </returns>
        /// <param name="entity">Entidad a crear en la base de datos.</param>
        /// 
        ActionResult<Dictionary<string, string>> New();

        /// <summary>
        ///     Crea un elemento en la base de datos.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que devuelve el <typeparamref name="TEntity"/> recientemente creado.
        /// </returns>
        /// <param name="entity">Entidad a crear en la base de datos.</param>
        /// 
        Task<ActionResult<TEntity>> Create(TEntity entity);

        /// <summary>
        ///     Borra un elemento en la base de datos.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que indica cuando termina el procedimiento.
        /// </returns>
        /// <param name="entity">Entidad a borrar en la base de datos.</param>
        /// 
        Task<ActionResult> Delete(TEntity entity);

        /// <summary>
        ///     Borra un elemento en la base de datos.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que indica cuando termina el procedimiento.
        /// </returns>
        /// <param name="key">PK de la entidad a borrar en la base de datos.</param>
        /// 
        Task<ActionResult> Delete(TKey key);
    }

    /// <summary>
    ///     Interface para especificar un <em>controlador abierto</em>, con entidades que si tienen un 
    ///     DTO relacionado.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Un <em>controlador abierto</em> consume un servicio abierto, que tiene funciones de solo 
    ///         lectura, escritura y borrado de elementos en la base de datos, dando la CRD de el CRUD.
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
    public interface IOpenControllerAsync<TKey, TEntity, TEntityDTO> : ICloseControllerAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
    {
        /// <summary>
        ///     Crea un elemento en la base de datos.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que devuelve el <typeparamref name="TEntityDTO"/> recientemente creado.
        /// </returns>
        /// <param name="entity">Entidad a crear en la base de datos.</param>
        /// 
        Task<ActionResult<TEntityDTO>> Create(TEntityDTO entity);

        /// <summary>
        ///     Borra un elemento en la base de datos.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que indica cuando termina el procedimiento.
        /// </returns>
        /// <param name="entity">Entidad a borrar en la base de datos.</param>
        /// 
        Task<ActionResult> Delete(TEntityDTO entity);

        /// <summary>
        ///     Borra un elemento en la base de datos.
        /// </summary> 
        /// <returns>
        ///     Un <see cref="Task"/> que indica cuando termina el procedimiento.
        /// </returns>
        /// <param name="key">PK de la entidad a borrar en la base de datos.</param>
        /// 
        Task<ActionResult> Delete(TKey key);
    }
}
