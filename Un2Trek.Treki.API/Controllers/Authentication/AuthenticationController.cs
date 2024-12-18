using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Un2Trek.Trekis.Application;

namespace Un2Trek.Trekis.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthenticationController : ApiController
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            LogInUserCommand logInUserCommand = new LogInUserCommand(loginRequest.Email, loginRequest.Password);
            var loginResult = await _mediator.Send(logInUserCommand);

            return loginResult.Match<IActionResult>(
                _ => Ok(new LogInUserResponse(loginResult.Value.AccessToken, loginResult.Value.RefreshToken)),
                _ => ProblemDetail(loginResult.Errors));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            RefreshTokenCommand refreshTokenCommand = new RefreshTokenCommand(refreshTokenRequest.ExpiredAccessToken, refreshTokenRequest.RefreshToken);
            var refreshTokenResult = await _mediator.Send(refreshTokenCommand);

            return refreshTokenResult.Match<IActionResult>(
                    _ => Ok(new LogInUserResponse(refreshTokenResult.Value.AccessToken, refreshTokenResult.Value.RefreshToken)),
                    _ => ProblemDetail(refreshTokenResult.Errors));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            RegisterUserCommand registerUserCommand = new RegisterUserCommand(registerRequest.Email, registerRequest.Name, registerRequest.LastName, registerRequest.Password, registerRequest.ReceivePromotionalEmails);
            var registerResult = await _mediator.Send(registerUserCommand);

            return registerResult.Match<IActionResult>(
                _ => Ok("User registered successfully"),
                _ => ProblemDetail(registerResult.Errors));
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            ValidateEmailCommand validateEmailCommand = new ValidateEmailCommand(token, email);
            var validateEmailResult = await _mediator.Send(validateEmailCommand);

            return validateEmailResult.Match<IActionResult>(
                _ => Ok("Email confirmed successfully"),
                _ => ProblemDetail(validateEmailResult.Errors));
        }
    }
}
