using Quicker.Abstracts.Component;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Common.Repository;

namespace Quicker.Service.Test.Fake.Component
{
    public class ReadComponent : ReadComponent<int, Category>
    {
        public ReadComponent(IServiceProvider service) : 
            base(service) { }
    }
}
