using AutoMapper;
using Test.Common.Repository;
using Test.Common.Repository.DTO;

namespace Test.Common.Mapper
{
    public class TestModelRelationMapper : Profile
    {
        public TestModelRelationMapper()
        {
            CreateMap<TestModelRelation, TestModelRelationDTO>()
                .ForMember(dest =>
                    dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                    dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest =>
                    dest.UniqueName,
                    opt => opt.MapFrom(src => src.UniqueName))
                .ForMember(dest =>
                    dest.TestModelId,
                    opt => opt.MapFrom(src => src.TestModelId))
                .ForMember(dest =>
                    dest.TestModel,
                    opt => opt.MapFrom(src => src.TestModelNavigation.Name));


            CreateMap<TestModelRelationDTO, TestModelRelation>();
        }
    }
}
