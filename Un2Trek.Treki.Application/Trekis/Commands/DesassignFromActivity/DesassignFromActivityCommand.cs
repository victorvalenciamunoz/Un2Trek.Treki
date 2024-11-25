using ErrorOr;
using MediatR;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public record DesassignFromActivityCommand(Guid TrekiId, Guid ActivityId) : IRequest<ErrorOr<Success>>;

public class DesassignFromActivityCommandHandler : IRequestHandler<DesassignFromActivityCommand, ErrorOr<Success>>
{
    private readonly IActivitiesTrekiRepository _activitiesTrekiRepository;

    public DesassignFromActivityCommandHandler(IActivitiesTrekiRepository activitiesTrekiRepository)
    {
        _activitiesTrekiRepository = activitiesTrekiRepository;
    }

    public async Task<ErrorOr<Success>> Handle(DesassignFromActivityCommand request, CancellationToken cancellationToken)
    {
        var trekiResult = await _activitiesTrekiRepository.GetTrekiByIdAsync(TrekiId.From(request.TrekiId));
        if (trekiResult.IsError)
        {
            return trekiResult.FirstError;
        }
        var activityTrekiResult = await _activitiesTrekiRepository.GetActivityTrekiWithTrekisAsync(ActivityId.From(request.ActivityId));
        if (activityTrekiResult.IsError)
        {
            return activityTrekiResult.FirstError;
        }

        var assignmentResult = activityTrekiResult.Value.DesassingTreki(trekiResult.Value);
        if (assignmentResult.IsError)
        {
            return assignmentResult.FirstError;
        }

        await _activitiesTrekiRepository.SaveChangesAsync();


        return Result.Success;
    }
}

