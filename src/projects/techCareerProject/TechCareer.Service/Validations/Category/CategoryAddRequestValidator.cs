using FluentValidation;
using TechCareer.Models.Dtos.Categories.Requests;

namespace TechCareer.Service.Validations.Categories;

public class CategoryAddRequestValidator : AbstractValidator<CategoryAddRequestDto>
{
    public CategoryAddRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Kategori adı boş bırakılamaz.")
            .Length(2, 100).WithMessage("Kategori adı 2 ile 100 karakter arasında olmalıdır.");
    }
}
