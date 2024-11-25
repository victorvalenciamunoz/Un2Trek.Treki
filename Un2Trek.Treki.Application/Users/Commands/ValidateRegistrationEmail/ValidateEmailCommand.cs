using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public record ValidateEmailCommand(string Token, string Email) : IRequest<ErrorOr<Success>>;

internal class ValidateEmailCommandHandler : IRequestHandler<ValidateEmailCommand, ErrorOr<Success>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ValidateEmailCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ErrorOr<Success>> Handle(ValidateEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        var token = request.Token.Replace(" ", "+");
        if (user == null)
        {
            return Error.NotFound(description: "User not found.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return Result.Success;
        }

        return Error.Unexpected(description: "Failed to confirm email.");
    }
}
