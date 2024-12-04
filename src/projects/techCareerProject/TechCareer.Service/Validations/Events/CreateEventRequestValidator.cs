using FluentValidation;
using TechCareer.Models.Dtos.Events.Request;

namespace TechCareer.Service.Validations.Events;

public class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>
{
    public CreateEventRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Etkinlik başlığı boş bırakılamaz.")
            .Length(5, 100).WithMessage("Etkinlik başlığı 5 ile 100 karakter arasında olmalıdır.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Etkinlik açıklaması boş bırakılamaz.")
            .Length(10, 1000).WithMessage("Etkinlik açıklaması 10 ile 1000 karakter arasında olmalıdır.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("Resim URL'si boş bırakılamaz.")
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("Geçerli bir URL giriniz.");

        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.Now).WithMessage("Etkinlik başlangıç tarihi bugünden sonra olmalıdır.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("Etkinlik bitiş tarihi, başlangıç tarihinden sonra olmalıdır.");

        RuleFor(x => x.ApplicationDeadline)
            .LessThanOrEqualTo(x => x.StartDate).WithMessage("Başvuru son tarihi, etkinlik başlangıç tarihinden önce veya aynı gün olmalıdır.")
            .GreaterThan(DateTime.Now).WithMessage("Başvuru son tarihi bugünden sonra olmalıdır.");

        RuleFor(x => x.ParticipationText)
            .NotEmpty().WithMessage("Katılım metni boş bırakılamaz.")
            .Length(5, 500).WithMessage("Katılım metni 5 ile 500 karakter arasında olmalıdır.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Kategori ID'si pozitif bir sayı olmalıdır.");
    }
}
