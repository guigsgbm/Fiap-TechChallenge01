using AutoMapper;
using WebAPI.Data.Dtos;

namespace WebAPI.Profiles;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<CreateImageDto, Models.Image>();
        CreateMap<UpdateImageDto, Models.Image>();
        CreateMap<Models.Image, UpdateImageDto>(); 
        CreateMap<Models.Image, ReadImageDto>();
        CreateMap<IEnumerable<Models.Image>, ReadImageDto>();
    }
}