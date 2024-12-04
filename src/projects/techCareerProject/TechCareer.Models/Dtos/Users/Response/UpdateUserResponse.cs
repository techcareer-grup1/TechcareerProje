namespace TechCareer.Models.Dtos.Users.Response;

public sealed record UpdatedUserResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    bool Status
);
