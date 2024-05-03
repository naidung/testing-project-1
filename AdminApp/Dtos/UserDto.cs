using AdminApp.Enums;
using AdminApp.Extensions;

namespace AdminApp.Dtos;

public class UserDto
{
    public long Id { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public DateTime? JoinDate { get; set; }
    public List<int> Roles { get; set; } = new();
    public string? AccessToken { get; set; }
    public string? Jwt { get; set; }

    public string? JoinDateString => JoinDate.NulltableDateTime2Str();
    public int Stt { get; set; }
    public string RolesString
    {
        get
        {
            string roles = "";
            if (Roles.Any(e => e == (int)ERole.Admin)) roles += "Admin";
            if (Roles.Any(e => e == (int)ERole.Updater))
            {
                if (roles == "") roles = "Updater";
                else roles += ", Updater";
            }
            return roles;
        }
    }
}
