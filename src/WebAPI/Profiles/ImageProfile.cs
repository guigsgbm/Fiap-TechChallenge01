using AutoMapper;
using Microsoft.Azure.Cosmos;
using WebAPI.Data.Dtos;

namespace WebAPI.Profiles;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<CreateImageDto, Models.Image>();
        CreateMap<Models.Image, ReadImageDto>();
        CreateMap<ItemResponse<Models.Image>, ReadImageDto>();
        CreateMap<IEnumerable<ItemResponse<Models.Image>>, IEnumerable<ReadImageDto>>()
            .ConvertUsing((source, destination, context) =>
            {
                var itemResponses = source.ToList();
                var ReadImageDtos = new List<ReadImageDto>();

                foreach (var item in itemResponses)
                {
                    var readImageDto = context.Mapper.Map<ReadImageDto>(item);
                    ReadImageDtos.Add(readImageDto);
                }

                return ReadImageDtos;
            });
    }
}