using Microsoft.AspNetCore.Mvc;
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
    public abstract class CloseControllerAsync<TKey, TEntity, TServiceInterface> : ControllerBase, ICloseControllerAsync<TKey, TEntity>
        where TEntity : class, IAbstractModel<TKey>
        where TServiceInterface : ICloseServiceAsync<TKey, TEntity>
    {
        protected TServiceInterface Service { get; private set; }

        public CloseControllerAsync(TServiceInterface service)
        {
            Service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> Read()
            => Ok(await Service.Read());

        [HttpGet("{key}")]
        public async Task<ActionResult<TEntity>> Read([FromRoute] TKey key)
            => Ok(await Service.Read(key));

        [HttpGet("paginate")]
        public async Task<ActionResult<IEnumerable<TEntity>>> Paginate([FromQuery] int number, [FromQuery] int page)
        {
            ActionResult<IEnumerable<TEntity>> result;

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
        public async Task<ActionResult<IEnumerable<TEntityDTO>>> Read()
            => Ok(await Service.Read());

        [HttpGet("{key}")]
        public async Task<ActionResult<TEntityDTO>> Read([FromRoute] TKey key)
            => Ok(await Service.Read(key));

        [HttpGet("paginate")]
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
        public async Task<ActionResult<bool>> CheckExistenceByKey([FromRoute] TKey key)
            => Ok(await Service.CheckExistenceByKey(key));
    }
}
