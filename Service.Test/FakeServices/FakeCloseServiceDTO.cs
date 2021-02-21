using AutoMapper;
using Quicker.Abstracts.Service;
using Quicker.Test.Repository;
using Quicker.Test.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quicker.Test.FakeServices
{
    public class FakeCloseServiceDTO : CloseServiceAsync<int, TestModelRelation, TestModelRelationDTO>
    {
        public FakeCloseServiceDTO(TestContext context, IMapper mapper) :
            base(context, mapper) { }
    }
}
