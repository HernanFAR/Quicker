using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quicker.Controller.Constants;
using Quicker.Interfaces.Model;
using Quicker.Interfaces.Service;
using Quicker.Interfaces.WebApiController;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quicker.Abstracts.Controller
{
#warning Pendiente agregar documentacion
    [ApiController]
    public class FullControllerAsync<TKey, TEntity, TServiceInterface> : OpenControllerAsync<TKey, TEntity, TServiceInterface>, IFullControllerAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
        where TServiceInterface : IFullServiceAsync<TKey, TEntity>
    {
        public FullControllerAsync(TServiceInterface service) :
            base(service) { }

        [HttpGet("Edit")]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Dictionary<string, string>> Edit()
            => Ok(Service.GetPropertyInformationForUpdating());

        [HttpPut("{key}")]
        [Consumes(ControllerConstants.JsonContentType)]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<TEntity>> Update([FromQuery] TKey key, [FromBody] TEntity entity)
        {
            ActionResult<TEntity> actionResult;

            try
            {
                actionResult = Ok(await Service.Update(key, entity));
            }
            catch (ArgumentNullException)
            {
                actionResult = BadRequest();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "key")
                    actionResult = StatusCode(StatusCodes.Status406NotAcceptable);
                else
                    actionResult = Conflict(ex.Message);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                actionResult = Conflict(ex.Message);
            }
            catch (ValidationException ex)
            {
                actionResult = UnprocessableEntity(ex.Errors);
            }
            catch (DbUpdateException ex)
            {
                actionResult = UnprocessableEntity(ex.Message);
            }

            return actionResult;
        }
    }

    [ApiController]
    public class FullControllerAsync<TKey, TEntity, TEntityDTO, TServiceInterface> : OpenControllerAsync<TKey, TEntity, TEntityDTO, TServiceInterface>, IFullControllerAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
        where TServiceInterface : IFullServiceAsync<TKey, TEntity, TEntityDTO>
    {
        public FullControllerAsync(TServiceInterface service) :
            base(service) { }

        [HttpGet("Edit")]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Dictionary<string, string>> Edit()
            => Ok(Service.GetPropertyInformationForUpdating());

        [HttpPut("{key}")]
        [Consumes(ControllerConstants.JsonContentType)]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<TEntityDTO>> Update([FromQuery] TKey key, [FromBody] TEntityDTO entity)
        {
            ActionResult<TEntityDTO> actionResult;

            try
            {
                actionResult = Ok(await Service.Update(key, entity));
            }
            catch (ArgumentNullException)
            {
                actionResult = BadRequest();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "key")
                    actionResult = StatusCode(StatusCodes.Status406NotAcceptable);
                else
                    actionResult = Conflict(ex.Message);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                actionResult = Conflict(ex.Message);
            }
            catch (ValidationException ex)
            {
                actionResult = UnprocessableEntity(ex.Errors);
            }
            catch (DbUpdateException ex)
            {
                actionResult = UnprocessableEntity(ex.Message);
            }

            return actionResult;
        }
    }
}
