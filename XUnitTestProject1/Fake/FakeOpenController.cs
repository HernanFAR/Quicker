using Quicker.Abstracts.Controller;
using Quicker.Services.Test.Fake;
using Test.Common.Repository;

namespace Quicker.Controller.Test.Fake
{
    public class FakeOpenController : OpenControllerAsync<int, TestModel, FakeOpenService>
    {
        public FakeOpenController(FakeOpenService service) : 
            base(service) { }
    }
}
