using Microsoft.AspNetCore.Http;
using WebAPI.Dtos;
using WebAPI.Enums;

namespace WebAPI.Helpers;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            var result = new APIResult
            {
                Status = EAPIStatus.ErrorHasMsg,
                Msg = exception.Message
            };

            context.Response.StatusCode = StatusCodes.Status200OK;

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}
