using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public interface ICaptureTrekisRepository
{
    Task CaptureTrekiAsync(UserTrekiCapture capture);
}
