using FluentValidation;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Localization;

namespace QuanNhauSanVuon.Validation.Validators;

public class MenuCategoryUpsertValidator : Validator<MenuCategoryUpsertDto>
{
    public MenuCategoryUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .WithName(DisplayNames.MenuCategoryName);
    }
}
