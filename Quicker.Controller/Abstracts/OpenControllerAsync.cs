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
    public abstract class OpenControllerAsync<TKey, TEntity, TServiceInterface> : CloseControllerAsync<TKey, TEntity, TServiceInterface>, IOpenControllerAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
        where TServiceInterface : IOpenServiceAsync<TKey, TEntity>
    {
        public OpenControllerAsync(TServiceInterface service) : 
            base(service) { }

        [HttpPost]
        [Consumes(ControllerConstants.JsonContentType)]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<TEntity>> Create([FromBody] TEntity entity)
        {
            ActionResult<TEntity> actionResult;

            try
            {
                actionResult = StatusCode(StatusCodes.Status201Created, await Service.Create(entity));
            }
            catch (ArgumentNullException)
            {
                actionResult = BadRequest();
            }
            catch (InvalidOperationException ex)
            {
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

        [HttpGet("new")]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Dictionary<string, string>> New()
            => Ok(Service.GetPropertyInformationForCreate());

        [HttpDelete]
        [Consumes(ControllerConstants.JsonContentType)]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> Delete([FromBody] TEntity entity)
        {
            ActionResult actionResult;

            try
            {
                await Service.Delete(entity);

                actionResult = Ok();
            }
            catch (ArgumentNullException)
            {
                actionResult = BadRequest();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "entity")
                    actionResult = StatusCode(StatusCodes.Status406NotAcceptable);
                else
                    actionResult = Conflict(ex.Message);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                actionResult = Conflict(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                actionResult = UnprocessableEntity(ex.Message);
            }

            return actionResult;
        }

        [HttpDelete("{key}")]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> Delete(TKey key)
        {
            ActionResult actionResult;

            try
            {
                await Service.Delete(key);

                actionResult = Ok();
            }
            catch (ArgumentNullException)
            {
                actionResult = BadRequest();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "entity")
                    actionResult = StatusCode(StatusCodes.Status406NotAcceptable);
                else
                    actionResult = Conflict(ex.Message);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                actionResult = Conflict(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                actionResult = UnprocessableEntity(ex.Message);
            }

            return actionResult;
        }
    }

    [ApiController]
    public abstract class OpenControllerAsync<TKey, TEntity, TEntityDTO, TServiceInterface> : CloseControllerAsync<TKey, TEntity, TEntityDTO, TServiceInterface>, IOpenControllerAsync<TKey, TEntity, TEntityDTO>
        where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
        where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
        where TServiceInterface : IOpenServiceAsync<TKey, TEntity, TEntityDTO>
    {
        public OpenControllerAsync(TServiceInterface service) : 
            base(service) { }

        [HttpPost]
        [Consumes(ControllerConstants.JsonContentType)]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<TEntityDTO>> Create([FromBody] TEntityDTO entity)
        {
            ActionResult<TEntityDTO> actionResult;

            try
            {
                actionResult = StatusCode(StatusCodes.Status201Created, await Service.Create(entity));
            }
            catch (ArgumentNullException)
            {
                actionResult = BadRequest();
            }
            catch (InvalidOperationException ex)
            {
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

        [HttpGet("new")]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Dictionary<string, string>> New()
            => Ok(Service.GetPropertyInformationForCreate());

        [HttpDelete]
        [Consumes(ControllerConstants.JsonContentType)]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> Delete([FromBody] TEntityDTO entity)
        {
            ActionResult actionResult;

            try
            {
                await Service.Delete(entity);

                actionResult = Ok();
            }
            catch (ArgumentNullException)
            {
                actionResult = BadRequest();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "entity")
                    actionResult = StatusCode(StatusCodes.Status406NotAcceptable);
                else
                    actionResult = Conflict(ex.Message);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                actionResult = Conflict(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                actionResult = UnprocessableEntity(ex.Message);
            }

            return actionResult;
        }

        [HttpDelete("{key}")]
        [Consumes(ControllerConstants.JsonContentType)]
        [Produces(ControllerConstants.JsonContentType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> Delete(TKey key)
        {
            ActionResult actionResult;

            try
            {
                await Service.Delete(key);

                actionResult = Ok();
            }
            catch (ArgumentNullException)
            {
                actionResult = BadRequest();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "entity")
                    actionResult = StatusCode(StatusCodes.Status406NotAcceptable);
                else
                    actionResult = Conflict(ex.Message);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                actionResult = Conflict(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                actionResult = UnprocessableEntity(ex.Message);
            }

            return actionResult;
        }
    }
}
