using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application.Abstractions.Users;

public interface IUsersRepository
{
    Task<ApplicationUser?> GetUserWithCapturesAsync(string userId, CancellationToken cancellationToken);
}
