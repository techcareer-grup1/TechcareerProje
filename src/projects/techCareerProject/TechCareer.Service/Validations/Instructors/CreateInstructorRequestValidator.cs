

using FluentValidation;
using TechCareer.Models.Dtos.Instructors.Request;

namespace TechCareer.Service.Validations.Instructors;

public class CreateInstructorRequestValidator : AbstractValidator<CreateInstructorRequest>
{
    public CreateInstructorRequestValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Eğitmen adı boş bırakılamaz.")
           .Length(2, 128).WithMessage("Eğitmen adı 3 ile 100 karakter arasında olmalıdır.");

        
        RuleFor(x => x.About)
            .Length(1,500).WithMessage("Hakkında bilgisi 1 ile 500 karakter arasında olmalıdır.");
    }
}
