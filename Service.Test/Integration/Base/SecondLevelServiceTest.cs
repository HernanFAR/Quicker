using AutoMapper;
using Quicker.Interfaces.Service;
using Service.Test.Mappers;
using Service.Test.Models;
using Service.Test.Repository;
using Service.Test.Services.Base;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Service.Test.Base
{
    public class SecondLevelServiceTest : IDisposable
    {
        private readonly TestContext _Context;
        private readonly IMapper _Mapper;
        private readonly IBaseService<int, SecondLevelEntity, SecondLevelEntityDTO> _Service;


        public SecondLevelServiceTest()
        {
            _Context = new ConnectionFactory()
                .CreateContextForSQLite();
            _Mapper = new MapperConfiguration(e => {
                e.AddProfile<SecondLevelEntityMapper>();
            })
            .CreateMapper();
            _Service = new SecondLevelEntityService(_Context, _Mapper);
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

        [Fact]
        public void ToDTO_Success_ShouldReturnAllProperties()
        {
            var model = new SecondLevelEntity
            {
                Id = 1,
                Name = "SecondLevelEntity",
                FirstLevelEntityId = 2,
                FirstLevelEntityNavigation = new FirstLevelEntity
                {
                    Name = "FirstLevelEntity"
                }
            };

            var mapped = _Service.ToDTO(model);

            Assert.Equal(model.Id, mapped.Id);
            Assert.Equal(model.Name, mapped.Name);
            Assert.Equal(model.FirstLevelEntityId, mapped.FirstLevelEntityId);
            Assert.Equal(model.FirstLevelEntityNavigation.Name, mapped.FirstLevelEntity);
        }

        [Fact]
        public void ToDomain_Success_ShouldReturnAllProperties()
        {
            var model = new SecondLevelEntityDTO
            {
                Id = 1,
                Name = "SecondLevelEntity",
                FirstLevelEntityId = 2,
                FirstLevelEntity = "FirstLevelEntity"
            };

            var mapped = _Service.ToDomain(model);

            Assert.Equal(model.Id, mapped.Id);
            Assert.Equal(model.Name, mapped.Name);
            Assert.Equal(model.FirstLevelEntityId, mapped.FirstLevelEntityId);
        }
    }
}
