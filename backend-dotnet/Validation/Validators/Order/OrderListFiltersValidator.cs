using FluentValidation;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Localization;
using QuanNhauSanVuon.Services.Enums;

namespace QuanNhauSanVuon.Validation.Validators;

public class OrderListFiltersValidator : Validator<OrderListFiltersDto>
{
    public OrderListFiltersValidator()
    {
        RuleFor(dto => dto.SortedByField)
            .NotEmpty()
            .Must(sortedByField => IsOneOfEnumElementNames(
                sortedByField,
                new SortedByFieldOption[]
                {
                    SortedByFieldOption.CreatedDateTime,
                    SortedByFieldOption.PaidDateTime
                })
            ).WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.SortedByField);
        RuleFor(dto => dto.Page)
            .NotEmpty()
            .GreaterThanOrEqualTo(1)
            .WithName(DisplayNames.Page);
        RuleFor(dto => dto.ResultsPerPage)
            .NotEmpty()
            .GreaterThanOrEqualTo(5)
            .LessThanOrEqualTo(50)
            .WithName(DisplayNames.ResultsPerPage);
        RuleFor(dto => dto.CreatedMonthYear)
            .SetValidator(new MonthYearValidator());
        RuleFor(dto => dto.PaidMonthYear)
            .SetValidator(new MonthYearValidator());
    }
}
