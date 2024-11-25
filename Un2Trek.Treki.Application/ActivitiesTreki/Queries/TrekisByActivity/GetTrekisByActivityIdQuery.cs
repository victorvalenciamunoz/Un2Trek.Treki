using ErrorOr;
using MediatR;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public record GetTrekisByActivityIdQuery(ActivityId ActivityId) : IRequest<ErrorOr<List<Treki>>>;

public class GetTrekisByActivityIdQueryHandler : IRequestHandler<GetTrekisByActivityIdQuery, ErrorOr<List<Treki>>>
{
    private readonly IActivitiesTrekiRepository _activitiesTrekiRepository;

    public GetTrekisByActivityIdQueryHandler(IActivitiesTrekiRepository activitiesTrekiRepository)
    {
        _activitiesTrekiRepository = activitiesTrekiRepository;
    }

    public async Task<ErrorOr<List<Treki>>> Handle(GetTrekisByActivityIdQuery request, CancellationToken cancellationToken)
    {
        var trekis = await _activitiesTrekiRepository.GetTrekisByActivityIdAsync(request.ActivityId);
        return trekis;
    }
}