using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public record RegisterUserCommand(string Email, string Name, string LastName, string Password, bool ReceivePromotionalEmails) : IRequest<ErrorOr<Success>>;


internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ErrorOr<Success>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IMediator _mediator;
    private readonly AppSettings _appSettings;

    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IJwtService jwtService, IMediator mediator, IOptions<AppSettings> appSettings)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _mediator = mediator;
        _appSettings = appSettings.Value;
    }

    public async Task<ErrorOr<Success>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = false,
            ReceivePromotionalEmails = request.ReceivePromotionalEmails
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
            {
                return Error.Unexpected(description: "Failed to assign role to user.");
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"{_appSettings.RegisteredUserMailVerificationUrl}?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

            var domainEvent = new UserRegisteredEvent(user.Email, $"{request.Name} {request.LastName}", confirmationLink);

            await _mediator.Publish(domainEvent);

            return Result.Success;
        }

        return Error.Unexpected(description: "Failed to create user.");
    }
}