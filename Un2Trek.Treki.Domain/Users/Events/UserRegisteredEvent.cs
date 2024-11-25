using MediatR;

namespace Un2Trek.Trekis.Domain;

public class UserRegisteredEvent : INotification
{
    public string Email { get; }

    public string Name { get; }

    public string ConfirmationLink { get; }

    public UserRegisteredEvent(string email, string name, string confirmationLink)
    {
        Email = email;
        Name = name;
        ConfirmationLink = confirmationLink;
    }
}
