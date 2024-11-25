using Azure.Core;
using ErrorOr;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace Un2Trek.Trekis.API.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    private const string HeaderKeyName = "Authorization";

    protected string TelegramIdFromToken()
    {
        Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
        if (!string.IsNullOrEmpty(headerValue))
        {
            var token = headerValue.ToString().Replace("Bearer", "").Trim();
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            var userIdClaim = securityToken.Claims.FirstOrDefault(c => c.Type == "unique_name");
            if (userIdClaim is not null)
            {
                return userIdClaim.Value;
            }
        }

        return string.Empty;
    }
    protected IActionResult ProblemDetail(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors[0]);
    }

    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode: statusCode, title: error.Description);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(
                error.Code,
                error.Description);
        }

        return ValidationProblem(modelStateDictionary);
    }
}
