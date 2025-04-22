using AutoMapper;
using Core.Entities.DTOs;
using Core.Entities.Models;

namespace Core.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : "Unknown"));

        CreateMap<UserCreateDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Hashing should be handled separately
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<UserUpdateDto, User>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
