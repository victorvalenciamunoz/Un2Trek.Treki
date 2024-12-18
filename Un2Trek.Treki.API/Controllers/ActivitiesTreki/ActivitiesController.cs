using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Un2Trek.Trekis.Application;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ActivitiesController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<ActivitiesController> _logger;

    public ActivitiesController(ISender sender, ILogger<ActivitiesController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    [HttpPost]  
    public async Task<IActionResult> CreateActivity([FromBody] CreateActivityRequest createActivityRequest)
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

        var command = new AddActivityCommand(
            createActivityRequest.Title,
            createActivityRequest.Description,
            createActivityRequest.ValidFromDate,
            createActivityRequest.ValidToDate
        );

        try
        {
            var activityId = await _sender.Send(command);
            return CreatedAtAction(nameof(GetActivityById), new { id = activityId }, activityId);
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

    [HttpPost("{activityId}/trekis/{trekiId}")]
    public async Task<IActionResult> AssignTrekiToActivity(Guid activityId, Guid trekiId)
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

        var command = new AssignTrekiToActivityCommand(trekiId, activityId);

        try
        {
            await _sender.Send(command);
            return NoContent();
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

    [HttpDelete("{activityId}/trekis/{trekiId}")]
    public async Task<IActionResult> DesassignTrekiFromActivity(Guid activityId, Guid trekiId)
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

        var command = new DesassignFromActivityCommand(trekiId, activityId);

        try
        {
            await _sender.Send(command);
            return NoContent();
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

    [HttpGet("{activityId}/trekis")]    
    public async Task<IActionResult> GetTrekisByActivityId(Guid activityId)
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

        var command = new GetTrekisByActivityIdQuery(ActivityId.From(activityId));
        var result = await _sender.Send(command);

        return result.Match(
        trekis => Ok(trekis.Select(t => new TrekiResponse
        {
            ActivityId = activityId,
            TrekiId = t.Id.Value,
            Latitude = t.Location.Latitude,
            Longitude = t.Location.Longitude,
            Title = t.Title,
            Description = t.Description,
            CaptureType = t.CaptureType.Value,
            IsActive = t.IsActive
        })),
        error => StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
        {
            Title = "An error occurred",
            Status = StatusCodes.Status500InternalServerError,
            Detail = error.First().Description
        })
    );
    }

    [HttpGet("{activityId}/unassigned-trekis")]    
    public async Task<IActionResult> GetTrekisNotAssignedToActivity(Guid activityId)
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

        var command = new GetUnassignedTrekisByActivityQuery(ActivityId.From(activityId));
        var result = await _sender.Send(command);

        return result.Match(
        trekis => Ok(trekis.Select(t => new TrekiResponse
        {
            ActivityId = activityId,
            TrekiId = t.Id.Value,
            Latitude = t.Location.Latitude,
            Longitude = t.Location.Longitude,
            Title = t.Title,
            Description = t.Description,
            CaptureType = t.CaptureType.Value,
            IsActive = t.IsActive
        })),
        error => StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
        {
            Title = "An error occurred",
            Status = StatusCodes.Status500InternalServerError,
            Detail = error.First().Description
        })
    );
    }

    [HttpGet]    
    public async Task<IActionResult> GetActiveActivities()
    {
        try
        {
            var query = new GetActiveActivitiesQuery();

            var result = await _sender.Send(query);

            return result.Match(
                activities => Ok(activities.Select(a => new ActivityResponse
                {
                    Description = a.Description,
                    Title = a.Title,
                    Id = a.Id.Value,
                    ValidFromDate = a.ValidFromDate,
                    ValidToDate = a.ValidToDate
                })),
                error => StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "An error occurred",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = error.First().Description
                }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting active activities.");
        }

        return BadRequest();

    }

    [HttpGet("{id}")]    
    public async Task<IActionResult> GetActivityById(Guid id)
    {
        try
        {
            var query = new GetActivityByIdQuery(ActivityId.From(id));

            var result = await _sender.Send(query);

            return result.Match(
                activity => Ok(new ActivityResponse
                {
                    Description = activity.Description,
                    Title = activity.Title,
                    Id = activity.Id.Value,
                    ValidFromDate = activity.ValidFromDate,
                    ValidToDate = activity.ValidToDate
                }),
                error => StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "An error occurred",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = error.First().Description
                }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting activity.");
        }

        return BadRequest();

    }
}

