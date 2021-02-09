using AutoMapper;
using Quicker.Abstracts.Service;
using Service.Test.Models;
using Service.Test.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Test.Services
{
    public class SecondLevelEntityService : BaseService<int, SecondLevelEntity, SecondLevelEntityDTO>
    {
        public SecondLevelEntityService(TestContext context, IMapper mapper) : 
            base(context, mapper) { }
    }
}
