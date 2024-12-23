using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Un2Trek.Trekis.Application.Abstractions.Users;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Infrastructure.Persistance.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UsersRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> GetUserWithCapturesAsync(string userId, CancellationToken cancellationToken)
    {
        return await _userManager.Users
            .Include(u => u.UserTrekiCaptures)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }
}
