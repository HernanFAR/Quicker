using Microsoft.AspNetCore.Mvc;
using Quicker.Abstracts.Controller;
using Quicker.Interfaces.Service;
using Test.Common.Repository;
using Test.Common.Repository.DTO;

namespace Quicker.Integration.Test.Fake
{
    [Route("api/[controller]")]
    public class FakeFullControllerDTO : FullControllerAsync<int, TestModelRelation, TestModelRelationDTO, IFullServiceAsync<int, TestModelRelation, TestModelRelationDTO>>
    {
        public FakeFullControllerDTO(IFullServiceAsync<int, TestModelRelation, TestModelRelationDTO> service) : 
            base(service) { }
    }
}
