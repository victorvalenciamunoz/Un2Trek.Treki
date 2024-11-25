using MediatR;
using Un2Trek.Trekis.Domain;
using Un2Trek.Trekis.Domain.ValueObjects;

namespace Un2Trek.Trekis.Application;

public record AddTrekiCommand(Location Location, string Title, string Description, bool IsActive, CaptureType CaptureType) : IRequest<TrekiId>;

public class AddTrekiCommandHandler : IRequestHandler<AddTrekiCommand, TrekiId>
{
    private readonly ITrekisRepository _trekiRepository;

    public AddTrekiCommandHandler(ITrekisRepository trekiRepository)
    {
        _trekiRepository = trekiRepository;
    }

    public async Task<TrekiId> Handle(AddTrekiCommand request, CancellationToken cancellationToken)
    {
        var treki = new Treki(
            request.Location,
            request.Title,
            request.Description,
            request.IsActive,
            request.CaptureType
        );

        await _trekiRepository.AddTrekiAsync(treki);

        return treki.Id;
    }
}
