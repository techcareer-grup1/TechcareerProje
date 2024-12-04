namespace TechCareer.Models.Dtos.Users.Request;

public sealed record CreateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    byte[] PasswordSalt,
    byte[] PasswordHash,
    bool Status
);


