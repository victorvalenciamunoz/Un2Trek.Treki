using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Un2Trek.Trekis.Application.Abstractions.Users;
using Un2Trek.Trekis.Domain;
using Un2Trek.Trekis.Domain.ValueObjects;

namespace Un2Trek.Trekis.Application;

public record CaptureTrekiCommand(Guid UserId, TrekiId TrekiId, ActivityId ActivityId, Location userLocation) : IRequest<ErrorOr<Success>>;

public class CaptureTrekiCommandHandler(ICaptureTrekisRepository captureTrekisRepository,
                                        ITrekisRepository trekisRepository,
                                        IActivitiesTrekiRepository activitiesTrekiRepository,
                                        IUsersRepository usersRepository,
                                        IConfiguration configuration) : IRequestHandler<CaptureTrekiCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(CaptureTrekiCommand request, CancellationToken cancellationToken)
    {
        var treki = await trekisRepository.GetByIdAsync(request.TrekiId, cancellationToken);
        if (treki == null)
        {
            return Error.NotFound(description: "Treki no encontrado");
        }

        var user = await usersRepository.GetUserWithCapturesAsync(request.UserId.ToString(), cancellationToken);

        if (user == null)
        {
            return Error.NotFound(description: "Usuario no encontrado");
        }

        var trekisInActivity = await activitiesTrekiRepository.GetTrekisByActivityIdAsync(request.ActivityId, cancellationToken);
        if (trekisInActivity == null || trekisInActivity.Count() == 0)
        {
            return Error.NotFound(description: "Actividad sin Trekis");
        }
        var isTrekiInActivity = trekisInActivity.Any(t => t.Id == request.TrekiId);
        if (!isTrekiInActivity)
        {
            return Error.Validation(description: "Treki no pertenece a la actividad");
        }

        var captureResult = user.CaptureTreki(request.TrekiId, request.ActivityId);
        if (captureResult.IsError)
        {
            return captureResult.FirstError;
        }

        var isInRangeResult = IsInRange(request.userLocation, treki);
        if (isInRangeResult.IsError)
        {
            return isInRangeResult.FirstError;
        }

        UserTrekiCapture capture = new UserTrekiCapture
        (
            trekiId: request.TrekiId,
            activityId: request.ActivityId,
            userId: request.UserId.ToString(),
            captureDate: DateTime.UtcNow
        );
        await captureTrekisRepository.CaptureTrekiAsync(capture);

        return Result.Success;
    }

    private ErrorOr<Success> IsInRange(Location userLocation, Treki treki)
    {
        Geolocation.Coordinate origin = new Geolocation.Coordinate(treki.Location.Latitude, treki.Location.Longitude);
        Geolocation.Coordinate destination = new Geolocation.Coordinate(userLocation.Latitude, userLocation.Longitude);
        double distance = Geolocation.GeoCalculator.GetDistance(origin, destination, decimalPlaces: 2, Geolocation.DistanceUnit.Meters);
        var threshold = Convert.ToDouble(configuration.GetValue<string>("Threshold")!);
        if (distance > threshold)
        {
            return Errors.InvalidDistance;
        }

        return Result.Success;
    }
}
