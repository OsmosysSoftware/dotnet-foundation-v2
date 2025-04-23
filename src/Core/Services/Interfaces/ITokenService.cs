using Core.Entities.Models;

namespace Core.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
