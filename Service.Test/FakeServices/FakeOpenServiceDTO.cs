using Quicker.Abstracts.Service;
using System;
using Test.Common.Repository;
using Test.Common.Repository.DTO;

namespace Quicker.Services.Test.Fake
{
    public class FakeOpenServiceDTO : OpenServiceAsync<int, TestModelRelation, TestModelRelationDTO>
    {
        public FakeOpenServiceDTO(IServiceProvider service) : 
            base(service) { }
    }
}
