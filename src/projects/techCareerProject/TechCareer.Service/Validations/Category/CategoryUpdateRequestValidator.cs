using FluentValidation;
using TechCareer.Models.Dtos.Categories.Requests;

namespace TechCareer.Service.Validations.Categories;

public class CategoryUpdateRequestValidator : AbstractValidator<CategoryUpdateRequestDto>
{
    public CategoryUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Kategori ID'si boş bırakılamaz.")
            .GreaterThan(0).WithMessage("Geçerli bir ID giriniz.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Kategori adı boş bırakılamaz.")
            .Length(2, 100).WithMessage("Kategori adı 2 ile 100 karakter arasında olmalıdır.");
    }
}
