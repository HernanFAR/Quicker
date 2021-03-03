using Quicker.Abstracts.Controller;
using Quicker.Services.Test.Fake;
using Test.Common.Repository;
using Test.Common.Repository.DTO;

namespace Quicker.Controller.Test.Fake
{
    public class FakeCloseControllerDTO : CloseControllerAsync<int, TestModelRelation, TestModelRelationDTO, FakeCloseServiceDTO>
    {
        public FakeCloseControllerDTO(FakeCloseServiceDTO service) : 
            base(service) { }
    }
}
