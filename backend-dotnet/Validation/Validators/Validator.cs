using FluentValidation;
using QuanNhauSanVuon.Dtos.Interfaces;

namespace QuanNhauSanVuon.Validation.Validators;

public partial class Validator<TRequestDto> : AbstractValidator<TRequestDto>
        where TRequestDto : IRequestDto
{
    public Validator()
    {
        ClassLevelCascadeMode = CascadeMode.Continue;
        RuleLevelCascadeMode = CascadeMode.Stop;
    }

    protected virtual bool EqualOrEarlierThanToday(DateTime value)
    {
        return value <= DateTime.UtcNow.Date;
    }

    protected virtual bool EqualOrEarlierThanToday(DateTime? value)
    {
        if (value.HasValue)
        {
            return EqualOrEarlierThanToday(value.Value);
        }
        return true;
    }

    protected virtual bool EqualOrEarlierThanToday(DateOnly value)
    {
        return value.ToDateTime(new TimeOnly(0, 0)) <= DateTime.UtcNow.Date;
    }

    protected virtual bool EqualOrEarlierThanToday(DateOnly? value)
    {
        if (value.HasValue)
        {
            return EqualOrEarlierThanToday(value.Value);
        }
        return true;
    }

    protected virtual bool IsEnumElementName<TEnum>(string name) where TEnum : Enum
    {
        return name != null && Enum.GetNames(typeof(TEnum)).ToList().Contains(name);
    }

    protected virtual bool IsOneOfEnumElementNames<TEnum>(
        string name,
        IEnumerable<TEnum> enumElements) where TEnum : Enum
    {
        IEnumerable<string> elementNames = enumElements.Select(element => element.ToString());
        return name != null && elementNames.Contains(name);
    }
}