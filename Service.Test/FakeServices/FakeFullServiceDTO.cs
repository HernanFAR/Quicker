using Quicker.Abstracts.Service;
using System;
using Test.Common.Repository;
using Test.Common.Repository.DTO;

namespace Quicker.Services.Test.Fake
{
    public class FakeFullServiceDTO : FullServiceAsync<int, TestModelRelation, TestModelRelationDTO>
    {
        public FakeFullServiceDTO(IServiceProvider service) : 
            base(service) { }
    }
}
