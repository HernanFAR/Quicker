using AutoMapper;
using Test.Common.Repository;
using Test.Common.Repository.DTO;

namespace Test.Common.Mapper
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest =>
                    dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                    dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest =>
                    dest.Count,
                    opt => opt.MapFrom(src => src.Count))
                .ForMember(dest =>
                    dest.Price,
                    opt => opt.MapFrom(src => src.Price))
                .ForMember(dest =>
                    dest.Code,
                    opt => opt.MapFrom(src => src.Code))
                .ForMember(dest =>
                    dest.CategoryId,
                    opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest =>
                    dest.Category,
                    opt => opt.MapFrom(src => src.Category))
                .ForMember(dest =>
                    dest.CreatedAt,
                    opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest =>
                    dest.LastUpdated,
                    opt => opt.MapFrom(src => src.LastUpdated));


            CreateMap<ProductDTO, Product>();
        }
    }
}
