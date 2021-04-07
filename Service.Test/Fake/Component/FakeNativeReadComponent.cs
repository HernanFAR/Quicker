using Quicker.Abstracts.Component;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Common.Repository;

namespace Quicker.Service.Test.Fake.Component
{
    public class FakeNativeReadComponent : ReadComponent<int, Category>
    {
        public FakeNativeReadComponent(IServiceProvider service) : 
            base(service) { }
    }
}
