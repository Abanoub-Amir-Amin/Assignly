using AutoMapper;

namespace Assignly.Core;

public class AppProfiler : Profile
{
    public AppProfiler()
    {
        CreateMap<Data.Models.Comment, DTOs.CommentDTOs.CommentResponse>().ReverseMap();
        CreateMap<DTOs.CommentDTOs.CommentRequest, Data.Models.Comment>().ReverseMap();
    }
}
