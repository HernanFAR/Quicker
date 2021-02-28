using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quicker.Interfaces.Model;
using Quicker.Interfaces.Service;
using Quicker.Interfaces.WebApiController;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quicker.Abstracts.Controller
{
#warning Pendiente agregar version con DTO
#warning Pendiente agregar documentacion
    [ApiController]
    public abstract class OpenControllerAsync<TKey, TEntity, TServiceInterface> : CloseControllerAsync<TKey, TEntity, TServiceInterface>, IOpenControllerAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
        where TServiceInterface : IOpenServiceAsync<TKey, TEntity>
    {
        public OpenControllerAsync(TServiceInterface service) : 
            base(service) { }

        [HttpPost]
        public async Task<ActionResult<TEntity>> Create([FromBody] TEntity entity)
        {
            ActionResult<TEntity> actionResult;

            try
            {
                actionResult = Ok(await Service.Create(entity));
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
        public ActionResult<Dictionary<string, string>> New()
            => Ok(Service.GetPropertyInformationForCreate());

        [HttpDelete]
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
