using AutoMapper;
using WebAPI.Data.Dtos;

namespace WebAPI.Profiles;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<CreateImageDto, Models.Image>();
    }
}
