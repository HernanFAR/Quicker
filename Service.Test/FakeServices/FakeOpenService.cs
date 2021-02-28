using Quicker.Abstracts.Service;
using System;
using Test.Common.Repository;

namespace Quicker.Services.Test.Fake
{
    public class FakeOpenService : OpenServiceAsync<int, TestModel>
    {
        public FakeOpenService(IServiceProvider service) : 
            base(service) { }
    }
}
