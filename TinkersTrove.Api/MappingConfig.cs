using AutoMapper;
using TinkersTrove.Api.DTOs;
using TinkersTrove.Api.Models;

namespace TinkersTrove.Api;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Category, CategoryDto>()
            .ForMember(
                dest => dest.ChildCategories,
                opt => opt.MapFrom(src => src.ChildCategories));
        CreateMap<CategoryDto, Category>();
    }
}