using Microsoft.AspNetCore.Mvc;
using Quicker.Abstracts.Controller;
using Quicker.Services.Test.Fake;
using Test.Common.Repository;
using Test.Common.Repository.DTO;

namespace Quicker.Integration.Test.Fake
{
    [Route("api/[controller]")]
    public class FakeCloseControllerDTO : CloseControllerAsync<int, TestModelRelation, TestModelRelationDTO, FakeCloseServiceDTO>
    {
        public FakeCloseControllerDTO(FakeCloseServiceDTO service) : 
            base(service) { }
    }
}
