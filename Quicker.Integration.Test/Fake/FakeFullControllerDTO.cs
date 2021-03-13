using Microsoft.AspNetCore.Mvc;
using Quicker.Abstracts.Controller;
using Quicker.Interfaces.Service;
using Quicker.Services.Test.Fake;
using Test.Common.Repository;
using Test.Common.Repository.DTO;

namespace Quicker.Integration.Test.Fake
{
    [Route("api/[controller]")]
    public class FakeFullDTOController : FullControllerAsync<int, TestModelRelation, TestModelRelationDTO, FakeFullServiceDTO>
    {
        public FakeFullDTOController(FakeFullServiceDTO service) : 
            base(service) { }
    }
}
