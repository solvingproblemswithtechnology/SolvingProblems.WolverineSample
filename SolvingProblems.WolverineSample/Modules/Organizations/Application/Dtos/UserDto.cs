using SolvingProblems.WolverineSample.Modules.Organizations.Domain;

namespace SolvingProblems.WolverineSample.Modules.Organizations.Application.Dtos;

public record UserDto(Guid? UserId, string FirstName, string LastName, string Email, Guid OrganizationId, string Role, string? PersonalPhoneNumber, string? Position, string? Department)
{
    public static UserDto FromUser(User user) => new UserDto(user.Id.AsGuid(), user.FirstName, user.LastName,
        user.Email.AsString(), user.OrganizationId.AsGuid(), user.Role, user.PersonalPhoneNumber?.AsString(),
        user.Position, user.Department);
}