using ErrorOr;
using MediatR;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public record AssignTrekiToActivityCommand(Guid TrekiId, Guid ActivityId) : IRequest<ErrorOr<Success>>;

public class AssignTrekiToActivityCommandHandler : IRequestHandler<AssignTrekiToActivityCommand, ErrorOr<Success>>
{
    private readonly IActivitiesTrekiRepository _activitiesTrekiRepository;

    public AssignTrekiToActivityCommandHandler(IActivitiesTrekiRepository activitiesTrekiRepository)
    {
        _activitiesTrekiRepository = activitiesTrekiRepository;
    }

    public async Task<ErrorOr<Success>> Handle(AssignTrekiToActivityCommand request, CancellationToken cancellationToken)
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

        var assignmentResult = activityTrekiResult.Value.AssignTreki(trekiResult.Value);
        if (assignmentResult.IsError)
        {
            return assignmentResult.FirstError;
        }

        await _activitiesTrekiRepository.SaveChangesAsync();


        return Result.Success;
    }
}