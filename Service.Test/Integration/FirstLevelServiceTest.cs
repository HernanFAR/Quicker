using Quicker.Interfaces.Service;
using Service.Test.Models;
using Service.Test.Repository;
using Service.Test.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Service.Test
{
    public class FirstLevelServiceTest : IDisposable
    {
        private readonly TestContext _Context;
        private readonly IBaseService<int, FirstLevelEntity> _Service;


        public FirstLevelServiceTest()
        {
            _Context = new ConnectionFactory()
                .CreateContextForSQLite();

            _Service = new FirstLevelEntityService(_Context);
        }

        public void Dispose()
        {
            _Context.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void QueryAll_Success_ShouldReturnListWithTwoElem()
        {
            var expCount = 2;

            var count = _Service.QueryAll()
                .Count();

            Assert.Equal(expCount, count);
        }

        [Fact]
        public async Task QueryOne_Success_ShouldReturnEntity()
        {
            var id = 1;

            var count = await _Service.QuerySingle(id);

            Assert.NotNull(count);
        }

        [Fact]
        public async Task QueryOne_Success_ShouldReturnNull()
        {
            var id = 10;

            var count = await _Service.QuerySingle(id);

            Assert.Null(count);
        }
    }
}
