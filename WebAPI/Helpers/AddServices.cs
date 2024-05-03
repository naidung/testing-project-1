using WebAPI.Enums;
using WebAPI.Helpers.JwtHelpers;

namespace WebAPI.Helpers;

public static class AddServices
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<ERoleName>();
        services.AddScoped<BCryptHelper>();
        services.AddTransient<IJwtHelper, JwtHelper>();
    }
}
