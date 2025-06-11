using FluentValidation;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Localization;

namespace QuanNhauSanVuon.Validation.Validators;

public class MonthYearValidator : Validator<MonthYearDto>
{
    public MonthYearValidator()
    {
        RuleFor(dto => dto.Month)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(12)
            .WithName(DisplayNames.Month);
    }
}
