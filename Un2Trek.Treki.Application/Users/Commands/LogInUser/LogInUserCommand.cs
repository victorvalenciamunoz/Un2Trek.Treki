using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public record LogInUserCommand(string Email, string Password) : IRequest<ErrorOr<(string AccessToken, string RefreshToken)>>;
internal class LogInUserCommandHandler(IJwtService jwtService, UserManager<ApplicationUser> userManager) : IRequestHandler<LogInUserCommand, ErrorOr<(string AccessToken, string RefreshToken)>>
{
    public async Task<ErrorOr<(string AccessToken, string RefreshToken)>> Handle(LogInUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                if (await userManager.CheckPasswordAsync(existingUser, request.Password))
                {
                    var resultGenerateTokens = GenerateTokens(existingUser);
                    if (resultGenerateTokens.IsError)
                    {
                        return resultGenerateTokens.FirstError;
                    }

                    existingUser.RefreshToken = resultGenerateTokens.Value.RefreshToken;
                    existingUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);

                    await userManager.UpdateAsync(existingUser);

                    return resultGenerateTokens.Value;
                }
            }

            return Error.Unauthorized(description: "Invalid credentials");
        }
        catch (Exception)
        {
            return Error.Unexpected(description: "Unexpected error validating user");
        }
    }

    private ErrorOr<(string AccessToken, string RefreshToken)> GenerateTokens(ApplicationUser user)
    {
        var token = jwtService.GenerateToken(user);
        if (token.IsError)
        {
            return token.FirstError;
        }

        var refreshToken = jwtService.GenerateRefreshToken();
        return (token.Value, refreshToken);
    }
}
