using Quicker.Abstracts.Service;
using System;
using Test.Common.Repository;

namespace Quicker.Services.Test.Fake
{
    public class FakeCloseService : CloseServiceAsync<int, TestModel>
    {
        public FakeCloseService(IServiceProvider service) : 
            base(service) { }
    }
}
