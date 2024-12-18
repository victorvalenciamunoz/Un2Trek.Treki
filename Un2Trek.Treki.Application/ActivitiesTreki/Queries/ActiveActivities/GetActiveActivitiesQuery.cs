using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public class GetActiveActivitiesQuery : IRequest<ErrorOr<List<ActivityTreki>>>
{
}

public class GetActiveActivitiesHandler : IRequestHandler<GetActiveActivitiesQuery, ErrorOr<List<ActivityTreki>>>
{
    private readonly IActivitiesTrekiRepository _activitiesRepository;
    private readonly HybridCache _hybridCache;

    public GetActiveActivitiesHandler(IActivitiesTrekiRepository activitiesRepository, HybridCache hybridCache)
    {
        _activitiesRepository = activitiesRepository;
        _hybridCache = hybridCache;
    }

    public async Task<ErrorOr<List<ActivityTreki>>> Handle(GetActiveActivitiesQuery request, CancellationToken cancellationToken)
    {
        var cachedActivities = await _hybridCache.GetOrCreateAsync("ActiveActivities",
            async token =>
            {
                var activities = await _activitiesRepository.GetActiveActivitiesAsync();
                return activities;
            },
            tags: new[] { CacheTags.ActiveActivities },
            cancellationToken: cancellationToken);

        if (cachedActivities.Count == 0)
        {
            return Error.NotFound(description: "No activities found");
        }

        return cachedActivities;
    }
}