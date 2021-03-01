using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quicker.Controller.Constants;
using Quicker.Interfaces.Model;
using Quicker.Interfaces.Service;
using Quicker.Interfaces.WebApiController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Quicker.Abstracts.Controller
{
#warning Pendiente agregar documentacion
    [ApiController]
    public abstract class CloseControllerAsync<TKey, TEntity, TServiceInterface> : ControllerBase, ICloseControllerAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
        where TServiceInterface : ICloseServiceAsync<TKey, TEntity>
    {
        protected TServiceInterface Service { get; }

        public CloseControllerAsync(TServiceInterface service)
        {
            Service = service;
        }

        [HttpGet]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TEntity>>> Read()
        {
            ActionResult<IEnumerable<TEntity>> actionResult;

            var entities = await Service.Read();

            if (entities.ToList().Count == 0)
                actionResult = NoContent();
            else
                actionResult = Ok(entities);

            return actionResult;
        }

        [HttpGet("{key}")]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<TEntity>> Read([FromRoute] TKey key)
        {
            ActionResult<TEntity> actionResult;

            try
            {
                var entity = await Service.Read(key);

                if (entity == null)
                    actionResult = NotFound();
                else
                    actionResult = Ok(entity);
            }
            catch (ArgumentException ex)
            {
                actionResult = UnprocessableEntity(ex.Message);
            }

            return actionResult;
        }

        [HttpGet("paginate")]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<IEnumerable<TEntity>>> Paginate([FromQuery] int number, [FromQuery] int page)
        {
            ActionResult<IEnumerable<TEntity>> result;

            try
            {
                var entities = await Service.Paginate(number, page);

                if (entities.ToList().Count == 0)
                    result = NoContent();
                else
                    result = Ok(entities);
            }
            catch (ArgumentException ex)
            {
                result = UnprocessableEntity(ex.Message);
            }

            return result;
        }

        [HttpGet("exists/{key}")]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CheckExistenceByKey([FromRoute] TKey key)
            => Ok(await Service.CheckExistenceByKey(key));
    }

    [ApiController]
    public abstract class CloseControllerAsync<TKey, TEntity, TEntityDTO, TServiceInterface> : ControllerBase, ICloseControllerAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
        where TServiceInterface : ICloseServiceAsync<TKey, TEntity, TEntityDTO>
    {
        protected TServiceInterface Service { get; private set; }

        public CloseControllerAsync(TServiceInterface service)
        {
            Service = service;
        }

        [HttpGet]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TEntityDTO>>> Read()
            => Ok(await Service.Read());

        [HttpGet("{key}")]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TEntityDTO>> Read([FromRoute] TKey key)
            => Ok(await Service.Read(key));

        [HttpGet("paginate")]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<IEnumerable<TEntityDTO>>> Paginate([FromQuery] int number, [FromQuery] int page)
        {
            ActionResult<IEnumerable<TEntityDTO>> result;

            try
            {
                result = Ok(await Service.Paginate(number, page));
            }
            catch (ArgumentException ex)
            {
                result = UnprocessableEntity(ex.Message);
            }

            return result;
        }

        [HttpGet("exists/{key}")]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CheckExistenceByKey([FromRoute] TKey key)
            => Ok(await Service.CheckExistenceByKey(key));
    }
}
