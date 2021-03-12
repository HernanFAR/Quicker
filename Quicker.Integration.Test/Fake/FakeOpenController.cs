using Microsoft.AspNetCore.Mvc;
using Quicker.Abstracts.Controller;
using Quicker.Services.Test.Fake;
using Test.Common.Repository;

namespace Quicker.Integration.Test.Fake
{
    [Route("api/[controller]")]
    public class FakeOpenController : OpenControllerAsync<int, TestModel, FakeOpenService>
    {
        public FakeOpenController(FakeOpenService service) : 
            base(service) { }
    }
}
