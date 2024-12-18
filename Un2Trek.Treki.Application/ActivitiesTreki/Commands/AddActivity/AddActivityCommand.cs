using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public record AddActivityCommand(string Title, string Description, DateTime ValidFromDate, DateTime? ValidToDate) : IRequest<ActivityId>;

public class AddActivityCommandHandler : IRequestHandler<AddActivityCommand, ActivityId>
{
    private readonly IActivitiesTrekiRepository _activitiesTrekiRepository;
    private readonly HybridCache _hybridCache;

    public AddActivityCommandHandler(IActivitiesTrekiRepository activitiesTrekiRepository, HybridCache hybridCache)
    {
        _activitiesTrekiRepository = activitiesTrekiRepository;
        _hybridCache = hybridCache;
    }

    public async Task<ActivityId> Handle(AddActivityCommand request, CancellationToken cancellationToken)
    {
        var activityTreki = new ActivityTreki
        {
            Id = ActivityId.From(Guid.NewGuid()),
            Title = request.Title,
            Description = request.Description,
            ValidFromDate = request.ValidFromDate,
            ValidToDate = request.ValidToDate
        };

        await _activitiesTrekiRepository.AddActivityTrekiAsync(activityTreki, cancellationToken);

        await _hybridCache.RemoveByTagAsync(CacheTags.ActiveActivities, cancellationToken);  

        return activityTreki.Id;
    }
}
