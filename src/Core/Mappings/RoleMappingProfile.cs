using AutoMapper;
using Core.Entities.DTOs;
using Core.Entities.Models;

namespace Core.Mappings;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<Role, RoleResponseDto>();  // Role -> RoleResponseDto
        CreateMap<RoleCreateDto, Role>();    // RoleCreateDto -> Role
        CreateMap<RoleUpdateDto, Role>();    // RoleUpdateDto -> Role
    }
}
