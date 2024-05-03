namespace WebAPI.Dtos;

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
}
