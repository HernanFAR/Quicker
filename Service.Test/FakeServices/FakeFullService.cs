using Quicker.Abstracts.Service;
using Quicker.Configuration;
using Quicker.Test.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quicker.Test.FakeServices
{
    public class FakeFullServiceDTO : FullServiceAsync<int, TestModel>
    {
        public FakeFullServiceDTO(IServiceProvider service) : 
            base(service) { }
    }
}
