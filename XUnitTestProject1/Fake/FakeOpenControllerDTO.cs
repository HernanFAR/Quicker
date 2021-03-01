using Quicker.Abstracts.Controller;
using Quicker.Services.Test.Fake;
using Test.Common.Repository;
using Test.Common.Repository.DTO;

namespace Quicker.Controller.Test.Fake
{
    public class FakeOpenControllerDTO : OpenControllerAsync<int, TestModelRelation, TestModelRelationDTO, FakeOpenServiceDTO>
    {
        public FakeOpenControllerDTO(FakeOpenServiceDTO service) : 
            base(service) { }
    }
}
