using Quicker.Abstracts.Controller;
using Quicker.Services.Test.Fake;
using Test.Common.Repository;

namespace Quicker.Controller.Test.Fake
{
    public class FakeCloseController : CloseControllerAsync<int, TestModel, FakeCloseService>
    {
        public FakeCloseController(FakeCloseService service) : 
            base(service) { }
    }
}
