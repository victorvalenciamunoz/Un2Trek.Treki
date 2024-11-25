using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public record RefreshTokenCommand(string ExpiredAccessToken, string RefreshToken) : IRequest<ErrorOr<(string AccessToken, string RefreshToken)>>;

internal class RefreshTokenCommandHandler(IJwtService jwtService, UserManager<ApplicationUser> userManager) : IRequestHandler<RefreshTokenCommand, ErrorOr<(string AccessToken, string RefreshToken)>>
{
    public async Task<ErrorOr<(string AccessToken, string RefreshToken)>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = jwtService.GetPrincipalFromExpiredToken(request.ExpiredAccessToken);
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userManager.FindByIdAsync(userId);

        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Error.Unauthorized(description: "Invalid refresh token");
        }

        var resultGenerateTokens = GenerateTokens(user);
        if (resultGenerateTokens.IsError)
        {
            return resultGenerateTokens.FirstError;
        }

        user.RefreshToken = resultGenerateTokens.Value.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await userManager.UpdateAsync(user);

        return resultGenerateTokens.Value;
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