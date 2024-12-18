using ErrorOr;
using MediatR;
using Un2Trek.Trekis.Domain;
using Un2Trek.Trekis.Domain.ValueObjects;

namespace Un2Trek.Trekis.Application;

public record UpdateTrekiCommand(TrekiId Id, Location Location, string Title, string Description, bool IsActive, CaptureType CaptureType) : IRequest<ErrorOr<Treki>>;

public class UpdateTrekiCommandHandler : IRequestHandler<UpdateTrekiCommand, ErrorOr<Treki>>
{
    private readonly ITrekisRepository _trekiRepository;

    public UpdateTrekiCommandHandler(ITrekisRepository trekiRepository)
    {
        _trekiRepository = trekiRepository;
    }

    public async Task<ErrorOr<Treki>> Handle(UpdateTrekiCommand request, CancellationToken cancellationToken)
    {
        var treki = await _trekiRepository.GetByIdAsync(request.Id, cancellationToken);
        if (treki == null)
        {
            return Error.NotFound("Treki not found");
        }

        treki.SetProperties(request.Id, request.Location, request.Title, request.Description, request.IsActive, request.CaptureType);

        await _trekiRepository.UpdateTrekiAsync(treki, cancellationToken);

        return treki;
    }
}
