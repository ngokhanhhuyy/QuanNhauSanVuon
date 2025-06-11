using FluentValidation;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Localization;
using QuanNhauSanVuon.Extensions;
using QuanNhauSanVuon.Services.Enums;

namespace QuanNhauSanVuon.Validation.Validators;

public class MenuItemListValidator : Validator<MenuItemListFiltersDto>
{
    public MenuItemListValidator()
    {
        RuleFor(dto => dto.SortedByField)
            .NotEmpty()
            .Must(sortedByField =>
                SortedByFieldOptions.Contains(sortedByField) ||
                SortedByFieldOptions.Contains(sortedByField.LowerCaseFirstLetter()))
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.SortedByField);
        RuleFor(dto => dto.Page)
            .GreaterThanOrEqualTo(1)
            .WithName(DisplayNames.Page);
        RuleFor(dto => dto.ResultsPerPage)
            .GreaterThanOrEqualTo(5)
            .LessThanOrEqualTo(50)
            .WithName(DisplayNames.ResultsPerPage);
    }

    private static string[] SortedByFieldOptions => new[]
    {
        nameof(SortedByFieldOption.Name),
        nameof(SortedByFieldOption.DefaultNetPrice),
        nameof(SortedByFieldOption.DefaultVatPercentage),
        nameof(SortedByFieldOption.CreatedDateTime)
    };
}