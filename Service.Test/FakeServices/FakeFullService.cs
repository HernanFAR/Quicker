using Quicker.Abstracts.Service;
using Quicker.Configuration;
using Quicker.Test.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quicker.Test.FakeServices
{
    public class FakeFullService : FullServiceAsync<int, TestModel>
    {
        public FakeFullService(IServiceProvider service) : 
            base(service) { }
    }
}
