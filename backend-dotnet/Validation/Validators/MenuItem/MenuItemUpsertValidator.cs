using FluentValidation;
using QuanNhauSanVuon.Localization;

using QuanNhauSanVuon.Dtos;

namespace QuanNhauSanVuon.Validation.Validators;

public class MenuItemUpsertValidator : Validator<MenuItemUpsertDto>
{
    public MenuItemUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(50)
            .WithName(DisplayNames.Name);
        RuleFor(dto => dto.DefaultNetPrice)
            .GreaterThanOrEqualTo(1000)
            .WithName(DisplayNames.DefaultAmount);
        RuleFor(dto => dto.DefaultVatPercentage)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100)
            .WithName(DisplayNames.DefaultVatPercentage);
        RuleFor(dto => dto.Unit)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(50)
            .WithName(DisplayNames.Unit);
        RuleFor(dto => dto.ThumbnailUrl)
            .MaximumLength(255)
            .WithName(DisplayNames.ThumbnailUrl);
    }
}