using WebAPI.Models;

namespace WebAPI.Helpers.JwtHelpers;

public interface IJwtHelper
{
    string GenerateToken(User user);

    string? GetClaimValue(string claimName);
}
