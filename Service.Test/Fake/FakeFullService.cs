using Quicker.Abstracts.Service;
using System;
using Test.Common.Repository;

namespace Quicker.Services.Test.Fake
{
    public class FakeFullService : FullServiceAsync<int, TestModel>
    {
        public FakeFullService(IServiceProvider service) : 
            base(service) { }
    }
}
