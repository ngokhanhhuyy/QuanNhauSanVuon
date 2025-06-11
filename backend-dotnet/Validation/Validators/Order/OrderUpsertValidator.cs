using FluentValidation;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Localization;

namespace QuanNhauSanVuon.Validation.Validators;

public class OrderUpsertValidator : Validator<OrderUpsertDto>
{
    public OrderUpsertValidator()
    {
        RuleFor(dto => dto.SeatingId)
            .NotEmpty()
            .WithName(DisplayNames.Seating);
        RuleFor(dto => dto.Items)
            .NotEmpty()
            .WithName(DisplayNames.OrderItemList);
        RuleForEach(dto => dto.Items)
            .SetValidator(new OrderItemUpsertValidator());
    }
}
