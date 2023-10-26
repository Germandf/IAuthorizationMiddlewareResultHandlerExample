using Api;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if (!authorizeResult.Succeeded)
        {
            context.Response.ContentType = "application/json";

            if (authorizeResult.AuthorizationFailure?.FailureReasons.Any(x => x.Message == AuthorizationFailureMessages.UserNotFoundOrExpired) ?? false)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                    title = "Unauthorized",
                    status = 401,
                    traceId = context.TraceIdentifier
                }));
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    type = "https://tools.ietf.org/html/rfc7235#section-2.1",
                    title = "Forbidden",
                    status = 403,
                    traceId = context.TraceIdentifier
                }));
            }
        }
        else
        {
            await next(context);
        }
    }
}
