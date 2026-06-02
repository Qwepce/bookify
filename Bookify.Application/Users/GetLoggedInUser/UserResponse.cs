namespace Bookify.Application.Users.GetLoggedInUser;

public sealed class UserResponse
{
    public Guid Id { get; init; }

    public string Email { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

}