using Microsoft.AspNetCore.Authorization;

namespace Api;

public class IsAuthorizedRequirement : IAuthorizationRequirement { }

public class IsAuthorizedHandler : AuthorizationHandler<IsAuthorizedRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IsAuthorizedRequirement requirement)
    {
        context.Fail(new AuthorizationFailureReason(this, AuthorizationFailureMessages.UserNotFoundOrExpired));
        return Task.CompletedTask;
    }
}

public static class AuthorizationFailureMessages
{
    public static string UserNotFoundOrExpired => "User not found or expired";
}

public static class Policies
{
    public const string IsAuthorized = nameof(IsAuthorized);
}