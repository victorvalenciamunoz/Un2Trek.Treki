using ErrorOr;
using MediatR;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public record GetActivityByIdQuery(ActivityId activityId) : IRequest<ErrorOr<ActivityTreki>>;

public class GetActivityByIdQueryHandler : IRequestHandler<GetActivityByIdQuery, ErrorOr<ActivityTreki>>
{
    private readonly IActivitiesTrekiRepository _activitiesRepository;

    public GetActivityByIdQueryHandler(IActivitiesTrekiRepository activitiesRepository)
    {
        _activitiesRepository = activitiesRepository;
    }

    public async Task<ErrorOr<ActivityTreki>> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
    {
        var activity = await _activitiesRepository.GetByIdAsync(request.activityId, cancellationToken);
        if (activity is null)
        {
            return Error.NotFound(description: "ActivityTreki not found");
        }

        return activity;
    }
}