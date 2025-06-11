using FluentValidation;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Localization;

namespace QuanNhauSanVuon.Validation.Validators;

public class SignInValidator : Validator<SignInDto>
{
    public SignInValidator()
    {
        RuleFor(dto => dto.UserName)
            .NotEmpty()
            .WithName(dto => DisplayNames.Get(nameof(dto.UserName)));
        RuleFor(dto => dto.Password)
            .NotEmpty()
            .WithName(dto => DisplayNames.Get(nameof(dto.Password)));
    }
}