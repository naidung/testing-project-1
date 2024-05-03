using WebAPI.Enums;

namespace WebAPI.Dtos;

public class APIResult
{
    public EAPIStatus Status { get; set; }
    public string? Msg { get; set; }
    public object? Data { get; set; }
}
