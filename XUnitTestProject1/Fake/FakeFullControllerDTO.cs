using Quicker.Abstracts.Controller;
using Quicker.Services.Test.Fake;
using Test.Common.Repository;
using Test.Common.Repository.DTO;

namespace Quicker.Controller.Test.Fake
{
    public class FakeFullControllerDTO : FullControllerAsync<int, TestModelRelation, TestModelRelationDTO, FakeFullServiceDTO>
    {
        public FakeFullControllerDTO(FakeFullServiceDTO service) : 
            base(service) { }
    }
}
