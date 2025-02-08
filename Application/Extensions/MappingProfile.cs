using Application.DTOs.Request.Entities;
using AutoMapper;

namespace Application.Extensions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ThreadDto, Thread>();
        CreateMap<Thread, ThreadDto>();
    }
}