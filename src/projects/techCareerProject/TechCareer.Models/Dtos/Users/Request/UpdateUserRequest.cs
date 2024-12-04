namespace TechCareer.Models.Dtos.Users.Request;

public sealed record UpdateUserRequest(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    bool Status
);

