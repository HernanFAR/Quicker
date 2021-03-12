using Microsoft.AspNetCore.Mvc;
using Quicker.Abstracts.Controller;
using Quicker.Services.Test.Fake;
using Test.Common.Repository;

namespace Quicker.Integration.Test.Fake
{
    [Route("api/[controller]")]
    public class FakeCloseController : CloseControllerAsync<int, TestModel, FakeCloseService>
    {
        public FakeCloseController(FakeCloseService service) : 
            base(service) { }
    }
}
