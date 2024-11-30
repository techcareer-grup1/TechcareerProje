using FluentValidation;
using TechCareer.Models.Dtos.VideoEducations.Request;

namespace TechCareer.Service.Validations.VideoEducations;

public class UpdateVideoEduRequestValidator : AbstractValidator<UpdateVideoEduRequestDto>
{
    public UpdateVideoEduRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.TotalHours)
            .GreaterThan(0);

        RuleFor(x => x.IsCertified)
            .NotNull();

        RuleFor(x => x.Level)
            .IsInEnum();

        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .MaximumLength(500)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _));

        RuleFor(x => x.InstructorId)
            .NotEmpty();

        RuleFor(x => x.ProgrammingLanguage)
            .NotEmpty()
            .MaximumLength(100);
    }
}