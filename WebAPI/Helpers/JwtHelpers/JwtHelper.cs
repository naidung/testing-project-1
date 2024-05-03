using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Enums;
using WebAPI.Models;

namespace WebAPI.Helpers.JwtHelpers;

public class JwtHelper : IJwtHelper
{
    private string secretKey = null!;
    private string issuer = null!;
    private string audience = null!;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ERoleName roleName;

    private string authorizationHeaderKey { get; set; } = "Authorization";

    public JwtHelper(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ERoleName roleName)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.roleName = roleName;
        this.secretKey = configuration["Jwt:Key"]!;
        this.issuer = configuration["Jwt:Issuer"]!;
        this.audience = configuration["Jwt:Audience"]!;
    }

    public string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>();
        claims.Add(new Claim("username", user.UserName!));
        claims.Add(new Claim("fullname", user.FullName!));
        claims.Add(new Claim("userid", user.Id.ToString()));
        foreach (var role in user.UserRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName.GetRoleName((ERole)role.RoleId!)));
        }

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddHours(6),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string? GetToken()
    {
        string? authorizationHeader = httpContextAccessor.HttpContext!.Request.Headers[authorizationHeaderKey];
        if (!string.IsNullOrEmpty(authorizationHeader))
        {
            if (authorizationHeader.StartsWith("Bearer ") && authorizationHeader.Length > 10)
            {
                return authorizationHeader.Replace("Bearer ", "");
            }
        }
        return null;
    }

    public string? GetClaimValue(string claimName)
    {
        try
        {
            string? token = GetToken();
            if (token is null) return null;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken? parsedToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            if (parsedToken == null) return null;

            var claims = parsedToken?.Claims;
            var claim = claims?.FirstOrDefault(x => x.Type == claimName);
            if (claim == null) return null;

            return claim.Value;
        }
        catch { }
        return null;
    }

}