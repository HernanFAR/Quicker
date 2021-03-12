using Quicker.Abstracts.Controller;
using Quicker.Services.Test.Fake;
using Test.Common.Repository;

namespace Quicker.Controller.Test.Fake
{
    public class FakeFullController : FullControllerAsync<int, TestModel, FakeFullService>
    {
        public FakeFullController(FakeFullService service) : 
            base(service) { }
    }
}
