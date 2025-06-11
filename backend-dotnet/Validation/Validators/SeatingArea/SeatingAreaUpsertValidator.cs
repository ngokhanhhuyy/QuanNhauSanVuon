using FluentValidation;
using QuanNhauSanVuon.Dtos;
using QuanNhauSanVuon.Localization;
using QuanNhauSanVuon.Services.Structs;

namespace QuanNhauSanVuon.Validation.Validators;

public class SeatingAreaUpsertValidator : Validator<SeatingAreaUpsertDto>
{
    public SeatingAreaUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .MaximumLength(50)
            .WithName(DisplayNames.Name);
        RuleFor(dto => dto.Color)
            .MaximumLength(30)
            .WithName(DisplayNames.Color);
        RuleFor(dto => dto.TakenUpPositions)
            .NotEmpty()
            .Must(takenUpPositions =>
            {
                foreach (Point position in takenUpPositions)
                {
                    bool hasTop = takenUpPositions
                        .Any(p => p.X == position.X && p.Y == position.Y - 1);
                    bool hasBottom = takenUpPositions
                        .Any(p => p.X == position.X && p.Y == position.Y + 1);
                    bool hasLeft = takenUpPositions
                        .Any(p => p.X == position.X - 1 && p.Y == position.Y);
                    bool hasRight = takenUpPositions
                        .Any(p => p.X == position.X + 1 && p.Y == position.Y);

                    if (!(hasTop || hasBottom || hasLeft || hasRight))
                    {
                        return false;
                    }
                }

                return true;
            }).WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.TakenUpPositions);
        RuleForEach(dto => dto.Seatings)
            .Must((areaDto, seatingDto) =>
            {
                return areaDto.TakenUpPositions.Any(p =>
                    p.X == seatingDto.PositionX &&
                    p.Y == seatingDto.PositionY);
            }).WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.SeatingPosition);

    }
}