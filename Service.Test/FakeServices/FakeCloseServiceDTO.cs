using Quicker.Abstracts.Service;
using System;
using Test.Common.Repository;
using Test.Common.Repository.DTO;

namespace Quicker.Services.Test.Fake
{
    public class FakeCloseServiceDTO : CloseServiceAsync<int, TestModelRelation, TestModelRelationDTO>
    {
        public FakeCloseServiceDTO(IServiceProvider service) : 
            base(service) { }
    }
}
