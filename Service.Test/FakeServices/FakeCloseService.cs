using Quicker.Abstracts.Service;
using Quicker.Test.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quicker.Test.FakeServices
{
    public class FakeCloseService : CloseServiceAsync<int, TestModel>
    {
        public FakeCloseService(TestContext context) :
            base(context) { }
    }
}
