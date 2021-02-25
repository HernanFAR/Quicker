using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quicker.Abstracts.Service;
using Quicker.Test.Repository;
using Quicker.Test.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quicker.Test.FakeServices
{
    public class FakeOpenServiceDTO : OpenServiceAsync<int, TestModelRelation, TestModelRelationDTO>
    {
        public FakeOpenServiceDTO(DbContext context) : 
            base(context) { }

        public FakeOpenServiceDTO(DbContext context, IMapper mapper) : 
            base(context, mapper) { }
    }
}
