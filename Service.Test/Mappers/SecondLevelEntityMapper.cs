using AutoMapper;
using Service.Test.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Test.Mappers
{
    public class SecondLevelEntityMapper : Profile
    {
        public SecondLevelEntityMapper()
        {
            CreateMap<SecondLevelEntity, SecondLevelEntityDTO>()
                .ForMember(dest =>
                    dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                    dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest =>
                    dest.FirstLevelEntityId,
                    opt => opt.MapFrom(src => src.FirstLevelEntityId))
                .ForMember(dest =>
                    dest.FirstLevelEntity,
                    opt => opt.MapFrom(src => src.FirstLevelEntityNavigation.Name));

            CreateMap<SecondLevelEntityDTO, SecondLevelEntity>();
        }
    }
}
