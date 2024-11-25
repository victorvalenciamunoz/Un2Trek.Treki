using ErrorOr;
using MediatR;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public class GetActiveActivitiesQuery : IRequest<ErrorOr<List<ActivityTreki>>>
{
}

public class GetActiveActivitiesHandler : IRequestHandler<GetActiveActivitiesQuery, ErrorOr<List<ActivityTreki>>>
{
    private readonly IActivitiesTrekiRepository _activitiesRepository;

    public GetActiveActivitiesHandler(IActivitiesTrekiRepository activitiesRepository)
    {
        _activitiesRepository = activitiesRepository;
    }

    public async Task<ErrorOr<List<ActivityTreki>>> Handle(GetActiveActivitiesQuery request, CancellationToken cancellationToken)
    {
        var activities = await _activitiesRepository.GetActiveActivitiesAsync();
        if (activities.Count == 0)
        {
            return Error.NotFound(description: "No activities found");
        }

        return activities;
    }
}