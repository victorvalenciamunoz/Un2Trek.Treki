using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Un2Trek.Trekis.Application;
using Un2Trek.Trekis.Domain;
using Un2Trek.Trekis.Domain.ValueObjects;

namespace Un2Trek.Trekis.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class TrekisController : ApiController
{
    private readonly ISender _sender;    

    public TrekisController(ISender sender)
    {
        _sender = sender;        
    }

    [HttpPost]
    public async Task<IActionResult> CreateTreki([FromBody] CreateTrekiRequest createTrekiRequest, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid data",
                Status = StatusCodes.Status400BadRequest,
                Detail = "The provided data is invalid."
            });
        }

        if (!CaptureType.TryFromValue(
            createTrekiRequest.CaptureType,
            out var captureType))
        {
            return Problem("Invalid capture type", statusCode: StatusCodes.Status400BadRequest);
        }

        var command = new AddTrekiCommand(
                new Location(createTrekiRequest.Latitude, createTrekiRequest.Longitude),
                createTrekiRequest.Title,
                createTrekiRequest.Description,
                createTrekiRequest.IsActive,
                captureType
            );

        try
        {
            var trekiId = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetTrekiById), new { id = trekiId }, trekiId);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "An error occurred",
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message
            });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTreki([FromBody] UpdateTrekiRequest updateTrekiRequest, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid data",
                Status = StatusCodes.Status400BadRequest,
                Detail = "The provided data is invalid."
            });
        }

        if (!CaptureType.TryFromValue(
            updateTrekiRequest.CaptureType,
            out var captureType))
        {
            return Problem("Invalid capture type", statusCode: StatusCodes.Status400BadRequest);
        }

        var command = new UpdateTrekiCommand(
                TrekiId.From(updateTrekiRequest.Id),
                new Location(updateTrekiRequest.Latitude, updateTrekiRequest.Longitude),
                updateTrekiRequest.Title,
                updateTrekiRequest.Description,
                updateTrekiRequest.IsActive,
                captureType
            );

        try
        {
            var updateTrekiResult= await _sender.Send(command, cancellationToken);
            if (updateTrekiResult.IsError)
            {
                return ProblemDetail(updateTrekiResult.Errors);
            }

            return Ok(updateTrekiResult.Value);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "An error occurred",
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message
            });
        }
    }

    [HttpPost("capture")]
    public async Task<IActionResult> CaptureTreki([FromBody] CaptureTrekiRequest captureTrekiRequest, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid data",
                Status = StatusCodes.Status400BadRequest,
                Detail = "The provided data is invalid."
            });
        }

        var command = new CaptureTrekiCommand(new Guid(captureTrekiRequest.UserId), 
                                    TrekiId.From(new Guid(captureTrekiRequest.TrekiId)),
                                    ActivityId.From(new Guid(captureTrekiRequest.ActivityId)),
                                    new Location(captureTrekiRequest.Latitude, captureTrekiRequest.Longitude));
        var commandResult = await _sender.Send(command, cancellationToken);
        return commandResult.Match<IActionResult>(
     _ => Ok(),
     _ => ProblemDetail(commandResult.Errors));

    }


    [HttpGet("{id}")]
    public IActionResult GetTrekiById(Guid id)
    {
        // Implement this method to retrieve the treki by ID
        return Ok();
    }
}
