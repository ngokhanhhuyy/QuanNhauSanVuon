using FluentValidation;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Localization;

namespace QuanNhauSanVuon.Validation.Validators;

public class OrderItemUpsertValidator : Validator<OrderItemUpsertDto>
{
    public OrderItemUpsertValidator()
    {
        RuleFor(dto => dto.Quantity)
            .GreaterThanOrEqualTo(1)
            .WithName(DisplayNames.Quantity);
        RuleFor(dto => dto.NetPricePerUnit)
            .GreaterThanOrEqualTo(1000)
            .WithName(DisplayNames.NetPricePerUnit);
        RuleFor(dto => dto.VatAmountPerUnit)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.VatAmountPerUnit);
        RuleFor(dto => dto.MenuItemId)
            .NotEmpty()
            .WithName(DisplayNames.MenuItem);
    }
}
