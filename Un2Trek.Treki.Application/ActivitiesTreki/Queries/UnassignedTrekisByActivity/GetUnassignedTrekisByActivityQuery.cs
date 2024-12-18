using ErrorOr;
using MediatR;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public record GetUnassignedTrekisByActivityQuery(ActivityId ActivityId) : IRequest<ErrorOr<List<Treki>>>;

public class GetUnassignedTrekisByActivityQueryHandler : IRequestHandler<GetUnassignedTrekisByActivityQuery, ErrorOr<List<Treki>>>
{
    private readonly IActivitiesTrekiRepository _activitiesTrekiRepository;

    public GetUnassignedTrekisByActivityQueryHandler(IActivitiesTrekiRepository activitiesTrekiRepository)
    {
        _activitiesTrekiRepository = activitiesTrekiRepository;
    }

    public async Task<ErrorOr<List<Treki>>> Handle(GetUnassignedTrekisByActivityQuery request, CancellationToken cancellationToken)
    {
        var trekis = await _activitiesTrekiRepository.GetTrekisNotAssignedToActivityAsync(request.ActivityId, cancellationToken);
        return trekis;
    }
}