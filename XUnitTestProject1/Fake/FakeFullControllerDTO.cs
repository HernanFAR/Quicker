using Quicker.Abstracts.Controller;
using Quicker.Interfaces.Service;
using Quicker.Services.Test.Fake;
using Test.Common.Repository;
using Test.Common.Repository.DTO;

namespace Quicker.Controller.Test.Fake
{
    public class FakeFullControllerDTO : FullControllerAsync<int, TestModelRelation, TestModelRelationDTO, IFullServiceAsync<int, TestModelRelation, TestModelRelationDTO>>
    {
        public FakeFullControllerDTO(IFullServiceAsync<int, TestModelRelation, TestModelRelationDTO> service) : 
            base(service) { }
    }
}
