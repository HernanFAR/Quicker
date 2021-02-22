using Quicker.Abstracts.Service;
using Quicker.Test.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quicker.Test.FakeServices
{
    public class FakeOpenService : OpenServiceAsync<int, TestModel>
    {
        public FakeOpenService(TestContext context) :
            base(context) { }
    }
}
