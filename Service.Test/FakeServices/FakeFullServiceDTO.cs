using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quicker.Abstracts.Service;
using Quicker.Configuration;
using Quicker.Test.Repository;
using Quicker.Test.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quicker.Test.FakeServices
{
    public class FakeFullServiceDTO : FullServiceAsync<int, TestModelRelation, TestModelRelationDTO>
    {
        public FakeFullServiceDTO(IServiceProvider service) : 
            base(service) { }
    }
}
