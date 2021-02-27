using AutoMapper;
using Quicker.Abstracts.Service;
using Quicker.Configuration;
using Quicker.Test.Repository;
using Quicker.Test.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quicker.Test.FakeServices
{
    public class FakeCloseServiceDTO : CloseServiceAsync<int, TestModelRelation, TestModelRelationDTO>
    {
        public FakeCloseServiceDTO(QuickerConfiguration configuration, IServiceProvider service) : 
            base(configuration, service) { }
    }
}
