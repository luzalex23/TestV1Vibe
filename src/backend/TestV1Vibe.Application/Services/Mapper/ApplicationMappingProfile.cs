using AutoMapper;
using TestV1Vibe.Application.DTOs;
using TestV1Vibe.Domain.Entities;

namespace TestV1Vibe.Application.Services.Mapper;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        // Define mapeamento de Entity para DTO
        CreateMap<Placemark, PlacemarkDto>();

        // Define mapeamento de DTO para Entity (caso necessário)
        CreateMap<PlacemarkDto, Placemark>();
    }
}
