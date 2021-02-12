using Quicker.Abstracts.Service;
using Service.Test.Models;
using Service.Test.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Test.Services.Base
{
    public class FirstLevelEntityService : BaseService<int, FirstLevelEntity>
    {
        public FirstLevelEntityService(TestContext context) : 
            base(context) { }
    }
}
